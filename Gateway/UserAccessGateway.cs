using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;
using HospitalManagementApp_Api.Models.DynamicMenuModel;

namespace HospitalManagementApp_Api.Gateway
{
    public class UserAccessGateway:DbConnection 
    {
        public string Save(List<Child> list)
        {
            if (FncSeekRecordNew("PasswordDtls", "UserName='" + list.ElementAt(0).UserName + "' AND Pno='" + HttpContext.Current.Session["Pno"] + "'"))
            {
                DeleteInsert("Delete From PasswordDtls WHERE UserName='" + list.ElementAt(0).UserName + "' AND Pno='" + HttpContext.Current.Session["Pno"] + "'");
            }
            Con.Open();
            foreach (var child in list)
            {
                string deptName = "";
                switch (child.DeptName)
                {
                    case "1":
                        deptName = "NEW ITEM DEPARTMENT";
                        break;
                    case "2":
                        deptName = "OLD ITEM DEPARTMENT";
                        break;
                    case "3":
                        deptName = "REPAIR ITEM DEPARTMENT";
                        break;
                }

                if (child.ActionName != null )
                {
                    if (child.ParentNode != "Administrator") 
                    {
                        if (child.ParentNode != "Old Item Department")
                        {
                            if (child.ParentNode != "Repair Item Department")
                            {

                                string lcstrSql = @"INSERT INTO PasswordDtls(UserName, PassWord, DepartMentName, ParentNodeTxt, ChildNodeTxt, ActionName, ControllerName, Pno, IconName) 
                                VALUES ('" + child.UserName + "','" + child.Password + "','" + deptName + "','" +
                                                  child.ParentNode + "','" + child.ChildNode + "','" + child.ActionName + "','" +
                                                  child.ControllerName + "','1','fa-diamond')";
                                var aCommand = new SqlCommand(lcstrSql, Con);
                                aCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            Con.Close();
            if (FncSeekRecordNew("tbl_PASSWORD_MASTER", "UserName='" + list.ElementAt(0).UserName + "' "))
            {
                DeleteInsert("Delete From tbl_PASSWORD_MASTER WHERE UserName='" + list.ElementAt(0).UserName + "'");
            }
            Con.Open();
            string query = @"INSERT INTO tbl_PASSWORD_MASTER(UserName,  Can_Give_AITSO_Discount, PermittedBy) VALUES ('" + list.ElementAt(0).UserName + "','" + list.ElementAt(0).CanGiveAitsoDiscount + "','" + System.Web.HttpContext.Current.Session["UserName"] + "')";
            var cmd = new SqlCommand(query, Con);
            cmd.ExecuteNonQuery();
            Con.Close();

            return "";
        }
        public List<Child> GetDetailsPermission(string userName)
        {
            var lists = new List<Child>();
            var query = @"SELECT  UserName,Password,DepartMentName, ParentNodeTxt, ChildNodeTxt, ActionName, ControllerName, Pno, IconName FROM PasswordDtls WHERE UserName='"+ userName  +"'";
            Con.Open();
            var cmd = new SqlCommand(query, Con);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                lists.Add(new Child()
                {
                    DeptName = rdr["DepartMentName"].ToString(),
                    ParentNode = rdr["ParentNodeTxt"].ToString(),
                    ChildNode = rdr["ChildNodeTxt"].ToString(),
                    ActionName = rdr["ActionName"].ToString(),
                    ControllerName = rdr["ControllerName"].ToString(),
                    UserName = rdr["UserName"].ToString(),
                    Password = rdr["Password"].ToString(),
                }); 
            }
            Con.Close();
            return lists;
        }
        internal Child  GetMasterPermission(string userName)
        {
            var lists = new Child();
            var query = @"SELECT  Can_Give_AITSO_Discount,UserImage,SignImage FROM tbl_PASSWORD_MASTER WHERE UserName='" + userName + "'";
            Con.Open();
            var cmd = new SqlCommand(query, Con);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                lists.CanGiveAitsoDiscount = Convert.ToInt32(rdr["Can_Give_AITSO_Discount"].ToString());
                lists.UserImageString = rdr["UserImage"].ToString();
                var image = (byte[])rdr["SignImage"];
                lists.UserSignString ="data:image/jpeg;base64,"+ Convert.ToBase64String(image);

            }
            Con.Close();
            return lists;
        }
        public string SaveImageAndSignature(string userName, string imageString, string imageSignatureString)
        {
            Image signImage = Base64ToImage(imageSignatureString.Replace("data:image/jpeg;base64,", ""));
            var logoImage = ImageToByteArray(signImage);

            if (FncSeekRecordNew("tbl_PASSWORD_MASTER", "UserName='" + userName + "'"))
            {
                Con.Open();
                const string query = @"UPDATE tbl_PASSWORD_MASTER SET UserImage=@UserImage,SignImage=@signImage WHERE UserName=@UserName ";
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@UserImage", imageString);
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@signImage", logoImage);
                cmd.ExecuteNonQuery();
                Con.Close();
            }
           return "Saved Success";
        }
        internal List<IdNameForDropdownModel> GetUserList()
        {
            var list = new List<IdNameForDropdownModel>();
            var cmd = new SqlCommand(@"SELECT Distinct UserName FROM tbl_PASSWORD_MASTER", Con);
            Con.Open();
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new IdNameForDropdownModel()
                {
                    Name = rdr["UserName"].ToString(),
                });
            }
            rdr.Close();
            Con.Close();
            return list;
        }
    }
}