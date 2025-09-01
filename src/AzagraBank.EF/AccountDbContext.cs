using AzagraBank.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace AzagraBank.EF;

public class AccountDbContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }

    public AccountDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }
}
