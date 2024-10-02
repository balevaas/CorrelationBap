using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseData.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        IQueryable<T> Items { get; }
        Task<T> GetItemByIdAsync(int id);        
        Task<int> DeleteAsync(T item);
        Task<int> UpdateAsync(T item);
    }
}
