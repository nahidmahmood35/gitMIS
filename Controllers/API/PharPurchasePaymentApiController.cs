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
    public class PharPurchasePaymentApiController : ApiController
    {
        readonly PharCompanyGateway _pharCompany = new PharCompanyGateway();
        readonly PharProductGateway _pharProduct = new PharProductGateway();
        readonly PharPurchaseGateway _pharPurchase = new PharPurchaseGateway();

        readonly PharPurchasePaymentGateway _pharPurchasePayment=new PharPurchasePaymentGateway();



        public async Task<HttpResponseMessage> Save([FromBody] PharSalesModel aModel)
        {
            var formate = RequestFormat.JsonFormaterString();
            try
            {
                string msg = await _pharPurchasePayment.Save(aModel);
                return Request.CreateResponse(HttpStatusCode.OK,
                    new Confirmation { Output = "success", Msg = msg }, formate);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK
                    , new Confirmation { Output = "error", Msg = ex.ToString() }, formate);
            }
        }





        public HttpResponseMessage GetPurchaseInvNo(string searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _pharPurchasePayment.GetPurchaseInvNo(searchString), formatter);
        }


        public HttpResponseMessage GetInvDetail(string searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _pharPurchasePayment.GetInvDetail(searchString), formatter);
        }


        public HttpResponseMessage GetCompanyListByName(string searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _pharCompany.GetCompanyListByName(searchString), formatter);
        }









    }
}
