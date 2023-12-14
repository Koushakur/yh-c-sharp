using SharedLogic.Models;
using SharedLogic.Services;

namespace xUnitTests
{
    public class Tests_ContactService
    {
        [Fact]
        public void AddContact_AddContact_ReturnTrue() {

            Contact tContact = CreateContact();

            var contactService = new ContactService();
            bool result = contactService.AddContact(tContact);

            Assert.True(result);
            Assert.True(contactService.GetContactCount() == 1);
        }

        [Fact]
        public void RemoveContact_AddThenRemoveContactUsingEmail_ReturnTrueAndListEmpty() {

            Contact tContact = CreateContact();

            var contactService = new ContactService();

            contactService.AddContact(CreateContact());
            bool resultEmail = contactService.RemoveContact(tContact.Email);

            Assert.True(resultEmail);
            Assert.True(contactService.GetContactCount() == 0);
        }

        [Fact]
        public void RemoveContact_AddThenRemoveContactUsingGuid_ReturnTrueAndListEmpty() {

            Contact tContact = CreateContact();

            var contactService = new ContactService();

            contactService.AddContact(tContact);
            bool resultGuid = contactService.RemoveContact(tContact.Id);

            Assert.True(resultGuid);
            Assert.True(contactService.GetContactCount() == 0);
        }

        [Fact]
        public void RemoveContact_AddThenRemoveContactUsingObject_ReturnTrueAndListEmpty() {

            Contact tContact = CreateContact();

            var contactService = new ContactService();

            contactService.AddContact(tContact);
            bool resultObject = contactService.RemoveContact(tContact);

            Assert.True(resultObject);
            Assert.True(contactService.GetContactCount() == 0);
        }

        private Contact CreateContact() {
            return new Contact {
                FirstName = "Enart",
                LastName = "Trömse",
                Email = "enart@domain.se",
                PhoneNumber = "070 128 65 79",
                Address = new Address {
                    Street = "Udegatan 8b",
                    City = "Stockholm",
                    PostalCode = "654 78",
                    Country = "Sweden"
                }
            };
        }
    }
}
