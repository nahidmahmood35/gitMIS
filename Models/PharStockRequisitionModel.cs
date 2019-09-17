using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class PharStockRequisitionModel
    {

        public int Id { get; set; }
        public int MasterId { get; set; }
        public string ReqNo { get; set; }
        public string Name { get; set; }
        public DateTime ReqDate { get; set; }
        public string ReqBy { get; set; }
        
        public string UserName { get; set; }
        public string UserDtls { get; set; }
        public int Valid { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public double ReqQty { get; set; }
        public string Allocationby { get; set; }
        public double AllocationQty { get; set; }
        public string ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public double ApproveQty { get; set; }
        public string ApproveTime { get; set; }
        public string DisburseBy { get; set; }
        public DateTime DisburseDate { get; set; }
        public DateTime ExpireDate { get; set; }
        
        public double DisburseQty { get; set; }
        public double BalQty { get; set; }
        public double InQty { get; set; }
        public double PurchasePrice { get; set; }
        public double SalesPrice { get; set; }
        public string DisburseTime { get; set; }
        public string ReqNote { get; set; }
        public string AllocationNote { get; set; }
        public string ApproveNote { get; set; }
        public string DisburseNote { get; set; }
        public string DeptName { get; set; }
        public int DeptId { get; set; }
        public string BarCodeId { get; set; }
        public int CompanyId { get; set; }






    }
}