using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{

    public class ComInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ComName { get; set; }
        public string  ComAddress { get; set; }
        public string ComSlogan { get; set; }
        public string NLogoImage { get; set; }
        public byte[] LogoImage { get; set; }
    }
}