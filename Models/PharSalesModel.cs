using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class PharSalesModel : PharCompanyModel
    {
        public string MainDeptName { get; set; }
        public string SaleNo { get; set; }
        public DateTime SaleDate { get; set; }
        public string PatientName { get; set; }
        public string PatientType { get; set; }
        public int Valid { get; set; }
        public double TotalAmt { get; set; }
        public double TotalPrice { get; set; }
        public double AdvanceAmt { get; set; }
        public double PaymentAmt { get; set; }
        public double CashRtn { get; set; }
        public double RtnAmt { get; set; }
        public double DueAmt { get; set; }
        public double Spd { get; set; }
        public double ReturnAmt { get; set; }
        public string TrNo { get; set; }
        public string RtnInvNo { get; set; }
        public string Remarks { get; set; }
        public double ReceiveDueAmt { get; set; }
        public double VatSlNo { get; set; }
        public double CommonLessAmt { get; set; }
        public double PrevDue { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string CollectionFrom { get; set; }
        public double ReceivedAmt { get; set; }
        public double ReceivedReturnAmt { get; set; }
        public int IsDone { get; set; }
        public string DoneBy { get; set; }
        public string RefNo { get; set; }
        public DateTime RefDate { get; set; }
        public string DrCode { get; set; }
        public string DrName { get; set; }
        public double CashAmt { get; set; }
        public double CardAmt { get; set; }
        public double ChequeAmt { get; set; }
        public string CardBank { get; set; }
        public string ChqBank { get; set; }
        public string ServedBy { get; set; }
        public string InvoiceNo { get; set; }
        public int MainDeptId { get; set; }
        public int RegNo { get; set; }
        public int IndoorId { get; set; }
        public double LessPc { get; set; }
        public string LessPcOrTk { get; set; }
        public double LessAmt { get; set; }
        public double LessAmtRtn { get; set; }
        public double TotalLess { get; set; }
        public double ItemLessTk { get; set; }
        public int IsTransfer { get; set; }
        public int SubSubPnoId { get; set; }
        public int CorporateId { get; set; }
        public int InvMasterId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double AvgPurchasePrice { get; set; }
        public double PurchasePrice { get; set; }
        public string PatientId { get; set; }
        public int IsRelease { get; set; }
        public int LessType { get; set; }
        public string PNo { get; set; }
        public int IsCashClosed { get; set; }
        public string CashClosedTrNo { get; set; }
        public string RefDrCode { get; set; }
        public double NetProfit { get; set; }
        public int Package { get; set; }
        public string PackCode { get; set; }
        public double AdjAmt { get; set; }
        public double TotalReceive { get; set; }
        public double BalQty { get; set; }
        public string Status { get; set; }
        public string CardNumber { get; set; }
        public int CardBankId { get; set; }
        public string CheaqueNumber { get; set; }
        public int ChequeBankId { get; set; }
        public double NewTp { get; set; }
        public DateTime TrDate { get; set; }
        public string BarCodeId { get; set; }
        public string UserName2 { get; set; }
        public double TotalQuantity { get; set; }
        public double RtnQty { get; set; }
        public double OutQty { get; set; }
        public double RegId { get; set; }
        public double ReOrderQty { get; set; }
        public string CustomerName { get; set; }
        





    }
}