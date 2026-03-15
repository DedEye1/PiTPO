using classes;
using interfaces;

public class Program
{
    public static void Main()
    {
        GetInput getInput = new();

        foreach (ITestBlock testBlock in getInput.TestBlocks)
        {
            Console.WriteLine(testBlock.CalculateWinner());
            Console.WriteLine();
        }
    }
}