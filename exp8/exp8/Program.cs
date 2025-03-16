using System;
using System.Collections.Generic;

public class Product
{
    public string Name { get; set; }
    public string Category { get; set; }
    public double Price { get; set; }

    public Product(string name, string category, double price)
    {
        Name = name;
        Category = category;
        Price = price;
    }

    public void Display()
    {
        Console.WriteLine($"Name: {Name}, Category: {Category}, Price: {Price}");
    }
}

public interface ISearchStrategy
{
    void Search(List<Product> products, string value);
}

public class SearchByName : ISearchStrategy
{
    public void Search(List<Product> products, string value)
    {
        foreach (var product in products)
        {
            if (product.Name.ToLower().Contains(value.ToLower()))
            {
                product.Display();
            }
        }
    }
}

public class SearchByCategory : ISearchStrategy
{
    public void Search(List<Product> products, string value)
    {
        foreach (var product in products)
        {
            if (product.Category.ToLower().Contains(value.ToLower()))
            {
                product.Display();
            }
        }
    }
}

public class SearchContext
{
    private ISearchStrategy _searchStrategy;

    public SearchContext(ISearchStrategy searchStrategy)
    {
        _searchStrategy = searchStrategy;
    }

    public void SetSearchStrategy(ISearchStrategy searchStrategy)
    {
        _searchStrategy = searchStrategy;
    }

    public void PerformSearch(List<Product> products, string value)
    {
        _searchStrategy.Search(products, value);
    }
}

class Program
{
    static void Main()
    {
        List<Product> products = new List<Product>
        {
            new Product("Laptop", "Electronics", 12000),
            new Product("Shirt", "Clothing", 900),
            new Product("Book", "Education", 500),
            new Product("Phone", "Electronics", 8000),
            new Product("Pants", "Clothing", 800)
        };

        Console.WriteLine("Products:");
        foreach (var product in products)
        {
            Console.WriteLine($"Name: {product.Name}");
        }

        Console.WriteLine("\nSelect search type:");
        Console.WriteLine("1. Search by Name");
        Console.WriteLine("2. Search by Category");
        int choice = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Enter search value:");
        string value = Console.ReadLine();

        SearchContext context = choice == 1
            ? new SearchContext(new SearchByName())
            : new SearchContext(new SearchByCategory());

        Console.WriteLine("\nSearch Results:");
        context.PerformSearch(products, value);
    }
}
