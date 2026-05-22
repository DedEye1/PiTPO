using Microsoft.AspNetCore.Mvc;
using API.Services;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SimulationController : ControllerBase
{
  private readonly SimulationService _simulation;

  public SimulationController(SimulationService simulation)
  {
    _simulation = simulation;
  }

  [HttpGet("state")]
  public IActionResult GetState() => Ok(_simulation.GetCurrentState());

  [HttpPost("step")]
  public IActionResult Step()
  {
    _simulation.Step();
    return Ok();
  }

  [HttpPost("start")]
  public IActionResult Start([FromQuery] int intervalMs = 100)
  {
    _simulation.StartAuto(intervalMs);
    return Ok();
  }

  [HttpPost("stop")]
  public IActionResult Stop()
  {
    _simulation.StopAuto();
    return Ok();
  }

  [HttpPost("reset")]
  public IActionResult Reset()
  {
    _simulation.Reset();
    return Ok();
  }
}