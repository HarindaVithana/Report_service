using voyage_pro_report_service.Interfaces.IRepository;
using voyage_pro_report_service.Interfaces.IServices;
using voyage_pro_report_service.Models;

namespace voyage_pro_report_service.Services
{
    public class ReportService(IReportDataRepository reportDataRepository) : IReportService
    {
        private readonly IReportDataRepository _reportDataRepository = reportDataRepository;

        public async Task<QuotationReportData?> GetQuotationReportDataAsync(int quoId, int companyId, int agencyId, int userId)
        {
            QuotationReportData quoReportDT = new QuotationReportData();

            quoReportDT.Header = await _reportDataRepository.GetQuotationHDAsync(quoId, companyId, agencyId);

            if(quoReportDT.Header != null)
            {
                quoReportDT.Containers = await _reportDataRepository.GetContainerTEUAsync(quoId, companyId, agencyId);
                quoReportDT.RevenueItems = await _reportDataRepository.GetRevenueItmAsync(quoId, companyId, agencyId);
                quoReportDT.CostItems = await _reportDataRepository.GetCostItmAsync(quoId, companyId, agencyId);
                quoReportDT.Header.ReportUserName = await _reportDataRepository.GetReportUserNameAsync(userId);
            }

            return quoReportDT;
        }
    }
}
