using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class BedModel:InvoiceModel
    {
        public int BedId { get; set; }
        public string BedNo { get; set; }
        public string RoomNo { get; set; }
        public string  FloorNo { get; set; }
        public int TypeOfBedId { get; set; }
        public string TypeOfBedName { get; set; }
        public string DeptName { get; set; }
        public double ServiceChargePc { get; set; }
        public string ServiceChargePcOrTk { get; set; }
        public int BedStatus { get; set; }
        public double BedCharge { get; set; }
        public double NoOfCount { get; set; }
        public double AdmissionCharge { get; set; }
    }
}