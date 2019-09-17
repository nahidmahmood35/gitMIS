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
    public class PharRequisitionApproveGateway : DbConnection
    {
        private SqlTransaction _trans;
        public Task<string> Update(List<PharStockRequisitionModel> aModel)
        {
            try
            {
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();
                const string query = @"UPDATE  tbl_PHAR_STOCK_REQUISITION SET
ApproveBy=@ApproveBy,ApproveDate=@ApproveDate,ApproveTime=@ApproveTime,UserName=@UserName,UserDtls=@UserDtls,ApproveNote=@ApproveNote,Status=@Status
    WHERE ReqNo=@ReqNo ";
                var cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ReqNo", aModel.ElementAt(0).ReqNo);
                cmd.Parameters.AddWithValue("@ApproveBy", aModel.ElementAt(0).ApproveBy);
                cmd.Parameters.AddWithValue("@ApproveDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ApproveTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd.Parameters.AddWithValue("@UserDtls", aModel.ElementAt(0).UserDtls);
                cmd.Parameters.AddWithValue("@ApproveNote", aModel.ElementAt(0).ApproveNote);
                cmd.Parameters.AddWithValue("@Status", 3);
                cmd.ExecuteNonQuery();


                for (int i = 0; i < aModel.Count; i++)
                {

                    const string lcQuery = @"UPDATE  tbl_PHAR_STOCK_REQUISITION_DETAIL SET  ApproveQty=@ApproveQty WHERE MasterId=@MasterId AND ProductId=@ProductId";
                    var cmd2 = new SqlCommand(lcQuery, Con, _trans);
                    cmd2.Parameters.Clear();
                    cmd2.Parameters.AddWithValue("@MasterId", aModel.ElementAt(i).MasterId);
                    cmd2.Parameters.AddWithValue("@ProductId", aModel.ElementAt(i).ProductId);
                    cmd2.Parameters.AddWithValue("@ApproveQty", aModel.ElementAt(i).ApproveQty);
                    cmd2.ExecuteNonQuery();
                }


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