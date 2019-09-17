using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class PharSalesMasterModel
    {
        public string MainDeptName { get; set; }
        public string SaleNo { get; set; }
        public DateTime SaleDate { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string PatientType { get; set; }
        public int Valid { get; set; }
        public double TotalAmt { get; set; }
        public double LessAmt { get; set; }
        public double AdvanceAmt { get; set; }
        public double PaidAmt { get; set; }
        public double DueAmt { get; set; }
        public double ReturnAmt { get; set; }
        public int IsTransfer { get; set; }
        public string TrNo { get; set; }
        public string Remarks { get; set; }
        public string UserName { get; set; }
        public string EntryTime { get; set; }
        public int isRelease { get; set; }
        public double ReceiveDueAmt { get; set; }
        public double VatSlNo { get; set; }
        public double CommonLessAmt { get; set; }
        public int LessType { get; set; }
        public string PNo { get; set; }
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
        public string CardNo { get; set; }
        public string CardBank{ get; set; }
        public string ChqNo{ get; set; }
        public string ChqBank{ get; set; }
        public int isCashClosed{ get; set; }
        public string CashClosedTrNo{ get; set; }
        public string RefDRCode{ get; set; }
        public string ServedBy{ get; set; }
        public double NetProfit{ get; set; }
        public int Package{ get; set; }
        public string PackCode{ get; set; }
        public double AdjAmt{ get; set; }
        








    }
}