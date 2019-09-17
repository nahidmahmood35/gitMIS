using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class AddMenuModel
    {
        public string DepartmentName { get; set; }
        public string MainMenuName { get; set; }
        public string SubMenuName { get; set; }
        public string ControllerName { get; set; }
        public string ViewName { get; set; }
        public int SubMenuSlNo { get; set; }
    }
}