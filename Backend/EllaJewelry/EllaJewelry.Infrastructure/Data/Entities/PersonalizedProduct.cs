using EllaJewelry.Infrastructure.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace EllaJewelry.Infrastructure.Data.Entities
{
    public class PersonalizedProduct : Product
    {
        public override Category Category => Category.PersonalizedNecklace;

        public ICollection<Element> Elements { get; set; } = new List<Element>();
        public PersonalizedProduct()
        {
            
        }
    }

    public class Element
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; } // e.g., "Charm", "Pendant", "Engraving"

        [StringLength(100)]
        public string Value { get; set; } // e.g., "Heart Shape", "Name: Ella"

        [AllowNull]
        public int Order { get; set; } // For sorting the elements in a necklace

        [ForeignKey("PersonalizedProduct")]
        public int ProductID { get; set; }

        public PersonalizedProduct PersonalizedProduct { get; set; }
        public Element()
        {
            
        }
    }
}
