using System;
using System.Collections.Generic;

// Интерфейс-контракт для всех сущностей
public interface IEntity
{
    int Id { get; }
}

// T — любой тип, реализующий IEntity

public class Repository<T> where T : IEntity
{
    private readonly Dictionary<int, T> _store = new();

    // Количество элементов
    public int Count => _store.Count;

    // Добавить элемент
    public void Add(T item)
    {
        if (_store.ContainsKey(item.Id))
            throw new InvalidOperationException(
                $"Элемент с Id={item.Id} уже существует.");

        _store[item.Id] = item;
    }

    // Удалить по Id
    public bool Remove(int id) => _store.Remove(id);

    // Получить по Id (null, если не найден)
    public T? GetById(int id) =>
        _store.TryGetValue(id, out var item) ? item : default;

    // Вернуть все элементы
    public IReadOnlyList<T> GetAll() =>
        new List<T>(_store.Values);

    // Поиск по условию
    public IReadOnlyList<T> Find(Predicate<T> predicate)
    {
        var result = new List<T>();
        foreach (var item in _store.Values)
            if (predicate(item))
                result.Add(item);
        return result;
    }
}

// ──────────────────────────────────────────────
// Конкретные сущности
// ──────────────────────────────────────────────
public class Product : IEntity
{
    public int Id { get; }
    public string Name { get; }
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

public class User : IEntity
{
    public int Id { get; }
    public string Username { get; }

    public User(int id, string username)
    {
        Id       = id;
        Username = username;
    }

    public override string ToString() =>
        $"User {{ Id={Id}, Username={Username} }}";
}


class Program
{
    static void Main()
    {
        var products = new Repository<Product>();

        products.Add(new Product(1, "Ноутбук",   75_000));
        products.Add(new Product(2, "Мышь",          800));
        products.Add(new Product(3, "Монитор",    25_000));
        products.Add(new Product(4, "Флешка",        500));

        Console.WriteLine($"Всего продуктов: {products.Count}");

        var found = products.GetById(3);
        Console.WriteLine($"GetById(3): {found}");

        Console.WriteLine("\nПродукты дороже 1000:");
        foreach (var p in products.Find(p => p.Price > 1_000))
            Console.WriteLine($"  {p}");

        // Попытка добавить дубликат
        Console.WriteLine("\nПробуем добавить дубликат с Id=1:");
        try
        {
            products.Add(new Product(1, "Дубль", 0));
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  Исключение: {ex.Message}");
        }

        bool removed = products.Remove(2);
        Console.WriteLine($"\nУдалён Id=2: {removed}");
        Console.WriteLine($"Всего продуктов после удаления: {products.Count}");

        // Тот же класс Repository<T> 
        var users = new Repository<User>();

        users.Add(new User(10, "alice"));
        users.Add(new User(11, "bob"));
        users.Add(new User(12, "carol"));

        Console.WriteLine($"\nВсего пользователей: {users.Count}");
        Console.WriteLine("Все пользователи:");
        foreach (var u in users.GetAll())
            Console.WriteLine($"  {u}");

        Console.WriteLine($"\nGetById(11): {users.GetById(11)}");
        Console.WriteLine($"GetById(99): {users.GetById(99)}");  // null
    }
}
