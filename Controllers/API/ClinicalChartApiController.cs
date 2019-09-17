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
    public class ClinicalChartApiController : ApiController
    {
        readonly ClinicalChartGateway _chartGateway = new ClinicalChartGateway();
        readonly DropDownGateway _gtdropDownGateway = new DropDownGateway();
        public async Task<HttpResponseMessage> Post([FromBody] ClinicalChartModel mClinicalChart)
        {
            try
            {
                var formatter = RequestFormat.JsonFormaterString();
                if (string.IsNullOrEmpty(mClinicalChart.Description)){return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Name is Empty" }, formatter);}
                if (string.IsNullOrEmpty(mClinicalChart.PCode)){return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "PCode is Empty" }, formatter);}
                if (string.IsNullOrEmpty(mClinicalChart.UserName)) { return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "UserName is Empty" }, formatter); }
                if (Math.Abs(mClinicalChart.Charge) < 1){return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Charge Must Be Positive" }, formatter);}
                
                else
                {
                    if (_chartGateway.FncSeekRecordNew("tbl_CLINICAL_CHART", "Id=" + mClinicalChart.ItemId + "") == false)
                    {

                        if (_chartGateway.FncSeekRecordNew("tbl_CLINICAL_CHART", "PCode='" + mClinicalChart.PCode+ "'"))
                        {
                            return Request.CreateResponse(HttpStatusCode.OK,new Confirmation{Output = "error",Msg = "PCode Already Exists"},formatter);
                        }
                            
                        else
                        {
                            string msg = await _chartGateway.Save(mClinicalChart);
                            return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formatter);
   
                        }
                    }
                    else
                    {
                        string msg = await _chartGateway.Update(mClinicalChart);
                        return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formatter);
                    }
                }
            }
            catch (Exception ex)
            {
                var formatter = RequestFormat.JsonFormaterString();
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }
        }
    
        public HttpResponseMessage GetIndoorBillGroupList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _chartGateway.GetIdNameForDropDownBox("SELECT Id,Name From tbl_INDOOR_BILL_GROUP_HEAD_MST Order By Id"), formatter);
        }
        public HttpResponseMessage GetReportGroupInfoList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _chartGateway.GetIdNameForDropDownBox("SELECT Id,Name From tbl_REPORT_GROUP_INFO_MST Order By Id"), formatter);
        }
        public HttpResponseMessage GetDiscountGroupInfoList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _chartGateway.GetIdNameForDropDownBox("SELECT Id,Name From tbl_DISCOUNT_GROUP_INFO_MST Order By Id"), formatter);
        }
        public HttpResponseMessage GetAccountReportGroupNameList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _chartGateway.GetIdNameForDropDownBox("SELECT Id,Name From tbl_ACCOUNT_REPORT_INFO_MST Order By Id"), formatter);
        }
        public HttpResponseMessage GetSubSubPnoList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _chartGateway.GetIdNameForDropDownBox("SELECT IdNo AS Id,SubSubPno  AS Name FROm project Order By IdNo"), formatter);
        }
        public HttpResponseMessage GetClinicalChartList(string searchString,int isShowAll)
        {
            var formatter = RequestFormat.JsonFormaterString();
            var s = _chartGateway.GetClinicalChartList(searchString, isShowAll);
            return Request.CreateResponse(HttpStatusCode.OK, _chartGateway.GetClinicalChartList(searchString, isShowAll), formatter);
        }
        public HttpResponseMessage GetDeleteById(int id)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _chartGateway.DeleteInsert("DELETE FROM tbl_CLINICAL_CHART WHERE Id=" + id + ""), formatter);
        }


        public HttpResponseMessage GetSaveIndoorBillGroup(string name)
        {
            var formatter = RequestFormat.JsonFormaterString();
            if(_gtdropDownGateway.FncSeekRecordNew("tbl_INDOOR_BILL_GROUP_HEAD_MST","Name='"+ name +"'"))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "This Name Already Exist", formatter);
            }
            return Request.CreateResponse(HttpStatusCode.OK, _gtdropDownGateway.SaveWithReturnId("tbl_INDOOR_BILL_GROUP_HEAD_MST", name), formatter);
        }


        public HttpResponseMessage GetSaveReportGroup(string name)
        {
            var formatter = RequestFormat.JsonFormaterString();
            if (_gtdropDownGateway.FncSeekRecordNew("tbl_REPORT_GROUP_INFO_MST", "Name='" + name + "'"))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "This Name Already Exist", formatter);
            }

            return Request.CreateResponse(HttpStatusCode.OK, _gtdropDownGateway.SaveWithReturnId("tbl_REPORT_GROUP_INFO_MST", name), formatter);
        }


        public HttpResponseMessage GetSaveDiscountGroup(string name)
        {
            var formatter = RequestFormat.JsonFormaterString();
            if (_gtdropDownGateway.FncSeekRecordNew("tbl_DISCOUNT_GROUP_INFO_MST", "Name='" + name + "'"))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "This Name Already Exist", formatter);
            }
            return Request.CreateResponse(HttpStatusCode.OK, _gtdropDownGateway.SaveWithReturnId("tbl_DISCOUNT_GROUP_INFO_MST", name), formatter);
        }


        public HttpResponseMessage GetSaveAccountReportGroup(string name)
        {
            var formatter = RequestFormat.JsonFormaterString();
            if (_gtdropDownGateway.FncSeekRecordNew("tbl_ACCOUNT_REPORT_INFO_MST", "Name='" + name + "'"))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "This Name Already Exist", formatter);
            }
            return Request.CreateResponse(HttpStatusCode.OK, _gtdropDownGateway.SaveWithReturnId("tbl_ACCOUNT_REPORT_INFO_MST", name), formatter);
        }



    }
}
