using App.Metrics;
using App.Metrics.Counter;

namespace AppMetricsInfluxDB.Api
{
    public class MetricsRegister
    {
        public static CounterOptions CreatedPromesCounter => new CounterOptions
        {
            Name = "Created Promes",
            Context = "AppMetricsInfluxDBApi",
            MeasurementUnit = Unit.Calls
        };

        public static CounterOptions CalledAllPromesCounter => new CounterOptions
        {
            Name = "Called All Promes",
            Context = "AppMetricsInfluxDBApi",
            MeasurementUnit = Unit.Calls
        };

    }
}
