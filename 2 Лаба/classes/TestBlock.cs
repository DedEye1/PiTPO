using interfaces;

namespace classes;

public class TestBlock : ITestBlock
{
    private readonly List<List<int>> _tricks = [];
    private readonly List<int> _witnessedOrder = [];

    public void AddTricks(List<int> tricks)
    {
        _tricks.Add(tricks);
    }

    public void AddWitnessedOrder(int order)
    {
        _witnessedOrder.Add(order);
    }

    public override string ToString()
    {
        string tricksString = "";
        foreach (List<int> tricks in _tricks)
        {
            tricksString += string.Join(", ", tricks) + "\n";
        }

        return $"Tricks:\n{tricksString}" +
        $"Witnessed order:\n{string.Join("\n", _witnessedOrder)}";
    }
}