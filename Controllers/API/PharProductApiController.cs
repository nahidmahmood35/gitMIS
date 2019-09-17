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
    public class PharProductApiController : ApiController
    {
        readonly PharProductGateway _gt = new PharProductGateway();
        public async Task<HttpResponseMessage> Save_tbl_PHAR_PRODUCT([FromBody] PharProductInfoModel aModel)
        {
            var formate = RequestFormat.JsonFormaterString();
            try{

                if (string.IsNullOrEmpty(aModel.Name))
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new Confirmation { Output = "error", Msg = "Name is Empty" }, formate);
                }

                if (aModel.GroupId == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new Confirmation { Output = "error", Msg = "PdGroupe Id Cannot be Zero" }, formate);
                }

                else
                {
                    if (_gt.FncSeekRecordNew("tbl_PHAR_PRODUCT", "Id=" + aModel.Id + "") == false)
                    {

                        string msg = await _gt.Save(aModel);
                        return Request.CreateResponse(HttpStatusCode.OK,
                            new Confirmation { Output = "success", Msg = msg }, formate);
                    }
                    else
                    {
                        string msg = await _gt.Update(aModel);
                        return Request.CreateResponse(HttpStatusCode.OK,
                            new Confirmation { Output = "success", Msg = msg }, formate);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK
                    , new Confirmation { Output = "error", Msg = ex.ToString() }, formate);
            }
        }


        public HttpResponseMessage GetPharProductList(string searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetPharProductList(searchString), formatter);
        }


        public HttpResponseMessage GetProductGroup(int searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetProductGroup(searchString), formatter);
        }

        //public HttpResponseMessage GetProductSubGroupByManinGroupId(int searchString)
        //{
        //    var formatter = RequestFormat.JsonFormaterString();
        //    return Request.CreateResponse(HttpStatusCode.OK, _gt.GetProductSubGroupByManinGroupId(searchString), formatter);
        //}

        public HttpResponseMessage Delete(int param)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.Delete(param), formatter);
        }

        public HttpResponseMessage GetSaveProductGroup(int groupId, string groupName)
        {
            var formatter = RequestFormat.JsonFormaterString();
            string rtnmsg = groupId != 0 ? _gt.UpdateProductGroup(groupId, groupName) : _gt.SaveProductGroup(groupName);
            return Request.CreateResponse(HttpStatusCode.OK, rtnmsg, formatter);
        }

        public HttpResponseMessage GetSaveGenericCategory(int groupId, string groupName)
        {
            var formatter = RequestFormat.JsonFormaterString();
            string rtnmsg = groupId != 0 ? _gt.UpdateGenericName(groupId, groupName) : _gt.SaveGenericName(groupName);
            return Request.CreateResponse(HttpStatusCode.OK, rtnmsg, formatter);
        }

        public HttpResponseMessage GetGenericCategory(int param)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetGenericName(param), formatter);
        }



    }
}
