using backend.Data.VO;
using backend.Model;
using backend.Model.Context;
using Microsoft.AspNetCore.Identity;

namespace backend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SQLContext _context;

        public UserRepository(SQLContext context)
        {
            _context = context;
        }

        public User ValidateCredentials(UserVO user)
        {
            if (user.Username == null || user.Password == null) return null;

            IdentityUser identityUser = new IdentityUser() { UserName = user.Username };
            PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();

            var result = _context.Users.FirstOrDefault(u => (u.Username == user.Username));
            if (result == null) return null;

            PasswordVerificationResult verifyPassword = hasher.VerifyHashedPassword(identityUser, result.Password, user.Password);
            if(verifyPassword == PasswordVerificationResult.Success)
            {
                return result;
            }
            return null;
        }
    }
}