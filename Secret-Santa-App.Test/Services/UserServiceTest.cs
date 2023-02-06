using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Secret_Santa_App.Data;
using Secret_Santa_App.DTOs.Requests;
using Secret_Santa_App.Exceptions;
using Secret_Santa_App.Services;
using Secret_Santa_App.Services.Interfaces;
using Secret_Santa_App.Test.Data;
using Secret_Santa_App.Test.Helpers;
using Xunit;

namespace Secret_Santa_App.Test.Services;

/// <summary>
/// User service test class.
/// </summary>
public class UserServiceTest : IDisposable
{
    private readonly DataContext _dataContextMock;
    private readonly UserService _systemUnderTest;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UserServiceTest"/> class.
    /// </summary>
    public UserServiceTest()
    {
        _dataContextMock = InMemoryDatabaseFactory.CreateInMemoryDatabase();
        
        var tokenServiceMock = new Mock<ITokenService>();

        _systemUnderTest = new UserService(tokenServiceMock.Object, _dataContextMock);

    }

    /// <summary>
    /// Test for creating a new user.
    /// </summary>
    [Fact]
    public async Task AddUserAsync_WhenCalled_AddsNewUser()
    {
        //Arrange
        var listOfUsers = UserData.Users();
        _dataContextMock.Users!.AddRange(listOfUsers);
        var newUser = UserData.AddUser();
        await _dataContextMock.SaveChangesAsync();
        
        //Act
        await _systemUnderTest.UserRegisterAsync(newUser);
        
        //Assert
        var numberOfUsersAfterAdding = listOfUsers.Count + 1;
        Assert.Equal(numberOfUsersAfterAdding, _dataContextMock.Users.Count());
    }
    
    /// <summary>
    /// Test for login a user.
    /// </summary>
    [Fact]
    public async Task LoginUser_WhenCalled_LogsInUser()
    {
        //Arrange
        var listOfUsers = UserData.Users();
        _dataContextMock.Users!.AddRange(listOfUsers);
        await _dataContextMock.SaveChangesAsync();
        var loginUserCredential = new LoginUserDto
        {
            Email = listOfUsers[0].Email,
            Password = "firstnamelastname"
        };
        
        //Act
        var loggedIn =await _systemUnderTest.UserLoginAsync(loginUserCredential);
        
        //Assert
        Assert.Equal(loginUserCredential.Email,loggedIn.Email);
        Assert.Equal(1, loggedIn.Id);
    }


    /// <summary>
    /// Test for getting all user pairs.
    /// </summary>
    [Fact]
    public async Task GetAllUserPairsAsync_WhenCalled_ReturnsListOfPairs()
    {
        //Arrange
        var listOfUsers = UserData.Users();
        _dataContextMock.Users!.AddRange(listOfUsers);
        await _dataContextMock.SaveChangesAsync();
        
        //Act
        var listOfPairs = await _systemUnderTest.GetAllUserPairsAsync();
        
        //Assert
        Assert.Equal(listOfUsers.Count,listOfPairs.Count());
    }
    
    /// <summary>
    /// Test for creating single user pair.
    /// </summary>
    [Fact]
    public async Task GetUserRecepientAsync_WhenCalled_ReturnsUserPair()
    {
        //Arrange
        var listOfUsers = UserData.Users();
        _dataContextMock.Users!.AddRange(listOfUsers);
        await _dataContextMock.SaveChangesAsync();
        var userToCheck = listOfUsers[0];
        //Act
        var userPair = await _systemUnderTest.GetUserRecepientAsync(listOfUsers[0].Id, listOfUsers[0].Id.ToString());
        
        //Assert
        Assert.Equal(userToCheck.RecepientFullName, userPair.RecepientFullName);
        Assert.Equal($"{userToCheck.FirstName} {userToCheck.LastName}" ,userPair.FullName);
    }
    
    
    /// <summary>
    /// Test for InvalidInputDataException when input data is invalid.
    /// </summary>
    [Fact]
    public async Task AddUserAsyncWithInvalidData_WhenCalled_ThrowsInvalidInputDataException()
    {
        //Arrange
        var newUser = UserData.AddUserWithInvalidData();
        await _dataContextMock.SaveChangesAsync();
        
        //Assert
        await Assert.ThrowsAsync<InvalidInputDataException>(() => _systemUnderTest.UserRegisterAsync(newUser));
    }
    
    
    /// <summary>
    /// Test for InvalidCredentialsException when email is incorrect.
    /// </summary>
    [Fact]
    public async Task LoginUserWithInvalidEmail_WhenCalled_ThrowsInvalidCredentialsException()
    {
        //Arrange
        var listOfUsers = UserData.Users();
        _dataContextMock.Users!.AddRange(listOfUsers);
        await _dataContextMock.SaveChangesAsync();
        var loginUserCredential = new LoginUserDto
        {
            Email = "InvalidEmail",
            Password = "firstnamelastname"
        };
        
        //Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(() => _systemUnderTest.UserLoginAsync(loginUserCredential));

    }
    
    /// <summary>
    /// Test for InvalidCredentialsException when password is incorrect.
    /// </summary>
    [Fact]
    public async Task LoginUserWithInvalidPassword_WhenCalled_ThrowsInvalidCredentialsException()
    {
        //Arrange
        var listOfUsers = UserData.Users();
        _dataContextMock.Users!.AddRange(listOfUsers);
        await _dataContextMock.SaveChangesAsync();
        var loginUserCredential = new LoginUserDto
        {
            Email = listOfUsers[0].Email,
            Password = "InvalidPassword"
        };
        
        //Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(() => _systemUnderTest.UserLoginAsync(loginUserCredential));

    }
    
    
    /// <summary>
    /// Test for UserNotFoundException when the specified user cannot be found.
    /// </summary>
    [Fact]
    public async Task GetUserRecepientAsyncWithNonExistingId_WhenCalled_ThrowsUserNotFoundException()
    {
        //Arrange
        var listOfUsers = UserData.Users();
        _dataContextMock.Users!.AddRange(listOfUsers);
        await _dataContextMock.SaveChangesAsync();

        //Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _systemUnderTest.GetUserRecepientAsync(100, listOfUsers[0].Id.ToString()));

    }
    
    
    /// <summary>
    /// Test for UnauthorizedAccessException when the caller of the method is unauthorized.
    /// </summary>
    [Fact]
    public async Task GetUserRecepientAsyncWithUnauthorizedCallerId_WhenCalled_ThrowsUnauthorizedAccessException()
    {
        //Arrange
        var listOfUsers = UserData.Users();
        _dataContextMock.Users!.AddRange(listOfUsers);
        await _dataContextMock.SaveChangesAsync();

        //Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _systemUnderTest.GetUserRecepientAsync(listOfUsers[0].Id, listOfUsers[1].Id.ToString()));

    }
    

    /// <summary>
    /// Test for InsufficientNumberOfUsersException when the number of users is less than 3.
    /// </summary>
    [Fact]
    public async Task GenerateUserPairsAsyncWithTwoUsersInDatabase_WhenCalled_ThrownInsufficientNumberOfUsersException()
    {   
        //Arrange
        var listOfUsers = UserData.Users();
        listOfUsers.RemoveAt(listOfUsers.Count-1);
        listOfUsers.RemoveAt(listOfUsers.Count-1);
        await _dataContextMock.Users!.AddRangeAsync(listOfUsers);
    
        //Assert
        await Assert.ThrowsAsync<InsufficientNumberOfUsersException>(() => _systemUnderTest.GenerateUserPairsAsync());


    }
    
    
    
    
    public void Dispose()
    {
        _dataContextMock.Database.EnsureDeleted();
        _dataContextMock.Dispose();
    }
}