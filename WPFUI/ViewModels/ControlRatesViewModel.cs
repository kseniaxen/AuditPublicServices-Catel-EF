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
    public class ControlRatesViewModel: ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IPleaseWaitService _pleaseWaitService;
        private readonly IMessageService _messageService;
        public UserViewModel userViewModel;
        public override string Title { get { return "Rates Management"; } }

        public ControlRatesViewModel(IUIVisualizerService uiVisualizerService, IPleaseWaitService pleaseWaitService, IMessageService messageService, UserViewModel userViewModel)
        {
            this.userViewModel = userViewModel;
            _uiVisualizerService = uiVisualizerService;
            _pleaseWaitService = pleaseWaitService;
            _messageService = messageService;
            RatesCollection = new ObservableCollection<Rate>();
            writeDataInList();
        }

        public ObservableCollection<Rate> RatesCollection
        {
            get { return GetValue<ObservableCollection<Rate>>(RatesCollectionProperty); }
            set { SetValue(RatesCollectionProperty, value); }
        }
        public static readonly PropertyData RatesCollectionProperty = RegisterProperty("RatesCollection", typeof(ObservableCollection<Rate>));

        public Rate SelectedRate
        {
            get { return GetValue<Rate>(SelectedRateProperty); }
            set { SetValue(SelectedRateProperty, value); }
        }
        public static readonly PropertyData SelectedRateProperty = RegisterProperty("SelectedRate", typeof(Rate));

        private Command _addCommand;
        public Command AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new Command(() =>
                {
                    var rateViewModel = new RateViewModel();
                    _uiVisualizerService.ShowDialogAsync(rateViewModel, (sender, e) =>
                    {
                        if (e.Result ?? false)
                        {
                            using (var db = new PSDBContext())
                            {
                                if (db.Rates.FirstOrDefault(a => a.Title == rateViewModel.RateTitle && a.User.Login == userViewModel.UserLogin) == null)
                                {
                                    if (isValidNumber(rateViewModel.RatePrice))
                                    {
                                        db.Rates.Add(new PublicServicesDomain.Models.Rate
                                        {
                                            Title = rateViewModel.RateTitle,
                                            MeasureTitle = rateViewModel.RateMeasureTitle,
                                            Price = parsePrice(rateViewModel.RatePrice),
                                            User = db.Users.FirstOrDefault(login => login.Login == userViewModel.UserLogin)
                                        });
                                        db.SaveChanges();
                                        writeDataInList();
                                    }
                                    else
                                    {
                                        _messageService.ShowAsync("Цена не должна быть отрицательной или равна 0!", "Добавление тарифа");
                                    }
                                }
                                else
                                {
                                    _messageService.ShowAsync("Такой тариф уже существует!", "Добавление тарифа");
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
                    var rateViewModel = new RateViewModel(SelectedRate);
                    string titlePrev = SelectedRate.Title;
                    _uiVisualizerService.ShowDialogAsync(rateViewModel, (sender, e) =>
                    {
                        if (e.Result ?? false)
                        {
                            using (var db = new PSDBContext())
                            {
                                if (db.Rates.FirstOrDefault(a => a.Title == rateViewModel.RateTitle && a.User.Login == userViewModel.UserLogin) == null || titlePrev == rateViewModel.RateTitle)
                                {
                                    if (isValidNumber(rateViewModel.RatePrice))
                                    {
                                        var rate = db.Rates.FirstOrDefault(title => title.Title == titlePrev && title.User.Login == userViewModel.UserLogin);
                                        rate.Title = rateViewModel.RateTitle;
                                        rate.MeasureTitle = rateViewModel.RateMeasureTitle;
                                        rate.Price = parsePrice(rateViewModel.RatePrice);
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        _messageService.ShowAsync("Цена не должна быть отрицательной или равна 0!", "Редактирование тарифа");
                                    }
                                }
                                else
                                {
                                    _messageService.ShowAsync("Такой тариф уже существует!", "Редактирование тарифа");
                                }
                                writeDataInList();
                            };
                        }
                    });

                }, () => SelectedRate != null));
            }
        }
        private Command _removeCommand;
        public Command RemoveCommand
        {
            get
            {
                return _removeCommand ?? (_removeCommand = new Command(async () =>
                {
                    if (await _messageService.ShowAsync("Вы действительно хотите удалить тариф?", "Внимание!",
                        MessageButton.YesNo, MessageImage.Warning) != MessageResult.Yes)
                    {
                        return;
                    }

                    _pleaseWaitService.Show("Удаление тарифа...");
                    using (var db = new PSDBContext())
                    {
                        var rate = db.Rates.FirstOrDefault(title => title.Title == SelectedRate.Title && title.User.Login == userViewModel.UserLogin);
                        db.Rates.Remove(rate);
                        var vi = db.VolumeIndications.Where(viAd => viAd.Rate.Title == SelectedRate.Title && viAd.User.Login == userViewModel.UserLogin);
                        foreach (var item in vi)
                        {
                            db.VolumeIndications.Remove(item);
                        }
                        db.SaveChanges();
                        writeDataInList();
                    };
                    RatesCollection.Remove(SelectedRate);

                    _pleaseWaitService.Hide();
                },
                () => SelectedRate != null));
            }
        }
        private bool isValidNumber(string price)
        {
            return Decimal.Parse(price) >= 0; 
        }
        private decimal parsePrice(string price)
        {
            decimal result = Decimal.Parse(price);
            return Decimal.Round(result, 2);
        }
        private void writeDataInList()
        {
            using (var db = new PSDBContext())
            {
                RatesCollection.Clear();
                db.Rates.Where(login => login.User.Login == userViewModel.UserLogin).ToList().ForEach(a =>
                {
                    RatesCollection.Add(new Rate
                    {
                        Title = a.Title,
                        MeasureTitle = a.MeasureTitle,
                        Price = a.Price.ToString()
                    });
                });
            }
        }
    }
}
