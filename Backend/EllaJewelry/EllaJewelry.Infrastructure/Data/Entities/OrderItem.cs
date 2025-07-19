using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace EllaJewelry.Infrastructure.Data.Entities
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [AllowNull]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string DesireDescription { get; set; } //string?
        [ForeignKey("Order")]
        public int OrderID { get; set; }
        public Order Order { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public Product Product { get; set; }

        [ForeignKey("Service")]
        public int ServiceID { get; set; }
        public Service Service { get; set; }


        [Required]
        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        public decimal PriceAtPurchase { get; private set; }
    }
}
