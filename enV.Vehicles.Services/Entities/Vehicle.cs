using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace enV.Vehicles.Services.Entities
{
    public class Vehicle
    {
        public int DealerId { get; internal set; }
        public string VIN { get; internal set; }
        public DateTimeOffset ModifiedDate { get; internal set; }
        public string Make { get; internal set; }
        public string Model { get; internal set; }
        public string Year { get; internal set; }
        public string Color { get; internal set; }
        public int Mileage { get; internal set; }
    }
}
