using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class SalesProductModel
    {

        public int IdNo { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public double UnitPrice { get; set; }
        public string FinishedPdCode { get; set; }
        public string SalesType { get; set; }
        public string Unit { get; set; }
        public string PNO { get; set; }
        public double Valid { get; set; }
        public string UserName { get; set; }
        public double ReminderStock { get; set; }
        public double Status { get; set; }
        public double MRP { get; set; }
        public double TP { get; set; }
        public string Type { get; set; }
        public string RackName { get; set; }
        public int RowNumber { get; set; }
        public string StoreName { get; set; }
        public string SubSubPNO { get; set; }
        public string CostCenter { get; set; }
        public double LastPurchasePrice { get; set; }
        public DateTime LastPurDate { get; set; }
        public int GCMId { get; set; }
        public int ProductCategoryId { get; set; }
         public int RackId { get; set; }
         public int RowId { get; set; }
         public int StoreId { get; set; }
         public int SubSubPNOId { get; set; }
       

         
        



    }
}