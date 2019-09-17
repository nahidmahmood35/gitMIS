using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Controllers
{
    public class AddMenuController : Controller
    {
        readonly AddMenuGateway _gateway=new AddMenuGateway();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Save(AddMenuModel aModel)
        {
            string msg = _gateway.Save(aModel);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult  GetList(string param)
        {
            var data = _gateway.GetList(param);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}