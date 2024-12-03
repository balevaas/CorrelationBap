using Microsoft.EntityFrameworkCore;

namespace BaseData.Context
{
    public class DbContextFactory(DbContextOptions options)
    {
        private readonly DbContextOptions _options = options;

        public DataContext Create()
        {
            var res = new DataContext(_options);
            res.Database.EnsureCreated();
            return res;
        }
    }
}
