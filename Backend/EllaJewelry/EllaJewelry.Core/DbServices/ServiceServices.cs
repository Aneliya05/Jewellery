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
    public class ServiceServices : ICRUD<Service, int>
    {
        private readonly EllaJewelryDbContext _dbContext;

        public ServiceServices(EllaJewelryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateAsync(Service item)
        {
            _dbContext.Services.Add(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int key)
        {
            Service service = await ReadAsync(key);
            if(service is null)
            {
                throw new ArgumentException(string.Format($"Service with id {key} does " +
                   $"not exist in the database!"));
            }
            _dbContext.Services.Remove(service);
            _dbContext.SaveChanges();
        }

        public async Task<List<Service>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            IQueryable<Service> services = _dbContext.Services;
            if (isReadOnly)
            {
                services = services.AsNoTracking();
            }
            return await services.ToListAsync();
        }

        public async Task<Service> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            IQueryable<Service> services = _dbContext.Services;

            if (isReadOnly)
            {
                services = services.AsNoTracking();
            }
            return await services.SingleOrDefaultAsync(x => x.Id == key);
        }

        public async Task UpdateAsync(Service item)
        {
            _dbContext.Services.Update(item);
            await _dbContext.SaveChangesAsync();
        }
    }
}
