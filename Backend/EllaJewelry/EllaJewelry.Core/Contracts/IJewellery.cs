using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllaJewelry.Core.Contracts
{
    public interface IJewellery
    {
        ICategory Categories { get; }
        IElement Elements { get; }
        IImage Images { get; }
        IOrder Orders { get; }
        IProduct Products { get; }
        IService Services { get; }
    }
}
