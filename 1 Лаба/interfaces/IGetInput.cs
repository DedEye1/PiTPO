namespace interfaces;

public interface IGetInput
{
    public int GetTestBlocksCount();

    public int GetCandidatesCount();

    public void GetCandidateName(ITestBlock testBlock);

    public bool GetBulletin(ITestBlock testBlock);
}