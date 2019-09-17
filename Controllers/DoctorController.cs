using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Controllers

{
    public class DoctorController : Controller
    {       
        public ActionResult Index()
        {
           
            return View();
        }
	}
}