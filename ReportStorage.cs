using DevExpress.XtraReports.Web.Extensions;
using System.Web;
using voyage_pro_report_service.DTOs;
using voyage_pro_report_service.Interfaces.Shared;
using voyage_pro_report_service.Models;
using voyage_pro_report_service.Reports;

namespace voyage_pro_report_service
{
    public class ReportStorage : ReportStorageWebExtension
    {
        private readonly IEnumerable<IReportDataBinder> _binders;

        public ReportStorage(IEnumerable<IReportDataBinder> binders)
        {
            _binders = binders;
        }

        public override bool CanSetData(string url) => false;

        public override bool IsValidUrl(string url) =>
            FindBinder(GetReportName(url)) is not null;

        public override byte[] GetData(string url)
        {
            var (reportName, query) = ParseUrl(url);
            var binder = FindBinder(reportName)
                ?? throw new InvalidOperationException($"No report binder registered for '{reportName}'.");

            using var report = binder.CreateReport(query);
            using var ms = new MemoryStream();
            report.SaveLayoutToXml(ms);
            return ms.ToArray();
        }

        private IReportDataBinder? FindBinder(string reportName) =>
            _binders.FirstOrDefault(b => b.ReportName.Equals(reportName, StringComparison.OrdinalIgnoreCase));

        private static string GetReportName(string url) => url.Split('?')[0];

        private static (string name, System.Collections.Specialized.NameValueCollection query) ParseUrl(string url)
        {
            var parts = url.Split('?', 2);
            var query = HttpUtility.ParseQueryString(parts.Length > 1 ? parts[1] : "");
            return (parts[0], query);
        }
    }
}
