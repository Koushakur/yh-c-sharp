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
        _contactService = contactService;
        _sp = sp;
        ContactListbox = _contactService!.GetContactList().ToList();
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

    #endregion

    #region RelayCommands

    [RelayCommand]
    public void Test() {
        //Xlist!.Add(new Contact { FirstName = ObsTest, Email = ObsTest });
        _contactService!.AddContact(new Contact {
            FirstName = ObsTest,
            LastName = ObsTest + "Lastname",
            Email = ObsTest + "@domain.net",
            PhoneNumber = "000"
        });
        _contactService.SaveContactsToFile();
        ContactListbox = _contactService!.GetContactList().ToList();
        ObsTest = string.Empty;
    }

    [RelayCommand]
    public void OpenFolder() {
        _contactService!.OpenSaveFolder();
    }

    [RelayCommand]
    public void AddContact() {
    }

    [RelayCommand]
    public void ViewContact() {
    }

    [RelayCommand]
    public void UpdateContact() {
    }

    [RelayCommand]
    public void RemoveContact() {
    }

    #endregion

}
