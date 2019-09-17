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
    public class LabParameterEntryApiController : ApiController
    {
        readonly LabParameterEntryGateway _gt= new LabParameterEntryGateway();

        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] List<LabParameterModel> mLab)
        {
            var formatter = RequestFormat.JsonFormaterString();
            try
            {
                if (_gt.FncSeekRecordNew("tbl_CLINICAL_CHART","Id="+ mLab.ElementAt(0).ItemId +"")==false)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Item Name Is Not Correct" }, formatter);
                }
                else
                {
                    string msg = _gt.FncSeekRecordNew("tbl_LAB_PARAMETER_DEFINITION", "ItemId=" + mLab.ElementAt(0).ItemId + "") ? await _gt.Update(mLab) : await _gt.Save(mLab);
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formatter);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }
        }

        public HttpResponseMessage GetParameterListByItemId(int itemId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetParameterListByItemId(itemId), formatter);
        }

        public HttpResponseMessage GetItemListForLabReport(string invoiceNo)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetItemListForLabReport(invoiceNo), formatter);
        }



        public HttpResponseMessage GetAliasName()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetAliasName(), formatter);
        }
        public HttpResponseMessage GetParameterName()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetParameterName(), formatter);
        }
        public HttpResponseMessage GetReportingGroupName()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetReportingGroupName(), formatter);
        }
        public HttpResponseMessage GetSpecimenName()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetSpecimenName(), formatter);
        }
        public HttpResponseMessage GetUnitName()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetUnitName(), formatter);
        }
        
        
        
        
        
        
        
        
       

    }
}
