using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Controllers
{
    public class PatientRegistrationController : Controller
    {
        readonly PatientGateway _aPatientGateway = new PatientGateway();
        public ActionResult Index()
        {
            return View();
        }
       
        public JsonResult GetCurrentAge(DateTime dateOfBirth)
        {
            var data = _aPatientGateway.GetCurrentAgeOfaPatientForField(dateOfBirth.AddDays(-1).ToString("yyyy-MM-dd"));
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDateOfBirth(int day,int month,int years)
        {
            years = years*365;
            month = month*30;
            int days = years + month + day;
            string  data = DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd");
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}