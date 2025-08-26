namespace AzagraBank.Messages.Commands;

public class DepositCommand : Command
{
    public override CommandType Type => CommandType.Withdraw;
    public int Amount { get; set; }
    public string AccountId { get; set; }
}
