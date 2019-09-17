using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;
using Microsoft.Reporting.WebForms;
namespace HospitalManagementApp_Api.Gateway
{
    public class BonusRegisterGetway : DbConnection
    {
        public List<SalaryCreateModel> GetEmployeeDetailById(string searchString)
        {
            try
            {
                string condition = "";
                var lists = new List<SalaryCreateModel>();
                string query = "";
               
                query = @"Select b.Name AS EmployeeName,c.Name AS Designation, d.Name AS Department,e.Name As MonthsName,
                            a.AttYear,b.DateOfJoining, b.EmployeeBankAccountNo, a.Basic_Earn, a.BonusPc, a.BonusAmount
                            From tbl_HR_BONUS_REGISTER As a inner join tbl_EMPLOYEE_HR AS b 
                            ON a.EmpId = b.id inner join tbl_DESIGNATION_HR AS c
                            ON a.DesignationId = c.Id inner join tbl_DEPARTMENT_HR AS d
                            ON a.DeparmentId = d.Id inner join tbl_HR_MONTH_INFO AS e
                            ON a.MonthId = e.Id WHERE a.EntryDate = ( SELECT MAX(EntryDate) FROM tbl_HR_BONUS_REGISTER) AND a.EmpId =" + searchString + "";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    lists.Add(new SalaryCreateModel()
                    {

                        Name = rdr["EmployeeName"].ToString(),
                        DepartmentName = rdr["Department"].ToString(),
                        DesignationName = rdr["Designation"].ToString(),
                        Year = Convert.ToDouble(rdr["AttYear"]),
                        EmDoj = Convert.ToDateTime(rdr["DateOfJoining"]),
                        EmployeeBankAccountNo = rdr["EmployeeBankAccountNo"].ToString(),
                        MonthName = rdr["MonthsName"].ToString(),
                        BasicEarning = Convert.ToDouble(rdr["Basic_Earn"]),
                        PrepareBonus = Convert.ToDouble(rdr["BonusPc"]),
                        BonusAmount = Convert.ToDouble(rdr["BonusAmount"]),

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
                return new List<SalaryCreateModel>();
            }
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


        public Task<string> Save(SalaryCreateModel aModel)
        {
            try
            {

                //                const string query = @"UPDATE tbl_EMPLOYEE_HR SET Code=@Code, Name=@Name, PresentAddress=@PresentAddress, PermanentAddress=@PermanentAddress, MobileNo=@MobileNo, DateOfBirth=@DateOfBirth, Gender=@Gender, Nationality=@Nationality, Religion=@Religion, DateOfJoining=@DateOfJoining, 
                //                                        DateOfConfirmation=@DateOfConfirmation, WorkingStatus=@WorkingStatus, DeparmentId=@DeparmentId, DesignationId=@DesignationId, EmployeeBankAccountNo=@EmployeeBankAccountNo, CompanyBankAccountNo=@CompanyBankAccountNo, CompanyBankId=@CompanyBankId, CompanyBankBranchId=@CompanyBankBranchId,
                //                                        BranchId=@BranchId, Basic_Al=@Basic_Al, HouseRent_Al=@HouseRent_Al, Conveyance_Al=@Conveyance_Al, Project_Al=@Project_Al, Mobile_Al=@Mobile_Al, Night_Al=@Night_Al, Medical_Al=@Medical_Al, Technical_Al=@Technical_Al,
                //                                            Meal_Al=@Meal_Al, Transport_Al=@Transport_Al, Other_Al=@Other_Al, BasicEarning=@BasicEarning, ProvidentFund_Deduct=@ProvidentFund_Deduct, TDS_Deduct=@TDS_Deduct, Security_Deduct=@Security_Deduct, Others_Deduct=@Others_Deduct, BasicDeduction=@BasicDeduction,
                //                                                GrossSalary=@GrossSalary,PaymentType=@PaymentType, PaymentAmountCashPc=@PaymentAmountCashPc, PaymentAmountBankPc=@PaymentAmountBankPc,ShiftStatus=@ShiftStatus, EmpCardNo=@EmpCardNo, IsGetHoliday=@IsGetHoliday, EmpImage=@EmpImage, UserName=@UserName,ProjectId=@ProjectId, Valid=@Valid, EntryDate=@EntryDate  WHERE Id=@Id ";
                const string query = @" UPDATE tbl_HR_BONUS_REGISTER SET 
                                CashBank=@CashBank,
                                BonusPc=@BonusPc,
                                BonusAmount=@BonusAmount
                                WHERE Id=@EmpId ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@EmpId", aModel.EmpId);
                cmd.Parameters.AddWithValue("@BonusAmount", aModel.BonusAmount);
                cmd.Parameters.AddWithValue("@BonusPc", aModel.PrepareBonus);
                cmd.Parameters.AddWithValue("@CashBank", aModel.CashBank);
                                
                cmd.ExecuteNonQuery();
                Con.Close();
                return Task.FromResult<string>("Update success");
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