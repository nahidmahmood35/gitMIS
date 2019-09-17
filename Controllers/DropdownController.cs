using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Controllers
{
    public class DropdownController : Controller
    {
        readonly DepartmentGateway _departmentGateway=new DepartmentGateway();
        readonly MarketRepresentativeGateway _mrGateway=new MarketRepresentativeGateway();
        readonly DoctorTypeGateway _doctorTypeGateway=new DoctorTypeGateway();
        #region Department
                public JsonResult GetDepartmentList(int id)
                {
                    var data = _departmentGateway.GetDepartmentList(id);
                    return Json(data,JsonRequestBehavior.AllowGet);
                }
                public JsonResult SaveDepartment(IdNameForDropdownModel aModel)
                {
                    var data = _departmentGateway.Save(aModel);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                public JsonResult UpdateDepartment(IdNameForDropdownModel aModel)
                {
                    var data =_departmentGateway.Update(aModel);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                public JsonResult DeleteDepartment(int id)
                {
                    var data=_departmentGateway.Delete(id);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
        #endregion
        #region MR
                public JsonResult GetMarketRepresentativeList(int id)
                {
                    var data=_mrGateway.GetMarketRepresentativeList(id);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                public JsonResult SaveMr( IdNameForDropdownModel aModel)
                {
                    var data= _mrGateway.Save(aModel);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                public JsonResult UpdateMr(IdNameForDropdownModel aModel)
                {
                    var data=_mrGateway.Update(aModel);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                public JsonResult DeleteMr([FromBody] int id)
                {
                    var data = _mrGateway.Delete(id);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
        #endregion
        #region DoctorType
                public JsonResult GetDocotorTypeList(int id)
                {
                    var data = _doctorTypeGateway.GetDoctorTypeList(id);
                    return Json(data,JsonRequestBehavior.AllowGet);
                }
                public JsonResult SaveDrType( IdNameForDropdownModel aModel)
                {
                    var data=_doctorTypeGateway.Save(aModel);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                public JsonResult UpdateDrType(IdNameForDropdownModel aModel)
                {
                    var data = _doctorTypeGateway.Update(aModel);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                public JsonResult DeleteDrType(int id)
                {
                    var data=_doctorTypeGateway.Delete(id);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                #endregion

    }
}













