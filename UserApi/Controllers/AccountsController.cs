using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserApi.Data;
using UserApi.Infrastructure;
using UserApi.Models;
using UserApi.Models.Login;
using UserApi.Models.Users;
using UserApi.Services;

namespace UserApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly UserDbContext _context;
    private readonly IJwtProvider _jwtProvider;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<string> _passwordHasher;
    private readonly IUserService _userService;
    public AccountsController(UserDbContext context, IJwtProvider jwtProvider, IConfiguration configuration, IUserService userService)
    {
        _context = context;
        _jwtProvider = jwtProvider;
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<string>();
        _userService = userService;
    }
    
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await _userService.RegisterAsync(request);
        return Ok(response);
    }


    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = new LoginResponse();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Pin == request.Pin);

        if (user == null) return BadRequest(response);

        var result = _passwordHasher.VerifyHashedPassword(user.Pin, user.Password, request.Password);
        if (result != PasswordVerificationResult.Success) Unauthorized(response);

        var token = _jwtProvider.GenerateToken(request.Pin);
        var refreshToken = _jwtProvider.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddHours(12);
        _context.Update(user);
        await _context.SaveChangesAsync();

        response = new LoginResponse
        {
            IsLogedIn = true,
            AccessToken = token,
            RefreshToken = refreshToken,
        };
        return Ok(response);
    }
    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var principal = GetTokenPrincipal(request.AccessToken);
        var response = new LoginResponse();
        if (principal?.Identity?.Name is null)
        {
            return BadRequest(response);
        }
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Pin == principal.Identity.Name);
        if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
        {
            return BadRequest(response);
        }
        response.IsLogedIn = true;
        response.AccessToken = _jwtProvider.GenerateToken(user.Pin);
        response.RefreshToken = _jwtProvider.GenerateRefreshToken();

        user.RefreshToken = response.RefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddHours(12);

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return Ok(response);

    }
    private ClaimsPrincipal GetTokenPrincipal(string token)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("JwtOptions:SecretKey").Value));

        var validation = new TokenValidationParameters
        {
            IssuerSigningKey = securityKey,
            ValidateLifetime = false,
            ValidateActor = false,
            ValidateIssuer = false,
            ValidateAudience = false,
        };
        return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
    }

}
