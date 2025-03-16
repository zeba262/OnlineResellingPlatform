using System;

public class Product
{
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
}

// Classes Implementation
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
}

// Interface Implementation
public interface IViewProducts
{
    void DisplayProducts(Product[] products);
}

public interface ISearchProducts
{
    void SearchByName(Product[] products, string name);
    void SearchByCategory(Product[] products, string category);
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
                Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price:C}");
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
                Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price:C}");
            }
        }
    }
}

// Abstract Class Implementation
public abstract class ProductViewer
{
    public abstract void DisplayProducts(Product[] products);
}

public abstract class ProductSearcher
{
    public abstract void SearchByName(Product[] products, string name);
    public abstract void SearchByCategory(Product[] products, string category);
}

public class ViewProductsAbstract : ProductViewer
{
    public override void DisplayProducts(Product[] products)
    {
        Console.WriteLine("Product List:");
        foreach (var product in products)
        {
            Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price:C}");
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
                Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price:C}");
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
                Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price:C}");
            }
        }
    }
}

// Virtual Method Implementation
public class ProductViewerVirtual
{
    public virtual void DisplayProducts(Product[] products)
    {
        Console.WriteLine("Product List:");
        foreach (var product in products)
        {
            Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price:C}");
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
                Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price:C}");
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
                Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price:C}");
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

        // Classes Implementation
        Console.WriteLine("--- Classes Implementation ---");
        ViewProducts viewer = new ViewProducts();
        viewer.DisplayProducts(products);

        Console.WriteLine("Enter search type ('name' or 'category'):");
        string searchType = Console.ReadLine();
        Console.WriteLine($"Enter the {searchType} to search:");
        string searchValue = Console.ReadLine();

        SearchProducts searcher = new SearchProducts();
        if (searchType.Equals("name", StringComparison.OrdinalIgnoreCase))
        {
            searcher.SearchByName(products, searchValue);
        }
        else if (searchType.Equals("category", StringComparison.OrdinalIgnoreCase))
        {
            searcher.SearchByCategory(products, searchValue);
        }

        // Interface Implementation
        Console.WriteLine("\n--- Interface Implementation ---");
        IViewProducts viewerInterface = new ViewProductsInterface();
        viewerInterface.DisplayProducts(products);

        Console.WriteLine("Enter search type ('name' or 'category'):");
        searchType = Console.ReadLine();
        Console.WriteLine($"Enter the {searchType} to search:");
        searchValue = Console.ReadLine();

        ISearchProducts searcherInterface = new SearchProductsInterface();
        if (searchType.Equals("name", StringComparison.OrdinalIgnoreCase))
        {
            searcherInterface.SearchByName(products, searchValue);
        }
        else if (searchType.Equals("category", StringComparison.OrdinalIgnoreCase))
        {
            searcherInterface.SearchByCategory(products, searchValue);
        }

        // Abstract Class Implementation
        Console.WriteLine("\n--- Abstract Class Implementation ---");
        ProductViewer viewerAbstract = new ViewProductsAbstract();
        viewerAbstract.DisplayProducts(products);

        Console.WriteLine("Enter search type ('name' or 'category'):");
        searchType = Console.ReadLine();
        Console.WriteLine($"Enter the {searchType} to search:");
        searchValue = Console.ReadLine();

        ProductSearcher searcherAbstract = new SearchProductsAbstract();
        if (searchType.Equals("name", StringComparison.OrdinalIgnoreCase))
        {
            searcherAbstract.SearchByName(products, searchValue);
        }
        else if (searchType.Equals("category", StringComparison.OrdinalIgnoreCase))
        {
            searcherAbstract.SearchByCategory(products, searchValue);
        }

        // Virtual Method Implementation
        Console.WriteLine("\n--- Virtual Method Implementation ---");
        ProductViewerVirtual viewerVirtual = new ProductViewerVirtual();
        viewerVirtual.DisplayProducts(products);

        Console.WriteLine("Enter search type ('name' or 'category'):");
        searchType = Console.ReadLine();
        Console.WriteLine($"Enter the {searchType} to search:");
        searchValue = Console.ReadLine();

        ProductSearcherVirtual searcherVirtual = new ProductSearcherVirtual();
        if (searchType.Equals("name", StringComparison.OrdinalIgnoreCase))
        {
            searcherVirtual.SearchByName(products, searchValue);
        }
        else if (searchType.Equals("category", StringComparison.OrdinalIgnoreCase))
        {
            searcherVirtual.SearchByCategory(products, searchValue);
        }
    }
}
