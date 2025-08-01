﻿using System;
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

        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string? DesireDescription { get; set; } //string?

        public int OrderID { get; set; }
        
        [ForeignKey("OrderID")]
        public Order Order { get; set; }

        public int? ProductID { get; set; }

        [ForeignKey(nameof(ProductID))]
        public Product? Product { get; set; }

        public int? ServiceID { get; set; }
        
        [ForeignKey(nameof(ServiceID))]
        public Service? Service { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        public decimal PriceAtPurchase { get; private set; }

        public OrderItem()
        {
            
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ProductID == null && ServiceID == null)
            {
                yield return new ValidationResult(
                    "Either ProductID or ServiceID must be provided.",
                    new[] { nameof(ProductID), nameof(ServiceID) });
            }

            if (ProductID != null && ServiceID != null)
            {
                yield return new ValidationResult(
                    "You cannot set both ProductID and ServiceID at the same time.",
                    new[] { nameof(ProductID), nameof(ServiceID) });
            }
        }
    }
}
