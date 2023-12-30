using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ContactManagerAvalonia.ViewModels;

public partial class MainWindowViewModel(IServiceProvider sp) : ViewModelBase {

    private readonly IServiceProvider? _sp = sp;

    [ObservableProperty]
    private ViewModelBase? _currentViewModel = sp.GetRequiredService<MainViewModel>();

    /// <summary>
    /// Changes the view to 'Add Contact'
    /// </summary>
    [RelayCommand]
    public void SetViewAddContact() {
        CurrentViewModel = _sp!.GetRequiredService<AddContactViewModel>();
    }

    /// <summary>
    /// Changes the view to 'Main'
    /// </summary>
    [RelayCommand]
    public void SetViewMain() {
        CurrentViewModel = _sp!.GetRequiredService<MainViewModel>();
    }

}