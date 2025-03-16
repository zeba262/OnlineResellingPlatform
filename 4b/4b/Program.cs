using System;

public class Product
{
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
}


public interface ISearchByName
{
    void SearchByName(Product[] products, string name);
}

public interface ISearchByCategory
{
    void SearchByCategory(Product[] products, string category);
}

public interface ISearchByPrice
{
    void SearchByPrice(Product[] products, decimal minPrice, decimal maxPrice);
}

public class ProductSearch : ISearchByName, ISearchByCategory, ISearchByPrice
{
    public void SearchByName(Product[] products, string name)
    {
        Console.WriteLine($"Searching for name: {name}");
        foreach (var product in products)
        {
            if (product.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price}");
            }
        }
    }

    public void SearchByCategory(Product[] products, string category)
    {
        Console.WriteLine($"Searching for category: {category}");
        foreach (var product in products)
        {
            if (product.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price}");
            }
        }
    }

    public void SearchByPrice(Product[] products, decimal minPrice, decimal maxPrice)
    {
        Console.WriteLine($"Searching for products priced between {minPrice} and {maxPrice}");
        foreach (var product in products)
        {
            if (product.Price >= minPrice && product.Price <= maxPrice)
            {
                Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price}");
            }
        }
    }
}

class Program
{
    static void Main()
    {
        Product[] products = {
            new Product { Name = "Phone", Category = "Electronics", Price = 8000 },
            new Product { Name = "Laptop", Category = "Electronics", Price = 25000 },
            new Product { Name = "Shoes", Category = "Clothing", Price = 5000 }
        };

        ProductSearch searcher = new ProductSearch();

        Console.WriteLine("Enter search type ('name', 'category', 'price'):");
        string searchType = Console.ReadLine();

        if (searchType.Equals("name", StringComparison.OrdinalIgnoreCase))
        {
            Console.Write("Enter the product name: ");
            string name = Console.ReadLine();
            searcher.SearchByName(products, name);
        }
        else if (searchType.Equals("category", StringComparison.OrdinalIgnoreCase))
        {
            Console.Write("Enter the category: ");
            string category = Console.ReadLine();
            searcher.SearchByCategory(products, category);
        }
        else if (searchType.Equals("price", StringComparison.OrdinalIgnoreCase))
        {
            Console.Write("Enter minimum price: ");
            decimal minPrice = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Enter maximum price: ");
            decimal maxPrice = Convert.ToDecimal(Console.ReadLine());

            searcher.SearchByPrice(products, minPrice, maxPrice);
        }
        else
        {
            Console.WriteLine("Invalid search type.");
        }
    }
}
