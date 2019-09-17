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
    public class IndoorProcedureEntryApiController : ApiController
    {
        readonly IndoorProcedureEntryGateway _gt = new IndoorProcedureEntryGateway();
       
        public  async Task<HttpResponseMessage> Post([FromBody] List<AdmissionModel> aModel)
        {

            var formate = RequestFormat.JsonFormaterString();
            try
            {
                if (aModel.ElementAt(0).PtIndoorId==0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,new Confirmation { Output = "error", Msg = "Name is Null" });
                }
                else
                {
                    string msg = await _gt.Save(aModel);
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formate);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,new Confirmation { Output = "error", Msg=ex.ToString() }, formate);
            }
        }
        
        public HttpResponseMessage GetReprintProcedureEntry(string refNo)
        {
            _gt.PrintReport("IndoorProcedureEntryRPT.rpt", "GET_IN_PROCEDURE_ENTRY_LIST", " WHERE RefNo='" + refNo + "'", "GET_IN_PROCEDURE_ENTRY_LIST", "", "", "V");
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, "Success", formatter);
        }
        public HttpResponseMessage GetProcedureEntryList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetProcedureEntryList(), formatter);
        }
      

    }
}
