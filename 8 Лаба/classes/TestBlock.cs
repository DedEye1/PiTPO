using interfaces;

namespace classes;

public class TestBlock : ITestBlock
{
    public int Length { get; }
    public List<int> Cuts { get; }

    public TestBlock(int length, List<int> cuts)
    {
        Length = length;
        Cuts = cuts;
    }

    public void PrintResult()
    {
        Console.WriteLine(CalculateMinCuttingPrice());
    }

    private string CalculateMinCuttingPrice()
    {
        List<int> points = [0, .. Cuts, Length];
        int m = points.Count;

        int[,] dp = new int[m, m];

        for (int length = 2; length < m; length++)
        {
            for (int i = 0; i + length < m; i++)
            {
                int j = i + length;
                dp[i, j] = int.MaxValue;

                for (int k = i + 1; k < j; k++)
                {
                    int cost = dp[i, k] + dp[k, j] + (points[j] - points[i]);
                    if (cost < dp[i, j])
                    {
                        dp[i, j] = cost;
                    }
                }
            }
        }

        int minPrice = dp[0, m - 1];
        return $"The minimum cutting price is {minPrice}.";
    }
}