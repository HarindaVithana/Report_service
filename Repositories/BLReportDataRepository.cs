using Dapper;
using Microsoft.Data.SqlClient;
using voyage_pro_report_service.DTOs;
using voyage_pro_report_service.Interfaces.IRepository;

namespace voyage_pro_report_service.Repositories
{
    public class BLReportDataRepository : IBLReportDataRepository
    {
        private readonly string _connectionString;

        public BLReportDataRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DB")
                ?? throw new InvalidOperationException("Connection not found.");
        }
        public async Task<BLDetailData?> GetBLReportDTAsync(int blId, int companyId, int agencyId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            const string headerSql = @"SELECT
                                            bl.intBLID                                   AS BlId,
                                            bl.varBLNo                                   AS BlNo,
                                            bl.numSerialNo                               AS SerialNo,
                                            ISNULL(bl.bitActive, 0)                      AS IsActive,
                                            COALESCE(bl.dteBLDate, bl.dteCreatedAt)      AS BlDate,
                                            bk.varBookingTypeCode                        AS BookingType,
                                            bk.varCargoType                              AS CargoType,
                                            ext.varBLType                                AS TradeType,
                                            bk.varPreCarrierVsslName                     AS PreCarrierVsslName,
                                            bk.varPreCarrierVoyage                       AS PreCarrierVoyage,
                                            ISNULL(bl.bitSOGenerated, 0)                 AS IsSOGenerated,
                                            bk.intPrncId                                 AS PrncId,
                                            bk.varYardCode                               AS YardCode,
                                            COALESCE(bl.varUCRNo, bk.varUCRNo)           AS UcrNo,
                                            slot.varOrgName                              AS SlotOperatorName,
                                            cntr.varOrgName                              AS ContainerOperatorName,
                                            vi.varVesselCode                             AS VesselCode,
                                            v.varVesselName                              AS VesselName,
                                            vi.varOutVoy                                 AS OutVoyage,
                                            COALESCE(bl.dteETD, vi.dteETD)               AS Etd,
                                            vi.dteETA                                    AS Eta,
                                            COALESCE(bl.varArrService, vi.varArrService) AS ArrivalService,
                                            COALESCE(bl.varDepService, vi.varDepService) AS DepartureService,
                                            ISNULL(v.bitOwnership, 0)                    AS IsOwnVessel,
                                            bl.numArrBookID                              AS ArrBookId,
                                            prnc.varOrgName                              AS PrincipalName,
                                            prnc.varOrgCode                              AS PrincipalCode,
                                            bl.intFwID                                   AS FwId,
                                            bl.intShID                                   AS ShId,
                                            bl.intConsineeID                             AS ConsineeId,
                                            bl.intNPID                                   AS NpId,
                                            bl.varFwName    AS FwName,
                                            bl.varFwAdd1    AS FwAdd1,
                                            bl.varFwAdd2    AS FwAdd2,
                                            bl.varFwAdd3    AS FwAdd3,
                                            bl.varFwAdd4    AS FwAdd4,
                                            bl.varFwCity    AS FwCity,
                                            bl.varFwCntr    AS FwCountry,
                                            bl.varShName    AS ShName,
                                            bl.varShAdd1    AS ShAdd1,
                                            bl.varShAdd2    AS ShAdd2,
                                            bl.varShAdd3    AS ShAdd3,
                                            bl.varShAdd4    AS ShAdd4,
                                            bl.varShCity    AS ShCity,
                                            bl.varShCntr    AS ShCountry,
                                            bl.varConsineeName    AS ConsineeName,
                                            bl.varConsineeAdd1    AS ConsineeAdd1,
                                            bl.varConsineeAdd2    AS ConsineeAdd2,
                                            bl.varConsineeAdd3    AS ConsineeAdd3,
                                            bl.varConsineeAdd4    AS ConsineeAdd4,
                                            bl.varConsineeCity    AS ConsineeCity,
                                            bl.varConsineeCntr    AS ConsineeCountry,
                                            bl.varNPName    AS NpName,
                                            bl.varNPAdd1    AS NpAdd1,
                                            bl.varNPAdd2    AS NpAdd2,
                                            bl.varNPAdd3    AS NpAdd3,
                                            bl.varNPAdd4    AS NpAdd4,
                                            bl.varNPCity    AS NpCity,
                                            bl.varNPCntr    AS NpCountry,
                                            bl.intAgentId       AS AgentId,
                                            bl.varAgentName     AS AgentName,
                                            bl.varAgentAdd1     AS AgentAdd1,
                                            bl.varAgentAdd2     AS AgentAdd2,
                                            bl.varAgentAdd3     AS AgentAdd3,
                                            bl.varAgentAdd4     AS AgentAdd4,
                                            bl.varAgentCity     AS AgentCity,
                                            bl.varAgentPhone    AS AgentPhone,
                                            bl.varAgentEmail    AS AgentEmail,
                                            bl.varPOR     AS PlaceOfReceipt,
                                            bl.varPOL     AS Pol,
                                            bl.varLPOL    AS Lpol,
                                            bl.varPOD     AS Pod,
                                            bl.varFDes    AS FDes,
                                            bl.varPODe    AS PDe,
                                            bl.varMrkandNo         AS MarkAndNos,
                                            bl.varNumofKindofPkgs    AS KindOfPkgs,
                                            bl.varOnCarrierCode    AS OnCarrierCode,
                                            bl.varOnCarrierVoy     AS OnCarrierVoy,
                                            bl.varNextVesselCode   AS NextVesselCode,
                                            bl.varNextVoyage       AS NextVoyage,
                                            bl.intCompanyID    AS CompanyId,
                                            bl.intAgencyID     AS AgencyId,
                                            bk.varTeminalCode                              AS TerminalCode,
                                            COALESCE(bl.dteFCLOpen, bk.dteFCLOpen)         AS FclOpening,
                                            COALESCE(bl.dteFCLClose, bk.dteFCLClose)       AS FclClosing,
                                            COALESCE(bl.dteETS, bk.dteETS)                 AS Ets,
                                            COALESCE(bl.varSertermCode, bk.varSerTermCode) AS TermOfShipment
                                    FROM VP.OPBLHD AS bl
                                    INNER JOIN VP.VIArrivalBook AS vi
                                        ON  vi.intArrBookID = bl.numArrBookID
                                        AND ISNULL(vi.intCompanyID, 0) = bl.intCompanyID
                                        AND ISNULL(vi.intAgencyID, 0)  = bl.intAgencyID
                                    INNER JOIN VP.OPBookingBLDet AS bbl
                                        ON  bbl.intBookingGrpId = bl.intBookingGrpID
                                        AND bbl.intBLId         = bl.intBLID
                                    INNER JOIN VP.OPBookingHd AS bk
                                        ON  bk.intBookingId = bbl.intBookingId
                                        AND bk.intCompanyId = bl.intCompanyID
                                    INNER JOIN VP.GLMFVessel AS v
                                        ON  v.intVesselID = vi.intVesselID
                                    LEFT JOIN VP.GLMFOrganization AS prnc
                                        ON  prnc.intOrgID = bk.intPrncId
                                    LEFT JOIN VP.GLMFOrganization AS slot
                                        ON  slot.intOrgID = bk.intSlotOpr
                                    LEFT JOIN VP.GLMFOrganization AS cntr
                                        ON  cntr.intOrgID = bk.intCntrOpr
                                    LEFT JOIN VP.MFServiceExt AS ext
                                        ON  ext.varServiceCode = ISNULL(bl.varServiceCode, '')
                                        AND ext.intCompanyID   = bl.intCompanyID
                                        AND ext.intAgencyID    = bl.intAgencyID
                                    WHERE bl.intBLID     = @blId
                                      --AND bl.intCompanyID = @companyId
                                      --AND bl.intAgencyID  = @agencyId;";

            var parameters = new { blId, companyId, agencyId };

            var header = await connection.QuerySingleOrDefaultAsync<BLDetailData>(headerSql, parameters);

            return header;
        }

        public async Task<IEnumerable<BLContainerResult>> GetBLContainersAsync(int blId, int companyId, int agencyId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"SELECT
                            c.varContainerNo      AS ContainerNo,
                            c.varSealNo           AS SealNo,
                            c.varISOCode          AS IsoCode,
                            c.varStatus           AS Status,
                            c.varCurrentStatus    AS CurrentStatus,
                            c.varServiceTerm      AS ServiceTerm,
                            c.numGrossWeight      AS GrossWeight,
                            c.numTaraWeight       AS TaraWeight,
                            c.numCBM              AS Cbm,
                            iso.chrCntrSizeCode     AS SizeCode,
                            iso.chrCntrTypeCode     AS TypeCode,
                            iso.varGroupDescription AS GroupDescription
                        FROM VP.OPBLContainerDT AS c
                        OUTER APPLY (
                            SELECT TOP (1)
                                   i.chrCntrSizeCode,
                                   i.chrCntrTypeCode,
                                   i.varGroupDescription
                            FROM VP.GLMFContainerISODet AS i
                            WHERE i.varISOCode = c.varISOCode
                              AND i.bitActive = 1
                            ORDER BY i.varISOCode
                        ) AS iso
                        WHERE c.intBLID     = @blId
                          --AND c.intCompanyID = @companyId
                          --AND c.intAgencyID  = @agencyId;";

            var parameters = new { blId, companyId, agencyId };

            var containers = await connection.QueryAsync<BLContainerResult>(
                new CommandDefinition(sql, parameters));

            return containers.ToList();
        }
    }
}
