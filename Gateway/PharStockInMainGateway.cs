using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Gateway
{
    public class PharStockInMainGateway : DbConnection
    {



        ///////////////////////////SAVE

        public string Save(PharStockInMainModel aModel)
        {
            try
            {
                string msg = "";
                const string query = @"INSERT INTO PharStockInMain (InvoiceNo,InvoiceDate,CompanyID,Remarks,UserName,EntryTime,Valid,PNo,SlipNo,RefNo,RefDate,SlipDate,Status)
                                                VALUES (@InvoiceNo,@InvoiceDate,@CompanyID,@Remarks,@UserName,@EntryTime,@Valid,@PNo,@SlipNo,@RefNo,@RefDate,@SlipDate,@Status)";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@InvoiceNo", aModel.InvoiceNo);
                cmd.Parameters.AddWithValue("@InvoiceDate", aModel.InvoiceDate);
                cmd.Parameters.AddWithValue("@CompanyID", aModel.CompanyID);
                cmd.Parameters.AddWithValue("@Remarks", aModel.Remarks);
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@EntryTime", aModel.EntryTime);
                cmd.Parameters.AddWithValue("@Valid", aModel.Valid);
                cmd.Parameters.AddWithValue("@PNo", aModel.PNo);
                cmd.Parameters.AddWithValue("@SlipNo", aModel.SlipNo);
                cmd.Parameters.AddWithValue("@RefNo", aModel.RefNo);
                cmd.Parameters.AddWithValue("@RefDate", aModel.RefDate);
                cmd.Parameters.AddWithValue("@SlipDate", aModel.SlipDate);
                cmd.Parameters.AddWithValue("@Status", aModel.Status);

                int rtn = cmd.ExecuteNonQuery();
                msg = rtn == 1 ? "Saved Success" : "Saved Failed";
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


        ///////////////////////////UPDATE

        public string Update(PharStockInMainModel aModel)
        {
            try
            {
                string msg = "";
                const string query = @"UPDATE PharStockInMain
                                SET InvoiceNo=@InvoiceNo,InvoiceDate=@InvoiceDate,CompanyID=@CompanyID,Remarks=@Remarks,UserName=@UserName,EntryTime=@EntryTime,Valid=@Valid,PNo=@PNo,SlipNo=@SlipNo,RefNo=@RefNo,RefDate=@RefDate,SlipDate=@SlipDate,Status=@Status
                                WHERE Remarks=@Remarks"; //   no id
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@InvoiceNo", aModel.InvoiceNo);
                cmd.Parameters.AddWithValue("@InvoiceDate", aModel.InvoiceDate);
                cmd.Parameters.AddWithValue("@CompanyID", aModel.CompanyID);
                cmd.Parameters.AddWithValue("@Remarks", aModel.Remarks);
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@EntryTime", aModel.EntryTime);
                cmd.Parameters.AddWithValue("@Valid", aModel.Valid);
                cmd.Parameters.AddWithValue("@PNo", aModel.PNo);
                cmd.Parameters.AddWithValue("@SlipNo", aModel.SlipNo);
                cmd.Parameters.AddWithValue("@RefNo", aModel.RefNo);
                cmd.Parameters.AddWithValue("@RefDate", aModel.RefDate);
                cmd.Parameters.AddWithValue("@SlipDate", aModel.SlipDate);
                cmd.Parameters.AddWithValue("@Status", aModel.Status);

                int rtn = cmd.ExecuteNonQuery();
                return msg = rtn == 1 ? "Update Success" : "Update Failed";
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


        ///////////////////////////LIST

        public List<PharStockInMainModel> GetPharStockInMainList(int param)
        {
            try
            {
                var list = new List<PharStockInMainModel>();
                string query = "";
                if (param != 0)
                {
                    query = @"SELECT InvoiceNo,InvoiceDate,CompanyID,Remarks,UserName,EntryTime,Valid,PNo,SlipNo,RefNo,RefDate,SlipDate,Status
                                    From PharStockInMain WHERE Id=@Param";
                }
                else
                {
                    query = @"SELECT InvoiceNo,InvoiceDate,CompanyID,Remarks,UserName,EntryTime,Valid,PNo,SlipNo,RefNo,RefDate,SlipDate,Status
                            From PharStockInMain";
                }
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Param", param);
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    list.Add(new PharStockInMainModel()
                    {
                        InvoiceNo = rdr["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToDateTime(rdr["InvoiceDate"]),
                        CompanyID = rdr["CompanyID"].ToString(),
                        Remarks = rdr["Remarks"].ToString(),
                        UserName = rdr["UserName"].ToString(),
                        EntryTime = rdr["EntryTime"].ToString(),
                        Valid = Convert.ToInt32(rdr["Valid"]),
                        PNo = rdr["PNo"].ToString(),
                        SlipNo = rdr["SlipNo"].ToString(),
                        RefNo = rdr["RefNo"].ToString(),
                        RefDate = Convert.ToDateTime(rdr["RefDate"]),
                        SlipDate = Convert.ToDateTime(rdr["SlipDate"]),
                        Status = rdr["Status"].ToString(),

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
                return  new List<PharStockInMainModel>();
            }


        }


        ///////////////////////////DELETE

        public string Delete(string Id)
        {
            try
            {
                string msg = "";
                const string query = @"DELETE FROM PharStockInMain WHERE CompanyID=@Id";
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