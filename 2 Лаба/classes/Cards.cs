namespace classes;

public static class Cards
{
    private static readonly List<int> _startOrder = [
        1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
        11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
        21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
        31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
        41, 42, 43, 44, 45, 46, 47, 48, 49, 50,
        51, 52
    ];

    public static List<int> GetCopyOfStartOrder()
    {
        return new(_startOrder);
    }

    public static List<string> TranslateOrderToNames(List<int> order)
    {
        string[] suits = ["Clubs", "Diamonds", "Hearts", "Spades"];

        string[] values = [
            "2", "3", "4", "5", "6", "7", "8", "9", "10",
            "Jack", "Queen", "King", "Ace"
        ];

        List<string> result = [];

        foreach (int cardNumber in order)
        {
            int suitIndex = (cardNumber - 1) / 13;
            int valueIndex = (cardNumber - 1) % 13;

            string cardName = $"{values[valueIndex]} of {suits[suitIndex]}";
            result.Add(cardName);
        }

        return result;
    }
}