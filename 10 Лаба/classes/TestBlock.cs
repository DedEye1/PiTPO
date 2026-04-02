using interfaces;

namespace classes;

public class TestBlock(int testCaseNumber, int numCategories, int numProblems) : ITestBlock
{
    public int TestCaseNumber { get; } = testCaseNumber;
    public int NumCategories { get; } = numCategories;
    public int NumProblems { get; } = numProblems;
    public List<int> CategoryNeeds { get; } = [];
    public List<Problem> Problems { get; } = [];

    public void PrintResult()
    {
        Console.WriteLine();
        var (possible, assignment) = Solve();
        Console.WriteLine(possible ? 1 : 0);

        if (possible)
        {
            for (int i = 1; i <= NumCategories; i++)
            {
                if (assignment.ContainsKey(i))
                {
                    Console.WriteLine(string.Join(" ", assignment[i]));
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }
    }

    private (bool possible, Dictionary<int, List<int>> assignment) Solve()
    {
        int source = 0;
        int sink = NumCategories + NumProblems + 1;
        int totalNodes = sink + 1;

        MaxFlowGraph graph = new MaxFlowGraph(totalNodes);

        for (int i = 1; i <= NumCategories; i++)
        {
            graph.AddEdge(source, i, CategoryNeeds[i - 1]);
        }

        for (int j = 1; j <= NumProblems; j++)
        {
            int problemNode = NumCategories + j;
            graph.AddEdge(problemNode, sink, 1);
        }

        for (int j = 0; j < NumProblems; j++)
        {
            Problem problem = Problems[j];
            int problemNode = NumCategories + problem.Id;

            foreach (int category in problem.Categories)
            {
                if (category >= 1 && category <= NumCategories)
                {
                    graph.AddEdge(category, problemNode, 1);
                }
            }
        }

        int maxFlow = graph.MaxFlow(source, sink);

        int totalNeeded = CategoryNeeds.Sum();
        bool possible = maxFlow == totalNeeded;

        if (!possible)
            return (false, null!);

        Dictionary<int, List<int>> assignment = [];
        for (int i = 1; i <= NumCategories; i++)
        {
            assignment[i] = [];
        }

        foreach (var edge in graph.GetEdges())
        {
            if (edge.From >= 1 && edge.From <= NumCategories &&
                edge.To >= NumCategories + 1 && edge.To <= NumCategories + NumProblems &&
                edge.Flow == 1)
            {
                int category = edge.From;
                int problemId = edge.To - NumCategories;
                assignment[category].Add(problemId);
            }
        }

        foreach (var category in assignment.Keys.ToList())
        {
            assignment[category].Sort();
        }

        return (true, assignment);
    }
}