using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalManagementApp_Api.Gateway;

namespace HospitalManagementApp_Api.Controllers
{
    public class EmployeeGlobalController : Controller
    {
        readonly EmployeeGetway _gtEmployeeGetway = new EmployeeGetway();
        //
        // GET: /EmployeeGlobal/
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult SaveImageAndSignature(string EmCode, string imageString)
        {
            string msg = _gtEmployeeGetway.SaveImageAndSignature(EmCode, imageString);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
	}
}