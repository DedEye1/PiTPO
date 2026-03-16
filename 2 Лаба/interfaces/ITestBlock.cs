namespace interfaces;

public interface ITestBlock
{
    public void PrintNewOrder();

    public void CalculateNewOrder();

    public List<int> ApplyTrick(List<int> currentOrder, List<int> trick);

    public void AddTrick(List<int> trick);

    public void AddWitnessedOrder(int order);
}