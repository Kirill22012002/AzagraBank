using AzagraBank.EventBus;
using AzagraBank.EventBus.Interfaces;
using AzagraBank.Messages.Commands;
using Microsoft.AspNetCore.Mvc;

namespace AzagraBank.ApiService.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly IMessagePublisher<DepositCommand> _depositCommandPublisher;
    private readonly IMessagePublisher<WithdrawCommand> _withdrawCommandPublisher;

    public AccountController(
        IMessagePublisher<DepositCommand> depositCommandPublisher,
        IMessagePublisher<WithdrawCommand> withdrawCommandPublisher)
    {
        _depositCommandPublisher = depositCommandPublisher;
        _withdrawCommandPublisher = withdrawCommandPublisher;
    }

    [HttpGet]
    public IActionResult Deposit([FromQuery] int amount)
    {
        if (amount <= 0) return BadRequest("not correct data");

        var depositCommand = new DepositCommand
        {
            Amount = amount
        };

        _depositCommandPublisher.PublishAsync(depositCommand, CONSTS.KAFKA_COMMANDS_TOPIC);

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

        _withdrawCommandPublisher.PublishAsync(withdrawCommand, CONSTS.KAFKA_COMMANDS_TOPIC);

        return Ok();
    }
}
