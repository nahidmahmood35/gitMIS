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
    public class InventoryStockDirectDisburseApiController : ApiController
    {
        readonly InventoryStockDirectDisburseGateway _gt = new InventoryStockDirectDisburseGateway();
        public HttpResponseMessage GetProductName(string NameOfProduct, int dipId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetProductList(NameOfProduct, dipId), formatter);
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
        public HttpResponseMessage GetRequisitionListTable(string userName)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetRequisitionList(userName), formatter);
        }

        public HttpResponseMessage GetDeleteRefNo(string reqNumber)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetDeleteRefNo(reqNumber), formatter);
        }
    }
}