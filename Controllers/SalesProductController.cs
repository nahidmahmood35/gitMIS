using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalManagementApp_Api.Models;
using HospitalManagementApp_Api.Gateway;

namespace HospitalManagementApp_Api.Controllers
{
    public class SalesProductController : Controller
    {
        //
        readonly SalesProductGateway _chartGateway = new SalesProductGateway();
        public ActionResult Index()
        {
            return View();
        }
	}
}