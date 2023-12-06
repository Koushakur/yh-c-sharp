using Newtonsoft.Json;
using SharedLogic.Models;
using System.Diagnostics;

namespace SharedLogic.Services
{
    public class CustomerService
    {
        private List<Customer> _customerList = [];
        private readonly string _saveLocation = @".\output\test.json";

        public CustomerService() {
            ReadCustomersFromFile();
        }

        public void AddCustomer(Customer customer) {
            _customerList.Add(customer);
        }

        public void RemoveCustomer(Customer customer) {
            _customerList.Remove(customer);
        }

        /// <summary>
        /// Returns read-only version of the customer list
        /// </summary>
        /// <returns>IEnumerable<Customer></returns>
        public IEnumerable<Customer> GetCustomerList() {
            return _customerList;
        }

        public void DisplayAllCustomers() {

            try {
                foreach (var customer in _customerList) {
                    //var tC = JsonConvert.SerializeObject(customer, Formatting.Indented);
                    Console.WriteLine(customer.FirstName);
                }
            }
            catch (Exception e) { Debug.WriteLine(e); }
        }

        public void DisplayCustomer(Customer customer) {

            try {
                Console.WriteLine($"{customer.FirstName} {customer.LastName}");
                Console.WriteLine($"{customer.Email}");
            }
            catch (Exception e) { Debug.WriteLine(e); }
        }

        public void SaveCustomersToFile() {
            FileService.SaveToFile(_saveLocation, JsonConvert.SerializeObject(_customerList, Formatting.Indented));

            FileService.OpenFolder(Path.Combine(Directory.GetCurrentDirectory(), Path.GetDirectoryName(_saveLocation[2..])!));
        }

        public void ReadCustomersFromFile() {

            try {
                var readContent = FileService.ReadFromFile(_saveLocation);
                if (!string.IsNullOrEmpty(readContent)) {
                    _customerList = JsonConvert.DeserializeObject<List<Customer>>(readContent)!;
                    //Debug.WriteLine(_customerList);
                }
            }
            catch (Exception e) { Debug.WriteLine(e); }
        }
    }
}
