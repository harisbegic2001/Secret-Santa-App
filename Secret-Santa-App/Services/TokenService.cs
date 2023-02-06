using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Secret_Santa_App.EnvironmentSettings;
using Secret_Santa_App.Models;
using Secret_Santa_App.Services.Interfaces;

namespace Secret_Santa_App.Services;

/// <summary>
/// The token service.
/// </summary>
public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="TokenService"/> class.
    /// </summary>
    /// <param name="options">The environment variable that is initialized in appsetings.json</param>
    public TokenService(IOptions<SecretConfiguration> options)
    {
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey!));
    }
    
    /// <inheritdoc />
    public string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, user.UserRole.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
            new Claim(JwtRegisteredClaimNames.NameId, user.Email!),



        };

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
        }
}