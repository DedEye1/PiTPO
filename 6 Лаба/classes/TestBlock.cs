using interfaces;

namespace classes;

public class TestBlock(int[] pair) : ITestBlock
{
    private readonly int[] _pair = pair;

    public void PrintPower()
    {
        Console.WriteLine(CalculateExp());
    }

    private string CalculateExp()
    {
        int n = _pair[0];
        int d = _pair[1];

        if (n % 2 != 0 || d < 1 || d > n / 2)
            return "0";

        int[,] dp = new int[n + 1, d + 1];
        dp[0, 0] = 1;

        for (int l = 2; l <= n; l += 2)
        {
            for (int h = 1; h <= Math.Min(d, l / 2); h++)
            {
                for (int k = 2; k <= l; k += 2)
                {
                    int innerLen = k - 2;
                    int outerLen = l - k;

                    for (int hA = 0; hA <= h - 1; hA++)
                    {
                        int inner = (innerLen == 0)
                            ? (hA == 0 ? 1 : 0)
                            : dp[innerLen, hA];

                        if (inner == 0) continue;

                        if (hA + 1 == h)
                        {
                            for (int hB = 0; hB <= h; hB++)
                            {
                                int outer = (outerLen == 0)
                                    ? (hB == 0 ? 1 : 0)
                                    : dp[outerLen, hB];
                                dp[l, h] += inner * outer;
                            }
                        }
                        else
                        {
                            int outer = (outerLen == 0)
                                ? (h == 0 ? 1 : 0)
                                : dp[outerLen, h];
                            dp[l, h] += inner * outer;
                        }
                    }
                }
            }
        }

        return dp[n, d].ToString();
    }
}