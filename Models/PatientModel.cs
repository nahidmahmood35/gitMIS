using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class PatientModel:ClinicalChartModel
    {
        public int PtRegId { get; set; }
        public string PtRegNo { get; set; }
        public int PtIndoorId { get; set; }
        public string PtName { get; set; }
        public DateTime PtDob { get; set; }
        public string PtAgeYear { get; set; }
        public string PtAgeMon { get; set; }
        public string PtAgeDay { get; set; }
        public string PtAgeDetail { get; set; }
        public int PtGendeId { get; set; }
        public string PtGenderName { get; set; }
        public string PtFatherName { get; set; }
        public string PtMotherName { get; set; }
        public string PtSpooseName { get; set; }
        public string PtAddress { get; set; }
        public string PtMobileNo { get; set; }
        public string PtOccupation { get; set; }

        public int PtBloodGroupId { get; set; }
        public string  PtBloodGroupName { get; set; }
        public int PtReligionId { get; set; }
        public string PtReligionName { get; set; }
        public string PtNationalityName { get; set; }
        public string PtNationalIdNo { get; set; }
        public string PtPassportNo { get; set; }
        public string PtUserName { get; set; }
        public DateTime PtRegDate { get; set; }
        public int PtIntroducerId { get; set; }
        public string PtIntroducerName { get; set; }
        public string  PtArea { get; set; }
      
    }
}