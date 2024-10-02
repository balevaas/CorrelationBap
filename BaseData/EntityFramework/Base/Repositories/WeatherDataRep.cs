using BaseData.Entities;
using BaseData.EntityFramework.Context;
using BaseData.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseData.EntityFramework.Base.Repositories
{
    //public class WeatherDataRep : IWeatherData
    //{
    //    private readonly DataContext Context;
    //    public WeatherDataRep(DataContext dataContext) => Context = dataContext;

    //    public IQueryable<WeatherData> Items => Context.WeatherDatas;

    //    public async Task<int> DeleteAsync(WeatherData item)
    //    {
    //        if (Items.Contains(item))
    //        {
    //            Context.Remove(item);
    //            return await Context.SaveChangesAsync();
    //        }
    //        return 0;
    //    }
    //    public async Task<WeatherData> GetItemByIdAsync(int id)
    //    {
    //        return await Items.FirstOrDefaultAsync(x => (int)x.StationID == id);
    //    }

    //    public async Task<WeatherData> GetItemByIdAsync(int id, DateTime date)
    //    {
    //        return await Items.FirstOrDefaultAsync(x => (int)x.StationID == id && x.Date == date);
    //    }

    //    public async Task<int> UpdateAsync(WeatherData item)
    //    {
    //        var rec = await Items.FirstOrDefaultAsync(x => x.StationID == item.StationID && x.Date == item.Date);
    //        if (rec != default) Context.Update(item);
    //        else await Context.AddAsync(item);
    //        return await Context.SaveChangesAsync();
    //    }
    //}
}
