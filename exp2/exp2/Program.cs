using System;
using System.Buffers;

public class Product
{
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
}

// Class-based implementation
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

public class SearchProducts
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

// Interface-based implementation
public interface IViewProducts
{
    void DisplayProducts(Product[] products);
}

public interface ISearchProducts
{
    void SearchByName(Product[] products, string name);
    void SearchByCategory(Product[] products, string category);
    void SearchByPrice(Product[] products, decimal minPrice, decimal maxPrice);
}

public class ViewProductsInterface : IViewProducts
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

public class SearchProductsInterface : ISearchProducts
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

// Abstract class-based implementation
public abstract class ProductViewer
{
    public abstract void DisplayProducts(Product[] products);
}

public abstract class ProductSearcher
{
    public abstract void SearchByName(Product[] products, string name);
    public abstract void SearchByCategory(Product[] products, string category);
    public abstract void SearchByPrice(Product[] products, decimal minPrice, decimal maxPrice);
}

public class ViewProductsAbstract : ProductViewer
{
    public override void DisplayProducts(Product[] products)
    {
        Console.WriteLine("Product List:");
        foreach (var product in products)
        {
            Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price}");
        }
    }
}

public class SearchProductsAbstract : ProductSearcher
{
    public override void SearchByName(Product[] products, string name)
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

    public override void SearchByCategory(Product[] products, string category)
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

    public override void SearchByPrice(Product[] products, decimal minPrice, decimal maxPrice)
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

// Virtual method-based implementation
public class ProductViewerVirtual
{
    public virtual void DisplayProducts(Product[] products)
    {
        Console.WriteLine("Product List:");
        foreach (var product in products)
        {
            Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price}");
        }
    }
}

public class ProductSearcherVirtual
{
    public virtual void SearchByName(Product[] products, string name)
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

    public virtual void SearchByCategory(Product[] products, string category)
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

    public virtual void SearchByPrice(Product[] products, decimal minPrice, decimal maxPrice)
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
            new Product { Name = "Phone", Category = "Electronics", Price = 500 },
            new Product { Name = "Laptop", Category = "Electronics", Price = 1000 },
            new Product { Name = "Shoes", Category = "Clothing", Price = 50 }
        };

        // Create searcher objects before use to avoid redundancy
        SearchProducts searcher = new SearchProducts();
        SearchProductsInterface searcherInterface = new SearchProductsInterface();
        SearchProductsAbstract searcherAbstract = new SearchProductsAbstract();
        ProductSearcherVirtual searcherVirtual = new ProductSearcherVirtual();

        // Classes Implementation
        Console.WriteLine("--- Classes Implementation ---");
        ViewProducts viewer = new ViewProducts();
        viewer.DisplayProducts(products);

        HandleSearch(products, searcher);

        // Interface Implementation
        Console.WriteLine("\n--- Interface Implementation ---");
        IViewProducts viewerInterface = new ViewProductsInterface();
        viewerInterface.DisplayProducts(products);

        HandleSearch(products, searcherInterface);

        // Abstract Class Implementation
        Console.WriteLine("\n--- Abstract Class Implementation ---");
        ProductViewer viewerAbstract = new ViewProductsAbstract();
        viewerAbstract.DisplayProducts(products);

        HandleSearch(products, searcherAbstract);

        // Virtual Method Implementation
        Console.WriteLine("\n--- Virtual Method Implementation ---");
        ProductViewerVirtual viewerVirtual = new ProductViewerVirtual();
        viewerVirtual.DisplayProducts(products);

        HandleSearch(products, searcherVirtual);
    }

    // Consolidated search handling logic to avoid redundancy
    static void HandleSearch(Product[] products, dynamic searcher)
    {
        Console.WriteLine("Enter search type ('name', 'category', or 'price'):");
        string searchType = Console.ReadLine();

        if (searchType.Equals("name", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"Enter the {searchType} to search:");
            string searchValue = Console.ReadLine();
            searcher.SearchByName(products, searchValue);
        }
        else if (searchType.Equals("category", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"Enter the {searchType} to search:");
            string searchValue = Console.ReadLine();
            searcher.SearchByCategory(products, searchValue);
        }
        else if (searchType.Equals("price", StringComparison.OrdinalIgnoreCase))
        {
            decimal minPrice = 0, maxPrice = 0;

            // Handle potential parsing errors for price
            Console.WriteLine("Enter minimum price:");
            while (!decimal.TryParse(Console.ReadLine(), out minPrice))
            {
                Console.WriteLine("Invalid input. Please enter a valid decimal value for minimum price:");
            }

            Console.WriteLine("Enter maximum price:");
            while (!decimal.TryParse(Console.ReadLine(), out maxPrice))
            {
                Console.WriteLine("Invalid input. Please enter a valid decimal value for maximum price:");
            }

            searcher.SearchByPrice(products, minPrice, maxPrice);
        }
        else
        {
            Console.WriteLine("Invalid search type. Please enter 'name', 'category', or 'price'.");
        }
    }
}
