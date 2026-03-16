using interfaces;

namespace classes;

public class TestBlock : ITestBlock
{
    private List<int> _order;
    private readonly List<List<int>> _tricks = [];
    private readonly List<int> _witnessedOrder = [];

    public TestBlock()
    {
        _order = Cards.GetCopyOfStartOrder();
    }

    public void PrintNewOrder()
    {
        CalculateNewOrder();
        List<string> cards = Cards.TranslateOrderToNames(_order);
        Console.WriteLine(string.Join("\n", cards));
    }

    public void CalculateNewOrder()
    {
        List<int> currentOrder = Cards.GetCopyOfStartOrder();

        foreach (int trickNumber in _witnessedOrder)
        {
            int trickIndex = trickNumber - 1;
            currentOrder = ApplyTrick(currentOrder, _tricks[trickIndex]);
        }

        _order = currentOrder;
    }

    public List<int> ApplyTrick(List<int> currentOrder, List<int> trick)
    {
        int[] newOrder = new int[currentOrder.Count];
        for (int i = 0; i < currentOrder.Count; i++)
        {
            int newPosition = trick[i] - 1;
            newOrder[newPosition] = currentOrder[i];
        }
        return newOrder.ToList();
    }

    public void AddTrick(List<int> trick)
    {
        _tricks.Add(trick);
    }

    public void AddWitnessedOrder(int order)
    {
        _witnessedOrder.Add(order);
    }
}