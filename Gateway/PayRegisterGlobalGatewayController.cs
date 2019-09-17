using System;
using System.Threading;
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
    public class PayRegisterGlobalGateway : DbConnection
    {
        public List<EmployeeModel> GetEmployeelDetalsList(int EmpId, int Month, int Year)
        {
            try
            {

                string msg = "", condition = "";
                var list = new List<EmployeeModel>();
                string query = "";
                //if (searchString != "0") { condition = "WHERE (Code+Name+MobileNo) LIKE N'%' + '" + searchString + "' + '%' "; }
                //if (searchString != "0") { condition = "WHERE (Code+Name+MobileNo) LIKE N'%" + searchString + "%' "; }
                if (EmpId != 0) { condition = "WHERE a.EmpId =" + EmpId + " AND MonthId = " + Month + " AND Year=" + Year + " "; }
                query = @"Select  e.Name AS BankBranchName,a.id,c.BName AS DepartmentName,d.BName AS DesigationName,b.Code,b.Name,a.MonthId, a.Year, a.EntryDate, a.FirstDate, a.SalaryCustomId, a.Amount, 
                          a.EmpId, a.SalaryType, a.SalaryId,a.ProjectId, a.DeparmentId, a.DesignationId, a.EmployeeBankAccountNo, a.EmployeeBankId, a.EmployeeBankBranchId, 
                        a.PaymentAmountCashPc, a.PaymentAmountBankPc 
                        from tbl_HR_GLO_PAYREGISTER AS a inner join tbl_HR_GLO_EMPLOYEE AS b ON a.EmpId=b.Id
                        inner join tbl_DEPARTMENT_HR AS c ON a.DeparmentId =c.Id
                        inner join tbl_DESIGNATION_HR AS d ON a.DesignationId=d.Id
                        inner join tbl_BRANCH_OF_BANK_HR AS e ON a.EmployeeBankBranchId = e.Id " + condition + "";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new EmployeeModel()
                    {
                        DepartmentName = rdr["DepartmentName"].ToString(),
                        EmDesignationName = rdr["DesigationName"].ToString(),
                        EmCode = rdr["Code"].ToString(),
                        EmName = rdr["Name"].ToString(),
                        MonthId = Convert.ToInt32(rdr["MonthId"]),
                        AttYear = Convert.ToInt32(rdr["Year"]),
                        EmEntryDate = Convert.ToDateTime(rdr["EntryDate"]),
                        EmFirstDate = Convert.ToDateTime(rdr["FirstDate"]),
                        SalaryCustomId = rdr["SalaryCustomId"].ToString(),
                        Amount = Convert.ToDouble(rdr["Amount"]),
                        EmId = Convert.ToInt32(rdr["EmpId"]),
                        SalaryType = rdr["SalaryType"].ToString(),
                        SalaryId = Convert.ToInt32(rdr["SalaryId"]),
                        EmProjectId = Convert.ToInt32(rdr["ProjectId"]),
                        EmDepartmentId = Convert.ToInt32(rdr["DeparmentId"]),
                        EmDesignationId = Convert.ToInt32(rdr["DesignationId"]),
                        EmMainBankAccountNo = rdr["EmployeeBankAccountNo"].ToString(),
                        EmBankId = Convert.ToInt32(rdr["EmployeeBankId"]),
                        EmMainBankBranchId = Convert.ToInt32(rdr["EmployeeBankBranchId"]),
                        EmPaymentAmountCashPc = Convert.ToInt32(rdr["PaymentAmountCashPc"]),
                        EmPaymentAmountBankPc = Convert.ToInt32(rdr["PaymentAmountBankPc"]),
                        EmBankBanchName = rdr["BankBranchName"].ToString(),
                  
                    });
                }
                rdr.Close();
                Con.Close();
                return list;
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return new List<EmployeeModel>();
            }
        }




        private SqlTransaction _trans;
        public Task<string> Save(List<EmployeeModel> aModel)
        {
            try
            {
                Con.Open();
                Thread.Sleep(50);
                _trans = Con.BeginTransaction();
                const string query = @"UPDATE tbl_HR_GLO_PAYREGISTER SET Amount=@Amount,EmployeeBankAccountNo=@EmployeeBankAccountNo,EmployeeBankId=@EmployeeBankId,EmployeeBankBranchId=@EmployeeBankBranchId,PaymentAmountCashPc=@PaymentAmountCashPc,PaymentAmountBankPc=@PaymentAmountBankPc Where MonthId=@MonthId AND  Year=@Year AND EmpId=@EmpId AND SalaryId=5000";
                var cmd = new SqlCommand(query, Con, _trans);
                //var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Amount", aModel[0].SumOfEarring);
                cmd.Parameters.AddWithValue("@EmployeeBankAccountNo", aModel[0].EmSalaryBankAccountNo);
                cmd.Parameters.AddWithValue("@EmployeeBankId", aModel[0].EmBankId);
                cmd.Parameters.AddWithValue("@EmployeeBankBranchId", aModel[0].EmBranchId);
                cmd.Parameters.AddWithValue("@PaymentAmountCashPc", aModel[0].EmPaymentAmountCashPc);
                cmd.Parameters.AddWithValue("@PaymentAmountBankPc", aModel[0].EmPaymentAmountBankPc);
                cmd.Parameters.AddWithValue("@MonthId", aModel[0].MonthId);
                cmd.Parameters.AddWithValue("@Year", aModel[0].AttYear);
                cmd.Parameters.AddWithValue("@EmpId", aModel[0].EmId);
                cmd.ExecuteNonQuery();


                const string query2 = @"UPDATE tbl_HR_GLO_PAYREGISTER SET Amount=@Amount,EmployeeBankAccountNo=@EmployeeBankAccountNo,
                                            EmployeeBankId=@EmployeeBankId,EmployeeBankBranchId=@EmployeeBankBranchId,
                                            PaymentAmountCashPc=@PaymentAmountCashPc,PaymentAmountBankPc=@PaymentAmountBankPc 
                                            Where MonthId=@MonthId AND  Year=@Year AND EmpId=@EmpId AND SalaryId=10000";
                var cmd2 = new SqlCommand(query2, Con, _trans);
                cmd2.Parameters.Clear();
                cmd2.Parameters.AddWithValue("@Amount", aModel[0].SumOfDetection);
                cmd2.Parameters.AddWithValue("@EmployeeBankAccountNo", aModel.ElementAt(0).EmSalaryBankAccountNo);
                cmd2.Parameters.AddWithValue("@EmployeeBankId", aModel.ElementAt(0).EmBankId);
                cmd2.Parameters.AddWithValue("@EmployeeBankBranchId", aModel.ElementAt(0).EmBranchId);
                cmd2.Parameters.AddWithValue("@PaymentAmountCashPc", aModel.ElementAt(0).EmPaymentAmountCashPc);
                cmd2.Parameters.AddWithValue("@PaymentAmountBankPc", aModel.ElementAt(0).EmPaymentAmountBankPc);
                cmd2.Parameters.AddWithValue("@MonthId", aModel.ElementAt(0).MonthId);
                cmd2.Parameters.AddWithValue("@Year", aModel.ElementAt(0).AttYear);
                cmd2.Parameters.AddWithValue("@EmpId", aModel.ElementAt(0).EmId);
                cmd2.ExecuteNonQuery();


                const string query3 = @"UPDATE tbl_HR_GLO_PAYREGISTER SET Amount=@Amount,EmployeeBankAccountNo=@EmployeeBankAccountNo,
                                            EmployeeBankId=@EmployeeBankId,EmployeeBankBranchId=@EmployeeBankBranchId,
                                            PaymentAmountCashPc=@PaymentAmountCashPc,PaymentAmountBankPc=@PaymentAmountBankPc 
                                            Where MonthId=@MonthId AND  Year=@Year AND EmpId=@EmpId AND SalaryId=15000";
                var cmd3 = new SqlCommand(query3, Con, _trans);
                cmd3.Parameters.Clear();
                cmd3.Parameters.AddWithValue("@Amount", aModel.ElementAt(0).SumOfGet);
                cmd3.Parameters.AddWithValue("@EmployeeBankAccountNo", aModel.ElementAt(0).EmSalaryBankAccountNo);
                cmd3.Parameters.AddWithValue("@EmployeeBankId", aModel.ElementAt(0).EmBankId);
                cmd3.Parameters.AddWithValue("@EmployeeBankBranchId", aModel.ElementAt(0).EmBranchId);
                cmd3.Parameters.AddWithValue("@PaymentAmountCashPc", aModel.ElementAt(0).EmPaymentAmountCashPc);
                cmd3.Parameters.AddWithValue("@PaymentAmountBankPc", aModel.ElementAt(0).EmPaymentAmountBankPc);
                cmd3.Parameters.AddWithValue("@MonthId", aModel.ElementAt(0).MonthId);
                cmd3.Parameters.AddWithValue("@Year", aModel.ElementAt(0).AttYear);
                cmd3.Parameters.AddWithValue("@EmpId", aModel.ElementAt(0).EmId);
                cmd3.ExecuteNonQuery();



                foreach (var model in aModel)
                {

                    const string query1 = @"UPDATE tbl_HR_GLO_PAYREGISTER SET Amount=@Amount,EmployeeBankAccountNo=@EmployeeBankAccountNo,
                                            EmployeeBankId=@EmployeeBankId,EmployeeBankBranchId=@EmployeeBankBranchId,
                                            PaymentAmountCashPc=@PaymentAmountCashPc,PaymentAmountBankPc=@PaymentAmountBankPc 
                                            Where MonthId=@MonthId AND  Year=@Year AND EmpId=@EmpId AND SalaryId=@SalaryId";
                    var cmd1 = new SqlCommand(query1, Con, _trans);
                    cmd1.Parameters.Clear();
                    cmd1.Parameters.AddWithValue("@Amount", model.Amount);
                    cmd1.Parameters.AddWithValue("@EmployeeBankAccountNo", model.EmSalaryBankAccountNo);
                    cmd1.Parameters.AddWithValue("@EmployeeBankId", model.EmBankId);
                    cmd1.Parameters.AddWithValue("@EmployeeBankBranchId", model.EmBranchId);
                    cmd1.Parameters.AddWithValue("@PaymentAmountCashPc", model.EmPaymentAmountCashPc);
                    cmd1.Parameters.AddWithValue("@PaymentAmountBankPc", model.EmPaymentAmountBankPc);
                    cmd1.Parameters.AddWithValue("@MonthId", model.MonthId);
                    cmd1.Parameters.AddWithValue("@Year", model.AttYear);
                    cmd1.Parameters.AddWithValue("@EmpId", model.EmId);
                    cmd1.Parameters.AddWithValue("@SalaryId", model.SalaryId);
                    cmd1.ExecuteNonQuery();
                }

                _trans.Commit();
                Con.Close();
                return Task.FromResult("Saved Success");
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    _trans.Rollback();
                    Con.Close();
                }
                return Task.FromResult(exception.Message);
            }

        }
	}
}