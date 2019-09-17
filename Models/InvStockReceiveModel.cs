using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class InvStockReceiveModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int DepartmentId { get; set; }
        public string Remarks { get; set; }
        public double TotalPrice { get; set; }
        public string SlipNo { get; set; }
        public DateTime SlipDate { get; set; }
        public int ItemId { get; set; }
        public string ProductName { get; set; }
        //public string productName { get; set; }
        public double quantity { get; set; }
        public double totalPricePerProduct { get; set; }
        public double pricePerProduct { get; set; }
        public DateTime expireDate { get; set; }
        public string rackNo { get; set; }
        public string callNo { get; set; }
        public string UserName { get; set; }
        public double LessAmount { get; set; }
        public double VatAmount { get; set; }
        public double PaymentAmount { get; set; }
        public string PoNo { get; set; }
        public string TrNo { get; set; }
        public DateTime TrDate { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string EntryDate { get; set; }
        public int BranchId { get; set; }
        public string EntryTime { get; set; }
        public string Status { get; set; }
        public int PnoId { get; set; }
        public string PnoName { get; set; }
        public int Valid { get; set; }
        public string Unit { get; set; }

        public double vatPerItem { get; set; }
        public double lessPerItem { get; set; }
        public double pricePerProductWithVatLess { get; set; }
    }
}