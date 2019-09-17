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
    public class InventoryStockRequsitionReqApiController : ApiController
    {
        InventoryStockRequsitionReqGateway _gtInventoryStockRequsitionReqGateway = new InventoryStockRequsitionReqGateway();
        
        public HttpResponseMessage GetProductName(string searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtInventoryStockRequsitionReqGateway.GetProductList(searchString), formatter);
        }


        public HttpResponseMessage GetDepartmentList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtInventoryStockRequsitionReqGateway.GetIdNameForDropDownBox("Select IdNo as Id, SubsubPNo AS Name from project where MainPNO='Inventory' AND Pno='Inventory' AND SubPNo='Inventory' order by IdNo"), formatter);
        }

        public async Task<HttpResponseMessage> Save([FromBody] List<InvStockRequisitionModel> aModels)
        {
            var formate = RequestFormat.JsonFormaterString();

            if (aModels[0].ReqNo=="0")
            {
                
                try
                {
                    string msg = await _gtInventoryStockRequsitionReqGateway.Save(aModels);

                    return Request.CreateResponse(HttpStatusCode.OK,
                        new Confirmation { Output = "success", Msg = msg }, formate);
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK
                        , new Confirmation { Output = "error", Msg = ex.ToString() }, formate);
                }
            }
            else
            {
                try
                {
                    string msg = await _gtInventoryStockRequsitionReqGateway.Update(aModels);

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

        public HttpResponseMessage GetRequisitionListTable(string userName)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtInventoryStockRequsitionReqGateway.GetRequisitionList(userName), formatter);
        }

        public async Task<HttpResponseMessage> SaveRequsitionUse([FromBody] List<InvStockRequisitionModel> aModels)
        {
            var formate = RequestFormat.JsonFormaterString();

            if (aModels[0].ReqNo == "0")
            {

                try
                {
                    string msg = await _gtInventoryStockRequsitionReqGateway.SaveRequsitionUse(aModels);

                    return Request.CreateResponse(HttpStatusCode.OK,
                        new Confirmation { Output = "success", Msg = msg }, formate);
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK
                        , new Confirmation { Output = "error", Msg = ex.ToString() }, formate);
                }
            }
            else
            {
                try
                {
                    string msg = await _gtInventoryStockRequsitionReqGateway.UpdateRequsitionUse(aModels);

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
}