namespace interfaces;

public interface IGetInput
{
    public void GetTestBlocksCount();

    public int GetCandidatesCount();

    public void GetCandidateName(ITestBlock testBlock);

    public bool GetBulletin(ITestBlock testBlock);
}