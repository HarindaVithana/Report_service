namespace voyage_pro_report_service.DTOs
{
    public class RevenueItm
    {
        public string? varchargeCode { get; set; }
        public int intUOMId { get; set; }
        public string? varSellCurCode { get; set; }
        public decimal decSellCurAmount { get; set; }
        public decimal decTaxPercentage { get; set; }
        public decimal decTaxAmount { get; set; }
        public string? varRemarks { get; set; } = null;
    }
}
