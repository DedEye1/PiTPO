using classes;

namespace interfaces;

public interface ITestBlock
{
    int TestCaseNumber { get; }
    List<Cube> Cubes { get; }
    void PrintResult();
}