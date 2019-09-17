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
    public class PharPurchasePaymentGateway:DbConnection
    {
        private SqlTransaction _trans;


         public Task<string> Save(PharSalesModel aModel)
        {
            try
            {
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();
                 string trNo = GetTrNo("TrNo", "tbl_PHAR_PURCHASE_LEDGER",_trans);
                 string trNoDuePayment = GetAutoIncrementNumberFromStoreProcedure(10, _trans);

                const string query2 = @"INSERT INTO tbl_PHAR_PURCHASE_LEDGER (TrNo,TrDate,InvoiceNo,InvoiceDate,ComPanyId,PurchaseAmount,LessAmount,PaymentAmount,Status,SubSubPnoId,UserName,PaymentStatus,BranchId,EntryDate,EntryTime) VALUES (@TrNo,@TrDate,@InvoiceNo,@InvoiceDate,@ComPanyId,@PurchaseAmount,@LessAmount,@PaymentAmount,@Status,@SubSubPnoId,@UserName,@PaymentStatus,@BranchId,@EntryDate,@EntryTime)";
                var cmd = new SqlCommand(query2, Con, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@TrNo", trNo);
                cmd.Parameters.AddWithValue("@TrDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@InvoiceNo", aModel.InvoiceNo);
                cmd.Parameters.AddWithValue("@InvoiceDate", aModel.InvoiceDate);
                cmd.Parameters.AddWithValue("@ComPanyId", aModel.CompanyId);
                cmd.Parameters.AddWithValue("@PurchaseAmount", 0);
                cmd.Parameters.AddWithValue("@LessAmount", aModel.LessAmt);
                cmd.Parameters.AddWithValue("@PaymentAmount", aModel.PaymentAmt);
                cmd.Parameters.AddWithValue("@Status", "Due Payment");
                cmd.Parameters.AddWithValue("@SubSubPnoId", "65");
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.UserName, _trans));
                cmd.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@PaymentStatus", aModel.PaymentAmt > 0 ? 3 : 1); //
                cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.ExecuteNonQuery();


                const string query3 = @"INSERT INTO tbl_PHAR_DUE_PAYMENT (TrNo,TrDate,InvoiceNo,Invoicedate,CompanyId,PaymentAmount,LessAmount,LessPc,LessPcOrTk,SubSubPnoId,CashAmount,CardAmount) VALUES  (@TrNo,@TrDate,@InvoiceNo,@Invoicedate,@CompanyId,@PaymentAmount,@LessAmount,@LessPc,@LessPcOrTk,@SubSubPnoId,@CashAmount,@CardAmount)"; 
                var cmd2 = new SqlCommand(query3, Con, _trans);
                cmd2.Parameters.Clear();
                cmd2.Parameters.AddWithValue("@TrNo", trNoDuePayment);
                cmd2.Parameters.AddWithValue("@TrDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd2.Parameters.AddWithValue("@InvoiceNo",aModel.InvoiceNo );
                cmd2.Parameters.AddWithValue("@Invoicedate", aModel.InvoiceDate);
                cmd2.Parameters.AddWithValue("@CompanyId", aModel.CompanyId);
                cmd2.Parameters.AddWithValue("@PaymentAmount", aModel.PaymentAmt);
                cmd2.Parameters.AddWithValue("@LessAmount", aModel.LessAmt);
                cmd2.Parameters.AddWithValue("@LessPc", aModel.LessPc);
                cmd2.Parameters.AddWithValue("@LessPcOrTk", aModel.LessPcOrTk);
                cmd2.Parameters.AddWithValue("@SubSubPnoId","65" );
                cmd2.Parameters.AddWithValue("@CashAmount", aModel.CashAmt);
                cmd2.Parameters.AddWithValue("@CardAmount", aModel.CardAmt);



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

        
        public List<PharPurchaseModel> GetPurchaseInvNo(string searchString)
        {
            try
            {
                string comId = "";  //DueAmount
                var lists = new List<PharPurchaseModel>();
                string query = "";
                query = @"SELECT InvoiceNo,InvoiceDate,CompanyId,SUM(PurchaseAmount-LessAmount-PaymentAmount) AS Due
                           FROM tbl_PHAR_PURCHASE_LEDGER WHERE CompanyId='" + searchString + "'Group by InvoiceNo,InvoiceDate,CompanyId HAVING SUM(PurchaseAmount-LessAmount-PaymentAmount)>0";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new PharPurchaseModel()
                    {
                        InvoiceNo = rdr["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToDateTime(rdr["InvoiceDate"]),
                        CompanyId = Convert.ToInt32(rdr["CompanyId"]),
                        DueAmount = Convert.ToDouble(rdr["Due"]),
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

        public List<PharPurchaseModel> GetInvDetail(string searchString)
        {
            try
            {
                string comId = "";  //DueAmount
                var lists = new List<PharPurchaseModel>();
                string query = "";
                query = @"SELECT SUM(PurchaseAmount-LessAmount-PaymentAmount) AS Due
                           FROM tbl_PHAR_PURCHASE_LEDGER WHERE InvoiceNo='" + searchString + "' ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new PharPurchaseModel()
                    {
                      DueAmount = Convert.ToDouble(rdr["Due"]),
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

       // GetInvDetail


        public List<PharPurchaseModel> GetInvDetailByDate(int companyId,string fromDate,string toDate)
        {
            try
            {
               
                var lists = new List<PharPurchaseModel>();
                string query = "";

                query = @"SELECT InvoiceNo,CompanyId,PurchaseAmount,Spd,InvoiceDate
                             FROM tbl_PHAR_PURCHASE_LEDGER 
                             where CompanyId=" + companyId + " and InvoiceDate between '"+fromDate+"' and '"+toDate+"' ";

                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new PharPurchaseModel()
                    {
                        InvoiceNo = rdr["InvoiceNo"].ToString(),
                        //CompanyId=Convert.ToInt32()
                        //PurchaseAmount=Convert.ToDouble(0)
                        //Spd=Convert.ToDouble(0)
                        //InvoiceDate=Convert.ToDateTime()

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