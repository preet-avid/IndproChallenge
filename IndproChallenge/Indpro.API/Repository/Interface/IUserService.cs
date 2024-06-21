using Indpro.API.Data.Models;

namespace Indpro.API.Repository.Interface;
public interface IUserService
{
    public Task<OperationResult> RegisterUser(UserModel model);
    public Task<OperationResult<UserModel>> Login(LoginModel model);
}
