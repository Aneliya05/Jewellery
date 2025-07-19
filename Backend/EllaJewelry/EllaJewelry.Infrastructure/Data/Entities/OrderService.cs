using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllaJewelry.Infrastructure.Data.Entities
{
    public class OrderService
    {
        [Key]
        public int ID { get; set; } 

        [Required]
        public string DesireDescription { get; set; }

        public ICollection<Element> Elements { get; set; }

        [ForeignKey("Service")]
        public int ServiceID { get; set; }
        public Service Service { get; set; }

        [ForeignKey("Order")]
        public int OrderID { get; set; }    
        public Order Order { get; set; }
        public OrderService()
        {
            
        }

    }
}
