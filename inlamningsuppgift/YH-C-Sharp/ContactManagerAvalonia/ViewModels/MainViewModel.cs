using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SharedLogic.Models;
using SharedLogic.Services;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;

namespace ContactManagerAvalonia.ViewModels;

public partial class MainViewModel : ViewModelBase {

    private readonly ContactService? _contactService;
    private readonly IServiceProvider? _sp;

    public MainViewModel() { }
    public MainViewModel(ContactService contactService, IServiceProvider sp) {
        _sp = sp;
        _contactService = contactService;
        RefreshList();
    }

    #region Observable properties

    [ObservableProperty]
    private List<Contact>? _contactListbox = [];

    [ObservableProperty]
    private int _viewPaneWidth = 400;
    [ObservableProperty]
    private string _viewFullName = "";
    [ObservableProperty]
    private string _viewEmail = "";
    [ObservableProperty]
    private string _viewPhoneNumber = "";
    [ObservableProperty]
    private string _viewStreet = "";
    [ObservableProperty]
    private string _viewCity = "";
    [ObservableProperty]
    private string _viewPostalCode = "";
    [ObservableProperty]
    private string _viewCountry = "";
    private List<string> _viewProps = [];

    [ObservableProperty]
    private string _updateFirstName = "";
    [ObservableProperty]
    private string _updateLastName = "";
    [ObservableProperty]
    private string _updateEmail = "";
    [ObservableProperty]
    private string _updatePhoneNumber = "";
    [ObservableProperty]
    private string _updateStreet = "";
    [ObservableProperty]
    private string _updateCity = "";
    [ObservableProperty]
    private string _updatePostalCode = "";
    [ObservableProperty]
    private string _updateCountry = "";

    [ObservableProperty]
    private bool _paneOutView = false;
    [ObservableProperty]
    private bool _paneOutUpdate = false;
    [ObservableProperty]
    private bool _paneOutRemove = false;
    [ObservableProperty]
    private object? _selectedContact;

    [ObservableProperty]
    private bool _checkboxConfirmRemove;

    #endregion

    #region RelayCommands

    /// <summary>
    /// Opens folder containing the contacts json
    /// </summary>
    [RelayCommand]
    public void OpenFolder() {
        _contactService!.OpenSaveFolder();
    }

    /// <summary>
    /// Changes the view to 'Add contact'
    /// </summary>
    [RelayCommand]
    public void SetViewAddContact() {
        HideAllPanes();
        _sp!.GetRequiredService<MainWindowViewModel>().SetViewAddContact();
    }

    /// <summary>
    /// Expands 'View' pane and displays all information of selected contact
    /// </summary>
    [RelayCommand]
    public void ViewContact() {
        try {
            if (SelectedContact == null)
                return;

            HideAllPanes();
            PaneOutView = true;

            var c = SelectedContact as Contact;

            ViewFullName = c!.FullName;
            ViewEmail = c.Email;
            ViewPhoneNumber = c.PhoneNumber;
            if (c.Address != null) {
                ViewStreet = c.Address.Street;
                ViewCity = c.Address.City;
                ViewPostalCode = c.Address.PostalCode + " ";
                ViewCountry = c.Address.Country;
            }

            //Expand width of splitview pane if needed due to long entries
            _viewProps = [ViewFullName, ViewEmail, ViewPhoneNumber, ViewStreet, ViewCity, ViewPostalCode + ViewCountry];
            string longestEntry = _viewProps.OrderByDescending(s => s.Length).First();
            ViewPaneWidth = (int)Math.Floor(Math.Max(longestEntry.Length * 12.5, 320));

        } catch (Exception e) { Debug.WriteLine(e); }
    }

    /// <summary>
    /// Opens the pane for updating a contact<br/>
    /// Populates all relevant TextBlocks and hides all other panes
    /// </summary>
    [RelayCommand]
    public void OpenPaneUpdate() {
        try {
            if (SelectedContact == null)
                return;

            HideAllPanes();
            PaneOutUpdate = true;

            var c = SelectedContact as Contact;

            UpdateFirstName = c!.FirstName;
            UpdateLastName = c.LastName;
            UpdateEmail = c.Email;
            UpdatePhoneNumber = c.PhoneNumber;
            UpdateStreet = c.Address.Street;
            UpdateCity = c.Address.City;
            UpdatePostalCode = c.Address.PostalCode;
            UpdateCountry = c.Address.Country;

        } catch (Exception e) { Debug.WriteLine(e); }
    }

    /// <summary>
    /// Updates selected contact with information from update pane
    /// </summary>
    [RelayCommand]
    public void UpdateContact() {

        try {
            if (string.IsNullOrWhiteSpace(UpdateFirstName)
                || string.IsNullOrWhiteSpace(UpdateEmail))
                return;

            var c = SelectedContact as Contact;

            var updatedContact = new Contact {
                FirstName = UpdateFirstName.Trim(),
                LastName = UpdateLastName.Trim(),
                Email = UpdateEmail.Trim(),
                PhoneNumber = UpdatePhoneNumber.Trim(),

                Address = new Address {
                    Street = UpdateStreet.Trim(),
                    City = UpdateCity.Trim(),
                    PostalCode = UpdatePostalCode.Trim(),
                    Country = UpdateCountry.Trim()
                }
            };

            _contactService!.UpdateContact(c!.Email, updatedContact);
            _contactService.SaveContactsToFile();
            RefreshList();
            HideAllPanes();

        } catch (Exception e) { Debug.WriteLine(e); }
    }

    /// <summary>
    /// Toggles pop-up to confirm removal of selected contact
    /// </summary>
    [RelayCommand]
    public void ToggleRemovalConfirmation() {
        if (SelectedContact == null)
            return;

        HideAllPanes();
        CheckboxConfirmRemove = false;
        PaneOutRemove = !PaneOutRemove;
    }

    /// <summary>
    /// Removes selected contact from contactlist
    /// </summary>
    [RelayCommand]
    public void RemoveContact() {
        try {
            if (SelectedContact == null)
                return;

            var c = SelectedContact as Contact;
            if (_contactService!.RemoveContact(c!.Email)) {
                _contactService.SaveContactsToFile();
                RefreshList();

                CheckboxConfirmRemove = false;
                PaneOutRemove = false;
            }

        } catch (Exception e) { Debug.WriteLine(e); }
    }

    #endregion

    /// <summary>
    /// Populate ListBox with all contacts
    /// </summary>
    private void RefreshList() {
        ContactListbox = _contactService!.GetContactList().ToList();
    }

    /// <summary>
    /// Hides View, Update and Remove panes/pop-ups
    /// </summary>
    private void HideAllPanes() {
        PaneOutView = false;
        PaneOutUpdate = false;
        PaneOutRemove = false;
    }

    /// <summary>
    /// Adds a contact to the contactlist
    /// </summary>
    /// <param name="contact">Contact object to add</param>
    public void AddContact(Contact contact) {
        try {

            if (_contactService!.AddContact(contact)) {
                _contactService?.SaveContactsToFile();
                RefreshList();
            }

        } catch (Exception e) {
            Debug.WriteLine(e);
        }
    }
}
