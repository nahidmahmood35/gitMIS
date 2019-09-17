using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class PatientReleaseModel:AdmissionModel
    {
        public int ReleaseId { get;set; }
        public DateTime ReleaseDate { get;set; }
        public Double TotalDays { get; set; }
        public Double AdvanceAmt { get; set; }
        public Double CollAmt { get; set; }
        public Double TotalServiceCharge { get; set; }
        public Double PayableAmount { get; set; }

    }
}