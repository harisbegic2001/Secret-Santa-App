using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Secret_Santa_App.Data;
using Secret_Santa_App.DTOs.Requests;
using Secret_Santa_App.Services;
using Secret_Santa_App.Services.Interfaces;
using Secret_Santa_App.Test.Data;
using Secret_Santa_App.Test.Helpers;
using Xunit;

namespace Secret_Santa_App.Test.Services;

public class UserServiceTest : IDisposable
{
    private readonly DataContext _dataContextMock;
    private readonly UserService _systemUnderTest;
    
    public UserServiceTest()
    {
        _dataContextMock = InMemoryDatabaseFactory.CreateInMemoryDatabase();
        
        var tokenServiceMock = new Mock<ITokenService>();

        _systemUnderTest = new UserService(tokenServiceMock.Object, _dataContextMock);

    }


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
    
    
    
    
    
    public void Dispose()
    {
        _dataContextMock.Database.EnsureDeleted();
        _dataContextMock.Dispose();
    }
}