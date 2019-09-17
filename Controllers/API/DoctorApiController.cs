using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Controllers.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")] 
    public class DoctorApiController : ApiController
    {
        readonly DoctorGateway _dGateway = new DoctorGateway();
        readonly DropDownGateway _gtdropDownGateway = new DropDownGateway();
        public async Task<HttpResponseMessage> Post([FromBody] List<DoctorModel> mDoctor)
        {
            try
            {
                string msg = "";
                var formatter = RequestFormat.JsonFormaterString();
                if (string.IsNullOrEmpty(mDoctor.ElementAt(0).Name)) { return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Name is Empty" }, formatter); }
                if (string.IsNullOrEmpty(mDoctor.ElementAt(0).Code)) { return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Code is Empty" }, formatter); }
                if (string.IsNullOrEmpty(mDoctor.ElementAt(0).UserName)) { return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "UserName is Empty" }, formatter); }

                else
                {
                    if (_dGateway.FncSeekRecordNew("tbl_DOCTOR_INFO", "Id=" + mDoctor.ElementAt(0).DrId + "") == false)
                    {

                        if (_dGateway.FncSeekRecordNew("tbl_DOCTOR_INFO", "Code='" + mDoctor.ElementAt(0).Code + "'") == false)
                        {
                            msg = await _dGateway.Save(mDoctor);
                        }
                        else
                        {
                            msg = "This Code Has Already Exist";
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formatter);
                    }
                    else
                    {
                        msg = await _dGateway.Update(mDoctor);                     
                        return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formatter);
                    }
                }
            }
            catch (Exception ex)
            {
                var formatter = RequestFormat.JsonFormaterString();
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }
        }

        public HttpResponseMessage GetOutdoorDepartmentList(string userName)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _dGateway.GetIdNameForDropDownBox("SELECT Id,Name FROm tbl_DEPARTMENT_FOR_OUTDOOR_MST WHERE BranchId="+ _dGateway.GetBranchIdByuserName(userName) +" Order By Id"),formatter);
        }

        public HttpResponseMessage GetGenderList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _dGateway.GetIdNameForDropDownBox("SELECT Id,Name FROm tbl_GENDER_INFO_MST Order By Id"), formatter);
        }

        public HttpResponseMessage GetCategoryList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _dGateway.GetIdNameForDropDownBox("SELECT Id,Name FROm tbl_DOCTOR_CATEGORY_MST Order By Id"), formatter);
        }

        public HttpResponseMessage GetMioList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _dGateway.GetIdNameForDropDownBox("SELECT Id,Name FROm tbl_MIO_INFO_MST Order By Id"), formatter);
        }

        public HttpResponseMessage GetDoctorList(string searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _dGateway.GetDoctorList(searchString), formatter);
        }

        public HttpResponseMessage GetSaveDepartment(string name)
        {
            var formatter = RequestFormat.JsonFormaterString();
            if (_gtdropDownGateway.FncSeekRecordNew("tbl_DEPARTMENT_FOR_OUTDOOR_MST", "Name='" + name + "'"))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "This Name Already Exist", formatter);
            }
            return Request.CreateResponse(HttpStatusCode.OK, _gtdropDownGateway.SaveWithReturnId("tbl_DEPARTMENT_FOR_OUTDOOR_MST", name), formatter);
        }

        public HttpResponseMessage GetSaveMrName(string name)
        {
            var formatter = RequestFormat.JsonFormaterString();
            if (_gtdropDownGateway.FncSeekRecordNew("tbl_MIO_INFO_MST", "Name='" + name + "'"))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "This Name Already Exist", formatter);
            }
            return Request.CreateResponse(HttpStatusCode.OK, _gtdropDownGateway.SaveWithReturnId("tbl_MIO_INFO_MST", name), formatter);
        }
        public HttpResponseMessage GetDeleteById(int id)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _dGateway.DeleteInsert("DELETE FROM tbl_DOCTOR_INFO WHERE Id=" + id + ""), formatter);
        }
    }
}
