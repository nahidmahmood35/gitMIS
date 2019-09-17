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
    public class SalesProductGateway : DbConnection
    {
        public string Delete(int id)
        {
            try
            {
                string msg = "";
                const string query = @"DELETE FROM SalesProductList WHERE IdNo=@Id";
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
        //------------------------------
        public List<SalesProductModel> GetSalesProductList(int param)
        {
            try
            {
                var lists = new List<SalesProductModel>();

                string query = "";      // top 200

                if (param !=0)
                {
                    query = @" SELECT a.IdNo,isnull(a.ProductName,0) as ProductName,isnull(a.UnitPrice,0) as UnitPrice ,isnull(a.FinishedPdCode,0) FinishedPdCode,isnull(a.SalesType,0) as SalesType,isnull(a.Unit,0) as Unit,isnull(a.PNO,0) as PNO,isnull(a.Valid,0) as Valid,isnull(a.UserName,0) as UserName,isnull(a.ReminderStock,0) as ReminderStock,isnull(a.Status,0) as Status,isnull(a.MRP,0) as MRP,isnull(a.TP,0) as TP,isnull(a.Type,0) as Type,isnull(a.LastPurchasePrice,0) as LastPurchasePrice,isnull(a.LastPurDate,0) as LastPurDate,isnull(a.GCMId,0) as GCMId,isnull(b.ProductCategoryName,0) as ProductCategoryName,isnull(a.ProductID,0) as ProductID,isnull(c.RowNumber,0) as RowNumber,isnull(d.StoreName,0) as StoreName,isnull(e.RackName,0) as RackName,isnull(f.Name,0) as Name,isnull(a.ProductCategoryId,0) as ProductCategoryId,isnull(a.RowId,0) as RowId,isnull(a.RackId,0) as RackId,isnull(a.StoreId,0) as StoreId,isnull(a.SubSubPNOId,0) as SubSubPNOId
                               FROM SalesProductList a    LEFT JOIN ProductCategory b ON a.ProductCategoryId=b.Id  LEFT JOIN RowInfo		c ON a.RowId=c.Id LEFT JOIN StoreInfo	d ON a.RackId=d.Id
                               LEFT JOIN RackInfo	e ON a.StoreId=e.Id LEFT JOIN tbl_SUB_SUB_PROJECT	f ON a.SubSubPNOId=f.Id WHERE a.IdNo=@param";                  
                                                   
                }
                else
                {
                    query = @" SELECT a.IdNo,isnull(a.ProductName,0) as ProductName,isnull(a.UnitPrice,0) as UnitPrice ,isnull(a.FinishedPdCode,0) FinishedPdCode,isnull(a.SalesType,0) as SalesType,isnull(a.Unit,0) as Unit,isnull(a.PNO,0) as PNO,isnull(a.Valid,0) as Valid,isnull(a.UserName,0) as UserName,isnull(a.ReminderStock,0) as ReminderStock,isnull(a.Status,0) as Status,isnull(a.MRP,0) as MRP,isnull(a.TP,0) as TP,isnull(a.Type,0) as Type,isnull(a.LastPurchasePrice,0) as LastPurchasePrice,isnull(a.LastPurDate,0) as LastPurDate,isnull(a.GCMId,0) as GCMId,isnull(b.ProductCategoryName,0) as ProductCategoryName,isnull(a.ProductID,0) as ProductID,isnull(c.RowNumber,0) as RowNumber,isnull(d.StoreName,0) as StoreName,isnull(e.RackName,0) as RackName,isnull(f.Name,0) as Name,isnull(a.ProductCategoryId,0) as ProductCategoryId,isnull(a.RowId,0) as RowId,isnull(a.RackId,0) as RackId,isnull(a.StoreId,0) as StoreId,isnull(a.SubSubPNOId,0) as SubSubPNOId
                               FROM SalesProductList a    LEFT JOIN ProductCategory b ON a.ProductCategoryId=b.Id  LEFT JOIN RowInfo		c ON a.RowId=c.Id LEFT JOIN StoreInfo	d ON a.RackId=d.Id
                               LEFT JOIN RackInfo	e ON a.StoreId=e.Id LEFT JOIN tbl_SUB_SUB_PROJECT	f ON a.SubSubPNOId=f.Id ";   
                }
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@param", param);
                var rdr = cmd.ExecuteReader();
                
                while (rdr.Read())
                {
                    lists.Add(new SalesProductModel()
                {

                IdNo = Convert.ToInt32(rdr["IdNo"]),
                ProductID = rdr["ProductID"].ToString(),
                ProductName = rdr["ProductName"].ToString(),
                ProductCategory = rdr["ProductCategoryName"].ToString(),
                UnitPrice = Convert.ToDouble(rdr["UnitPrice"]),
                FinishedPdCode = rdr["FinishedPdCode"].ToString(),
                SalesType = rdr["SalesType"].ToString(),
                Unit = rdr["Unit"].ToString(),
                PNO = rdr["PNO"].ToString(),
                Valid = Convert.ToDouble(rdr["Valid"]),
                UserName = rdr["UserName"].ToString(),
                ReminderStock = Convert.ToDouble(rdr["ReminderStock"]),
                Status = Convert.ToDouble(rdr["Status"]),
                MRP = Convert.ToDouble(rdr["MRP"]),
                TP = Convert.ToDouble(rdr["TP"]),
                Type = rdr["Type"].ToString(),
                RackName = rdr["RackName"].ToString(),
                RowNumber = Convert.ToInt32(rdr["RowNumber"].ToString()),
                StoreName = rdr["StoreName"].ToString(),
                SubSubPNO = rdr["Name"].ToString(),
                LastPurchasePrice = Convert.ToDouble(rdr["LastPurchasePrice"]),
                LastPurDate = Convert.ToDateTime(rdr["LastPurDate"]),
                GCMId = Convert.ToInt32(rdr["GCMId"]),
                ProductCategoryId = Convert.ToInt32(rdr["ProductCategoryId"]),
                RackId = Convert.ToInt32(rdr["RackId"]),
                RowId = Convert.ToInt32(rdr["RowId"]),
                StoreId = Convert.ToInt32(rdr["StoreId"]),
                SubSubPNOId = Convert.ToInt32(rdr["SubSubPNOId"]),

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
                return new List<SalesProductModel>();
            }
        }
        //------------------------------
        public List<IdNameForDropdownModel> GetProductCategoryList(int param)
        {
            try
            {
                var lists = new List<IdNameForDropdownModel>();
                string query = "";

                if (param !=0)
                {
                    query = @"SELECT Id,ProductCategoryName
                                FROM ProductCategory WHERE ProductCategory=@Param";
                }
                else
                {
                    query = @"SELECT  Id,ProductCategoryName
                                        FROM ProductCategory ORDER BY ProductCategoryName";
                }
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Param", param);
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    lists.Add(new IdNameForDropdownModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["ProductCategoryName"].ToString(),
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
                return new List<IdNameForDropdownModel>();
            }
        }
        //------------------------------
        public List<IdNameForDropdownModel> GetRackNameList(int param)
        {
            try
            {
                var lists = new List<IdNameForDropdownModel>();
                string query = "";

                if (param != 0)
                {
                    query = @"Select Id,RackName
                                from RackInfo WHERE Id=@Param";
                }
                else
                {
                    query = @"Select  Id,RackName
                                        from RackInfo ORDER BY RackName";
                }
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Param", param);
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    lists.Add(new IdNameForDropdownModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["RackName"].ToString(),
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
                return new List<IdNameForDropdownModel>();
            }
        }
        //------------------------------
        public List<IdNameForDropdownModel> GetRowNumberList(int param)
        {
            try
            {
                var lists = new List<IdNameForDropdownModel>();
                string query = "";

                if (param != 0)
                {
                    query = @"Select Id,RowNumber
                                from RowInfo WHERE Id=@Param";
                }
                else
                {
                    query = @"Select  Id,RowNumber
                                        from RowInfo ORDER BY RowNumber";
                }
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Param", param);
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    lists.Add(new IdNameForDropdownModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["RowNumber"].ToString(),
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
                return new List<IdNameForDropdownModel>();
            }
        }
        //------------------------------
        public List<IdNameForDropdownModel> GetStoreNameList(int param)
        {
            try
            {
                var lists = new List<IdNameForDropdownModel>();
                string query = "";

                if (param != 0)
                {
                    query = @"Select Id,StoreName
                                from StoreInfo WHERE Id=@Param";
                }
                else
                {
                    query = @"Select  Id,StoreName
                                        from StoreInfo ORDER BY StoreName";
                }
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Param", param);
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    lists.Add(new IdNameForDropdownModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["StoreName"].ToString(),
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
                return new List<IdNameForDropdownModel>();
            }
        }
        //------------------------------
        public Task<string> Save(SalesProductModel aModel)
        {
            try
            {
                string msg = "";

                int pid = Convert.ToInt32(GetMaxId("SalesProductList", "ProductID"));
                string str = Convert.ToString(pid);

                const string query = @"INSERT INTO SalesProductList (ProductCategoryId,ProductID,ProductName,Unit,UnitPrice,RackId,RowId,StoreId,ReminderStock,SubSubPNOId) VALUES(@ProductCategoryId,@ProductID,@ProductName,@Unit,@UnitPrice,@RackId,@RowId,@StoreId,@ReminderStock,@SubSubPNOId) ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ProductCategoryId", aModel.ProductCategoryId);
                cmd.Parameters.AddWithValue("@ProductID", aModel.ProductID);
                cmd.Parameters.AddWithValue("@ProductName", aModel.ProductName);
                cmd.Parameters.AddWithValue("@Unit", aModel.Unit);
                cmd.Parameters.AddWithValue("@UnitPrice", aModel.UnitPrice);
                cmd.Parameters.AddWithValue("@RackId", aModel.RackId);
                cmd.Parameters.AddWithValue("@RowId", aModel.RowId);
                cmd.Parameters.AddWithValue("@StoreId", aModel.StoreId);
                cmd.Parameters.AddWithValue("@ReminderStock", aModel.ReminderStock);
                cmd.Parameters.AddWithValue("@SubSubPNOId", aModel.SubSubPNOId);
                cmd.ExecuteNonQuery();
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
        //------------------------------
        public List<IdNameForDropdownModel> GetCostCenterList(int param)
        {
            try
            {
                var lists = new List<IdNameForDropdownModel>();
                string query = "";

                if (param != 0)
                {
                    query = @"Select Id,Name
                                from tbl_SUB_SUB_PROJECT WHERE Id=@Param";
                }
                else
                {
                    query = @"Select  Id,Name
                                        from tbl_SUB_SUB_PROJECT ORDER BY Name";
                }
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Param", param);
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    lists.Add(new IdNameForDropdownModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["Name"].ToString(),
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
                return new List<IdNameForDropdownModel>();
            }
        }
        //------------------------------
        public Task<string> Update(SalesProductModel aModel)
        {
            try
            {
                string msg = "";
               // const string query = @"INSERT INTO SalesProductList (ProductCategory,ProductID,ProductName,Unit,UnitPrice,RackName,RowNumber,StoreName,ReminderStock) VALUES(@ProductCategory,@ProductID,@ProductName,@Unit,@UnitPrice,@RackName,@RowNumber,@StoreName,@ReminderStock) ";
                const string query = @"UPDATE SalesProductList SET ProductCategoryId=@ProductCategoryId,ProductName=@ProductName,Unit=@Unit,UnitPrice=@UnitPrice,RackId=@RackId,RowId=@RowId,StoreId=@StoreId,ReminderStock=@ReminderStock,SubSubPNOId=@SubSubPNOId WHERE IdNo=@IdNo";
               
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@IdNo", aModel.IdNo);
                cmd.Parameters.AddWithValue("@ProductCategoryId", aModel.ProductCategoryId);
                cmd.Parameters.AddWithValue("@ProductID", aModel.ProductID);
                cmd.Parameters.AddWithValue("@ProductName", aModel.ProductName);
                cmd.Parameters.AddWithValue("@Unit", aModel.Unit);
                cmd.Parameters.AddWithValue("@UnitPrice", aModel.UnitPrice);
                cmd.Parameters.AddWithValue("@RackId", aModel.RackId);
                cmd.Parameters.AddWithValue("@RowId", aModel.RowId);
                cmd.Parameters.AddWithValue("@StoreId", aModel.StoreId);
                cmd.Parameters.AddWithValue("@ReminderStock", aModel.ReminderStock);
                cmd.Parameters.AddWithValue("@SubSubPNOId", aModel.SubSubPNOId);
               cmd.ExecuteNonQuery();
                Con.Close();
                return Task.FromResult("Update successful");
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
        //------------------------------
       public string AddNewProductCategory(IdNameForDropdownModel aModel)
       {
           try
           {
               string msg = "";
               const string query = @"INSERT INTO ProductCategory (ProductCategoryName) VALUES(@Name) ";
               Con.Open();
               var cmd = new SqlCommand(query, Con);
               cmd.Parameters.Clear();
               cmd.Parameters.AddWithValue("@Name", aModel.Name);
               int rtn = cmd.ExecuteNonQuery();
               msg = rtn == 1 ? "Save Successful" : "Save Failed";
               Con.Close();
               return msg;
           }
           catch (Exception exception)
           {
               if (Con.State == ConnectionState.Open)
               {
                   Con.Close();
               }
               return exception.Message;
           }
       }

        //------------------------------

       
    }
}