using interfaces;

namespace classes;

public class GetInput : IGetInput
{
    public List<TestBlock> testBlocks = [];

    public GetInput()
    {
        int testBlocksCount = GetTestBlocksCount();
        Console.ReadLine();
        for (int i = 0; i < testBlocksCount; i++)
        {
            testBlocks.Add(new TestBlock());

            int tricksCount = GetTricksCount();
            for (int j = 0; j < tricksCount; j++)
            {
                GetTricks();
            }

            bool readEmptyLine = false;
            for (int j = 0; j < tricksCount || !readEmptyLine; j++)
            {
                readEmptyLine = GetWitnessedOrder();
            }
        }
    }

    public int GetTestBlocksCount()
    {
        return 0;
    }

    public int GetTricksCount()
    {
        return 0;
    }

    public void GetTricks()
    {

    }

    public bool GetWitnessedOrder()
    {
        return false;
    }
}