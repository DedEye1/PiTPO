using interfaces;

namespace classes;

public class GetInput : IGetInput
{
    public List<TestBlock> TestBlocks { get; } = [];

    public GetInput()
    {
        int testBlocksCount = GetTestBlocksCount();
        Console.ReadLine();
        for (int i = 0; i < testBlocksCount; i++)
        {
            TestBlock curTestBlock = new TestBlock();
            TestBlocks.Add(curTestBlock);

            int tricksCount = GetTricksCount();
            for (int j = 0; j < tricksCount; j++)
            {
                GetTricks(curTestBlock);
            }

            bool readEmptyLine = false;
            for (int j = 0; j < tricksCount || !readEmptyLine; j++)
            {
                readEmptyLine = GetWitnessedOrder(curTestBlock);
            }
        }
    }

    public int GetTestBlocksCount()
    {
        return Convert.ToInt32(Console.ReadLine());
    }

    public int GetTricksCount()
    {
        return Convert.ToInt32(Console.ReadLine());
    }

    public void GetTricks(ITestBlock testBlock)
    {
        List<int> tricks = [];

        string tricksString = Console.ReadLine()!;
        tricksString.Split(" ").ToList().ForEach(
            trick => tricks.Add(Convert.ToInt32(trick))
        );

        testBlock.AddTricks(tricks);
    }

    public bool GetWitnessedOrder(ITestBlock testBlock)
    {
        bool readEmptyLine;

        string? readInput = Console.ReadLine();
        readEmptyLine = string.IsNullOrEmpty(readInput);

        if (!readEmptyLine)
        {
            testBlock.AddWitnessedOrder(Convert.ToInt32(readInput));
        }

        return readEmptyLine;
    }
}