using BaseData.Entities;
using BaseData.EntityFramework.Context;
using BaseData.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BaseData.EntityFramework.Base.Repositories
{
    //public class StationRep : IStation
    //{
    //    private readonly DataContext Context;
    //    public StationRep(DataContext dataContext) => Context = dataContext;

    //    public IQueryable<Station> Items => Context.Stations;

    //    public async Task<int> DeleteAsync(Station item)
    //    {
    //        if (Items.Contains(item))
    //        {
    //            Context.Remove(item);
    //            return await Context.SaveChangesAsync();
    //        }
    //        return 0;
    //    }

    //    public async Task<Station> GetItemByIdAsync(int id)
    //    {
    //        return await Items.FirstOrDefaultAsync(x => x.StationID == id);
    //    }

    //    public async Task<int> UpdateAsync(Station item)
    //    {
    //        var rec = await Items.FirstOrDefaultAsync(x => x.StationID == item.StationID);
    //        if (rec != default) Context.Update(item);
    //        else await Context.AddAsync(item);
    //        return await Context.SaveChangesAsync();
    //    }
    //}
}
