using CustomerListing.DB.Domain;
using CustomerListing.DB.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerListing.Persistence.V1
{
    public class RequestResponseBase : EntityBase
    {
        public Guid CustomerId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        public string Cellphone { get; set; }
        public decimal AmountTotal { get; set; }
        [Required]
        public CustomerType Type { get; set; }
    }
}
