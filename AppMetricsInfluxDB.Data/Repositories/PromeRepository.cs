using AppMetricsInfluxDB.Data.Data;
using AppMetricsInfluxDB.Domain;
using AppMetricsInfluxDB.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMetricsInfluxDB.Data.Repositories
{
    public class PromeRepository : IPromeRepository
    {
        private readonly PromeDbContext _dbContext;

        public PromeRepository(PromeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Prome prome)
        {
            await _dbContext.Promes.AddAsync(prome);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Prome>> GetAll()
        {
            return await _dbContext.Promes.ToListAsync();
        }

        public void Remove(Guid id)
        {
            var prome = _dbContext.Promes.FirstOrDefault(x => x.Id == id);
            if (prome != null)
            {
                _dbContext.Promes.Remove(prome);
                _dbContext.SaveChanges();
            }
        }
    }
}
