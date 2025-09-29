using EllaJewelry.Infrastructure.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllaJewelry.Infrastructure.Data.Entities
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Give a name to your incredible jewelry")]
        public string Name { get; set; }

        [Required]
        [Range(0.01, 100000)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Z0-9\-]+$", ErrorMessage = "SKU must be alphanumeric with dashes.")]
        public string SKU { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Availability cannot be negative.")]
        public uint Availability { get; set; } //available quantity

        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; } //string?

        [Required]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [ForeignKey(nameof(CategoryID))]    
        public ProductCategory Category { get; set; } = null!;

        public ICollection<ProductImage> Images { get; set; }  = new List<ProductImage>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public Product()
        {
            
        }
    }
}
