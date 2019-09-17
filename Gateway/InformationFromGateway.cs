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
    public class InformationFromGateway : DbConnection

    {


/////////////////SAVE

        public string Save(IdNameForDropdownModel amodel)
        {

            try
            {
                string msg="";
                const string query = @"INSERT INTO InformationFrom (Name) VALUES (@Name)";
                Con.Open();
                var cmd = new SqlCommand(query,Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Name", amodel.Name);
                int rtn = cmd.ExecuteNonQuery();
                msg=rtn==1 ? "Saved Success" : "Saved Failed";
                Con.Close();
                return msg;

            }
            catch (Exception exception)
            {
                if (Con.State==ConnectionState.Open)
                {
                    Con.Close();
                }


                return exception.ToString();
            }
            
        }

/////////////////////////UPDATE


        public string Update(IdNameForDropdownModel aModel)
        {

            try
            {
                string msg = "";
                const string query = @"UPDATE InformationFrom SET Name=@Name WHERE Id=@Id";
                Con.Open();
                var cmd = new SqlCommand(query,Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Name", aModel.Name);
                cmd.Parameters.AddWithValue("@Id", aModel.Id);
                int  rtn =cmd.ExecuteNonQuery();
                msg = rtn == 1 ? "Update Success" : "Update Failed";
                Con.Close();
                return msg;

            }
            catch (Exception exception)
            {

                if (Con.State==ConnectionState.Open)
                {
                    Con.Close();
                }


                return exception.ToString();
            }



        }



/////////////////////////LIST


        public List<IdNameForDropdownModel> GetInformationFromList(int param)
    {

            try
            {
                var List = new List<IdNameForDropdownModel>();
                string query = "";
                if (param!=0)
                {
                    query = @"SELECT Id,Name From InformationFrom WHERE Id=@Param";
                }

                else
                {
                    query = query = @"SELECT Id,Name From InformationFrom";
                }

                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Param",param);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    List.Add(new IdNameForDropdownModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name=rdr["Name"].ToString(),
                        
                      });

                }
                rdr.Close();
                Con.Close();
                return List;
            }
            catch (Exception exception)
            {

                if (Con.State==ConnectionState.Open)
                {
                   Con.Close(); 
                }
                return new List<IdNameForDropdownModel>();
            }


    }


////////////////////////DELETE

        public string Delete(int Id)
        {

            try
            {
                string msg = "";
                const string query = @"DELETE FROM InformationFrom WHERE Id=@Id";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", Id);
                int rtn = cmd.ExecuteNonQuery();
                msg = rtn == 1 ? "Delete Success" : "Delete Failed";
                Con.Close();
                return msg;


            }
            catch (Exception exception)
            {

                if (Con.State==ConnectionState.Open)
                {
                    Con.Close();
                }


                return exception.ToString();
            }



        }



        
    }

}