using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class PharStockInMainModel
    {
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CompanyID { get; set; }
        public string Remarks { get; set; }
        public string UserName { get; set; }
        public string EntryTime { get; set; }
        public int Valid { get; set; }
        public string PNo { get; set; }
        public string SlipNo { get; set; }
        public string RefNo { get; set; }
        public DateTime RefDate { get; set; }
        public DateTime SlipDate { get; set; }
        public string Status { get; set; }


    }
}