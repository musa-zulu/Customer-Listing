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
        public string Email { get; set; }
        [MaxLength(10)]
        public string Cellphone { get; set; }
        public decimal AmountTotal { get; set; }
    }
}
