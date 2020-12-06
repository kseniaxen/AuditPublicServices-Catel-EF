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
                    VICollection = new ObservableCollection<VolumeIndication>();
                    AddressesCollection = new ObservableCollection<Address>();
                    RatesCollection = new ObservableCollection<Rate>();
                    ServicesCollection = new ObservableCollection<Service>();
                    reWriteDataInTable();
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
                        if (e.Result ?? false)
                        {
                            using (var db = new PSDBContext())
                            {
                                AddressesCollection.Clear();
                                var addressDb = db.Addresses.Where(login => login.User.Login == userViewModel.UserLogin);
                                foreach (var ad in addressDb)
                                {
                                    AddressesCollection.Add(new Address
                                    {
                                        Title = ad.Title
                                    });
                                }
                            }
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
                        using (var db = new PSDBContext())
                        {
                            RatesCollection.Clear();
                            var rateDb = db.Rates.Where(login => login.User.Login == userViewModel.UserLogin);
                            foreach (var ra in rateDb)
                            {
                                RatesCollection.Add(new Rate
                                {
                                    Title = ra.Title,
                                    MeasureTitle = ra.MeasureTitle,
                                    Price = ra.Price.ToString()
                                });
                            }
                        }
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
                        using (var db = new PSDBContext())
                        {
                            ServicesCollection.Clear();
                            var serviceDb = db.Services.Where(login => login.User.Login == userViewModel.UserLogin);
                            foreach (var se in serviceDb)
                            {
                                ServicesCollection.Add(new Service
                                {
                                    Title = se.Title
                                });
                            }
                        }
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
                                        Address = db.Addresses.FirstOrDefault(ad => ad.Title == VIViewModel.VISelectedAddress.Title),
                                        Rate = db.Rates.FirstOrDefault(rate => rate.Title == VIViewModel.VISelectedRate.Title),
                                        Service = db.Services.FirstOrDefault(ser => ser.Title == VIViewModel.VISelectedService.Title),
                                        PrevIndication = Int32.Parse(VIViewModel.VIPrevIndication),
                                        CurIndication = Int32.Parse(VIViewModel.VICurIndication),
                                        Total = Convert.ToDecimal((Int32.Parse(VIViewModel.VICurIndication) - Int32.Parse(VIViewModel.VIPrevIndication)) * Convert.ToDecimal(VIViewModel.VISelectedRate.Price)),
                                        DatePaid = VIViewModel.VISelectedDate,
                                        User = db.Users.FirstOrDefault(login => login.Login == userViewModel.UserLogin)
                                    });
                                    db.SaveChanges();
                                    reWriteDataInTable();
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

        private void reWriteAddressesCollection(PSDBContext db)
        {

            AddressesCollection.Clear();
            db.Addresses.Where(login => login.User.Login == userViewModel.UserLogin).ToList().ForEach(ad =>
                AddressesCollection.Add(new Address
                {
                    Title = ad.Title
                })
            );

        }
        private void reWriteServicesCollection(PSDBContext db)
        {
            ServicesCollection.Clear();
            db.Services.Where(login => login.User.Login == userViewModel.UserLogin).ToList().ForEach(ad =>
                 ServicesCollection.Add(new Service
                 {
                     Title = ad.Title
                 })
            );
        }
        private void reWriteRatesCollection(PSDBContext db)
        {
            RatesCollection.Clear();
            db.Rates.Where(login => login.User.Login == userViewModel.UserLogin).ToList().ForEach(ad =>
                 RatesCollection.Add(new Rate
                 {
                     Title = ad.Title,
                     MeasureTitle = ad.MeasureTitle,
                     Price = ad.Price.ToString()
                 })
            );
        }
        private void reWriteVICollection(PSDBContext db)
        {
            VICollection.Clear();
            db.VolumeIndications.Where(login => login.User.Login == userViewModel.UserLogin).ToList().ForEach(vi =>
                 VICollection.Add(new VolumeIndication
                 {
                     SelectedAddress = AddressesCollection.FirstOrDefault(adcol => adcol.Title == vi.Address.Title),
                     SelectedService = ServicesCollection.FirstOrDefault(adcol => adcol.Title == vi.Service.Title),
                     SelectedRate = RatesCollection.FirstOrDefault(adcol => adcol.Title == vi.Rate.Title),
                     PrevIndication = vi.PrevIndication.ToString(),
                     CurIndication = vi.CurIndication.ToString(),
                     Total = vi.Total.ToString(),
                     SelectedDate = vi.DatePaid
                 }
            ));

        }
        public void reWriteDataInTable()
        {
            using (var db = new PSDBContext())
            {
                reWriteAddressesCollection(db);
                reWriteServicesCollection(db);
                reWriteRatesCollection(db);
                reWriteVICollection(db);
            }
        }

    }
}
