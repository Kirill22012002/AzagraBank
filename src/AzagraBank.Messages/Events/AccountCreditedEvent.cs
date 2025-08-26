namespace AzagraBank.Messages.Events;

public class AccountCreditedEvent : Event
{
    public override EventType Type => EventType.AccountCredited;
    public int Amount { get; set; }
}
