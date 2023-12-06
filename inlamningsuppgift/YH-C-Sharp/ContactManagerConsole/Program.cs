using SharedLogic.Models;
using SharedLogic.Services;

Console.WriteLine("Wello, Horld!");

CustomerService customerService = new();


customerService.AddCustomer(new Customer {
    FirstName = "Ein",
    LastName = "asd",
    Email = "ab@ab.ab",
    PhoneNumber = "1234567890"
});

customerService.AddCustomer(new Customer {
    FirstName = "Zwei",
    LastName = "dsa",
    Email = "bab@bab.bab",
    PhoneNumber = "c1234567890"
});

customerService.SaveCustomersToFile();

var tList = customerService.GetCustomerList();

customerService.DisplayAllCustomers();

Console.ReadKey();