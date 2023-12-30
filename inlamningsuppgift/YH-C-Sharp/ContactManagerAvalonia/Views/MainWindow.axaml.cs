using Avalonia.Controls;
using ContactManagerAvalonia.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ContactManagerAvalonia.Views;

public partial class MainWindow : Window {

    public MainWindow() {
        InitializeComponent();
    }

    public MainWindow(IServiceProvider sp) {
        InitializeComponent();
        DataContext = sp.GetRequiredService<MainWindowViewModel>();
    }
}
