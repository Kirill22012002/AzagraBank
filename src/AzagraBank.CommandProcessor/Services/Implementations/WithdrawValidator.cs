using AzagraBank.CommandProcessor.Services.Interfaces;
using AzagraBank.EF.Repositories;

namespace AzagraBank.CommandProcessor.Services.Implementations;

public class WithdrawValidator : IWithdrawValidator
{
    private readonly IAccountRepository _accountRepository;

    public async Task<bool> ValidateAsync(int mount, string accountId)
    {
        var account = await _accountRepository.GetAccountByIdAsNoTrackingAsync(accountId);

        if (account.State == EF.Models.Enums.AccountState.Active) return true;
        else return false;
    }
}
