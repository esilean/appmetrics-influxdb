using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppMetricsInfluxDB.Domain.Interfaces
{
    public interface IPromeRepository
    {
        Task<IEnumerable<Prome>> GetAll();
        Task Add(Prome prome);
        void Remove(Guid id);

    }
}
