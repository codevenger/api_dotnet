using Microsoft.EntityFrameworkCore;

namespace backend.Model.Context
{
    public class SQLContext : DbContext
    {
        public SQLContext()
        {
        
        }
        public SQLContext(DbContextOptions<SQLContext> options) : base(options) {}

    }
}
