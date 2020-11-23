using App.Metrics;
using AppMetricsInfluxDB.Api.Dtos;
using AppMetricsInfluxDB.Domain;
using AppMetricsInfluxDB.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppMetricsInfluxDB.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PromeController : ControllerBase
    {

        private readonly IPromeRepository _promeRepository;
        private readonly ILogger<PromeController> _logger;
        private readonly IMetrics _metrics;

        public PromeController(IPromeRepository promeRepository, IMetrics metrics, ILogger<PromeController> logger)
        {
            _logger = logger;
            _promeRepository = promeRepository;
            _metrics = metrics;
        }

        [HttpGet]
        public async Task<IEnumerable<Prome>> GetAll()
        {
            _logger.LogInformation("Getting prome...");

            var promes = await _promeRepository.GetAll();
            _metrics.Measure.Counter.Increment(MetricsRegister.CalledAllPromesCounter);
            return promes;
        }

        [HttpPost]
        public async Task<IActionResult> Post(PromeDto promeDto)
        {
            _logger.LogInformation("Posting prome...");

            var prome = Prome.Factory(promeDto.Description);
            await _promeRepository.Add(prome);

            _metrics.Measure.Counter.Increment(MetricsRegister.CreatedPromesCounter);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _logger.LogInformation("Deleting prome...");

            _promeRepository.Remove(id);

            return Ok();
        }
    }
}
