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
            string? line = ReadNonEmptyLine();
            if (line == null) break;

            string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2) continue;

            int nk = int.Parse(parts[0]);
            int np = int.Parse(parts[1]);

            if (nk == 0 && np == 0)
                break;

            TestBlock testBlock = new TestBlock(testCaseNumber, nk, np);

            // Чтение потребностей категорий
            line = ReadNonEmptyLine();
            if (line == null) break;
            parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < nk && i < parts.Length; i++)
            {
                testBlock.CategoryNeeds.Add(int.Parse(parts[i]));
            }

            // Чтение задач
            for (int i = 1; i <= np; i++)
            {
                line = ReadNonEmptyLine();
                if (line == null) break;

                parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) break;

                int numCategories = int.Parse(parts[0]);
                List<int> categories = [];
                for (int j = 0; j < numCategories && (1 + j) < parts.Length; j++)
                {
                    categories.Add(int.Parse(parts[1 + j]));
                }
                testBlock.Problems.Add(new Problem(i, categories));
            }

            TestBlocks.Add(testBlock);
            testCaseNumber++;
        }
    }

    private static string? ReadNonEmptyLine()
    {
        while (true)
        {
            string? line = Console.ReadLine();
            if (line == null) return null;
            if (!string.IsNullOrWhiteSpace(line))
                return line.Trim();
        }
    }
}