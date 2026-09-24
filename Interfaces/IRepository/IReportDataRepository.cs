using voyage_pro_report_service.DTOs;

namespace voyage_pro_report_service.Interfaces.IRepository
{
    public interface IReportDataRepository
    {
        Task<QuotationHD?> GetQuotationHDAsync(int quoId, int companyId, int agencyId);
        Task<List<ContainerTEU>?> GetContainerTEUAsync(int quoId, int companyId, int agencyId);
        Task<List<CostItm>?> GetCostItmAsync(int quoId, int companyId, int agencyId);
        Task<List<RevenueItm>?> GetRevenueItmAsync(int quoId, int companyId, int agencyId);
        Task<string?> GetReportUserNameAsync(int intUserID);
    }
}
