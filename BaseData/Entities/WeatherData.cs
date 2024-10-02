using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseData.Entities
{
    [PrimaryKey(nameof(StationID), nameof(Date))]
    public class WeatherData
    {
        [ForeignKey(nameof(StationID))]
        public int StationID { get; set; }
        public DateTime Date { get; set; }
        public decimal? WindDirection { get; set; }
        public int WindSpeed { get; set; }
        public decimal Temperature { get; set; }
        public int Humidity { get; set; }
        public decimal Pressure { get; set; }
        public int? SnowHeight { get; set; }

        public Station Station { get; set; }
    }
}
