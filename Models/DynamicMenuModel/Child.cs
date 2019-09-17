

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models.DynamicMenuModel
{
    public class Child
    {
        public string DeptName { get; set; }
        public string Name { get; set; }
        public string ParentNode { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string ChildNode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int  CanGiveAitsoDiscount { get; set; }
        public string UserImageString { get; set; }
        public string UserSignString { get; set; }
        public byte[] UserSignByte { get; set; }


    }
}