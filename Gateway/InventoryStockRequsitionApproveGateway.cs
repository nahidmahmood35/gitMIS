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
    public class InventoryStockRequsitionApproveGateway : DbConnection
    {
        private SqlTransaction _trans;
        public Task<string> Update(List<InvStockRequisitionModel> aModel)
        {
            try
            {
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();
                const string query = @"UPDATE  tbl_INVSTOCK_REQUISITION SET
                                ApproveBy=@ApproveBy,ApproveDate=@ApproveDate,ApproveTime=@ApproveTime,UserName=@UserName,UserDtls=@UserDtls,ApproveNote=@ApproveNote,Status=@Status
                                WHERE ReqNo=@ReqNo ";
                var cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ReqNo", aModel.ElementAt(0).ReqNo);
                cmd.Parameters.AddWithValue("@ApproveBy", aModel.ElementAt(0).UserName);
                cmd.Parameters.AddWithValue("@ApproveDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ApproveTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                //  cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));
                cmd.Parameters.AddWithValue("@UserDtls", aModel.ElementAt(0).UserDtls);
                cmd.Parameters.AddWithValue("@ApproveNote", aModel.ElementAt(0).ApproveNote);
                cmd.Parameters.AddWithValue("@Status", 3);
                cmd.ExecuteNonQuery();


                for (int i = 0; i < aModel.Count; i++)
                {

                    const string lcQuery = @"UPDATE  tbl_INVSTOCK_REQUISITION_DETAIL SET  ApproveQty=@ApproveQty WHERE MasterId=@MasterId AND ProductId=@ProductId";
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
        public List<InvStockRequisitionModel> GetInventoryRequisitionList(string userName)
        {
            try
            {

                int branchId = 0;
                int status = 1;
                branchId = GetBranchIdByuserName(userName);
                string condition = "";
                var lists = new List<InvStockRequisitionModel>();
                string query = "";
                //if (searchString != "0") { condition = " AND (ReqNo) LIKE '%' + '" + searchString + "' + '%' "; }

                //                query = @"SELECT a.Id,a.ReqNo,a.ReqDate,a.ReqBy,a.ReqNote,a.ReqTime,b.SubsubPNo
                //	                    FROM tbl_INVSTOCK_REQUISITION as a
                //	                    LEFT JOIN project as b on a.DeptId=b.IdNo WHERE a.BranchId=" + branchId + " AND a.Status=" + status + " ";
                query = @"  SELECT a.Id,a.ReqNo,a.ReqDate,a.ReqBy,a.ReqNote,a.ReqTime,b.SubsubPNo,case when a.Status =2 then 'New' else 'Old' end as Status
	                    FROM tbl_INVSTOCK_REQUISITION as a
	                    LEFT JOIN project as b on a.DeptId=b.IdNo WHERE a.BranchId=" + branchId + " AND a.Status IN (2,3) ";
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
                        ReqBy = rdr["ReqBy"].ToString(),
                        ReqNote = rdr["ReqNote"].ToString(),
                        DeptName = rdr["SubsubPNo"].ToString(),
                        Status = rdr["Status"].ToString(),

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

        public List<InvStockRequisitionModel> GetInventoryRequisitionDetailList(string searchString, string userName)
        {
            try
            {


                string condition = "";
                var lists = new List<InvStockRequisitionModel>();
                string query = "";
                int branchId = 0;
                int subSubPnoId = 65;
                branchId = GetBranchIdByuserName(userName);

                query = @"Select a.Id AS MasterId,a.ApproveNote,a.AllocationNote,a.BranchId,a.ReqNo,a.ReqDate,a.DeptId,a.ReqNote,b.Unite,b.ReqQty,b.AllocationQty,b.ApproveQty,b.DisburseQty,b.ProductId,c.ProductName,d.Pno,ISNULL(e.Balance, 0) AS Balance,case when a.Status =2 then 'New' else 'Old' end as Status,
(SELECT Isnull(SUM(InQty-OutQty+RtnQty),0)  FROM tbl_INVSTOCK_StockLedger WHERE PnoId=a.DeptId AND ItemId=b.ProductId) AS StockInHand,
Isnull((SELECT Top 1 EntryDate  FROM tbl_INVSTOCK_StockLedger WHERE PnoId=a.DeptId AND ItemId=b.ProductId AND InQty>0 AND Status='IN' Order By EntryDate Desc),0) AS LastIn
from tbl_INVSTOCK_REQUISITION AS a inner join tbl_INVSTOCK_REQUISITION_DETAIL AS b ON a.Id=b.MasterId
inner Join tbl_INVSTOCK_SalesProductList AS c ON c.Id=b.ProductId
inner join project AS d ON d.IdNo=a.DeptId
left join VW_INVENTORY_CURRENT_STOCK AS e ON b.ProductId=e.itemId WHERE a.ReqNo= " + searchString + " AND a.BranchId=" + branchId + "";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvStockRequisitionModel()
                    {
                        Id = Convert.ToInt32(rdr["MasterId"]),
                        ReqNo = rdr["ReqNo"].ToString(),
                        ReqDate = Convert.ToDateTime(rdr["ReqDate"]),
                        DeptId = Convert.ToInt32(rdr["DeptId"]),
                        Unit = rdr["Unite"].ToString(),
                        ReqQty = Convert.ToDouble(rdr["ReqQty"]),
                        AllocationQty = Convert.ToDouble(rdr["AllocationQty"]),
                        ApproveQty = Convert.ToDouble(rdr["ApproveQty"]),
                        DisburseQty = Convert.ToDouble(rdr["DisburseQty"]),
                        ProductId = Convert.ToInt32(rdr["ProductId"]),
                        ProductName = rdr["ProductName"].ToString(),
                        BalQty = Convert.ToDouble(rdr["Balance"]),
                        ReqNote = rdr["ReqNote"].ToString(),
                        Status = rdr["Status"].ToString(),
                        LestIssue = Convert.ToDateTime(rdr["LastIn"]),
                        StockInHand = Convert.ToInt32(rdr["StockInHand"]),
                        AllocationNote = rdr["AllocationNote"].ToString(),
                        ApproveNote = rdr["ApproveNote"].ToString(),

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
    }
}