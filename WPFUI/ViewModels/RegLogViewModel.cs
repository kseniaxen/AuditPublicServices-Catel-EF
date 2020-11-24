using System.Collections.ObjectModel;
using System.Threading;
using Catel.Data;
using Catel.MVVM;
using Catel.Services;
using WPFUI.Models;
using System.Windows;
using System;
using Catel.IoC;
using PublicServicesDomain;
using System.Linq;

namespace WPFUI.ViewModels
{
    public class RegLogViewModel : ViewModelBase
    {
        private ObservableCollection<PublicServicesDomain.Models.User> users
           = new ObservableCollection<PublicServicesDomain.Models.User>();

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
            using (var db = new PSDBContext())
            {
                db.Users.ToList().ForEach(
                    a => {
                        Console.WriteLine("Id " + a.Login + " Password " + a.Password);
                        users.Add(a);
                    }
                );
            }
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
                            using (var db = new PSDBContext())
                            {
                                if (users.FirstOrDefault(login => login.Login == userViewModel.UserLogin) == null)
                                {
                                    db.Users.Add(new PublicServicesDomain.Models.User
                                    {
                                        Login = userViewModel.UserLogin,
                                        Password = userViewModel.UserPassword
                                    });
                                    db.SaveChanges();
                                    _messageService.ShowAsync("Пользователь " + userViewModel.UserLogin + " создан!", "Регистрация");
                                    isUserViewModelExist = true;
                                }
                                else
                                {
                                    _messageService.ShowAsync("Пользователь " + userViewModel.UserLogin + " уже создан! Введите другой логин!", "Регистрация");
                                }
                            }
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
                            var user = users.FirstOrDefault(login => login.Login == userViewModel.UserLogin);
                             if (user != null)
                             {
                                if(user.Login == userViewModel.UserLogin && user.Password == userViewModel.UserPassword)
                                {
                                    isUserViewModelExist = true;
                                }
                                else
                                {
                                    _messageService.ShowAsync("Неверный логин или пароль!", "Вход");
                                }
                             }
                             else
                             {
                                 _messageService.ShowAsync("Пользователь " + userViewModel.UserLogin + " не существует!", "Вход");
                             }
                        }
                    });
                }));
            }
        }
    }
}