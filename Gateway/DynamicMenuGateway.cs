using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;
using HospitalManagementApp_Api.Models.DynamicMenuModel;

namespace HospitalManagementApp_Api.Gateway
{
    public class DynamicMenuGateway : DbConnection
    {

        public List<Parent> GetParentNode()
        {
            var aMenuModels = new List<Parent>();

           // string query = @"SELECT distinct ParentNodeTxt,SLNo FROM PasswordDtls WHERE UserName='" + HttpContext.Current.Session["UserName"] + "' AND ParentNodeTxt <>'' Order by SlNo ";
            string query = @"SELECT distinct ParentNodeTxt FROM PasswordDtls WHERE UserName='" + HttpContext.Current.Session["UserName"] + "' AND ParentNodeTxt <>'' ";
            Con.Open();
            var aComand = new SqlCommand(query,Con);
            SqlDataReader aReader = aComand.ExecuteReader();
            while (aReader.Read())
            {
                var amenumodel = new Parent
                {
                    Name = aReader["ParentNodeTxt"].ToString(),
                };
                aMenuModels.Add(amenumodel);
            }
            Con.Close();
            return aMenuModels;
        }
        public List<Child> ChildNode()
        {
            var amodels = new List<Child>();
            string query = @"SELECT * FROM PasswordDtls WHERE ParentNodeTxt <> ChildNodeTxt AND UserName='" + HttpContext.Current.Session["UserName"] + "' Order by SlNo ";
            Con.Open();
            var aComand = new SqlCommand(query, Con);
            SqlDataReader aReader = aComand.ExecuteReader();

            while (aReader.Read())
            {
                var model = new Child
                {
                   // DeptName = aReader["DepartMentName"].ToString(),
                    ParentNode= aReader["ParentNodeTxt"].ToString(),
                    Name = aReader["ChildNodeTxt"].ToString(),
                    ActionName = aReader["ActionName"].ToString(),
                    ControllerName = aReader["ControllerName"].ToString(),
                };
                amodels.Add(model);
            }
            
            Con.Close();

            return amodels;
        }

        public int IsExistUserNamePassword(ComInfo aComInfo)
        {
            int a = 0;
            a = FncSeekRecordNew("PasswordDtls", "UserName='" + aComInfo.UserName + "'") ? 1 : 0;
            a = FncSeekRecordNew("PasswordDtls", "Password='" + aComInfo.Password + "'") ? 1 : 0;
            a = FncSeekRecordNew("PasswordDtls", "UserName='" + aComInfo.UserName + "' AND Password='" + aComInfo.Password + "'") ? 1 : 0;
            
            return a;
        }
        public string  GetUserImageByUserName(string userName)
        {
            var imaBytes = "";
            string query = @"SELECT Userimage FROM tbl_PASSWORD_MASTER WHERE UserName ='"+ userName +"' ";
            Con.Open();
            var aComand = new SqlCommand(query, Con);
            SqlDataReader aReader = aComand.ExecuteReader();
            while (aReader.Read())
            {
                    imaBytes = aReader["Userimage"].ToString();
            }
            Con.Close();
            return imaBytes;
        }
    }
}