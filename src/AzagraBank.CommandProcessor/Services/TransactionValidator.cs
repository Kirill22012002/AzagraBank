using AzagraBank.EF.Repositories;

namespace AzagraBank.CommandProcessor.Services;

public interface ITransactionValidator
{
    Task<bool> ValidateAsync(int mount, string accountId);
}

public class DepositValidator : ITransactionValidator
{
    private readonly IAccountRepository _accountRepository;

    public async Task<bool> ValidateAsync(int mount, string accountId)
    {
        var account = await _accountRepository.GetAccountByIdAsNoTrackingAsync(accountId);

        if (account.State == EF.Models.Enums.AccountState.Active) return true;

        if (account.Count >= mount) return true;
        else return false;
    }
}

public class WithdrawValidator : ITransactionValidator
{
    private readonly IAccountRepository _accountRepository;

    public async Task<bool> ValidateAsync(int mount, string accountId)
    {
        var account = await _accountRepository.GetAccountByIdAsNoTrackingAsync(accountId);

        if (account.State == EF.Models.Enums.AccountState.Active) return true;
        else return false;
    }
}