using AzagraBank.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace AzagraBank.EF.Repositories;

public interface IAccountRepository
{
    Task<Account> GetAccountByIdAsNoTrackingAsync(string id);
}

public class AccountRepository : IAccountRepository
{
    private readonly AccountDbContext _context;

    public AccountRepository(AccountDbContext context)
    {
        _context = context;
    }

    public async Task<Account> GetAccountByIdAsNoTrackingAsync(string id)
    {
        var account = await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return account;
    }
}
