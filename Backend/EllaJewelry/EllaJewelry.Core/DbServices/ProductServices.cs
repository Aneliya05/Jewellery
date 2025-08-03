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
                throw new ArgumentException(string.Format($"Product with id {key} does " +
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
