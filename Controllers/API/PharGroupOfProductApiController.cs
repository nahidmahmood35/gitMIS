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
    public class PharGroupOfProductApiController : ApiController
    {

        readonly PharGroupOfProductGateway _gt=new PharGroupOfProductGateway();

        public async Task<HttpResponseMessage> Save_GROUP_OF_PRODUCT_PHAR([FromBody] IdNameForDropdownModel aModel)
        {
            var formate = RequestFormat.JsonFormaterString();
            try
            {
                if (string.IsNullOrEmpty(aModel.Name))
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new Confirmation { Output = "error", Msg = "Name is Empty" }, formate);
                }
                


                if (_gt.FncSeekRecordNew("tbl_GROUP_OF_PRODUCT_PHAR", "Name='" + aModel.Name + "'") == true)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                    new Confirmation { Output = "error", Msg = "Name Already Exists" }, formate);
                }


                else
                {
                    if (_gt.FncSeekRecordNew("tbl_GROUP_OF_PRODUCT_PHAR", "Id=" + aModel.Id + "") == false)
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


        //public HttpResponseMessage GetGetCompanyProductListList(int param)
        //{
        //    var formatter = RequestFormat.JsonFormaterString();
        //    return Request.CreateResponse(HttpStatusCode.OK, _gt.GetCompanyProductList(param), formatter);
        //}






        public HttpResponseMessage Delete(int param)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.Delete(param), formatter);
        }












    }
}
