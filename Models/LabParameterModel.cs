using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class LabParameterModel:InvoiceModel
    {
        public string ItemName { get; set; }
        public string Specimen { get; set; }
        public string AliasName { get; set; }
        public string ParameterName { get; set; }
        public string Result { get; set; }
        public string Unit { get; set; }
        public string NormalValue { get; set; }
        public string GroupName { get; set; }
        public int GroupSlNo { get; set; }
        public int ItemSlNo { get; set; }
        public string ParamViewType { get; set; }

        
        public int ReportDrId { get; set; }
        public int LabInchargeId { get; set; }
        public int CheckedById { get; set; }
        public string ReportDrDetails { get; set; }
        public string LabInchargeDetails { get; set; }
        public string CheckedByDetails { get; set; }
        public string InvTime { get; set; }
        

    }
}