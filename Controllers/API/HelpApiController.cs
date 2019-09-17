using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HospitalManagementApp_Api.Gateway.DB_Helper;

namespace HospitalManagementApp_Api.Controllers.API
{
    public class HelpApiController : ApiController
    {
        readonly DbConnection _db=new DbConnection();
        public string GetCurrentAgeById(int id)
        {
            DateTime dob =Convert.ToDateTime(_db.ReturnFieldValue("tbl_PATIENT_REGISTRATION", "Id=" + id + "", "DateofBirth"));
            return _db.GetCurrentAgeOfaPatient(dob.ToString("yyyy-MM-dd"));
        }
        public string GetRoomNoByDeptId(int subSubPnoId,int deptId)
        {
            return _db.ReturnFieldValue("tbl_DEPARTMENT_WISE_ROOM_INFO","SubSubPnoId=" + subSubPnoId + " AND DeptId=" + deptId + "", "RoomNo");
        }


    }
}
