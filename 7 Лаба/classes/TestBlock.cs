using interfaces;

namespace classes;

public class TestBlock() : ITestBlock
{
    public List<int> Weights { get; } = [];

    public void PrintSumWeights()
    {
        Console.WriteLine(CalculateWeights());
    }

    private string CalculateWeights()
    {
        int n = Weights.Count;
        int totalSum = Weights.Sum();

        int targetCount = n / 2;
        int maxWeight = totalSum / 2;

        bool[,] dp = new bool[targetCount + 1, maxWeight + 1];
        dp[0, 0] = true;

        foreach (int weight in Weights)
        {
            for (int count = targetCount; count >= 1; count--)
            {
                for (int sum = maxWeight; sum >= weight; sum--)
                {
                    if (dp[count - 1, sum - weight])
                    {
                        dp[count, sum] = true;
                    }
                }
            }
        }

        int bestSum = 0;
        for (int sum = maxWeight; sum >= 0; sum--)
        {
            if (dp[targetCount, sum])
            {
                bestSum = sum;
                break;
            }
        }

        if (n % 2 != 0)
        {
            int altCount = targetCount + 1;
            bool[,] dpAlt = new bool[altCount + 1, maxWeight + 1];
            dpAlt[0, 0] = true;

            foreach (int weight in Weights)
            {
                for (int count = altCount; count >= 1; count--)
                {
                    for (int sum = maxWeight; sum >= weight; sum--)
                    {
                        if (dpAlt[count - 1, sum - weight])
                        {
                            dpAlt[count, sum] = true;
                        }
                    }
                }
            }

            for (int sum = maxWeight; sum >= 0; sum--)
            {
                if (dpAlt[altCount, sum])
                {
                    bestSum = Math.Max(bestSum, sum);
                    break;
                }
            }
        }

        int team1 = bestSum;
        int team2 = totalSum - bestSum;
        string res;

        if (team1 < team2)
            res = $"{team1}\t{team2}";
        else
            res = $"{team2}\t{team1}";
        return res;
    }
}