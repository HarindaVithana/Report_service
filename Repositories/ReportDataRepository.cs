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

            const string headerSql = @"SELECT
                                        A.varQuoNo,
                                        A.varQuoName,
                                        A.dteFromDate,
                                        A.dteToDate,
                                        A.intOrgID,
                                        A.varBookingType,
                                        varSerTermCode,
                                        varPOL,
                                        varPOD,
                                        varPOOR,
                                        varPOfDelivery,
                                        varServiceCode,
                                        varShipmentType,
                                        varCommodityCode,
                                        varKindOfPkg,
                                        varGoodsDescription,
                                        intPackageType,
                                        A.varHDCurrency,
                                        OgCus.varOrgName AS varCustomerName,
                                        CONCAT_WS(', ',
                                            NULLIF(OgCus.varOrgAddr1, ''),
                                            NULLIF(OgCus.varOrgAddr2, ''),
                                            NULLIF(OgCus.varOrgAddr3, ''),
                                            NULLIF(OgCus.varOrgAddr4, ''),
                                            NULLIF(OgCus.varOrgCity, ''),
                                            NULLIF(OgCus.varOrgCountry, '')
                                        ) AS varCustomerAddress,
                                        OgAgn.varOrgName AS varAgentName,
                                        CONCAT_WS(', ',
                                            NULLIF(OgAgn.varOrgAddr1, ''),
                                            NULLIF(OgAgn.varOrgAddr2, ''),
                                            NULLIF(OgAgn.varOrgAddr3, ''),
                                            NULLIF(OgAgn.varOrgAddr4, ''),
                                            NULLIF(OgAgn.varOrgCity, ''),
                                            NULLIF(OgAgn.varOrgCountry, '')
                                        ) AS varAgentAddress,
                                        SUM(C.numGrossWeight) AS totGrossWeight,
                                        SUM(C.numCBM) AS totCBM
                                    FROM QM.QuotationMainHD A

                                    INNER JOIN QM.QuotationShpHD B
                                        ON A.intQuoID = B.intQuoID
                                    INNER JOIN QM.QuotationAllocatedCntrs C
                                        ON B.intQuoID = C.intQuoID
                                    LEFT JOIN VP.GLMFOrganization OgCus
                                        ON A.intOrgID = OgCus.intOrgID
                                    LEFT JOIN VP.GLMFOrganization OgAgn
                                        ON A.intAgentID = OgAgn.intOrgID
                                    WHERE A.intQuoID = @QuoId
                                      AND A.bitActive = 1
                                    GROUP BY
                                        A.varQuoNo,
                                        A.varQuoName,
                                        A.dteFromDate,
                                        A.dteToDate,
                                        A.intOrgID,
                                        A.varBookingType,
                                        varSerTermCode,
                                        varPOL,
                                        varPOD,
                                        varPOOR,
                                        varPOfDelivery,
                                        varServiceCode,
                                        varShipmentType,
                                        varCommodityCode,
                                        varKindOfPkg,
                                        varGoodsDescription,
                                        intPackageType,
                                        A.varHDCurrency,
                                        OgCus.varOrgName,
                                        OgCus.varOrgAddr1,
                                        OgCus.varOrgAddr2,
                                        OgCus.varOrgAddr3,
                                        OgCus.varOrgAddr4,
                                        OgCus.varOrgCity,
                                        OgCus.varOrgCountry,
                                        OgAgn.varOrgName,
                                        OgAgn.varOrgAddr1,
                                        OgAgn.varOrgAddr2,
                                        OgAgn.varOrgAddr3,
                                        OgAgn.varOrgAddr4,
                                        OgAgn.varOrgCity,
                                        OgAgn.varOrgCountry;";

            var parameters = new { QuoId = quoId, CompanyId = companyId, AgencyId = agencyId };

            var header = await connection.QuerySingleOrDefaultAsync<QuotationHD>(headerSql, parameters);

            return header;
        }

        public async Task<List<ContainerTEU>?> GetContainerTEUAsync(int quoId, int companyId, int agencyId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string containerSql = @"
                SELECT 
	                UOM.varUOMCode
	                varContainerSize, 
	                varContainerType, 
	                intQuantity,
	                intNoOfTEUs, 
	                numGrossWeight, 
	                numCBM, 
	                varRemarks
                FROM QM.QuotationAllocatedCntrs QAC 
                LEFT OUTER JOIN [VP].[MFUOM] UOM ON QAC.intUOMID = UOM.intUOMID
                WHERE intQuoID = @QuoId AND QAC.bitActive = 1";
            var parameters = new { QuoId = quoId, CompanyId = companyId, AgencyId = agencyId };
            var containers = await connection.QueryAsync<ContainerTEU>(containerSql, parameters);
            return containers.ToList();
        }

        public async Task<List<RevenueItm>?> GetRevenueItmAsync(int quoId, int companyId, int agencyId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string revenueSql = @"
                SELECT varchargeCode, intUOMId, varSellCurCode, decSellCurAmount,  
                    decTaxPercentage, decTaxAmount, varRE, varRemarks, intAgentInvoicing, decExchangeRate 
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
                SELECT varchargeCode, intUOMId, varLCurCode, decLCurAmount, decTaxPercentage, 
                decTaxAmount, varRE, varRemarks, intAgentInvoicing, decExchangeRate 
                FROM QM.QuotationShpDt
                WHERE intQuoID = @QuoId AND varRE = 'E' AND bitActive = 1";
            var parameters = new { QuoId = quoId, CompanyId = companyId, AgencyId = agencyId };
            var costs = await connection.QueryAsync<CostItm>(costSql, parameters);
            return costs.ToList();
        }

        public async Task<string?> GetReportUserNameAsync(int intUserID)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            const string headerSql = @"
                SELECT CONCAT ([varFirstName],' ',[varLastName]) AS varUsersName
                FROM [GL].[MFUser]
                WHERE intUserID = @UserID AND bitActive = 1";
            var parameters = new { UserID = intUserID };

            var userName = await connection.QuerySingleOrDefaultAsync<string>(headerSql, parameters);

            return userName;
        }
    }
}
