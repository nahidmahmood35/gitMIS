using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Gateway
{
    public class PharGroupOfProductGateway :DbConnection
    {

        public Task<string> Save(IdNameForDropdownModel aModel)
        {
            try
            {

                const string query = @"INSERT INTO tbl_GROUP_OF_PRODUCT_PHAR (Name) 
										VALUES (@Name)";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Name", aModel.Name);
                cmd.ExecuteNonQuery();
                Con.Close();
                return Task.FromResult("Save successful");
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return Task.FromResult(exception.Message);
            }

        }



           public Task<string> Update(IdNameForDropdownModel aModel)
        {
            try
            {
                const string query = @"UPDATE tbl_GROUP_OF_PRODUCT_PHAR SET   Name=@Name
                                        WHERE Id=@Id";
                Con.Open();

                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", aModel.Id);
                cmd.Parameters.AddWithValue("@Name", aModel.Name);

                cmd.ExecuteNonQuery();
                Con.Close();
                return Task.FromResult<string>("Update Successful");
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return Task.FromResult(exception.Message);
            }

        }




           public string Delete(int id)
           {
               try
               {
                   string msg = "";
                   const string query = @"DELETE FROM tbl_GROUP_OF_PRODUCT_PHAR WHERE Id=@Id";
                   Con.Open();
                   var cmd = new SqlCommand(query, Con);
                   cmd.Parameters.Clear();
                   cmd.Parameters.AddWithValue("@Id", id);


                   int rtn = cmd.ExecuteNonQuery();
                   msg = rtn == 1 ? "Delete Successful" : "Delete Failed";
                   Con.Close();
                   return msg;
               }
               catch (Exception exception)
               {
                   if (Con.State == ConnectionState.Open)
                   {
                       Con.Close();
                   }
                   return exception.ToString();
               }
           }



//           public List<IdNameForDropdownModel> GetPharGroupOfProductList(int param)
//           {
//               try
//               {
//                   var lists = new List<IdNameForDropdownModel>();
//                   string query = "";

//                   if (param != 0)
//                   {
//                       query = @"SELECT Id,Name
//							From tbl_GROUP_OF_PRODUCT_PHAR WHERE Id=@Param";
//                   }
//                   else
//                   {
//                       query = @"SELECT Id,Name 
//							From tbl_GROUP_OF_PRODUCT_PHAR";
//                   }
//                   Con.Open();
//                   var cmd = new SqlCommand(query, Con);
//                   cmd.Parameters.Clear();
//                   cmd.Parameters.AddWithValue("@Param", param);

//                   var rdr = cmd.ExecuteReader();

//                   while (rdr.Read())
//                   {
//                       lists.Add(new IdNameForDropdownModel()
//                       {


//                       });
//                   }
//                   rdr.Close();
//                   Con.Close();
//                   return lists;
//               }
//               catch (Exception exception)
//               {
//                   if (Con.State == ConnectionState.Open)
//                   {
//                       Con.Close();
//                   }
//                   return new List<IdNameForDropdownModel>();
//               }
//           }















    }
}