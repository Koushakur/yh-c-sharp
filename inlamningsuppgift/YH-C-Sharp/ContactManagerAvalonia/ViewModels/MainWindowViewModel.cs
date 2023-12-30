
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ContactManagerAvalonia.ViewModels;

public partial class MainWindowViewModel : ViewModelBase {

    [ObservableProperty]
    private ViewModelBase? _contentViewModel;

    public MainWindowViewModel() { }
    public MainWindowViewModel(IServiceProvider sp) {
        ContentViewModel = sp.GetRequiredService<MainViewModel>();
        //ContentViewModel = new MainViewModel();
    }

}