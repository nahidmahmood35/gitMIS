using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;
using Microsoft.SqlServer.Server;

namespace HospitalManagementApp_Api.Controllers.API
{
    public class SalaryCreateApiController : ApiController
    {
        readonly SalaryCreateGetway _gtSalaryCreateGetway = new SalaryCreateGetway();
        readonly EmployeeGetway _gtEmployeeGetway = new EmployeeGetway();
        //readonly SalaryCreateModel _gSalaryCreateModel = new SalaryCreateModel();
        public async Task<HttpResponseMessage> Post([FromBody] EmployeeModel mEmployeeModel)
        {
            var formatter = RequestFormat.JsonFormaterString();
            string msg = "";
            try
            {
                bool m, y;
                 m = _gtEmployeeGetway.FncSeekRecordNew("tbl_HR_GLO_PAYREGISTER",
                    "MonthId=" + mEmployeeModel.MonthId + "");
                 y =_gtEmployeeGetway.FncSeekRecordNew("tbl_HR_GLO_PAYREGISTER", "Year=" + mEmployeeModel.AttYear + "");
                
                
                if (m==true && y==true)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "This month of this year salary are already created!!" }, formatter);
                }

                if (mEmployeeModel.MonthId == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Month is Empty" }, formatter);
                }
                if (mEmployeeModel.AttYear == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Years is Empty" }, formatter);
                }
               
                else
                {

                    msg = await _gtSalaryCreateGetway.Save(mEmployeeModel);
                        return Request.CreateResponse(HttpStatusCode.OK,
                            new Confirmation { Output = "success", Msg = msg }, formatter);
             

                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }

        }
        public HttpResponseMessage GetGenderList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Name From tbl_HR_MONTH_INFO Order By Id"), formatter);
        }
        public HttpResponseMessage GetDepartmentNameList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Name From tbl_DEPARTMENT_HR Order By Id"), formatter);
        }
	}
}