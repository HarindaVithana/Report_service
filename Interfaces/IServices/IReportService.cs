using voyage_pro_report_service.Models;

namespace voyage_pro_report_service.Interfaces.IServices
{
    public interface IReportService
    {
        Task<QuotationReportData?> GetQuotationReportDataAsync(int quoId, int companyId, int agencyId);
    }
}
