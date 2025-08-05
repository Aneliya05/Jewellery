using EllaJewelry.Core.Contracts;
using EllaJewelry.Infrastructure.Data;
using EllaJewelry.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllaJewelry.Core.DbServices
{
    public class ElementServices : ICRUD<Element, int>
    {
        private EllaJewelryDbContext _dbContext;

        public ElementServices(EllaJewelryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateAsync(Element item)
        {
            _dbContext.Elements.Add(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int key)
        {
            Element element = await ReadAsync(key);   
            if(element is null)
            {
                throw new ArgumentException(string.Format($"Element with id {key} does " +
                    $"not exist in the database!"));
            }
            _dbContext.Elements.Remove(element);
            await _dbContext.SaveChangesAsync();    
        }

        public async Task<List<Element>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            IQueryable<Element> elements = _dbContext.Elements;

            if (isReadOnly)
            {
                elements = elements.AsNoTracking();
            }
            return await elements.ToListAsync();
        }

        public async Task<Element> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            IQueryable<Element> elements = _dbContext.Elements;

            if (isReadOnly)
            {
                elements = elements.AsNoTracking();
            }
            return await elements.SingleOrDefaultAsync(x => x.ID == key);
        }

        public async Task UpdateAsync(Element item)
        {
            _dbContext.Elements.Update(item);
            await _dbContext.SaveChangesAsync();
        }
    }
}
