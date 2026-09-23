using Dapper;
using Microsoft.Data.SqlClient;
using voyage_pro_report_service.DTOs;
using voyage_pro_report_service.Interfaces.IRepository;
using voyage_pro_report_service.Models;

namespace voyage_pro_report_service.Repositories
{
    public class ReportDataRepository: IReportDataRepository
    {
        private readonly string _connectionString;

        public ReportDataRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DB")
                ?? throw new InvalidOperationException("Connection not found.");
        }

        public async Task<QuotationHD?> GetQuotationHDAsync(int quoId, int companyId, int agencyId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            const string headerSql = @"
                SELECT varQuoNo, varQuoName, dteFromDate, dteToDate, intOrgID, varBookingType, varSerTermCode,
                       varPOL, varPOD, varPOOR, varPOfDelivery, varShipmentType, varCommodityCode,
                       varKindOfPkg, varGoodsDescription, intPackageType
                FROM QM.QuotationMainHD A
                INNER JOIN QM.QuotationShpHD B ON A.intQuoID = B.intQuoID
                WHERE A.intQuoID = @QuoId AND A.bitActive = 1
                  AND intUserCompanyID = @CompanyId AND intUserAgencyID = @AgencyId";

            var parameters = new { QuoId = quoId, CompanyId = companyId, AgencyId = agencyId };

            var header = await connection.QuerySingleOrDefaultAsync<QuotationHD>(headerSql, parameters);

            return header;
        }

        public async Task<List<ContainerTEU>?> GetContainerTEUAsync(int quoId, int companyId, int agencyId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string containerSql = @"
                SELECT varContainerSize, varContainerType, intQuantity,
                       intNoOfTEUs, numGrossWeight, numCBM, varRemarks
                FROM QM.QuotationAllocatedCntrs
                WHERE intQuoID = @QuoId AND bitActive = 1";
            var parameters = new { QuoId = quoId, CompanyId = companyId, AgencyId = agencyId };
            var containers = await connection.QueryAsync<ContainerTEU>(containerSql, parameters);
            return containers.ToList();
        }

        public async Task<List<RevenueItm>?> GetRevenueItmAsync(int quoId, int companyId, int agencyId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string revenueSql = @"
                SELECT varchargeCode, intUOMId, varSellCurCode, decSellCurAmount, decTaxPercentage, decTaxAmount, varRE
                FROM QM.QuotationShpDt
                WHERE intQuoID = @QuoId AND varRE = 'R' AND bitActive = 1";
            var parameters = new { QuoId = quoId, CompanyId = companyId, AgencyId = agencyId };
            var revenues = await connection.QueryAsync<RevenueItm>(revenueSql, parameters);
            return revenues.ToList();
        }

        public async Task<List<CostItm>?> GetCostItmAsync(int quoId, int companyId, int agencyId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string costSql = @"
                SELECT varchargeCode, intUOMId, varLCurCode, decLCurAmount, decTaxPercentage, decTaxAmount, varRE
                FROM QM.QuotationShpDt
                WHERE intQuoID = @QuoId AND varRE = 'E' AND bitActive = 1";
            var parameters = new { QuoId = quoId, CompanyId = companyId, AgencyId = agencyId };
            var costs = await connection.QueryAsync<CostItm>(costSql, parameters);
            return costs.ToList();
        }
    }
}
