using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using HospitalManagementApp_Api.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HospitalManagementApp_Api.Report.ReportViewer
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        ReportDocument _rptFile = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                try
                {
                    _rptFile = (ReportDocument)Session[SessionInfo.ReportFile];
                    var hParametre = (Hashtable)Session[SessionInfo.ReportParam];
                    var param = new ParameterValues();
                    var val = new ParameterDiscreteValue();

                    foreach (ParameterFieldDefinition obj in _rptFile.DataDefinition.ParameterFields)
                    {
                        if (hParametre.ContainsKey(obj.Name))
                        {
                            val.Value = hParametre[obj.Name].ToString();
                            param.Add(val);
                            obj.ApplyCurrentValues(param);
                        }
                    }

                    CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                    CrystalReportViewer1.ReportSource = _rptFile;
                    _rptFile.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "ExportedReport");
                    Session.Remove(SessionInfo.ReportFile);
                    Session.Remove(SessionInfo.ReportParam);
                    Session[SessionInfo.ReportFile] = _rptFile;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                }
            }
            else if (Session[SessionInfo.ReportFile] != null)
            {
                _rptFile = (ReportDocument)Session[SessionInfo.ReportFile];
                CrystalReportViewer1.ReportSource = _rptFile;
            }
        }
    }
}