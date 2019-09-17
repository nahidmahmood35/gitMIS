using System;
using System.Threading;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Gateway
{
    public class InvStokeProductRegistrationGatway : DbConnection
    {
        private SqlTransaction _trans;
        public Task<string> Save(InvStockProductRegistrationModel aModel)
        {
            try
            {
                string msg = "";
                const string query = @"INSERT INTO tbl_INVSTOCK_SalesProductList (ProductName, ProductCategoryId, Unit, Valid, UserName, RackNumber, cellNumber, DepreciationMethodId, DepreciationAmount, DepreciationAmountType, AssetType, MinimumQuantity, EconomicQuantity) 
                                                                          VALUES (@ProductName, @ProductCategoryId, @Unit, @Valid, @UserName, @RackNumber, @cellNumber, @DepreciationMethodId, @DepreciationAmount, @DepreciationAmountType, @AssetType, @MinimumQuantity, @EconomicQuantity)";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ProductName", aModel.ProductName);
                cmd.Parameters.AddWithValue("@ProductCategoryId", aModel.ProductCategory);
                cmd.Parameters.AddWithValue("@Unit", aModel.Unit);
                cmd.Parameters.AddWithValue("@Valid", 1);
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@RackNumber", aModel.rackNumber);
                cmd.Parameters.AddWithValue("@cellNumber", aModel.cellNumber);
                cmd.Parameters.AddWithValue("@DepreciationMethodId", aModel.DepreciationMethodId);
                cmd.Parameters.AddWithValue("@DepreciationAmount", aModel.DepreciationAmount);
                cmd.Parameters.AddWithValue("@DepreciationAmountType", aModel.DepreciationAmountType);
                cmd.Parameters.AddWithValue("@AssetType", aModel.AssetType);
                cmd.Parameters.AddWithValue("@MinimumQuantity", aModel.MinimumQuantity);
                cmd.Parameters.AddWithValue("@EconomicQuantity", aModel.EconomicQuantity);
                

                int rtn = cmd.ExecuteNonQuery();
                msg = rtn == 1 ? "Saved Success" : "Saved Failed";
                Con.Close();
               // return msg;
                return Task.FromResult(msg); 
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


        public Task<string> Update(InvStockProductRegistrationModel aModel)
        {
            try
            {
                string msg = "";
                const string query = @"Update tbl_INVSTOCK_SalesProductList SET ProductName=@ProductName,ProductCategoryId=@ProductCategoryId,Unit=@Unit,Valid=@Valid,UserName=@UserName ,
                                       RackNumber=@RackNumber, cellNumber=@cellNumber, DepreciationMethodId=@DepreciationMethodId, DepreciationAmount=@DepreciationAmount, DepreciationAmountType=@DepreciationAmountType, AssetType=@AssetType, MinimumQuantity=@MinimumQuantity, EconomicQuantity=@EconomicQuantity Where Id=@Id";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", aModel.Id);
                cmd.Parameters.AddWithValue("@ProductName", aModel.ProductName);
                cmd.Parameters.AddWithValue("@ProductCategoryId", aModel.ProductCategory);
                cmd.Parameters.AddWithValue("@Unit", aModel.Unit);
                cmd.Parameters.AddWithValue("@Valid", 1);
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@RackNumber", aModel.rackNumber);
                cmd.Parameters.AddWithValue("@cellNumber", aModel.cellNumber);
                cmd.Parameters.AddWithValue("@DepreciationMethodId", aModel.DepreciationMethodId);
                cmd.Parameters.AddWithValue("@DepreciationAmount", aModel.DepreciationAmount);
                cmd.Parameters.AddWithValue("@DepreciationAmountType", aModel.DepreciationAmountType);
                cmd.Parameters.AddWithValue("@AssetType", aModel.AssetType);
                cmd.Parameters.AddWithValue("@MinimumQuantity", aModel.MinimumQuantity);
                cmd.Parameters.AddWithValue("@EconomicQuantity", aModel.EconomicQuantity);

                int rtn = cmd.ExecuteNonQuery();
                msg = rtn == 1 ? "Saved Success" : "Saved Failed";
                Con.Close();
                // return msg;
                return Task.FromResult(msg);
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


        public List<InvStockProductRegistrationModel> GetProductById(int Id)
        {
            try
            {

                var lists = new List<InvStockProductRegistrationModel>();
                string query = "";

                query = @"Select Id, ProductName, ProductCategoryId, ProductCategoryName, UnitPrice, Unit, Valid, UserName, RackNumber, cellNumber, DepreciationMethodId, 
                            DepreciationMethodName, DepreciationAmount, DepreciationAmountType, AssetType, MinimumQuantity, EconomicQuantity from VW_INVSTOCK_SalesProductList where Id=" + Id + "";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    lists.Add(new InvStockProductRegistrationModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        ProductName = rdr["ProductName"].ToString(),
                        ProductCategory = Convert.ToInt32(rdr["ProductCategoryId"]),
                        UnitPrice = Convert.ToDouble(rdr["UnitPrice"]),
                        Unit = rdr["Unit"].ToString(),
                        AssetType = Convert.ToInt32(rdr["AssetType"]),
                        DepreciationMethodId = Convert.ToInt32(rdr["DepreciationMethodId"]),
                        DepreciationAmount = Convert.ToDouble(rdr["DepreciationAmount"]),
                        DepreciationAmountType = Convert.ToInt32(rdr["DepreciationAmountType"]),
                        MinimumQuantity = Convert.ToDouble(rdr["MinimumQuantity"]),
                        EconomicQuantity = Convert.ToDouble(rdr["EconomicQuantity"]),
                        rackNumber = rdr["RackNumber"].ToString(),
                        cellNumber = rdr["cellNumber"].ToString(),
                       
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
                return new List<InvStockProductRegistrationModel>();
            }
        }


        public List<IdNameForDropdownModel> GetIdCasCadeDropDown(string query)
        {
            var lists = new List<IdNameForDropdownModel>();
            try
            {
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new IdNameForDropdownModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["Name"].ToString(),
                        // CatId = Convert.ToInt32(rdr["CatId"])
                    });
                }
                rdr.Close();
                Con.Close();
                return lists;
            }
            catch (Exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                throw;
            }
        }


        public Task<int> SaveWithReturnId(string tableName, string name)
        {
            try
            {
                string query = @"INSERT INTO " + tableName + " (Name) OUTPUT INSERTED.ID VALUES (@Name)";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Name", name);
                int rtn = Convert.ToInt32(cmd.ExecuteScalar());
                Con.Close();
                return Task.FromResult(rtn);
            }
            catch (Exception ex)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return Task.FromResult(0);
            }
        }

        public List<InvStockProductRegistrationModel> GetProductList()
        {
            try
            {

                var lists = new List<InvStockProductRegistrationModel>();
                string query = "";

                query = @"Select a.Id,a.ProductName,a.ProductCategoryId,a.UnitPrice,a.Unit,b.Name 
                        from tbl_INVSTOCK_SalesProductList AS a  inner join tbl_INVSTOCK_ProductCategory AS b ON a.ProductCategoryId = b.Id where Valid =1";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                
                while (rdr.Read())
                {
                    lists.Add(new InvStockProductRegistrationModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        ProductName = rdr["ProductName"].ToString(),
                        ProductCategory = Convert.ToInt32(rdr["ProductCategoryId"]),
                        UnitPrice = Convert.ToDouble(rdr["UnitPrice"]),
                        Unit = rdr["Unit"].ToString(),
                        ProductCategoryName = rdr["Name"].ToString(),
                     
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
                return new List<InvStockProductRegistrationModel>();
            }
        }
    }
}