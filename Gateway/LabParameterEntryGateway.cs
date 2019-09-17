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
    public class LabParameterEntryGateway : DbConnection
    {
        private SqlTransaction _trans;
        public Task<string> Save(List<LabParameterModel> mLab)
        {
            try
            {
                Con.Open();                
                _trans = Con.BeginTransaction();
                DataTable dt = ConvertListDataTable(mLab);
                var objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_LAB_PARAMETER_DEFINITION"};
                objbulk.ColumnMappings.Add("ItemId", "ItemId");
                objbulk.ColumnMappings.Add("Specimen", "Specimen");
                objbulk.ColumnMappings.Add("AliasName", "AliasName");
                objbulk.ColumnMappings.Add("ParameterName", "ParameterName");
                objbulk.ColumnMappings.Add("Result", "Result");
                objbulk.ColumnMappings.Add("Unit", "Unit");
                objbulk.ColumnMappings.Add("NormalValue", "NormalValue");
                objbulk.ColumnMappings.Add("GroupName", "GroupName");
                objbulk.ColumnMappings.Add("GroupSlNo", "GroupSlNo");
                objbulk.ColumnMappings.Add("ItemSlNo", "ItemSlNo");
                objbulk.ColumnMappings.Add("ReportFileName", "ReportFileName");
                
                objbulk.WriteToServer(dt);
                _trans.Commit();
                Con.Close();
                return Task.FromResult<string>("Saved Success");
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                    _trans.Rollback();
                }
                return Task.FromResult(exception.Message);
            }
        }
        public Task<string> Update(List<LabParameterModel> mLab)
        {
            try
            {

                Con.Open();
                _trans = Con.BeginTransaction();
                if (FncSeekRecordNew("tbl_LAB_PARAMETER_DEFINITION", "ItemId=" + mLab.ElementAt(0).ItemId + "",_trans))
                {
                    DeleteInsert("DELETE FROM tbl_LAB_PARAMETER_DEFINITION WHERE ItemId=" + mLab.ElementAt(0).ItemId + "",_trans);
                }

                DataTable dt = ConvertListDataTable(mLab);
                var objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_LAB_PARAMETER_DEFINITION" };
                objbulk.ColumnMappings.Add("ItemId", "ItemId");
                objbulk.ColumnMappings.Add("Specimen", "Specimen");
                objbulk.ColumnMappings.Add("AliasName", "AliasName");
                objbulk.ColumnMappings.Add("ParameterName", "ParameterName");
                objbulk.ColumnMappings.Add("Result", "Result");
                objbulk.ColumnMappings.Add("Unit", "Unit");
                objbulk.ColumnMappings.Add("NormalValue", "NormalValue");
                objbulk.ColumnMappings.Add("GroupName", "GroupName");
                objbulk.ColumnMappings.Add("GroupSlNo", "GroupSlNo");
                objbulk.ColumnMappings.Add("ItemSlNo", "ItemSlNo");
                objbulk.ColumnMappings.Add("ReportFileName", "ReportFileName");
                objbulk.WriteToServer(dt);
                _trans.Commit();
                Con.Close();
                return Task.FromResult<string>("Update Success");
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                    _trans.Rollback();
                }
                return Task.FromResult(exception.Message);
            }
        }
        internal List<LabParameterModel> GetParameterListByItemId(int itemId)
        {
            try
            {

                var list = new List<LabParameterModel>();
                string query = "";
                query = @"SELECT * FROM tbl_LAB_PARAMETER_DEFINITION WHERE ItemId=@itemId";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@itemId", itemId);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new LabParameterModel()
                    {
                        ItemId = Convert.ToInt32(rdr["ItemId"]),
                        Specimen = rdr["Specimen"].ToString(),
                        AliasName = rdr["AliasName"].ToString(),
                        ParameterName = rdr["ParameterName"].ToString(),
                        Result = rdr["Result"].ToString(),
                        Unit = rdr["Unit"].ToString(),
                        NormalValue = rdr["NormalValue"].ToString(),
                        GroupName = rdr["GroupName"].ToString(),
                        GroupSlNo = Convert.ToInt32(rdr["GroupSlNo"]),
                        ItemSlNo = Convert.ToInt32(rdr["ItemSlNo"]),
                        ReportFileName = rdr["ReportFileName"].ToString(),

                    });
                }
                rdr.Close();
                Con.Close();
                return list;
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                throw;
            }
        }



        internal List<IdNameForDropdownModel> GetAliasName()
        {
            try
            {

                var list = new List<IdNameForDropdownModel>();
                string query = "";
                query = @"SELECT * FROM tbl_LAB_Alias ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                   list.Add(new IdNameForDropdownModel()
                   {
                       Name = rdr["Name"].ToString(),
                   });
                }
                rdr.Close();
                Con.Close();
                return list;
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                throw;
            }
        }
        internal List<IdNameForDropdownModel> GetParameterName()
        {
            try
            {

                var list = new List<IdNameForDropdownModel>();
                string query = "";
                query = @"SELECT * FROM tbl_LAB_Parameter ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new IdNameForDropdownModel()
                    {
                        Name = rdr["Name"].ToString(),
                    });
                }
                rdr.Close();
                Con.Close();
                return list;
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                throw;
            }
        }
        internal List<IdNameForDropdownModel> GetReportingGroupName()
        {
            try
            {

                var list = new List<IdNameForDropdownModel>();
                string query = "";
                query = @"SELECT * FROM tbl_LAB_ReportingGroup ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new IdNameForDropdownModel()
                    {
                        Name = rdr["Name"].ToString(),
                    });
                }
                rdr.Close();
                Con.Close();
                return list;
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                throw;
            }
        }
        internal List<IdNameForDropdownModel> GetSpecimenName()
        {
            try
            {

                var list = new List<IdNameForDropdownModel>();
                string query = "";
                query = @"SELECT * FROM tbl_LAB_Specimen ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new IdNameForDropdownModel()
                    {
                        Name = rdr["Name"].ToString(),
                    });
                }
                rdr.Close();
                Con.Close();
                return list;
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                throw;
            }
        }
        internal List<IdNameForDropdownModel> GetUnitName()
        {
            try
            {

                var list = new List<IdNameForDropdownModel>();
                string query = "";
                query = @"SELECT * FROM tbl_LAB_Unit ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new IdNameForDropdownModel()
                    {
                        Name = rdr["Name"].ToString(),
                    });
                }
                rdr.Close();
                Con.Close();
                return list;
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                throw;
            }
        }
        internal List<InvoiceModel> GetItemListForLabReport(string invoiceNo)
        {
            try
            {
                var list = new List<InvoiceModel>();
                string query = "";
                query = @"SELECT Id,RegId, IndoorId, ItemId, InvoiceNo, InvoiceDate, Age, ConsultantId,  Pcode, Description, CnsltDoctorName, BedNo, 
                        PtName, MobileNo, GenderName, DeptId, DeptName FROM VW_GET_ITEMLIST_FOR_LAB_REPORT WHERE InvoiceNo=@invoiceNo";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@invoiceNo", invoiceNo);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new InvoiceModel()
                    {
                        InvMasterId = Convert.ToInt32(rdr["Id"]),
                        PtRegId = Convert.ToInt32(rdr["RegId"]),
                        PtIndoorId = Convert.ToInt32(rdr["IndoorId"]),
                        ItemId = Convert.ToInt32(rdr["ItemId"]),
                        DeptId = Convert.ToInt32(rdr["DeptId"]),
                        ConsultantId = Convert.ToInt32(rdr["ConsultantId"]),
                        InvoiceNo = rdr["InvoiceNo"].ToString(),
                        InvoiceDate =Convert.ToDateTime(rdr["InvoiceDate"]),
                        PtAgeDetail = rdr["Age"].ToString(),
                        PCode = rdr["Pcode"].ToString(),
                        Description = rdr["Description"].ToString(),
                        DrName = rdr["CnsltDoctorName"].ToString(),
                        BedNo = rdr["BedNo"].ToString(),
                        Name = rdr["PtName"].ToString(),
                        PtMobileNo = rdr["MobileNo"].ToString(),
                        PtGenderName = rdr["GenderName"].ToString(),
                        SubDeptName = rdr["DeptName"].ToString(),
                    });
                }
                rdr.Close();
                Con.Close();
                return list;
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                throw;
            }
        }

        internal Task<string> SaveOther(LabParameterModel mLab)
        {
            try
            {
                Con.Open();
                const string query = @"INSERT INTO tbl_LAB_PARAMETER_DEFINITION_FOR_OTHERS (ItemId,DefaultResult,UserName,Specimen)  VALUES (@ItemId,@DefaultResult,@UserName,@Specimen)";
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ItemId", mLab.ItemId);
                cmd.Parameters.AddWithValue("@DefaultResult", mLab.Result);
                cmd.Parameters.AddWithValue("@UserName", mLab.UserName);
                cmd.Parameters.AddWithValue("@Specimen", mLab.Specimen);
                cmd.ExecuteNonQuery();
                Con.Close();
                return Task.FromResult<string>("Saved Success");
            }
            catch (Exception ex)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return Task.FromResult(ex.Message);
            }
        }

        internal Task<string> UpdateOther(LabParameterModel mLab)
        {
            try
            {
                Con.Open();
                const string query = @"UPDATE tbl_LAB_PARAMETER_DEFINITION_FOR_OTHERS SET DefaultResult=@DefaultResult,UserName=@UserName,Specimen=@Specimen WHERE ItemId=@ItemId";
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ItemId", mLab.ItemId);
                cmd.Parameters.AddWithValue("@DefaultResult", mLab.Result);
                cmd.Parameters.AddWithValue("@UserName", mLab.UserName);
                cmd.Parameters.AddWithValue("@Specimen", mLab.Specimen);
                cmd.ExecuteNonQuery();
                Con.Close();
                return Task.FromResult<string>("Saved Success");
            }
            catch (Exception ex)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return Task.FromResult(ex.Message);
            }
        }

        internal LabParameterModel GetParameterByItemId(int itemId)
        {
            try
            {
                var data = new LabParameterModel();
                Con.Open();
                const string query =@"SELECT DefaultResult,Specimen From tbl_LAB_PARAMETER_DEFINITION_FOR_OTHERS WHERE ItemId=@ItemId";
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@ItemId", itemId);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    data.Result = rdr["DefaultResult"].ToString();
                    data.Specimen = rdr["Specimen"].ToString();
                }

                rdr.Close();
                Con.Close();
                return data;
            }
            catch (Exception ex)
            {

                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                throw ;
            }
        }
    }
}