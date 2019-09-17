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
    public class BonusRegisterApiController : ApiController
    {
        readonly BonusRegisterGetway _BonusRegisterGetway = new BonusRegisterGetway();
        public HttpResponseMessage GetEmployeeDetailById(string param)
        {

            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _BonusRegisterGetway.GetEmployeeDetailById(param), formatter);

        }

        public HttpResponseMessage GetEmployeeId()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _BonusRegisterGetway.GetIdCasCadeDropDown("SELECT Id,EmpId AS Name From tbl_HR_BONUS_REGISTER Order By Id"), formatter);
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
                if (mSalaryCreateModel.PrepareBonus == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Bonus Percentage is Empty" }, formatter);
                }

                else
                {

                    msg = await _BonusRegisterGetway.Save(mSalaryCreateModel);
                return Request.CreateResponse(HttpStatusCode.OK,
                    new Confirmation { Output = "success", Msg = msg }, formatter);


                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }

        }
	}
}