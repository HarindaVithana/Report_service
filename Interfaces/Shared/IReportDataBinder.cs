namespace voyage_pro_report_service.Interfaces.Shared
{
    public interface IReportDataBinder
    {
        string ReportName { get; }
        DevExpress.XtraReports.UI.XtraReport CreateReport(System.Collections.Specialized.NameValueCollection query);
    }
}
