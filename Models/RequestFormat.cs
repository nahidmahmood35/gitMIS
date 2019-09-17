using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;

namespace HospitalManagementApp_Api.Models
{
    public static class RequestFormat
    {
        public static JsonMediaTypeFormatter JsonFormaterString()
        {
            var formatter = new JsonMediaTypeFormatter();
            var json = formatter.SerializerSettings;

            json.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
            json.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            json.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            json.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return formatter;
        }
    }
}