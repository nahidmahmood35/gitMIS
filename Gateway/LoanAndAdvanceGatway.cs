using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Gateway
{
    public class LoanAndAdvanceGatway : DbConnection
    {
        private SqlTransaction _trans;
        public List<IdNameForDropdownModel> GetIdCasCadeDropDown(string query)
        {
            var lists = new List<IdNameForDropdownModel>();
            try
            {
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new IdNameForDropdownModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["Name"].ToString(),
                        // CatId = Convert.ToInt32(rdr["CatId"])
                    });
                }
                rdr.Close();
                Con.Close();
                return lists;
            }
            catch (Exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                throw;
            }
        }


        public List<LoanAndAdvanceModel> GetEmployeeDetailById(string searchString)
        {
            try
            {
                string condition = "";
                var lists = new List<LoanAndAdvanceModel>();
                string query = "";
                //if (searchString != "0") { condition = "WHERE (Name) LIKE '%' + '" + searchString + "' + '%' "; }

//                query = @"SELECT Id,CompanyId,Name,PackSize,ProductUnit,Tp,SalePrice,ReminderStock,PDGroupId,UserName,EntryDate,BranchId,EntryTime
//							From tbl_PHAR_PRODUCT WHERE Id = " + searchString + " ";
//                query = @"SELECT e.Name AS employeeName , d.Name As DESIGNATIONname , a.Name As DEPARTMENTname , e.GrossSalary AS GrossSalary, e.ProjectId AS ProjectId
//                           From tbl_EMPLOYEE_HR As  e inner join tbl_DESIGNATION_HR As d 
//                           ON e.DesignationId = d.Id inner join tbl_DEPARTMENT_HR As a 
//                           ON e.DeparmentId = a.Id WHERE e.Id = " + searchString + "";

                query = @"Select a.Id,a.Name AS EmployeeName,c.BName AS DepartmentName,d.BName AS DesigationName,
                            (select SUM(Amount) from tbl_HR_GLO_EMPLOYEE_DLS where SalaryType='e' AND EmpId=a.id)-(
                            select SUM(Amount) from tbl_HR_GLO_EMPLOYEE_DLS where SalaryType='d' AND EmpId=a.id) AS TotalSalary

                            from tbl_HR_GLO_EMPLOYEE AS a inner join tbl_HR_GLO_EMPLOYEE_DLS AS b ON a.Id=b.EmpId
                            inner join tbl_DEPARTMENT_HR AS c ON a.DeparmentId = c.Id
                            inner Join tbl_DESIGNATION_HR AS d ON a.DesignationId=d.Id 
                            where a.id=" + searchString + " Group by a.Id,a.Name,c.BName,d.BName";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    lists.Add(new LoanAndAdvanceModel()
                    {
                        Name = rdr["EmployeeName"].ToString(),
                        DepartmentName = rdr["DepartmentName"].ToString(),
                        DesignationName = rdr["DesigationName"].ToString(),
                        GrossSalary = Convert.ToDouble(rdr["TotalSalary"]),
                       

                    });
                }
                rdr.Close();
                Con.Close();
                return lists;
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return new List<LoanAndAdvanceModel>();
            }
        }

        public Task<string> Save(LoanAndAdvanceModel aModel)
        {
            try
            {


                string query = @"INSERT INTO tbl_HR_LOAN_LEDGER (EmpId,TrNo,TrDate,MonthId,AttYear,LoanTypeId,LoanNo,LoanAmt,InstallmentSize,PaidAmount,ExcemptAmount,LoanAndAdvance,
                                 LoanStatus,UserName,EntryDate,EntryTime) VALUES (@EmpId,@TrNo,@TrDate,@MonthId,@AttYear,@LoanTypeId,@LoanNo,@LoanAmt,@InstallmentSize,0,0,
                                       @LoanAndAdvance,'take','NotDevelopment',@EntryDate,'') ";
                

                Con.Open();

                _trans = Con.BeginTransaction();
                string trNo = GetTrNo("TrNo", "tbl_HR_LOAN_LEDGER", _trans);
                DateTime today = DateTime.Today;

                var cmd = new SqlCommand(query, Con, _trans);
                
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@EmpId", aModel.EmployeeId);
                cmd.Parameters.AddWithValue("@TrNo", trNo);
                cmd.Parameters.AddWithValue("@TrDate", today.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MonthId", today.ToString("MM"));
                cmd.Parameters.AddWithValue("@AttYear", today.ToString("yyyy"));
                cmd.Parameters.AddWithValue("@LoanTypeId", aModel.LoanType);
                cmd.Parameters.AddWithValue("@LoanNo", aModel.LoanId);
                cmd.Parameters.AddWithValue("@LoanAmt", aModel.LoanAmound);
                cmd.Parameters.AddWithValue("@InstallmentSize", aModel.InstallmentAmound);
                cmd.Parameters.AddWithValue("@LoanAndAdvance", aModel.LoanAndAdvance);
                //cmd.Parameters.AddWithValue("@LoanStatus", aModel.SequentialStatuss);
                //cmd.Parameters.AddWithValue("@ProjectId", aModel.ProjectId);
                cmd.Parameters.AddWithValue("@EntryDate", today.ToString("yyyy-MM-dd"));
                //cmd.Parameters.AddWithValue("@EntryTime", today.ToLongTimeString());
                
                cmd.ExecuteNonQuery();
                string query2 = @"update tbl_HR_GLO_EMPLOYEE_DLS SET Amount = @InstallmentAmound where EmpId = @EmpId AND SalaryTypeId = @LoanTypeId";
                var cmd2 = new SqlCommand(query2, Con, _trans);
                cmd2.Parameters.Clear();
                cmd2.Parameters.AddWithValue("@InstallmentAmound", aModel.InstallmentAmound);
                cmd2.Parameters.AddWithValue("@EmpId", aModel.EmployeeId);
                cmd2.Parameters.AddWithValue("@LoanTypeId", aModel.LoanType);
                cmd2.ExecuteNonQuery();
                _trans.Commit();
                Con.Close();
                return Task.FromResult<string>("Save uccess");
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return Task.FromResult(exception.Message);
            }
        }

        public List<LoanAndAdvanceModel> GetLoanList()
        {
            try
            {
                
                var lists = new List<LoanAndAdvanceModel>();
                string query = "";

                query = @"Select a.InstallmentSize,a.EmpId,b.Code,b.Name AS employeeName,d.BName AS DepartmentName,e.BName AS DesigationName,a.LoanNo,c.SalaryNameBD,SUM(a.LoanAmt-a.PaidAmount) AS RestAmount, SUM(a.LoanAmt) AS LoanAmount,
                            SUM(a.PaidAmount) AS PaidAmount  from tbl_HR_LOAN_LEDGER AS a inner join tbl_HR_GLO_EMPLOYEE AS b ON a.EmpId = b.Id
                            inner join tbl_HR_GLO_SALARY_TYPE AS c ON a.LoanTypeId = c.Id
                            inner join tbl_DEPARTMENT_HR AS d ON b.DeparmentId = d.Id
                            inner join tbl_DESIGNATION_HR AS e ON b.DesignationId = e.Id
                            group by a.EmpId,a.LoanNo,b.Code,b.Name,d.BName,e.BName,c.SalaryNameBD,a.InstallmentSize
                            having SUM(a.LoanAmt-a.PaidAmount)>0";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    lists.Add(new LoanAndAdvanceModel()
                    {
                
                        EmployeeId = Convert.ToInt32(rdr["EmpId"]),
                        EmCode = rdr["Code"].ToString(),
                        EmName = rdr["employeeName"].ToString(),
                        DepartmentName = rdr["DepartmentName"].ToString(),
                        EmDesignationName = rdr["DesigationName"].ToString(),
                        LoanNo = Convert.ToInt32(rdr["LoanNo"]),
                        SalaryNameBD = rdr["SalaryNameBD"].ToString(),
                        RestLoanAmount = Convert.ToDouble(rdr["RestAmount"]),
                        LoanAmound = Convert.ToInt32(rdr["LoanAmount"]),
                        PaidAmount = Convert.ToDouble(rdr["PaidAmount"]),
                        InstallmentAmound = Convert.ToDouble(rdr["InstallmentSize"]),
                
                    });
                }
                rdr.Close();
                Con.Close();
                return lists;
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return new List<LoanAndAdvanceModel>();
            }
        }

        public Task<string> GetUpdateLoanInstallment(int EmpId, int LoanId, int Amount)
        {
            try
            {
                string query = @"Update tbl_HR_LOAN_LEDGER SET InstallmentSize=@InstallmentSize where EmpId = @EmpId AND LoanNo=@LoanNo";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@InstallmentSize", Amount);
                cmd.Parameters.AddWithValue("@EmpId", EmpId);
                cmd.Parameters.AddWithValue("@LoanNo", LoanId);
                cmd.ExecuteNonQuery();
                Con.Close();
                return Task.FromResult<string>("Save SSuccess");

               }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return Task.FromResult(exception.Message);
            }
        }

    }
}