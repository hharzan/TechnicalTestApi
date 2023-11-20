namespace TechnicalTestAPI.Models
{
    public class ReceivableStatistics
    {
        public ReceivableStatistics()
        {

        }

        public Guid DebtorId { get; set; }

        public int? OpenInvoiceCount { get; set; }

        public double? OpenInvoiceTotalValue { get; set; }

        public List<Receivable>? OverdueReceivables { get; set; }

        public List<Receivable>? ImminentReceivables { get; set; }
        
        public int? ClosedInvoiceCount { get; set; }

        public double? ClosedInvoiceTotalValue { get; set; }

        public double? CancelledInvoicePercentage { get; set; }

        public double? CompletedInvoicePercentage { get; set; }
    }
}
