using DevExpress.XtraReports.UI;
using System.Collections.Specialized;
using voyage_pro_report_service.Interfaces.IServices;
using voyage_pro_report_service.Interfaces.Shared;
using voyage_pro_report_service.Models;

namespace voyage_pro_report_service.Reports.BL
{
    public class BLReportBinder : IReportDataBinder
    {
        private readonly IReportService _reportService;

        public BLReportBinder(IReportService reportService)
        {
            _reportService = reportService;
        }
        public string ReportName => "rptExportBL";
        private const string TcIntlResourceName = "voyage_pro_report_service.Assets.TC_International.pdf";
        private const string TcDomesticResourceName = "voyage_pro_report_service.Assets.TC_Domestic.pdf";

        public XtraReport CreateReport(NameValueCollection query)
        {
            var report = new rptExportBL();

            var quoId = int.Parse(query["id"]!);
            var companyId = 2; //int.Parse(query["companyID"]!);
            var agencyId = 2;//int.Parse(query["agencyID"]!);
            var userId = 1; //int.Parse(query["userID"]!);

            var data = _reportService
                .GetBlReportDataAsync(quoId, companyId, agencyId, userId)
                .GetAwaiter()
                .GetResult();

            if (data == null) return report;

            var containers = _reportService
                .GetBLContainersAsync(quoId, companyId, agencyId)
                .GetAwaiter()
                .GetResult()
                .ToList();

            data.TotalContainers = containers.Count;

            data.ContainerCountSummary = string.Join(" / ", containers
                .Where(c => !string.IsNullOrWhiteSpace(c.SizeCode))
                .GroupBy(c => new { c.SizeCode, Type = c.GroupDescription ?? c.TypeCode })
                .Select(g => $"{g.Count()} X {g.Key.SizeCode}'{g.Key.Type}"));

            data.ContainerCountWord = NumberToWords(data.TotalContainers);

            data.ContainerDetails = string.Join("\n", containers
                .Where(c => !string.IsNullOrWhiteSpace(c.ContainerNo))
                .Select(c => string.IsNullOrWhiteSpace(c.SealNo)
                    ? c.ContainerNo
                    : $"{c.ContainerNo} / SEAL: {c.SealNo}"));

            data.PrintAs = query["printAs"];
            data.PrePaidAt = query["prePaidAt"];
            data.AsCarrier = query["asCarrier"];
            data.PrintOnBoard = bool.TryParse(query["printOnBoard"], out var pob) && pob;
            data.NoOfOriginalBLs = int.TryParse(query["noOfOriginalBLs"], out var n) ? n : 0;

            data.ShippedOnBoard = DateTime.TryParseExact(
                query["shippedOnBoard"], "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var sob) ? sob : null;

            // Port fields: use the modal value if typed, otherwise keep the DB value
            data.PlaceOfReceipt = Pick(query["placeOfReceipt"], data.PlaceOfReceipt);
            data.PDe = Pick(query["placeOfDelivery"], data.PDe);
            data.Pol = Pick(query["portOfLoading"], data.Pol);
            data.Pod = Pick(query["portOfDischarge"], data.Pod);

            //Logo
            data.LogoData = LoadEmbeddedFile("ASRL.PNG");

            report.DataSource = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource
            {
                DataSource = data
            };
            report.DataMember = "";

            var tcBytes = LoadTermsPdf(data.TradeType);
            if (tcBytes != null)
            {
                var pdfContent = new XRPdfContent
                {
                    Source = tcBytes,
                    GenerateOwnPages = true,              
                    LocationF = new DevExpress.Utils.PointFloat(0F, 0F),
                    SizeF = new System.Drawing.SizeF(report.PageWidth - report.Margins.Left - report.Margins.Right, 100F)
                };

                var tcBand = new ReportHeaderBand
                {
                    HeightF = 100F,
                    PageBreak = PageBreak.AfterBand
                };
                tcBand.Controls.Add(pdfContent);

                report.Bands.Add(tcBand);
            }

            return report;
        }

        private static string? Pick(string? fromForm, string? fromDb) => string.IsNullOrWhiteSpace(fromForm) ? fromDb : fromForm;

        public static string NumberToWords(int number)
        {
            string[] ones =
            {
                "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE",
                "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN",
                "SEVENTEEN", "EIGHTEEN", "NINETEEN",
            };
            string[] tens =
            {
                "", "", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY",
            };

            if (number < 0 || number >= 100)
                return number.ToString();

            if (number < 20)
                return ones[number];

            return tens[number / 10] + (number % 10 > 0 ? "-" + ones[number % 10] : string.Empty);
        }

        private static byte[]? LoadTermsPdf(string? tradeType)
        {
            var resourceName = string.Equals(tradeType, "D", StringComparison.OrdinalIgnoreCase)
                ? TcDomesticResourceName
                : TcIntlResourceName;

            using var stream = typeof(BLReportBinder).Assembly.GetManifestResourceStream(resourceName);
            if (stream is null) return null;          

            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }

        private static byte[]? LoadEmbeddedFile(string fileName)
        {
            var assembly = typeof(BLReportBinder).Assembly;

            var names = assembly.GetManifestResourceNames();
            System.Diagnostics.Debug.WriteLine("RESOURCES: " + string.Join(" | ", names));

            var resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith("." + fileName, StringComparison.OrdinalIgnoreCase));
            if (resourceName is null) return null;

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream is null) return null;

            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
