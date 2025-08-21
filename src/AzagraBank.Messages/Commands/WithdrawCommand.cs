namespace AzagraBank.Messages.Commands;

public class WithdrawCommand : ICommand
{
    public override CommandType Type => CommandType.Withdraw;
    public int Amount { get; set; }
}
