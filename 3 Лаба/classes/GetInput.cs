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
        bool readEmptyLine = false;

        ITestBlock testBlock = new TestBlock();
        for (int i = 1; i <= 2; i++)
        {
            string input = Console.ReadLine() ?? "Unknown";
            if (readEmptyLine = input == "") break;
            testBlock.AddString(input);
        }
        if (testBlock.StringsEmpty()) TestBlocks.Add(testBlock);

        return readEmptyLine;
    }
}