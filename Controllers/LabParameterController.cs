using Rotativa.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalManagementApp_Api.Gateway;


namespace HospitalManagementApp_Api.Controllers
{
    public class LabParameterController : Controller
    {

        LabDataGateway _gt = new LabDataGateway();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MakePdf(int itemId,int invMasterId)
        {
            ViewBag.data = _gt.GetLabdata(itemId, invMasterId);

            return View();
        }

        public ActionResult GenteratePdf(int itemId, int invMasterId)
        {

            return new ActionAsPdf("MakePdf", new { itemId = itemId, invMasterId = invMasterId });
        }

       
	}
}