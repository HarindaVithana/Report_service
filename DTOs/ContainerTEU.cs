namespace voyage_pro_report_service.DTOs
{
    public class ContainerTEU
    {
        public string? varContainerSize { get; set; }
        public string? varContainerType { get; set; }
        public int intQuantity { get; set; }
        public int intNoOfTEUs { get; set; }
        public double numGrossWeight { get; set; }
        public double numCBM { get; set; }
        public string? varRemarks { get; set; } = null;
    }
}
