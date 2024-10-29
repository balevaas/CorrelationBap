using BaseData.Entities;
using Microsoft.EntityFrameworkCore;

namespace BaseData.Context
{
    public sealed class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<Correlation> Correlations { get; set; } = null!;
        public DbSet<Pollution> Pollutions { get; set; } = null!;
        public DbSet<Point> Points { get; set; } = null!;
        public DbSet<Station> Stations { get; set; } = null!;
        public DbSet<WeatherData> WeatherDatas { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Station>().HasData(
                new Station { ID = 30716, Name = "Хомутово", Location = "Иркутская область, Россия", Latitude = 52.46m, Longitude = 104.3m, Height = 454 },
                new Station { ID = 30711, Name = "Шелехов", Location = "Иркутская область, Россия", Latitude = 52.2m, Longitude = 104m, Height = 458 },
                new Station { ID = 30710, Name = "Иркутск", Location = "Иркутская область, Россия", Latitude = 52.27m, Longitude = 104.3m, Height = 469 },
                new Station { ID = 30715, Name = "Ангарск", Location = "Иркутская область, Россия", Latitude = 52.48m, Longitude = 103.8m, Height = 437 },
                new Station { ID = 30617, Name = "Черемхово", Location = "Иркутская область, Россия", Latitude = 53.18m, Longitude = 103m, Height = 598 },
                new Station { ID = 30818, Name = "Байкальск", Location = "Иркутская область, Россия", Latitude = 51.65m, Longitude = 104.1m, Height = 478 },
                new Station { ID = 30603, Name = "Зима", Location = "Иркутская область, Россия", Latitude = 53.93m, Longitude = 102m, Height = 458 },
                new Station { ID = 30758, Name = "Чита", Location = "Забайкальский край, Россия", Latitude = 52.08m, Longitude = 113.4m, Height = 671 },
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

            modelBuilder.Entity<Pollution>().HasData(
                new Pollution { Date = new DateTime(2021, 01, 01), PointID = 25, Concentration = 2.3m },
                new Pollution { Date = new DateTime(2021, 02, 01), PointID = 25, Concentration = 4.1m },
                new Pollution { Date = new DateTime(2021, 03, 01), PointID = 25, Concentration = 3.4m },
                new Pollution { Date = new DateTime(2021, 04, 01), PointID = 25, Concentration = 2.3m },
                new Pollution { Date = new DateTime(2021, 05, 01), PointID = 25, Concentration = 0.57m },
                new Pollution { Date = new DateTime(2021, 06, 01), PointID = 25, Concentration = 0.9m },
                new Pollution { Date = new DateTime(2021, 07, 01), PointID = 25, Concentration = 1.2m },
                new Pollution { Date = new DateTime(2021, 08, 01), PointID = 25, Concentration = 0.85m },
                new Pollution { Date = new DateTime(2021, 09, 01), PointID = 25, Concentration = 1.1m },
                new Pollution { Date = new DateTime(2021, 10, 01), PointID = 25, Concentration = 2.8m },
                new Pollution { Date = new DateTime(2021, 11, 01), PointID = 25, Concentration = 3.0m },
                new Pollution { Date = new DateTime(2021, 12, 01), PointID = 25, Concentration = 7.9m },
                new Pollution { Date = new DateTime(2022, 01, 01), PointID = 25, Concentration = 1.6m },
                new Pollution { Date = new DateTime(2022, 02, 01), PointID = 25, Concentration = 2.3m },
                new Pollution { Date = new DateTime(2022, 03, 01), PointID = 25, Concentration = 2.2m },
                new Pollution { Date = new DateTime(2022, 04, 01), PointID = 25, Concentration = 0.82m },
                new Pollution { Date = new DateTime(2022, 05, 01), PointID = 25, Concentration = 0.44m },
                new Pollution { Date = new DateTime(2022, 06, 01), PointID = 25, Concentration = 0.42m },
                new Pollution { Date = new DateTime(2022, 07, 01), PointID = 25, Concentration = 0.65m },
                new Pollution { Date = new DateTime(2022, 08, 01), PointID = 25, Concentration = 0.96m },
                new Pollution { Date = new DateTime(2022, 09, 01), PointID = 25, Concentration = 2.4m },
                new Pollution { Date = new DateTime(2022, 10, 01), PointID = 25, Concentration = 1.9m },
                new Pollution { Date = new DateTime(2022, 11, 01), PointID = 25, Concentration = 3.1m },
                new Pollution { Date = new DateTime(2022, 12, 01), PointID = 25, Concentration = 6.2m }
                );
        }
    }
}
