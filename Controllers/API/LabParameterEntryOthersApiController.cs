using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
//using System.Web.Mvc;
using System.Web.Http.Cors;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Controllers.API
{
    public class LabParameterEntryOthersApiController : ApiController
    {
        readonly LabParameterEntryGateway _gt= new LabParameterEntryGateway();

        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] LabParameterModel mLab)
        {
            var formatter = RequestFormat.JsonFormaterString();
            try
            {
                if (_gt.FncSeekRecordNew("tbl_CLINICAL_CHART","Id="+ mLab.ItemId +"")==false)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Item Name Is Not Correct" }, formatter);
                }
                else
                {
                    string msg = _gt.FncSeekRecordNew("tbl_LAB_PARAMETER_DEFINITION_FOR_OTHERS", "ItemId=" + mLab.ItemId + "") ? await _gt.UpdateOther(mLab) : await _gt.SaveOther(mLab);
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formatter);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }
        }

       


        public HttpResponseMessage GetParameterByItemId(int itemId)
        {
            var formatter = RequestFormat.JsonFormaterString();

            if (_gt.FncSeekRecordNew("tbl_LAB_PARAMETER_DEFINITION_FOR_OTHERS","ItemId="+ itemId +"")==false)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "There Have No Default Result With This Item" }, formatter);
            }
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetParameterByItemId(itemId), formatter);
        }



        public HttpResponseMessage GetReportFileName()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetIdNameForDropDownBox("SELECT Distinct 0 as Id, ISNULL(ReportFileName,'N/A') AS Name FROM tbl_LAB_PARAMETER_DEFINITION"), formatter);
        }
        
        
        
        
        
       

    }
}
