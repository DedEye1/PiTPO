namespace interfaces;

public interface ITestBlock
{
    public string CalculateWinner();

    public void DiscardCandidate(ICandidate candidate);

    public void CalculateCandidatesPercentages();

    public void AddCandidate(ICandidate candidate);

    public void PassBulletinToCandidateID(int id, List<int> bulletin);

    public void AddBulletinToCandidateID(int id, List<int> bulletin);
}