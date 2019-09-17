using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class SalaryCreateModel : LoanAndAdvanceModel
    {

        public int Id { get; set; }
        public int EmpId{ get; set; }
        public int DeparmentId{ get; set; }
        public int DesignationId{ get; set; }
        
        
        public int PresentDays{ get; set; }
        public int AbsentDays{ get; set; }
        public int LeaveDays{ get; set; }
        public int LateComingDays{ get; set; }
        public int DaysOfTheMonth{ get; set; }
        public string EmployeeBankAccountNo{ get; set; }
        public int EmployeeBankId{ get; set; }
        public int EmployeeBankBranchId{ get; set; }
        public string CompanyBankAccountNo{ get; set; }
        public int CompanyBankId{ get; set; }
        public int CompanyBankBranchId{ get; set; }
        public double Basic_Al{ get; set; }
        public double HouseRent_Al{ get; set; }
        public double Conveyance_Al{ get; set; }
        public double Project_Al{ get; set; }
        public double Mobile_Al{ get; set; }
        public double Night_Al{ get; set; }
        public double Medical_Al{ get; set; }
        public double Technical_Al{ get; set; }
        public double Meal_Al{ get; set; }
        public double Transport_Al{ get; set; }
        public double Other_Al{ get; set; }
        public double BasicEarning{ get; set; }
        public double ProvidentFund_Deduct{ get; set; }
        public double TDS_Deduct{ get; set; }
        public double Security_Deduct{ get; set; }
        public double Others_Deduct{ get; set; }
        public double BasicDeduction{ get; set; }
       // public double GrossSalary{ get; set; }
        public double Arrear_Salary{ get; set; }
        public double Arrear_Bonus{ get; set; }
        public double Arrear_Conveyance{ get; set; }
        public double Arrear_Mobile{ get; set; }
        public double Arrear_Medical{ get; set; }
        public double Arrear_Washing{ get; set; }
        public double Arrear_OverTime{ get; set; }
        public double Arrear_Transport{ get; set; }
        public double Arrear_DayAllowances{ get; set; }
        public double Arrear_Others{ get; set; }
        public double TotalArrear{ get; set; }
        public double TotalPayableSalary{ get; set; }
        public double Deduct_Loan{ get; set; }
        public double Deduct_Advance{ get; set; }
        public double Deduct_Late_Absence{ get; set; }
        public double Deduct_Others{ get; set; }
        public double Total_Other_Deduction{ get; set; }
        public double NetPayment{ get; set; }
        public int ProjectId{ get; set; }
        public int Valid{ get; set; }
        public string UserName{ get; set; }
        public int BranchId{ get; set; }
        public DateTime EntryDate{ get; set; }
        public string EntryTime{ get; set; }
        public string MonthName { get; set; }
        public string ProjectName { get; set; }
        public double PrepareBonus { get; set; }
        public int BonusFor{ get; set; }
        public double BonusAmount { get; set; }
        public double Year { get; set; }
        public int CashBank { get; set; }
        
    }
}