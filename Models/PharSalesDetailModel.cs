using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class PharSalesDetailModel
    {
        public string MainDeptName { get; set; }
        public string SaleNo { get; set; }
        public DateTime SaleDate { get; set; }
        public string CompanyID { get; set; }
        public string ProductName { get; set; }
        public double ProductQty { get; set; }
        public double SalePrice { get; set; }
        public int Valid { get; set; }
        public string UserName { get; set; }
        public string PNo { get; set; }
        public double AvgPurchasePrice { get; set; }
        public string CollectionFrom { get; set; }
        public string RefDRCode { get; set; }
        public DateTime ExpireDate { get; set; }
        public int Package { get; set; }
        public string PackCode { get; set; }



    }
}