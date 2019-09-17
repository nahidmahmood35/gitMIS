using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http.Routing.Constraints;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Gateway
{
    public class VaqGateway:DbConnection
    {
        public List<VaqModel> GetVaqList(int itemId)
        {
            try
            {
                var lists = new List<VaqModel>();
                const string query = @"SELECT Id,GroupName,ItemId,Name,Price From tbl_VAQ_SETUP WHERE ItemId=@ItemId";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@ItemId", itemId);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new VaqModel()
                    {
                        VaqId = Convert.ToInt32(rdr["Id"]),
                        ItemId = Convert.ToInt32(rdr["ItemId"]),
                        VaqGroup = rdr["GroupName"].ToString(),
                        VaqName = rdr["Name"].ToString(),
                        VaqCharge = Convert.ToDouble(rdr["Price"])
                        
                    });
                }
                rdr.Close();
                Con.Close();
                return lists;
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                } 
                throw;
            }

        }
    }
}