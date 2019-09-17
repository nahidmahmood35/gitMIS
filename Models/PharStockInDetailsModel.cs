using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class PharStockInDetailsModel
    {
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CompanyId { get; set; }
        public string ProductName { get; set; }
        public int ProductUnit { get; set; }
        public double ProductQty { get; set; }
        public double InvPrice { get; set; }
        public int Valid { get; set; }
        public int Row_Id { get; set; }
        public string PNo { get; set; }
        public string SlipNo { get; set; }
        public DateTime ExpireDate { get; set; }
        public double TotalTP { get; set; }
        public double VatAmt { get; set; }
        public double DiscountAmt { get; set; }
        public string VoucherType { get; set; }
        public string StoreName { get; set; }
        public string RefNo { get; set; }
        public DateTime RefDate { get; set; }
        public double SalesPrice { get; set; }
        public double BonusQty { get; set; }

    }
}