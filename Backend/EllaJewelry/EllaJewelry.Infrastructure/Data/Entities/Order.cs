using EllaJewelry.Infrastructure.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EllaJewelry.Infrastructure.Data.Entities
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        [Display(Name = "Customer Name")]
        [RegularExpression(@"^[A-Z][a-z]+ [A-Z][a-z]+$", ErrorMessage = "Please enter a valid name (e.g., Ivan Ivanov).")]
        public string CustomerName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        public uint PostalCode { get; set; }

        [Required]
        public Delivery Shipping { get; set; }

        [Required]
        public string ShippingOffice { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        [MaxLength(200)]
        public string GreetingCardName { get; set; }
        public string? Comment { get; set; }
        public string? PromoCode { get; set; }

        [NotMapped]
        public decimal CalculatedTotalPrice => OrderItems.Sum(x => x.Quantity * x.PriceAtPurchase);

        [Required]
        public decimal TotalPrice { get; private set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set;} = DateTime.UtcNow;

        [DataType(DataType.Date)]
        public DateTime? ForecastDateDelivered { get; set;}

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public Order()
        {
            TotalPrice = CalculatedTotalPrice;
        }
    }
}
