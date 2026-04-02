using classes;

public class Program
{
    public static void Main()
    {
        for (int i = 0; i < GetInput.TestBlocks.Count; i++)
        {
            GetInput.TestBlocks[i].PrintResult();
            if (i < GetInput.TestBlocks.Count - 1)
            {
                Console.WriteLine();
            }
        }
    }
}