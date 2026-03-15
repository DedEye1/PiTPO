using interfaces;

namespace classes;

public class GetInput : IGetInput
{
    private const int BULLETIN_LINES = 1000;

    private int _testBlocksCount = 0;
    public List<ITestBlock> TestBlocks { get; } = [];

    public GetInput()
    {
        GetTestBlocksCount();
        Console.ReadLine();
        for (int i = 0; i < _testBlocksCount; i++)
        {
            TestBlocks.Add(new TestBlock());

            int candidatesCount = GetCandidatesCount();
            for (int j = 0; j < candidatesCount; j++)
            {
                GetCandidateName(TestBlocks[i]);
            }

            bool readEmptyLine = false;
            for (int j = 0; j < BULLETIN_LINES && !readEmptyLine; j++)
            {
                readEmptyLine = GetBulletin(TestBlocks[i]);
            }
        }
    }

    public void GetTestBlocksCount()
    {
        _testBlocksCount = Convert.ToInt32(Console.ReadLine());
    }

    public int GetCandidatesCount()
    {
        return Convert.ToInt32(Console.ReadLine());
    }

    public void GetCandidateName(ITestBlock testBlock)
    {
        string? candidateName = Console.ReadLine();
        ICandidate candidate = new Candidate(candidateName);
        testBlock.AddCandidate(candidate);
    }

    public bool GetBulletin(ITestBlock testBlock)
    {
        bool readEmptyLine;

        List<int> bulletin = [];
        string? readInput = Console.ReadLine();
        readEmptyLine = string.IsNullOrEmpty(readInput);

        if (!readEmptyLine)
        {
            readInput!.Split(" ").ToList().ForEach(
                vote => bulletin.Add(Convert.ToInt32(vote))
            );

            int id = bulletin[0];
            bulletin.RemoveAt(0);

            testBlock.AddBulletinToCandidateID(id, bulletin);
        }

        return readEmptyLine;
    }
}