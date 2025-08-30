namespace AzagraBank.CommandProcessor.Services.Interfaces;

public interface ITransactionValidator
{
    Task<bool> ValidateAsync(int mount, string accountId);
}

public interface IDepositValidator : ITransactionValidator { }

public interface IWithdrawValidator : ITransactionValidator { }
