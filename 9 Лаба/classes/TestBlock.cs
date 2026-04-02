using interfaces;

namespace classes;

public class TestBlock(int testCaseNumber) : ITestBlock
{
    public int TestCaseNumber { get; } = testCaseNumber;
    public List<Cube> Cubes { get; } = [];

    private static readonly string[] FaceNames = ["front", "back", "left", "right", "top", "bottom"];
    private static readonly int[] Opposite = [1, 0, 3, 2, 5, 4];

    public void PrintResult()
    {
        Console.WriteLine($"Case #{TestCaseNumber}");

        var result = FindHighestTower();
        Console.WriteLine(result.Count);

        foreach (var cube in result)
        {
            Console.WriteLine($"{cube.CubeId} {cube.TopFace}");
        }
    }

    private List<TowerCube> FindHighestTower()
    {
        int n = Cubes.Count;

        int[,] dp = new int[n, 6];
        int[,] prevCube = new int[n, 6];
        int[,] prevFace = new int[n, 6];

        for (int i = 0; i < n; i++)
        {
            for (int f = 0; f < 6; f++)
            {
                dp[i, f] = 1;
                prevCube[i, f] = -1;
                prevFace[i, f] = -1;
            }
        }

        for (int i = 0; i < n; i++)
        {
            for (int f = 0; f < 6; f++)
            {
                int bottomColor = Cubes[i].Colors[Opposite[f]];

                for (int j = 0; j < i; j++)
                {
                    for (int f2 = 0; f2 < 6; f2++)
                    {
                        int topColorJ = Cubes[j].Colors[f2];

                        if (topColorJ == bottomColor)
                        {
                            if (dp[j, f2] + 1 > dp[i, f])
                            {
                                dp[i, f] = dp[j, f2] + 1;
                                prevCube[i, f] = j;
                                prevFace[i, f] = f2;
                            }
                        }
                    }
                }
            }
        }

        int bestHeight = 0;
        int bestCube = -1;
        int bestFace = -1;

        for (int i = 0; i < n; i++)
        {
            for (int f = 0; f < 6; f++)
            {
                if (dp[i, f] > bestHeight)
                {
                    bestHeight = dp[i, f];
                    bestCube = i;
                    bestFace = f;
                }
            }
        }

        List<TowerCube> tower = [];

        List<(int cube, int face)> path = [];
        int currCube = bestCube;
        int currFace = bestFace;

        while (currCube != -1)
        {
            path.Add((currCube, currFace));
            int nextCube = prevCube[currCube, currFace];
            int nextFace = prevFace[currCube, currFace];
            currCube = nextCube;
            currFace = nextFace;
        }

        for (int i = path.Count - 1; i >= 0; i--)
        {
            tower.Add(new TowerCube(Cubes[path[i].cube].Id, FaceNames[path[i].face]));
        }

        return tower;
    }
}