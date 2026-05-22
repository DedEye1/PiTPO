using Core.structures;
using Core.enums;

namespace Core.classes;

public class Agent(AgentType type, NeuralNetwork? brain = null)
{
  public AgentType Type = type;
  public Point Location;
  public Direction Facing = Direction.North;
  public double Energy = 100.0;
  public int Age = 0;
  public int Generation = 1;
  public bool IsAlive = true;
  public NeuralNetwork Brain = brain ?? new NeuralNetwork();

  public double[] GetSensors(SimulationEnvironment env)
  {
    double[] sensors = new double[12];
    int idx = 0;
    for (int zone = 0; zone < 4; zone++)
    {
      var positions = GetZonePositions(zone);
      int plantCount = 0, herbCount = 0, carnCount = 0;
      foreach (var pos in positions)
      {
        EntityType entity = env.GetEntityAt(pos);
        if (entity == EntityType.Plant) plantCount++;
        else if (entity == EntityType.Herbivore) herbCount++;
        else if (entity == EntityType.Carnivore) carnCount++;
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
    int[,] offsets;
    switch (zone)
    {
      case 0: offsets = new int[,] { { -2, -2 }, { -2, -1 }, { -2, 0 }, { -2, 1 }, { -2, 2 } }; break;
      case 1: offsets = new int[,] { { -1, -2 }, { 0, -2 } }; break;
      case 2: offsets = new int[,] { { -1, 2 }, { 0, 2 } }; break;
      default: offsets = new int[,] { { 0, -1 }, { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, 1 } }; break;
    }
    for (int i = 0; i < offsets.GetLength(0); i++)
    {
      int dx = offsets[i, 0], dy = offsets[i, 1];
      (dx, dy) = RotateOffset(dx, dy, Facing);
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
      case 0: Facing = (Direction)(((int)Facing + 3) % 4); break;
      case 1: Facing = (Direction)(((int)Facing + 1) % 4); break;
      case 2: Move(env); break;
      case 3: Eat(env); break;
    }
  }

  private void Move(SimulationEnvironment env)
  {
    Point newPos = Location;
    switch (Facing)
    {
      case Direction.North: newPos.X--; break;
      case Direction.South: newPos.X++; break;
      case Direction.East: newPos.Y++; break;
      case Direction.West: newPos.Y--; break;
    }
    if (SimulationEnvironment.IsValidPosition(newPos) && env.GetEntityAt(newPos) == EntityType.Empty)
    {
      env.MoveAgent(this, newPos);
      Location = newPos;
    }
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
        Energy += 30;
        return;
      }
      else if (Type == AgentType.Carnivore && food == EntityType.Herbivore)
      {
        env.RemoveHerbivore(pos);
        Energy += 50;
        return;
      }
    }
  }

  public void UpdateMetabolism()
  {
    Energy -= (Type == AgentType.Herbivore) ? 2.0 : 1.0;
    Age++;
    if (Energy <= 0) IsAlive = false;
  }

  public Agent? Reproduce()
  {
    const double MaxEnergy = 200.0;
    if (Energy < MaxEnergy * 0.9) return null;

    NeuralNetwork childBrain = Brain.Clone();
    childBrain.Mutate(0.2);
    Agent child = new(Type, childBrain)
    {
      Energy = Energy / 2,
      Generation = Generation + 1
    };
    Energy /= 2;
    return child;
  }
}