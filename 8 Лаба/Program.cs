using classes;

namespace CuttingWood;

class Program
{
    static void Main()
    {
        foreach (var testBlock in GetInput.TestBlocks)
        {
            testBlock.PrintResult();
        }
    }
}