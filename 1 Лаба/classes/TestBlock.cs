using interfaces;

namespace classes;

public class TestBlock : ITestBlock
{
    private readonly List<ICandidate> _candidates = [];
    private int _totalVotes = 0;

    private int iteration = 0;
    public string CalculateWinner()
    {
        List<string> winners = [];
        while (!winners.Any())
        {
            CalculateCandidatesPercentages();
            foreach (ICandidate candidate in _candidates)
            {
                if (candidate.VotesPercentage > 50 || EqualCandidates())
                {
                    winners.Add(candidate.Name);
                }
            }
            if (!winners.Any())
            {
                int minVotes = _candidates.MinBy(cand => cand.Votes)!.Votes;
                foreach (ICandidate candidate in _candidates.Where(cand => cand.Votes == minVotes).ToList())
                {
                    DiscardCandidate(candidate);
                }
            }
        }

        return string.Join("\n", winners);
    }

    private bool EqualCandidates()
    {
        int minVotes = _candidates.MinBy(cand => cand.Votes)!.Votes;
        int equalCands = _candidates.Count(cand => cand.Votes == minVotes);
        return equalCands == _candidates.Count;
    }

    public void DiscardCandidate(ICandidate candidate)
    {
        _candidates.Remove(candidate);
        foreach (ICandidate cand in _candidates)
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
        foreach (ICandidate candidate in _candidates)
        {
            candidate.CalculatePercentage(_totalVotes);
        }
    }

    public void AddCandidate(ICandidate candidate)
    {
        _candidates.Add(candidate);
    }

    public void PassBulletinToCandidateID(int id, List<int> bulletin)
    {
        _candidates[id - 1].AddBulletin(bulletin);
    }

    public void AddBulletinToCandidateID(int id, List<int> bulletin)
    {
        _candidates[id - 1].AddBulletin(bulletin);
        _totalVotes++;
    }

    public override string ToString()
    {
        return string.Join("\n", _candidates) + $"\nTotal Votes:{_totalVotes}";
    }
}