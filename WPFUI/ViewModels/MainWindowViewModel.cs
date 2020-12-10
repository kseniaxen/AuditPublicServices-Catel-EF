using System.Collections.ObjectModel;
using Catel.Data;
using Catel.MVVM;
using Catel.Services;
using WPFUI.Models;
using System;
using PublicServicesDomain;
using System.Linq;

namespace WPFUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IPleaseWaitService _pleaseWaitService;
        private readonly IMessageService _messageService;
        public UserViewModel userViewModel;
        private RegLogViewModel regLogViewModel;
        public override string Title { get { return "Audit Public Services"; } }
        public string UserLogin { get; set; }
        public MainWindowViewModel(IUIVisualizerService uiVisualizerService, IPleaseWaitService pleaseWaitService, IMessageService messageService)
        {
            _uiVisualizerService = uiVisualizerService;
            _pleaseWaitService = pleaseWaitService;
            _messageService = messageService;
            regLogViewModel = new RegLogViewModel(_uiVisualizerService, _pleaseWaitService, _messageService);
            _uiVisualizerService.ShowDialogAsync(regLogViewModel, (sender, e) =>
            {
                if (regLogViewModel.isUserViewModelExist)
                {
                    this.userViewModel = regLogViewModel.userViewModel;
                    UserLogin = userViewModel.UserLogin;
                    VICollection = new ObservableCollection<VolumeIndication>();
                    AddressesCollection = new ObservableCollection<Address>();
                    RatesCollection = new ObservableCollection<Rate>();
                    ServicesCollection = new ObservableCollection<Service>();
                    UserAddressesCollection = new ObservableCollection<Address>();
                    writeDataInTable();
                    foreach (var item in AddressesCollection)
                    {
                        UserAddressesCollection.Add(item);
                    }

                    if (UserAddressesCollection.Count > 0)
                    {
                        SelectedAddress = UserAddressesCollection.First();
                    }
                    SelectedDate = DateTime.Now;
                    writeDataInTable();
                }
                else
                {
                    this.CloseViewModelAsync(true);
                }
            });
        }
        public string CurrDate { get { return DateTime.Now.ToString("dd.MM.yyy"); } }

        public ObservableCollection<Address> AddressesCollection { get; set; }
        public ObservableCollection<Rate> RatesCollection { get; set; }
        public ObservableCollection<Service> ServicesCollection { get; set; }

        public ObservableCollection<VolumeIndication> VICollection
        {
            get { return GetValue<ObservableCollection<VolumeIndication>>(VICollectionProperty); }
            set { SetValue(VICollectionProperty, value); }
        }
        public static readonly PropertyData VICollectionProperty = RegisterProperty("VICollection", typeof(ObservableCollection<VolumeIndication>));

        public VolumeIndication SelectedVI
        {
            get { return GetValue<VolumeIndication>(SelectedVIProperty); }
            set { SetValue(SelectedVIProperty, value); }
        }
        public static readonly PropertyData SelectedVIProperty = RegisterProperty("SelectedVI", typeof(VolumeIndication));

        public ObservableCollection<Address> UserAddressesCollection { get; set; }
        public Address SelectedAddress
        {
            get { return GetValue<Address>(SelectedAddressProperty); }
            set
            {
                SetValue(SelectedAddressProperty, value);
                writeDataInTable();
            }

        }
        public static readonly PropertyData SelectedAddressProperty = RegisterProperty("SelectedAddress", typeof(Address));

        public DateTime SelectedDate
        {
            get { return GetValue<DateTime>(SelectedDateProperty); }
            set
            {
                SetValue(SelectedDateProperty, value);
                writeDataWithDateTimeInTable();
            }
        }
        public static readonly PropertyData SelectedDateProperty = RegisterProperty("SelectedDate", typeof(DateTime));

        private Command _controlAddresses;
        public Command ControlAddresses
        {
            get
            {
                return _controlAddresses ?? (_controlAddresses = new Command(() =>
                {
                    var controlAddressesViewModel = new ControlAddressesViewModel(_uiVisualizerService, _pleaseWaitService, _messageService, userViewModel);
                    _uiVisualizerService.ShowDialogAsync(controlAddressesViewModel, (sender, e) =>
                    {
                        writeDataInTable();
                        UserAddressesCollection.Clear();
                        foreach (var item in AddressesCollection)
                        {
                            UserAddressesCollection.Add(item);
                        }
                        if (UserAddressesCollection.Count > 0)
                        {
                            SelectedAddress = UserAddressesCollection.First();
                        }
                    });
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
                    _uiVisualizerService.ShowDialogAsync(controlRatesViewModel, (sender, e) =>
                    {
                        writeDataInTable();
                    });

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
                    _uiVisualizerService.ShowDialogAsync(controlServicesViewModel, (sender, e) =>
                    {
                        writeDataInTable();
                    });

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

                    var VIViewModel = new VolumeIndicationViewModel(null, AddressesCollection, ServicesCollection, RatesCollection);

                    _uiVisualizerService.ShowDialogAsync(VIViewModel, (sender, e) =>
                    {
                        if (e.Result ?? false)
                        {
                            if (Int32.Parse(VIViewModel.VICurIndication) >= Int32.Parse(VIViewModel.VIPrevIndication))
                            {

                                using (var db = new PSDBContext())
                                {
                                    db.VolumeIndications.Add(new PublicServicesDomain.Models.VolumeIndication
                                    {
                                        Address = db.Addresses.FirstOrDefault(ad => ad.Title == VIViewModel.VISelectedAddress.Title && ad.User.Login == userViewModel.UserLogin),
                                        Rate = db.Rates.FirstOrDefault(rate => rate.Title == VIViewModel.VISelectedRate.Title && rate.User.Login == userViewModel.UserLogin),
                                        Service = db.Services.FirstOrDefault(ser => ser.Title == VIViewModel.VISelectedService.Title && ser.User.Login == userViewModel.UserLogin),
                                        PrevIndication = Int32.Parse(VIViewModel.VIPrevIndication),
                                        CurIndication = Int32.Parse(VIViewModel.VICurIndication),
                                        Total = Convert.ToDecimal((Int32.Parse(VIViewModel.VICurIndication) - Int32.Parse(VIViewModel.VIPrevIndication)) * Convert.ToDecimal(VIViewModel.VISelectedRate.Price)),
                                        DatePaid = VIViewModel.VISelectedDate,
                                        User = db.Users.FirstOrDefault(login => login.Login == userViewModel.UserLogin)
                                    });
                                    Console.WriteLine(db.Rates.FirstOrDefault(rate => rate.Title == VIViewModel.VISelectedRate.Title && rate.User.Login == userViewModel.UserLogin).Title);
                                    db.SaveChanges();
                                    writeDataInTable();
                                }
                            }
                            else
                            {
                                _messageService.ShowAsync("Текущее показание не должно быть больше предыдущего!", "Добавление записи");
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
                    var VIViewModel = new VolumeIndicationViewModel(SelectedVI, AddressesCollection, ServicesCollection, RatesCollection);
                    int viPrevId = SelectedVI.Id;
                    _uiVisualizerService.ShowDialogAsync(VIViewModel, (sender, e) =>
                    {
                        if (e.Result ?? false)
                        {
                            if (Int32.Parse(VIViewModel.VICurIndication) >= Int32.Parse(VIViewModel.VIPrevIndication))
                            {
                                using (var db = new PSDBContext())
                                {
                                    var viCurr = db.VolumeIndications.FirstOrDefault(id => id.Id == viPrevId);
                                    viCurr.Address = db.Addresses.FirstOrDefault(ad => ad.Title == VIViewModel.VISelectedAddress.Title && ad.User.Login == userViewModel.UserLogin);
                                    viCurr.Service = db.Services.FirstOrDefault(ser => ser.Title == VIViewModel.VISelectedService.Title && ser.User.Login == userViewModel.UserLogin);
                                    viCurr.Rate = db.Rates.FirstOrDefault(rate => rate.Title == VIViewModel.VISelectedRate.Title && rate.User.Login == userViewModel.UserLogin);
                                    viCurr.PrevIndication = Int32.Parse(VIViewModel.VIPrevIndication);
                                    viCurr.CurIndication = Int32.Parse(VIViewModel.VICurIndication);
                                    viCurr.Total = Convert.ToDecimal((Int32.Parse(VIViewModel.VICurIndication) - Int32.Parse(VIViewModel.VIPrevIndication)) * Convert.ToDecimal(VIViewModel.VISelectedRate.Price));
                                    viCurr.DatePaid = VIViewModel.VISelectedDate;
                                    db.SaveChanges();
                                }
                            }
                        }
                        writeDataInTable();
                    });
                }, () => SelectedVI != null));
            }
        }

        private Command _removeCommand;
        public Command RemoveCommand
        {
            get
            {
                return _removeCommand ?? (_removeCommand = new Command(async () =>
                {
                    if (await _messageService.ShowAsync("Вы действительно хотите удалить эту запись?", "Внимание!",
                       MessageButton.YesNo, MessageImage.Warning) != MessageResult.Yes)
                    {
                        return;
                    }

                    _pleaseWaitService.Show("Удаление записи...");
                    using (var db = new PSDBContext())
                    {
                        var viCurr = db.VolumeIndications.FirstOrDefault(id => id.Id == SelectedVI.Id);
                        db.VolumeIndications.Remove(viCurr);
                        db.SaveChanges();
                        writeDataInTable();
                    };
                    VICollection.Remove(SelectedVI);

                    _pleaseWaitService.Hide();
                }, () => SelectedVI != null));
            }
        }

        private Command _regLogCommand;
        public Command RegLogCommand
        {
            get
            {
                return _regLogCommand ?? (_regLogCommand = new Command(async () =>
                {
                    var tempUserViewModel = new UserViewModel();
                    tempUserViewModel = userViewModel;
                    regLogViewModel = new RegLogViewModel(_uiVisualizerService, _pleaseWaitService, _messageService);
                    await _uiVisualizerService.ShowDialogAsync(regLogViewModel, (sender, e) =>
                     {
                         if (regLogViewModel.isUserViewModelExist)
                         {
                             this.userViewModel = regLogViewModel.userViewModel;
                         }
                         else
                         {
                             this.userViewModel = tempUserViewModel;
                         }
                         Console.WriteLine(userViewModel.UserLogin);
                         Console.WriteLine(userViewModel.UserPassword);
                         UserLogin = userViewModel.UserLogin;
                         VICollection = new ObservableCollection<VolumeIndication>();
                         AddressesCollection = new ObservableCollection<Address>();
                         RatesCollection = new ObservableCollection<Rate>();
                         ServicesCollection = new ObservableCollection<Service>();
                         UserAddressesCollection = new ObservableCollection<Address>();
                         writeDataInTable();
                         foreach (var item in AddressesCollection)
                         {
                             UserAddressesCollection.Add(item);
                         }

                         if (UserAddressesCollection.Count > 0)
                         {
                             SelectedAddress = UserAddressesCollection.First();
                         }
                         SelectedDate = DateTime.Now;
                         writeDataInTable();
                     });
                }));
            }
        }

        public void writeDataInTable()
        {
            using (var db = new PSDBContext())
            {
                AddressesCollection.Clear();
                db.Addresses.Where(login => login.User.Login == userViewModel.UserLogin).ToList().ForEach(ad =>
                    AddressesCollection.Add(new Address
                    {
                        Title = ad.Title
                    })
                );

                ServicesCollection.Clear();
                db.Services.Where(login => login.User.Login == userViewModel.UserLogin).ToList().ForEach(ad =>
                     ServicesCollection.Add(new Service
                     {
                         Title = ad.Title
                     })
                );

                RatesCollection.Clear();
                db.Rates.Where(login => login.User.Login == userViewModel.UserLogin).ToList().ForEach(ad =>
                     RatesCollection.Add(new Rate
                     {
                         Title = ad.Title,
                         MeasureTitle = ad.MeasureTitle,
                         Price = ad.Price.ToString()
                     })
                );
                VICollection.Clear();
                if (SelectedAddress == null)
                {
                    db.VolumeIndications.Where(login => login.User.Login == userViewModel.UserLogin).ToList().ForEach(vi =>
                         VICollection.Add(new VolumeIndication
                         {
                             SelectedAddress = AddressesCollection.FirstOrDefault(adcol => adcol.Title == vi.Address.Title),
                             SelectedService = ServicesCollection.FirstOrDefault(adcol => adcol.Title == vi.Service.Title),
                             SelectedRate = RatesCollection.FirstOrDefault(adcol => adcol.Title == vi.Rate.Title),
                             PrevIndication = vi.PrevIndication.ToString(),
                             CurIndication = vi.CurIndication.ToString(),
                             Total = vi.Total.ToString(),
                             SelectedDate = vi.DatePaid,
                             Id = vi.Id
                         }
                    ));
                }
                else
                {
                    db.VolumeIndications.Where(login => login.User.Login == userViewModel.UserLogin && login.Address.Title == SelectedAddress.Title).ToList().ForEach(vi =>
                         VICollection.Add(new VolumeIndication
                         {
                             SelectedAddress = AddressesCollection.FirstOrDefault(adcol => adcol.Title == vi.Address.Title),
                             SelectedService = ServicesCollection.FirstOrDefault(adcol => adcol.Title == vi.Service.Title),
                             SelectedRate = RatesCollection.FirstOrDefault(adcol => adcol.Title == vi.Rate.Title),
                             PrevIndication = vi.PrevIndication.ToString(),
                             CurIndication = vi.CurIndication.ToString(),
                             Total = vi.Total.ToString(),
                             SelectedDate = vi.DatePaid,
                             Id = vi.Id
                         }
                    ));
                }
            }
        }
        public void writeDataWithDateTimeInTable()
        {
            using (var db = new PSDBContext())
            {
                AddressesCollection.Clear();
                db.Addresses.Where(login => login.User.Login == userViewModel.UserLogin).ToList().ForEach(ad =>
                    AddressesCollection.Add(new Address
                    {
                        Title = ad.Title
                    })
                );

                ServicesCollection.Clear();
                db.Services.Where(login => login.User.Login == userViewModel.UserLogin).ToList().ForEach(ad =>
                     ServicesCollection.Add(new Service
                     {
                         Title = ad.Title
                     })
                );

                RatesCollection.Clear();
                db.Rates.Where(login => login.User.Login == userViewModel.UserLogin).ToList().ForEach(ad =>
                     RatesCollection.Add(new Rate
                     {
                         Title = ad.Title,
                         MeasureTitle = ad.MeasureTitle,
                         Price = ad.Price.ToString()
                     })
                );
                VICollection.Clear();
                db.VolumeIndications.Where(login => login.User.Login == userViewModel.UserLogin && login.DatePaid == SelectedDate).ToList().ForEach(vi =>
                     VICollection.Add(new VolumeIndication
                     {
                         SelectedAddress = AddressesCollection.FirstOrDefault(adcol => adcol.Title == vi.Address.Title),
                         SelectedService = ServicesCollection.FirstOrDefault(adcol => adcol.Title == vi.Service.Title),
                         SelectedRate = RatesCollection.FirstOrDefault(adcol => adcol.Title == vi.Rate.Title),
                         PrevIndication = vi.PrevIndication.ToString(),
                         CurIndication = vi.CurIndication.ToString(),
                         Total = vi.Total.ToString(),
                         SelectedDate = vi.DatePaid,
                         Id = vi.Id
                     }
                ));
            }
        }
    }
}
