namespace AzagraBank.Messages.Events;

public class AccountDebited : IEvent
{
    public override EventType Type => EventType.AccountDebited;
    public int Amount { get; set; }
}
