using Avalonia.Controls;
using ContactManagerAvalonia.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ContactManagerAvalonia.Views;

public partial class MainView : UserControl {

    public MainView() {
        InitializeComponent();
    }

    public MainView(IServiceProvider sp) {
        InitializeComponent();
        DataContext = sp.GetRequiredService<MainViewModel>();
    }
}
