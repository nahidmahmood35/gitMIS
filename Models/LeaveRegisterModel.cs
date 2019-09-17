using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class LeaveRegisterModel
    {
               public int PurposeId { get; set; }
               public string EmCode { get; set; }
               public int LeaveTypeId { get; set; }
               public DateTime LeaveFrom { get; set; }
               public string Remark { get; set; }
               public int Day { get; set; }
               public DateTime EntryDay { get; set; }
    }
}