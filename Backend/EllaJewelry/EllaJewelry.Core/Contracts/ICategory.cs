using EllaJewelry.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllaJewelry.Core.Contracts
{
    public interface ICategory : ICRUD<ProductCategory, int>
    {
        Task CreateAsync(ProductCategory item);

        Task<ProductCategory> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true);

        Task<List<ProductCategory>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true);

        Task UpdateAsync(ProductCategory item);

        Task DeleteAsync(int key);
    }
}
