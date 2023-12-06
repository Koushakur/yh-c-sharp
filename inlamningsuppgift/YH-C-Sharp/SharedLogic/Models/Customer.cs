namespace SharedLogic.Models
{
    public class Customer
    {
        public Customer() {

        }
        public Customer(string firstName, string lastName, string email, string phoneNumber) {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        //public Address Address;
    }
}
