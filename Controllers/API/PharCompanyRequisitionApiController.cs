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
    public class PharCompanyRequisitionApiController : ApiController
    {
        readonly PharCompanyRequisitionGateway _gateway=new PharCompanyRequisitionGateway();
        readonly PharCompanyGateway _companyGateway=new PharCompanyGateway();
        readonly PharProductGateway _productGateway = new PharProductGateway();

        public async Task<HttpResponseMessage> Save([FromBody] List<PharPurchaseModel> aModels)
        {
            var formate = RequestFormat.JsonFormaterString();           //  VW_PHAR_COMPANY_REQUISITION
            try
            {
                string msg = await _gateway.Save(aModels);
                _gateway.PrintReportPhar("PharCompanyRequisition.rpt", "VW_PHAR_COMPANY_REQUISITION", " WHERE MasterId='" + aModels.ElementAt(0).InvoiceMasterId + "'", "VW_PHAR_COMPANY_REQUISITION", "Requisition Invoice", "", "V");
                return Request.CreateResponse(HttpStatusCode.OK,
                    new Confirmation { Output = "success", Msg = msg }, formate);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK
                    , new Confirmation { Output = "error", Msg = ex.ToString() }, formate);
            }
        }


        public HttpResponseMessage GetMedicineName(string searchString,int companyId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _productGateway.GetPharProductList(searchString,companyId), formatter);
        }


        public HttpResponseMessage GetReqProductList(int param, string userName)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gateway.GetReqProductList(param,userName), formatter);
        }

        

        public HttpResponseMessage GetCompanyListByName(string param)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _companyGateway.GetCompanyListByName(param), formatter);
        }




        public HttpResponseMessage GetReprintInvoice(string refNo)
        {
            _gateway.PrintReportPhar("PharCompanyRequisition.rpt", "VW_PHAR_COMPANY_REQUISITION", " WHERE ReqNo='" + refNo + "'", "VW_PHAR_COMPANY_REQUISITION", "Requisition Invoice", "", "V");
     //   _gateway.PrintReportPhar("PharCompanyRequisition.rpt", "VW_PHAR_COMPANY_REQUISITION", " WHERE MasterId='" + aModels.ElementAt(0).InvoiceMasterId + "'", "VW_PHAR_COMPANY_REQUISITION", "Requisition Invoice", "", "V");
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, "Success", formatter);
        }

        public HttpResponseMessage GetInvoiceList(string userName)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gateway.GetPharInvoiceList("0", userName), formatter);
        }





    }
}
