using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using HospitalManagementApp_Api.Gateway;

namespace HospitalManagementApp_Api.Controllers
{
    public class DiagnosisInvoiceController : Controller
    {
        InvoiceGateway _gtInvoice = new InvoiceGateway();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult InvoiceList()
        {
            return View();
        }
        public JsonResult GetInvoiceList(string dateFrom, string dateTo)
        {
            var data = _gtInvoice.GetInvoiceList(dateFrom, dateTo);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }


    }
}