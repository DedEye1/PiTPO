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
        bool isEmptyLine = false;

        ITestBlock testBlock = new TestBlock();
        string? input = Console.ReadLine();
        isEmptyLine = string.IsNullOrEmpty(input);
        if (!isEmptyLine)
        {
            testBlock.Order = input!.Split(" ").Select(int.Parse).ToList();
            TestBlocks.Add(testBlock);
        }

        return isEmptyLine;
    }
}