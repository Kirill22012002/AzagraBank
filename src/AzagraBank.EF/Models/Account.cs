using AzagraBank.EF.Models.Enums;

namespace AzagraBank.EF.Models;

public class Account : BaseModel
{
    public long Count { get; set; }
    public AccountState State { get; set; }
}
