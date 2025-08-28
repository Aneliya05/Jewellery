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
    public class ServiceServices : IService
    {
        private readonly EllaJewelryDbContext _dbContext;
        private readonly ILogger<ServiceServices> _logger;

        public ServiceServices(EllaJewelryDbContext dbContext, ILogger<ServiceServices> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        #region CRUD For Services

        /// <summary>
        /// Creates a new product.
        /// </summary>
        public async Task CreateAsync(Service item)
        {
            if (item == null)
            {
                _logger.LogWarning("Service passed to Service.CreateAsync() is null.");
                throw new ArgumentNullException(nameof(item));
            }
            _logger.LogInformation("Creating Service {Name}...", item.Name);

            _dbContext.Services.Add(item);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Service {Name} created successfully with ID {ID}.", item.Name, item.Id);
        }

        /// <summary>
        /// Deletes a product by ID.
        /// </summary>

        public async Task DeleteAsync(int key)
        {
            Service item = await ReadAsync(key, false, false);
            if (item is null)
            {
                _logger.LogWarning("Attempted to delete non-existent service with ID {Key}.", key);
                throw new ArgumentException(string.Format($"Service with id {key} does " +
                    $"not exist in the database!"));
            }

            _logger.LogInformation("Deleting Service with ID {Key}...", key);

            _dbContext.Services.Remove(item);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Service with ID {Key} deleted successfully.", key);
        }

        /// <summary>
        /// Returns all products.
        /// </summary>
        public async Task<List<Service>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            _logger.LogInformation("Reading all services. Navigational properties: {UseNav}, ReadOnly: {IsReadOnly}",
                useNavigationalProperties, isReadOnly);

            IQueryable<Service> services = _dbContext.Services;

            if (useNavigationalProperties)
            {
                _logger.LogDebug("Including navigational properties: Elements.");
                services = services
                    .Include(p => p.Elements);
            }

            if (isReadOnly)
            {
                _logger.LogDebug("Query set to AsNoTracking for read-only operation.");
                services = services.AsNoTracking();
            }

            var result = await services.ToListAsync();

            _logger.LogInformation("Retrieved {Count} services from the database.", result.Count);

            return result;
        }

        /// <summary>
        /// Reads a single product by ID.
        /// </summary>
        public async Task<Service> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            _logger.LogInformation("Reading service with ID {ServiceID}...", key);
            IQueryable<Service> query = _dbContext.Services;

            if (useNavigationalProperties)
            {
                query = query
                    .Include(p => p.Elements);
            }

            if (isReadOnly)
            {
                query = query.AsNoTracking();
            }

            Service service = await query.SingleOrDefaultAsync(p => p.Id == key);
            if (service == null)
            {
                _logger.LogWarning("Service with ID {Service ID} not found.", key);
                throw new ArgumentException(nameof(service));
            }
            _logger.LogInformation("Service  with ID {Service ID} retrieved successfully.", key);
            return service;
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>

        public async Task UpdateAsync(Service item)
        {
            if (item == null)
            {
                _logger.LogWarning("UpdateAsync called with a null Service.");
                throw new ArgumentNullException(nameof(item));
            }

            _logger.LogInformation("Attempting to update Service  with ID {Service ID}.", item.Id);

            bool exists = await _dbContext.Services.AnyAsync(p => p.Id == item.Id);
            if (!exists)
            {
                _logger.LogWarning("Update failed: Service with ID {Service ID} does not exist.", item.Id);
                throw new ArgumentException($"Service with ID {item.Id} does not exist.");
            }

            _dbContext.Services.Update(item);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Service with ID {Service ID} updated successfully.", item.Id);
        }
        #endregion

    }
}
