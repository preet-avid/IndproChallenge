using Indpro.API.Data.Models;

namespace Indpro.API.DTO.Interface;

public interface IUserDto
{
    public Task<OperationResult> RegisterUser(UserModel model);
    public Task<OperationResult<UserModel>> Login(LoginModel model);
}
