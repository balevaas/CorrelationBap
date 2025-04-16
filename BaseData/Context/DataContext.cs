using BaseData.Entities;
using Microsoft.EntityFrameworkCore;

namespace BaseData.Context
{
    public sealed class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Correlation> Correlations { get; set; } = null!;
        public DbSet<Pollution> Pollutions { get; set; } = null!;
        public DbSet<Point> Points { get; set; } = null!;
        public DbSet<Station> Stations { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Station>().HasData(
                new Station { ID = 30715, Name = "Ангарск", Location = "Иркутская область, Россия", Latitude = 52.4831m, Longitude = 103.8497m, Height = 436 },
                new Station { ID = 30818, Name = "Байкальск", Location = "Иркутская область, Россия", Latitude = 51.5157m, Longitude = 104.1749m, Height = 478 },
                new Station { ID = 30309, Name = "Братск", Location = "Иркутская область, Россия", Latitude = 56.2831m, Longitude = 101.7500m, Height = 411 },
                new Station { ID = 30308, Name = "Вихоревка", Location = "Иркутская область, Россия", Latitude = 56.1700m, Longitude = 101.1000m, Height = 369},
                new Station { ID = 30603, Name = "Зима", Location = "Иркутская область, Россия", Latitude = 53.9331m, Longitude = 102.0497m, Height = 457 },
                new Station { ID = 30812, Name = "Култук", Location = "Иркутская область, Россия", Latitude = 51.7196m, Longitude = 103.7093m, Height = 465 },
                new Station { ID = 30514, Name = "Листвянка", Location = "Иркутская область, Россия", Latitude = 51.7333m, Longitude = 103.7000m, Height = 472 },
                new Station { ID = 30509, Name = "Саянск", Location = "Иркутская область, Россия", Latitude = 54.0500m, Longitude = 102.1000m, Height = 550 },
                new Station { ID = 30504, Name = "Тулун", Location = "Иркутская область, Россия", Latitude = 54.6000m, Longitude = 100.6331m, Height = 523 },
                new Station { ID = 30712, Name = "Усолье-Сибирское", Location = "Иркутская область, Россия", Latitude = 52.7800m, Longitude = 103.6000m, Height = 437 },
                new Station { ID = 30210, Name = "Усть-Илимск", Location = "Иркутская область, Россия", Latitude = 57.98000m, Longitude = 102.6000m, Height = 298 },
                new Station { ID = 30617, Name = "Черемхово", Location = "Иркутская область, Россия", Latitude = 53.1667m, Longitude = 103.0833m, Height = 598 }
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

                new Point { ID = 1, StationID = 30308 },

                //new Point { ID = 1, StationID = 30603 },
                //new Point { ID = 2, StationID = 30603 },

                new Point { ID = 5, StationID = 30812 },

                //new Point { ID = 1, StationID = 30814 },

                new Point { ID = 3, StationID = 30509 },

                //new Point { ID = 1, StationID = 30504 },

                new Point { ID = 4, StationID = 30712 },
                //new Point { ID = 5, StationID = 30712 },

                //new Point { ID = 2, StationID = 30210 },
                new Point { ID = 3, StationID = 30210 },

                new Point { ID = 6, StationID = 30617 },
                new Point { ID = 7, StationID = 30617 });

            string[] station = ["Ангарск", "Байкальск", "Братск", "Вихоревка", "Зима", "Култук", "Саянск", "Тулун", "Усолье-Сибирское", "Усть-Илимск", "Черемхово"];
            List<Pollution> pollutions = [];
            for (int i = 0; i < station.Length; i++)
            {
                var poll = ExcelDataReader.ReadData("D:\\Concentration.xlsx", station[i]);
                pollutions.AddRange(poll);
            }
            modelBuilder.Entity<Pollution>().HasData(pollutions);
        }
    }
}
