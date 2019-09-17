using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class OpdModel:InvoiceModel
    {
        public int OpdId { get; set; }
        public string DeptName { get; set; }
        public string RoomNo { get; set; }

    }
}