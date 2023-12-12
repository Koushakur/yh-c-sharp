using Newtonsoft.Json;
using SharedLogic.Models;
using System.Diagnostics;

namespace SharedLogic.Services
{
    public class ContactService
    {
        private List<Contact> _contactList = [];
        private readonly string _saveLocation = @".\save\contacts.json";

        public ContactService() {
            ReadContactsFromFile();
        }

        /// <summary>
        /// Add a contact to the list and re-save to file
        /// </summary>
        /// <param name="contact">Contact to add</param>
        public void AddContact(Contact contact) {
            try {
                _contactList.Add(contact);
                SaveContactsToFile();
            }
            catch (Exception e) { Debug.WriteLine(e); }
        }

        /// <summary>
        /// Remove a contact from the list and then re-save to file
        /// </summary>
        /// <param name="email">E-mail to identify the user with</param>
        /// <returns>True if contact was successully removed, otherwise false</returns>
        public bool RemoveContact(string email) {
            try {
                Contact foundContact = _contactList.Find(x => x.Email == email)!;
                if (foundContact != null) {
                    _contactList.Remove(foundContact);
                    SaveContactsToFile();
                    return true;
                }
            }
            catch (Exception e) { Debug.WriteLine(e); }
            return false;
        }

        /// <summary>
        /// Tries to remove the supplied Contact object from the contactlist and then re-save the contact list
        /// </summary>
        /// <param name="contact">Contact to attempt to remove</param>
        /// <returns>true if sucessfully removed, otherwise false</returns>
        public bool RemoveContact(Contact contact) {
            try {
                if (_contactList.Remove(contact)) {
                    SaveContactsToFile();
                    return true;
                }
            }
            catch (Exception e) { Debug.WriteLine(e); }
            return false;
        }

        /// <summary>
        /// Remove a contact from the list and re-save to file
        /// </summary>
        /// <param name="contactId">GUID to identify the user with</param>
        /// <returns>True if contact was successully removed, otherwise false</returns>
        public bool RemoveContact(Guid contactId) {
            try {
                if (_contactList.Remove(_contactList.Find(x => x.Id == contactId)!)) {
                    SaveContactsToFile();
                    return true;
                }
            }
            catch (Exception e) { Debug.WriteLine(e); }
            return false;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void UpdateContact() {
            throw new NotImplementedException("Can't update contacts yet");
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
        /// Save current list of contact to file at _savelocation
        /// </summary>
        public void SaveContactsToFile() {
            FileService.SaveToFile(_saveLocation, JsonConvert.SerializeObject(_contactList, Formatting.Indented));
        }

        /// <summary>
        /// Retrieves list of contacts from _savelocation
        /// </summary>
        private void ReadContactsFromFile() {
            try {
                var readContent = FileService.ReadFromFile(_saveLocation);
                if (!string.IsNullOrEmpty(readContent)) {
                    _contactList = JsonConvert.DeserializeObject<List<Contact>>(readContent)!;
                }
            }
            catch (Exception e) { Debug.WriteLine(e); }
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
