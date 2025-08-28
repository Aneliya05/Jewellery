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
    public class OrderServices : IOrder
    {
        private readonly EllaJewelryDbContext _dbContext;
        private readonly ILogger<OrderServices> _logger;

        public OrderServices(EllaJewelryDbContext dbContext, ILogger<OrderServices> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        #region CRUD For Orders

        /// <summary>
        /// Creates a new Order.
        /// </summary>
        public async Task CreateAsync(Order item)
        {
            if (item == null)
            {
                _logger.LogWarning("Order passed to Order.CreateAsync() is null.");
                throw new ArgumentNullException(nameof(item));
            }
            _logger.LogInformation("Creating Order {ID}...", item.OrderID);

            _dbContext.Orders.Add(item);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Order created successfully with ID {ID}.",item.OrderID);
        }

        /// <summary>
        /// Deletes a Order by ID.
        /// </summary>

        public async Task DeleteAsync(int key)
        {
            Order item = await ReadAsync(key, false, false);
            if (item is null)
            {
                _logger.LogWarning("Attempted to delete non-existent order with ID {Key}.", key);
                throw new ArgumentException(string.Format($"Order with id {key} does " +
                    $"not exist in the database!"));
            }

            _logger.LogInformation("Deleting Order with ID {Key}...", key);

            _dbContext.Orders.Remove(item);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Order with ID {Key} deleted successfully.", key);
        }

        /// <summary>
        /// Returns all Orders.
        /// </summary>
        public async Task<List<Order>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            _logger.LogInformation("Reading all Orders. Navigational properties: {UseNav}, ReadOnly: {IsReadOnly}",
                useNavigationalProperties, isReadOnly);

            IQueryable<Order> orders = _dbContext.Orders;

            if (useNavigationalProperties)
            {
                _logger.LogDebug("Including navigational properties: OrderItems.");
                orders = orders
                    .Include(p => p.OrderItems);
            }

            if (isReadOnly)
            {
                _logger.LogDebug("Query set to AsNoTracking for read-only operation.");
                orders = orders.AsNoTracking();
            }

            var result = await orders.ToListAsync();

            _logger.LogInformation("Retrieved {Count} orders from the database.", result.Count);

            return result;
        }

        /// <summary>
        /// Reads a single Order by ID.
        /// </summary>
        public async Task<Order> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            _logger.LogInformation("Reading Order with ID {OrderID}...", key);
            IQueryable<Order> query = _dbContext.Orders;

            if (useNavigationalProperties)
            {
                query = query
                    .Include(p => p.OrderItems);
            }

            if (isReadOnly)
            {
                query = query.AsNoTracking();
            }

            Order Order = await query.SingleOrDefaultAsync(p => p.OrderID == key);
            if (Order == null)
            {
                _logger.LogWarning("Order with ID {OrderID} not found.", key);
                throw new ArgumentException(nameof(Order));
            }
            _logger.LogInformation("Order with ID {OrderID} retrieved successfully.", key);
            return Order;
        }

        /// <summary>
        /// Updates an existing Order.
        /// </summary>

        public async Task UpdateAsync(Order item)
        {
            if (item == null)
            {
                _logger.LogWarning("UpdateAsync called with a null Order.");
                throw new ArgumentNullException(nameof(item));
            }

            _logger.LogInformation("Attempting to update Order with ID {OrderID}.", item.OrderID);

            bool exists = await _dbContext.Orders.AnyAsync(p => p.OrderID == item.OrderID);
            if (!exists)
            {
                _logger.LogWarning("Update failed: Order with ID {OrderID} does not exist.", item.OrderID);
                throw new ArgumentException($"Order with ID {item.OrderID} does not exist.");
            }

            _dbContext.Orders.Update(item);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Order with ID {OrderID} updated successfully.", item.OrderID);
        }


        #endregion
    }
}
