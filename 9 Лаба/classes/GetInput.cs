using interfaces;

namespace classes;

public static class GetInput
{
    public static List<ITestBlock> TestBlocks { get; } = [];

    static GetInput()
    {
        int testCaseNumber = 1;
        while (true)
        {
            string? line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
                continue;

            int N = int.Parse(line);
            if (N == 0)
                break;

            GetTestBlock(N, testCaseNumber);
            testCaseNumber++;
        }
    }

    private static void GetTestBlock(int N, int testCaseNumber)
    {
        ITestBlock testBlock = new TestBlock(testCaseNumber);
        for (int i = 0; i < N; i++)
        {
            string? line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
                return;

            string[] colors = line.Split(' ');
            if (colors.Length >= 6)
            {
                int[] cubeColors = new int[6];
                for (int j = 0; j < 6; j++)
                {
                    cubeColors[j] = int.Parse(colors[j]);
                }
                testBlock.Cubes.Add(new Cube(i + 1, cubeColors));
            }
        }
        TestBlocks.Add(testBlock);
    }
}