using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using System.Web;
using voyage_pro_report_service.Interfaces.Shared;

namespace voyage_pro_report_service
{
    /// <summary>
    /// Returns the populated report instance directly to the Document Viewer.
    /// Going through ReportStorage.GetData serializes the report to XML, which drops the
    /// in-memory data and fails to reload because the DTO types are not DevExpress-trusted.
    /// </summary>
    public class ReportResolver : IWebDocumentViewerReportResolver
    {
        private readonly IEnumerable<IReportDataBinder> _binders;

        public ReportResolver(IEnumerable<IReportDataBinder> binders)
        {
            _binders = binders;
        }

        public XtraReport Resolve(string reportEntry)
        {
            var parts = reportEntry.Split('?', 2);
            var reportName = parts[0];
            var query = HttpUtility.ParseQueryString(parts.Length > 1 ? parts[1] : "");

            var binder = _binders.FirstOrDefault(b => b.ReportName.Equals(reportName, StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidOperationException($"No report binder registered for '{reportName}'.");

            return binder.CreateReport(query);
        }
    }
}
