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
    public class ProjectGateway : DbConnection
    {


        
///////////////////////////SAVE

        public string Save(ProjectModel aModel)
        {
            try
            {

                string msg = "";
                const string query = @"INSERT INTO project (PnoCode,pno,ContractPerson,title,pl,ps,ec,status,UserName,SubPno,SubSubPno,ProjectCost,IsShowAccounts,WillShowInHospital,WillShowInPayment)
                                        VALUES (@PnoCode,@pno,@ContractPerson,@title,@pl,@ps,@ec,@status,@UserName,@SubPno,@SubSubPno,@ProjectCost,@IsShowAccounts,@WillShowInHospital,@WillShowInPayment)";
                Con.Open();
                var cmd = new SqlCommand(query,Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@PnoCode", aModel.PnoCode);
                cmd.Parameters.AddWithValue("@pno", aModel.Pno);
                cmd.Parameters.AddWithValue("@ContractPerson", aModel.ContractPerson);
                cmd.Parameters.AddWithValue("@title", aModel.Title);
                cmd.Parameters.AddWithValue("@status", aModel.Status);
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@SubPno", aModel.SubPno);
                cmd.Parameters.AddWithValue("@SubSubPno", aModel.SubSubPno);
                cmd.Parameters.AddWithValue("@ProjectCost", aModel.ProjectCost);
                cmd.Parameters.AddWithValue("@IsShowAccounts", aModel.IsShowAccounts);
                cmd.Parameters.AddWithValue("@WillShowInHospital", aModel.WillShowInHospital);
                cmd.Parameters.AddWithValue("@WillShowInPayment", aModel.WillShowInPayment);

                int rtn = cmd.ExecuteNonQuery();
                msg = rtn == 1 ? "Saved Success" : "Saved Failed";
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


///////////////////////////UPDATE

        public string Update(ProjectModel aModel)
        {
            try
            {
                string msg = "";
                const string query = @"UPDATE project SET PnoCode=@PnoCode,pno=@pno,ContractPerson=@ContractPerson,title=@title,pl=@pl,ps=@ps,ec=@ec,status=@status,UserName=@UserName,SubPno=@SubPno,SubSubPno=@SubSubPno,ProjectCost=@ProjectCost,IsShowAccounts=@IsShowAccounts,WillShowInHospital=@WillShowInHospital,WillShowInPayment=@WillShowInPayment
                                        WHERE PnoCode=@PnoCode";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@PnoCode", aModel.PnoCode);
                cmd.Parameters.AddWithValue("@pno", aModel.Pno);
                cmd.Parameters.AddWithValue("@ContractPerson", aModel.ContractPerson);
                cmd.Parameters.AddWithValue("@title", aModel.Title);
                cmd.Parameters.AddWithValue("@status", aModel.Status);
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@SubPno", aModel.SubPno);
                cmd.Parameters.AddWithValue("@SubSubPno", aModel.SubSubPno);
                cmd.Parameters.AddWithValue("@ProjectCost", aModel.ProjectCost);
                cmd.Parameters.AddWithValue("@IsShowAccounts", aModel.IsShowAccounts);
                cmd.Parameters.AddWithValue("@WillShowInHospital", aModel.WillShowInHospital);
                cmd.Parameters.AddWithValue("@WillShowInPayment", aModel.WillShowInPayment);
                int rtn = cmd.ExecuteNonQuery();
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


///////////////////////////LIST
       
        public List<ProjectModel> GetProjectList(int param)
        {
            try
            {
                var list = new List<ProjectModel>();
                string query = "";
                if (param!=0)
                {

                    query = @"SELECT PnoCode,pno,ContractPerson,title,pl,ps,ec,status,UserName,SubPno,SubSubPno,ProjectCost,IsShowAccounts,WillShowInHospital,WillShowInPayment
                                From project WHERE Id=@Param";
                }

                else
                {
                    query = @"SELECT PnoCode,pno,ContractPerson,title,pl,ps,ec,status,UserName,SubPno,SubSubPno,ProjectCost,IsShowAccounts,WillShowInHospital,WillShowInPayment
                                From project";
                }
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Param", param);
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    list.Add(new ProjectModel()
                    {

                        PnoCode = rdr["PnoCode"].ToString(),
                        Pno = rdr["pno"].ToString(),
                        ContractPerson = rdr["ContractPerson"].ToString(),
                        Title = rdr["title"].ToString(),
                        Status = rdr["status"].ToString(),
                        UserName = rdr["UserName"].ToString(),
                        SubPno = rdr["SubPno"].ToString(),
                        SubSubPno = rdr["SubSubPno"].ToString(),
                        ProjectCost = Convert.ToDouble(rdr["ProjectCost"]),
                        IsShowAccounts = Convert.ToInt32(rdr["IsShowAccounts"]),
                        WillShowInHospital = Convert.ToInt32(rdr["WillShowInHospital"]),
                        WillShowInPayment = Convert.ToInt32(rdr["WillShowInPayment"]),
                    

                    });
                   

                }

                rdr.Close();
                Con.Close();
                return list;
            }
            catch (Exception exception)
            {
                if (Con.State==ConnectionState.Open)
                {
                    Con.Close();
                }
                return new List<ProjectModel>();
            }


        }


///////////////////////////DELETE

        public string Delete(int param)
        {
            try
            {
                string msg = "";
                const string query = @"DELETE FROM project WHERE IsShowAccounts=123"; // Id 
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@param", param);
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