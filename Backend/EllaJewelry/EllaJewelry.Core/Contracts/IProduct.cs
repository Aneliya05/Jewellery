using EllaJewelry.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllaJewelry.Core.Contracts
{
    public interface IProduct : ICRUD<Product, int>
    {
        Task CreateAsync(Product item);

        Task<Product> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true);

        Task<List<Product>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true);

        Task UpdateAsync(Product item);

        Task DeleteAsync(int key);
    }
}
