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
                }
                else
                {
                    this.CloseViewModelAsync(true);
                }
            });
        }

    }
}
