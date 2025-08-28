using EllaJewelry.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllaJewelry.Core.Contracts
{
    public interface IOrder : ICRUD<Order, int>
    {
        Task CreateAsync(Order item);

        Task<Order> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true);

        Task<List<Order>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true);

        Task UpdateAsync(Order item);

        Task DeleteAsync(int key);
    }
}
