using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace env.Vehicles.Infrastructure.Models
{
    public class Vehicle
    {
        public int DealerId { get; set; }
        public string VIN { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Color { get; set; }
        public int Mileage { get; set; }
    }
}
