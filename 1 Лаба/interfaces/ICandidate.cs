namespace interfaces;

public interface ICandidate
{
    public int ID { get; }

    public string Name { get; }

    public int Votes { get; }

    public float VotesPercentage { get; }

    public List<List<int>> Bulletins { get; }

    public void AddBulletin(List<int> bulletin);

    public void CalculatePercentage(int totalVotes);

    public void RemoveCandidateID(int id);
}