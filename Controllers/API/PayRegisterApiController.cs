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
    public class PayRegisterApiController : ApiController
    {
        readonly PayregisterGatway _payregisterGatway = new PayregisterGatway();
        public HttpResponseMessage GetEmployeeId()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _payregisterGatway.GetIdCasCadeDropDown("SELECT Id,Code AS Name From tbl_EMPLOYEE_HR Order By Id"), formatter);
        }

        public HttpResponseMessage GetEmployeeDetailById(string param)
        {

            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _payregisterGatway.GetEmployeeDetailById(param), formatter);

        }
        
        
        public async Task<HttpResponseMessage> Post([FromBody] SalaryCreateModel mSalaryCreateModel)
        {
            var formatter = RequestFormat.JsonFormaterString();
            string msg = "";
            try
            {

                //if (mSalaryCreateModel.MonthId == 0)
                //{
                //    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Month is Empty" }, formatter);
                //}
                //if (mSalaryCreateModel.AttYear == 0)
                //{
                //    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Years is Empty" }, formatter);
                //}

                //else
                //{

                msg = await _payregisterGatway.Save(mSalaryCreateModel);
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new Confirmation { Output = "success", Msg = msg }, formatter);


                //}
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }

        }
	}
}