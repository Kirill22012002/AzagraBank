namespace AzagraBank.Messages.Events;

public class AccountCreditedEvent : IEvent
{
    public override EventType Type => EventType.AccountCredited;
    public int Amount { get; set; }
}
