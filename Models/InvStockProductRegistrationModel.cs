using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class InvStockProductRegistrationModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int  ProductCategory { get; set; }
        public double UnitPrice { get; set; }
        public string Unit { get; set; }
        public int Valid { get; set; }
        public string UserName { get; set; }
        public string rackNumber { get; set; }
        public string cellNumber { get; set; }

        public string ProductCategoryName{ get; set; }

        public int DepreciationMethodId { get; set; }
        public double DepreciationAmount { get; set; }
        public int DepreciationAmountType { get; set; }
        public int AssetType { get; set; }
        public double MinimumQuantity { get; set; }
        public double EconomicQuantity { get; set; }
    }
}