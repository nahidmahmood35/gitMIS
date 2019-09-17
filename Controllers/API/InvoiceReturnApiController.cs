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
    public class InvoiceReturnApiController : ApiController
    {
        readonly InvoiceReturnGateway _gtRtnInvoice = new InvoiceReturnGateway();
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
                    string msg = await _gtRtnInvoice.Save(mInvoice);
                    //_gtInvoice.PrintReport("DiagnosisInvoiceRPT.rpt", "SP_GET_INVOICE_PRINT", "" + mInvoice.ElementAt(0).InvMasterId + "", "SP_GET_INVOICE_PRINT", "", "", "S");
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formatter);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }
        }

        public  HttpResponseMessage GetReturnInvoicePrint(int id)
        {
            //int idN =Convert.ToInt32(_gtInvoice.ReturnFieldValue("tbl_INVOICE_MASTER", "", "MAX(Id)"));
            //_gtInvoice.PrintReport("DiagnosisInvoiceRPT.rpt", "SP_GET_INVOICE_PRINT", "" + idN + "", "SP_GET_INVOICE_PRINT", "", "", "S");
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, "Success", formatter);
        }

       

        public HttpResponseMessage GetInvoiceDetails(string invNo,DateTime invDate)
        {
            int invMasterId = 0;
            if (_gtRtnInvoice.FncSeekRecordNew("tbl_INVOICE_MASTER","InvoiceNo='"+ invNo +"' AND InvoiceDate='"+ invDate.ToString("yyyy-MM-dd") +"'"))
            {
                invMasterId =Convert.ToInt32(_gtRtnInvoice.ReturnFieldValue("tbl_INVOICE_MASTER","InvoiceNo='" + invNo + "' AND InvoiceDate='" + invDate.ToString("yyyy-MM-dd") + "'", "Id"));
            }
            var invDtls = _gtRtnInvoice.GetInvoiceDetails(invMasterId);
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, invDtls, formatter);
        }




    }
}
