using System.Drawing;
using System.IO;
using System.Text;

using System;
using HospitalManagementApp_Api.Models;
using HospitalManagementApp_Api.Gateway;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalManagementApp_Api.Models.DynamicMenuModel;


namespace HospitalManagementApp_Api.Controllers
{
    public class LoginController : Controller
    {
        readonly DynamicMenuGateway _aGateway = new DynamicMenuGateway();
        public ActionResult Index()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult Index(ComInfo aComInfo)
        {
            var aChild=new Child();
            int foundStatus = _aGateway.IsExistUserNamePassword(aComInfo);
            if (foundStatus != 0)
            {
                Session["UserImage"] = _aGateway.GetUserImageByUserName(aComInfo.UserName);
                Session["BranchId"] = _aGateway.ReturnFieldValue("tbl_USER_BRANCH_INFO", "UserName='" + aComInfo.UserName + "'", "BranchId");
                Session["UserName"] = aComInfo.UserName;
                
                Session["allParent"] = _aGateway.GetParentNode();
                Session["allChild"] = _aGateway.ChildNode();
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ViewBag.msg = "Invalid UserName Or Password";
                return View();
            }
        }
	}
}