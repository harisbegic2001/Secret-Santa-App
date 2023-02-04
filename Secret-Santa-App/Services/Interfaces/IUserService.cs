using Secret_Santa_App.DTOs.Reponses;
using Secret_Santa_App.DTOs.Requests;
using Secret_Santa_App.Models;

namespace Secret_Santa_App.Services.Interfaces;

public interface IUserService
{
    Task<User> UserRegisterAsync(CreateUserDto createUserDto);

    Task<ReadUserDto> UserLoginAsync(LoginUserDto loginUserDto);

    Task<IEnumerable<ReadUserPairsDto>> GetAllUserPairsAsync();

    Task<ReadUserPairsDto> GetUserRecepientAsync(int id, string callerId);

    Task GenerateUserPairsAsync();


}