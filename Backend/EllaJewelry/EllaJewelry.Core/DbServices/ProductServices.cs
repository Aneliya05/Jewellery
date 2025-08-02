using EllaJewelry.Infrastructure.Data.Entities;
using EllaJewelry.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EllaJewelry.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EllaJewelry.Core.DbServices
{
    public class ProductServices : ICRUD<Product, int>
    {
        private readonly EllaJewelryDbContext _dbContext;

        public ProductServices(EllaJewelryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region CRUD For Products
        public async Task CreateAsync(Product item)
        {
            _dbContext.Products.Add(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int key)
        {
            Product item = await ReadAsync(key, false, false);
            if (item is null)
            {
                throw new ArgumentException(string.Format($"Hotel with id {key} does " +
                    $"not exist in the database!"));
            }
            _dbContext.Products.Remove(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Product>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            IQueryable<Product> products = _dbContext.Products;
            if (isReadOnly)
            {
                products = products.AsNoTracking();
            }
            return await products.ToListAsync();
        }

        public async Task<Product> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            IQueryable<Product> products = _dbContext.Products;

            if (isReadOnly)
            {
                products = products.AsNoTracking();
            }
            return await products.SingleOrDefaultAsync(x => x.ID == key);
        }

        public async Task UpdateAsync(Product item)
        {
            _dbContext.Products.Update(item);
            await _dbContext.SaveChangesAsync();
        }

        #endregion  
    }
}
