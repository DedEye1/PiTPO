using interfaces;

namespace classes;

public class TestBlock : ITestBlock
{
    public TestBlock()
    {
        Candidate.ResetIDs();
    }

    private readonly Dictionary<int, ICandidate> _candidates = [];
    private int _totalVotes = 0;

    private int _iteration = 0;
    public string CalculateWinner()
    {
        List<string> winners = [];
        while (!winners.Any())
        {
            System.Console.WriteLine($"###{++_iteration}###");
            CalculateCandidatesPercentages();
            System.Console.WriteLine(ToString());
            foreach (ICandidate candidate in _candidates.Values)
            {
                if (candidate.VotesPercentage > 50 || EqualCandidates())
                {
                    winners.Add(candidate.Name);
                }
            }
            if (!winners.Any())
            {
                int minVotes = _candidates.MinBy(cand => cand.Value.Votes).Value.Votes;
                foreach (ICandidate candidate in _candidates.Where(cand => cand.Value.Votes == minVotes).ToDictionary().Values)
                {
                    DiscardCandidate(candidate);
                }
            }
        }

        return string.Join("\n", winners);
    }

    private bool EqualCandidates()
    {
        int minVotes = _candidates.MinBy(cand => cand.Value.Votes).Value.Votes;
        int equalCands = _candidates.Count(cand => cand.Value.Votes == minVotes);
        return equalCands == _candidates.Count;
    }

    public void DiscardCandidate(ICandidate candidate)
    {
        System.Console.WriteLine($"To remove: {candidate.ID} - {candidate.Name}");
        _candidates.Remove(candidate.ID);
        foreach (ICandidate cand in _candidates.Values)
        {
            cand.RemoveCandidateID(candidate.ID);
        }
        foreach (List<int> bulletin in candidate.Bulletins)
        {
            int id = bulletin[0];
            bulletin.RemoveAt(0);

            PassBulletinToCandidateID(id, bulletin);
        }
    }

    public void CalculateCandidatesPercentages()
    {
        foreach (ICandidate candidate in _candidates.Values)
        {
            candidate.CalculatePercentage(_totalVotes);
        }
    }

    public void AddCandidate(ICandidate candidate)
    {
        _candidates.Add(candidate.ID, candidate);
    }

    public void PassBulletinToCandidateID(int id, List<int> bulletin)
    {
        _candidates[id].AddBulletin(bulletin);
    }

    public void AddBulletinToCandidateID(int id, List<int> bulletin)
    {
        _candidates[id].AddBulletin(bulletin);
        _totalVotes++;
    }

    public override string ToString()
    {
        return string.Join("\n", _candidates.Values) + $"\nTotal Votes:{_totalVotes}";
    }
}