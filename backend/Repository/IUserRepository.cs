using backend.Data.VO;
using backend.Model;

namespace backend.Repository
{
    public interface IUserRepository
    {
        User ValidateCredentials(UserVO user);
    }
}
