using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Secret_Santa_App.Models;

namespace Secret_Santa_App.Data;

/// <summary>
/// Database context class, that will use Microsoft SQL Server Database.
/// </summary>
public class DataContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataContext"/> class.
    /// </summary>
    /// <param name="options">Database context options.</param>
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    /// <summary>
    /// Method that seeds initial user (admin) in database. This will be called only once, after the migration is created an database updated.
    /// </summary>
    /// <param name="modelBuilder">Database context modelBuilder parameter</param>
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

    /// <summary>
    /// Gets or sets Users.
    /// </summary>
    /// <value>
    /// The Users table.
    /// </value>
    public DbSet<User>? Users { get; set; }
}