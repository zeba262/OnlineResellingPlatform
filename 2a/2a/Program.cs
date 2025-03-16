using System;

public class Product
{
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
}

public class ViewProducts
{
    public void DisplayProducts(Product[] products)
    {
        Console.WriteLine("Product List:");
        foreach (var product in products)
        {
            Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price}");
        }
    }
}

public class SearchByName
{
    public void Search(Product[] products, string name)
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
}

public class SearchByCategory
{
    public void Search(Product[] products, string category)
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
}

public class SearchByPrice
{
    public void Search(Product[] products, decimal minPrice, decimal maxPrice)
    {
        Console.WriteLine($"Searching for products with price between {minPrice} and {maxPrice}");
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
            new Product { Name = "Phone", Category = "Electronics", Price = 12000 },
            new Product { Name = "Laptop", Category = "Electronics", Price = 30000 },
            new Product { Name = "Shoes", Category = "Clothing", Price = 5000 }
        };

        ViewProducts viewer = new ViewProducts();
        viewer.DisplayProducts(products);

        Console.WriteLine("Enter search type ('name', 'category', or 'price'):");
        string searchType = Console.ReadLine();

        if (searchType.Equals("name", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Enter the name to search:");
            string searchValue = Console.ReadLine();
            SearchByName searcher = new SearchByName();
            searcher.Search(products, searchValue);
        }
        else if (searchType.Equals("category", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Enter the category to search:");
            string searchValue = Console.ReadLine();
            SearchByCategory searcher = new SearchByCategory();
            searcher.Search(products, searchValue);
        }
        else if (searchType.Equals("price", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Enter the minimum price:");
            decimal minPrice = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Enter the maximum price:");
            decimal maxPrice = Convert.ToDecimal(Console.ReadLine());
            SearchByPrice searcher = new SearchByPrice();
            searcher.Search(products, minPrice, maxPrice);
        }
    }
}
