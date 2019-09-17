<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="HospitalManagementApp_Api.Report.ReportViewer.ReportViewer" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
        <form id="form1" runat="server">
        <table class="col-sm-offset-2 col-sm-4">
            <tr>
                <td style="text-align: center">
                    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" HasRefreshButton="True" ReuseParameterValuesOnRefresh="False" HasPrintButton="True" HasToggleGroupTreeButton="False" ShowAllPageIds="True"   />
                </td>
            </tr>
        </table>
        <table style="width:100%;">
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblMessage" runat="server" Font-Names="Arial" Font-Size="X-Small" ForeColor="#FF3300"></asp:Label>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
