using classes;
using interfaces;

public class Program
{
    public static void Main()
    {
        foreach (ITestBlock testBlock in GetInput.TestBlocks)
        {
            testBlock.PrintResult();
        }
    }
}