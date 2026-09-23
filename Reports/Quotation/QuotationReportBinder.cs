using DevExpress.XtraReports.UI;
using System.Collections.Specialized;
using voyage_pro_report_service.DTOs;
using voyage_pro_report_service.Interfaces.IServices;
using voyage_pro_report_service.Interfaces.Shared;

namespace voyage_pro_report_service.Reports.Quotation
{
    public class QuotationReportBinder : IReportDataBinder
    {
        private readonly IReportService _reportService;

        public QuotationReportBinder(IReportService reportService)
        {
            _reportService = reportService;
        }

        public string ReportName => "rptQuotation";

        public XtraReport CreateReport(NameValueCollection query)
        {
            var quoId = int.Parse(query["id"]!);
            var companyId = int.Parse(query["companyID"]!);
            var agencyId = int.Parse(query["agencyID"]!);

            var data = _reportService
                .GetQuotationReportDataAsync(quoId, companyId, agencyId)
                .GetAwaiter()
                .GetResult();

            var report = new rptQuotation();
            if (data?.Header is null) return report;

            report.DataSource = new List<QuotationHD> { data.Header };
            report.DataMember = "";

            BindSubreport(report, "xrSubreportContainers", data.Containers);
            BindSubreport(report, "xrSubreportRevenue", data.RevenueItems);
            BindSubreport(report, "xrSubreportCost", data.CostItems);

            return report;
        }

        private static void BindSubreport(XtraReport report, string controlName, object dataSource)
        {
            if (report.FindControl(controlName, true) is XRSubreport sub
                && sub.ReportSource is XtraReport subReport)
            {
                subReport.DataSource = dataSource;
            }
        }
    }
}
