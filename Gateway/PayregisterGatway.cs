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
    public class PayregisterGatway : DbConnection
    {
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

        public List<SalaryCreateModel> GetEmployeeDetailById(string searchString)
        {
            try
            {
                string condition = "";
                var lists = new List<SalaryCreateModel>();
                string query = "";
                //if (searchString != "0") { condition = "WHERE (Name) LIKE '%' + '" + searchString + "' + '%' "; }

                //                query = @"SELECT Id,CompanyId,Name,PackSize,ProductUnit,Tp,SalePrice,ReminderStock,PDGroupId,UserName,EntryDate,BranchId,EntryTime
                //							From tbl_PHAR_PRODUCT WHERE Id = " + searchString + " ";
                query = @"Select a.EmpId,
                                b.Name AS EmployeeName,
                                c.Name AS Disingination,
                                d.Name As Department,
                                a.AbsentDays,
                                b.DateOfJoining,
                                a.EmployeeBankAccountNo,
	                            e.Name AS NameOfMonth, 
                                f.Name AS ProjecName,
                                a.Basic_Al,
                                a.HouseRent_Al,
                                a.Conveyance_Al,
                                a.Project_Al,
                                a.Mobile_Al,
                                a.Night_Al,
                                a.Medical_Al,
	                            a.Technical_Al,
                                a.Meal_Al,
                                a.Transport_Al,
                                a.Other_Al,
                                a.BasicEarning,
                                a.ProvidentFund_Deduct,
                                a.TDS_Deduct,
                                a.Security_Deduct,
                                a.Others_Deduct,
	                            a.BasicDeduction,
                                a.GrossSalary,
                                a.Arrear_Salary,
                                a.Arrear_Bonus,
                                a.Arrear_Conveyance, 
                                a.Arrear_Mobile,
                                a.Arrear_Medical,
                                a.Arrear_Washing,
                                a.Arrear_OverTime,
                                a.Arrear_Transport,
                                a.Arrear_DayAllowances, 
                                a.Arrear_Others,
                                a.TotalArrear,
                                a.TotalPayableSalary,
                                a.Deduct_Loan,
                                a.Deduct_Advance,
                                a.Deduct_Late_Absence,
                                a.Deduct_Others,
                                a.Total_Other_Deduction,
                                a.NetPayment
                            From tbl_HR_PAYREGISTER As a inner join tbl_EMPLOYEE_HR AS b
	                        ON a.EmpId = b.Id inner join tbl_DESIGNATION_HR AS c
	                        ON a.DesignationId = c.Id inner join tbl_DEPARTMENT_HR AS d
	                        ON a.DeparmentId = d.Id inner join tbl_HR_MONTH_INFO AS e
	                        ON a.MonthId = e.Id inner join tbl_PROJECT_HR AS f
	                        ON a.ProjectId = f.Id WHERE a.EntryDate = ( SELECT MAX(EntryDate) FROM tbl_HR_PAYREGISTER) AND a.EmpId = " + searchString + " ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    lists.Add(new SalaryCreateModel()
                    {
                        EmpId = Convert.ToInt32(rdr["EmpId"]),
                        Name = rdr["EmployeeName"].ToString(),
                        DepartmentName = rdr["Department"].ToString(),
                        DesignationName = rdr["Disingination"].ToString(),
                        AbsentDays = Convert.ToInt32(rdr["AbsentDays"]),
                        EmDoj = Convert.ToDateTime(rdr["DateOfJoining"]),
                        EmployeeBankAccountNo = rdr["EmployeeBankAccountNo"].ToString(),
                        MonthName = rdr["NameOfMonth"].ToString(),
                        ProjectName = rdr["ProjecName"].ToString(),

                        Basic_Al = Convert.ToDouble(rdr["Basic_Al"]),
                        HouseRent_Al = Convert.ToDouble(rdr["HouseRent_Al"]),
                        Conveyance_Al = Convert.ToDouble(rdr["Conveyance_Al"]),
                        Project_Al = Convert.ToDouble(rdr["Project_Al"]),
                        Mobile_Al = Convert.ToDouble(rdr["Mobile_Al"]),
                        Night_Al = Convert.ToDouble(rdr["Night_Al"]),
                        Medical_Al = Convert.ToDouble(rdr["Medical_Al"]),
                        Technical_Al = Convert.ToDouble(rdr["Technical_Al"]),
                        Meal_Al = Convert.ToDouble(rdr["Meal_Al"]),
                        Transport_Al = Convert.ToDouble(rdr["Transport_Al"]),
                        Other_Al= Convert.ToDouble(rdr["Other_Al"]),
                        BasicEarning = Convert.ToDouble(rdr["BasicEarning"]),
                        
                        ProvidentFund_Deduct= Convert.ToDouble(rdr["ProvidentFund_Deduct"]),
                        TDS_Deduct= Convert.ToDouble(rdr["TDS_Deduct"]),
                        Security_Deduct = Convert.ToDouble(rdr["Security_Deduct"]),
                        Others_Deduct = Convert.ToDouble(rdr["Others_Deduct"]),
                        BasicDeduction = Convert.ToDouble(rdr["BasicDeduction"]),

                        EmGrossSalary = Convert.ToDouble(rdr["GrossSalary"]),
                        
                        Arrear_Salary= Convert.ToDouble(rdr["Arrear_Salary"]),
                        Arrear_Bonus= Convert.ToDouble(rdr["Arrear_Bonus"]),
                        Arrear_Conveyance = Convert.ToDouble(rdr["Arrear_Conveyance"]),
                        Arrear_Mobile = Convert.ToDouble(rdr["Arrear_Mobile"]),
                        Arrear_Medical = Convert.ToDouble(rdr["Arrear_Medical"]),
                        Arrear_Washing = Convert.ToDouble(rdr["Arrear_Washing"]),
                        Arrear_OverTime  = Convert.ToDouble(rdr["Arrear_OverTime"]),
                        Arrear_Transport = Convert.ToDouble(rdr["Arrear_Transport"]),
                        Arrear_DayAllowances = Convert.ToDouble(rdr["Arrear_DayAllowances"]),
                        Arrear_Others = Convert.ToDouble(rdr["Arrear_Others"]),
                        TotalArrear = Convert.ToDouble(rdr["TotalArrear"]),
                        
                        TotalPayableSalary = Convert.ToDouble(rdr["TotalPayableSalary"]),
                       
                        Deduct_Loan = Convert.ToDouble(rdr["Deduct_Loan"]),
                        Deduct_Advance = Convert.ToDouble(rdr["Deduct_Advance"]),
                        Deduct_Late_Absence = Convert.ToDouble(rdr["Deduct_Late_Absence"]),
                        Deduct_Others = Convert.ToDouble(rdr["Deduct_Others"]),
                        Total_Other_Deduction = Convert.ToDouble(rdr["Total_Other_Deduction"]),

                        NetPayment = Convert.ToDouble(rdr["NetPayment"]),

                    
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

        public Task<string> Save(SalaryCreateModel aModel)
        {
            try
            {

//                const string query = @"UPDATE tbl_EMPLOYEE_HR SET Code=@Code, Name=@Name, PresentAddress=@PresentAddress, PermanentAddress=@PermanentAddress, MobileNo=@MobileNo, DateOfBirth=@DateOfBirth, Gender=@Gender, Nationality=@Nationality, Religion=@Religion, DateOfJoining=@DateOfJoining, 
//                                        DateOfConfirmation=@DateOfConfirmation, WorkingStatus=@WorkingStatus, DeparmentId=@DeparmentId, DesignationId=@DesignationId, EmployeeBankAccountNo=@EmployeeBankAccountNo, CompanyBankAccountNo=@CompanyBankAccountNo, CompanyBankId=@CompanyBankId, CompanyBankBranchId=@CompanyBankBranchId,
//                                        BranchId=@BranchId, Basic_Al=@Basic_Al, HouseRent_Al=@HouseRent_Al, Conveyance_Al=@Conveyance_Al, Project_Al=@Project_Al, Mobile_Al=@Mobile_Al, Night_Al=@Night_Al, Medical_Al=@Medical_Al, Technical_Al=@Technical_Al,
//                                            Meal_Al=@Meal_Al, Transport_Al=@Transport_Al, Other_Al=@Other_Al, BasicEarning=@BasicEarning, ProvidentFund_Deduct=@ProvidentFund_Deduct, TDS_Deduct=@TDS_Deduct, Security_Deduct=@Security_Deduct, Others_Deduct=@Others_Deduct, BasicDeduction=@BasicDeduction,
//                                                GrossSalary=@GrossSalary,PaymentType=@PaymentType, PaymentAmountCashPc=@PaymentAmountCashPc, PaymentAmountBankPc=@PaymentAmountBankPc,ShiftStatus=@ShiftStatus, EmpCardNo=@EmpCardNo, IsGetHoliday=@IsGetHoliday, EmpImage=@EmpImage, UserName=@UserName,ProjectId=@ProjectId, Valid=@Valid, EntryDate=@EntryDate  WHERE Id=@Id ";
                const string query = @" UPDATE tbl_HR_PAYREGISTER SET 
                                Basic_Al=@Basic_Al,
                                HouseRent_Al=@HouseRent_Al,
                                Conveyance_Al=@Conveyance_Al,
                                Project_Al=@Project_Al,
                                Mobile_Al=@Mobile_Al,
                                Night_Al=@Night_Al,
                                Medical_Al=@Medical_Al,
	                            Technical_Al=@Technical_Al,
                                Meal_Al=@Meal_Al,
                                Transport_Al=@Transport_Al,
                                Other_Al=@Other_Al,
                                BasicEarning=@BasicEarning,
                                ProvidentFund_Deduct=@ProvidentFund_Deduct,
                                TDS_Deduct=@TDS_Deduct,
                                Security_Deduct=@Security_Deduct,
                                Others_Deduct=@Others_Deduct,
	                            BasicDeduction=@BasicDeduction,
                                GrossSalary=@GrossSalary,
                                Arrear_Salary=@Arrear_Salary,
                                Arrear_Bonus=@Arrear_Bonus,
                                Arrear_Conveyance=@Arrear_Conveyance, 
                                Arrear_Mobile=@Arrear_Mobile,
                                Arrear_Medical=@Arrear_Medical,
                                Arrear_Washing=@Arrear_Washing,
                                Arrear_OverTime=@Arrear_OverTime,
                                Arrear_Transport=@Arrear_Transport,
                                Arrear_DayAllowances=@Arrear_DayAllowances, 
                                Arrear_Others=@Arrear_Others,
                                TotalArrear=@TotalArrear,
                                TotalPayableSalary=@TotalPayableSalary,
                                Deduct_Loan=@Deduct_Loan,
                                Deduct_Advance=@Deduct_Advance,
                                Deduct_Late_Absence=@Deduct_Late_Absence,
                                Deduct_Others=@Deduct_Others,
                                Total_Other_Deduction=@Total_Other_Deduction,
                                NetPayment=@NetPayment
                                WHERE EmpId=@EmpId ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@EmpId", aModel.EmpId);
                
                cmd.Parameters.AddWithValue("@Basic_Al", aModel.Basic_Al);
                cmd.Parameters.AddWithValue("@HouseRent_Al", aModel.HouseRent_Al);
                cmd.Parameters.AddWithValue("@Conveyance_Al", aModel.Conveyance_Al);
                cmd.Parameters.AddWithValue("@Project_Al", aModel.Project_Al);
                cmd.Parameters.AddWithValue("@Mobile_Al", aModel.Mobile_Al);
                cmd.Parameters.AddWithValue("@Night_Al", aModel.Night_Al);
                cmd.Parameters.AddWithValue("@Medical_Al", aModel.Medical_Al);
                cmd.Parameters.AddWithValue("@Technical_Al", aModel.Technical_Al);
                cmd.Parameters.AddWithValue("@Meal_Al", aModel.Meal_Al);
                cmd.Parameters.AddWithValue("@Transport_Al", aModel.Transport_Al);
                cmd.Parameters.AddWithValue("@Other_Al", aModel.Other_Al);
                cmd.Parameters.AddWithValue("@BasicEarning", aModel.BasicEarning);
                
                cmd.Parameters.AddWithValue("@ProvidentFund_Deduct", aModel.ProvidentFund_Deduct);
                cmd.Parameters.AddWithValue("@TDS_Deduct", aModel.TDS_Deduct);
                cmd.Parameters.AddWithValue("@Security_Deduct", aModel.Security_Deduct);
                cmd.Parameters.AddWithValue("@Others_Deduct", aModel.Others_Deduct);
                cmd.Parameters.AddWithValue("@BasicDeduction", aModel.BasicDeduction);

                cmd.Parameters.AddWithValue("@GrossSalary", aModel.EmGrossSalary);

                cmd.Parameters.AddWithValue("@Arrear_Salary", aModel.Arrear_Salary);
                cmd.Parameters.AddWithValue("@Arrear_Bonus", aModel.Arrear_Bonus);
                cmd.Parameters.AddWithValue("@Arrear_Conveyance", aModel.Arrear_Conveyance);
                cmd.Parameters.AddWithValue("@Arrear_Mobile", aModel.Arrear_Mobile);
                cmd.Parameters.AddWithValue("@Arrear_Medical", aModel.Arrear_Medical);
                cmd.Parameters.AddWithValue("@Arrear_Washing", aModel.Arrear_Washing);
                cmd.Parameters.AddWithValue("@Arrear_OverTime", aModel.Arrear_OverTime);
                cmd.Parameters.AddWithValue("@Arrear_Transport", aModel.Arrear_Transport);
                cmd.Parameters.AddWithValue("@Arrear_DayAllowances", aModel.Arrear_DayAllowances);
                cmd.Parameters.AddWithValue("@Arrear_Others", aModel.Arrear_Others);
                cmd.Parameters.AddWithValue("@TotalArrear", aModel.TotalArrear);

                cmd.Parameters.AddWithValue("@TotalPayableSalary", aModel.TotalPayableSalary);

                cmd.Parameters.AddWithValue("@Deduct_Loan", aModel.Deduct_Loan);
                cmd.Parameters.AddWithValue("@Deduct_Advance", aModel.Deduct_Advance);
                cmd.Parameters.AddWithValue("@Deduct_Late_Absence", aModel.Deduct_Late_Absence);
                cmd.Parameters.AddWithValue("@Deduct_Others", aModel.Deduct_Others);
                cmd.Parameters.AddWithValue("@Total_Other_Deduction", aModel.Total_Other_Deduction);

                cmd.Parameters.AddWithValue("@NetPayment", aModel.NetPayment);
                
               
               
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