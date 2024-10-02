using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseData.Entities
{
    [PrimaryKey(nameof(ID))]
    public class Station
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Height { get; set; }
        public override string ToString() => Name;
        //public ICollection<WeatherData> WDateCollect { get; set; }
        //public ICollection<Point> PointCollect { get; set; }
    }
}
