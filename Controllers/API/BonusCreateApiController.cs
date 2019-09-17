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
    public class BonusCreateApiController : ApiController
    {
        readonly BonusCreateGetway _gtBonusCreateGetway = new BonusCreateGetway();
        
        //readonly SalaryCreateModel _gSalaryCreateModel = new SalaryCreateModel();
        public async Task<HttpResponseMessage> Post([FromBody] SalaryCreateModel mSalaryCreateModel)
        {
            var formatter = RequestFormat.JsonFormaterString();
            string msg = "";
            try
            {

                if (mSalaryCreateModel.MonthId == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Month is Empty" }, formatter);
                }
                
                if (mSalaryCreateModel.AttYear == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Years is Empty" }, formatter);
                }

                if (_gtBonusCreateGetway.FncSeekRecordNew("tbl_HR_BONUS_REGISTER", " MonthId=" + mSalaryCreateModel.MonthId + " AND AttYear=" + mSalaryCreateModel.AttYear + " "))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Bonus Already create!!" }, formatter);
                }

                else
                {

                    msg = await _gtBonusCreateGetway.Save(mSalaryCreateModel);
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