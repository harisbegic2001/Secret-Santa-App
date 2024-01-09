namespace Secret_Santa_App.Services;

using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Data;
using DTOs.Reponses;
using DTOs.Requests;
using Exceptions;
using Models;
using Interfaces;
public class UserService : IUserService
{
    private readonly ITokenService _tokenService;
    private readonly DataContext _context;


    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="tokenService"></param>
    /// <param name="context"></param>
    public UserService(ITokenService tokenService, DataContext context)
    {
        _tokenService = tokenService;
        _context = context;
    }

    /// <inheritdoc />
    public async Task<User> UserRegisterAsync(CreateUserDto createUserDto)
    {
        if (string.IsNullOrWhiteSpace(createUserDto.Email) || string.IsNullOrWhiteSpace(createUserDto.FirstName) || string.IsNullOrWhiteSpace(createUserDto.LastName))
        {
            throw new InvalidInputDataException();
        }

        if (await _context.Users!.AnyAsync(x => x.Email == createUserDto.Email))
        {
            throw new UserAlreadyExistsException("User with that email already exists!");
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
            RecepientFullName = null,
        };
        await _context.Users!.AddAsync(newUser);
        await _context.SaveChangesAsync();
        return newUser;
    }

    /// <inheritdoc />
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
            Token = _tokenService.CreateToken(existingUser),
        };
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ReadUserPairsDto>> GetAllUserPairsAsync()
    {
        var listOfPairs = await _context.Users!.AsNoTracking().Select(x => new ReadUserPairsDto
        {
            FullName = $"{x.FirstName} {x.LastName}",
            RecepientFullName = x.RecepientFullName == null ? "Doesn't have recepient" : x.RecepientFullName
            /*RecepientFullName = x.RecepientFullName == $"{x.FirstName} {x.LastName}" || x.RecepientFullName == null ? "Doesn't have recepient" : x.RecepientFullName*/
        }).ToListAsync();

        return listOfPairs;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task GenerateUserPairsAsync()
    {
        var random = new Random();
        var userIds = await _context.Users!.AsNoTracking().Select(x => x.Id).ToListAsync();
        var listOfUsers = await _context.Users!.ToListAsync();


        if (userIds.Count < 3)
        {
            throw new InsufficientNumberOfUsersException("Number of users must be at least 3!");
        }

        foreach (var user in listOfUsers)
        {
            user.GiftRecepientId = 0;
            user.RecepientFullName = null;
        }

        foreach (var user in listOfUsers)
        {
            var randomIndex = random.Next(userIds.Count);
            var randomId = userIds[randomIndex];
            var getUserWithRandomIndex =  _context.Users.Where(x => x.Id == randomId).FirstOrDefault();


            while (randomId == user.Id || getUserWithRandomIndex?.GiftRecepientId == user.Id )
            {
                randomIndex = random.Next(userIds.Count);
                randomId = userIds[randomIndex];
                getUserWithRandomIndex =  _context.Users.Where(x => x.Id == randomId).FirstOrDefault();

                if (userIds.Count == 1 && (user.Id == userIds[0] || getUserWithRandomIndex!.GiftRecepientId == user.Id))
                {
                    user.GiftRecepientId = 0;
                    user.RecepientFullName = null;
                    break;
                }
            }


            /* if (user.Id == listOfUsers[^1].Id && getUserWithRandomIndex.GiftRecepientId == user.Id)
             {
                 continue;
             }

             if (userIds[0] != user.Id || getUserWithRandomIndex.GiftRecepientId != user.Id)
             {
                 user.GiftRecepientId = randomId;
                 user.RecepientFullName = $"{getUserWithRandomIndex?.FirstName} {getUserWithRandomIndex?.LastName}";
             }*/

            if ((user.Id == listOfUsers[^1].Id && getUserWithRandomIndex!.GiftRecepientId == user.Id) || (user.Id == listOfUsers[^1].Id && user.Id == userIds[0]))
            {
                continue;
            }

            user.GiftRecepientId = randomId;
            user.RecepientFullName = $"{getUserWithRandomIndex?.FirstName} {getUserWithRandomIndex?.LastName}";

            userIds.Remove(randomId);
        }

        await _context.SaveChangesAsync();
    }
}