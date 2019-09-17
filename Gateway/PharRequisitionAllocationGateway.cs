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
    public class PharRequisitionAllocationGateway : DbConnection
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
Allocationby=@Allocationby,AllocationDate=@AllocationDate,AllocationTime=@AllocationTime,UserName=@UserName,UserDtls=@UserDtls,AllocationNote=@AllocationNote,Status=@Status
    WHERE ReqNo=@ReqNo ";
                var cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ReqNo", aModel.ElementAt(0).ReqNo);
                cmd.Parameters.AddWithValue("@Allocationby", aModel.ElementAt(0).Allocationby);
                cmd.Parameters.AddWithValue("@AllocationDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@AllocationTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
              //  cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));
                cmd.Parameters.AddWithValue("@UserDtls", aModel.ElementAt(0).UserDtls);
                cmd.Parameters.AddWithValue("@AllocationNote", aModel.ElementAt(0).AllocationNote);
                cmd.Parameters.AddWithValue("@Status", 2);
                cmd.ExecuteNonQuery();

               
                for (int i=0; i<aModel.Count; i++)
                {

                    const string lcQuery = @"UPDATE  tbl_PHAR_STOCK_REQUISITION_DETAIL SET  AllocationQty=@AllocationQty WHERE MasterId=@MasterId AND ProductId=@ProductId";
                      var  cmd2 = new SqlCommand(lcQuery, Con, _trans);
                        cmd2.Parameters.Clear();
                        cmd2.Parameters.AddWithValue("@MasterId", aModel.ElementAt(i).MasterId);
                        cmd2.Parameters.AddWithValue("@ProductId", aModel.ElementAt(i).ProductId);
                        cmd2.Parameters.AddWithValue("@AllocationQty", aModel.ElementAt(i).AllocationQty);
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


        
        public List<PharStockRequisitionModel> GetPharRequisitionList(string searchString,int status,string userName)
        {
            try
            {

                int branchId = 0;
                branchId = GetBranchIdByuserName(userName);
                string condition = "";
                var lists = new List<PharStockRequisitionModel>();
                string query = "";
                if (searchString != "0") { condition = " AND (ReqNo) LIKE '%' + '" + searchString + "' + '%' "; }

                query = @"SELECT a.Id,a.ReqNo,a.ReqDate,a.ReqBy,a.ReqNote,a.ReqTime,b.SubsubPNo
	FROM tbl_PHAR_STOCK_REQUISITION as a
	LEFT JOIN project as b on a.DeptId=b.IdNo WHERE a.BranchId="+branchId+" AND a.Status=" + status + " " + condition + " ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new PharStockRequisitionModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        ReqNo = rdr["ReqNo"].ToString(),
                        ReqDate = Convert.ToDateTime(rdr["ReqDate"]),
                        ReqBy = rdr["ReqBy"].ToString(),
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
                return new List<PharStockRequisitionModel>();
            }
        }


        public List<PharStockRequisitionModel> GetPharRequisitionDetailList(string searchString, string userName)
        
        {
            try
            {
               
                
                string condition = "";
                var lists = new List<PharStockRequisitionModel>();
                string query = "";
                int branchId = 0;
                int subSubPnoId = 65;
                branchId = GetBranchIdByuserName(userName);

                query = @"SELECT a.Id,a.ReqNo,d.BranchId,a.ReqDate,a.DeptId,b.MasterId,b.Unite,b.ReqQty,b.AllocationQty,b.ApproveQty,b.DisburseQty,	
                    c.Name,c.Id	as ItemId,e.SubsubPNo,d.SubSubPnoId as StockPno,ISNULL (SUM(d.InQty-d.OutQty+d.RtnQty+d.BonusQty-d.PRtnQty),0) AS BalQty FROM tbl_PHAR_STOCK_REQUISITION AS a LEFT JOIN tbl_PHAR_STOCK_REQUISITION_DETAIL AS b ON a.Id=b.MasterId LEFT JOIN tbl_PHAR_PRODUCT AS c ON c.Id=b.ProductId LEFT JOIN tbl_PHAR_STOCKLEDGER AS d ON d.ItemId=b.ProductId LEFT JOIN project as e ON e.IdNo=a.DeptId GROUP BY a.Id,a.ReqNo,a.ReqDate,b.MasterId,b.Unite,b.ReqQty,b.AllocationQty,b.ApproveQty,
                    b.DisburseQty,c.Name,c.Id,e.SubsubPNo,a.DeptId,d.SubSubPnoId,d.BranchId 
                    HAVING  d.SubSubPnoId=" + subSubPnoId+" AND d.BranchId=" + branchId + " AND ReqNo=" + searchString + "  ";

             //   LIKE '%' + '181' + '%' 
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new PharStockRequisitionModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        ReqNo = rdr["ReqNo"].ToString(),
                        ReqDate = Convert.ToDateTime(rdr["ReqDate"]),
                        Unit = rdr["Unite"].ToString(),
                        ReqQty = Convert.ToDouble(rdr["ReqQty"]),
                        AllocationQty = Convert.ToDouble(rdr["AllocationQty"]),
                        ApproveQty = Convert.ToDouble(rdr["ApproveQty"]),
                        DisburseQty = Convert.ToDouble(rdr["DisburseQty"]),
                        Name = rdr["Name"].ToString(),
                        ProductId = Convert.ToInt32(rdr["ItemId"]),
                        BalQty = Convert.ToDouble(rdr["BalQty"]),
                        //ReqQty = Convert.ToDouble(rdr["ReqQty"]),
                        DeptName = rdr["SubsubPNo"].ToString(),
                        DeptId = Convert.ToInt32(rdr["DeptId"]),



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
                return new List<PharStockRequisitionModel>();
            }
        }





    }
}