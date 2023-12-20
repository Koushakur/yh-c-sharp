using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SharedLogic.Models;
using SharedLogic.Services;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace ContactManagerAvalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly ContactService? _contactService;
    private readonly IServiceProvider? _sp;

    public MainViewModel() { }
    public MainViewModel(ContactService contactService, IServiceProvider sp) {
        _sp = sp;
        _contactService = contactService;
        UpdateList();
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
    private bool _paneOutAdd = false;
    [ObservableProperty]
    private bool _paneOutView = false;
    [ObservableProperty]
    private bool _paneOut3 = false;
    [ObservableProperty]
    private object _selectedContact;

    [ObservableProperty]
    private string _viewFullName = "";
    [ObservableProperty]
    private string _viewEmail = "";
    [ObservableProperty]
    private string _viewPhoneNumber = "";

    #endregion

    #region RelayCommands

    [RelayCommand]
    public void OpenFolder() {
        _contactService!.OpenSaveFolder();
    }

    [RelayCommand]
    public void TogglePaneAdd() {
        PaneOutView = false;
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
        //_contactService?.SaveContactsToFile();
        UpdateList();

        InputFirstName = string.Empty;
        InputLastName = string.Empty;
        InputEmail = string.Empty;
        InputPhoneNumber = string.Empty;

    }

    [RelayCommand]
    public void ViewContact() {
        if (SelectedContact == null)
            return;

        PaneOutView = true;
        PaneOutAdd = false;

        Contact t = (Contact)SelectedContact;
        ViewFullName = t.FullName;
        ViewEmail = t.Email;
        ViewPhoneNumber = t.PhoneNumber;
    }

    [RelayCommand]
    public void UpdateContact() {
    }

    [RelayCommand]
    public void RemoveContact() {
    }

    [RelayCommand]
    public void TogglePane3() {
        PaneOut3 = !PaneOut3;
    }

    #endregion

    /// <summary>
    /// Populate ListBox with all contacts
    /// </summary>
    private void UpdateList() {
        ContactListbox = _contactService!.GetContactList().ToList();
    }

}
