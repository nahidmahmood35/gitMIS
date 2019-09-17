using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class PharPurchaseModel:PharCompanyModel
    {
        public string TrNo { get; set; }
        public DateTime TrDate { get; set; }
        public string InvoiceNo { get; set; }
        public int InvoiceMasterId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double PurchaseAmount { get; set; }
        public double LessAmount { get; set; }
        public double PaymentAmount { get; set; }
        public double DueAmount { get; set; }
        public int Pno { get; set; }
        public int PaymentStatus { get; set; }
        public string BarCodeId { get; set; }
        public double InvPrice { get; set; }
        public double TotalTp { get; set; }
        public double VatAmt { get; set; }
        public double DiscountAmt { get; set; }       
        public double PurchasePrice { get; set; }
        public double TotalPrice { get; set; }
        public string Remarks { get; set; }
        public int SubSubPnoId { get; set; }
        public string SlipNo { get; set; }
        public DateTime  SlipDate { get; set; }
        public double Spd { get; set; }
        public double SpdAmt { get; set; }
         
    }
}