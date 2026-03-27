using interfaces;

namespace classes;

public class TestBlock(int number) : ITestBlock
{
    private readonly int number = number;

    public void PrintPower()
    {
        Console.WriteLine(CalculatePower(Convert.ToString(number)));
    }

    private string CalculatePower(string number)
    {
        if (number == "0") return "no power of 2";

        int len = number.Length;
        long target = long.Parse(number);

        for (int E = 1; E <= 1000000; E++)
        {
            double logValue = E * Math.Log10(2);
            double fractionalPart = logValue - Math.Floor(logValue);

            int powerLen = (int)Math.Floor(logValue) + 1;

            if (powerLen <= len * 2)
                continue;

            double firstDigits = Math.Pow(10, fractionalPart + len - 1);
            long firstDigitsLong = (long)Math.Floor(firstDigits + 1e-10);

            if (firstDigitsLong == target)
            {
                return E.ToString();
            }
        }

        return "no power of 2";
    }
}