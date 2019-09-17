using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class RosterViewModel
    {
        public int DayName { get; set; }
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
    }
}