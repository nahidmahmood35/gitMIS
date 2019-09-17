using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class PharProductInfoModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int GenericId { get; set; }
        public string GenericName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int PackSize { get; set; }
        public string ProductUnit { get; set; }
        public double Tp { get; set; }
        public double SalesPrice { get; set; }
        public int ReminderStock { get; set; }
        public int GroupId { get; set; }
        public int SubGroupId { get; set; }
        public string GroupName { get; set; }
        public string SubGroupName { get; set; }
        public string RackNo { get; set; }
        public int RowNo { get; set; }
        public string UserName { get; set; }
        public DateTime EntryDate { get; set; }
        public int BranchId { get; set; }
        public string EntryTime { get; set; }
        public double Quantity { get; set; }
        public double BonusQty { get; set; }
        public double ReqQty { get; set; }
        public double BalQty { get; set; }
        public string Status { get; set; }
        public int Valid { get; set; }
        public DateTime ExpireDate { get; set; }

    }
}