namespace interfaces;

public interface IGetInput
{
    public int GetTestBlocksCount();

    public int GetTricksCount();

    public void GetTricks(ITestBlock testBlock);

    public bool GetWitnessedOrder(ITestBlock testBlock);
}