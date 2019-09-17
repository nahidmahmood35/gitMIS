using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class LoanAndAdvanceModel : EmployeeModel
    {
     
        public string Name { get; set; }
        
        public string DesignationName { get; set; }
        public double GrossSalary { get; set; }
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }
        public string Status { get; set; }
        public int LoanAmound { get; set; }
        public DateTime Date { get; set; }
        public int LoanId { get; set; }
        public string Note { get; set; }
        public int LoanType { get; set; }
        public double InstallmentAmound { get; set; }
        public int SequentialStatuss { get; set; }
        public int MonthId { get; set; }
        public int Year { get; set; }
        public int LoanAndAdvance { get; set; }
        public string SalaryNameBD { get; set; }
        public double PaidAmount { get; set; }
       


    }
}