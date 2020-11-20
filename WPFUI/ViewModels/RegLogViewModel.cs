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
    public class RegLogViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IPleaseWaitService _pleaseWaitService;
        private readonly IMessageService _messageService;
        public UserViewModel userViewModel;
        public bool isUserViewModelExist = false;
        public RegLogViewModel(IUIVisualizerService uiVisualizerService, IPleaseWaitService pleaseWaitService, IMessageService messageService)
        { 
            _uiVisualizerService = uiVisualizerService;
            _pleaseWaitService = pleaseWaitService;
            _messageService = messageService;
        }

        public override string Title { get { return "Audit Public Services"; } }

        private Command _registrationCommand;
        public Command RegistrationCommand
        {
            get
            {
                return _registrationCommand ?? (_registrationCommand = new Command(() =>
                {
                    userViewModel = new UserViewModel();
                    _uiVisualizerService.ShowDialogAsync(userViewModel, (sender, e) =>
                    {
                        if (e.Result ?? false)
                        {
                            Console.WriteLine(userViewModel.UserLogin);
                            Console.WriteLine(userViewModel.UserPassword);
                            isUserViewModelExist = true;
                        }
                    });
                }));
            }
        }

        private Command _logInCommand;
        public Command LogInCommand
        {
            get
            {
                return _logInCommand ?? (_logInCommand = new Command(() =>
                {
                    userViewModel = new UserViewModel();
                    _uiVisualizerService.ShowDialogAsync(userViewModel, (sender, e) =>
                    {
                        if (e.Result ?? false)
                        {
                            Console.WriteLine(userViewModel.UserLogin);
                            Console.WriteLine(userViewModel.UserPassword);
                            isUserViewModelExist = true;
                        }
                    });
                }));
            }
        }
    }
}