using System;
using System.Collections.Generic;

// Статический класс с обобщёнными методами

public static class CollectionUtils
{
    // 1. Убbраем дубликаты сохраняя порядок первых вхождений
    public static List<T> Distinct<T>(List<T> source)
    {
        var seen   = new HashSet<T>();
        var result = new List<T>();

        foreach (var item in source)
        {
            // HashSet.Add возвращает false, если элемент уже есть
            if (seen.Add(item))
                result.Add(item);
        }

        return result;
    }

    // 2. Сгруппируем элементы по ключу
    public static Dictionary<TKey, List<TValue>> GroupBy<TValue, TKey>(
        List<TValue> source,
        Func<TValue, TKey> keySelector) where TKey : notnull
    {
        var result = new Dictionary<TKey, List<TValue>>();

        foreach (var item in source)
        {
            TKey key = keySelector(item);

            if (!result.ContainsKey(key))
                result[key] = new List<TValue>();

            result[key].Add(item);
        }

        return result;
    }

    // 3. Объединим два словаря с резолвером конфликтов
    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
        Dictionary<TKey, TValue> first,
        Dictionary<TKey, TValue> second,
        Func<TValue, TValue, TValue> conflictResolver) where TKey : notnull
    {
        // Начинаем с копии первого словаря
        var result = new Dictionary<TKey, TValue>(first);

        foreach (var pair in second)
        {
            if (result.ContainsKey(pair.Key))
                
                result[pair.Key] = conflictResolver(result[pair.Key], pair.Value);
            else
                
                result[pair.Key] = pair.Value;
        }

        return result;
    }

    // 4. Найем элемент с максимальным значением селектора
    public static T MaxBy<T, TKey>(List<T> source, Func<T, TKey> selector)
        where TKey : IComparable<TKey>
    {
        if (source.Count == 0)
            throw new InvalidOperationException("Коллекция пуста.");

        T   maxItem  = source[0];
        TKey maxKey  = selector(source[0]);

        for (int i = 1; i < source.Count; i++)
        {
            TKey currentKey = selector(source[i]);

            
            if (currentKey.CompareTo(maxKey) > 0)
            {
                maxItem = source[i];
                maxKey  = currentKey;
            }
        }

        return maxItem;
    }
}


// Сущность из предыдущего ДЗ

public class Product
{
    public int     Id    { get; }
    public string  Name  { get; }
    public decimal Price { get; }

    public Product(int id, string name, decimal price)
    {
        Id    = id;
        Name  = name;
        Price = price;
    }

    public override string ToString() =>
        $"Product {{ Id={Id}, Name={Name}, Price={Price} }}";
}


// Точка входа — проверка всех методов

class Program
{
    static void Main()
    {
       
        Console.WriteLine("=== Distinct ===");

        var ints = new List<int> { 1, 2, 3, 2, 4, 1, 5 };
        var distinctInts = CollectionUtils.Distinct(ints);
        Console.WriteLine($"Исходный:  [{string.Join(", ", ints)}]");
        Console.WriteLine($"Distinct:  [{string.Join(", ", distinctInts)}]");

        var words = new List<string> { "яблоко", "груша", "яблоко", "слива", "груша" };
        var distinctWords = CollectionUtils.Distinct(words);
        Console.WriteLine($"\nИсходный:  [{string.Join(", ", words)}]");
        Console.WriteLine($"Distinct:  [{string.Join(", ", distinctWords)}]");

        
        Console.WriteLine("\n=== GroupBy (по длине слова) ===");

        var wordList = new List<string>
            { "кот", "слон", "кит", "тигр", "лев", "волк", "ёж" };

        var grouped = CollectionUtils.GroupBy(wordList, w => w.Length);

        foreach (var pair in grouped)
            Console.WriteLine($"  Длина {pair.Key}: [{string.Join(", ", pair.Value)}]");

       
        Console.WriteLine("\n=== Merge (счётчики слов, резолвер = сумма) ===");

        var text1 = new Dictionary<string, int>
        {
            ["привет"] = 3,
            ["мир"]    = 5,
            ["код"]    = 2
        };
        var text2 = new Dictionary<string, int>
        {
            ["мир"]     = 7,
            ["код"]     = 1,
            ["дракон"]  = 4
        };

        var merged = CollectionUtils.Merge(text1, text2,
            (a, b) => a + b);   // резолвер: складываем счётчики

        foreach (var pair in merged)
            Console.WriteLine($"  \"{pair.Key}\": {pair.Value}");

        
        Console.WriteLine("\n=== MaxBy (самый дорогой продукт) ===");

        var products = new List<Product>
        {
            new Product(1, "Мышь",     800),
            new Product(2, "Ноутбук",  75_000),
            new Product(3, "Монитор",  25_000),
            new Product(4, "Флешка",   500),
        };

        var mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
        Console.WriteLine($"Самый дорогой: {mostExpensive}");

        // Проверка исключения на пустой коллекции
        Console.WriteLine("\nMaxBy на пустом списке:");
        try
        {
            CollectionUtils.MaxBy(new List<Product>(), p => p.Price);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  Исключение: {ex.Message}");
        }
    }
}
