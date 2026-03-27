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
            int number = int.Parse(input!);
            ITestBlock testBlock = new TestBlock(number);
            TestBlocks.Add(testBlock);
        }

        return isEmptyLine;
    }
}