using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using HospitalManagementApp_Api.Models;
using HospitalManagementApp_Api.Models.DynamicMenuModel;
using HospitalManagementApp_Api.Report.DataSet;


namespace HospitalManagementApp_Api.Gateway.DB_Helper
{
    public class DbConnection
    {

        public SqlConnection Con = new SqlConnection(WebConfigurationManager.ConnectionStrings["PointOfSalesDBConnectionString"].ConnectionString.ToString());
        //public SqlConnection Con = new SqlConnection(@"Data Source=RAKIB\SQL_2008;Initial Catalog=PointOfSalesDB;User ID=sa;Password=ra710983");
        public string DeleteInsert(string id)
        {
            try
            {
                Con.Open();
                string autoId = id;
                var command = new SqlCommand(autoId, Con);
                return command.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
            }
        }


        public string DeleteInsert(string id,SqlTransaction transaction)
        {
            try
            {
                //Con.Open();
                string autoId = id;
                var command = new SqlCommand(autoId, Con,transaction);
                return command.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                {
                //    Con.Close();
                }
            }
        }
        public bool FncSeekRecordNew(string lcTableName, string lcCondition)
        {
            string query = "";
            if (lcCondition != "")
            {
                query = "Select * from " + lcTableName + " where " + lcCondition + "";
            }
            else
            {
                query = "Select * from " + lcTableName + "";
            }
            Con.Open();
            var cmd = new SqlCommand(query, Con);
            var aReader = cmd.ExecuteReader();
            bool lnTrueFlase = aReader.HasRows;
            Con.Close();
            return lnTrueFlase;
        }
        public bool FncSeekRecordNew(string lcTableName, string lcCondition,SqlTransaction transaction)
        {
            string query = "";
            if (lcCondition != "")
            {
                query = "Select * from " + lcTableName + " where " + lcCondition + "";
            }
            else
            {
                query = "Select * from " + lcTableName + "";
            }
            //Con.Open();
            var cmd = new SqlCommand(query, Con,transaction);
            var aReader = cmd.ExecuteReader();
            bool lnTrueFlase = aReader.HasRows;
            aReader.Close();
            //Con.Close();
            return lnTrueFlase;
        }


        public string GetBarCodeMaxId(string tableName, SqlTransaction transaction)
        {
            string trno = "";
            string sql = @"SELECT ISNULL(Max(SUBSTRING(BarcodeId,11,4)),0)+1 AS TrNo FROM " + tableName + " WHERE BarCodeId IS Not null  AND RefDate='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            //Con.Open();
            var aCommand = new SqlCommand(sql, Con, transaction);
            SqlDataReader aReader = aCommand.ExecuteReader();
            while (aReader.Read())
            {
                trno = aReader["TrNo"].ToString();
            }
            // Con.Close();
            aReader.Close();
            return trno;
        }
        public string ReturnFieldValue(string lcTableName, string lcCondition, string lcFieldName)
        {
            string query = "", result = "";
            if (lcCondition != "")
            {
                query = "Select " + lcFieldName + " as Description from " + lcTableName + " where " + lcCondition + "";
            }
            else
            {
                query = "Select " + lcFieldName + " as Description from " + lcTableName + "";
            }
            Con.Open();
            var aCommand = new SqlCommand(query, Con);
            SqlDataReader aReader = aCommand.ExecuteReader();

            while (aReader.Read())
            {
                result = aReader["Description"].ToString();
            }
            aReader.Close();
            Con.Close();
            return result;
        }
        public string ReturnFieldValueOpenCon(string lcTableName, string lcCondition, string lcFieldName, SqlTransaction transaction)
        {
            string query = "", result = "";
            if (lcCondition != "")
            {
                query = "Select " + lcFieldName + " as Description from " + lcTableName + " where " + lcCondition + "";
            }
            else
            {
                query = "Select " + lcFieldName + " as Description from " + lcTableName + "";
            }
            //Con.Open();
            var aCommand = new SqlCommand(query, Con, transaction);
            SqlDataReader aReader = aCommand.ExecuteReader();

            while (aReader.Read())
            {
                result = aReader["Description"].ToString();
            }
            aReader.Close();
            // Con.Close();
            return result;
        }
        public string ReturnFieldValueOpenCon(string lcTableName, string lcCondition, string lcFieldName)
        {
            string query = "", result = "";
            if (lcCondition != "")
            {
                query = "Select " + lcFieldName + " as Description from " + lcTableName + " where " + lcCondition + "";
            }
            else
            {
                query = "Select " + lcFieldName + " as Description from " + lcTableName + "";
            }
            //Con.Open();
            var aCommand = new SqlCommand(query, Con);
            SqlDataReader aReader = aCommand.ExecuteReader();

            while (aReader.Read())
            {
                result = aReader["Description"].ToString();
            }
            aReader.Close();
            // Con.Close();
            return result;
        }
        public int GetSubSubPnoByIndoorId(int indoorId,SqlTransaction trans)
        {
            int subsubPnoId = 0;
            string query = "Select Isnull(SubSubPnoId,0) as Description from tbl_IN_PATIENT_ADMISSION where ID="+ indoorId +"";
            var aCommand = new SqlCommand(query, Con,trans);
            SqlDataReader aReader = aCommand.ExecuteReader();
            while (aReader.Read())
            {
                subsubPnoId =Convert.ToInt32(aReader["Description"]);
            }
            aReader.Close();
            return subsubPnoId;
        }
        public List<IdNameForDropdownModel> GetIdNameForDropDownBox(string query)
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
        public DataTable ConvertListDataTable<T>(List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        public string GetMaxId(string tableName, string fieldName)
        {
            string trno = "";
            string sql = @"SELECT  RIGHT('0000'+CAST(ISNULL(Max(Code),0)+1 AS VARCHAR(4)),4) AS TrNo  FROM " + tableName + " ";
            Con.Open();
            var aCommand = new SqlCommand(sql, Con);
            SqlDataReader aReader = aCommand.ExecuteReader();
            while (aReader.Read())
            {
                trno = aReader["TrNo"].ToString();
            }
            aReader.Close();
            Con.Close();
            return trno;
        }
        public string GetAutoIncrementNumberFromStoreProcedure(int tableId, SqlTransaction trans)
        {
            string lcsql = @"Exec SP_GET_AUTO_GENERATED_NUMBER " + tableId + "";
            //Con.Open();
            var aCommand = new SqlCommand(lcsql, Con, trans);
            SqlDataReader aReader = aCommand.ExecuteReader();
            while (aReader.Read())
            {
                lcsql = aReader["InvNo"].ToString();
            }
            //Con.Close();
            aReader.Close();
            return lcsql;
        }
        public string GetAutoIncrementNumberFromStoreProcedure(int tableId)
        {
            string lcsql = @"Exec SP_GET_AUTO_GENERATED_NUMBER " + tableId + "";
            Con.Open();
            var aCommand = new SqlCommand(lcsql, Con);
            SqlDataReader aReader = aCommand.ExecuteReader();
            while (aReader.Read())
            {
                lcsql = aReader["InvNo"].ToString();
            }
            Con.Close();
            aReader.Close();
            return lcsql;
        }
        public string GetTrNoWithOpenCon(string fieldName, string tableName, SqlTransaction trans)
        {
            string lcsql = @"SELECT  RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) + RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(" + fieldName + ", 6))),0)+ 1), 4) AS TrNo FROM " + tableName + "";
            // Con.Open();
            var aCommand = new SqlCommand(lcsql, Con, trans);
            SqlDataReader aReader = aCommand.ExecuteReader();
            while (aReader.Read())
            {
                lcsql = aReader["TrNo"].ToString();
            }
            // Con.Close();
            aReader.Close();
            return lcsql;
        }
        public string GetTrNoWithOpenCon(string fieldName, string tableName)
        {
            string lcsql = @"SELECT  RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) + RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(" + fieldName + ", 6))),0)+ 1), 4) AS TrNo FROM " + tableName + "";
            // Con.Open();
            var aCommand = new SqlCommand(lcsql, Con);
            SqlDataReader aReader = aCommand.ExecuteReader();
            while (aReader.Read())
            {
                lcsql = aReader["TrNo"].ToString();
            }
            // Con.Close();
            aReader.Close();
            return lcsql;
        }
        public string GetTrNo(string fieldName, string tableName)
        {
            string lcsql = @"SELECT  RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) + RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(" + fieldName + ", 4))),0)+ 1), 4) AS TrNo FROM " + tableName + "";
            Con.Open();
            var aCommand = new SqlCommand(lcsql, Con);
            SqlDataReader aReader = aCommand.ExecuteReader();
            while (aReader.Read())
            {
                lcsql = aReader["TrNo"].ToString();
            }
            Con.Close();
            aReader.Close();
            return lcsql;
        }
        public string GetTrNo(string fieldName, string tableName, SqlTransaction trans)
        {
            string lcsql = @"SELECT  RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) + RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(" + fieldName + ", 4))),0)+ 1), 4) AS TrNo FROM " + tableName + "";
            //Con.Open();
            var aCommand = new SqlCommand(lcsql, Con, trans);
            SqlDataReader aReader = aCommand.ExecuteReader();
            while (aReader.Read())
            {
                lcsql = aReader["TrNo"].ToString();
            }
            //Con.Close();
            aReader.Close();
            return lcsql;
        }

        public string GetCurrentAgeOfaPatient(string dateOfBirth)
        {

            string year = "", month = "", day = "";
            try
            {
                string lcsql = @"Exec SP_GET_CURRENT_AGE '" + dateOfBirth + "'";
                Con.Open();
                var aCommand = new SqlCommand(lcsql, Con);
                SqlDataReader aReader = aCommand.ExecuteReader();
                while (aReader.Read())
                {
                    year = aReader["YYYY"].ToString();
                    month = aReader["MM"].ToString();
                    day = aReader["DD"].ToString();
                }
                Con.Close();
                aReader.Close();
                return year + "Y " + month + "M " + day + "D";
            }
            catch (Exception ex)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return ex.Message;
            }



        }
        public dynamic GetCurrentAgeOfaPatientForField(string dateOfBirth)
        {

            var currentAge = new CurrentAgeModel();
            try
            {
                string lcsql = @"Exec SP_GET_CURRENT_AGE '" + dateOfBirth + "'";
                Con.Open();
                var aCommand = new SqlCommand(lcsql, Con);
                SqlDataReader aReader = aCommand.ExecuteReader();
                while (aReader.Read())
                {
                    currentAge.Year = Convert.ToInt32(aReader["YYYY"]);
                    currentAge.Month = Convert.ToInt32(aReader["MM"]);
                    currentAge.Day = Convert.ToInt32(aReader["DD"]);
                }

                Con.Close();
                aReader.Close();
                return currentAge;
            }
            catch (Exception ex)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return ex.Message;
            }



        }
        public DataTable GetDataFromProc(DataTable table, string lcparameter, string lcProcedureName, string dataReadBy)
        {


            try
            {
                string lcStrSql = "";
                if (dataReadBy != "V")
                {
                    lcStrSql = "" + lcProcedureName + " " + lcparameter;
                }
                else
                {
                    lcStrSql = "SELECT * FROM " + lcProcedureName + " " + lcparameter;
                }
                var cmd = new SqlCommand(lcStrSql, Con);
                var objAdapter = new SqlDataAdapter(cmd);
                objAdapter.Fill(table);
                return table;
            }
            catch (Exception exception)
            {
                throw;

            }

        }
        public void AddParameters(Hashtable hParameter, String lcComName, String lcComAddress, String lcTitle, String lcDateRange)
        {
            hParameter.Add("lcComName", lcComName);
            hParameter.Add("lcComAddress", lcComAddress);
            hParameter.Add("lcTitle", lcTitle);
            hParameter.Add("lcDateRange", lcDateRange);
        }





        DataSet1 _ds = new DataSet1();
        HrDataSet _hrDataSet = new HrDataSet();
        ReportDocument _rptdoc = new ReportDocument();
        PharmacyV2 _pharmacyV2 = new PharmacyV2();
        private string _rptPath = "";
        public string PrintReport(string reportFileName, string dataTableName, string parameterName, string procedureName, string titleName, string dateRange, string dataReadBy)
        {
            _rptdoc = new ReportDocument();
            _ds = new DataSet1();
            _rptPath = System.Web.HttpContext.Current.Server.MapPath("~/Report/ReportFile/" + reportFileName + "");
            _rptdoc.Load(_rptPath);
            new DbConnection().GetDataFromProc(_ds.Tables["" + dataTableName + ""], "" + parameterName + "", "" + procedureName + "", dataReadBy);
            _rptdoc.SetDataSource(_ds.Tables["" + dataTableName + ""]);
            _rptdoc.SetDataSource(_ds.Tables["" + dataTableName + ""].DefaultView);
            var hParameter = new Hashtable();
            new DbConnection().AddParameters(hParameter, ReturnFieldValue("tbl_COMPANY_INFORMATION", "", "Name"), ReturnFieldValue("tbl_COMPANY_INFORMATION", "", "Address"), titleName, dateRange);
            System.Web.HttpContext.Current.Session.Add(SessionInfo.ReportFile, _rptdoc);
            System.Web.HttpContext.Current.Session.Add(SessionInfo.ReportParam, hParameter);
            return "";
        }
        public string PrintReportHr(string reportFileName, string dataTableName, string parameterName, string procedureName, string titleName, string dateRange, string dataReadBy)
        {
            _rptdoc = new ReportDocument();
            _hrDataSet = new HrDataSet();
            _rptPath = System.Web.HttpContext.Current.Server.MapPath("~/Report/ReportFile/" + reportFileName + "");
            _rptdoc.Load(_rptPath);
            new DbConnection().GetDataFromProc(_hrDataSet.Tables["" + dataTableName + ""], "" + parameterName + "", "" + procedureName + "", dataReadBy);
            _rptdoc.SetDataSource(_hrDataSet.Tables["" + dataTableName + ""]);
            _rptdoc.SetDataSource(_hrDataSet.Tables["" + dataTableName + ""].DefaultView);
            var hParameter = new Hashtable();
            new DbConnection().AddParameters(hParameter, ReturnFieldValue("tbl_COMPANY_INFORMATION", "", "Name"), ReturnFieldValue("tbl_COMPANY_INFORMATION", "", "Address"), titleName, dateRange);
            System.Web.HttpContext.Current.Session.Add(SessionInfo.ReportFile, _rptdoc);
            System.Web.HttpContext.Current.Session.Add(SessionInfo.ReportParam, hParameter);
            return "";
        }

        LabReportDataSet _dsLab=new LabReportDataSet();
        public string PrintReportLab(string reportFileName, string dataTableName, string parameterName, string procedureName, string titleName, string dateRange, string dataReadBy)
        {
            _rptdoc = new ReportDocument();
            _dsLab = new LabReportDataSet();
            _rptPath = System.Web.HttpContext.Current.Server.MapPath("~/Report/LabReportFile/" + reportFileName + "");
            _rptdoc.Load(_rptPath);
            new DbConnection().GetDataFromProc(_dsLab.Tables["" + dataTableName + ""], "" + parameterName + "", "" + procedureName + "", dataReadBy);
            _rptdoc.SetDataSource(_dsLab.Tables["" + dataTableName + ""]);
            _rptdoc.SetDataSource(_dsLab.Tables["" + dataTableName + ""].DefaultView);
            var hParameter = new Hashtable();
            new DbConnection().AddParameters(hParameter, ReturnFieldValue("tbl_COMPANY_INFORMATION", "", "Name"), ReturnFieldValue("tbl_COMPANY_INFORMATION", "", "Address"), titleName, dateRange);
            System.Web.HttpContext.Current.Session.Add(SessionInfo.ReportFile, _rptdoc);
            System.Web.HttpContext.Current.Session.Add(SessionInfo.ReportParam, hParameter);
            return "";
        }





        public string PrintReportPhar(string reportFileName, string dataTableName, string parameterName, string procedureName, string titleName, string dateRange, string dataReadBy)
        {
            _rptdoc = new ReportDocument();
            _pharmacyV2 = new PharmacyV2();
            _rptPath = System.Web.HttpContext.Current.Server.MapPath("~/Report/ReportFile/" + reportFileName + "");
            _rptdoc.Load(_rptPath);
            new DbConnection().GetDataFromProc(_pharmacyV2.Tables["" + dataTableName + ""], "" + parameterName + "", "" + procedureName + "", dataReadBy);
            _rptdoc.SetDataSource(_pharmacyV2.Tables["" + dataTableName + ""]);
            _rptdoc.SetDataSource(_pharmacyV2.Tables["" + dataTableName + ""].DefaultView);
            var hParameter = new Hashtable();
            new DbConnection().AddParameters(hParameter, ReturnFieldValue("tbl_COMPANY_INFORMATION", "", "Name"), ReturnFieldValue("tbl_COMPANY_INFORMATION", "", "Address"), titleName, dateRange);
            System.Web.HttpContext.Current.Session.Add(SessionInfo.ReportFile, _rptdoc);
            System.Web.HttpContext.Current.Session.Add(SessionInfo.ReportParam, hParameter);
            return "";
        }
       

        public Image ByteArrayToImage(byte[] byteArrayIn)
        {
            var ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            var ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
        public Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            var ms = new MemoryStream(imageBytes, 0,
            imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }
        public int GetBranchIdByuserNameOpenCon(string userName, SqlTransaction trans)
        {
            string lcsql = @"SELECT BranchId From tbl_USER_BRANCH_INFO WHERE UserName=@UserName";
            //    Con.Open();
            var aCommand = new SqlCommand(lcsql, Con, trans);
            aCommand.Parameters.AddWithValue("@UserName", userName);

            SqlDataReader aReader = aCommand.ExecuteReader();
            while (aReader.Read())
            {
                lcsql = aReader["BranchId"].ToString();
            }
            //  Con.Close();
            aReader.Close();
            return Convert.ToInt32(lcsql);
        }
        public int GetBranchIdByuserNameOpenCon(string userName)
        {
            string lcsql = @"SELECT BranchId From tbl_USER_BRANCH_INFO WHERE UserName=@UserName";
            //    Con.Open();
            var aCommand = new SqlCommand(lcsql, Con);
            aCommand.Parameters.AddWithValue("@UserName", userName);

            SqlDataReader aReader = aCommand.ExecuteReader();
            while (aReader.Read())
            {
                lcsql = aReader["BranchId"].ToString();
            }
            //  Con.Close();
            aReader.Close();
            return Convert.ToInt32(lcsql);
        }
        public int GetBranchIdByuserName(string userName)
        {
            string lcsql = @"SELECT BranchId From tbl_USER_BRANCH_INFO WHERE UserName=@UserName";
            Con.Open();
            var aCommand = new SqlCommand(lcsql, Con);
            aCommand.Parameters.AddWithValue("@UserName", userName);
            SqlDataReader aReader = aCommand.ExecuteReader();
            while (aReader.Read())
            {
                lcsql = aReader["BranchId"].ToString();
            }
            Con.Close();
            aReader.Close();
            return Convert.ToInt32(lcsql);
        }



        internal void InsertUserLog(string description)
        {
            const string query = @"INSERT INTO tbl_USER_LOG (UserName,Description) VALUES (@UserName,@Description)";
            var cmd = new SqlCommand(query, Con);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@UserName", System.Web.HttpContext.Current.Session["UserName"]);
            cmd.Parameters.AddWithValue("@Description", description);


            Con.Open();
            cmd.ExecuteNonQuery();
            Con.Close();
        }

        public void InsertIntoTransact(string code, double debit, double credit, string naration, string remark, string trNo, string transDt, string subTrNo, string voucherType)
        {
            const string query = @"INSERT INTO tbl_TRANSACT (Code, Debit, Credit, Naration, Remark, TrNo, Trans_dt, SubTrNo, UserName,VoucherType) 
                                        VALUES (@Code, @Debit, @Credit, @Naration, @Remark, @TrNo, @Trans_dt, @SubTrNo, @UserName,@VoucherType)";
            var aComand = new SqlCommand(query, Con);
            aComand.Parameters.Clear();
            aComand.Parameters.AddWithValue("@Code", code);
            aComand.Parameters.AddWithValue("@Debit", debit);
            aComand.Parameters.AddWithValue("@Credit", credit);
            aComand.Parameters.AddWithValue("@Naration", naration);
            aComand.Parameters.AddWithValue("@Remark", remark);
            aComand.Parameters.AddWithValue("@TrNo", trNo);
            aComand.Parameters.AddWithValue("@Trans_dt", transDt);
            aComand.Parameters.AddWithValue("@SubTrNo", subTrNo);
            aComand.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
            aComand.Parameters.AddWithValue("@VoucherType", voucherType);
            try
            {
                Con.Open();
                aComand.ExecuteNonQuery();
                Con.Close();

            }
            catch (Exception exception)
            {

                throw;
            }
        }
        public void InsertIntoInvoiceLedger(int regId, int indoorId, int invmasterId, string trNo, DateTime trDate, double salesAmt, double lessAmt, double collAmt, double rtnAmt, int subsubPnoId, string saleCollStatus, string userName, SqlTransaction transaction)
        {
            try
            {
                const string query = @"INSERT INTO tbl_INVOICE_LEDGER (RegId, IndoorId, InvMasterId, TrNo, TrDate, SalesAmt, LessAmt, CollectionAmt,CollectionFromIndoor, ReturnAmt, SubSubPnoId, SalesCollStatus, UserName, BranchId) 
                                        VALUES (@RegId, @IndoorId, @InvMasterId, @TrNo, @TrDate, @SalesAmt, @LessAmt, @CollectionAmt,@CollectionFromIndoor, @ReturnAmt, @SubSubPnoId, @SalesCollStatus, @UserName, @BranchId)";
                var aComand = new SqlCommand(query, Con, transaction);
                aComand.Parameters.Clear();
                aComand.Parameters.AddWithValue("@RegId", regId);
                aComand.Parameters.AddWithValue("@IndoorId", indoorId);
                aComand.Parameters.AddWithValue("@InvMasterId", invmasterId);
                aComand.Parameters.AddWithValue("@TrNo", trNo);
                aComand.Parameters.AddWithValue("@TrDate", trDate.ToString("yyyy-MM-dd"));
                aComand.Parameters.AddWithValue("@SalesAmt", salesAmt);
                aComand.Parameters.AddWithValue("@LessAmt", lessAmt);
                if (indoorId!=0)
                {
                    aComand.Parameters.AddWithValue("@CollectionAmt", 0);
                    aComand.Parameters.AddWithValue("@CollectionFromIndoor", collAmt);
                }
                else
                {
                    aComand.Parameters.AddWithValue("@CollectionAmt", collAmt);
                    aComand.Parameters.AddWithValue("@CollectionFromIndoor", 0);
                }

                aComand.Parameters.AddWithValue("@ReturnAmt", rtnAmt);
                aComand.Parameters.AddWithValue("@SubSubPnoId", subsubPnoId);
                aComand.Parameters.AddWithValue("@SalesCollStatus", saleCollStatus);
                aComand.Parameters.AddWithValue("@UserName", userName);
                aComand.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(userName, transaction));
                //  Con.Open();
                aComand.ExecuteNonQuery();
                //   Con.Close();
            }
            catch (Exception exception)
            {

                throw;
            }

        }
        public void InsertIntoHonoriumLedger(int drId, string trNo, DateTime trDate, int invmasterId, double invoiceValue, double honoriumAmount, double lessAmount, double paymentAmount, int subSubPnoId, string userName, SqlTransaction transaction)
        {
            try
            {
                const string query = @"INSERT INTO tbl_DOCTOR_HONORIUM_LEDGER (DrId, TrNo, TrDate, InvmasterId, InvoiceValue, HonoriumAmount, LessAmount, PaymentAmount, SubSubPnoId, UserName, BranchId) 
                                        VALUES (@DrId, @TrNo, @TrDate, @InvmasterId, @InvoiceValue, @HonoriumAmount, @LessAmount, @PaymentAmount, @SubSubPnoId, @UserName, @BranchId)";
                var aComand = new SqlCommand(query, Con, transaction);
                aComand.Parameters.Clear();
                aComand.Parameters.AddWithValue("@DrId", drId);
                aComand.Parameters.AddWithValue("@TrNo", trNo);
                aComand.Parameters.AddWithValue("@TrDate", trDate.ToString("yyyy-MM-dd"));
                aComand.Parameters.AddWithValue("@InvmasterId", invmasterId);
                aComand.Parameters.AddWithValue("@InvoiceValue", invoiceValue);
                aComand.Parameters.AddWithValue("@HonoriumAmount", honoriumAmount);
                aComand.Parameters.AddWithValue("@LessAmount", lessAmount);
                aComand.Parameters.AddWithValue("@PaymentAmount", paymentAmount);
                aComand.Parameters.AddWithValue("@SubSubPnoId", subSubPnoId);
                aComand.Parameters.AddWithValue("@UserName", userName);
                aComand.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(userName, transaction));
                aComand.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                throw;
            }
        }

    }
}