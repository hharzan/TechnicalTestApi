using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TechnicalTestAPI.Models
{
    [Table("Receivable")]
    public class Receivable
    {
        public Receivable() 
        {
            Id = Guid.NewGuid();
            CurrencyCode = string.Empty; 
            IsCancelled = false;
        }


        [Key]
        public Guid Id { get; set; }

        public string CurrencyCode { get; set; }

        public string IssueDate { get; set; }

        public string OpeningValue { get; set; }

        public string PaidValue { get; set; }

        public string DueDate { get; set; }

        public string? ClosedDate { get; set; }

        public bool IsCancelled { get; set; }

        public string DebtorId { get; set; }
        
        public string DebtorName { get; set; }
        
        public string DebtorCountryCode { get; set; }


    }
}
