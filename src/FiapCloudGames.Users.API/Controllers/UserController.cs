using FiapCloudGames.Users.Application.UserFeature.Commands.AuthUser;
using FiapCloudGames.Users.Application.UserFeature.Commands.CreateUser;
using FiapCloudGames.Users.Application.UserFeature.Commands.MakeAdmin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.Users.API.Controllers;

[Route("api/v1/[controller]")]
public class UserController
    (
        IMediator mediator
    )
    : ControllerBase   
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserCommand command)
    {
        var result = await mediator.Send(command);

        if (result)
            return Created();

        return BadRequest();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserCommand command)
    {
        var result = await mediator.Send(command);

        if (result.IdToken != null)
            return Ok(result);

        return Unauthorized();
    }

    [HttpPut("MakeAdmin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> MakeAdminAsync([FromBody] MakeAdminCommand command)
    {
        var result = await mediator.Send(command);

        if (result)
            return Created();

        return BadRequest();
    }
}