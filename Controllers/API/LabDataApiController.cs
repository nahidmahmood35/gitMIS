using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
//using System.Web.Mvc;
using System.Web.Http.Cors;
using CrystalDecisions.CrystalReports.Engine;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Controllers.API
{
    public class LabDataApiController : ApiController
    {
        readonly LabDataGateway _gt = new LabDataGateway();

        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] List<LabParameterModel> mLab)
        {
            var formatter = RequestFormat.JsonFormaterString();
            try
            {
                string msg = await _gt.Save(mLab);
              //  int subsubPnoId =Convert.ToInt32(_gt.ReturnFieldValue("tbl_CLINICAL_CHART", "Id=" + mLab.ElementAt(0).ItemId + "","SubSubPnoId"));
              //  string reportFileName = _gt.ReturnFieldValue("Project", "IdNo=" + subsubPnoId + "","SubSubPno");
              //  _gt.PrintReportLab(reportFileName + ".rpt", "VW_GET_LAB_REPORT_VIEW", " WHERE ItemId=" + mLab.ElementAt(0).ItemId + " AND InvoiceNo='"+ mLab.ElementAt(0).InvoiceNo +"'", "VW_GET_LAB_REPORT_VIEW", "Advance Collection", reportFileName, "V");
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formatter);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }
        }



        public HttpResponseMessage GetReprintLabReport(string invNo, int itemId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            string msg = "";
            try
            {
                string reportFileName = _gt.ReturnFieldValue("tbl_LAB_PARAMETER_DEFINITION", "ItemId=" + itemId + "", "Isnull(ReportFileName,'N/A')");

                if ((reportFileName != ""))
                {
                    _gt.PrintReportLab(reportFileName + ".rpt", "VW_GET_LAB_REPORT_VIEW", " WHERE ItemId=" + itemId + " AND InvoiceNo='" + invNo + "'", "VW_GET_LAB_REPORT_VIEW", "Advance Collection", reportFileName, "V");    
                }
                else
                {
                    _gt.PrintReportLab("Single.rpt", "VW_GET_LAB_REPORT_VIEW", " WHERE ItemId=" + itemId + " AND InvoiceNo='" + invNo + "'", "VW_GET_LAB_REPORT_VIEW", "Advance Collection", reportFileName, "V");

                }
                msg = "Ok";

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formatter);
        }


        public HttpResponseMessage GetLabDataByItemId(int invMasterId,int itemId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetResultByInvmasterAndItemId(invMasterId,itemId), formatter);
        }
   
    }
}
