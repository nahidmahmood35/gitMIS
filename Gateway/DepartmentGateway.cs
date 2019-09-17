using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Gateway
{
    public class DepartmentGateway:DbConnection
    {
        public string Save(IdNameForDropdownModel aModel)
        {
            try
            {
                string msg = "";
                const string query = @"INSERT INTO Department (Name) VALUES (@Name)";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Name", aModel.Name);
                int rtn = cmd.ExecuteNonQuery();
                msg = rtn==1 ? "Saved Success" : "Saved Failed";
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
        public string Update(IdNameForDropdownModel aModel)
        {
            try
            {
                string msg = "";
                const string query = @"UPDATE Department SET Name=@Name WHERE Id=@Id";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Name", aModel.Name);
                cmd.Parameters.AddWithValue("@Id", aModel.Id);
                int rtn = cmd.ExecuteNonQuery();
                msg = rtn == 1 ? "Update Success" : "Update Failed";
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
        public List<IdNameForDropdownModel> GetDepartmentList(int param)
        {
            try
            {
                var lists = new List<IdNameForDropdownModel>();
                string query="";
                if (param!=0)
                {
                    query = @"SELECT Id,Name From Department WHERE Id=@Param";
                }
                else
                {
                    query = @"SELECT Id,Name From Department";
                }
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Param", param);
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    lists.Add(new IdNameForDropdownModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["Name"].ToString(),
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
                return new List<IdNameForDropdownModel>();
            }
        }
        public string Delete(int id)
        {
            try
            {
                string msg = "";
                const string query = @"DELETE FROM Department WHERE Id=@Id";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", id);
                int rtn = cmd.ExecuteNonQuery();
                msg = rtn == 1 ? "Delete Success" : "Delete Failed";
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
    }
}