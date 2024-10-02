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
    //public class PointRep : IPoint
    //{
    //    private readonly DataContext Context;
    //    public PointRep(DataContext dataContext) => Context = dataContext;

    //    public IQueryable<Point> Items => Context.Points;

    //    public async Task<int> DeleteAsync(Point item)
    //    {
    //        if (Items.Contains(item))
    //        {
    //            Context.Remove(item);
    //            return await Context.SaveChangesAsync();
    //        }
    //        return 0;
    //    }

    //    public async Task<Point> GetItemByIdAsync(int id)
    //    {
    //        return await Items.FirstOrDefaultAsync(x => x.PointID == id);
    //    }

    //    public async Task<int> UpdateAsync(Point item)
    //    {
    //        var rec = await Items.FirstOrDefaultAsync(x => x.PointID == item.PointID);
    //        if (rec != default) Context.Update(item);
    //        else await Context.AddAsync(item);
    //        return await Context.SaveChangesAsync();
    //    }
    //}
}
