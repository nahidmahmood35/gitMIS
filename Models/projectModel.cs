using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class ProjectModel
    {
        public int ProjectId { get; set; }
        public string PnoCode { get; set; }
        public string Pno { get; set; }
        public string ContractPerson { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public string SubPno { get; set; }
        public string SubSubPno { get; set; }
        public double ProjectCost { get; set; }
        public int IsShowAccounts { get; set; }
        public int WillShowInHospital { get; set; }
        public int WillShowInPayment { get; set; }
        public double AdmCharge { get; set; }
    }
}