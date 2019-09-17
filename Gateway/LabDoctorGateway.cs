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
    public class LabDoctorGateway:DbConnection
    {
        internal Task<string> Save(DoctorModel mLabDoctor)
        {
            try
            {
                Con.Open();
                const string query = @"INSERT INTO tbl_LAB_DOCTOR_INFO(DrStatus, DrDetails, UserName,DrName) VALUES (@DrStatus, @DrDetails, @UserName,@DrName)";
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@DrStatus", mLabDoctor.Description);
                cmd.Parameters.AddWithValue("@DrDetails", mLabDoctor.Details);
                cmd.Parameters.AddWithValue("@UserName", mLabDoctor.UserName);
                cmd.Parameters.AddWithValue("@DrName", mLabDoctor.Name);
                cmd.ExecuteNonQuery();
                Con.Close();
                return Task.FromResult("Save Successful");
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

        internal object GetLabDoctorList(int id, string searchString)
        {
            try
            {
                var lists = new List<DoctorModel>();
                string cond = "";
                if (searchString!="0")
                {
                    cond = "AND DrName like '%'+'" + searchString + "'+'%'";
                }
                if (id!=0)
                {
                    cond = "AND Id="+ id +"";
                }
                string query = @"SELECT * FROM tbl_LAB_DOCTOR_INFO WHERE 1=1 "+ cond +"";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new DoctorModel()
                    {
                        DrId = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["DrName"].ToString(),
                        Details = rdr["DrDetails"].ToString(),
                    });
                }
                Con.Close();
                return lists;
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        internal Task<string> Update(DoctorModel mLabDoctor)
        {
            try
            {
                Con.Open();
                const string query = @"UPDATE tbl_LAB_DOCTOR_INFO SET  DrDetails=@DrDetails, UserName=@UserName,DrName=@DrName WHERE Id=@Id";
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", mLabDoctor.DrId);
                cmd.Parameters.AddWithValue("@DrStatus", mLabDoctor.Description);
                cmd.Parameters.AddWithValue("@DrDetails", mLabDoctor.Details);
                cmd.Parameters.AddWithValue("@UserName", mLabDoctor.UserName);
                cmd.Parameters.AddWithValue("@DrName", mLabDoctor.Name);
                cmd.ExecuteNonQuery();
                Con.Close();
                return Task.FromResult("Update Successful");
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
    }
}