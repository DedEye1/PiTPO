using Core.classes;
using Core.enums;
using Core.structures;
using API.DTO;

namespace API.Services;

public class SimulationService
{
  private SimulationEnvironment _env;
  private int _currentStep;
  private Timer? _autoTimer;
  private bool _isRunning;

  public SimulationService()
  {
    _env = new SimulationEnvironment();
    _env.Initialize(plantCount: 30, herbivoreCount: 20, carnivoreCount: 10);
  }

  public SimulationStateDto GetCurrentState()
  {
    var cells = new List<CellDto>();
    for (var x = 0; x < _env.Size; x++)
      for (var y = 0; y < _env.Size; y++)
      {
        var entity = _env.GetEntityAt(new Point(x, y));
        cells.Add(new CellDto { X = x, Y = y, EntityType = entity.ToString() });
      }

    var agents = _env.Agents;
    var herbivores = agents.Where(a => a is { IsAlive: true, Type: AgentType.Herbivore }).ToList();
    var carnivores = agents.Where(a => a is { IsAlive: true, Type: AgentType.Carnivore }).ToList();

    var stats = new StatisticsDto
    {
      Plants = _env.PlantsCount,
      Herbivores = herbivores.Count,
      Carnivores = carnivores.Count,
      TotalEatenPlants = _env.TotalEatenPlants,
      TotalEatenHerbivores = _env.TotalEatenHerbivores,
      AvgHerbivoreAge = herbivores.Count != 0 ? herbivores.Average(a => a.Age) : 0,
      AvgCarnivoreAge = carnivores.Count != 0 ? carnivores.Average(a => a.Age) : 0
    };

    return new SimulationStateDto
    {
      Step = _currentStep,
      GridSize = _env.Size,
      Grid = cells,
      Statistics = stats
    };
  }

  public void Step()
  {
    if (!_env.Agents.Any(a => a.IsAlive)) return;
    _env.SimulateStep();
    _currentStep++;
  }

  public void StartAuto(int intervalMs)
  {
    if (_isRunning) return;
    _autoTimer = new Timer(_ => Step(), null, 0, intervalMs);
    _isRunning = true;
  }

  public void StopAuto()
  {
    _autoTimer?.Dispose();
    _autoTimer = null;
    _isRunning = false;
  }

  public void Reset()
  {
    StopAuto();
    _env = new SimulationEnvironment();
    _env.Initialize(plantCount: 30, herbivoreCount: 20, carnivoreCount: 10);
    _currentStep = 0;
  }
}