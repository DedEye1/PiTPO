namespace classes;

public class Problem
{
    public int Id { get; }
    public List<int> Categories { get; }

    public Problem(int id, List<int> categories)
    {
        Id = id;
        Categories = categories;
    }
}