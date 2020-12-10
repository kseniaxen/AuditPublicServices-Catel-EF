using Catel.Data;
using Catel.MVVM;
using Catel.Services;
using PublicServicesDomain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WPFUI.Models;

namespace WPFUI.ViewModels
{
    public class ControlServicesViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IPleaseWaitService _pleaseWaitService;
        private readonly IMessageService _messageService;
        public UserViewModel userViewModel;
        public override string Title { get { return "Services Management"; } }

        public ControlServicesViewModel(IUIVisualizerService uiVisualizerService, IPleaseWaitService pleaseWaitService, IMessageService messageService, UserViewModel userViewModel)
        {
            this.userViewModel = userViewModel;
            _uiVisualizerService = uiVisualizerService;
            _pleaseWaitService = pleaseWaitService;
            _messageService = messageService;

            ServicesCollection = new ObservableCollection<Service>();
            writeDatainList();
        }
        public ObservableCollection<Service> ServicesCollection
        {
            get { return GetValue<ObservableCollection<Service>>(ServicesCollectionProperty); }
            set { SetValue(ServicesCollectionProperty, value); }
        }
        public static readonly PropertyData ServicesCollectionProperty = RegisterProperty("ServicesCollection", typeof(ObservableCollection<Service>));

        public Service SelectedService
        {
            get { return GetValue<Service>(SelectedServiceProperty); }
            set { SetValue(SelectedServiceProperty, value); }
        }
        public static readonly PropertyData SelectedServiceProperty = RegisterProperty("SelectedService", typeof(Service));

        private Command _addCommand;
        public Command AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new Command(() =>
                {
                    var serviceViewModel = new ServiceViewModel();
                    _uiVisualizerService.ShowDialogAsync(serviceViewModel, (sender, e) =>
                    {
                        if (e.Result ?? false)
                        {
                            using (var db = new PSDBContext())
                            {
                                if (db.Services.FirstOrDefault(a => a.Title == serviceViewModel.ServiceTitle && a.User.Login == userViewModel.UserLogin) == null)
                                {
                                    db.Services.Add(new PublicServicesDomain.Models.Service
                                    {
                                        Title = serviceViewModel.ServiceTitle,
                                        User = db.Users.FirstOrDefault(login => login.Login == userViewModel.UserLogin)
                                    });
                                    db.SaveChanges();
                                    writeDatainList();
                                }
                                else
                                {
                                    _messageService.ShowAsync("Такая услуга уже существует!", "Добавление услуги");
                                }
                            }
                        }
                    });
                }));
            }
        }
        private Command _editCommand;
        public Command EditCommand
        {
            get
            {
                return _editCommand ?? (_editCommand = new Command(() =>
                {
                    var serviceViewModel = new ServiceViewModel(SelectedService);
                    string titlePrev = SelectedService.Title;
                    _uiVisualizerService.ShowDialogAsync(serviceViewModel, (sender, e) =>
                    {
                        if (e.Result ?? false)
                        {
                            using (var db = new PSDBContext())
                            {
                                if (db.Services.FirstOrDefault(a => a.Title == serviceViewModel.ServiceTitle && a.User.Login == userViewModel.UserLogin) == null || titlePrev == serviceViewModel.ServiceTitle)
                                {
                                    var service = db.Services.FirstOrDefault(title => title.Title == titlePrev && title.User.Login == userViewModel.UserLogin);
                                    service.Title = serviceViewModel.ServiceTitle;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    _messageService.ShowAsync("Такая услуга уже существует!", "Редактирование услуги");
                                }
                                writeDatainList();
                            };
                        }
                    });

                }, () => SelectedService != null));
            }
        }
        private Command _removeCommand;
        public Command RemoveCommand
        {
            get
            {
                return _removeCommand ?? (_removeCommand = new Command(async () =>
                {
                    if (await _messageService.ShowAsync("Вы действительно хотите удалить услугу?", "Внимание!",
                        MessageButton.YesNo, MessageImage.Warning) != MessageResult.Yes)
                    {
                        return;
                    }

                    _pleaseWaitService.Show("Удаление услуги...");
                    using (var db = new PSDBContext())
                    {
                        var service = db.Services.FirstOrDefault(title => title.Title == SelectedService.Title && title.User.Login == userViewModel.UserLogin);
                        db.Services.Remove(service);
                        var vi = db.VolumeIndications.Where(viAd => viAd.Service.Title == SelectedService.Title && viAd.User.Login == userViewModel.UserLogin);
                        foreach (var item in vi)
                        {
                            db.VolumeIndications.Remove(item);
                        }
                        db.SaveChanges();
                        writeDatainList();
                    };
                    ServicesCollection.Remove(SelectedService);

                    _pleaseWaitService.Hide();
                },
                () => SelectedService != null));
            }
        }

        private void writeDatainList()
        {
            using (var db = new PSDBContext())
            {
                ServicesCollection.Clear();
                db.Services.Where(login => login.User.Login == userViewModel.UserLogin).ToList().ForEach(a =>
                {
                    ServicesCollection.Add(new Service { Title = a.Title });
                });
            }
        }
    }
}
