using interfaces;

namespace classes;

public class Candidate : ICandidate
{
    private static int _ID = 1;

    public int ID { get; }
    public string Name { get; }
    public int Votes { get; private set; } = 0;
    public float VotesPercentage { get; private set; }
    public List<List<int>> Bulletins { get; } = [];

    public Candidate(string? name)
    {
        Name = name ?? "Unknown";
        ID = _ID++;
    }

    public void AddBulletin(List<int> bulletin)
    {
        Bulletins.Add(bulletin);
        Votes++;
    }

    public void CalculatePercentage(int totalVotes)
    {
        VotesPercentage = (float)Votes / totalVotes * 100;
    }

    public void RemoveCandidateID(int id)
    {
        foreach (List<int> bulletin in Bulletins)
        {
            bulletin.Remove(id);
        }
    }

    public override string ToString()
    {
        string bulletins = "";
        foreach (List<int> bulletin in Bulletins)
        {
            bulletins += string.Join(", ", bulletin) + "\n";
        }

        return $"###Candidate ID: {ID}###\n" +
        $"Bulletins:\n{bulletins}\n" +
        $"Percentage: {VotesPercentage}\n";
    }
}