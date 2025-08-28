using EllaJewelry.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllaJewelry.Core.Contracts
{
    public interface IService : ICRUD<Service, int>
    {
        Task CreateAsync(Service item);

        Task<Service> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true);

        Task<List<Service>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true);

        Task UpdateAsync(Service item);

        Task DeleteAsync(int key);
    }
}
