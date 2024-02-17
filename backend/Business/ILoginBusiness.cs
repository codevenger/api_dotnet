using backend.Data.VO;

namespace backend.Business
{
    public interface ILoginBusiness
    {
        bool ValidateCredentials(UserVO user);
    }
}
