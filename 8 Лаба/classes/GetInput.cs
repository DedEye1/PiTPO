using interfaces;

namespace classes;

public static class GetInput
{
    public static List<ITestBlock> TestBlocks { get; } = [];

    static GetInput()
    {
        while (true)
        {
            int length = GetLength();
            if (length == 0) break;

            int cutsCount = GetCutsCount();
            List<int> cuts = GetCuts(cutsCount);

            ITestBlock testBlock = new TestBlock(length, cuts);
            TestBlocks.Add(testBlock);
        }
    }

    private static int GetLength()
    {
        return int.Parse(Console.ReadLine() ?? "0");
    }

    private static int GetCutsCount()
    {
        return int.Parse(Console.ReadLine() ?? "0");
    }

    private static List<int> GetCuts(int cutsCount)
    {
        string line = Console.ReadLine() ?? "";

        string[] parts = line.Split(" ");
        List<int> cuts = [];

        for (int i = 0; i < cutsCount && i < parts.Length; i++)
        {
            cuts.Add(int.Parse(parts[i]));
        }

        return cuts;
    }
}