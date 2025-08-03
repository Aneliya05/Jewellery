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
    public class CategoryServices : ICRUD<ProductCategory, int>
    {
        private readonly EllaJewelryDbContext _dbContext;

        public CategoryServices(EllaJewelryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateAsync(ProductCategory item)
        {
            _dbContext.ProductCategories.Add(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int key)
        {
            ProductCategory item = await ReadAsync(key, false, false);
            if (item is null)
            {
                throw new ArgumentException(string.Format($"Category with id {key} does " +
                    $"not exist in the database!"));
            }
            _dbContext.ProductCategories.Remove(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ProductCategory>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            IQueryable<ProductCategory> categories = _dbContext.ProductCategories;
            if (isReadOnly)
            {
                categories = categories.AsNoTracking();
            }
            return await categories.ToListAsync();
        }

        public async Task<ProductCategory> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            IQueryable<ProductCategory> categories = _dbContext.ProductCategories;

            if (isReadOnly)
            {
                categories = categories.AsNoTracking();
            }
            return await categories.SingleOrDefaultAsync(x => x.Id == key);
        }

        public async Task UpdateAsync(ProductCategory item)
        {
            _dbContext.ProductCategories.Update(item);
            await _dbContext.SaveChangesAsync();
        }
    }
}
