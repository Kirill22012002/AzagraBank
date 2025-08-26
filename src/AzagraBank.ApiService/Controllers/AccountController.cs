using AzagraBank.ApiService.Services;
using AzagraBank.Messages.Commands;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AzagraBank.ApiService.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly IProducerService _producerService;

    public AccountController(IProducerService producerService)
    {
        _producerService = producerService;
    }

    [HttpGet]
    public IActionResult Deposit([FromQuery] int amount)
    {
        if (amount <= 0) return BadRequest("not correct data");

        var depositCommand = new DepositCommand
        {
            Amount = amount
        };

        _producerService.SendMessageAsync("commands", JsonConvert.SerializeObject(depositCommand));

        return Ok();
    }

    [HttpGet]
    public IActionResult Withdraw([FromQuery] int amount)
    {
        if (amount <= 0) return BadRequest("not correct data");

        var withdrawCommand = new WithdrawCommand
        {
            Amount = amount
        };

        _producerService.SendMessageAsync("commands", JsonConvert.SerializeObject(withdrawCommand));

        return Ok();
    }
}
