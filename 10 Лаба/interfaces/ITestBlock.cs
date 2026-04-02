using classes;

namespace interfaces;

public interface ITestBlock
{
    int TestCaseNumber { get; }
    int NumCategories { get; }
    int NumProblems { get; }
    List<int> CategoryNeeds { get; }
    List<Problem> Problems { get; }
    void PrintResult();
}