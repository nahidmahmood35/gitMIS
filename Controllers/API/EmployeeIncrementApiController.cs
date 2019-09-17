using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;
using Microsoft.SqlServer.Server;

namespace HospitalManagementApp_Api.Controllers.API
{
    public class EmployeeIncrementApiController : ApiController
    {
        readonly EmployeeIncrementGateway _gtEmployeeIncrementGateway = new EmployeeIncrementGateway();

        public async Task<HttpResponseMessage> Post([FromBody] List<EmployeeModel> mEmployeeModel)
        {

            var formatter = RequestFormat.JsonFormaterString();
            string msg = "";
            try
            {
                if (string.IsNullOrEmpty(mEmployeeModel.ElementAt(0).MonthName))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Month is Empty" }, formatter);
                }
                if (string.IsNullOrEmpty(mEmployeeModel.ElementAt(0).Year))
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new Confirmation { Output = "error", Msg = "Year is Empty" }, formatter);
                }
                else
                {


                    if (_gtEmployeeIncrementGateway.FncSeekRecordNew("tbl_HR_Increment", " EmpId='" + mEmployeeModel.ElementAt(0).EmId + "' AND Year=" + mEmployeeModel.ElementAt(0).Year + " AND Month='" + mEmployeeModel.ElementAt(0).MonthName + "'"))
                    {
                        //msg = await _gtEmployeeGetway.Update(mEmployeeModel);
                        //return Request.CreateResponse(HttpStatusCode.OK,
                        //    new Confirmation { Output = "success", Msg = msg }, formatter);
                        return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Employee already Increment" }, formatter);
                    }
                    else
                    {

                        msg = await _gtEmployeeIncrementGateway.Save(mEmployeeModel);
                        return Request.CreateResponse(HttpStatusCode.OK,
                            new Confirmation { Output = "success", Msg = msg }, formatter);
                        //return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "Write", Msg = "OK" }, formatter);
                    }

                }
                
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }
        }
	}
}