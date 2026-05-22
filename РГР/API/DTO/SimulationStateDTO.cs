namespace API.DTO;

public class SimulationStateDto
{
  public int Step { get; set; }
  public int GridSize { get; set; }
  public List<CellDto> Grid { get; set; } = new();
  public StatisticsDto Statistics { get; set; } = new();
}