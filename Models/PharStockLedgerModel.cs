using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class PharStockLedgerModel
    {
        public string RefNo { get; set; }
        public DateTime TranDate { get; set; }
        public string CompanyID { get; set; }
        public string ProductName { get; set; }
        public double InQty { get; set; }
        public double OutQty { get; set; }
        public double RtnQty { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public int Valid { get; set; }
        public int RowId { get; set; }
        public string PNO { get; set; }
        public double ShortQty { get; set; }
        public double ExcessQty { get; set; }
        public double DamageQty { get; set; }
        public double SampleQty { get; set; }
        public double BonusQty { get; set; }
        public string SlipNo { get; set; }
        public string SaleNo { get; set; }
        public string PrevRefNo { get; set; }
        public DateTime PrevRefDate { get; set; }
        public string VoucherType { get; set; }
        public string StoreName { get; set; }
        public string PatientId { get; set; }
        public double TP { get; set; }
        public DateTime ExpireDate { get; set; }
        public string BatchNo { get; set; }
        public string TransferTo { get; set; }
        public string RefDRCode { get; set; }
        public int Package { get; set; }
        public string PackCode { get; set; }

    }
}