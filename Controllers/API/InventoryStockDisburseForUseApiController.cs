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
    public class InventoryStockDisburseForUseApiController : ApiController
    {
        readonly InventoryStockDisburseForUseGateway _gt = new InventoryStockDisburseForUseGateway();

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

        //public HttpResponseMessage GetInventoryRequisitionDetailList(string searchString, string userName)
        //{
        //    var formatter = RequestFormat.JsonFormaterString();
        //    return Request.CreateResponse(HttpStatusCode.OK, _gt.GetInventoryRequisitionDetailList(searchString, userName), formatter);
        //}

        //public HttpResponseMessage GetDetailsByProduct(int productId, int depId)
        //{

        //    var trList = _gt.GeTtrList(productId, depId);
        //    var TotalQty = _gt.GeTTotalQty(productId, depId);
        //    //var suppDetails = _aSupplierManager.GetDetailsSupplierListById(param);
        //    var data = new { trList = trList, TotalQty = TotalQty };
        //    //return Json(data, JsonRequestBehavior.AllowGet);
        //    //var formatter = RequestFormat.JsonFormaterString();
        //    //return Request.CreateResponse(HttpStatusCode.OK, _gt.GetInventoryRequisitionDetailList(searchString, userName), formatter);
        //    return Request.CreateResponse(data);

            
        //}

        //public async Task<HttpResponseMessage> Save([FromBody] List<InvStockRequisitionModel> aModels)
        //{
        //    var formate = RequestFormat.JsonFormaterString();
        //    try
        //    {
        //        string msg = await _gt.Update(aModels);

        //        return Request.CreateResponse(HttpStatusCode.OK,
        //            new Confirmation { Output = "success", Msg = msg }, formate);
        //    }

        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK
        //            , new Confirmation { Output = "error", Msg = ex.ToString() }, formate);
        //    }
        //}

        


    }
}