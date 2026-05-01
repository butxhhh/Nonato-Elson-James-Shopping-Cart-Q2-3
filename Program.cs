using System;
using System.Text;   // Added for Peso Sign UTF-8 Support

class Product
{
    public int Id;
    public string Name;
    public string Category;
    public double Price;
    public int RemainingStock;

    public Product(int id, string name, string category, double price, int stock)
    {
        Id = id;
        Name = name;
        Category = category;
        Price = price;
        RemainingStock = stock;
    }

    public void DisplayProduct()
    {
        Console.WriteLine($"[{Id}] {Name} | {Category} | ₱{Price} | Stock: {RemainingStock}");
    }
}

// Cart Item class
class CartItem
{
    public Product Product;
    public int Quantity;
    public double SubTotal;
}

class Program
{
    static CartItem[] cart = new CartItem[20];
    static int cartCount = 0;

    static string[] orderHistory = new string[50];
    static int historyCount = 0;

    static int receiptNo = 1;

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;   // Added for Peso Sign

        Product[] store = new Product[]
        {
            new Product(1,"Burger Patties","Food",120,23),
            new Product(2,"Crab Sticks","Food",99,26),
            new Product(3,"Chicken Wings","Food",109,35),
            new Product(4,"White T-Shirt","Clothing",299,26),
            new Product(5,"Black T-Shirt","Clothing",299,27),
            new Product(6,"Cargo Pants","Clothing",799,17),
            new Product(7,"Mouse Pad","Electronics",299,15),
            new Product(8,"Mouse","Electronics",599,13),
            new Product(9,"Keyboard","Electronics",2999,13)
        };

        bool run = true;

        while (run)
        {
            Console.WriteLine("\n==== MAIN MENU ====");
            Console.WriteLine("1. View Products");
            Console.WriteLine("2. Search Product");
            Console.WriteLine("3. Filter by Category");
            Console.WriteLine("4. Cart Menu");
            Console.WriteLine("5. Order History");
            Console.WriteLine("6. Exit");

            Console.Write("Choose: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowProducts(store);
                    AddToCart(store);
                    break;

                case "2":
                    SearchProduct(store);
                    break;

                case "3":
                    FilterCategory(store);
                    break;

                case "4":
                    CartMenu(store);
                    break;

                case "5":
                    ShowHistory();
                    break;

                case "6":
                    run = false;
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    static void ShowProducts(Product[] store)
    {
        Console.WriteLine("\n==== PRODUCT LIST ====");
        foreach (Product p in store)
            p.DisplayProduct();
    }

    static void SearchProduct(Product[] store)
    {
        Console.Write("Enter product name: ");
        string search = Console.ReadLine().ToLower();

        Console.WriteLine("\nSearch Result:");
        foreach (Product p in store)
        {
            if (p.Name.ToLower().Contains(search))
                p.DisplayProduct();
        }
    }

    static void FilterCategory(Product[] store)
    {
        Console.WriteLine("\n1. Food");
        Console.WriteLine("2. Electronics");
        Console.WriteLine("3. Clothing");
        Console.Write("Choose category: ");

        string category = "";
        string input = Console.ReadLine();

        if (input == "1") category = "Food";
        else if (input == "2") category = "Electronics";
        else if (input == "3") category = "Clothing";
        else
        {
            Console.WriteLine("Invalid category.");
            return;
        }

        Console.WriteLine($"\nProducts in {category}:");

        foreach (Product p in store)
        {
            if (p.Category == category)
                p.DisplayProduct();
        }
    }

    static void AddToCart(Product[] store)
    {
        Console.Write("Enter Product ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid input.");
            return;
        }

        Product selected = null;

        foreach (Product p in store)
        {
            if (p.Id == id)
            {
                selected = p;
                break;
            }
        }

        if (selected == null)
        {
            Console.WriteLine("Product not found.");
            return;
        }

        Console.Write("Enter Quantity: ");

        if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
        {
            Console.WriteLine("Invalid quantity.");
            return;
        }

        if (qty > selected.RemainingStock)
        {
            Console.WriteLine("Not enough stock.");
            return;
        }

        bool found = false;

        for (int i = 0; i < cartCount; i++)
        {
            if (cart[i].Product.Id == selected.Id)
            {
                cart[i].Quantity += qty;
                cart[i].SubTotal += qty * selected.Price;
                found = true;
                break;
            }
        }

        if (!found)
        {
            cart[cartCount] = new CartItem();
            cart[cartCount].Product = selected;
            cart[cartCount].Quantity = qty;
            cart[cartCount].SubTotal = qty * selected.Price;
            cartCount++;
        }

        selected.RemainingStock -= qty;

        Console.WriteLine("Added to cart.");
    }

    static void CartMenu(Product[] store)
    {
        bool menu = true;

        while (menu)
        {
            Console.WriteLine("\n==== CART MENU ====");
            Console.WriteLine("1. View Cart");
            Console.WriteLine("2. Remove Item");
            Console.WriteLine("3. Update Quantity");
            Console.WriteLine("4. Clear Cart");
            Console.WriteLine("5. Checkout");
            Console.WriteLine("6. Back");

            Console.Write("Choose: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewCart();
                    break;
                case "2":
                    RemoveItem();
                    break;
                case "3":
                    UpdateQuantity();
                    break;
                case "4":
                    ClearCart();
                    break;
                case "5":
                    Checkout();
                    break;
                case "6":
                    menu = false;
                    break;
                default:
                    Console.WriteLine("Invalid input.");
                    break;
            }
        }
    }

    static void ViewCart()
    {
        Console.WriteLine("\n==== CART ====");
        double total = 0;

        for (int i = 0; i < cartCount; i++)
        {
            Console.WriteLine($"{i + 1}. {cart[i].Product.Name} x {cart[i].Quantity} = ₱{cart[i].SubTotal}");
            total += cart[i].SubTotal;
        }

        Console.WriteLine("Total: ₱" + total);
    }

    static void RemoveItem()
    {
        ViewCart();

        Console.Write("Enter item number to remove: ");

        if (!int.TryParse(Console.ReadLine(), out int item))
        {
            Console.WriteLine("Invalid input.");
            return;
        }

        item--;

        if (item < 0 || item >= cartCount)
        {
            Console.WriteLine("Invalid item.");
            return;
        }

        for (int i = item; i < cartCount - 1; i++)
            cart[i] = cart[i + 1];

        cartCount--;

        Console.WriteLine("Item removed.");
    }

    static void UpdateQuantity()
    {
        ViewCart();

        Console.Write("Enter item number: ");
        if (!int.TryParse(Console.ReadLine(), out int item))
        {
            Console.WriteLine("Invalid.");
            return;
        }

        item--;

        if (item < 0 || item >= cartCount)
        {
            Console.WriteLine("Invalid item.");
            return;
        }

        Console.Write("Enter new quantity: ");
        if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
        {
            Console.WriteLine("Invalid qty.");
            return;
        }

        cart[item].Quantity = qty;
        cart[item].SubTotal = qty * cart[item].Product.Price;

        Console.WriteLine("Quantity updated.");
    }

    static void ClearCart()
    {
        cartCount = 0;
        Console.WriteLine("Cart cleared.");
    }

    static void Checkout()
    {
        if (cartCount == 0)
        {
            Console.WriteLine("Cart empty.");
            return;
        }

        double grandTotal = 0;

        for (int i = 0; i < cartCount; i++)
            grandTotal += cart[i].SubTotal;

        double discount = 0;

        if (grandTotal >= 5000)
            discount = grandTotal * 0.10;

        double finalTotal = grandTotal - discount;

        Console.WriteLine("\nFinal Total: ₱" + finalTotal);

        double payment;

        while (true)
        {
            Console.Write("Enter Payment: ");

            if (!double.TryParse(Console.ReadLine(), out payment))
            {
                Console.WriteLine("Payment must be numeric.");
                continue;
            }

            if (payment < finalTotal)
            {
                Console.WriteLine("Insufficient payment.");
                continue;
            }

            break;
        }

        double change = payment - finalTotal;

        Console.WriteLine("\n===== RECEIPT =====");
        Console.WriteLine("Receipt No: " + receiptNo);
        Console.WriteLine("Date: " + DateTime.Now);

        for (int i = 0; i < cartCount; i++)
        {
            Console.WriteLine($"{cart[i].Product.Name} x {cart[i].Quantity} = ₱{cart[i].SubTotal}");
        }

        Console.WriteLine("Grand Total: ₱" + grandTotal);
        Console.WriteLine("Discount: ₱" + discount);
        Console.WriteLine("Final Total: ₱" + finalTotal);
        Console.WriteLine("Payment: ₱" + payment);
        Console.WriteLine("Change: ₱" + change);

        orderHistory[historyCount] = "Receipt #" + receiptNo + " - Final Total ₱" + finalTotal;

        historyCount++;
        receiptNo++;

        cartCount = 0;
    }

    static void ShowHistory()
    {
        Console.WriteLine("\n==== ORDER HISTORY ====");

        if (historyCount == 0)
        {
            Console.WriteLine("No orders yet.");
            return;
        }

        for (int i = 0; i < historyCount; i++)
            Console.WriteLine(orderHistory[i]);
    }
}
