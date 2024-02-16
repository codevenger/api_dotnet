using backend.Data.VO;
using backend.Model;
using backend.Model.Context;

namespace backend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SQLContext _context;

        public UserRepository(SQLContext context)
        {
            _context = context;
        }
    }
}