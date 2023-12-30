using Newtonsoft.Json;
using SharedLogic.Models;
using System.Diagnostics;

namespace SharedLogic.Services {
    public class ContactService {
        private List<Contact> _contactList = [];
        private readonly string _saveLocation = @".\save\contacts.json";

        public ContactService() {
            if (!ReadContactsFromFile()) {
                // Failed to load contacts, save empty json. Mostly to create directory
                SaveContactsToFile();
            }
        }

        /// <summary>
        /// Add a contact to the list and re-save to file
        /// </summary>
        /// <param name="contact">Contact to add</param>
        public bool AddContact(Contact contact) {
            try {
                _contactList.Add(contact);
                return true;
            } catch (Exception e) { Debug.WriteLine(e); }
            return false;
        }

        /// <summary>
        /// Remove a contact from the list
        /// </summary>
        /// <param name="email">E-mail to identify the user with</param>
        /// <returns>True if contact was successully removed, otherwise false</returns>
        public bool RemoveContact(string email) {
            try {
                Contact foundContact = FindContactByEmail(email);
                if (foundContact != null) {
                    return _contactList.Remove(foundContact);
                }
            } catch (Exception e) { Debug.WriteLine(e); }
            return false;
        }

        /// <summary>
        /// Tries to remove the supplied Contact object from the contactlist
        /// </summary>
        /// <param name="contact">Contact to attempt to remove</param>
        /// <returns>true if sucessfully removed, otherwise false</returns>
        public bool RemoveContact(Contact contact) {
            try {
                return _contactList.Remove(contact);
            } catch (Exception e) { Debug.WriteLine(e); }
            return false;
        }

        /// <summary>
        /// Remove a contact from the list
        /// </summary>
        /// <param name="contactId">GUID to identify the user with</param>
        /// <returns>True if contact was successully removed, otherwise false</returns>
        public bool RemoveContact(Guid contactId) {
            try {
                return _contactList.Remove(_contactList.Find(x => x.Id == contactId)!);
            } catch (Exception e) { Debug.WriteLine(e); }
            return false;
        }

        /// <summary>
        /// Updates a contact with the information stored in updatedContact
        /// </summary>
        /// <param name="email">E-mail of contact to update</param>
        /// <param name="updatedContact">Contact object storing the updated information</param>
        public void UpdateContact(string email, Contact updatedContact) {
            Contact foundContact = FindContactByEmail(email);
            if (foundContact != null) {

                foundContact.FirstName = updatedContact.FirstName;
                foundContact.LastName = updatedContact.LastName;
                foundContact.Email = updatedContact.Email;
                foundContact.PhoneNumber = updatedContact.PhoneNumber;

                foundContact.Address = updatedContact.Address;
            }
        }

        /// <summary>
        /// Returns read-only version of the customer list
        /// </summary>
        public IEnumerable<Contact> GetContactList() {
            return _contactList;
        }

        /// <summary>
        /// Returns number of contacts in list
        /// </summary>
        public int GetContactCount() {
            return _contactList.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Contact if found, otherwise null</returns>
        public Contact FindContactByEmail(string email) {
            return _contactList.Find(x => x.Email == email)!;
        }

        /// <summary>
        /// Save current list of contact to file at _savelocation
        /// </summary>
        public bool SaveContactsToFile() {
            return FileService.SaveToFile(_saveLocation, JsonConvert.SerializeObject(_contactList, Formatting.None));
        }

        /// <summary>
        /// Retrieves list of contacts from _savelocation
        /// </summary>
        private bool ReadContactsFromFile() {
            try {
                var readContent = FileService.ReadFromFile(_saveLocation);
                if (!string.IsNullOrEmpty(readContent)) {
                    _contactList = JsonConvert.DeserializeObject<List<Contact>>(readContent)!;

                } else return false;

            } catch (Exception e) { Debug.WriteLine(e); return false; }
            return true;
        }

        /// <summary>
        /// Open _savelocation in explorer or equivalent
        /// </summary>
        /// <returns>true if successfully opened folder, otherwise false</returns>
        public bool OpenSaveFolder() {
            return FileService.OpenFolder(Path.GetDirectoryName(Path.GetFullPath(_saveLocation))!);
        }
    }
}
