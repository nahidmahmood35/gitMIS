using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Gateway
{
    public class InventoryStockRequsitionReqGateway : DbConnection
    {
        public List<InvStockRequisitionModel> GetProductList(string searchString)
        {
            try
            {
                string condition = "";
                var lists = new List<InvStockRequisitionModel>();
                string query = "";

                if (searchString != "0") { condition = " WHERE (CONVERT(nvarchar(10), Id)+ProductName) LIKE '%' + '" + searchString + "' + '%' "; }

                query = @"Select Id,ProductName,Unit from tbl_INVSTOCK_SalesProductList " + condition + " ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvStockRequisitionModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        ProductName = rdr["ProductName"].ToString(),
                        Unit = rdr["Unit"].ToString(),
                  

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
                return new List<InvStockRequisitionModel>();
            }
        }


        private SqlTransaction _trans;
        public Task<string> Save(List<InvStockRequisitionModel> aModel)
        {
            try
            {
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();
                string invoiceNo = GetAutoIncrementNumberFromStoreProcedure(14, _trans);
                const string query = @"INSERT INTO tbl_INVSTOCK_REQUISITION (ReqNo,ReqBy,ReqDate,ReqTime,UserName,BranchId,UserDtls,ReqNote,DeptId,Status)  
                                        OUTPUT INSERTED.ID VALUES (@ReqNo,@ReqBy,@ReqDate,@ReqTime,@UserName,@BranchId,@UserDtls,@ReqNote,@DeptId,@Status)";
                var cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@ReqNo", invoiceNo);
                cmd.Parameters.AddWithValue("@ReqBy", aModel.ElementAt(0).ReqBy);
                cmd.Parameters.AddWithValue("@ReqDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ReqTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));
                cmd.Parameters.AddWithValue("@UserDtls", aModel.ElementAt(0).UserDtls);
                cmd.Parameters.AddWithValue("@ReqNote", aModel.ElementAt(0).ReqNote);
                cmd.Parameters.AddWithValue("@DeptId", aModel.ElementAt(0).DeptId);
                cmd.Parameters.AddWithValue("@Status", 1);
                var invMasterId = (int)cmd.ExecuteScalar();

                aModel.ForEach(z => z.MasterId = invMasterId);
                DataTable dt = ConvertListDataTable(aModel);
                var objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_INVSTOCK_REQUISITION_DETAIL" };
                objbulk.ColumnMappings.Add("MasterId", "MasterId");  // model----table
                objbulk.ColumnMappings.Add("ProductId", "ProductId");
                objbulk.ColumnMappings.Add("Unit", "Unite");
                objbulk.ColumnMappings.Add("ReqQty", "ReqQty");
                objbulk.WriteToServer(dt);
                _trans.Commit();
                Con.Close();
                return Task.FromResult("Save Successful");
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    _trans.Rollback();
                    Con.Close();
                }
                return Task.FromResult(exception.Message);
            }

        }

        public Task<string> Update(List<InvStockRequisitionModel> aModel)
        {
            try
            {
                string ReqNo = aModel.ElementAt(0).ReqNo;
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();
                //string invoiceNo = GetAutoIncrementNumberFromStoreProcedure(14, _trans);
//                const string query = @"INSERT INTO tbl_INVSTOCK_REQUISITION (ReqNo,ReqBy,ReqDate,ReqTime,UserName,BranchId,UserDtls,ReqNote,DeptId,Status)  
//                                        OUTPUT INSERTED.ID VALUES (@ReqNo,@ReqBy,@ReqDate,@ReqTime,@UserName,@BranchId,@UserDtls,@ReqNote,@DeptId,@Status)";
                string query = @"UPDATE tbl_INVSTOCK_REQUISITION SET ReqBy=@ReqBy,ReqDate=@ReqDate,ReqTime=@ReqTime,UserName=@UserName,BranchId=@BranchId,UserDtls=@UserDtls,ReqNote=@ReqNote,DeptId=@DeptId,Status=@Status OUTPUT INSERTED.ID  where ReqNo= " + ReqNo + " ";
                var cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@ReqNo", ReqNo);
                cmd.Parameters.AddWithValue("@ReqBy", aModel.ElementAt(0).ReqBy);
                cmd.Parameters.AddWithValue("@ReqDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ReqTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));
                cmd.Parameters.AddWithValue("@UserDtls", aModel.ElementAt(0).UserDtls);
                cmd.Parameters.AddWithValue("@ReqNote", aModel.ElementAt(0).ReqNote);
                cmd.Parameters.AddWithValue("@DeptId", aModel.ElementAt(0).DeptId);
                cmd.Parameters.AddWithValue("@Status", 1);
                var invMasterId = (int)cmd.ExecuteScalar();

                string query1 = @"DELETE FROM tbl_INVSTOCK_REQUISITION_DETAIL Where MasterId=" + invMasterId + "";
                var cmd1 = new SqlCommand(query1, Con, _trans);
                cmd1.ExecuteNonQuery();

                aModel.ForEach(z => z.MasterId = invMasterId);
                DataTable dt = ConvertListDataTable(aModel);
                var objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_INVSTOCK_REQUISITION_DETAIL" };
                objbulk.ColumnMappings.Add("MasterId", "MasterId");  // model----table
                objbulk.ColumnMappings.Add("ProductId", "ProductId");
                objbulk.ColumnMappings.Add("Unit", "Unite");
                objbulk.ColumnMappings.Add("ReqQty", "ReqQty");
                objbulk.WriteToServer(dt);
                _trans.Commit();
                Con.Close();
                return Task.FromResult("Save Successful");
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    _trans.Rollback();
                    Con.Close();
                }
                return Task.FromResult(exception.Message);
            }

        }

        //public List<InvStockRequisitionModel> GetRequisitionList(string searchString, int status, string userName)
        public List<InvStockRequisitionModel> GetRequisitionList(string userName)
        {
            try
            {
                int status = 1;
                int branchId = 0;
                branchId = GetBranchIdByuserName(userName);
                var lists = new List<InvStockRequisitionModel>();
                string query = "";
             
                query =@"Select a.Id,a.ReqNo,a.ReqDate,a.ReqNote,b.SubsubPNo from tbl_INVSTOCK_REQUISITION AS a inner join project AS b ON a.DeptId=b.IdNo
                where a.BranchId=" + branchId + " AND a.UserName=" + userName + " AND a.Status=" + status + "";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvStockRequisitionModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        ReqNo = rdr["ReqNo"].ToString(),
                        ReqDate = Convert.ToDateTime(rdr["ReqDate"]),
                        ReqNote = rdr["ReqNote"].ToString(),
                        DeptName = rdr["SubsubPNo"].ToString(),


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
                return new List<InvStockRequisitionModel>();
            }
        }

        public Task<string> SaveRequsitionUse(List<InvStockRequisitionModel> aModel)
        {
            try
            {
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();
                string invoiceNo = GetAutoIncrementNumberFromStoreProcedure(14, _trans);
                const string query = @"INSERT INTO tbl_INVSTOCK_REQUISITION (ReqNo,ReqBy,ReqDate,ReqTime,UserName,BranchId,UserDtls,ReqNote,DeptId,Status)  
                                        OUTPUT INSERTED.ID VALUES (@ReqNo,@ReqBy,@ReqDate,@ReqTime,@UserName,@BranchId,@UserDtls,@ReqNote,@DeptId,@Status)";
                var cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@ReqNo", invoiceNo);
                cmd.Parameters.AddWithValue("@ReqBy", aModel.ElementAt(0).ReqBy);
                cmd.Parameters.AddWithValue("@ReqDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ReqTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));
                cmd.Parameters.AddWithValue("@UserDtls", aModel.ElementAt(0).UserDtls);
                cmd.Parameters.AddWithValue("@ReqNote", aModel.ElementAt(0).ReqNote);
                cmd.Parameters.AddWithValue("@DeptId", aModel.ElementAt(0).DeptId);
                cmd.Parameters.AddWithValue("@Status", 5);
                var invMasterId = (int)cmd.ExecuteScalar();

                aModel.ForEach(z => z.MasterId = invMasterId);
                DataTable dt = ConvertListDataTable(aModel);
                var objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_INVSTOCK_REQUISITION_DETAIL" };
                objbulk.ColumnMappings.Add("MasterId", "MasterId");  // model----table
                objbulk.ColumnMappings.Add("ProductId", "ProductId");
                objbulk.ColumnMappings.Add("Unit", "Unite");
                objbulk.ColumnMappings.Add("ReqQty", "ReqQty");
                objbulk.WriteToServer(dt);
                _trans.Commit();
                Con.Close();
                return Task.FromResult("Save Successful");
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    _trans.Rollback();
                    Con.Close();
                }
                return Task.FromResult(exception.Message);
            }

        }

        public Task<string> UpdateRequsitionUse(List<InvStockRequisitionModel> aModel)
        {
            try
            {
                string ReqNo = aModel.ElementAt(0).ReqNo;
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();
                //string invoiceNo = GetAutoIncrementNumberFromStoreProcedure(14, _trans);
                //                const string query = @"INSERT INTO tbl_INVSTOCK_REQUISITION (ReqNo,ReqBy,ReqDate,ReqTime,UserName,BranchId,UserDtls,ReqNote,DeptId,Status)  
                //                                        OUTPUT INSERTED.ID VALUES (@ReqNo,@ReqBy,@ReqDate,@ReqTime,@UserName,@BranchId,@UserDtls,@ReqNote,@DeptId,@Status)";
                string query = @"UPDATE tbl_INVSTOCK_REQUISITION SET ReqBy=@ReqBy,ReqDate=@ReqDate,ReqTime=@ReqTime,UserName=@UserName,BranchId=@BranchId,UserDtls=@UserDtls,ReqNote=@ReqNote,DeptId=@DeptId,Status=@Status OUTPUT INSERTED.ID  where ReqNo= " + ReqNo + " ";
                var cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@ReqNo", ReqNo);
                cmd.Parameters.AddWithValue("@ReqBy", aModel.ElementAt(0).ReqBy);
                cmd.Parameters.AddWithValue("@ReqDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ReqTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));
                cmd.Parameters.AddWithValue("@UserDtls", aModel.ElementAt(0).UserDtls);
                cmd.Parameters.AddWithValue("@ReqNote", aModel.ElementAt(0).ReqNote);
                cmd.Parameters.AddWithValue("@DeptId", aModel.ElementAt(0).DeptId);
                cmd.Parameters.AddWithValue("@Status", 5);
                var invMasterId = (int)cmd.ExecuteScalar();

                string query1 = @"DELETE FROM tbl_INVSTOCK_REQUISITION_DETAIL Where MasterId=" + invMasterId + "";
                var cmd1 = new SqlCommand(query1, Con, _trans);
                cmd1.ExecuteNonQuery();

                aModel.ForEach(z => z.MasterId = invMasterId);
                DataTable dt = ConvertListDataTable(aModel);
                var objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_INVSTOCK_REQUISITION_DETAIL" };
                objbulk.ColumnMappings.Add("MasterId", "MasterId");  // model----table
                objbulk.ColumnMappings.Add("ProductId", "ProductId");
                objbulk.ColumnMappings.Add("Unit", "Unite");
                objbulk.ColumnMappings.Add("ReqQty", "ReqQty");
                objbulk.WriteToServer(dt);
                _trans.Commit();
                Con.Close();
                return Task.FromResult("Save Successful");
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    _trans.Rollback();
                    Con.Close();
                }
                return Task.FromResult(exception.Message);
            }

        }
    }
}