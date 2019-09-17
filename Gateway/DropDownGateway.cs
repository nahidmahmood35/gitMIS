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
    public class DropDownGateway : DbConnection
    {
        public Task<string> Save(string tableName, string name)
        {
            try
            {
                string query = @"INSERT INTO " + tableName + " (Name) VALUES (@Name)";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Name", name);
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
        public Task<string> Update(string tableName,string name,int id)
        {
            try
            {
                string query = @"UPDATE "+ tableName +" SET Name=@Name 	WHERE Id=@Id";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.ExecuteNonQuery();
                Con.Close();
                return Task.FromResult("Update successful");
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
        public Task<int> SaveWithReturnId(string tableName, string name)
        {
            try
            {
                string query = @"INSERT INTO " + tableName + " (Name) OUTPUT INSERTED.ID VALUES (@Name)";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Name", name);
                int rtn=Convert.ToInt32(cmd.ExecuteScalar());
                Con.Close();
                return Task.FromResult(rtn);
            }
            catch (Exception ex)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return Task.FromResult(0);
            }
        }

      


    }
}