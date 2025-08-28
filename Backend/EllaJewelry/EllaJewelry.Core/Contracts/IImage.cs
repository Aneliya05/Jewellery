using EllaJewelry.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllaJewelry.Core.Contracts
{
    public interface IImage
    {
        Task AddImages(int productID, string imagePath);

        Task<List<ProductImage>> ShowProductImages(int productID, bool useNavigationalProperties = true, bool isReadOnly = true);

        Task DeleteImage(int productID, int imageID);

        Task<ProductCategory> ViewCategory(int productID);
    }
}
