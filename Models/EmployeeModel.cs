using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class EmployeeModel
    {

        public int EmId { get; set; }
        public string EmCode { get; set; }
        public string EmName { get; set; }
        public string EmPresentAddress { get; set; }
        public string EmPermanentAddress { get; set; }
        public string EmMobileNo { get; set; }
        public DateTime EmDob { get; set; }
        public string EmGender { get; set; }
        public string EmNationality { get; set; }
        public string Emreligion { get; set; }
        public DateTime EmDoj { get; set; }
        public DateTime EmDoConfirmation { get; set; }
        public int EmWorkingStatus { get; set; }
        public int EmDepartmentId { get; set; }
        public int EmDesignationId { get; set; }
        public string EmSalaryBankAccountNo { get; set; }
        public string EmMainBankAccountNo { get; set; }
        public string EmMainBankId { get; set; }
        public int EmMainBankBranchId { get; set; }
        public int EmBankId { get; set; }
        public int EmBranchId { get; set; }
        public double EmBasicAl { get; set; }
        public double EmHouseRentAl { get; set; }
        public double EmConveyanceAl { get; set; }
        public double EmProjectAl { get; set; }
        public double EmMobileAl { get; set; }
        public double EmNightAl { get; set; }
        public double EmMedicalAl { get; set; }
        public double EmTechnicalAl { get; set; }
        public double EmMealAl { get; set; }
        public double EmTransportAl { get; set; }
        public double EmOtherAl { get; set; }
        public double EmBasicEarning { get; set; }
        public double EmProvidentFundDeduct { get; set; }
        public double EmTdsDeduct { get; set; }
        public double EmSecurityDeduct { get; set; }
        public double EmOthersDeduct { get; set; }
        public double EmBasicDeduction { get; set; }
        public double EmGrossSalary { get; set; }
        public string EmPaymentType { get; set; }
        public int EmPaymentAmountCashPc { get; set; }
        public int EmPaymentAmountBankPc { get; set; }
        public int EmShiftStatus { get; set; }
        public string EmEmpCardNo { get; set; }
        public int EmIsGetHoliday { get; set; }
        public string EmEmpImage { get; set; }
        public string EmUserName { get; set; }
        public int EmProjectId { get; set; }
        public int EmValid { get; set; }
        public DateTime EmEntryDate { get; set; }
        public DateTime EmFirstDate { get; set; }

        public string ProtithanikCode { get; set; }
        public string Tahabil { get; set; }
        public string NID { get; set; }
        public string TAXNumber { get; set; }
        public string JibonBimaNumber { get; set; }


        public double SikkahSohayakAI { get; set; }
        public double OtiriktodayattoAI { get; set; }
        public double motrjanAI { get; set; }

        public double PourokorDeduct { get; set; }
        public double StampDuteiDeduct { get; set; }
        public double rinSudDeduct { get; set; }
        public double HouseRentDeduct { get; set; }
        public double WaterDeduct { get; set; }
        public double grihoNirmanDeduct { get; set; }
        public double SudMuktoDeduct { get; set; }
        public double DakjibonDeduct { get; set; }
        public double MotorsaikelDeduct { get; set; }
        public double ComputerDeduct { get; set; }
        public double VobissoTahabilDeduct { get; set; }
        public double KollanTahabilDeduct { get; set; }
        public double JouthoBimaDeduct { get; set; }
        public double TitasDeduct { get; set; }
        public double MotorgariOgrimDeduct { get; set; }

        public int Grade { get; set; }

        public int Itemid { get; set; }
        public string ItemType { get; set; }
        public double ItemCharge { get; set; }

        public string EmDesignationName { get; set; }

        public int MonthId { get; set; }
        public int AttYear { get; set; }

        public string EmBankBanchName { get; set; }

        public int LoanTypeId { get; set; }
        public int LoanNo { get; set; }
        public double RestLoanAmount { get; set; }
        public double InstallmentSize { get; set; }
        public int LoanAndAdvance { get; set; }

        public string DepartmentName { get; set; }

        public string SalaryCustomId { get; set; }
        public double Amount { get; set; }
        public string SalaryType { get; set; }
        public int SalaryId { get; set; }


        public double SumOfDetection { get; set; }
        public double SumOfEarring { get; set; }
        public double SumOfGet { get; set; }

        public int Id { get; set; }
        public int DayNameId { get; set; }
        public string DayName { get; set; }
        public int ShiftNameId { get; set; }
        public string ShiftName { get; set; }
        public int UserId { get; set; }
        //public im image { get; set; }

        public string GenderName { get; set; }
        public string NationalName { get; set; }
        public string ReligionName { get; set; }
        public string UnitName { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string MonthName { get; set; }
        public string Year { get; set; }
    }
}