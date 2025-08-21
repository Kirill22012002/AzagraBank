namespace AzagraBank.Messages.Commands;

public class DepositCommand : ICommand
{
    public override CommandType Type => CommandType.Withdraw;
    public int Amount { get; set; }
}
