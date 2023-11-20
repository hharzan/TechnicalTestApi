using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace TechnicalTestAPI.Models
{
    [Table("Debtor")]
    public class Debtor
    {

        // What is the relationship between a debtor and a creditor?
        // need to create a creditor class as well? but defined payload does not include a reference to creditor. 
        // if that's the case, the relationship is only limited between debtor and receivable e.g. 1 debtor can have many receivable records.
        public Debtor() 
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            CountryCode = string.Empty;
        }

        public Debtor(Guid id, string name, string? address1, string? address2, string? town, string? state, string? zipCode, string countryCode, string? registrationNumber)
        {
            Id = id;
            Name = name;
            Address1 = address1;
            Address2 = address2;
            Town = town;
            State = state;
            ZipCode = zipCode;
            CountryCode = countryCode;
            RegistrationNumber = registrationNumber;
        }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? Town { get; set; }

        public string? State { get; set; }

        public string? ZipCode { get; set; }

        public string CountryCode { get; set; }

        public string? RegistrationNumber { get; set; }
    }
}
