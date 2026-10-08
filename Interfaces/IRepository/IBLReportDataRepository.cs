using voyage_pro_report_service.DTOs;

namespace voyage_pro_report_service.Interfaces.IRepository
{
    public interface IBLReportDataRepository
    {
        Task<BLDetailData?> GetBLReportDTAsync(int blId, int companyId, int agencyId);
        Task<IEnumerable<BLContainerResult>> GetBLContainersAsync(int blId, int companyId, int agencyId);
    }
}
