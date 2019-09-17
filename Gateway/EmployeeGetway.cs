using System;
using System.Threading;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;
using Microsoft.Ajax.Utilities;

namespace HospitalManagementApp_Api.Gateway
{
    public class EmployeeGetway : DbConnection
    {
        private SqlTransaction _trans;
        public Task<string> Save(List<EmployeeModel> aModel)
        {
           try
         {

            Con.Open();
            Thread.Sleep(50);
            _trans = Con.BeginTransaction();
            const string query = @"INSERT INTO tbl_HR_GLO_EMPLOYEE (PaymentAmountCashPc,PaymentAmountBankPc,Code,ProtithanikCode,Tahabil,NID,TAXNumber,JibonBimaNumber,Name,PresentAddress,PermanentAddress,MobileNo,DateOfBirth,Gender,Nationality,Religion,DateOfJoining,DateOfConfirmation,DeparmentId,DesignationId,EmployeeBankAccountNo
      ,EmployeeBankId,EmployeeBankBranchId,ProjectId,EntryDate,Grade) OUTPUT Inserted.Id VALUES (@PaymentAmountCashPc,@PaymentAmountBankPc,@Code,@ProtithanikCode,@Tahabil,@NID,@TAXNumber,@JibonBimaNumber,@Name,@PresentAddress,@PermanentAddress,@MobileNo,@DateOfBirth,@Gender,@Nationality,@Religion,@DateOfJoining,@DateOfConfirmation,@DeparmentId,@DesignationId,@EmployeeBankAccountNo
      ,@EmployeeBankId,@EmployeeBankBranchId,@ProjectId,@EntryDate,@Grade)";

            var cmd = new SqlCommand(query, Con, _trans);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Code",aModel.ElementAt(0).EmCode);
            cmd.Parameters.AddWithValue("@ProtithanikCode",aModel.ElementAt(0).ProtithanikCode);
            cmd.Parameters.AddWithValue("@Tahabil",aModel.ElementAt(0).Tahabil);
            cmd.Parameters.AddWithValue("@NID",aModel.ElementAt(0).NID);
            cmd.Parameters.AddWithValue("@TAXNumber",aModel.ElementAt(0).TAXNumber);
            cmd.Parameters.AddWithValue("@JibonBimaNumber",aModel.ElementAt(0).JibonBimaNumber);
            cmd.Parameters.AddWithValue("@Name",aModel.ElementAt(0).EmName);
            cmd.Parameters.AddWithValue("@PresentAddress",aModel.ElementAt(0).EmPresentAddress);
            cmd.Parameters.AddWithValue("@PermanentAddress",aModel.ElementAt(0).EmPermanentAddress);
            cmd.Parameters.AddWithValue("@MobileNo",aModel.ElementAt(0).EmMobileNo);
            cmd.Parameters.AddWithValue("@DateOfBirth",aModel.ElementAt(0).EmDob.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@Gender",aModel.ElementAt(0).EmGender);
            cmd.Parameters.AddWithValue("@Nationality",aModel.ElementAt(0).EmNationality);
            cmd.Parameters.AddWithValue("@Religion",aModel.ElementAt(0).Emreligion);
            cmd.Parameters.AddWithValue("@DateOfJoining",aModel.ElementAt(0).EmDoj.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@DateOfConfirmation",aModel.ElementAt(0).EmDoConfirmation.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@DeparmentId",aModel.ElementAt(0).EmDepartmentId);
            cmd.Parameters.AddWithValue("@DesignationId",aModel.ElementAt(0).EmDesignationId);
            cmd.Parameters.AddWithValue("@EmployeeBankAccountNo",aModel.ElementAt(0).EmSalaryBankAccountNo);
            cmd.Parameters.AddWithValue("@EmployeeBankId",aModel.ElementAt(0).EmBankId);
            cmd.Parameters.AddWithValue("@EmployeeBankBranchId",aModel.ElementAt(0).EmBranchId);
            //cmd.Parameters.AddWithValue("@EmpImage",aModel.ElementAt(0).EmEmpImage);
            cmd.Parameters.AddWithValue("@ProjectId",aModel.ElementAt(0).EmProjectId);
            cmd.Parameters.AddWithValue("@EntryDate",DateTime.Now.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@Grade",aModel.ElementAt(0).Grade);
            cmd.Parameters.AddWithValue("@PaymentAmountCashPc", aModel.ElementAt(0).EmPaymentAmountCashPc);
            cmd.Parameters.AddWithValue("@PaymentAmountBankPc", aModel.ElementAt(0).EmPaymentAmountBankPc);
            //cmd.Parameters.AddWithValue("@BasicDeduction",aModel.ElementAt(0).EmBasicDeduction);
            //cmd.Parameters.AddWithValue("@GrossSalary",aModel.ElementAt(0).EmGrossSalary);
            //cmd.Parameters.AddWithValue("@BasicEarning", aModel.ElementAt(0).EmBasicEarning);
            var id = (int)cmd.ExecuteScalar();

             foreach (var employevar  in aModel)
             {
                employevar.EmId = id;
             }

            DataTable dt = ConvertListDataTable(aModel);
            var objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_HR_GLO_EMPLOYEE_DLS" };
            objbulk.ColumnMappings.Add("EmId", "EmpId");
            objbulk.ColumnMappings.Add("ItemType", "SalaryType");
            objbulk.ColumnMappings.Add("Itemid", "SalaryTypeId");
            objbulk.ColumnMappings.Add("ItemCharge", "Amount");
            objbulk.WriteToServer(dt);
            _trans.Commit();
            Con.Close();
            return Task.FromResult("Saved Success"); 
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

        public Task<string> Update(List<EmployeeModel> aModel)
        {
            try
            {
               
                Thread.Sleep(50);
                Con.Open();
                _trans = Con.BeginTransaction();
                string query = @"UPDATE tbl_HR_GLO_EMPLOYEE SET PaymentAmountCashPc=@PaymentAmountCashPc,PaymentAmountBankPc=@PaymentAmountBankPc,Code=@Code,ProtithanikCode=@ProtithanikCode,Tahabil=@Tahabil,NID=@NID,TAXNumber=@TAXNumber,JibonBimaNumber=@JibonBimaNumber,Name=@Name,PresentAddress=@PresentAddress,PermanentAddress=@PermanentAddress,MobileNo=@MobileNo,DateOfBirth=@DateOfBirth,Gender=@Gender,Nationality=@Nationality,Religion=@Religion,DateOfJoining=@DateOfJoining,DateOfConfirmation=@DateOfConfirmation,DeparmentId=@DeparmentId,DesignationId=@DesignationId,EmployeeBankAccountNo=@EmployeeBankAccountNo
                                 ,EmployeeBankId=@EmployeeBankId,EmployeeBankBranchId=@EmployeeBankBranchId,ProjectId=@ProjectId,EntryDate=@EntryDate,Grade=@Grade WHERE Id=@EmpId";

                var cmd = new SqlCommand(query, Con, _trans);
                               cmd.Parameters.Clear();
                               cmd.Parameters.AddWithValue("@Code", aModel.ElementAt(0).EmCode);
                               cmd.Parameters.AddWithValue("@ProtithanikCode", aModel.ElementAt(0).ProtithanikCode);
                               cmd.Parameters.AddWithValue("@Tahabil", aModel.ElementAt(0).Tahabil);
                               cmd.Parameters.AddWithValue("@NID", aModel.ElementAt(0).NID);
                               cmd.Parameters.AddWithValue("@TAXNumber", aModel.ElementAt(0).TAXNumber);
                               cmd.Parameters.AddWithValue("@JibonBimaNumber", aModel.ElementAt(0).JibonBimaNumber);
                               cmd.Parameters.AddWithValue("@Name", aModel.ElementAt(0).EmName);
                               cmd.Parameters.AddWithValue("@PresentAddress", aModel.ElementAt(0).EmPresentAddress);
                               cmd.Parameters.AddWithValue("@PermanentAddress", aModel.ElementAt(0).EmPermanentAddress);
                               cmd.Parameters.AddWithValue("@MobileNo", aModel.ElementAt(0).EmMobileNo);
                               cmd.Parameters.AddWithValue("@DateOfBirth", aModel.ElementAt(0).EmDob.ToString("yyyy-MM-dd"));
                               cmd.Parameters.AddWithValue("@Gender", aModel.ElementAt(0).EmGender);
                               cmd.Parameters.AddWithValue("@Nationality", aModel.ElementAt(0).EmNationality);
                               cmd.Parameters.AddWithValue("@Religion", aModel.ElementAt(0).Emreligion);
                               cmd.Parameters.AddWithValue("@DateOfJoining", aModel.ElementAt(0).EmDoj.ToString("yyyy-MM-dd"));
                               cmd.Parameters.AddWithValue("@DateOfConfirmation", aModel.ElementAt(0).EmDoConfirmation.ToString("yyyy-MM-dd"));
                               cmd.Parameters.AddWithValue("@DeparmentId", aModel.ElementAt(0).EmDepartmentId);
                               cmd.Parameters.AddWithValue("@DesignationId", aModel.ElementAt(0).EmDesignationId);
                               cmd.Parameters.AddWithValue("@EmployeeBankAccountNo", aModel.ElementAt(0).EmSalaryBankAccountNo);
                               cmd.Parameters.AddWithValue("@EmployeeBankId", aModel.ElementAt(0).EmBankId);
                               cmd.Parameters.AddWithValue("@EmployeeBankBranchId", aModel.ElementAt(0).EmBranchId);
                               //cmd.Parameters.AddWithValue("@EmpImage", aModel.ElementAt(0).EmEmpImage);
                               cmd.Parameters.AddWithValue("@ProjectId", aModel.ElementAt(0).EmProjectId);
                               cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                               cmd.Parameters.AddWithValue("@Grade", aModel.ElementAt(0).Grade);
                               cmd.Parameters.AddWithValue("@PaymentAmountCashPc", aModel.ElementAt(0).EmPaymentAmountCashPc);
                               cmd.Parameters.AddWithValue("@PaymentAmountBankPc", aModel.ElementAt(0).EmPaymentAmountBankPc);
                               //cmd.Parameters.AddWithValue("@BasicDeduction", aModel.ElementAt(0).EmBasicDeduction);
                               //cmd.Parameters.AddWithValue("@GrossSalary", aModel.ElementAt(0).EmGrossSalary);
                               //cmd.Parameters.AddWithValue("@BasicEarning", aModel.ElementAt(0).EmBasicEarning);
                               cmd.Parameters.AddWithValue("@EmpId", aModel.ElementAt(0).EmId);

                               cmd.ExecuteNonQuery();
                               var cmd1 = new SqlCommand(@"DELETE FROM tbl_HR_GLO_EMPLOYEE_DLS WHERE EmpId=" + aModel.ElementAt(0).EmId + "", Con, _trans);
                               cmd1.ExecuteNonQuery();
                             

                               DataTable dt = ConvertListDataTable(aModel);
                               var objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_HR_GLO_EMPLOYEE_DLS" };
                               objbulk.ColumnMappings.Add("EmId", "EmpId");
                               objbulk.ColumnMappings.Add("ItemType", "SalaryType");
                               objbulk.ColumnMappings.Add("Itemid", "SalaryTypeId");
                               objbulk.ColumnMappings.Add("ItemCharge", "Amount");
                               objbulk.WriteToServer(dt);
                               _trans.Commit();
                               Con.Close();
                               return Task.FromResult("Saved Success"); 

               
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
        public string SaveImageAndSignature(string EmCode, string imageString)
        {
            //Image signImage = Base64ToImage(imageString.Replace("data:image/jpeg;base64,", ""));
            //var logoImage = ImageToByteArray(signImage);

            if (FncSeekRecordNew("tbl_HR_GLO_EMPLOYEE", "Code='" + EmCode + "'"))
            {
                Con.Open();
                const string query = @"UPDATE tbl_HR_GLO_EMPLOYEE SET EmpImage=@EmpImage WHERE Code=@Code ";
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Code", EmCode);
                cmd.Parameters.AddWithValue("@EmpImage", imageString);
                cmd.ExecuteNonQuery();
                Con.Close();
            }
            return "Saved Success";
        }
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
        public List<IdNameForDropdownModel> GetIdCasCadeDropDown2(string query)
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
                        AnotherName = rdr["AnotherName"].ToString(),
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

        public int SaveNameOnly(string query)
        {
            //const string query = @"INSERT INTO tbl_BRAND (Cat_Id,Name,UserName) OUTPUT INSERTED.ID VALUES (@Cat_Id,@Name,@UserName)";
            var cmd = new SqlCommand(query, Con);
            cmd.Parameters.Clear();
            //cmd.Parameters.AddWithValue("@Cat_Id", catId);
            //cmd.Parameters.AddWithValue("@Name", brandName);
            
            Con.Open();
            //  cmd.ExecuteNonQuery();
            var id = (int)cmd.ExecuteScalar();
            Con.Close();
            return id;
        }


        public List<EmployeeModel> GetEmployeelDetalsList(string searchString)
        {
            try
            {

                string msg = "", condition = "";
                var list = new List<EmployeeModel>();
                string query = "";
                //if (searchString != "0") { condition = "WHERE (Code+Name+MobileNo) LIKE N'%' + '" + searchString + "' + '%' "; }
               //if (searchString != "0") { condition = "WHERE (Code+Name+MobileNo) LIKE N'%" + searchString + "%' "; }
                if (searchString != "0") { condition = "WHERE a.id =" + searchString + " "; }
                query = @"select 	e.Name AS GenderName,d.Name AS BankBranchName,f.BName AS DesignationName,g.Name AS BankName,h.Name AS ReligionName,
                        i.BName AS DeparmentName,j.BName AS UnitName,k.Name AS NationalityName,a.Id,a.Code,a.ProtithanikCode,a.Tahabil,a.NID,a.TAXNumber,a.JibonBimaNumber,a.Name,a.PresentAddress
                          ,a.PermanentAddress,a.MobileNo,a.DateOfBirth,a.Gender,a.Nationality,a.Religion,a.DateOfJoining,a.DateOfConfirmation
	                      ,a.WorkingStatus,a.DeparmentId,a.DesignationId,a.EmployeeBankAccountNo,a.CompanyBankAccountNo,a.CompanyBankId,a.CompanyBankBranchId
                          ,a.EmployeeBankId,a.EmployeeBankBranchId,a.PaymentType,a.PaymentAmountCashPc,a.PaymentAmountBankPc,a.ShiftStatus,a.EmpCardNo
	                      ,a.IsGetHoliday,a.EmpImage,a.UserName,a.ProjectId,a.Valid,a.EntryDate,a.Grade
	                      ,b.SalaryType,b.SalaryTypeId,b.Amount,c.SalaryNameBD,c.CustomId
	                       from tbl_HR_GLO_EMPLOYEE AS a inner join tbl_HR_GLO_EMPLOYEE_DLS AS b ON a.Id = b.EmpId
						   inner join tbl_HR_GLO_SALARY_TYPE AS c ON b.SalaryTypeId = c.Id
                            inner join tbl_GENDER_INFO_MST AS e ON a.Gender = e.Id
                            inner join tbl_DESIGNATION_HR AS f ON a.DesignationId=f.Id
                            inner join tbl_BANK AS g ON a.EmployeeBankId=g.Id
                            inner join tbl_RELIGION_INFO AS h ON a.Religion=h.Id
                            inner join tbl_DEPARTMENT_HR AS i ON a.DeparmentId=i.Id
                            inner join tbl_PROJECT_HR AS j ON a.ProjectId=j.Id
                            inner join tbl_NATIONALITY_INFO AS k ON a.Nationality=k.Id
						   inner Join tbl_BRANCH_OF_BANK_HR AS d ON a.EmployeeBankBranchId = d.Id " + condition + "";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
               cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                
                while (rdr.Read())
                {
                    
                    list.Add(new EmployeeModel
                    {
                        GenderName = rdr["GenderName"].ToString(),
                        EmDesignationName = rdr["DesignationName"].ToString(),
                        BankName = rdr["BankName"].ToString(),
                        ReligionName = rdr["ReligionName"].ToString(),
                        DepartmentName = rdr["DeparmentName"].ToString(),
                        UnitName = rdr["UnitName"].ToString(),
                        NationalName = rdr["NationalityName"].ToString(),
                        SalaryCustomId = rdr["CustomId"].ToString(),
                        //---------------------
                        EmId = Convert.ToInt32(rdr["Id"]),
                        EmCode = rdr["Code"].ToString(),
                        ProtithanikCode = rdr["ProtithanikCode"].ToString(),
                        Tahabil = rdr["Tahabil"].ToString(),
                        NID = rdr["NID"].ToString(),
                        TAXNumber = rdr["TAXNumber"].ToString(),
                        JibonBimaNumber = rdr["JibonBimaNumber"].ToString(),
                        EmName = rdr["Name"].ToString(),
                        EmPresentAddress = rdr["PresentAddress"].ToString(),
                        EmPermanentAddress = rdr["PermanentAddress"].ToString(),
                        EmMobileNo = rdr["MobileNo"].ToString(),
                        EmDob = Convert.ToDateTime(rdr["DateOfBirth"]),
                        EmGender = rdr["Gender"].ToString(),
                        EmNationality = rdr["Nationality"].ToString(),
                        Emreligion = rdr["Religion"].ToString(),
                        EmDoj = Convert.ToDateTime(rdr["DateOfJoining"]),
                        EmDoConfirmation = Convert.ToDateTime(rdr["DateOfConfirmation"]),
                        EmWorkingStatus = Convert.ToInt32(rdr["WorkingStatus"]),
                        EmDepartmentId = Convert.ToInt32(rdr["DeparmentId"]),
                        EmDesignationId = Convert.ToInt32(rdr["DesignationId"]),
                        EmSalaryBankAccountNo = rdr["EmployeeBankAccountNo"].ToString(),
                        EmMainBankAccountNo = rdr["CompanyBankAccountNo"].ToString(),
                        EmMainBankId = rdr["CompanyBankId"].ToString(),
                        EmMainBankBranchId = Convert.ToInt32(rdr["CompanyBankBranchId"]),
                        EmBankId = Convert.ToInt32(rdr["EmployeeBankId"]),
                        EmBranchId = Convert.ToInt32(rdr["EmployeeBankBranchId"]),
                        
                        //EmBasicEarning = Convert.ToDouble(rdr["BasicEarning"]),
                      
                        //EmBasicDeduction = Convert.ToDouble(rdr["BasicDeduction"]),
                        //EmGrossSalary = Convert.ToDouble(rdr["GrossSalary"]),
                        EmPaymentType = rdr["PaymentType"].ToString(),
                        EmPaymentAmountCashPc = Convert.ToInt32(rdr["PaymentAmountCashPc"]),
                        EmPaymentAmountBankPc = Convert.ToInt32(rdr["PaymentAmountBankPc"]),
                        EmShiftStatus = Convert.ToInt32(rdr["ShiftStatus"]),
                        EmEmpCardNo = rdr["EmpCardNo"].ToString(),
                        EmIsGetHoliday = Convert.ToInt32(rdr["IsGetHoliday"]),
                        EmUserName = rdr["UserName"].ToString(),
                        EmProjectId = Convert.ToInt32(rdr["ProjectId"]),
                        EmEmpImage = rdr["EmpImage"].ToString(),
              
                        EmValid = Convert.ToInt32(rdr["Valid"]),
                        EmEntryDate = Convert.ToDateTime(rdr["EntryDate"]), 
                        Grade = Convert.ToInt32(rdr["Grade"]),
                        Itemid = Convert.ToInt32(rdr["SalaryTypeId"]),
                        ItemType = rdr["SalaryType"].ToString(),
                        ItemCharge = Convert.ToDouble(rdr["Amount"]),
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
        public List<EmployeeModel> GetEmployeelList(string searchString)
        {
            try
            {

                string msg = "", condition = "";
                var list = new List<EmployeeModel>();
                string query = "";
                //if (searchString != "0") { condition = "WHERE (Code+Name+MobileNo) LIKE N'%' + '" + searchString + "' + '%' "; }
                if (searchString != "0") { condition = "WHERE (Code+a.Name+MobileNo) LIKE N'%" + searchString + "%' "; }
                //if (searchString != "0") { condition = "WHERE a.id =" + searchString + " "; }
                query = @"select a.Id,a.Code,a.Name,a.MobileNo,d.BName from tbl_HR_GLO_EMPLOYEE AS a inner join tbl_DESIGNATION_HR AS d ON a.DesignationId = d.Id " + condition + "";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new EmployeeModel()
                    {
                        EmId = Convert.ToInt32(rdr["Id"]),
                        EmCode = rdr["Code"].ToString(),
                        EmName = rdr["Name"].ToString(),
                        EmMobileNo = rdr["MobileNo"].ToString(),
                        EmDesignationName = rdr["BName"].ToString(),
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
    }
}