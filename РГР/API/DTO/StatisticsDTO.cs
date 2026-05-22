namespace API.DTO;

public class StatisticsDto
{
  public int Plants { get; set; }
  public int Herbivores { get; set; }
  public int Carnivores { get; set; }
  public int TotalEatenPlants { get; set; }
  public int TotalEatenHerbivores { get; set; }
  public double AvgHerbivoreAge { get; set; }
  public double AvgCarnivoreAge { get; set; }
}