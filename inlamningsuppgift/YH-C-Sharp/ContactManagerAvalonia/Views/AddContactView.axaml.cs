using Avalonia.Controls;
using ContactManagerAvalonia.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ContactManagerAvalonia.Views;

public partial class AddContactView : UserControl {

    public AddContactView() {
        InitializeComponent();
    }

    public AddContactView(IServiceProvider sp) {
        InitializeComponent();
        DataContext = sp.GetRequiredService<AddContactViewModel>();
    }
}
