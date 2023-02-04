using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Secret_Santa_App.Models;

namespace Secret_Santa_App.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) 
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        using var hmac = new HMACSHA512();
        modelBuilder.Entity<User>().HasData(

            new User
            {
                Id = 1,
                FirstName = "Haris",
                LastName = "Begic",
                Email = "harisbegic01@hotmail.com",
                UserRole = Role.Admin,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("harisbegic")),
                PasswordSalt = hmac.Key,
                GiftRecepientId = null,
                RecepientFullName = null
            }
        );
    }
    
    public DbSet<User>? Users { get; set; }
}