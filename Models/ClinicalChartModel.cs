using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class ClinicalChartModel
    {
        public int ItemIdForVaq { get; set; }
        public int ItemId { get; set; }
        public string PCode { get; set; }
        public string Description { get; set; }
        public double Charge { get; set; }
        public double ServiceCharge { get; set; }
        public string ServiceChargePcOrTkId { get; set; }
        public double Vat { get; set; }
        public string VatPcOrTkId { get; set; }
        public double LessFixedAmount { get; set; }
        public double ReferelFee { get; set; }
        public string ReferelFeePcOrTkId { get; set; }
        public double ReportFee { get; set; }
        public string ReportFeePcOrTkId { get; set; }
        public double CollectionFee { get; set; }
        public string CollectionFeePcOrTkId { get; set; }
        public double OthersFee { get; set; }
        public string OthersFeePcOrTkId { get; set; }
        public int DeliveryDuration { get; set; }
        public int OutTest { get; set; }
        public int CanChangePrice { get; set; }
        public int ShowDoctorCode { get; set; }
        public int CanGiveLess { get; set; }
        public string  ReportFileName { get; set; }
        public int SubSubPnoId { get; set; }
        public int InDoorBillGroupId { get; set; }
        public string InDoorBillGroupName { get; set; }
        public int ReportGroupId { get; set; }
        public string ReportGroupName { get; set; }
        public int Active { get; set; }
        public int DiscountGroupId { get; set; }
        public string  DiscountGroupName { get; set; }
        public int AccountReportGroupId { get; set; }
        public string AccountReportGroupName { get; set; }
        public int IsShowId { get; set; }
        public int IsAdjustAmtId { get; set; }
        public string UserName { get; set; }
        public DateTime DeliveryDate { get; set; }
        public double MaxRefFee { get; set; }
        public double ItemwiseLess { get; set; }
        public double Quantity { get; set; }
        public double TotalCharge { get; set; }

        public double ItemTotal { get; set; }


    }
}
