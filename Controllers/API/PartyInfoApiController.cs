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
    public class PartyInfoApiController : ApiController
    {

        PharPartyGateway _gt=new PharPartyGateway();

        public async Task<HttpResponseMessage> Save_PartyInfo([FromBody] PharCompanyModel aModel)
        {

            var formate = RequestFormat.JsonFormaterString();
            try
            {
                if (string.IsNullOrEmpty(aModel.Name))
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new Confirmation { Output = "error", Msg = "Name is Null" });
                }
                else
                {
                    if (_gt.FncSeekRecordNew("PartyInfo", "Id=" + aModel.Id + "") == false)
                    {
                        if (_gt.FncSeekRecordNew("PartyInfo", "Name='" + aModel.Name + "'") == true)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK,
                                new Confirmation { Output = "error", Msg = "Name Already Exist" }, formate);
                        }
                        else
                        {
                            string msg = await _gt.Save(aModel);
                            return Request.CreateResponse(HttpStatusCode.OK,
                                new Confirmation { Output = "success", Msg = msg }, formate);
                        }

                    }

                    else
                    {
                        string msg = await _gt.Update(aModel);
                        return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formate);
                       
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                    new Confirmation { Output = "error",Msg=ex.ToString() }, formate);
            }
        }

        //public HttpResponseMessage GetPartyList(int searchString)
        //{
        //    var formatter = RequestFormat.JsonFormaterString();
        //    return Request.CreateResponse(HttpStatusCode.OK, _gt.GetPartyInfoList(searchString), formatter);
        //}



    }
}
