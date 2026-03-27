using interfaces;

namespace classes;

public static class GetInput
{
    public static List<ITestBlock> TestBlocks { get; } = [];

    static GetInput()
    {
        while (!GetTestBlock()) ;
    }

    private static bool GetTestBlock()
    {
        string? input = Console.ReadLine();
        bool isEmptyLine = string.IsNullOrEmpty(input);
        if (!isEmptyLine)
        {
            int[] pair = input!.Split(" ").Select(int.Parse).ToArray();
            ITestBlock testBlock = new TestBlock(pair);
            TestBlocks.Add(testBlock);
        }

        return isEmptyLine;
    }
}