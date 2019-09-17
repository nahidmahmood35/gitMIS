using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class AdmissionModel:BedModel
    {
        public string PatientId { get; set; }
        public DateTime AdmissionDate { get; set; }
        public string AdmissionTime { get; set; }
        public double ContractAmount { get; set; }
        public string RName1 { get; set; }
        public string Relation1 { get; set; }
        public string RPhone1 { get; set; }
        public string RName2 { get; set; }
        public string Relation2 { get; set; }
        public string RPhone2 { get; set; }
       
        public int RefDrId { get; set; }
        public int UnderDrId { get; set; }
        public int AdmitDrId { get; set; }

        public string RefDrName { get; set; }
        public string UnderDrName { get; set; }
        public string AdmitDrName { get; set; }
        public int Id { get; set; }

    }
}