using System;
using System.Collections.Generic;

class Product
{
    public string Name { get; set; }
    public int ProductId { get; set; }
    private decimal _price; // Member variable with underscore
    public int Quantity { get; set; }

    public decimal Price // Public property for _price
    {
        get { return _price; }
        set { _price = value; }
    }

    public decimal CalculatePrice()
    {
        return _price * Quantity;
    }
}

class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }

    public bool IsInUSA()
    {
        return Country == "USA";
    }

    public string GetFullAddress()
    {
        return $"{Street}\n{City}, {State}\n{Country}";
    }
}

class Customer
{
    public string Name { get; set; }
    public Address CustomerAddress { get; set; }

    public bool IsInUSA()
    {
        return CustomerAddress.IsInUSA();
    }
}

class Order
{
    private List<Product> _products;
    private Customer _customer;

    public Order(List<Product> products, Customer customer)
    {
        _products = products;
        _customer = customer;
    }

    public decimal CalculateTotal()
    {
        decimal total = 0;
        foreach (var product in _products)
        {
            total += product.CalculatePrice();
        }

        total += _customer.IsInUSA() ? 5 : 35;
        return total;
    }

    public string GetPackingLabel()
    {
        string packingLabel = "Packing Label:\n";
        foreach (var product in _products)
        {
            packingLabel += $"{product.Name} (ID: {product.ProductId})\n";
        }
        return packingLabel;
    }

    public string GetShippingLabel()
    {
        string shippingLabel = "Shipping Label:\n";
        shippingLabel += $"Name: {_customer.Name}\n";
        shippingLabel += "Address:\n";
        shippingLabel += _customer.CustomerAddress.GetFullAddress();
        return shippingLabel;
    }
}

class Program
{
    static void Main()
    {
        // Create products
        var product1 = new Product { Name = "Laptop", ProductId = 1, Price = 999.99m, Quantity = 2 };
        var product2 = new Product { Name = "Mouse", ProductId = 2, Price = 19.99m, Quantity = 5 };

        // Create customer
        var customerAddress = new Address { Street = "123 Main St", City = "Anytown", State = "CA", Country = "USA" };
        var customer = new Customer { Name = "John Doe", CustomerAddress = customerAddress };

        // Create an order
        var order = new Order(new List<Product> { product1, product2 }, customer);

        // Display order information
        Console.WriteLine("Order Details:");
        Console.WriteLine(order.GetPackingLabel());
        Console.WriteLine(order.GetShippingLabel());
        Console.WriteLine($"Total Price: {order.CalculateTotal():C}");
    }
}
