using interfaces;
using classes;

public class Program
{
    public static void Main()
    {
        GetInput getInput = new();

        foreach (ITestBlock testBlock in getInput.TestBlocks)
        {
            testBlock.PrintNewOrder();
            Console.WriteLine();
        }
    }
}