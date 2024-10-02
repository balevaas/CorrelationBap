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
    //internal class PollutionRep : IPollution
    //{
    //    private readonly DataContext Context;
    //    public PollutionRep(DataContext dataContext) => Context = dataContext;

    //    public IQueryable<Pollution> Items => Context.Pollutions;

    //    public async Task<int> DeleteAsync(Pollution item)
    //    {
    //        if (Items.Contains(item))
    //        {
    //            Context.Remove(item);
    //            return await Context.SaveChangesAsync();
    //        }
    //        return 0;
    //    }

    //    public async Task<Pollution> GetItemByIdAsync(int id)
    //    {
    //        return await Items.FirstOrDefaultAsync(x => (int)x.PointID == id);
    //    }

    //    public async Task<Pollution> GetItemByIdAsync(int id, DateTime date)
    //    {
    //        return await Items.FirstOrDefaultAsync(x => (int)x.PointID == id && x.Date == date);
    //    }

    //    public async Task<int> UpdateAsync(Pollution item)
    //    {
    //        var rec = await Items.FirstOrDefaultAsync(x => x.PointID == item.PointID);
    //        if (rec != default) Context.Update(item);
    //        else await Context.AddAsync(item);
    //        return await Context.SaveChangesAsync();
    //    }
    //}
}
