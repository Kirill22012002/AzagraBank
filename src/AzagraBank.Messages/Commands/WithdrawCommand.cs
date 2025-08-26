namespace AzagraBank.Messages.Commands;

public class WithdrawCommand : Command
{
    public override CommandType Type => CommandType.Withdraw;
    public int Amount { get; set; }
    public string AccountId { get; set; }
}
