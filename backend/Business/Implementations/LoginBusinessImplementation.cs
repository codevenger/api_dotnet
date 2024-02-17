using backend.Data.VO;
using backend.Repository;
using backend.Model;
using backend.Model.Context;

namespace backend.Business.Implementations
{
    public class LoginBusinessImplementation : ILoginBusiness
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private IUserRepository _repository;

        public LoginBusinessImplementation(IUserRepository repository)
        {
            _repository = repository;
        }

        public bool ValidateCredentials(UserVO userCredentials)
        {
            if (userCredentials == null) return false;
            var user = _repository.ValidateCredentials(userCredentials);
            if (user == null) return false;
            return true;
        }
    }
}
