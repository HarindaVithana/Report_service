using DevExpress.XtraReports.UI;
using System.Collections;
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
            var userId = int.Parse(query["userID"]!); // User ID

            var data = _reportService
                .GetQuotationReportDataAsync(quoId, companyId, agencyId, userId)
                .GetAwaiter()
                .GetResult();

            var report = new rptQuotation();
            if (data?.Header is null) return report;

            report.DataSource = new List<QuotationHD> { data.Header };
            report.DataMember = "";

            BindSubreport(report, "rptQuotationContTeu", data.Containers);
            BindSubreport(report, "rptQuotationRevenue", data.RevenueItems);
            BindSubreport(report, "rptQuotationCost", data.CostItems);

            return report;
        }

        private static void BindSubreport(XtraReport report, string controlName, IEnumerable dataSource)
        {
            var control = report.FindControl(controlName, true);
            if (control is not XRSubreport sub)
            {
                Console.WriteLine($"[BindSubreport] Control '{controlName}' not found or not an XRSubreport.");
                return;
            }
            if (sub.ReportSource is not XtraReport subReport)
            {
                Console.WriteLine($"[BindSubreport] '{controlName}' has no ReportSource assigned.");
                return;
            }

            var hasData = dataSource?.Cast<object>().Any() == true;

            sub.Visible = hasData;
            subReport.DataSource = hasData ? dataSource : null;
        }
    }
}
