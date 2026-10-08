namespace voyage_pro_report_service.DTOs
{
    public class BLContainerResult
    {
        public string? ContainerNo { get; set; }
        public string? SealNo { get; set; }
        public string? IsoCode { get; set; }
        public string? Status { get; set; }
        public string? CurrentStatus { get; set; }
        public string? ServiceTerm { get; set; }
        public decimal? GrossWeight { get; set; }
        public decimal? TaraWeight { get; set; }
        public decimal? Cbm { get; set; }
        public string? SizeCode { get; set; }
        public string? TypeCode { get; set; }
        public string? GroupDescription { get; set; }
    }
}
