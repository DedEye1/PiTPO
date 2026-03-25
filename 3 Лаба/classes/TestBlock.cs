using interfaces;
using System.Text;

namespace classes;

public class TestBlock : ITestBlock
{
    private readonly List<string> _strings = [];

    public void AddString(string str)
    {
        _strings.Add(str);
    }

    public void PrintSubstring()
    {
        Console.WriteLine(CalculateSubstring(_strings[0], _strings[1]));
    }

    public bool StringsEmpty()
    {
        return _strings.Any();
    }

    private string CalculateSubstring(string a, string b)
    {
        int[] freqA = new int[26];
        int[] freqB = new int[26];

        foreach (char c in a)
        {
            freqA[c - 'a']++;
        }

        foreach (char c in b)
        {
            freqB[c - 'a']++;
        }

        StringBuilder result = new();
        for (int i = 0; i < 26; i++)
        {
            int count = Math.Min(freqA[i], freqB[i]);
            result.Append((char)('a' + i), count);
        }

        return result.ToString();
    }
}