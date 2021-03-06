using CustomerListing.DB.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerListing.DB.Domain
{
    public class Customer
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
        public CustomerType Type { get; set; }
    }
}
