using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EllaJewelry.Infrastructure.Data.Enums;

namespace EllaJewelry.Infrastructure.Data.Entities
{
    public class Element
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public ItemCategory ItemCategory { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; } // e.g., "Charm", "Pendant", "Engraving"

        [StringLength(100)]
        public string? Name { get; set; } // e.g., "Heart Shape", "Name: Ella"
        public int? NumberInSorting { get; set; } // For sorting the elements in a necklace
        
        public int? PersonalizedProductID { get; set; }
        public int? ServiceID { get; set; }

        [ForeignKey(nameof(PersonalizedProductID))]
        public PersonalizedProduct? PersonalizedProduct { get; set; }

        [ForeignKey(nameof(ServiceID))]
        public Service? Service { get; set; }
        public Element()
        {

        }
    }
}
