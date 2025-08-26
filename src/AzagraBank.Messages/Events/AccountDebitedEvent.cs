namespace AzagraBank.Messages.Events;

public class AccountDebitedEvent : Event
{
    public override EventType Type => EventType.AccountDebited;
    public int Amount { get; set; }
}
