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
    public class ElementServices : IElement
    {
        private readonly EllaJewelryDbContext _dbContext;
        private readonly ILogger<ElementServices> _logger;

        public ElementServices(EllaJewelryDbContext dbContext, ILogger<ElementServices> logger)
        {
            _dbContext = dbContext;
            _logger = logger;   
        }
        public async Task CreateAsync(Element item)
        {
            if (item == null)
            {
                _logger.LogWarning("Element passed to Element.CreateAsync() is null.");
                throw new ArgumentNullException(nameof(item));
            }
            _logger.LogInformation("Creating element {Name}...", item.Name);

            _dbContext.Elements.Add(item);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Element {Name} created successfully with ID {ID}.", item.Name, item.ID);
        }

        public async Task DeleteAsync(int key)
        {
            Element item = await ReadAsync(key, false, false);
            if (item is null)
            {
                _logger.LogWarning("Attempted to delete non-existent element with ID {Key}.", key);
                throw new ArgumentException(string.Format($"Element with id {key} does " +
                    $"not exist in the database!"));
            }

            _logger.LogInformation("Deleting Element with ID {Key}...", key);

            _dbContext.Elements.Remove(item);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Element with ID {Key} deleted successfully.", key);
        }

        public async Task<List<Element>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            _logger.LogInformation("Reading all elements. Navigational properties: {UseNav}, ReadOnly: {IsReadOnly}",
                useNavigationalProperties, isReadOnly);

            IQueryable<Element> elements = _dbContext.Elements;

            if (useNavigationalProperties)
            {
                _logger.LogDebug("Including navigational properties: PersonalizedProduct, Service.");
                elements = elements
                    .Include(p => p.PersonalizedProduct)
                    .Include(p => p.Service);
            }

            if (isReadOnly)
            {
                _logger.LogDebug("Query set to AsNoTracking for read-only operation.");
                elements = elements.AsNoTracking();
            }

            var result = await elements.ToListAsync();

            _logger.LogInformation("Retrieved {Count} elements from the database.", result.Count);

            return result;
        }

        public async Task<Element> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            _logger.LogInformation("Reading element with ID {ElementID}...", key);
            IQueryable<Element> query = _dbContext.Elements;

            if (useNavigationalProperties)
            {
                query = query
                    .Include(p => p.PersonalizedProduct)
                    .Include(p => p.Service);
            }

            if (isReadOnly)
            {
                query = query.AsNoTracking();
            }

            Element element = await query.SingleOrDefaultAsync(p => p.ID == key);
            if (element == null)
            {
                _logger.LogWarning("Element with ID {ElementID} not found.", key);
                throw new ArgumentException(nameof(element));
            }

            _logger.LogInformation("Element with ID {ElementID} retrieved successfully.", key);
            return element;

        }

        public async Task UpdateAsync(Element item)
        {
            if (item == null)
            {
                _logger.LogWarning("UpdateAsync called with a null Element.");
                throw new ArgumentNullException(nameof(item));
            }

            _logger.LogInformation("Attempting to update Element with ID {ElementID}.", item.ID);

            bool exists = await _dbContext.Elements.AnyAsync(p => p.ID == item.ID);
            if (!exists)
            {
                _logger.LogWarning("Update failed: Element with ID {ElementID} does not exist.", item.ID);
                throw new ArgumentException($"Element with ID {item.ID} does not exist.");
            }

            _dbContext.Elements.Update(item);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Element with ID {ElementID} updated successfully.", item.ID);
        }
    }
}
