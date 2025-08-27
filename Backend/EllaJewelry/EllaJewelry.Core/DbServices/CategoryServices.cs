using EllaJewelry.Core.Contracts;
using EllaJewelry.Infrastructure.Data;
using EllaJewelry.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ProductCategory> _logger;

        public CategoryServices(EllaJewelryDbContext dbContext, ILogger<ProductCategory> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        #region CRUD For Categories

        /// <summary>
        /// Creates a new product.
        /// </summary>
        public async Task CreateAsync(ProductCategory item)
        {
            if (item == null)
            {
                _logger.LogWarning("Category passed to ProductCategory.CreateAsync() is null.");
                throw new ArgumentNullException(nameof(item));
            }
            _logger.LogInformation("Creating ProductCategory {ProductCategory.Id}...", item.Id);

            _dbContext.ProductCategories.Add(item);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("ProductCategory {Name} created successfully with ID {ID}.", item.Name, item.Id);
        }

        /// <summary>
        /// Deletes a product by ID.
        /// </summary>

        public async Task DeleteAsync(int key)
        {
            ProductCategory item = await ReadAsync(key, false, false);
            if (item is null)
            {
                _logger.LogWarning("Attempted to delete non-existent ProductCategory with ID {Key}.", key);
                throw new ArgumentException(string.Format($"ProductCategory with id {key} does " +
                    $"not exist in the database!"));
            }

            _logger.LogInformation("Deleting ProductCategory with ID {Key}...", key);

            _dbContext.ProductCategories.Remove(item);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("ProductCategory with ID {Key} deleted successfully.", key);
        }

        /// <summary>
        /// Returns all products.
        /// </summary>
        public async Task<List<ProductCategory>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            _logger.LogInformation("Reading all product categories. Navigational properties: {UseNav}, ReadOnly: {IsReadOnly}",
                useNavigationalProperties, isReadOnly);

            IQueryable<ProductCategory> productCategories = _dbContext.ProductCategories;

            if (useNavigationalProperties)
            {
                _logger.LogDebug("Including navigational properties: Products.");
                productCategories = productCategories
                    .Include(p => p.Products);
            }

            if (isReadOnly)
            {
                _logger.LogDebug("Query set to AsNoTracking for read-only operation.");
                productCategories = productCategories.AsNoTracking();
            }

            var result = await productCategories.ToListAsync();

            _logger.LogInformation("Retrieved {Count} Product Categories from the database.", result.Count);

            return result;
        }

        /// <summary>
        /// Reads a single product by ID.
        /// </summary>
        public async Task<ProductCategory> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            _logger.LogInformation("Reading ProductCategory with ID {ID}...", key);
            IQueryable<ProductCategory> query = _dbContext.ProductCategories;

            if (useNavigationalProperties)
            {
                query = query
                    .Include(p => p.Products);
            }

            if (isReadOnly)
            {
                query = query.AsNoTracking();
            }

            ProductCategory category = await query.SingleOrDefaultAsync(p => p.Id == key);
            if (category == null)
            {
                _logger.LogWarning("ProductCategory with ID {ID} not found.", key);
                throw new ArgumentException(nameof(category));
            }
            _logger.LogInformation("ProductCategory with ID {ID} retrieved successfully.", key);
            
            return category;
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>

        public async Task UpdateAsync(ProductCategory item)
        {
            if (item == null)
            {
                _logger.LogWarning("UpdateAsync called with a null ProductCategory.");
                throw new ArgumentNullException(nameof(item));
            }

            _logger.LogInformation("Attempting to update ProductCategory with ID {ID}.", item.Id);

            bool exists = await _dbContext.ProductCategories.AnyAsync(p => p.Id == item.Id);
            if (!exists)
            {
                _logger.LogWarning("Update failed: ProductCategory with ID {ID} does not exist.", item.Id);
                throw new ArgumentException($"ProductCategory with ID {item.Id} does not exist.");
            }

            _dbContext.ProductCategories.Update(item);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("ProductCategory with ID {ID} updated successfully.", item.Id);
        }
        #endregion
    }
}
