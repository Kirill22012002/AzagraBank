using System.Runtime.Serialization;

namespace AzagraBank.Messages;

public abstract class Command
{ 
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public virtual CommandType Type { get; set; }
}

public abstract class Event
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public virtual EventType Type { get; set; }
}

public enum CommandType
{
    [EnumMember(Value = "deposit")] Deposit,
    [EnumMember(Value = "withdraw")] Withdraw
}

public enum EventType
{
    [EnumMember(Value = "accountCredited")] AccountCredited,
    [EnumMember(Value = "accountDebited")] AccountDebited
}
