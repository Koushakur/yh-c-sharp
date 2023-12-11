using SharedLogic.Models;
using SharedLogic.Services;
using System.Diagnostics;

//TODO: Unit test av nått slag
namespace ContactManagerConsole.Services
{
    internal class MenuService(ContactService contactService)
    {
        private readonly ContactService _contactService = contactService;
        private static readonly IReadOnlyList<string> _exitStrings = ["q", "quit", "e", "exit"];
        private static readonly IReadOnlyList<string> _yesStrings = ["y", "yes", "1", "true"];

        /// <summary>
        /// The main menu loop, loops forever until user exits app
        /// </summary>
        public void StartMenuLoop() {
            while (true) {
                Console.Clear();
                Console.WriteLine("WELCOME TO THE CONTACT MANAGER 2000");
                DrawLine();
                Console.WriteLine("1. Add contact");
                Console.WriteLine("2. Remove contact");
                Console.WriteLine("3. Display information");
                Console.WriteLine("4. Open Folder");
                Console.WriteLine("Q. Exit");
                DrawLine('=');
                Console.WriteLine("");

                var selection = RequestInput(":", true, true);

                //Check if user wants to exit app
                if (_exitStrings.Contains(selection)) { return; }

                Console.Clear();

                switch (selection) {
                    //Add contact
                    case "1":
                        ShowMenuAddContact();
                        break;

                    //Remove contact
                    case "2":
                        if (!CheckListEmpty()) {
                            ShowMenuRemoveContact();
                        }
                        break;

                    //List all contact info
                    case "3":
                        if (!CheckListEmpty()) {
                            ShowMenuDisplayContacts();
                        }
                        break;

                    //Open folder
                    case "4":
                        if (_contactService.OpenSaveFolder()) {
                            Console.WriteLine("Opening folder...");
                        } else {
                            Console.WriteLine("Save file directory has not been created yet, add a contact!");
                        }

                        AnyKeyToContinue();
                        break;

                    default:
                        continue;
                }
            };
        }

        //ShowMenu Methods
        #region ShowMenu Methods

        /// <summary>
        /// Menu for adding a contact
        /// </summary>
        public void ShowMenuAddContact() {
            Console.WriteLine("Entering new contact");
            DrawLine();
            var tContact = new Contact();

            tContact.FirstName = RequestInput("First name (Required): ", true);
            tContact.LastName = RequestInput("Last name: ");
            tContact.Email = RequestInput("E-mail (Required): ", true);
            tContact.PhoneNumber = RequestInput("Phone number: ");

            string? selectionAddress = RequestInput("Add an address for this contact? (y/n): ", true, true);

            if (_yesStrings.Contains(selectionAddress)) {
                Console.WriteLine("");
                tContact.Address = RequestAddress();
            }

            Console.WriteLine("\nThis contact will be added: ");
            DisplayContact(tContact);

            string? selectionConfirm = RequestInput("OK? (y/n): ", true, true);

            if (_yesStrings.Contains(selectionConfirm)) {
                _contactService.AddContact(tContact);
            }
        }

        /// <summary>
        /// Menu for removing a contact
        /// </summary>
        public void ShowMenuRemoveContact() {
        TryAgain:
            Console.Clear();

            var tEmail = RequestInput("Enter e-mail of contact to be removed: ", true);

            var foundContacts = _contactService.GetContactList().Where(x => x.Email == tEmail).ToList();

            if (foundContacts.Count == 1) {
                Console.WriteLine("\nFound this contact:");
                DisplayContact(foundContacts[0]);

                if (_yesStrings.Contains(RequestInput("OK to remove? (y/n): ", true, true))) {
                    if (_contactService.RemoveContact(foundContacts[0].Email)) {
                        Console.WriteLine("Successfully removed contact");
                        AnyKeyToContinue();
                    } else {
                        Console.WriteLine("Failed to remove contact");
                    }
                }

            } else if (foundContacts.Count > 1) {
                Console.WriteLine("\nMultiple contacts found\n");

                Contact c;
                for (int i = 0; i < foundContacts.Count; i++) {
                    c = foundContacts[i];
                    Console.WriteLine($"{i + 1}: {c.FullName}, {(c.Address.City == null ? "No address" : $"{c.Address.City}")}");
                }

                int result;
                string selection = null!;
                Console.WriteLine();
                do {
                    if (selection != null) {
                        if (!int.TryParse(selection, out _)) {
                            //Is not number, user wants to abort
                            return;
                        }
                        Console.WriteLine("Number not in list, try again");
                        Console.SetCursorPosition(0, Console.CursorTop - 2);
                    }
                    selection = RequestInput("Which number contact do you want to remove? (Enter any letter to skip removing): ", true);

                } while (!int.TryParse(selection, out result) || result <= 0 || result > foundContacts.Count);

                DisplayContact(foundContacts[result - 1]);

                if (_yesStrings.Contains(RequestInput("OK to remove? (y/n): ", true, true))) {
                    _contactService.RemoveContact(foundContacts[result - 1].Id);
                }
            } else {
                if (_yesStrings.Contains(RequestInput("Failed to find any contact with that e-mail, try again? (y/n): ", true, true))) {
                    goto TryAgain;
                } else return;
            }
        }

        /// <summary>
        /// Menu for displaying all contacts and optionally detailed info for one contact
        /// </summary>
        public void ShowMenuDisplayContacts() {
            DisplayContactsNumbered();

            int selection = 0;

            var tList = _contactService.GetContactList();
            do {
                Console.WriteLine(selection);
                int.TryParse(RequestInput("Which contact would you like to view detailed info for?: ", true), out selection);
            } while (selection <= 0 || selection >= tList.Count());

            Console.WriteLine();
            DrawLine();
            DisplayContact(tList.ElementAt(selection - 1));

            AnyKeyToContinue();
        }

        #endregion

        #region Display methods

        /// <summary>
        /// Prints detailed information for all the contacts in the list
        /// </summary>
        public void DisplayContactsDetailed() {
            try {
                foreach (var customer in _contactService.GetContactList()) {
                    DisplayContact(customer);
                }
            }
            catch (Exception e) { Debug.WriteLine(e); }
        }

        /// <summary>
        /// Displays all contacts in a numbered list<br/>
        /// Only name and email will be displayed. To display all info see DisplayContactsDetailed()
        /// </summary>
        public void DisplayContactsNumbered() {
            try {
                int i = 1;
                foreach (var c in _contactService.GetContactList()) {
                    Console.WriteLine($"{i,2}: {c.FullName} ({c.Email})");
                    i++;
                }
            }
            catch (Exception e) { Debug.WriteLine(e); }
        }

        /// <summary>
        /// Prints a contact's complete information to the console
        /// </summary>
        /// <param name="contact">Contact object to display info for</param>
        public void DisplayContact(Contact contact) {

            try {
                Console.WriteLine($"Name:   {contact.FullName}");
                Console.WriteLine($"E-mail: {contact.Email}");
                Console.WriteLine($"Phone:  {contact.PhoneNumber}");
                if (contact.Address != null) {
                    DisplayAddress(contact.Address);
                } else {
                    Console.WriteLine("No address");
                }
                DrawLine('~', 25);
            }
            catch (Exception e) { Debug.WriteLine(e); }
        }

        /// <summary>
        /// Prints an address to the console
        /// </summary>
        /// <param name="address">Address to be printed</param>
        public void DisplayAddress(Address address) {
            Console.WriteLine("Address:");
            Console.WriteLine($"  {address.Street}, {address.City}");
            Console.WriteLine($"  {address.PostalCode} {address.Country}");
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Requests input from user<br/>
        /// Will accept empty input if not 'required'
        /// </summary>
        /// <param name="displayText">Text explaining to the user what's expected</param>
        /// <param name="required">If true user needs to give non-empty input</param>
        /// <param name="lowercase">If true return value is made lowercase</param>
        /// <returns>User's input, or empty string if no input given</returns>
        private static string RequestInput(string displayText, bool required = false, bool lowercase = false) {
            Console.Write(displayText);
            string? tInput;

            while (string.IsNullOrWhiteSpace(tInput = Console.ReadLine()!.Trim())) {
                if (!required) break;

                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(displayText);
            }

            if (lowercase) return tInput.ToLower(); else return tInput;
        }

        /// <summary>
        /// Asks user to enter address information
        /// </summary>
        /// <returns>Address object with users input</returns>
        private static Address RequestAddress() {
            var tAddress = new Address {
                Street = RequestInput("Street name and number: "),
                City = RequestInput("City: "),
                PostalCode = RequestInput("Postal code: "),
                Country = RequestInput("Country: ")
            };

            return tAddress;
        }

        /// <summary>
        /// Check if the contacts list is empty or not, if empty tell user and wait for input
        /// </summary>
        /// <returns>True if list is empty, otherwise false</returns>
        private bool CheckListEmpty() {
            if (!_contactService.GetContactList().Any()) {
                Console.WriteLine("Contact list is empty");
                AnyKeyToContinue();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Standard "Press any key to continue" thing
        /// </summary>
        private static void AnyKeyToContinue() {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Draws a decorative line to the console
        /// </summary>
        /// <param name="typedChar">Character to repeat</param>
        /// <param name="numChars">How many characters to write</param>
        private static void DrawLine(char typedChar = '~', int numChars = 50) {
            Console.WriteLine(new string(typedChar, numChars));
        }

        #endregion
    }
}
