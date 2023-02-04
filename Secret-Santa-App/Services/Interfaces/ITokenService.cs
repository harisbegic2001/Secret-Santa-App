using Secret_Santa_App.Models;

namespace Secret_Santa_App.Services.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}