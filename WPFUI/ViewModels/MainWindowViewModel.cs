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

        private Command _controlService;
        public Command ControlServices
        {
            get
            {
                return _controlService ?? (_controlService = new Command(() =>
                {
                    var controlServicesViewModel = new ControlServicesViewModel(_uiVisualizerService, _pleaseWaitService, _messageService, userViewModel);
                    _uiVisualizerService.ShowDialogAsync(controlServicesViewModel);
                }));
            }
        }

        private Command _addCommand;
        public Command AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new Command(() =>
                {
                    ObservableCollection<Address> obs = new ObservableCollection<Address>
                    {
                        new Address
                        {
                            Title = "Address 1"
                        },
                        new Address
                        {
                            Title = "Address 2"
                        },
                        new Address
                        {
                            Title = "Address 3"
                        }
                    };

                    ObservableCollection<Service> obsS = new ObservableCollection<Service>
                    {
                        new Service
                        {
                            Title = "Service 1"
                        },
                        new Service
                        {
                            Title = "Service 2"
                        },
                        new Service
                        {
                            Title = "Service 3"
                        }
                    };
                    var VIViewModel = new VolumeIndicationViewModel(null, obs, obsS);

                    _uiVisualizerService.ShowDialogAsync(VIViewModel, (sender, e) =>
                    {
                        if(e.Result ?? false)
                        {
                            Console.WriteLine("Address = " + VIViewModel.VISelectedAddress.Title);
                            Console.WriteLine("Service = " + VIViewModel.VISelectedService.Title);
                        }
                    });

                }));
            }
        }
    }
}
