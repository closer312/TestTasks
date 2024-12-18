using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.Models.Users;
using UserApi.Services;

namespace UserApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpGet("GetUserById")]
    public async Task<IActionResult> GetByIdAsync([FromQuery] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("Empty Id");

        var user = await _userService.GetByIdAsync(id, true);

        if (user == null)
            return BadRequest("Not found");

        return Ok(user);
    }
    [HttpPut("EditUser")]
    public async Task<IActionResult> EditAsync([FromBody] UserDto userDto)
    {
        if (userDto == null)
            return BadRequest("Empty !");

        var userId = await _userService.UpdateAsync(userDto);
        if (userId == Guid.Empty)
            return BadRequest("Error");

        return Ok($"User with id = {userId} updated");
    }

    [HttpDelete("DeleteUser")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("Empty Id");

        await _userService.DeleteAsync(id);

        return Ok("User deleted");
    }
}

