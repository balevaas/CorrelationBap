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
    //public class CorrealtionRep : ICorrelation
    //{
    //    private readonly DataContext Context;
    //    public CorrealtionRep(DataContext dataContext) => Context = dataContext;

    //    public IQueryable<Correlation> Items => Context.Correlations;

    //    public async Task<int> DeleteAsync(Correlation item)
    //    {
    //        if (Items.Contains(item))
    //        {
    //            Context.Remove(item);
    //            return await Context.SaveChangesAsync();
    //        }
    //        return 0;
    //    }

    //    public async Task<Correlation> GetItemByIdAsync(int id)
    //    {
    //        return await Items.FirstOrDefaultAsync(x => x.CorrelationID == id);
    //    }
    //    public Station GetItemById(int id, DateTime date) { return null; }
    //    public async Task<int> UpdateAsync(Correlation item)
    //    {
    //        var rec = await Items.FirstOrDefaultAsync(x => x.CorrelationID == item.CorrelationID);
    //        if (rec != default) Context.Update(item);
    //        else await Context.AddAsync(item);
    //        return await Context.SaveChangesAsync();
    //    }
    //}
}
