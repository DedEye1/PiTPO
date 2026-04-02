namespace interfaces;

public interface ITestBlock
{
    public List<int> Weights { get; }
    public void PrintSumWeights();
}