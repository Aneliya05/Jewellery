using EllaJewelry.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllaJewelry.Core.Contracts
{
    public interface IElement : ICRUD<Element, int>
    {
        Task CreateAsync(Element item);

        Task<Element> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true);

        Task<List<Element>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true);

        Task UpdateAsync(Element item);

        Task DeleteAsync(int key);
    }
}
