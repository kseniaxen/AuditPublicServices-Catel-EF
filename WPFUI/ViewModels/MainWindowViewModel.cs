using System.Collections.ObjectModel;
using System.Threading;
using Catel.Data;
using Catel.MVVM;
using Catel.Services;
using WPFUI.Models;
using System.Windows;
using System;
using Catel.IoC;

namespace WPFUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IPleaseWaitService _pleaseWaitService;
        private readonly IMessageService _messageService;
        public UserViewModel userViewModel;

        public string UserLogin { get; set; }
        public MainWindowViewModel(IUIVisualizerService uiVisualizerService, IPleaseWaitService pleaseWaitService, IMessageService messageService)
        {
            _uiVisualizerService = uiVisualizerService;
            _pleaseWaitService = pleaseWaitService;
            _messageService = messageService;

            RegLogViewModel regLogViewModel = new RegLogViewModel(_uiVisualizerService, _pleaseWaitService, _messageService);
            _uiVisualizerService.ShowDialogAsync(regLogViewModel, (sender, e) =>
            {
                if (regLogViewModel.isUserViewModelExist)
                {
                    this.userViewModel = regLogViewModel.userViewModel;
                    Console.WriteLine(userViewModel.UserLogin);
                    Console.WriteLine(userViewModel.UserPassword);
                    UserLogin = userViewModel.UserLogin;
                }
                else
                {
                    this.CloseViewModelAsync(true);
                }
            });
        }
        public string CurrDate { get { return DateTime.Now.ToString("dd.MM.yyy"); } }

        private Command _controlAddresses;
        public Command ControlAddresses
        {
            get
            {
                return _controlAddresses ?? (_controlAddresses = new Command(() =>
                {
                    var controlAddressesViewModel = new ControlAddressesViewModel(_uiVisualizerService, _pleaseWaitService, _messageService, userViewModel);
                    _uiVisualizerService.ShowDialogAsync(controlAddressesViewModel);
                }));
            }
        }

        private Command _controlRates;
        public Command ControlRates
        {
            get
            {
                return _controlRates ?? (_controlRates = new Command(() =>
                {
                    var controlRatesViewModel = new ControlRatesViewModel(_uiVisualizerService, _pleaseWaitService, _messageService, userViewModel);
                    _uiVisualizerService.ShowDialogAsync(controlRatesViewModel);
                }));
            }
        }
    }
}
