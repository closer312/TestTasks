using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace UserApi.Infrastructure;

public interface IJwtProvider {
    string GenerateRefreshToken();
    string GenerateToken(string pin);
}
public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider {
    private readonly JwtOptions _options = options.Value;

    public string GenerateRefreshToken() {

        var number  = new byte[64];

        using (var numberGenerator = RandomNumberGenerator.Create()) { 
            numberGenerator.GetBytes(number);
        }

        return Convert.ToBase64String(number);
    }

    public string GenerateToken(string pin) {
        
        var claims = new List<Claim>() {
            new Claim(ClaimTypes.Name,pin)
        };

        var signinCredaentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256
            );

        var token = new JwtSecurityToken(
            claims:claims,
            signingCredentials:signinCredaentials,
            expires:DateTime.UtcNow.AddMinutes(_options.ExpiresHours)
            );

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenValue;
    }
}
