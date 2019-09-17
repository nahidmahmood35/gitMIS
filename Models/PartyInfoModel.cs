using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HospitalManagementApp_Api.Gateway.DB_Helper;

namespace HospitalManagementApp_Api.Models
{
    public class PartyInfoModel 
    {
        public int IdNo { get; set; }
        public string PartyId { get; set; }
        public string PartyName { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public double OpeningBal { get; set; }
        public DateTime TrDate { get; set; }
        public string UserName { get; set; }
        public string EntryTime { get; set; }
        public double ShowPC { get; set; }
        public string PStatus { get; set; }
        public string VendorFor { get; set; }
        public double Ratio { get; set; }





    }
}