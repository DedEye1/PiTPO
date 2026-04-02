using interfaces;

namespace classes;

public static class GetInput
{
    public static List<ITestBlock> TestBlocks { get; } = [];

    static GetInput()
    {
        int testBlocksCount = GetTestBlocksCount();
        for (int i = 0; i < testBlocksCount; i++)
        {
            int participantsCount = GetParticipantsCount();
            GetTestBlock(participantsCount);
        }
    }

    private static int GetTestBlocksCount()
    {
        return int.Parse(Console.ReadLine() ?? "0");
    }

    private static int GetParticipantsCount()
    {
        Console.ReadLine();
        return int.Parse(Console.ReadLine() ?? "0");
    }

    private static void GetTestBlock(int participantsCount)
    {
        ITestBlock testBlock = new TestBlock();
        for (int i = 0; i < participantsCount; i++)
        {
            int weight = int.Parse(Console.ReadLine() ?? "0");
            testBlock.Weights.Add(weight);
        }
        TestBlocks.Add(testBlock);
    }
}