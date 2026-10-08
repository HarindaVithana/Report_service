namespace voyage_pro_report_service.DTOs
{
    public class BLDetailData
    {
        public int BlId { get; set; }
        public int ArrBookId { get; set; }
        public string? BlNo { get; set; }
        public int? SerialNo { get; set; }
        public bool IsActive { get; set; }
        public DateTime? BlDate { get; set; }
        public string? BookingType { get; set; }
        public string? CargoType { get; set; }
        public string? TradeType { get; set; }
        public string? PreCarrierVsslName { get; set; }
        public string? PreCarrierVoyage { get; set; }
        public bool IsSOGenerated { get; set; }
        public int? PrncId { get; set; }
        public string? YardCode { get; set; }
        public string? UcrNo { get; set; }
        public string? SlotOperatorName { get; set; }
        public string? ContainerOperatorName { get; set; }
        public string? VesselCode { get; set; }
        public string? VesselName { get; set; }
        public string? OutVoyage { get; set; }
        public DateTime? Etd { get; set; }
        public DateTime? Eta { get; set; }
        public string? ArrivalService { get; set; }
        public string? DepartureService { get; set; }
        public bool IsOwnVessel { get; set; }
        public string? TerminalCode { get; set; }
        public DateTime? FclOpening { get; set; }
        public DateTime? FclClosing { get; set; }
        public DateTime? Ets { get; set; }
        public string? PrincipalName { get; set; }
        public string? PrincipalCode { get; set; }
        public int? FwId { get; set; }
        public int? ShId { get; set; }
        public int? ConsineeId { get; set; }
        public int? NpId { get; set; }
        public string? FwName { get; set; }
        public string? FwAdd1 { get; set; }
        public string? FwAdd2 { get; set; }
        public string? FwAdd3 { get; set; }
        public string? FwAdd4 { get; set; }
        public string? FwCity { get; set; }
        public string? FwCountry { get; set; }

        // Shipper
        public string? ShName { get; set; }
        public string? ShAdd1 { get; set; }
        public string? ShAdd2 { get; set; }
        public string? ShAdd3 { get; set; }
        public string? ShAdd4 { get; set; }
        public string? ShCity { get; set; }
        public string? ShCountry { get; set; }

        // Consignee
        public string? ConsineeName { get; set; }
        public string? ConsineeAdd1 { get; set; }
        public string? ConsineeAdd2 { get; set; }
        public string? ConsineeAdd3 { get; set; }
        public string? ConsineeAdd4 { get; set; }
        public string? ConsineeCity { get; set; }
        public string? ConsineeCountry { get; set; }

        // Notify Party
        public string? NpName { get; set; }
        public string? NpAdd1 { get; set; }
        public string? NpAdd2 { get; set; }
        public string? NpAdd3 { get; set; }
        public string? NpAdd4 { get; set; }
        public string? NpCity { get; set; }
        public string? NpCountry { get; set; }

        // Delivery / Local Agent
        public int? AgentId { get; set; }
        public string? AgentName { get; set; }
        public string? AgentAdd1 { get; set; }
        public string? AgentAdd2 { get; set; }
        public string? AgentAdd3 { get; set; }
        public string? AgentAdd4 { get; set; }
        public string? AgentCity { get; set; }
        public string? AgentPhone { get; set; }
        public string? AgentEmail { get; set; }

        // Port Details
        public string? PlaceOfReceipt { get; set; }
        public string? Pol { get; set; }
        public string? Lpol { get; set; }
        public string? Pod { get; set; }
        public string? FDes { get; set; }
        public string? PDe { get; set; }

        // Term of Shipment (CY/CY etc.)
        public string? TermOfShipment { get; set; }

        // Cargo
        public string? MarkAndNos { get; set; }
        public string? MarkAndNos2 { get; set; }
        public string? KindOfPkgs { get; set; }
        public string? KindOfPkgs2 { get; set; }
        public string? ContainerNos { get; set; }

        // Computed for B/L print (populated by the report renderer)
        public string? ContainerCountSummary { get; set; }   // e.g. "2 X 20'DRY / 1 X 40'DRY"
        public string? ContainerCountWord { get; set; }      // e.g. "TWO"
        public string? ContainerDetails { get; set; }        // container + seal numbers
        public int TotalContainers { get; set; }
        public string? ContainerModeCode { get; set; }       // FCL / LCL
        public string? ShipmentLoadDesc { get; set; }
        public int TotalPackages { get; set; }
        public string? PackageUom { get; set; }
        public decimal? TotalNetWeight { get; set; }

        // Free Hand Routing (stored in OPBLHD)
        public string? OnCarrierCode { get; set; }
        public string? OnCarrierVoy { get; set; }
        public string? NextVesselCode { get; set; }
        public string? NextVoyage { get; set; }

        // Tenant
        public int CompanyId { get; set; }
        public int AgencyId { get; set; }

        //Print Data
        public string? PrintAs { get; set; }
        public int NoOfOriginalBLs { get; set; }
        public string? PrePaidAt { get; set; }
        public string? AsCarrier { get; set; }
        public DateTime? ShippedOnBoard { get; set; }
        public bool PrintOnBoard { get; set; }

        //Logo
        public byte[]? LogoData { get; set; }
    }
}
