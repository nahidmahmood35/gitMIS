using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class VaqModel
    {
        public int VaqId { get; set; }
        public int ItemId { get; set; }
        public string VaqGroup { get; set; }
        public string VaqName { get; set; }
        public double VaqCharge { get; set; }
    }
}