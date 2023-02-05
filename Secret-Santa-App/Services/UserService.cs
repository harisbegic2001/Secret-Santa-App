using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Secret_Santa_App.Data;
using Secret_Santa_App.DTOs.Reponses;
using Secret_Santa_App.DTOs.Requests;
using Secret_Santa_App.Exceptions;
using Secret_Santa_App.Models;
using Secret_Santa_App.Services.Interfaces;

namespace Secret_Santa_App.Services;

public class UserService : IUserService
{
    private readonly ITokenService _tokenService;
    private readonly DataContext _context;


    public UserService(ITokenService tokenService, DataContext context)
    {
        _tokenService = tokenService;
        _context = context;
    }
    
    public async Task<User> UserRegisterAsync(CreateUserDto createUserDto)
    {

        if (string.IsNullOrWhiteSpace(createUserDto.Email) || string.IsNullOrWhiteSpace(createUserDto.FirstName) || string.IsNullOrWhiteSpace(createUserDto.LastName))
        {
            throw new InvalidInputDataException();
        }
        
        using var hmac = new HMACSHA512();
        var newUser = new User
        {

            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            Email = createUserDto.Email,
            UserRole = Role.User,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(createUserDto.FirstName!.ToLower()+createUserDto.LastName!.ToLower())),
            PasswordSalt = hmac.Key,
            GiftRecepientId = 0,
            RecepientFullName = null
        };

        await _context.Users!.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return newUser;
    }

    public async Task<ReadUserDto> UserLoginAsync(LoginUserDto loginUserDto)
    {
        var existingUser = await _context.Users!.Where(x => x.Email == loginUserDto.Email).FirstOrDefaultAsync();

        if (existingUser is null)
        {
            throw new InvalidCredentialsException("Invalid Username or Password");
        }

        using var hmac = new HMACSHA512(existingUser.PasswordSalt!);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginUserDto.Password!));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != existingUser.PasswordHash![i])
            {
                throw new InvalidCredentialsException("Invalid Username or Password");
            }
        }

        return new ReadUserDto
        {
            Id = existingUser.Id,
            Email = existingUser.Email,
            Token = _tokenService.CreateToken(existingUser)
        };
    }

    public async Task<IEnumerable<ReadUserPairsDto>> GetAllUserPairsAsync()
    {
        var listOfPairs = await _context.Users!.AsNoTracking().Select(x => new ReadUserPairsDto
        {
            FullName = $"{x.FirstName} {x.LastName}",
            RecepientFullName = x.RecepientFullName == null ? "Doesn't have recepient" : x.RecepientFullName
        }).ToListAsync();

        return listOfPairs;
    }

    public async Task<ReadUserPairsDto> GetUserRecepientAsync(int id, string callerId)
    {
        var existingUser = await _context.Users!.Where(x => x.Id == id).FirstOrDefaultAsync();

        if (existingUser is null)
        {
            throw new UserNotFoundException("User does not exist");
        }

        if (existingUser.Id != Int32.Parse(callerId))
        {
            throw new UnauthorizedAccessException("Not Allowed!");
        }

        return new ReadUserPairsDto
        {
            FullName = $"{existingUser.FirstName} {existingUser.LastName}",
            RecepientFullName = existingUser.RecepientFullName
        };

    }

    public async Task GenerateUserPairsAsync()
    {
        var random = new Random();
        var userIds = await _context.Users!.AsNoTracking().Select(x => x.Id).ToListAsync();
        var listOfUsers = await _context.Users!.ToListAsync();

        if (userIds.Count < 4)
        {
            throw new InsufficientNumberOfUsersException("Number of users must be at least 4!");
        }
        
        
        // Ovo možda uraditi u donjoj petlji!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! - Na početku petlje mislim
      /* foreach (var user in listOfUsers)
        {
            user.GiftRecepientId = 0;
            user.RecepientFullName = null;

        }*/
        
        //Ovo prebaciti sve u donji if i izbrisati numberOfUsers varijablu i u if uslov staviti userIds.Count
        var randomIndexToDelete = random.Next(userIds.Count);
        var usertoDelete = await _context.Users!.Where(x => x.Id == userIds[randomIndexToDelete]).FirstOrDefaultAsync();
        var numberOfUsers = userIds.Count;
        //Ovaj blok  
        
        if (numberOfUsers % 2 != 0)
        {
            
            userIds.Remove(userIds[randomIndexToDelete]);
            listOfUsers.Remove(usertoDelete!);

        }

        foreach (var user in listOfUsers)
        {
            user.GiftRecepientId = 0;
            user.RecepientFullName = null;
            
            var randomIndex = random.Next(userIds.Count);
            var randomId = userIds[randomIndex];
            var getUserWithRandomIndex = await _context.Users.Where(x => x.Id == randomId).FirstOrDefaultAsync();

            
            while (randomId == user.Id || getUserWithRandomIndex?.GiftRecepientId == user.Id )
            {
                randomIndex = random.Next(userIds.Count);
                randomId = userIds[randomIndex];
                getUserWithRandomIndex = await _context.Users.Where(x => x.Id == randomId).FirstOrDefaultAsync();
            }
            user.GiftRecepientId = randomId;
            user.RecepientFullName = $"{getUserWithRandomIndex?.FirstName} {getUserWithRandomIndex?.LastName}";

            userIds.Remove(randomId);

        }

        await _context.SaveChangesAsync();
    }
}