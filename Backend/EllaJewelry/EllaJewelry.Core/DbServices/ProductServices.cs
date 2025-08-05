using EllaJewelry.Infrastructure.Data.Entities;
using EllaJewelry.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EllaJewelry.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EllaJewelry.Core.DbServices
{
    public class ProductServices : ICRUD<Product, int>
    {
        private readonly EllaJewelryDbContext _dbContext;
        private readonly ILogger<ProductServices> _logger;

        public ProductServices(EllaJewelryDbContext dbContext, ILogger<ProductServices> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        #region CRUD For Products

        /// <summary>
        /// Creates a new product.
        /// </summary>
        public async Task CreateAsync(Product item)
        {
            if (item == null)
            {
                _logger.LogWarning("Product passed to Product.CreateAsync() is null.");
                throw new ArgumentNullException(nameof(item));
            }
            _logger.LogInformation("Creating product {Name}...", item.Name);

            _dbContext.Products.Add(item);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Product {Name} created successfully with ID {ID}.", item.Name, item.ID);
        }

        /// <summary>
        /// Deletes a product by ID.
        /// </summary>

        public async Task DeleteAsync(int key)
        {
            Product item = await ReadAsync(key, false, false);
            if (item is null)
            {
                _logger.LogWarning("Attempted to delete non-existent product with ID {Key}.", key);
                throw new ArgumentException(string.Format($"Product with id {key} does " +
                    $"not exist in the database!"));
            }

            _logger.LogInformation("Deleting Product with ID {Key}...", key);

            _dbContext.Products.Remove(item);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Product with ID {Key} deleted successfully.", key);
        }

        /// <summary>
        /// Returns all products.
        /// </summary>
        public async Task<List<Product>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            _logger.LogInformation("Reading all products. Navigational properties: {UseNav}, ReadOnly: {IsReadOnly}",
                useNavigationalProperties, isReadOnly);

            IQueryable<Product> products = _dbContext.Products;

            if (useNavigationalProperties)
            {
                _logger.LogDebug("Including navigational properties: Category, Images.");
                products = products
                    .Include(p => p.Category)
                    .Include(p => p.Images);
            }

            if (isReadOnly)
            {
                _logger.LogDebug("Query set to AsNoTracking for read-only operation.");
                products = products.AsNoTracking();
            }

            var result = await products.ToListAsync();

            _logger.LogInformation("Retrieved {Count} products from the database.", result.Count);

            return result;
        }



        /// <summary>
        /// Reads a single product by ID.
        /// </summary>
        public async Task<Product> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            _logger.LogInformation("Reading product with ID {ProductID}...", key);
            IQueryable<Product> query = _dbContext.Products;
   
            if (useNavigationalProperties)
            {
                query = query
                    .Include(p => p.Category)
                    .Include(p => p.Images);
            }

            if (isReadOnly)
            {
                query = query.AsNoTracking();
            }

            Product product = await query.SingleOrDefaultAsync(p => p.ID == key);
            if(product == null)
            {
                _logger.LogWarning("Product with ID {ProductID} not found.", key);
                throw new ArgumentException(nameof(product));
            }
            _logger.LogInformation("Product with ID {ProductID} retrieved successfully.", key);
            return product;
        }


        /// <summary>
        /// Updates an existing product.
        /// </summary>

        public async Task UpdateAsync(Product item)
        {
            if (item == null)
            {
                _logger.LogWarning("UpdateAsync called with a null Product.");
                throw new ArgumentNullException(nameof(item));
            }

            _logger.LogInformation("Attempting to update Product with ID {ProductID}.", item.ID);

            bool exists = await _dbContext.Products.AnyAsync(p => p.ID == item.ID);
            if (!exists)
            {
                _logger.LogWarning("Update failed: Product with ID {ProductID} does not exist.", item.ID);
                throw new ArgumentException($"Product with ID {item.ID} does not exist.");
            }

            _dbContext.Products.Update(item);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Product with ID {ProductID} updated successfully.", item.ID);
        }


        #endregion

        #region CRUD For Images
        public async Task AddImages(int productID, string imagePath)
        {
            ProductImage image = new ProductImage(imagePath, productID);
            //Product product = await ReadAsync(productID);
            //product.Images.Add(image);
            _dbContext.ProductImages.Add(image);
            await _dbContext.SaveChangesAsync();
        }
    
        public async Task<List<ProductImage>> ShowProductImages(int productID, bool useNavigationalProperties = true, bool isReadOnly = true)
        {
            IQueryable<ProductImage> images = _dbContext.ProductImages.Where(x => x.ProductID == productID);
            if (useNavigationalProperties)
            {
                images = images.Include(x => x.Product);
            }
            if (isReadOnly)
            {
                images = images.AsNoTrackingWithIdentityResolution();
            }
            return await images.ToListAsync();
        }
        public async Task DeleteImage(int productID, int imageID)
        {
            Product product = await ReadAsync(productID, false, false);
            if (product is null)
            {
                throw new ArgumentException(string.Format($"Product with id {productID} does " +
                    $"not exist in the database!"));
            }
            IEnumerable<ProductImage> productImages = await ShowProductImages(productID);
            foreach (var image in productImages)
            {
                if (image.Id == imageID)
                {
                    _dbContext.ProductImages.Remove(image);
                    product.Images.Remove(image);
                }
            }

            await _dbContext.SaveChangesAsync();
        }
        #endregion

        public async Task<ProductCategory> ViewCategory(int productID)
        {
            Product product = await ReadAsync(productID);
            return product.Category;
        }
    }
}
