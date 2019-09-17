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
    public class BonusCreateGetway : DbConnection
    {
        public Task<string> Save(SalaryCreateModel aModel)
        {
            try
            {
                string query ="";
                if (aModel.BonusFor == 1 || aModel.BonusFor == 2)
                {
                     query = @"INSERT INTO tbl_HR_BONUS_REGISTER(EmpId, DeparmentId, DesignationId, MonthId, AttYear, EmployeeBankAccountNo, EmployeeBankId, EmployeeBankBranchId, BonusFor, Basic_Earn, BonusAmount, UserName) 
                Select a.Id,a.DeparmentId,a.DesignationId,@MonthId,@AttYear,a.EmployeeBankAccountNo,a.EmployeeBankId,a.EmployeeBankBranchId,@BonusFor,
                b.Amount AS bsaisSalary,b.Amount AS Bonus,@EntryName
                from tbl_HR_GLO_EMPLOYEE AS a inner join tbl_HR_GLO_EMPLOYEE_DLS AS b ON a.Id=b.EmpId
                where b.Amount >0 And b.SalaryTypeId in (34,1) And a.Religion=1";

                }
                
                else if (aModel.BonusFor == 3)
                {
                    query = @"INSERT INTO tbl_HR_BONUS_REGISTER(EmpId, DeparmentId, DesignationId, MonthId, AttYear, EmployeeBankAccountNo, EmployeeBankId, EmployeeBankBranchId, BonusFor, Basic_Earn, BonusAmount, UserName) 
                Select a.Id,a.DeparmentId,a.DesignationId,@MonthId,@AttYear,a.EmployeeBankAccountNo,a.EmployeeBankId,a.EmployeeBankBranchId,@BonusFor,
                b.Amount AS bsaisSalary,(b.Amount*0.2) AS Bonus,@EntryName
                from tbl_HR_GLO_EMPLOYEE AS a inner join tbl_HR_GLO_EMPLOYEE_DLS AS b ON a.Id=b.EmpId
                where b.Amount >0 And b.SalaryTypeId in (34,1) ";
                }
                else if (aModel.BonusFor == 4)
                {
                    query = @"INSERT INTO tbl_HR_BONUS_REGISTER(EmpId, DeparmentId, DesignationId, MonthId, AttYear, EmployeeBankAccountNo, EmployeeBankId, EmployeeBankBranchId, BonusFor, Basic_Earn, BonusAmount, UserName) 
                Select a.Id,a.DeparmentId,a.DesignationId,@MonthId,@AttYear,a.EmployeeBankAccountNo,a.EmployeeBankId,a.EmployeeBankBranchId,@BonusFor,
                b.Amount AS bsaisSalary,(b.Amount*2) AS Bonus,@EntryName
                from tbl_HR_GLO_EMPLOYEE AS a inner join tbl_HR_GLO_EMPLOYEE_DLS AS b ON a.Id=b.EmpId
                where b.Amount >0 And b.SalaryTypeId in (34,1) And a.Religion<>1";

                }
                //INSERT INTO tbl_HR_BONUS_REGISTER(EmpId, DeparmentId, DesignationId, MonthId, AttYear, EmployeeBankAccountNo, EmployeeBankId, EmployeeBankBranchId, BonusFor, Basic_Earn, BonusAmount, ProjectId, UserName, EntryDate, EntryTime) 
                //Select a.Id,a.DeparmentId,a.DesignationId,5,2018,a.EmployeeBankAccountNo,a.EmployeeBankId,a.EmployeeBankBranchId,1,
                //b.Amount AS bsaisSalary,b.Amount AS Bonus,a.ProjectId,1,'2019-05-27','21:59'
                //from tbl_HR_GLO_EMPLOYEE AS a inner join tbl_HR_GLO_EMPLOYEE_DLS AS b ON a.Id=b.EmpId
                //where b.Amount >0 and b.SalaryTypeId in (34,1)

//                 string query = @"INSERT INTO tbl_HR_BONUS_REGISTER ( EmpId,DeparmentId,DesignationId,MonthId,AttYear,EmployeeBankAccountNo,EmployeeBankId,
//                                        EmployeeBankBranchId,CompanyBankAccountNo,CompanyBankId,CompanyBankBranchId,BonusFor,Basic_Earn,BonusPc,BonusAmount,ProjectId,
//                                        Valid,UserName,BranchId,EntryDate,EntryTime)  SELECT EmpId,DeparmentId,DesignationId,@MonthId,@AttYear,EmployeeBankAccountNo,EmployeeBankId,
//                                        EmployeeBankBranchId,CompanyBankAccountNo,CompanyBankId,CompanyBankBranchId,@BonusFor,BasicEarning,@BonusPc,BasicEarning*@BonusPc*0.01,ProjectId,
//                                        Valid,UserName,BranchId,@EntryDate,''  FROM tbl_HR_PAYREGISTER where DeparmentId = @DeparmentId ";

                Con.Open();

                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@MonthId", aModel.MonthId);
                cmd.Parameters.AddWithValue("@AttYear", aModel.AttYear);
                cmd.Parameters.AddWithValue("@BonusFor", aModel.BonusFor);
                cmd.Parameters.AddWithValue("@EntryName", aModel.UserName);
                

                cmd.ExecuteNonQuery();
                Con.Close();
                return Task.FromResult<string>("Save Success");
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