using System;
using System.Collections.Generic;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } // "1" for Seller, "2" for Buyer
    public string SubscriptionType { get; set; } // "Basic", "Premium", or null
    public string ContactNumber { get; set; } // Seller's contact number

    public User(string username, string password, string role, string contactNumber = null)
    {
        Username = username;
        Password = password;
        Role = role;
        SubscriptionType = null;
        ContactNumber = contactNumber; // Seller's contact number
    }
}

public class Product
{
    private static int _nextId = 1;
    public int Id { get; private set; }
    public string Name { get; set; }
    public string Model { get; set; }
    public string Category { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal DiscountedPrice { get; set; }
    public string Description { get; set; }
    public string Owner { get; set; }
    public List<string> Reviews { get; set; } = new List<string>();
    public double Rating { get; set; } = 0;
    public int Quantity { get; set; }
    public bool IsSoldOut { get; set; }
    public Product(string name, string model, string category, decimal originalPrice, decimal discountedPrice, string description, string owner, int quantity)
    {
        Id = _nextId++;
        Name = name;
        Model = model;
        Category = category;
        OriginalPrice = originalPrice;
        DiscountedPrice = discountedPrice;
        Description = description;
        Owner = owner;
        Quantity = quantity;
        IsSoldOut = quantity <= 0; // Mark as sold out if quantity is zero or less
    }
}

// Order Class
public class Order
{
    private static int _nextId = 1;
    public int OrderId { get; private set; }
    public string BuyerUsername { get; set; }
    public int ProductId { get; set; }
    public string Status { get; set; } // "Placed", "Shipped", "Delivered", "Cancelled"
    public DateTime OrderDate { get; set; }

    public Order(string buyerUsername, int productId)
    {
        OrderId = _nextId++;
        BuyerUsername = buyerUsername;
        ProductId = productId;
        Status = "Placed";
        OrderDate = DateTime.Now;
    }
}

// Seller Feedback on Software
public class SellerFeedback
{
    public string SellerUsername { get; set; }
    public string Feedback { get; set; }
    public int Rating { get; set; } // Rating out of 5

    public SellerFeedback(string sellerUsername, string feedback, int rating)
    {
        SellerUsername = sellerUsername;
        Feedback = feedback;
        Rating = rating;
    }
}

// Buyer Feedback on Products
public class BuyerFeedback
{
    public string BuyerUsername { get; set; }
    public int ProductId { get; set; }
    public string Feedback { get; set; }
    public int Rating { get; set; } // Rating out of 5

    public BuyerFeedback(string buyerUsername, int productId, string feedback, int rating)
    {
        BuyerUsername = buyerUsername;
        ProductId = productId;
        Feedback = feedback;
        Rating = rating;
    }
}

public interface ISellerAction
{
    void SExecute();
}

public interface IBuyerAction
{
    void BExecute(); // Unique method name for Buyer
}

public interface IAdminAction
{
    void AExecute(); // Unique method name for Admin
}

public interface IOrderAction
{
    void Order(); // Renamed from Execute to Order
}

public interface ISearchAction
{
    void Search(List<Product> products); // Renamed from Execute to Search
}

public class ViewProducts : ISellerAction, IBuyerAction, IAdminAction
{
    private List<Product> products;
    private List<User> users;

    public ViewProducts(List<Product> products, List<User> users)
    {
        this.products = products;
        this.users = users;
    }

    public void SExecute() => DisplayAvailableProducts();
    public void BExecute() => DisplayAvailableProducts();
    public void AExecute() => DisplayAvailableProducts();

    private void DisplayAvailableProducts()
    {
        Console.WriteLine("\nAvailable Products:");
        foreach (var product in products)
        {
            if (!product.IsSoldOut)
            {
                User seller = null;
                foreach (var user in users)
                {
                    if (user.Username == product.Owner)
                    {
                        seller = user;
                        break;
                    }
                }
                DisplayProductDetails(product, seller);
            }
            else
            {
                Console.WriteLine($"PId:{product.Id}  ->  Product Name: {product.Name} (Sold Out)");
            }
        }
    }

    private void DisplayProductDetails(Product product, User seller)
    {
        Console.WriteLine($"PId:{product.Id}  ->  Product Name: {product.Name} , Model:{product.Model} , Category: {product.Category} , {product.DiscountedPrice}rs ,  Sold By: {product.Owner}, Contact: {seller?.ContactNumber}");
        Console.WriteLine($"Rating: {product.Rating:F1}, Reviews: {product.Reviews.Count}, Quantity: {product.Quantity}");
        DisplayProductReviews(product);
    }

    private void DisplayProductReviews(Product product)
    {
        foreach (var review in product.Reviews)
        {
            Console.WriteLine($"- {review}");
        }
    }
}

public class AddProduct : ISellerAction
{
    private List<Product> products;
    private string sellerUsername;

    public AddProduct(List<Product> products, string sellerUsername)
    {
        this.products = products;
        this.sellerUsername = sellerUsername;
    }

    public void SExecute()
    {
        var product = GetProductDetailsFromUser();
        products.Add(product);
        Console.WriteLine("\nProduct added successfully!");
    }

    private Product GetProductDetailsFromUser()
    {
        Console.Write("Enter Product Name: ");
        string name = Console.ReadLine();
        Console.Write("Enter Model: ");
        string model = Console.ReadLine();
        Console.Write("Enter Category: ");
        string category = Console.ReadLine();
        Console.Write("Enter Original Price (INR): ");
        decimal originalPrice = Convert.ToDecimal(Console.ReadLine());
        Console.Write("Enter Discounted Price (INR): ");
        decimal discountedPrice = Convert.ToDecimal(Console.ReadLine());
        Console.Write("Enter Description: ");
        string description = Console.ReadLine();
        Console.Write("Enter Quantity: ");
        int quantity = Convert.ToInt32(Console.ReadLine());

        return new Product(name, model, category, originalPrice, discountedPrice, description, sellerUsername, quantity);
    }
}

// Update Product Class
public class UpdateProduct : ISellerAction
{
    private List<Product> products;

    public UpdateProduct(List<Product> products)
    {
        this.products = products;
    }

    public void SExecute()
    {
        Console.Write("Enter Product ID to Update: ");
        int productId = Convert.ToInt32(Console.ReadLine());
        Product product = null;
        foreach (var p in products)
        {
            if (p.Id == productId)
            {
                product = p;
                break;
            }
        }
        if (product != null)
        {
            UpdateProductDetails(product);
            Console.WriteLine("Product updated successfully!");
        }
        else
        {
            Console.WriteLine("\nProduct not found!");
        }
    }

    private void UpdateProductDetails(Product product)
    {
        Console.Write("Enter New Product Name: ");
        product.Name = Console.ReadLine();
        Console.Write("Enter New Model: ");
        product.Model = Console.ReadLine();
        Console.Write("Enter New Category: ");
        product.Category = Console.ReadLine();
        Console.Write("Enter New Discounted Price: ");
        product.DiscountedPrice = Convert.ToDecimal(Console.ReadLine());
        Console.Write("Enter New Quantity: ");
        product.Quantity = Convert.ToInt32(Console.ReadLine());
        product.IsSoldOut = product.Quantity <= 0; // Update sold out status
    }
}

// Delete Product Class
public class DeleteProduct : ISellerAction
{
    private List<Product> products;

    public DeleteProduct(List<Product> products)
    {
        this.products = products;
    }

    public void SExecute()
    {
        Console.Write("Enter Product ID to Delete: ");
        int productId = Convert.ToInt32(Console.ReadLine());

        Product product = null;
        foreach (var p in products)
        {
            if (p.Id == productId)
            {
                product = p;
                break;
            }
        }
        if (product != null)
        {
            products.Remove(product);
            Console.WriteLine("\nProduct deleted successfully!");
        }
        else
        {
            Console.WriteLine("\nProduct not found!");
        }
    }
}

// Place Order Class
public class PlaceOrder : IOrderAction
{
    private List<Order> orders;
    private List<Product> products;
    private string buyerUsername;

    public PlaceOrder(List<Order> orders, List<Product> products, string buyerUsername)
    {
        this.orders = orders;
        this.products = products;
        this.buyerUsername = buyerUsername;
    }

    public void Order()
    {
        Console.Write("Enter Product ID to Order: ");
        int productId = Convert.ToInt32(Console.ReadLine());
        Product product = null;
        foreach (var p in products)
        {
            if (p.Id == productId)
            {
                product = p;
                break;
            }
        }
        if (product != null)
        {
            if (product.Quantity > 0)
            {
                orders.Add(new Order(buyerUsername, productId));
                product.Quantity--; // Decrease quantity
                if (product.Quantity == 0)
                {
                    product.IsSoldOut = true; // Mark as sold out
                }
                Console.WriteLine("Order placed successfully!");
            }
            else
            {
                Console.WriteLine("Product is sold out!");
            }
        }
        else
        {
            Console.WriteLine("Invalid Product ID!");
        }
    }
}

// Cancel Order Class
public class CancelOrder : IOrderAction
{
    private List<Order> orders;
    private List<Product> products;
    private string buyerUsername;

    public CancelOrder(List<Order> orders, List<Product> products, string buyerUsername)
    {
        this.orders = orders;
        this.products = products;
        this.buyerUsername = buyerUsername;
    }

    public void Order()
    {
        Console.Write("Enter Order ID to Cancel: ");
        int orderId = Convert.ToInt32(Console.ReadLine());
        Order order = null;
        foreach (var o in orders)
        {
            if (o.OrderId == orderId && o.BuyerUsername == buyerUsername)
            {
                order = o;
                break;
            }
        }
        if (order != null)
        {
            order.Status = "Cancelled";
            Product product = null;
            foreach (var p in products)
            {
                if (p.Id == order.ProductId)
                {
                    product = p;
                    break;
                }
            }
            if (product != null)
            {
                product.Quantity++; // Increase quantity
                product.IsSoldOut = false; // Mark as available
            }
            Console.WriteLine("Order cancelled successfully!");
        }
        else
        {
            Console.WriteLine("Invalid Order ID or you do not have permission to cancel this order!");
        }
    }
}

// Track Order Class
public class TrackOrder : IOrderAction
{
    private List<Order> orders;
    private string buyerUsername;

    public TrackOrder(List<Order> orders, string buyerUsername)
    {
        this.orders = orders;
        this.buyerUsername = buyerUsername;
    }

    public void Order()
    {
        Console.Write("Enter Order ID to Track: ");
        int orderId = Convert.ToInt32(Console.ReadLine());
        Order order = null;
        foreach (var o in orders)
        {
            if (o.OrderId == orderId && o.BuyerUsername == buyerUsername)
            {
                order = o;
                break;
            }
        }
        if (order != null)
        {
            Console.WriteLine($"Order ID: {order.OrderId}, Status: {order.Status}, Order Date: {order.OrderDate}");
        }
        else
        {
            Console.WriteLine("Invalid Order ID or you do not have permission to track this order!");
        }
    }
}

// View Order History Class
public class ViewOrderHistory : IOrderAction
{
    private List<Order> orders;
    private string buyerUsername;

    public ViewOrderHistory(List<Order> orders, string buyerUsername)
    {
        this.orders = orders;
        this.buyerUsername = buyerUsername;
    }

    public void Order()
    {
        List<Order> buyerOrders = new List<Order>();
        foreach (var order in orders)
        {
            if (order.BuyerUsername == buyerUsername)
            {
                buyerOrders.Add(order);
            }
        }
        if (buyerOrders.Count > 0)
        {
            Console.WriteLine("\nOrder History:");
            foreach (var order in buyerOrders)
            {
                Console.WriteLine($"Order ID: {order.OrderId}, Product ID: {order.ProductId}, Status: {order.Status}, Order Date: {order.OrderDate}");
            }
        }
        else
        {
            Console.WriteLine("\nNo orders found!");
        }
    }
}

// Display Order Menu Class
public class DisplayOrderMenu
{
    public void Display()
    {
        Console.WriteLine("\nOrder Menu:");
        Console.WriteLine("1. Place Order");
        Console.WriteLine("2. Cancel Order");
        Console.WriteLine("3. Track Order");
        Console.WriteLine("4. View Order History");
        Console.WriteLine("5. Back to Main Menu");
        Console.Write("Enter your choice: ");
    }
}

// Order Actions Class (Coordinator)
public class OrderActions : IBuyerAction
{
    private List<IOrderAction> orderActions;
    private DisplayOrderMenu displayOrderMenu;

    public OrderActions(List<IOrderAction> orderActions, DisplayOrderMenu displayOrderMenu)
    {
        this.orderActions = orderActions;
        this.displayOrderMenu = displayOrderMenu;
    }

    public void BExecute()
    {
        string choice;
        do
        {
            displayOrderMenu.Display();
            choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    orderActions[0].Order(); // PlaceOrder
                    break;
                case "2":
                    orderActions[1].Order(); // CancelOrder
                    break;
                case "3":
                    orderActions[2].Order(); // TrackOrder
                    break;
                case "4":
                    orderActions[3].Order(); // ViewOrderHistory
                    break;
                case "5":
                    Console.WriteLine("Returning to main menu...");
                    break;
                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }
        } while (choice != "5");
    }
}

// Feedback Service Class
public class FeedbackService
{
    private List<BuyerFeedback> buyerFeedbacks;
    private List<SellerFeedback> sellerFeedbacks;

    public FeedbackService(List<BuyerFeedback> buyerFeedbacks, List<SellerFeedback> sellerFeedbacks)
    {
        this.buyerFeedbacks = buyerFeedbacks;
        this.sellerFeedbacks = sellerFeedbacks;
    }

    public void SubmitBuyerFeedback(string buyerUsername, int productId, string feedback, int rating)
    {
        buyerFeedbacks.Add(new BuyerFeedback(buyerUsername, productId, feedback, rating));
        Console.WriteLine("\nFeedback submitted successfully!");
    }

    public void SubmitSellerFeedback(string sellerUsername, string feedback, int rating)
    {
        sellerFeedbacks.Add(new SellerFeedback(sellerUsername, feedback, rating));
        Console.WriteLine("\nFeedback submitted successfully!");
    }

    public void DisplayAverageRating(string username, string role)
    {
        if (role == "1")
        {
            List<SellerFeedback> sellerFeedback = new List<SellerFeedback>();
            foreach (var feedback in sellerFeedbacks)
            {
                if (feedback.SellerUsername == username)
                {
                    sellerFeedback.Add(feedback);
                }
            }
            if (sellerFeedback.Count > 0)
            {
                double totalRating = 0;
                foreach (var feedback in sellerFeedback)
                {
                    totalRating += feedback.Rating;
                }
                double averageRating = totalRating / sellerFeedback.Count;
                Console.WriteLine($"Average Software Rating: {averageRating:F1} stars");
            }
            else
            {
                Console.WriteLine("No feedback available yet.");
            }
        }
        else if (role == "2") // Buyer
        {
            List<BuyerFeedback> buyerFeedback = new List<BuyerFeedback>();
            foreach (var feedback in buyerFeedbacks)
            {
                if (feedback.BuyerUsername == username)
                {
                    buyerFeedback.Add(feedback);
                }
            }
            if (buyerFeedback.Count > 0)
            {
                double totalRating = 0;
                foreach (var feedback in buyerFeedback)
                {
                    totalRating += feedback.Rating;
                }
                double averageRating = totalRating / buyerFeedback.Count;
                Console.WriteLine($"Average Product Rating: {averageRating:F1} stars");
            }
            else
            {
                Console.WriteLine("No feedback available yet.");
            }
        }
    }

    // New method to display product feedback and ratings
    public void DisplayProductFeedbackAndRatings(List<Product> products)
    {
        Console.WriteLine("\nProduct Feedback and Ratings:");
        foreach (var product in products)
        {
            Console.WriteLine($"Product: {product.Name}, Rating: {product.Rating:F1}, Reviews: {product.Reviews.Count}, Quantity: {product.Quantity}");
            foreach (var review in product.Reviews)
            {
                Console.WriteLine($"- {review}");
            }
        }
    }

    // New method to display seller feedback on software
    public void DisplaySellerFeedbackOnSoftware()
    {
        Console.WriteLine("\nSeller Feedback on Software:");
        foreach (var feedback in sellerFeedbacks)
        {
            Console.WriteLine($"Seller: {feedback.SellerUsername}, Feedback: {feedback.Feedback}, Rating: {feedback.Rating}");
        }

        if (sellerFeedbacks.Count > 0)
        {
            double totalRating = 0;
            foreach (var feedback in sellerFeedbacks)
            {
                totalRating += feedback.Rating;
            }
            double averageRating = totalRating / sellerFeedbacks.Count;
            Console.WriteLine($"\nAverage Software Rating: {averageRating:F1} stars");
        }
        else
        {
            Console.WriteLine("No feedback available yet.");
        }
    }
}

// Give Buyer Feedback Class
public class GiveBuyerFeedback : IBuyerAction
{
    private FeedbackService feedbackService;
    private List<Product> products;
    private List<Order> orders;
    private string buyerUsername;

    public GiveBuyerFeedback(FeedbackService feedbackService, List<Product> products, List<Order> orders, string buyerUsername)
    {
        this.feedbackService = feedbackService;
        this.products = products;
        this.orders = orders;
        this.buyerUsername = buyerUsername;
    }

    public void BExecute()
    {
        Console.Write("Enter Product ID to Review: ");
        int productId = Convert.ToInt32(Console.ReadLine());

        if (!HasPurchasedProduct(productId))
        {
            Console.WriteLine("\nYou can only review purchased products!");
            return;
        }

        int rating = GetRatingFromUser();
        if (rating < 1 || rating > 5)
        {
            Console.WriteLine("\nInvalid rating! Please enter a value between 1 and 5.");
            return;
        }

        Console.Write("Enter Your Review: ");
        string feedback = Console.ReadLine();

        feedbackService.SubmitBuyerFeedback(buyerUsername, productId, feedback, rating);
        feedbackService.DisplayAverageRating(buyerUsername, "2");
    }

    private bool HasPurchasedProduct(int productId)
    {
        foreach (var order in orders)
        {
            if (order.BuyerUsername == buyerUsername && order.ProductId == productId)
            {
                return true;
            }
        }
        return false;
    }

    private int GetRatingFromUser()
    {
        Console.Write("Enter Your Rating (1 to 5 stars): ");
        return Convert.ToInt32(Console.ReadLine());
    }
}

// Give Seller Feedback Class
public class GiveSellerFeedback : ISellerAction
{
    private FeedbackService feedbackService;
    private string sellerUsername;

    public GiveSellerFeedback(FeedbackService feedbackService, string sellerUsername)
    {
        this.feedbackService = feedbackService;
        this.sellerUsername = sellerUsername;
    }

    public void SExecute()
    {
        Console.Write("Enter Your Feedback on the Software: ");
        string feedback = Console.ReadLine();

        int rating = GetRatingFromUser();
        if (rating < 1 || rating > 5)
        {
            Console.WriteLine("\nInvalid rating! Please enter a value between 1 and 5.");
            return;
        }

        feedbackService.SubmitSellerFeedback(sellerUsername, feedback, rating);
        feedbackService.DisplayAverageRating(sellerUsername, "1");
    }

    private int GetRatingFromUser()
    {
        Console.Write("Enter Your Rating (1 to 5 stars): ");
        return Convert.ToInt32(Console.ReadLine());
    }
}

// Payment Service Class
public class PaymentService
{
    private List<User> users;

    public PaymentService(List<User> users) //constructure injectio
    {
        this.users = users;
    }

    public void ProcessPayment(User user)
    {
        var subscriptionDetails = GetSubscriptionDetails();
        if (subscriptionDetails == null) return;

        decimal amount = subscriptionDetails.Amount;
        user.SubscriptionType = subscriptionDetails.PlanName;

        Console.WriteLine($"\nAmount to Pay: ₹{amount}");
        ProcessPaymentMethod(amount);
        Console.WriteLine($"\nSubscription Activated: {user.SubscriptionType} Account");
    }

    private SubscriptionDetails GetSubscriptionDetails()
    {
        Console.WriteLine("\nSubscription Plans:");
        Console.WriteLine("1. Basic Account - ₹500/month");
        Console.WriteLine("2. Premium Account - ₹1000/month");
        Console.Write("Choose a subscription plan (1 or 2): ");
        string planChoice = Console.ReadLine();

        if (planChoice != "1" && planChoice != "2")
        {
            Console.WriteLine("Invalid choice!");
            return null;
        }

        return planChoice == "1"
            ? new SubscriptionDetails { PlanName = "Basic", Amount = 500 }
            : new SubscriptionDetails { PlanName = "Premium", Amount = 1000 };
    }

    private void ProcessPaymentMethod(decimal amount)
    {
        Console.WriteLine("Select Payment Method:");
        Console.WriteLine("1. GPay");
        Console.WriteLine("2. Credit Card");
        Console.Write("Enter your choice (1 or 2): ");
        string paymentChoice = Console.ReadLine();

        if (paymentChoice == "1")
        {
            Console.WriteLine("\nRedirecting to GPay...");
            Console.WriteLine("Payment successful via GPay!");
        }
        else if (paymentChoice == "2")
        {
            ProcessCreditCardPayment();
        }
        else
        {
            Console.WriteLine("Invalid payment method!");
        }
    }

    private void ProcessCreditCardPayment()
    {
        Console.WriteLine("\nEnter Credit Card Details:");
        Console.Write("Card Number: ");
        string cardNumber = Console.ReadLine();
        Console.Write("Expiry Date (MM/YY): ");
        string expiryDate = Console.ReadLine();
        Console.Write("CVV: ");
        string cvv = Console.ReadLine();
        Console.WriteLine("Payment successful via Credit Card!");
    }

    private class SubscriptionDetails
    {
        public string PlanName { get; set; }
        public decimal Amount { get; set; }
    }
}

// Payment Module Class
public class PaymentModule : IBuyerAction, ISellerAction
{
    private PaymentService paymentService;
    private User user;

    public PaymentModule(PaymentService paymentService, User user)
    {
        this.paymentService = paymentService;
        this.user = user;
    }

    public void BExecute() => paymentService.ProcessPayment(user);
    public void SExecute() => paymentService.ProcessPayment(user);
}


//Display products module
public interface IDisplayProducts
{
    void Display(List<Product> products);
}

public class DisplayProducts : IDisplayProducts
{
    public void Display(List<Product> products)
    {
        foreach (var product in products)
        {
            Console.WriteLine($"PId: {product.Id}  ->  Name: {product.Name}, Model: {product.Model}, Rs.{product.DiscountedPrice}");
        }
    }
}

// Dependency injection using constructor
public class ProductSearcher
{
    private readonly ISearchAction _searchProduct;

    public ProductSearcher(ISearchAction searchProduct)
    {
        _searchProduct = searchProduct;
    }

    public void PerformSearch(List<Product> products)
    {
        _searchProduct.Search(products);
    }
}


// Search By Name Class
public class SearchByName : ISearchAction
{
    public void Search(List<Product> products)
    {
        Console.Write("Enter Product Name: ");
        string name = Console.ReadLine();
        List<Product> results = new List<Product>();
        foreach (var product in products)
        {
            if (product.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            {
                results.Add(product);
            }
        }
        DisplaySearchResults(results);
    }

    private void DisplaySearchResults(List<Product> results)
    {
        if (results.Count > 0)
        {
            Console.WriteLine("\nSearch Results:");
            foreach (var product in results)
            {
                if (!product.IsSoldOut)
                {
                    Console.WriteLine($"PId:{product.Id}  ->  Name: {product.Name}, Model: {product.Model}, Category: {product.Category}, Rs{product.DiscountedPrice},  Sold by: {product.Owner}");
                    Console.WriteLine($"Rating: {product.Rating:F1}, Reviews: {product.Reviews.Count}, Quantity: {product.Quantity}");
                    foreach (var review in product.Reviews)
                    {
                        Console.WriteLine($"- {review}");
                    }
                }
                else
                {
                    Console.WriteLine($"PId:{product.Id}  ->  Product Name: {product.Name} (Sold Out)");
                }
            }
        }
        else
        {
            Console.WriteLine("\nNo products found matching your criteria.");
        }
    }
}



// Search By Category Class
public class SearchByCategory : ISearchAction
{
    public void Search(List<Product> products)
    {
        Console.Write("Enter Product Category: ");
        string category = Console.ReadLine();
        List<Product> results = new List<Product>();
        foreach (var product in products)
        {
            if (product.Category.Contains(category, StringComparison.OrdinalIgnoreCase))
            {
                results.Add(product);
            }
        }
        DisplaySearchResults(results);
    }

    private void DisplaySearchResults(List<Product> results)
    {
        if (results.Count > 0)
        {
            Console.WriteLine("\nSearch Results:");
            foreach (var product in results)
            {
                if (!product.IsSoldOut)
                {
                    Console.WriteLine($"PId:{product.Id}  ->  Name: {product.Name}, Model: {product.Model}, Category: {product.Category}, Rs{product.DiscountedPrice},  Sold by: {product.Owner}");
                    Console.WriteLine($"Rating: {product.Rating:F1}, Reviews: {product.Reviews.Count}, Quantity: {product.Quantity}");
                    foreach (var review in product.Reviews)
                    {
                        Console.WriteLine($"- {review}");
                    }
                }
                else
                {
                    Console.WriteLine($"PId:{product.Id}  ->  Product Name: {product.Name} (Sold Out)");
                }
            }
        }
        else
        {
            Console.WriteLine("\nNo products found matching your criteria.");
        }
    }
}

// Search By Price Class
public class SearchByPrice : ISearchAction
{
    public void Search(List<Product> products)
    {
        Console.Write("Enter Maximum Price (INR): ");
        decimal maxPrice = Convert.ToDecimal(Console.ReadLine());
        List<Product> results = new List<Product>();
        foreach (var product in products)
        {
            if (product.DiscountedPrice <= maxPrice)
            {
                results.Add(product);
            }
        }
        DisplaySearchResults(results);
    }

    private void DisplaySearchResults(List<Product> results)
    {
        if (results.Count > 0)
        {
            Console.WriteLine("\nSearch Results:");
            foreach (var product in results)
            {
                if (!product.IsSoldOut)
                {
                    Console.WriteLine($"PId:{product.Id}  ->  Name: {product.Name}, Model: {product.Model}, Category: {product.Category}, Rs{product.DiscountedPrice},  Sold by: {product.Owner}");
                    Console.WriteLine($"Rating: {product.Rating:F1}, Reviews: {product.Reviews.Count}, Quantity: {product.Quantity}");
                    foreach (var review in product.Reviews)
                    {
                        Console.WriteLine($"- {review}");
                    }
                }
                else
                {
                    Console.WriteLine($"PId:{product.Id}  ->  Product Name: {product.Name} (Sold Out)");
                }
            }
        }
        else
        {
            Console.WriteLine("\nNo products found matching your criteria.");
        }
    }
}

// Register User Class
public class RegisterUser : IAdminAction
{
    private List<User> users;

    public RegisterUser(List<User> users)
    {
        this.users = users;
    }

    public void AExecute()
    {
        Console.Write("Enter Username: ");
        string username = Console.ReadLine();
        Console.Write("Enter Password: ");
        string password = Console.ReadLine();
        Console.Write("Enter Role (1.Seller  2.Buyer): ");
        string role = Console.ReadLine();

        string contactNumber = null;
        if (role == "1") // Seller
        {
            Console.Write("Enter Contact Number: ");
            contactNumber = Console.ReadLine();
        }

        users.Add(new User(username, password, role, contactNumber));
        Console.WriteLine("Registration successful!");
    }
}

// Admin Menu Class
public class AdminMenu : IAdminAction
{
    private List<User> users;
    private List<Product> products;
    private FeedbackService feedbackService;

    public AdminMenu(List<User> users, List<Product> products, FeedbackService feedbackService)
    {
        this.users = users;
        this.products = products;
        this.feedbackService = feedbackService;
    }

    public void AExecute()
    {
        string choice;
        do
        {
            Console.WriteLine("\nAdmin Menu:");
            Console.WriteLine("1. View User Details");
            Console.WriteLine("2. View Total Product Count");
            Console.WriteLine("3. View Product Feedback and Ratings");
            Console.WriteLine("4. View Seller Feedback on Software");
            Console.WriteLine("5. View All Products");
            Console.WriteLine("6. Logout");
            Console.Write("Enter choice: ");
            choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewUserDetails();
                    break;
                case "2":
                    ViewTotalProductCount();
                    break;
                case "3":
                    feedbackService.DisplayProductFeedbackAndRatings(products);
                    break;
                case "4":
                    feedbackService.DisplaySellerFeedbackOnSoftware();
                    break;
                case "5":
                    new ViewProducts(products, users).AExecute();
                    break;
                case "6":
                    Console.WriteLine("Logged out successfully!");
                    break;
                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }
        } while (choice != "6");
    }

    private void ViewUserDetails()
    {
        Console.WriteLine("\nUser Details:");
        Console.WriteLine("Sellers:");
        foreach (var user in users)
        {
            if (user.Role == "1")
            {
                Console.WriteLine($"Username: {user.Username}, Contact: {user.ContactNumber}, Subscription: {user.SubscriptionType ?? "None"}");
            }
        }
        Console.WriteLine("\nBuyers:");
        foreach (var user in users)
        {
            if (user.Role == "2")
            {
                Console.WriteLine($"Username: {user.Username}, Subscription: {user.SubscriptionType ?? "None"}");
            }
        }
    }

    private void ViewTotalProductCount()
    {
        Console.WriteLine($"\nTotal Products: {products.Count}");
    }
}

// User Menu Class
public class UserMenu
{
    private User user;
    private List<Product> products;
    private List<Order> orders;
    private FeedbackService feedbackService;
    private PaymentService paymentService;
    private List<User> users;

    public UserMenu(User user, List<Product> products, List<Order> orders, List<User> users, FeedbackService feedbackService, PaymentService paymentService)
    {
        this.user = user;
        this.products = products;
        this.orders = orders;
        this.feedbackService = feedbackService;
        this.paymentService = paymentService;
        this.users = users;
    }

    public void Execute()
    {
        string choice;

        if (user.Role == "1") // Seller
        {
            do
            {
                Console.WriteLine("\nSeller Menu:");
                Console.WriteLine("1. View Products");
                Console.WriteLine("2. Add Product");
                Console.WriteLine("3. Update Product");
                Console.WriteLine("4. Delete Product");
                Console.WriteLine("5. Give Feedback on Software");
                Console.WriteLine("6. Subscription Payment");
                Console.WriteLine("7. Logout");
                Console.Write("Enter your choice: ");
                choice = Console.ReadLine();

                if (choice == "1")
                    new ViewProducts(products, users).SExecute();
                else if (choice == "2")
                    new AddProduct(products, user.Username).SExecute();
                else if (choice == "3")
                    new UpdateProduct(products).SExecute();
                else if (choice == "4")
                    new DeleteProduct(products).SExecute();
                else if (choice == "5")
                    new GiveSellerFeedback(feedbackService, user.Username).SExecute();
                else if (choice == "6")
                    new PaymentModule(paymentService, user).SExecute();
                else if (choice == "7")
                    Console.WriteLine("Logged out successfully!");
                else
                    Console.WriteLine("Invalid choice, please try again.");

            } while (choice != "7");
        }
        else if (user.Role == "2")
        {
            do
            {
                Console.WriteLine("\nBuyer Menu:");
                Console.WriteLine("1. View Products");
                Console.WriteLine("2. Place Order");
                Console.WriteLine("3. Give Feedback");
                Console.WriteLine("4. Subscription Payment");
                Console.WriteLine("5. Search Products");
                Console.WriteLine("6. Logout");
                Console.Write("Enter your choice: ");
                choice = Console.ReadLine();

                if (choice == "1")
                    new DisplayProducts().Display(products);
                else if (choice == "2")
                {
                    var orderActions = new List<IOrderAction>
                    {
                        new PlaceOrder(orders, products, user.Username),
                        new CancelOrder(orders, products, user.Username),
                        new TrackOrder(orders, user.Username),
                        new ViewOrderHistory(orders, user.Username)
                    };
                    var displayOrderMenu = new DisplayOrderMenu();
                    new OrderActions(orderActions, displayOrderMenu).BExecute();
                }
                else if (choice == "3")
                    new GiveBuyerFeedback(feedbackService, products, orders, user.Username).BExecute();
                else if (choice == "4")
                    new PaymentModule(paymentService, user).BExecute();
                else if (choice == "5")
                {
                    Console.WriteLine("Select search type:");
                    Console.WriteLine("1. Name");
                    Console.WriteLine("2. Category");
                    Console.WriteLine("3. Price");
                    Console.Write("Enter your choice: ");
                    string searchType = Console.ReadLine();

                    ISearchAction searcher;
                    switch (searchType)
                    {
                        case "1":
                            searcher = new SearchByName();
                            break;
                        case "2":
                            searcher = new SearchByCategory();
                            break;
                        case "3":
                            searcher = new SearchByPrice();
                            break;
                        default:
                            Console.WriteLine("Invalid search type.");
                            return;
                    }

                    ProductSearcher searchHandler = new ProductSearcher(searcher);
                    searchHandler.PerformSearch(products);
                }
                else if (choice == "6")
                    Console.WriteLine("Logged out successfully!");
                else
                    Console.WriteLine("Invalid choice, please try again.");

            } while (choice != "6");
        }
        else
        {
            Console.WriteLine("Invalid role.");
        }
    }
}

// Main Program
class Program
{
    static List<Order> orders = new List<Order>();
    static List<User> users = new List<User>();
    static List<Product> products = new List<Product>();
    static List<SellerFeedback> sellerFeedbacks = new List<SellerFeedback>();
    static List<BuyerFeedback> buyerFeedbacks = new List<BuyerFeedback>();

    static void Main()
    {
        string adminUsername = "admin";
        string adminPassword = "admin123";

        var feedbackService = new FeedbackService(buyerFeedbacks, sellerFeedbacks);
        var paymentService = new PaymentService(users);

        while (true)
        {
            Console.WriteLine("\nWelcome to the Online Reselling Platform");
            Console.WriteLine("1. Register\n2. Login\n3. Exit\n");
            Console.Write("Enter choice: ");
            string choice = Console.ReadLine();

            if (choice == "1")
                new RegisterUser(users).AExecute();
            else if (choice == "2")
            {
                Console.Write("Are you an 1.Seller, 2.Buyer, 3.Admin?: ");
                string userType = Console.ReadLine();

                Console.Write("Enter Username: ");
                string username = Console.ReadLine();
                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                if (userType == "3") // Admin
                {
                    if (username == adminUsername && password == adminPassword)
                        new AdminMenu(users, products, feedbackService).AExecute();
                    else
                        Console.WriteLine("Invalid Admin credentials!");
                }
                else if (userType == "1" || userType == "2") // Seller (1) or Buyer (2)
                {
                    string mappedRole = userType == "1" ? "1" : "2";
                    User user = null;
                    foreach (var u in users)
                    {
                        if (u.Username == username && u.Password == password && u.Role == mappedRole)
                        {
                            user = u;
                            break;
                        }
                    }
                    if (user != null)
                        new UserMenu(user, products, orders, users,feedbackService, paymentService).Execute();
                    else
                        Console.WriteLine("\nInvalid credentials or role! Try again.");
                }
                else
                {
                    Console.WriteLine("\nInvalid choice! Try again.");
                }
            }
            else if (choice == "3")
                return;
            else
                Console.WriteLine("\nInvalid choice! Try again.");


        }
        Console.ReadKey();
    }
}
