using Indpro.API.Data.Models;
using Indpro.API.DTO.Interface;
using Indpro.API.Repository.Interface;

namespace Indpro.API.DTO.Service;

public class UserDto : IUserDto
{
    private readonly IUserService _user;
 
    public UserDto(IUserService userService)
    {
        this._user = userService;
    }

    public Task<OperationResult<UserModel>> Login(LoginModel model)
    {
        return _user.Login(model);
    }

    public Task<OperationResult> RegisterUser(UserModel model)
    {
        return _user.RegisterUser(model);
    }
}
