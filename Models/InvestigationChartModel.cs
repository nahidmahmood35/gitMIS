using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models
{
    public class InvestigationChartModel
    {
        

          public int  Id  { get; set; }
          public string  DeptName { get; set; }
          public string  PCode { get; set; }
          public string  ShortDesc { get; set; }
          public string  LongDesc { get; set; }
          public double  Charge { get; set; }
          public double  TradePrice { get; set; }
          public double  Discount { get; set; }
          public string  DiscountStatus { get; set; }
          public string  NormalValue { get; set; }
          public double  LessAmount { get; set; }
          public string  RptType { get; set; }
          public string  UserName { get; set; }
          public int  Category { get; set; }
          public string  GroupId { get; set; }
          public string  GroupName { get; set; }
          public int  DeliveryDuration { get; set; }
          public string  SubDeptName { get; set; }
          public string  SubSubDeptName { get; set; }
          public string  VaqGroup { get; set; }
          public string  VaqName { get; set; }
          public double  VaqPrice { get; set; }
          public double  VaqVat { get; set; }
          public double  VaqComm { get; set; }
          public string  VaqStatus { get; set; }
          public int  TestType { get; set; }
          public double  RptCom { get; set; }
          public int  RptComPcorTk { get; set; }
          public int  ISShow { get; set; }
          public double  CollectionFee { get; set; }
          public string  CollFeeTkPC { get; set; }
          public double  MedicineFee { get; set; }
          public string  MedicineFeeTkPc { get; set; }
          public string  MedicineFeeName { get; set; }
          public string  ReportGroup { get; set; }



    }
}