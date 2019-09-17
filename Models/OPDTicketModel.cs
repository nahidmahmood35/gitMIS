using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class OpdTicketModel
    {
        public string TicketNo { get; set; }
        public DateTime TicketDate { get; set; }
        public double Charge { get; set; }
        public string PtRegNo { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime Dob { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public string Telephone { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public int Valid { get; set; }         // Data type Bit
        public string UserName { get; set; }
        public string DeptName { get; set; }
        public string EntryTime { get; set; }
        public string RoomNo { get; set; }
        public string TokenNo { get; set; }
        public string DrType { get; set; }
        public string DrCode { get; set; }
        public string DrName { get; set; }

    }
}