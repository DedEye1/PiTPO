using Core.structures;
using Core.enums;

namespace Core.classes;

public class SimulationEnvironment
{
  private const int GridSize = 30;
  private readonly EntityType[,] _grid;
  private readonly List<Agent> _agents;
  private readonly List<Point> _plants;
  private readonly Random _random = new();

  private int _totalEatenPlants, _totalEatenHerbivores;

  public IReadOnlyList<Agent> Agents => _agents.AsReadOnly();
  public int PlantsCount => _plants.Count;
  public int TotalEatenPlants => _totalEatenPlants;
  public int TotalEatenHerbivores => _totalEatenHerbivores;
  public int Size => GridSize;

  public SimulationEnvironment()
  {
    _grid = new EntityType[GridSize, GridSize];
    _agents = [];
    _plants = [];
    for (var i = 0; i < GridSize; i++)
      for (var j = 0; j < GridSize; j++)
        _grid[i, j] = EntityType.Empty;
  }

  public void Initialize(int plantCount, int herbivoreCount, int carnivoreCount)
  {
    for (var i = 0; i < plantCount; i++) AddRandomPlant();
    for (var i = 0; i < herbivoreCount; i++) AddRandomAgent(AgentType.Herbivore);
    for (var i = 0; i < carnivoreCount; i++) AddRandomAgent(AgentType.Carnivore);
  }

  private void AddRandomPlant()
  {
    if (_plants.Count >= GridSize * GridSize) return;

    Point pos;
    var attempts = 0;
    do
    {
      pos = new Point(_random.Next(GridSize), _random.Next(GridSize));
      attempts++;
      if (attempts > 1000) return;
    }
    while (_grid[pos.X, pos.Y] != EntityType.Empty);

    _grid[pos.X, pos.Y] = EntityType.Plant;
    _plants.Add(pos);
  }

  private void AddRandomAgent(AgentType type)
  {
    Point pos;
    do { pos = new Point(_random.Next(GridSize), _random.Next(GridSize)); }
    while (_grid[pos.X, pos.Y] != EntityType.Empty);
    Agent agent = new(type) { Location = pos };
    _grid[pos.X, pos.Y] = (type == AgentType.Herbivore) ? EntityType.Herbivore : EntityType.Carnivore;
    _agents.Add(agent);
  }

  public EntityType GetEntityAt(Point pos)
  {
    return !IsValidPosition(pos) ? EntityType.Empty : _grid[pos.X, pos.Y];
  }
  public static bool IsValidPosition(Point pos) => pos.X is >= 0 and < GridSize && pos.Y is >= 0 and < GridSize;

  public void RemovePlant(Point pos)
  {
    if (_grid[pos.X, pos.Y] != EntityType.Plant) return;
    _grid[pos.X, pos.Y] = EntityType.Empty;
    _plants.Remove(pos);
    _totalEatenPlants++;
  }

  public void RemoveHerbivore(Point pos)
  {
    if (_grid[pos.X, pos.Y] != EntityType.Herbivore) return;
    _grid[pos.X, pos.Y] = EntityType.Empty;
    var herb = _agents.FirstOrDefault(a => a.Location.X == pos.X && a.Location.Y == pos.Y && a.Type == AgentType.Herbivore);
    if (herb == null) return;
    herb.IsAlive = false;
    _totalEatenHerbivores++;
  }

  public void MoveAgent(Agent agent, Point newPos)
  {
    _grid[agent.Location.X, agent.Location.Y] = EntityType.Empty;
    agent.Location = newPos;
    _grid[newPos.X, newPos.Y] = (agent.Type == AgentType.Herbivore) ? EntityType.Herbivore : EntityType.Carnivore;
  }

  public void SimulateStep()
  {
    foreach (var type in new[] { AgentType.Herbivore, AgentType.Carnivore })
    {
      var activeAgents = _agents.Where(a => a.IsAlive && a.Type == type).ToList();
      foreach (var agent in activeAgents)
      {
        if (!agent.IsAlive) continue;
        var sensors = agent.GetSensors(this);
        var action = agent.Brain.Activate(sensors);
        agent.PerformAction(action, this);
        agent.UpdateMetabolism();
        var child = agent.Reproduce();
        if (child == null) continue;
        var emptyPos = FindEmptyPosition();
        if (emptyPos.X == -1) continue;
        child.Location = emptyPos;
        _agents.Add(child);
        _grid[emptyPos.X, emptyPos.Y] = (child.Type == AgentType.Herbivore) ? EntityType.Herbivore : EntityType.Carnivore;
      }
    }

    for (var i = _agents.Count - 1; i >= 0; i--)
    {
      if (_agents[i].IsAlive) continue;
      _grid[_agents[i].Location.X, _agents[i].Location.Y] = EntityType.Empty;
      _agents.RemoveAt(i);
    }

    var maxFreeCells = GridSize * GridSize - _plants.Count - _agents.Count;
    var newPlantCount = Math.Min(Math.Max(1, _plants.Count / 10), maxFreeCells);
    if (newPlantCount <= 0) return;
    {
      for (var i = 0; i < newPlantCount; i++)
        AddRandomPlant();
    }
  }

  private Point FindEmptyPosition()
  {
    if (_agents.Count + _plants.Count >= GridSize * GridSize)
      return new Point(-1, -1);

    for (var attempt = 0; attempt < 200; attempt++)
    {
      Point pos = new(_random.Next(GridSize), _random.Next(GridSize));
      if (_grid[pos.X, pos.Y] == EntityType.Empty)
        return pos;
    }
    return new Point(-1, -1);
  }
}