using BaseData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseData.Repositories
{
    public interface ICorrelation : IRepositoryBase<Correlation> { }
    public interface IPoint : IRepositoryBase<Point> { }
    public interface IPollution : IRepositoryBase<Pollution> 
    {
        Task<Pollution> GetItemByIdAsync(int id, DateTime date);
    }
    public interface IStation : IRepositoryBase<Station> { }
    public interface IWeatherData : IRepositoryBase<WeatherData> 
    {
        Task<WeatherData> GetItemByIdAsync(int id, DateTime date);
    }
}
