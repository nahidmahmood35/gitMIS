using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class DoctorModel:PatientModel
    {
            public int ConsultantId { get; set; }
            public int DrId { get; set; }
            public string Code { get; set; }
            public string Name{ get; set; }
            public string  Category{ get; set; }
            public string Gender { get; set; }
            public string Degree { get; set; }
            public string Type { get; set; }
            public string Speciality { get; set; }
            public int DepartmentId { get; set; }
            public string PresentAddress { get; set; }
            public string Area { get; set; }
            public string Thana { get; set; }
            public string Zilla { get; set; }
            public string MobileNo { get; set; }
            public string Email { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string SpouseName { get; set; }
            public DateTime SpouseDob { get; set; }
            public string RefType { get; set; }
            public DateTime MarriageDate { get; set; }
            public string Availability { get; set; }
            public int TakeComisionId { get; set; }
            public int MioId { get; set; }
            public int PrescriptionStatus { get; set; }
            public string NameBangla { get; set; }
            public string Details { get; set; }
            public double VisitNewPatient { get; set; }
            public double VisitOldPatient { get; set; }
            public int NoOfPatientPerDay { get; set; }

            // new
            public string RefFeePcOrTk { get; set; }  
            public string EntryTime { get; set; }
            public double PnoCharge { get; set; }
            public int PnoId { get; set; }
    }
}