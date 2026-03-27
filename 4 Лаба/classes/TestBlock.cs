using interfaces;

namespace classes;

public class TestBlock : ITestBlock
{
    private List<int> _order = [];

    public List<int> Order
    {
        set
        {
            _order = value;
        }
    }

    public void PrintOrder()
    {
        Console.WriteLine(CalculateOrder());
    }

    private string CalculateOrder()
    {
        var pancakes = _order;
        List<int> flips = [];
        List<int> stack = [.. pancakes];
        int n = pancakes.Count;

        for (int size = n; size > 1; size--)
        {
            // Находим индекс максимального элемента в текущей неотсортированной части
            int maxIndex = 0;
            for (int i = 1; i < size; i++)
            {
                if (stack[i] > stack[maxIndex])
                    maxIndex = i;
            }

            // Если максимальный уже на месте (внизу текущей части), пропускаем
            if (maxIndex == size - 1)
                continue;

            // Если максимальный не наверху, поднимаем его наверх
            if (maxIndex != 0)
            {
                Flip(stack, maxIndex + 1);
                flips.Add(n - maxIndex); // Позиция от низа
            }

            // Опускаем максимальный на место (вниз текущей части)
            Flip(stack, size);
            flips.Add(n - size + 1); // Позиция от низа
        }

        // Добавляем завершающий 0
        flips.Add(0);

        return $"{string.Join(" ", pancakes)} {string.Join(" ", flips)}";
    }

    private void Flip(List<int> stack, int k)
    {
        int left = 0;
        int right = k - 1;

        while (left < right)
        {
            (stack[left], stack[right]) = (stack[right], stack[left]);
            left++;
            right--;
        }
    }
}