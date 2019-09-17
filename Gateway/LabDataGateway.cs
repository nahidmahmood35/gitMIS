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
    public class LabDataGateway:DbConnection
    {
        private SqlTransaction _trans;
        internal Task<string> Save(List<LabParameterModel> mLab)
        {
            try
            {
                Con.Open();
                _trans = Con.BeginTransaction();

                if (FncSeekRecordNew("tbl_LAB_RESULT", "InvmasterId=" + mLab.ElementAt(0).InvMasterId + " AND ItemId=" + mLab.ElementAt(0).ItemId + "",_trans))
                {
                    DeleteInsert("DELETE FROM tbl_LAB_RESULT WHERE InvmasterId=" + mLab.ElementAt(0).InvMasterId + " AND ItemId=" + mLab.ElementAt(0).ItemId + "", _trans);
                }

                DataTable dt = ConvertListDataTable(mLab);
                var objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_LAB_RESULT" };
                objbulk.ColumnMappings.Add("InvMasterId", "InvmasterId");
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

                objbulk.ColumnMappings.Add("ReportDrId", "ReportDrId");
                objbulk.ColumnMappings.Add("LabInchargeId", "LabInchargeId");
                objbulk.ColumnMappings.Add("CheckedById", "CheckedById");

                objbulk.ColumnMappings.Add("ReportDrDetails", "ReportDrDetails");
                objbulk.ColumnMappings.Add("LabInchargeDetails", "LabInchargeDetails");
                objbulk.ColumnMappings.Add("CheckedByDetails", "CheckedByDetails");





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
        internal List<LabParameterModel> GetResultByInvmasterAndItemId(int invmasterId, int itemId)
        {
            try
            {
                var list = new List<LabParameterModel>();
                string query = "";
                query = @"SELECT * FROM tbl_LAB_RESULT WHERE ItemId=@itemId AND InvMasterId=@InvMasterId Order By Id";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@itemId", itemId);
                cmd.Parameters.AddWithValue("@InvMasterId", invmasterId);
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


        internal LabParameterModel GetLabdata(int itemId, int invMasterId)
        {
            try
            {
                var list = new LabParameterModel();
                string query = "SELECT Specimen,Result,ReportDrDetails,LabInchargeDetails,CheckedByDetails,InvoiceNo,InvoiceDate,Age,PtRegNo,Name,MobileNo,InvTime,GenderName,UnderDrName  FROM VW_GET_LAB_REPORT_VIEW WHERE ItemId=" + itemId + " AND InvmasTerId=" + invMasterId + "";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Specimen = rdr["Specimen"].ToString();
                    list.Result = rdr["Result"].ToString();
                    list.ReportDrDetails = rdr["ReportDrDetails"].ToString();
                    list.LabInchargeDetails = rdr["LabInchargeDetails"].ToString();
                    list.CheckedByDetails = rdr["CheckedByDetails"].ToString();
                    list.InvoiceNo = rdr["InvoiceNo"].ToString();
                    list.InvoiceDate = Convert.ToDateTime(rdr["InvoiceDate"]);
                    list.PtAgeDetail = rdr["Age"].ToString();
                    list.PtRegNo = rdr["PtRegNo"].ToString();
                    list.PtName = rdr["Name"].ToString();
                    list.PtMobileNo = rdr["MobileNo"].ToString();
                    list.PtGenderName = rdr["GenderName"].ToString();
                    list.DrName = rdr["UnderDrName"].ToString();
                    list.InvTime = rdr["InvTime"].ToString();

                }

                rdr.Close();
                Con.Close();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}