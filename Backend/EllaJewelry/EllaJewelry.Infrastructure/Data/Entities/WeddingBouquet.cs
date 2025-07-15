using EllaJewelry.Infrastructure.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllaJewelry.Infrastructure.Data.Entities
{
    public class WeddingBouquet : Product
    {
        public override Category Category => Category.WeddingBouquetService;

        [Required]
        public string Modification { get; set; }

        public WeddingBouquet()
        {
            
        }
    }

    //public class BouquetModification
    //{
    //    public string Name { get; set; }

    //}
}
