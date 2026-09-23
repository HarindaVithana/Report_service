using voyage_pro_report_service.DTOs;

namespace voyage_pro_report_service.Models
{
    public class QuotationReportData
    {
        public QuotationHD? Header { get; set; } = null!;
        public List<ContainerTEU>? Containers { get; set; } = new();
        public List<RevenueItm>? RevenueItems { get; set; } = new();
        public List<CostItm>? CostItems { get; set; } = new();
    }
}
