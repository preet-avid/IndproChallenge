using Indpro.API.Data.Models;
using Indpro.API.DTO.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Indpro.API.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserDto _user;

    public UserController(IUserDto user) => _user = user;

    [HttpPost]
    [Route("Register")]
    public async Task<OperationResult> Register([FromBody] UserModel model) => await _user.RegisterUser(model);

    [HttpPost]
    [Route("Login")]
    public async Task<OperationResult<UserModel>> LoginUser([FromBody] LoginModel model) => await _user.Login(model);
}
