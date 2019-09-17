using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;


namespace HospitalManagementApp_Api.Controllers.API
{
    public class InvoiceApiController : ApiController
    {
        readonly InvoiceGateway _gtInvoice = new InvoiceGateway();
        #region
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> Post([FromBody] List<InvoiceModel> mInvoice)
        {
            var formatter = RequestFormat.JsonFormaterString();
            try
            {
                if (mInvoice.ElementAt(0).DrId == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Doctor is Empty" }, formatter);
                }

                if (mInvoice.ElementAt(0).UserName == "")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "UserName is Empty" }, formatter);
                }
                if (mInvoice.ElementAt(0).ItemId == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Item is Empty" }, formatter);
                }

                else
                {
                    string msg = await _gtInvoice.Save(mInvoice);
                    _gtInvoice.PrintReport("DiagnosisInvoiceRPT.rpt", "SP_GET_INVOICE_PRINT", "" + mInvoice.ElementAt(0).InvMasterId + "", "SP_GET_INVOICE_PRINT", "", "", "S");
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formatter);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }
        }
        #endregion

        public  HttpResponseMessage GetInvoicePrint(int id)
        {
            int idN =Convert.ToInt32(_gtInvoice.ReturnFieldValue("tbl_INVOICE_MASTER", "", "MAX(Id)"));
            _gtInvoice.PrintReport("DiagnosisInvoiceRPT.rpt", "SP_GET_INVOICE_PRINT", "" + idN + "", "SP_GET_INVOICE_PRINT", "", "", "S");
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, "Success", formatter);
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get([FromBody] List<InvoiceModel> mInvoice)
        {
            var formatter = RequestFormat.JsonFormaterString();
            var amount =  _gtInvoice.GetMaximumLessAmountByDoctor(mInvoice);
            return Request.CreateResponse(HttpStatusCode.OK, amount, formatter);
        }



        public HttpResponseMessage GetInvoiceList(string dateFrom,string dateTo)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtInvoice.GetInvoiceList(dateFrom,dateTo), formatter);
        }


        public HttpResponseMessage GetPackageCodeMain()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtInvoice.GetIdNameForDropDownBox("SELECT Id,Name From tbl_PACKAGE_INFO_MST Order By Id"), formatter);
        }
        public HttpResponseMessage GetPackageItemDetail(int mainpackId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtInvoice.GetPackageItemDetail(mainpackId), formatter);
        }





    }
}
