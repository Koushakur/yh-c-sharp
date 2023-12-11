namespace SharedLogic.Models
{
    public class Contact
    {
        public Contact() { }

        public Contact(string firstName, string lastName, string email, string phoneNumber, Address address) {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName => (string.IsNullOrEmpty(LastName) ? $"{FirstName}" : $"{FirstName} {LastName}");
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public Address Address { get; set; } = null!;
    }
}
