using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Controllers.API
{
    public class BedApiController : ApiController
    {
        readonly BedInfoGateway _gt = new BedInfoGateway();
        readonly DropDownGateway _gtdropDownGateway = new DropDownGateway();
        public async Task<HttpResponseMessage> Post([FromBody] BedModel aModel)
        {

            var formate = RequestFormat.JsonFormaterString();
            try
            {
                if (string.IsNullOrEmpty(aModel.Description))
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new Confirmation { Output = "error", Msg = "Name is Null" });
                }
                else
                {
                    if (_gt.FncSeekRecordNew("tbl_IN_BED_INFO", "Id=" + aModel.BedId + "") == false)
                    {
                        if (_gt.FncSeekRecordNew("tbl_IN_BED_INFO", "Description='" + aModel.Description + "'") == true)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK,new Confirmation { Output = "error", Msg = "Name Already Exist" }, formate);
                        }
                        else
                        {
                            string msg = await _gt.Save(aModel);
                            return Request.CreateResponse(HttpStatusCode.OK,new Confirmation { Output = "success", Msg = msg }, formate);
                        }
                    }
                    else
                    {
                        string msg = await _gt.Update(aModel);
                        return Request.CreateResponse(HttpStatusCode.OK,new Confirmation { Output = "success", Msg = msg }, formate);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,new Confirmation { Output = "error", Msg=ex.ToString() }, formate);
            }
        }
        public HttpResponseMessage GetSaveBedType(string name)
        {
            var formatter = RequestFormat.JsonFormaterString();
            if (_gtdropDownGateway.FncSeekRecordNew("tbl_IN_TYPES_OF_BED", "Name='" + name + "'"))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "This Name Already Exist", formatter);
            }
            return Request.CreateResponse(HttpStatusCode.OK, _gtdropDownGateway.SaveWithReturnId("tbl_IN_TYPES_OF_BED", name), formatter);
        }
        public HttpResponseMessage GetBedTypeList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetIdNameForDropDownBox("SELECT Id,Name From tbl_IN_TYPES_OF_BED Order by Id"), formatter);
        }
        public HttpResponseMessage GetAvailabeBedList(string searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetAvailabeBedList(searchString), formatter);
        }
        public HttpResponseMessage GetAllBedList(string searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetAllBedList(searchString), formatter);
        }

        public HttpResponseMessage GetDelete(int id)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.DeleteInsert("DELETE FROM tbl_IN_BED_INFO WHERE Id=" + id + ""), formatter);
        }

        public HttpResponseMessage GetBedInfoFromLedgerCountFromLedgerByPtId(int ptId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetBedInfoFromLedgerCountFromLedgerByPtId(ptId), formatter);
        }

    }
}
