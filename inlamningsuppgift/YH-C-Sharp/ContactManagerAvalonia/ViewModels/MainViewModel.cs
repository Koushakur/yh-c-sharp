using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SharedLogic.Models;
using SharedLogic.Services;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

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

    #region Observables

    [ObservableProperty]
    private List<Contact>? _contactListbox = [];

    [ObservableProperty]
    private string _obsTest = "";
    [ObservableProperty]
    private string _inputFirstName = "";
    [ObservableProperty]
    private string _inputLastName = "";
    [ObservableProperty]
    private string _inputEmail = "";
    [ObservableProperty]
    private string _inputPhoneNumber = "";
    [ObservableProperty]
    private Address _inputAddress = new Address();

    [ObservableProperty]
    private string _viewFullName = "";
    [ObservableProperty]
    private string _viewEmail = "";
    [ObservableProperty]
    private string _viewPhoneNumber = "";

    [ObservableProperty]
    private string _updateFirstName = "";
    [ObservableProperty]
    private string _updateLastName = "";
    [ObservableProperty]
    private string _updateEmail = "";
    [ObservableProperty]
    private string _updatePhoneNumber = "";

    [ObservableProperty]
    private bool _paneOutAdd = false;
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

    [RelayCommand]
    public void OpenFolder() {
        _contactService!.OpenSaveFolder();
    }

    [RelayCommand]
    public void TogglePaneAdd() {
        HideAllPanes();
        PaneOutAdd = true;
    }

    [RelayCommand]
    public void AddContact() {
        var tContact = new Contact {
            FirstName = InputFirstName,
            LastName = InputLastName,
            Email = InputEmail,
            PhoneNumber = InputPhoneNumber,
        };

        _contactService!.AddContact(tContact);
        _contactService?.SaveContactsToFile();
        RefreshList();

        InputFirstName = string.Empty;
        InputLastName = string.Empty;
        InputEmail = string.Empty;
        InputPhoneNumber = string.Empty;

    }

    [RelayCommand]
    public void ViewContact() {
        if (SelectedContact == null)
            return;

        HideAllPanes();
        PaneOutView = true;

        var t = SelectedContact as Contact;
        ViewFullName = t.FullName;
        ViewEmail = t.Email;
        ViewPhoneNumber = t.PhoneNumber;
        if (t.Address != null) {

        }
    }

    [RelayCommand]
    public void TogglePaneUpdate() {
        if (SelectedContact == null)
            return;

        HideAllPanes();
        PaneOutUpdate = true;

        var tContact = SelectedContact as Contact;

        UpdateFirstName = tContact.FirstName;
        UpdateLastName = tContact.LastName;
        UpdateEmail = tContact.Email;
        UpdatePhoneNumber = tContact.PhoneNumber;
    }

    [RelayCommand]
    public void UpdateContact() {

        var tContact = SelectedContact as Contact;

        var updatedContact = new Contact {
            FirstName = UpdateFirstName,
            LastName = UpdateLastName,
            Email = UpdateEmail,
            PhoneNumber = UpdatePhoneNumber,
        };
        _contactService!.UpdateContact(tContact.Email, updatedContact);
        _contactService.SaveContactsToFile();
        RefreshList();
    }

    [RelayCommand]
    public void RemovalConfirmation() {
        PaneOutRemove = !PaneOutRemove;
    }

    [RelayCommand]
    public void RemoveContact() {
        if (SelectedContact == null)
            return;

        var c = SelectedContact as Contact;
        _contactService!.RemoveContact(c.Email);
        _contactService.SaveContactsToFile();
        RefreshList();
    }

    #endregion

    /// <summary>
    /// Populate ListBox with all contacts
    /// </summary>
    private void RefreshList() {
        ContactListbox = _contactService!.GetContactList().ToList();
    }

    private void HideAllPanes() {
        PaneOutAdd = false;
        PaneOutView = false;
        PaneOutUpdate = false;
    }
}
