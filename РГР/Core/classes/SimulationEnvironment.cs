using Core.structures;
using Core.enums;

namespace Core.classes;

public class SimulationEnvironment
{
  private const int GridSize = 30;
  private readonly EntityType[,] grid;
  private readonly List<Agent> agents;
  private readonly List<Point> plants;
  private readonly Random random = new();

  private int totalEatenPlants = 0;
  private int totalEatenHerbivores = 0;

  public IReadOnlyList<Agent> Agents => agents.AsReadOnly();
  public int PlantsCount => plants.Count;
  public int TotalEatenPlants => totalEatenPlants;
  public int TotalEatenHerbivores => totalEatenHerbivores;
  public int Size => GridSize;

  public SimulationEnvironment()
  {
    grid = new EntityType[GridSize, GridSize];
    agents = [];
    plants = [];
    for (int i = 0; i < GridSize; i++)
      for (int j = 0; j < GridSize; j++)
        grid[i, j] = EntityType.Empty;
  }

  public void Initialize(int plantCount, int herbivoreCount, int carnivoreCount)
  {
    for (int i = 0; i < plantCount; i++) AddRandomPlant();
    for (int i = 0; i < herbivoreCount; i++) AddRandomAgent(AgentType.Herbivore);
    for (int i = 0; i < carnivoreCount; i++) AddRandomAgent(AgentType.Carnivore);
  }

  private void AddRandomPlant()
  {
    if (plants.Count >= GridSize * GridSize) return;

    Point pos;
    int attempts = 0;
    do
    {
      pos = new Point(random.Next(GridSize), random.Next(GridSize));
      attempts++;
      if (attempts > 1000) return;
    }
    while (grid[pos.X, pos.Y] != EntityType.Empty);

    grid[pos.X, pos.Y] = EntityType.Plant;
    plants.Add(pos);
  }

  private void AddRandomAgent(AgentType type)
  {
    Point pos;
    do { pos = new Point(random.Next(GridSize), random.Next(GridSize)); }
    while (grid[pos.X, pos.Y] != EntityType.Empty);
    Agent agent = new(type) { Location = pos };
    grid[pos.X, pos.Y] = (type == AgentType.Herbivore) ? EntityType.Herbivore : EntityType.Carnivore;
    agents.Add(agent);
  }

  public EntityType GetEntityAt(Point pos)
  {
    if (!IsValidPosition(pos)) return EntityType.Empty;
    return grid[pos.X, pos.Y];
  }
  public static bool IsValidPosition(Point pos) => pos.X >= 0 && pos.X < GridSize && pos.Y >= 0 && pos.Y < GridSize;

  public void RemovePlant(Point pos)
  {
    if (grid[pos.X, pos.Y] == EntityType.Plant)
    {
      grid[pos.X, pos.Y] = EntityType.Empty;
      plants.Remove(pos);
      totalEatenPlants++;
    }
  }

  public void RemoveHerbivore(Point pos)
  {
    if (grid[pos.X, pos.Y] == EntityType.Herbivore)
    {
      grid[pos.X, pos.Y] = EntityType.Empty;
      var herb = agents.FirstOrDefault(a => a.Location.X == pos.X && a.Location.Y == pos.Y && a.Type == AgentType.Herbivore);
      if (herb != null)
      {
        herb.IsAlive = false;
        totalEatenHerbivores++;
      }
    }
  }

  public void MoveAgent(Agent agent, Point newPos)
  {
    grid[agent.Location.X, agent.Location.Y] = EntityType.Empty;
    agent.Location = newPos;
    grid[newPos.X, newPos.Y] = (agent.Type == AgentType.Herbivore) ? EntityType.Herbivore : EntityType.Carnivore;
  }

  public void SimulateStep()
  {
    foreach (var type in new[] { AgentType.Herbivore, AgentType.Carnivore })
    {
      var activeAgents = agents.Where(a => a.IsAlive && a.Type == type).ToList();
      foreach (var agent in activeAgents)
      {
        if (!agent.IsAlive) continue;
        double[] sensors = agent.GetSensors(this);
        int action = agent.Brain.Activate(sensors);
        agent.PerformAction(action, this);
        agent.UpdateMetabolism();
        Agent? child = agent.Reproduce();
        if (child != null)
        {
          Point emptyPos = FindEmptyPosition();
          if (emptyPos.X != -1)
          {
            child.Location = emptyPos;
            agents.Add(child);
            grid[emptyPos.X, emptyPos.Y] = (child.Type == AgentType.Herbivore) ? EntityType.Herbivore : EntityType.Carnivore;
          }
        }
      }
    }

    for (int i = agents.Count - 1; i >= 0; i--)
    {
      if (!agents[i].IsAlive)
      {
        grid[agents[i].Location.X, agents[i].Location.Y] = EntityType.Empty;
        agents.RemoveAt(i);
      }
    }

    int maxFreeCells = GridSize * GridSize - plants.Count - agents.Count;
    int newPlantCount = Math.Min(Math.Max(1, plants.Count / 10), maxFreeCells);
    if (newPlantCount > 0)
    {
      for (int i = 0; i < newPlantCount; i++)
        AddRandomPlant();
    }
  }

  private Point FindEmptyPosition()
  {
    if (agents.Count + plants.Count >= GridSize * GridSize)
      return new Point(-1, -1);

    for (int attempt = 0; attempt < 200; attempt++)
    {
      Point pos = new(random.Next(GridSize), random.Next(GridSize));
      if (grid[pos.X, pos.Y] == EntityType.Empty)
        return pos;
    }
    return new Point(-1, -1);
  }
}