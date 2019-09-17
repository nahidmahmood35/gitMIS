using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class PharCompanyModel:PharProductInfoModel
    {

       // public int CompanyId { get; set; }
        public string ComName { get; set; }
        public string ComCode { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public double OpeningBal { get; set; }
        public int ComStatus { get; set; }
        public DateTime ComEntryDate { get; set; }



        
    }


















}