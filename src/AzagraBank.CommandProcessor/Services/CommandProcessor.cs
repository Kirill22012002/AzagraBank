using AzagraBank.CommandProcessor.Exceptions;
using AzagraBank.Messages;
using AzagraBank.Messages.Commands;
using AzagraBank.Messages.Events;

namespace AzagraBank.CommandProcessor.Services;

public interface ICommandProcessor
{
    Task<Event> ProcessCommandAsync(Command command);
}


public class CommandProcessor : ICommandProcessor
{
    private readonly ITransactionValidator _depositValidator;
    private readonly ITransactionValidator _withdrawValidator;

    public CommandProcessor(
        DepositValidator depositValidator, 
        WithdrawValidator withdrawValidator)
    {
        _depositValidator = depositValidator;
        _withdrawValidator = withdrawValidator;
    }

    public async Task<Event> ProcessCommandAsync(Command command)
    {
        if(command.Type == CommandType.Deposit)
        {
            return await ProcessDepositAsync((DepositCommand)command);
        }
        else if(command.Type == CommandType.Withdraw)
        {
            return await ProcessWithdrawAsync((WithdrawCommand)command);
        }
        else
        {
            throw new ArgumentException("not correct command type");
        }
    }

    private async Task<Event> ProcessDepositAsync(DepositCommand command) 
    {
        var isValid = await _depositValidator.ValidateAsync(command.Amount, command.AccountId);
        if (!isValid) throw new TransactionValidationException("not valid");

        return new AccountDebitedEvent
        {
            Amount = command.Amount
        };
    }

    private async Task<Event> ProcessWithdrawAsync(WithdrawCommand command)
    {
        var isValid = await _withdrawValidator.ValidateAsync(command.Amount, command.AccountId);
        if (!isValid) throw new TransactionValidationException("not valid");

        return new AccountCreditedEvent
        {
            Amount = command.Amount
        };
    }
}
