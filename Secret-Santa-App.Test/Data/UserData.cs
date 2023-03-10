using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Secret_Santa_App.DTOs.Requests;
using Secret_Santa_App.Models;

namespace Secret_Santa_App.Test.Data;

/// <summary>
/// The User data.
/// </summary>
public static class UserData
{
    /// <summary>
    /// Creates the list of Users.
    /// </summary>
    /// <returns>The list of users.</returns>
    public static List<User> Users()
    {
        using var hmac = new HMACSHA512();

        return new List<User>()
        {
            new User
            {
                Id = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                UserRole = Role.Admin,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("firstnamelastname")),
                PasswordSalt = hmac.Key,
                GiftRecepientId = 0,
                RecepientFullName = "RecepientFullName"
            },

            new User
            {
                Id = 2,
                FirstName = "FirstName2",
                LastName = "LastName2",
                Email = "Email2",
                UserRole = Role.User,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("firstname2lastname2")),
                PasswordSalt = hmac.Key,
                GiftRecepientId = 0,
                RecepientFullName = "RecepientFullName2"
            },

            new User
            {
                Id = 3,
                FirstName = "FirstName3",
                LastName = "LastName3",
                Email = "Email3",
                UserRole = Role.User,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("firstname3lastname3")),
                PasswordSalt = hmac.Key,
                GiftRecepientId = 0,
                RecepientFullName = "RecepientFullName3"
            },

            new User
            {
                Id = 4,
                FirstName = "FirstName4",
                LastName = "LastName4",
                Email = "Email4",
                UserRole = Role.User,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("firstname4lastname4")),
                PasswordSalt = hmac.Key,
                GiftRecepientId = 0,
                RecepientFullName = "RecepientFullName4"
            },
        };
    }


    /// <summary>
    /// Creates the request body for creating a user;
    /// </summary>
    /// <returns>The create user request body.</returns>
    public static CreateUserDto AddUser()
    {
        return new CreateUserDto
        {
            FirstName = "FirstName3",
            LastName = "LastName3",
            Email = "Email3"
        };
    }

    /// <summary>
    /// Creates the request body for creating a user;
    /// </summary>
    /// <returns>The create user request body that is invalid.</returns>
    public static CreateUserDto AddUserWithInvalidData()
    {
        return new CreateUserDto
        {
            FirstName = " ",
            LastName = " ",
            Email = " "
        };
    }
}