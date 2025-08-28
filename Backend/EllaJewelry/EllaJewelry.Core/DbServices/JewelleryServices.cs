using EllaJewelry.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllaJewelry.Core.DbServices
{
    public class JewelleryServices : IJewellery
    {
        public ICategory Categories { get; }
        public IElement Elements { get; }
        public IImage Images { get; }
        public IOrder Orders { get; }
        public IProduct Products { get; }
        public IService Services { get; }

        public JewelleryServices(ICategory categoryServices, IElement elementServices, IImage imageServices, 
            IOrder orderServices, IProduct productServices, IService servicesServices )
        {
            Categories = categoryServices;
            Elements = elementServices;
            Images = imageServices;
            Orders = orderServices;
            Products = productServices;
            Services = servicesServices;
        }
    }
}
