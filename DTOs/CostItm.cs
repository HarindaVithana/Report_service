namespace voyage_pro_report_service.DTOs
{
    public class CostItm
    {
        public string? varchargeCode { get; set; }
        public int intUOMId { get; set; }
        public string? varLCurCode { get; set; }
        public decimal decLCurAmount { get; set; }
        public decimal decTaxPercentage { get; set; }
        public decimal decTaxAmount { get; set; }
        public string? varRE { get; set; } = null;
        public string? varRemarks { get; set; } = null;
        public int intAgentInvoicing { get; set; }
        public decimal decExchangeRate { get; set; }
    }
}
