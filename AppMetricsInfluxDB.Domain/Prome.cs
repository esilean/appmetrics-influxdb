using System;

namespace AppMetricsInfluxDB.Domain
{
    public class Prome
    {
        public Guid Id { get; private set; }
        public string Description { get; private set; }

        private Prome(string description)
        {
            Id = Guid.NewGuid();
            Description = description;
        }

        public static Prome Factory(string description)
        {
            return new Prome(description);
        }
    }
}
