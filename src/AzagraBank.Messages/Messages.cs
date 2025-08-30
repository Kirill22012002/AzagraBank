using System.Runtime.Serialization;

namespace AzagraBank.Messages;

public interface IMessage { }

public abstract class Command : IMessage
{ 
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public virtual CommandType Type { get; set; }
}

public abstract class Event : IMessage
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
