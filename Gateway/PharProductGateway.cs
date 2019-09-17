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
    public class PharProductGateway : DbConnection
    {
        private SqlTransaction _trans;

        public Task<string> Save(PharProductInfoModel aModel)
        {
            try
            {
                string code = GetMaxId("tbl_PHAR_PRODUCT", "Code");
                Con.Open();
                const string query = @"INSERT INTO tbl_PHAR_PRODUCT (CompanyId,Name,Code,GenericId,ProductUnit,Tp,SalesPrice,ReminderStock,GroupId,RackNo,RowNo,UserName,BranchId) 
										VALUES (@CompanyId,@Name,@Code,@GenericId,@ProductUnit,@Tp,@SalesPrice,@ReminderStock,@GroupId,@RackNo,@RowNo,@UserName,@BranchId)";
               
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CompanyId", aModel.CompanyId);
                cmd.Parameters.AddWithValue("@Name", aModel.Name);
                cmd.Parameters.AddWithValue("@Code", code);
                cmd.Parameters.AddWithValue("@GenericId", aModel.GenericId);
                cmd.Parameters.AddWithValue("@ProductUnit", aModel.ProductUnit);
                cmd.Parameters.AddWithValue("@Tp", aModel.Tp);
                cmd.Parameters.AddWithValue("@SalesPrice", aModel.SalesPrice);
                cmd.Parameters.AddWithValue("@ReminderStock", aModel.ReminderStock);
                cmd.Parameters.AddWithValue("@GroupId", aModel.GroupId);
                cmd.Parameters.AddWithValue("@RackNo", aModel.RackNo);
                cmd.Parameters.AddWithValue("@RowNo", aModel.RowNo);
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.UserName));
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

           public Task<string> Update( PharProductInfoModel aModel)
        {

            try
            {
                const string query = @"UPDATE tbl_PHAR_PRODUCT SET CompanyId=@CompanyId,Name=@Name,GenericId=@GenericId,ProductUnit=@ProductUnit,Tp=@Tp,SalesPrice=@SalesPrice,ReminderStock=@ReminderStock,GroupId=@GroupId,RackNo=@RackNo,RowNo=@RowNo,UserName=@UserName,BranchId=@BranchId
                                        WHERE Id=@Id";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", aModel.Id);
                cmd.Parameters.AddWithValue("@CompanyId", aModel.CompanyId);
                cmd.Parameters.AddWithValue("@Name", aModel.Name);
                cmd.Parameters.AddWithValue("@GenericId", aModel.GenericId);
                cmd.Parameters.AddWithValue("@ProductUnit", aModel.ProductUnit);
                cmd.Parameters.AddWithValue("@Tp", aModel.Tp);
                cmd.Parameters.AddWithValue("@SalesPrice", aModel.SalesPrice);
                cmd.Parameters.AddWithValue("@ReminderStock", aModel.ReminderStock);
                cmd.Parameters.AddWithValue("@GroupId", aModel.GroupId);
                cmd.Parameters.AddWithValue("@RackNo", aModel.RackNo);
                cmd.Parameters.AddWithValue("@RowNo", aModel.RowNo);
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.UserName));
          
                
                cmd.ExecuteNonQuery();
                Con.Close();
                return Task.FromResult<string>("Update Successful");
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

           public List<PharProductInfoModel> GetPharProductList(string searchString)
           {
               try
               {
                   string condition = "";
                   var lists = new List<PharProductInfoModel>();
                   string query = "";

                   if (searchString != "0") { condition = " WHERE (a.Name+a.Code) LIKE '%' + '" + searchString + "' + '%' "; }

                   query = @"SELECT a.Id,a.CompanyId,a.Name,a.Code,a.GenericId,a.ProductUnit,a.Tp,a.SalesPrice,a.ReminderStock,a.GroupId,a.RackNo,a.RowNo,a.UserName,a.EntryDate,a.BranchId,a.EntryTime,b.Name AS CompanyName,c.Name AS GroupName,d.Name as GernericName
                                From tbl_PHAR_PRODUCT AS a 
                                LEFT Join tbl_PHAR_COMPANY AS b ON a.CompanyId=b.Id
                                LEFT Join tbl_PHAR_PRODUCT_GROUP AS c ON a.GroupId=c.Id 
                                LEFT Join tbl_PHAR_GENERIC_NAME as d on d.Id=a.GenericId " + condition + " ";
                   Con.Open();
                   var cmd = new SqlCommand(query, Con);
                   cmd.Parameters.Clear();
                   var rdr = cmd.ExecuteReader();
                   while (rdr.Read())
                   {
                       lists.Add(new PharProductInfoModel()
                       {
                           Id = Convert.ToInt32(rdr["Id"]),
                           GenericId = Convert.ToInt32(rdr["GenericId"]),
                           CompanyId = Convert.ToInt32(rdr["CompanyId"]),
                           CompanyName = rdr["CompanyName"].ToString(),
                           Name = rdr["Name"].ToString(),
                           Code = rdr["Code"].ToString(),
                           ProductUnit = rdr["ProductUnit"].ToString(),
                           Tp = Convert.ToDouble(rdr["Tp"]),
                           SalesPrice = Convert.ToDouble(rdr["SalesPrice"]),
                           ReminderStock = Convert.ToInt32(rdr["ReminderStock"]),
                           GroupId = Convert.ToInt32(rdr["GroupId"]),
                          // SubGroupId = Convert.ToInt32(rdr["SubGroupId"]),
                           GroupName = rdr["GroupName"].ToString(),
                          // SubGroupName = rdr["SubGroupName"].ToString(),
                           RackNo = rdr["RackNo"].ToString(),
                           RowNo = Convert.ToInt32(rdr["RowNo"]),
                           UserName = rdr["UserName"].ToString(),
                           EntryDate = Convert.ToDateTime(rdr["EntryDate"]),
                           BranchId = Convert.ToInt32(rdr["BranchId"]),
                           GenericName = rdr["GernericName"].ToString(),

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
                   return new List<PharProductInfoModel>();
               }
           }



           public List<PharProductInfoModel> GetPharProductList(string searchString,int companyId)
           {
               try
               {
                   string condition = "";
                   var lists = new List<PharProductInfoModel>();
                   string query = "";
                   if (searchString != "0") { condition = " WHERE a.CompanyId=" + companyId + " AND (a.Name+a.Code) LIKE '%' + '" + searchString + "' + '%' "; }

                   query = @"SELECT a.Id,a.CompanyId,a.Name,a.Code,a.GenericId,a.ProductUnit,a.Tp,a.SalesPrice,a.ReminderStock,a.GroupId,a.RackNo,a.RowNo,a.UserName,a.EntryDate,a.BranchId,a.EntryTime,b.Name AS CompanyName,c.Name AS GroupName,d.Name as GernericName
                                From tbl_PHAR_PRODUCT AS a 
                                LEFT Join tbl_PHAR_COMPANY AS b ON a.CompanyId=b.Id
                                LEFT Join tbl_PHAR_PRODUCT_GROUP AS c ON a.GroupId=c.Id 
                                LEFT Join tbl_PHAR_GENERIC_NAME as d on d.Id=a.GenericId " + condition + " ";
                   Con.Open();
                   var cmd = new SqlCommand(query, Con);
                   cmd.Parameters.Clear();
                   var rdr = cmd.ExecuteReader();
                   while (rdr.Read())
                   {
                       lists.Add(new PharProductInfoModel()
                       {
                           Id = Convert.ToInt32(rdr["Id"]),
                           GenericId = Convert.ToInt32(rdr["GenericId"]),
                           CompanyId = Convert.ToInt32(rdr["CompanyId"]),
                           CompanyName = rdr["CompanyName"].ToString(),
                           Name = rdr["Name"].ToString(),
                           Code = rdr["Code"].ToString(),
                           ProductUnit = rdr["ProductUnit"].ToString(),
                           Tp = Convert.ToDouble(rdr["Tp"]),
                           SalesPrice = Convert.ToDouble(rdr["SalesPrice"]),
                           ReminderStock = Convert.ToInt32(rdr["ReminderStock"]),
                           GroupId = Convert.ToInt32(rdr["GroupId"]),
                           // SubGroupId = Convert.ToInt32(rdr["SubGroupId"]),
                           GroupName = rdr["GroupName"].ToString(),
                           // SubGroupName = rdr["SubGroupName"].ToString(),
                           RackNo = rdr["RackNo"].ToString(),
                           RowNo = Convert.ToInt32(rdr["RowNo"]),
                           UserName = rdr["UserName"].ToString(),
                           EntryDate = Convert.ToDateTime(rdr["EntryDate"]),
                           BranchId = Convert.ToInt32(rdr["BranchId"]),
                           GenericName = rdr["GernericName"].ToString(),

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
                   return new List<PharProductInfoModel>();
               }
           }

           public List<PharProductInfoModel> GetPharProductListWithoutBarCode(string searchString)
           {
               try
               {
                   string condition = "";
                   var lists = new List<PharProductInfoModel>();
                   string query = "";
                   if (searchString != "0") { condition = "WHERE (a.Name+a.Code) LIKE '%' + '" + searchString + "' + '%' "; }

                   query = @"SELECT a.Id,a.CompanyId,a.Name,a.Code,a.ProductUnit,a.Tp,a.SalesPrice,a.ReminderStock,a.GroupId,a.RackNo,a.RowNo,a.UserName,a.EntryDate,a.BranchId,a.EntryTime,b.Name AS CompanyName,c.Name AS GroupName
                                From tbl_PHAR_PRODUCT AS a 
                                LEFT Join tbl_PHAR_COMPANY AS b ON a.CompanyId=b.Id
                                LEFT Join tbl_PHAR_PRODUCT_GROUP AS c ON a.GroupId=c.Id
                               " + condition + " ";
                   Con.Open();
                   var cmd = new SqlCommand(query, Con);
                   cmd.Parameters.Clear();
                   var rdr = cmd.ExecuteReader();
                   while (rdr.Read())
                   {
                       lists.Add(new PharProductInfoModel()
                       {
                           Id = Convert.ToInt32(rdr["Id"]),
                           CompanyId = Convert.ToInt32(rdr["CompanyId"]),
                           CompanyName = rdr["CompanyName"].ToString(),
                           Name = rdr["Name"].ToString(),
                           Code = rdr["Code"].ToString(),
                           ProductUnit = rdr["ProductUnit"].ToString(),
                           Tp = Convert.ToDouble(rdr["Tp"]),
                           SalesPrice = Convert.ToDouble(rdr["SalesPrice"]),
                           ReminderStock = Convert.ToInt32(rdr["ReminderStock"]),
                           GroupId = Convert.ToInt32(rdr["GroupId"]),
                           GroupName = rdr["GroupName"].ToString(),
                           RackNo = rdr["RackNo"].ToString(),
                           RowNo = Convert.ToInt32(rdr["RowNo"]),
                           UserName = rdr["UserName"].ToString(),
                           EntryDate = Convert.ToDateTime(rdr["EntryDate"]),
                           BranchId = Convert.ToInt32(rdr["BranchId"]),

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
                   return new List<PharProductInfoModel>();
               }
           }
        
           public List<IdNameForDropdownModel> GetProductGroup(int param)
           {
               try
               {
                   var lists = new List<IdNameForDropdownModel>();
                   string query = "";
                   if (param != 0)
                   {
                       query = @"SELECT Id,Name From tbl_PHAR_PRODUCT_GROUP WHERE Id=@Param";
                   }
                   else
                   {
                       query = @"SELECT Id,Name From tbl_PHAR_PRODUCT_GROUP";
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

           public List<IdNameForDropdownModel> GetGenericName(int param)
           {
               try
               {
                   var lists = new List<IdNameForDropdownModel>();
                   string query = "";
                   if (param != 0)
                   {
                       query = @"SELECT Id,Name From tbl_PHAR_GENERIC_NAME WHERE Id=@Param";
                   }
                   else
                   {
                       query = @"SELECT Id,Name From tbl_PHAR_GENERIC_NAME";
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

           public List<PharProductInfoModel> GetMedicineInfoById(int param)
           {
               try
               {
                   string condition = "";
                   var lists = new List<PharProductInfoModel>();
                   string query = "";
                   if (param != 0) { condition = "WHERE a.Id= '" + param + "' "; }

                   query = @"SELECT a.Id,a.CompanyId,a.Name,a.Code,a.ProductUnit,a.Tp,a.SalesPrice,a.ReminderStock,a.GroupId,a.RackNo,a.RowNo,a.UserName,a.EntryDate,a.BranchId,a.EntryTime,b.Name AS CompanyName,c.Name AS GroupName 
                                From tbl_PHAR_PRODUCT AS a 
                                LEFT Join tbl_PHAR_COMPANY AS b ON a.CompanyId=b.Id
                                LEFT Join tbl_PHAR_PRODUCT_GROUP AS c ON a.GroupId=c.Id
                                " + condition + " ";
                   Con.Open();
                   var cmd = new SqlCommand(query, Con);
                   cmd.Parameters.Clear();
                   var rdr = cmd.ExecuteReader();
                   while (rdr.Read())
                   {
                       lists.Add(new PharProductInfoModel()
                       {
                           Id = Convert.ToInt32(rdr["Id"]),
                           CompanyId = Convert.ToInt32(rdr["CompanyId"]),
                           CompanyName = rdr["CompanyName"].ToString(),
                           Name = rdr["Name"].ToString(),
                           Code = rdr["Code"].ToString(),
                           ProductUnit = rdr["ProductUnit"].ToString(),
                           Tp = Convert.ToDouble(rdr["Tp"]),
                           SalesPrice = Convert.ToDouble(rdr["SalesPrice"]),
                           ReminderStock = Convert.ToInt32(rdr["ReminderStock"]),
                           GroupId = Convert.ToInt32(rdr["GroupId"]),
                           GroupName = rdr["GroupName"].ToString(),
                           RackNo = rdr["RackNo"].ToString(),
                           RowNo = Convert.ToInt32(rdr["RowNo"]),
                           UserName = rdr["UserName"].ToString(),
                           EntryDate = Convert.ToDateTime(rdr["EntryDate"]),
                           BranchId = Convert.ToInt32(rdr["BranchId"]),

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
                   return new List<PharProductInfoModel>();
               }
           }

           //public List<IdNameForDropdownModel> GetProductSubGroup(int param)
           //{
           //    try
           //    {
           //        var lists = new List<IdNameForDropdownModel>();
           //        string query = "";
           //        if (param != 0)
           //        {
           //            query = @"SELECT Id,Name From tbl_PRODUCT_GROUP_SUB_DTL WHERE Id=@Param";
           //        }
           //        else
           //        {
           //            query = @"SELECT Id,Name From tbl_PRODUCT_GROUP_SUB_DTL";
           //        }
           //        Con.Open();
           //        var cmd = new SqlCommand(query, Con);
           //        cmd.Parameters.Clear();
           //        cmd.Parameters.AddWithValue("@Param", param);
           //        var rdr = cmd.ExecuteReader();

           //        while (rdr.Read())
           //        {
           //            lists.Add(new IdNameForDropdownModel()
           //            {
           //                Id = Convert.ToInt32(rdr["Id"]),
           //                Name = rdr["Name"].ToString(),
           //            });
           //        }
           //        rdr.Close();
           //        Con.Close();
           //        return lists;
           //    }
           //    catch (Exception exception)
           //    {
           //        if (Con.State == ConnectionState.Open)
           //        {
           //            Con.Close();
           //        }
           //        return new List<IdNameForDropdownModel>();
           //    }
           //}

           //public List<IdNameForDropdownModel> GetProductSubGroupByManinGroupId(int param)
           //{
           //    try
           //    {
           //        var lists = new List<IdNameForDropdownModel>();
           //        string query = "";
           //        if (param != 0)
           //        {
           //            query = @"SELECT Id,Name From tbl_PRODUCT_GROUP_SUB_DTL WHERE MainGroupId=@Param";
           //        }
           //        else
           //        {
           //            query = @"SELECT Id,Name From tbl_PRODUCT_GROUP_SUB_DTL";
           //        }
           //        Con.Open();
           //        var cmd = new SqlCommand(query, Con);
           //        cmd.Parameters.Clear();
           //        cmd.Parameters.AddWithValue("@Param", param);
           //        var rdr = cmd.ExecuteReader();

           //        while (rdr.Read())
           //        {
           //            lists.Add(new IdNameForDropdownModel()
           //            {
           //                Id = Convert.ToInt32(rdr["Id"]),
           //                Name = rdr["Name"].ToString(),
           //            });
           //        }
           //        rdr.Close();
           //        Con.Close();
           //        return lists;
           //    }
           //    catch (Exception exception)
           //    {
           //        if (Con.State == ConnectionState.Open)
           //        {
           //            Con.Close();
           //        }
           //        return new List<IdNameForDropdownModel>();
           //    }
           //}

           public string Delete(int id)
           {
               try
               {
                   string msg = "";
                   const string query = @"DELETE FROM tbl_PHAR_PRODUCT WHERE Id=@Id";
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

           public string SaveProductGroup(string groupName)
           {
               try
               {
                   Con.Open();
                   const string query = @"INSERT INTO tbl_PHAR_PRODUCT_GROUP (Name) VALUES (@Name)";
                   var cmd = new SqlCommand(query, Con);
                   cmd.Parameters.Clear();
                   cmd.Parameters.AddWithValue("@Name", groupName);
                   cmd.ExecuteNonQuery();
                   Con.Close();
                   return "Save successful";
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

           public string SaveGenericName(string groupName)
           {
               try
               {
                   Con.Open();
                   const string query = @"INSERT INTO tbl_PHAR_GENERIC_NAME (Name) VALUES (@Name)";
                   var cmd = new SqlCommand(query, Con);
                   cmd.Parameters.Clear();
                   cmd.Parameters.AddWithValue("@Name", groupName);
                   cmd.ExecuteNonQuery();
                   Con.Close();
                   return "Save successful";
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

           //public string SaveProductSubGroup(int mainGroupId,string subGroupName)
           //{
           //    try
           //    {
           //        Con.Open();
           //        const string query = @"INSERT INTO tbl_PRODUCT_GROUP_SUB_DTL (Name,MainGroupId) VALUES (@Name,@MainGroupId)";
           //        var cmd = new SqlCommand(query, Con);
           //        cmd.Parameters.Clear();
           //        cmd.Parameters.AddWithValue("@Name", subGroupName);
           //        cmd.Parameters.AddWithValue("@MainGroupId", mainGroupId);
           //        cmd.ExecuteNonQuery();
           //        Con.Close();
           //        return "Save successful";
           //    }
           //    catch (Exception exception)
           //    {
           //        if (Con.State == ConnectionState.Open)
           //        {
           //            Con.Close();
           //        }
           //        return exception.Message;
           //    }

           //}
   //     
           public string UpdateProductGroup(int groupId,string groupName)
           {

               try
               {
                   const string query = @"UPDATE tbl_PHAR_PRODUCT_GROUP SET Name=@Name
                                        WHERE Id=@groupId";
                   Con.Open();
                   var cmd = new SqlCommand(query, Con);
                   cmd.Parameters.Clear();
                   cmd.Parameters.AddWithValue("@groupId", groupId);
                   cmd.Parameters.AddWithValue("@Name", groupName);
                   cmd.ExecuteNonQuery();
                   Con.Close();
                   return "Update Successful";
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

           public string UpdateGenericName(int groupId, string groupName)
           {

               try
               {
                   const string query = @"UPDATE tbl_PHAR_GENERIC_NAME SET Name=@Name
                                        WHERE Id=@groupId";
                   Con.Open();
                   var cmd = new SqlCommand(query, Con);
                   cmd.Parameters.Clear();
                   cmd.Parameters.AddWithValue("@groupId", groupId);
                   cmd.Parameters.AddWithValue("@Name", groupName);
                   cmd.ExecuteNonQuery();
                   Con.Close();
                   return "Update Successful";
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


        //           public string UpdateProductSubGroup(int mainGroupId,int subGroupId,string subGroupName)
        //           {
        //               try
        //               {
        //                   const string query = @"UPDATE tbl_PRODUCT_GROUP_SUB_DTL SET MainGroupId=@MainGroupId,Name=@Name
        //                                        WHERE Id=@Id";
        //                   Con.Open();
        //                   var cmd = new SqlCommand(query, Con);
        //                   cmd.Parameters.Clear();
        //                   cmd.Parameters.AddWithValue("@Id", subGroupId);
        //                   cmd.Parameters.AddWithValue("@MainGroupId", mainGroupId);
        //                   cmd.Parameters.AddWithValue("@Name", subGroupName);
        //                   cmd.ExecuteNonQuery();
        //                   Con.Close();
        //                   return "Update Successful";
        //               }
        //               catch (Exception exception)
        //               {
        //                   if (Con.State == ConnectionState.Open)
        //                   {
        //                       Con.Close();
        //                   }
        //                   return exception.Message;
        //               }


        //           }



    }
}