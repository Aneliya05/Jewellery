using EllaJewelry.Core.Contracts;
using EllaJewelry.Infrastructure.Data;
using EllaJewelry.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EllaJewelry.Core.DbServices
{
    public class ImageServices : IImage
    {
        private readonly EllaJewelryDbContext _dbContext;
        private readonly ILogger<ImageServices> _logger;
        public ImageServices(EllaJewelryDbContext dbContext, ILogger<ImageServices> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
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

        public async Task<ProductImage> ShowImageThumbnail(int productID, bool useNavigationalProperties = false, bool isReadOnly = true)
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

            ProductImage image = await images.SingleOrDefaultAsync(p => p.ProductID == productID);
            if (image == null)
            {
                _logger.LogWarning("Image with ID {ProductID} not found.", productID);
                throw new ArgumentException(nameof(productID));
            }
            _logger.LogInformation("Image with ID {ProductID} retrieved successfully.", productID);
            return image;
        }
        public async Task DeleteImage(int productID, int imageID)
        {
            Product product = await _dbContext.Products.FindAsync(productID);
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
            Product product = await _dbContext.Products.FindAsync(productID);
            return product.Category;
        }
    }
}
