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
    public class PharCompanyRequisitionGateway : DbConnection
    {
        private SqlTransaction _trans;

        public Task<string> Save(List<PharPurchaseModel> aModel)
        {
            try
            {
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();
                string invNo = GetAutoIncrementNumberFromStoreProcedure(8, _trans);
                const string query = @"INSERT INTO tbl_PHAR_COMPANY_REQUISITION (ReqNo,ReqDate,CompanyId,UserName,BranchId) 
OUTPUT INSERTED.ID VALUES (@ReqNo,@ReqDate,@CompanyId,@UserName,@BranchId)";
                var cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ReqNo", invNo);
                cmd.Parameters.AddWithValue("@ReqDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@CompanyId", aModel.ElementAt(0).CompanyId);
                cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));
                var invMasterId = (int)cmd.ExecuteScalar();

                aModel.ForEach(z => z.InvoiceMasterId = invMasterId);
                aModel.ForEach(z => z.InvoiceNo = invNo);
                aModel.ForEach(z => z.EntryDate = DateTime.Now.Date);
                aModel.ForEach(z => z.InvoiceDate = DateTime.Now.Date);
                aModel.ForEach(z => z.EntryTime = DateTime.Now.ToShortTimeString());
                aModel.ForEach(z => z.BranchId = Convert.ToInt32(GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans)));

                DataTable dt = ConvertListDataTable(aModel);
                var objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_PHAR_COMPANY_REQUISITION_DETAIL" };
                objbulk.ColumnMappings.Add("InvoiceMasterId", "MasterId");
                objbulk.ColumnMappings.Add("ItemId", "ProductId");
                objbulk.ColumnMappings.Add("ReqQty", "ReqQty");
                objbulk.ColumnMappings.Add("BalQty", "BalQty");
                objbulk.ColumnMappings.Add("Tp", "Tp");

                objbulk.WriteToServer(dt);
                _trans.Commit();
                Con.Close();
                return Task.FromResult("Save successful");
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



        public List<PharSalesModel> GetReqProductList(int param, string userName)
        {
            try
            {
                
                var lists = new List<PharSalesModel>();
                string query = "";
                int subSubPno = 65;
                int branchId = 0;
                branchId = GetBranchIdByuserName(userName);
                //if (param != 0) { condition = " && a.CompanyId=" + param + " "; }
                if (param != 0)
                {
                    query = @"select a.ItemId,sum(a.InQty+a.BonusQty-a.OutQty+a.RtnQty-a.PRtnQty) as BalQty,
                            a.SubSubPnoId,a.BranchId,a.Valid,b.Code,b.Name,b.Id,b.ProductUnit,b.Tp,b.SalesPrice,b.ReminderStock,b.GroupId,c.Name as GroupName
                            from tbl_PHAR_STOCKLEDGER as a inner join 
                            tbl_PHAR_PRODUCT as b on a.ItemId=b.Id left join
                            tbl_PHAR_PRODUCT_GROUP as c on  c.Id=b.GroupId
                            group by a.ItemId,a.SubSubPnoId,a.BranchId,a.Valid,b.Code,b.Name,b.Id,b.ProductUnit,b.Tp,b.SalesPrice,b.ReminderStock,b.GroupId,a.CompanyId,c.Name,c.Id
                            having sum(a.InQty+a.BonusQty-a.OutQty+a.RtnQty-a.PRtnQty)<=b.ReminderStock AND a.SubSubPnoId=" + subSubPno + " AND a.BranchId=" + branchId + " AND a.CompanyId=" + param + "";
                }
                else
                {

                    query = @"select a.ItemId,sum(a.InQty+a.BonusQty-a.OutQty+a.RtnQty-a.PRtnQty) as BalQty,
                                a.SubSubPnoId,a.BranchId,a.Valid,b.Code,b.Name,b.Id,b.ProductUnit,b.Tp,b.SalesPrice,b.ReminderStock,b.GroupId,c.Name as GroupName
                                from tbl_PHAR_STOCKLEDGER as a inner join 
                                tbl_PHAR_PRODUCT as b on a.ItemId=b.Id left join
                                tbl_PHAR_PRODUCT_GROUP as c on  c.Id=b.GroupId
                                group by a.ItemId,a.SubSubPnoId,a.BranchId,a.Valid,b.Code,b.Name,b.Id,b.ProductUnit,b.Tp,b.SalesPrice,b.ReminderStock,b.GroupId,a.CompanyId,c.Name,c.Id
                                having sum(a.InQty+a.BonusQty-a.OutQty+a.RtnQty-a.PRtnQty)<=b.ReminderStock  AND a.SubSubPnoId=" + subSubPno + " AND a.BranchId=" + branchId + " ";
                }
              
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new PharSalesModel()
                    {
                       
                        ItemId = Convert.ToInt32(rdr["ItemId"]),
                        Name = rdr["Name"].ToString(),
                        Code = rdr["Code"].ToString(),
                    //  CompanyId = Convert.ToInt32(rdr["CompanyId"]),
                        SalesPrice = Convert.ToDouble(rdr["SalesPrice"]),
                        AvgPurchasePrice = Convert.ToDouble(rdr["Tp"]),
                        Id = Convert.ToInt32(rdr["Id"]),
                        BalQty = Convert.ToDouble(rdr["BalQty"]),
                        SubSubPnoId = Convert.ToInt32(rdr["SubSubPnoId"]),
                        BranchId = Convert.ToInt32(rdr["BranchId"]),
                        Valid = Convert.ToInt32(rdr["Valid"]),
                        ProductUnit = rdr["ProductUnit"].ToString(),
                        ReminderStock =Convert.ToInt32(rdr["ReminderStock"]),
                        GroupId = Convert.ToInt32(rdr["GroupId"]),
                        GroupName = rdr["GroupName"].ToString(),
                        //Id

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
                return new List<PharSalesModel>();
            }
        }


        public List<PharPurchaseModel> GetPharInvoiceList(string searchString, string userName)
        {
            try
            {
                int branchId = 0;
                branchId = GetBranchIdByuserName(userName);
                string condition = "";
                var lists = new List<PharPurchaseModel>();
                string query = "";
                if (searchString != "0") { condition = " AND (InvoiceNo) LIKE '%' + '" + searchString + "' + '%' "; }
                query = @"SELECT a.Id,a.ReqNo,a.ReqDate,b.Name FROM tbl_PHAR_COMPANY_REQUISITION AS a
       LEFT JOIN tbl_PHAR_COMPANY AS b On a.CompanyId=b.Id  WHERE  BranchId=" + branchId + "  " + condition + "  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new PharPurchaseModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        InvoiceNo = rdr["ReqNo"].ToString(),
                        InvoiceDate = Convert.ToDateTime(rdr["ReqDate"]),
                        CompanyName = rdr["Name"].ToString(),
                        
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
                return new List<PharPurchaseModel>();
            }
        }
























    }
}