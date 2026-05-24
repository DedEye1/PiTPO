using Core.structures;
using Core.enums;

namespace Core.classes;

public class Agent(AgentType type, NeuralNetwork? brain = null)
{
  public readonly AgentType Type = type;
  public Point Location;
  private Direction _facing = Direction.North;
  private double _energy = 100.0;
  public int Age = 0;
  private int _generation = 1;
  public bool IsAlive = true;
  public readonly NeuralNetwork Brain = brain ?? new NeuralNetwork();

  public double[] GetSensors(SimulationEnvironment env)
  {
    var sensors = new double[12];
    var idx = 0;
    for (var zone = 0; zone < 4; zone++)
    {
      var positions = GetZonePositions(zone);
      int plantCount = 0, herbCount = 0, carnCount = 0;
      foreach (var entity in positions.Select(env.GetEntityAt))
      {
        switch (entity)
        {
          case EntityType.Plant:
            plantCount++;
            break;
          case EntityType.Herbivore:
            herbCount++;
            break;
          case EntityType.Carnivore:
            carnCount++;
            break;
        }
      }
      sensors[idx++] = plantCount;
      sensors[idx++] = herbCount;
      sensors[idx++] = carnCount;
    }
    return sensors;
  }

  private List<Point> GetZonePositions(int zone)
  {
    List<Point> positions = [];
    var offsets = zone switch
    {
      0 => new int[,] { { -2, -2 }, { -2, -1 }, { -2, 0 }, { -2, 1 }, { -2, 2 } },
      1 => new int[,] { { -1, -2 }, { 0, -2 } },
      2 => new int[,] { { -1, 2 }, { 0, 2 } },
      _ => new int[,] { { 0, -1 }, { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, 1 } }
    };
    for (var i = 0; i < offsets.GetLength(0); i++)
    {
      int dx = offsets[i, 0], dy = offsets[i, 1];
      (dx, dy) = RotateOffset(dx, dy, _facing);
      positions.Add(new Point(Location.X + dx, Location.Y + dy));
    }
    return positions;
  }

  private static (int, int) RotateOffset(int dx, int dy, Direction dir)
  {
    return dir switch
    {
      Direction.North => (dx, dy),
      Direction.East => (dy, -dx),
      Direction.South => (-dx, -dy),
      Direction.West => (-dy, dx),
      _ => (dx, dy),
    };
  }

  public void PerformAction(int action, SimulationEnvironment env)
  {
    switch (action)
    {
      case 0: _facing = (Direction)(((int)_facing + 3) % 4); break;
      case 1: _facing = (Direction)(((int)_facing + 1) % 4); break;
      case 2: Move(env); break;
      case 3: Eat(env); break;
    }
  }

  private void Move(SimulationEnvironment env)
  {
    var newPos = Location;
    switch (_facing)
    {
      case Direction.North: newPos.X--; break;
      case Direction.South: newPos.X++; break;
      case Direction.East: newPos.Y++; break;
      case Direction.West: newPos.Y--; break;
    }

    if (!SimulationEnvironment.IsValidPosition(newPos) || env.GetEntityAt(newPos) != EntityType.Empty) return;
    env.MoveAgent(this, newPos);
    Location = newPos;
  }

  private void Eat(SimulationEnvironment env)
  {
    var proximityPositions = GetZonePositions(3);
    foreach (var pos in proximityPositions)
    {
      EntityType food = env.GetEntityAt(pos);
      if (Type == AgentType.Herbivore && food == EntityType.Plant)
      {
        env.RemovePlant(pos);
        _energy += 30;
        return;
      }
      else if (Type == AgentType.Carnivore && food == EntityType.Herbivore)
      {
        env.RemoveHerbivore(pos);
        _energy += 50;
        return;
      }
    }
  }

  public void UpdateMetabolism()
  {
    _energy -= (Type == AgentType.Herbivore) ? 2.0 : 1.0;
    Age++;
    if (_energy <= 0) IsAlive = false;
  }

  public Agent? Reproduce()
  {
    const double maxEnergy = 200.0;
    if (_energy < maxEnergy * 0.9) return null;

    var childBrain = Brain.Clone();
    childBrain.Mutate(0.2);
    Agent child = new(Type, childBrain)
    {
      _energy = _energy / 2,
      _generation = _generation + 1
    };
    _energy /= 2;
    return child;
  }
}