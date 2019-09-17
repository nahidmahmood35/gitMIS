using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Gateway
{
    public class PharCompanyGateway : DbConnection
    {
        private SqlTransaction _trans;

        public Task<string> Save(PharSalesModel aModel)
        {
            try
            {
                
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();

                string trNo = GetTrNo("TrNo", "tbl_PHAR_PURCHASE_LEDGER",_trans);
                string invoiceNo = trNo;
                const string query = @"INSERT INTO tbl_PHAR_COMPANY (Name,Address,Phone,EntryDate,UserName) 
										output inserted.id VALUES (@Name,@Address,@Phone,@EntryDate,@UserName)";
                var cmd = new SqlCommand(query, Con,_trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Name", aModel.Name);
                cmd.Parameters.AddWithValue("@Address", aModel.Address);
                cmd.Parameters.AddWithValue("@Phone", aModel.Phone);
                cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@Status", aModel.Status);
                var comid = (int)cmd.ExecuteScalar();


             //   tbl_PHAR_PURCHASE_LEDGER

                const string query2 = @"INSERT INTO tbl_PHAR_PURCHASE_LEDGER (TrNo,TrDate,InvoiceNo,InvoiceDate,ComPanyId,PurchaseAmount,LessAmount,PaymentAmount,Status,SubSubPnoId,UserName,PaymentStatus,BranchId,EntryDate,EntryTime) VALUES (@TrNo,@TrDate,@InvoiceNo,@InvoiceDate,@ComPanyId,@PurchaseAmount,@LessAmount,@PaymentAmount,@Status,@SubSubPnoId,@UserName,@PaymentStatus,@BranchId,@EntryDate,@EntryTime)";
                var cmd2 = new SqlCommand(query2, Con, _trans);
                cmd2.Parameters.Clear();
                cmd2.Parameters.AddWithValue("@TrNo", trNo);
                cmd2.Parameters.AddWithValue("@TrDate", aModel.InvoiceDate.ToString("yyyy-MM-dd"));
                cmd2.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                cmd2.Parameters.AddWithValue("@InvoiceDate", aModel.InvoiceDate.ToString("yyyy-MM-dd"));
                cmd2.Parameters.AddWithValue("@ComPanyId", comid);
                cmd2.Parameters.AddWithValue("@PurchaseAmount", aModel.TotalPrice);
                cmd2.Parameters.AddWithValue("@LessAmount", 0);
                cmd2.Parameters.AddWithValue("@PaymentAmount", 0);
                cmd2.Parameters.AddWithValue("@Status", "Opening Balance");
                cmd2.Parameters.AddWithValue("@SubSubPnoId", "65");
                cmd2.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd2.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.UserName, _trans));
                cmd2.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                cmd2.Parameters.AddWithValue("@PaymentStatus", aModel.PaymentAmt > 0 ? 2 : 1);
                cmd2.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd2.ExecuteNonQuery();


                _trans.Commit();
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



        public Task<string> Update(PharCompanyModel aModel)
        {
            try
            {
                const string query = @"UPDATE tbl_PHAR_COMPANY SET  Name=@Name,Address=@Address,Phone=@Phone,EntryDate=@EntryDate,UserName=@UserName 
                                        WHERE Id=@Id";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", aModel.Id);
                cmd.Parameters.AddWithValue("@Name", aModel.Name);
               // cmd.Parameters.AddWithValue("@Name", aModel.);
                cmd.Parameters.AddWithValue("@Address", aModel.Address);
                cmd.Parameters.AddWithValue("@Phone", aModel.Phone);
                cmd.Parameters.AddWithValue("@EntryDate", aModel.EntryDate);
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
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


        public List<PharCompanyModel> GetCompanyProductList(int param)
        {
            try
            {
                var lists = new List<PharCompanyModel>();
                string query = "";

                if (param != 0)
                {
                    query = @"SELECT Id,Name,Address,Phone,EntryDate,UserName,Code,Valid
							From tbl_PHAR_COMPANY WHERE Id=@Param";
                }
                else
                {
                    query = @"SELECT Id,Name,Address,Phone,EntryDate,UserName,Code,Valid
							From tbl_PHAR_COMPANY";
                }
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Param", param);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new PharCompanyModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["Name"].ToString(),
                        Address = rdr["Address"].ToString(),
                        Phone = rdr["Phone"].ToString(),
                        EntryDate = Convert.ToDateTime(rdr["EntryDate"]),
                        UserName = rdr["UserName"].ToString(),
                        Code = rdr["Code"].ToString(),
                        Valid= Convert.ToInt32(rdr["Status"]),
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
                return new List<PharCompanyModel>();
            }
        }


        public string Delete(int id)
        {
            try
            {
                string msg = "";
                const string query = @"DELETE FROM tbl_PHAR_COMPANY WHERE Id=@Id";
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
        public List<PharCompanyModel> GetCompanyList(int param)
        {
            try
            {
                var lists = new List<PharCompanyModel>();
                string query = "";

                if (param != 0)
                {
                    query = @"SELECT Id,Name,Address,Phone,EntryDate,UserName,Code,Valid
							From tbl_PHAR_COMPANY WHERE Id=@Param";
                }
                else
                {
                    query = @"SELECT Id,Name,Address,Phone,EntryDate,UserName,Code,Valid
							From tbl_PHAR_COMPANY";
                }
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Param", param);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new PharCompanyModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["Name"].ToString(),
                        Address = rdr["Address"].ToString(),
                        Phone = rdr["Phone"].ToString(),
                        EntryDate = Convert.ToDateTime(rdr["EntryDate"]),
                        UserName = rdr["UserName"].ToString(),
                        Code = rdr["Code"].ToString(),
                        Valid = Convert.ToInt32(rdr["Valid"]),
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
                return new List<PharCompanyModel>();
            }
        }

        public List<PharCompanyModel> GetCompanyListByName(string searchString)
        {
            try
            {
                string condition = "";
                var lists = new List<PharCompanyModel>();
                string query = "";
                if (searchString != "0") { condition = "WHERE (Name) LIKE '%' + '" + searchString + "' + '%' "; }
                query = @"SELECT Id,Name,Address,Phone,EntryDate,UserName
							From tbl_PHAR_COMPANY " + condition + " ";

                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new PharCompanyModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["Name"].ToString(),
                        Address = rdr["Address"].ToString(),
                        Phone = rdr["Phone"].ToString(),
                        EntryDate = Convert.ToDateTime(rdr["EntryDate"]),
                        UserName = rdr["UserName"].ToString(),
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
                return new List<PharCompanyModel>();
            }
        }










    }
}