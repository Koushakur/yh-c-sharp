using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SharedLogic.Models;
using System;

namespace ContactManagerAvalonia.ViewModels {
    public partial class AddContactViewModel(IServiceProvider sp) : ViewModelBase {

        private readonly IServiceProvider? _sp = sp;

        #region Observable properties

        [ObservableProperty]
        private string _inputFirstName = "";
        [ObservableProperty]
        private string _inputLastName = "";
        [ObservableProperty]
        private string _inputEmail = "";
        [ObservableProperty]
        private string _inputPhoneNumber = "";

        [ObservableProperty]
        private string _inputStreet = "";
        [ObservableProperty]
        private string _inputCity = "";
        [ObservableProperty]
        private string _inputPostalCode = "";
        [ObservableProperty]
        private string _inputCountry = "";

        #endregion

        /// <summary>
        /// Adds contact to list using the entered information
        /// </summary>
        [RelayCommand]
        public void AddContact() {
            //Do nothing if firstname or email is empty, they're required
            if (string.IsNullOrWhiteSpace(InputFirstName)
                || string.IsNullOrWhiteSpace(InputEmail)
            )
                return;

            var tContact = new Contact {
                FirstName = InputFirstName.Trim(),
                LastName = InputLastName.Trim(),
                Email = InputEmail.Trim(),
                PhoneNumber = InputPhoneNumber.Trim(),
                Address = new Address {
                    Street = InputStreet.Trim(),
                    City = InputCity.Trim(),
                    PostalCode = InputPostalCode.Trim(),
                    Country = InputCountry.Trim()
                }
            };

            _sp!.GetRequiredService<MainViewModel>().AddContact(tContact);
            SetViewMain();
        }

        /// <summary>
        /// Changes the view to 'Main'
        /// </summary>
        [RelayCommand]
        public void SetViewMain() {
            _sp!.GetRequiredService<MainWindowViewModel>().SetViewMain();
        }
    }
}