using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;



namespace HospitalManagementApp_Api.Controllers.API
{
    public class StockRequsitionDisburseApiController : ApiController
    {
        readonly StockRequsitionDisburseGateway _gt = new StockRequsitionDisburseGateway();
        public HttpResponseMessage GetInventoryRequisitionListTable(string userName)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetInventoryRequisitionList(userName), formatter);
        }

        public async Task<HttpResponseMessage> Save([FromBody] List<InvStockRequisitionModel> aModels)
        {
            var formate = RequestFormat.JsonFormaterString();
            try
            {
                string msg = await _gt.Update(aModels);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new Confirmation { Output = "success", Msg = msg }, formate);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK
                    , new Confirmation { Output = "error", Msg = ex.ToString() }, formate);
            }
        }
    }
}