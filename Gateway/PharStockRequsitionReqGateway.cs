using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Gateway
{
    public class PharStockRequsitionReqGateway : DbConnection
    {
        private SqlTransaction _trans;
        public Task<string> Save(List<PharStockRequisitionModel> aModel)
        {
            try
            {
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();
                string invoiceNo = GetAutoIncrementNumberFromStoreProcedure(12, _trans);
                const string query = @"INSERT INTO tbl_PHAR_STOCK_REQUISITION (ReqNo,ReqBy,ReqDate,ReqTime,UserName,BranchId,UserDtls,ReqNote,DeptId,Status)  
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
                var objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_PHAR_STOCK_REQUISITION_DETAIL" };
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