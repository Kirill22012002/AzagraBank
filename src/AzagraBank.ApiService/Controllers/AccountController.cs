using AzagraBank.Messages.Commands;
using Microsoft.AspNetCore.Mvc;

namespace AzagraBank.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    [HttpGet(Name = "deposit")]
    public IActionResult Deposit([FromQuery] int amount)
    {
        if (amount <= 0) return BadRequest("not correct data");

        var depositCommand = new DepositCommand
        {
            Amount = amount
        };

        // send to message broker

        return Ok();
    }

    [HttpGet(Name = "withdraw")]
    public IActionResult Withdraw([FromQuery] int amount)
    {
        if (amount <= 0) return BadRequest("not correct data");

        var withdrawCommand = new WithdrawCommand
        {
            Amount = amount
        };

        // send to message broker

        return Ok();
    }
}
