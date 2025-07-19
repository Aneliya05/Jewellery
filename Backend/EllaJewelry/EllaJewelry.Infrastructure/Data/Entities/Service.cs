using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllaJewelry.Infrastructure.Data.Entities
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Give a name to your new services")]
        public string Name { get; set; }

        [Required]
        [Range(0.01, 100000)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [AllowNull]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } //string?

        [Required]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
        public Service()
        {
            
        }
    }
}
