using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class IdNameForDropdownModel
    {
        public int Id { get; set; }
        public int CatId { get; set; }
        public string Name { get; set; }
        public string AnotherName { get; set; }
        public int Qty { get; set; }
    }
}