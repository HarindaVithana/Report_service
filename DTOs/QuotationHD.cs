namespace voyage_pro_report_service.DTOs
{
    public class QuotationHD
    {
        public int intQuoID { get; set; }
        public string? varQuoNo { get; set; }
        public string? varQuoName { get; set; }
        public DateTime dteFromDate { get; set; }
        public DateTime dteToDate { get; set; }
        public int intOrgID { get; set; }
        public string? varBookingType { get; set; }
        public string? varSerTermCode { get; set; }
        public string? varPOL { get; set; }
        public string? varPOD { get; set; }
        public string? varPOOR { get; set; }
        public string? varPOfDelivery { get; set; }
        public string? varShipmentType { get; set; }
        public string? varCommodityCode { get; set; }
        public string? varKindOfPkg { get; set; }
        public string? varGoodsDescription { get; set; }
        public int intPackageType { get; set; }
    }
}
