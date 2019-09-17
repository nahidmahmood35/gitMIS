using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class InvoiceModel:DoctorModel
    {

        public DateTime TrDate { get; set; }
        public string TrNo { get; set; }
        public int InvMasterId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double TotalAmount { get; set; }
        public double LessPc { get; set; }
        public string LessPcOrTk { get; set; }
        public string LessFrom { get; set; }
        public double LessAmount { get; set; }
        public double ReceiveAmount { get; set; }
        public double ReturnAmount { get; set; }
        public string Remarks { get; set; }
        public string MainDeptName { get; set; }
        public string SubDeptName { get; set; }
        public string PatientStatus { get; set; }
        public int CorporateId { get; set; }
        public int PackageId { get; set; }
        public double CashAmount { get; set; }
        public double CardAmount { get; set; }
        public string CardNumber { get; set; }
        public int CardBankId { get; set; }
        public double CheaqueAmount { get; set; }
        public string CheaqueNumber { get; set; }
        public int CheaqueBankId { get; set; }
        public int IsUrgent { get; set; }
        public int IndoorSeviceDrId { get; set; }
        public double DueAmt { get; set; }
        public double BalAmt { get; set; }
        public int DeptId { get; set; }
        public double AdjustAmount { get; set; }
        public string DrName { get; set; }
        public string BedNo { get; set; }
    }
}