using BaseData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;

namespace BaseData.Context
{
    public sealed class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<Correlation> Correlations { get; set; } = null!;
        public DbSet<Pollution> Pollutions { get; set; } = null!;
        public DbSet<Point> Points { get; set; } = null!;
        public DbSet<Station> Stations { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Station>().HasData(
                new Station { ID = 30715, Name = "Ангарск", Location = "Иркутская область, Россия", Latitude = 52.48m, Longitude = 103.8m, Height = 437 },                
                new Station { ID = 30818, Name = "Байкальск", Location = "Иркутская область, Россия", Latitude = 51.65m, Longitude = 104.1m, Height = 478 },
                new Station { ID = 30603, Name = "Зима", Location = "Иркутская область, Россия", Latitude = 53.93m, Longitude = 102m, Height = 458 },                
                new Station { ID = 30309, Name = "Братск", Location = "Иркутская область, Россия", Latitude = 56.28m, Longitude = 101.7m, Height = 416 },
                new Station { ID = 30509, Name = "Саянск", Location = "Иркутская область, Россия", Latitude = 54.05m, Longitude = 102.1m, Height = 550 }
                );

            modelBuilder.Entity<Point>().HasData(
                new Point { ID = 25, StationID = 30715 },
                new Point { ID = 26, StationID = 30715 },
                new Point { ID = 27, StationID = 30715 },
                new Point { ID = 41, StationID = 30715 },

                new Point { ID = 48, StationID = 30818 },

                new Point { ID = 2, StationID = 30309 },
                new Point { ID = 8, StationID = 30309 },
                new Point { ID = 11, StationID = 30309 },

                new Point { ID = 1, StationID = 30603 },
                new Point { ID = 2, StationID = 30603 },

                new Point { ID = 3, StationID = 30509 });
            string[] station = { "Ангарск", "Байкальск", "Зима", "Братск", "Саянск" };
            List<Pollution> pollutions = new List<Pollution>();
            for (int i = 0; i < station.Length; i++)
            {
                var poll = ExcelDataReader.ReadData("D:\\Concentration.xlsx", station[i]);
                pollutions.AddRange(poll);                
            }
            modelBuilder.Entity<Pollution>().HasData(pollutions);
        }
    }
}
