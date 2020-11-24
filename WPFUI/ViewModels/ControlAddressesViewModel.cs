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
    public class ControlAddressesViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IPleaseWaitService _pleaseWaitService;
        private readonly IMessageService _messageService;
        public UserViewModel userViewModel;

        public ControlAddressesViewModel(IUIVisualizerService uiVisualizerService, IPleaseWaitService pleaseWaitService, IMessageService messageService, UserViewModel userViewModel)
        {
            this.userViewModel = userViewModel;
            _uiVisualizerService = uiVisualizerService;
            _pleaseWaitService = pleaseWaitService;
            _messageService = messageService;
            AddressesCollection = new ObservableCollection<Address>();
            using (var db = new PSDBContext())
            {
                var addressesDb = db.Addresses.Where(login => login.User.Login == userViewModel.UserLogin);
                foreach(var address in addressesDb)
                {
                    AddressesCollection.Add(new Address { Title = address.Title });
                }
            }
        }
        public ObservableCollection<Address> AddressesCollection
        {
            get { return GetValue<ObservableCollection<Address>>(AddressesCollectionProperty); }
            set { SetValue(AddressesCollectionProperty, value); }
        }
        public static readonly PropertyData AddressesCollectionProperty = RegisterProperty("AddressesCollection", typeof(ObservableCollection<Address>));

        public Address SelectedAddress
        {
            get { return GetValue<Address>(SelectedAddressProperty); }
            set { SetValue(SelectedAddressProperty, value); }
        }
        public static readonly PropertyData SelectedAddressProperty = RegisterProperty("SelectedAddress", typeof(Address));

        private Command _addCommand;
        public Command AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new Command(() =>
                {
                    var addressViewModel = new AddressViewModel();
                    _uiVisualizerService.ShowDialogAsync(addressViewModel, (sender, e) =>
                    {
                        if(e.Result ?? false)
                        {
                            if (AddressesCollection.FirstOrDefault(adTitle => adTitle.Title == addressViewModel.AddressTitle) == null)
                            {
                                using (var db = new PSDBContext())
                                {
                                    db.Addresses.Add(new PublicServicesDomain.Models.Address
                                    {
                                        Title = addressViewModel.AddressTitle,
                                        User = db.Users.FirstOrDefault(login => login.Login == userViewModel.UserLogin)
                                    });
                                    db.SaveChanges();
                                    AddressesCollection.Clear();
                                    db.Addresses.Where(login => login.User.Login == userViewModel.UserLogin).ToList().ForEach(a =>
                                    {
                                        AddressesCollection.Add(new Address { Title = a.Title });
                                    });
                                }
                            }
                            else
                            {
                                _messageService.ShowAsync("Такой адрес уже существует!", "Добавление адреса");
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
                    var addressViewModel = new AddressViewModel(SelectedAddress);
                    string titleTemp = SelectedAddress.Title;
                    _uiVisualizerService.ShowDialogAsync(addressViewModel, (sender, e) =>
                    { 
                        using (var db = new PSDBContext())
                        { 
                            if (db.Addresses.FirstOrDefault(a => a.Title == addressViewModel.AddressTitle) == null)
                            {
                                    var address = db.Addresses.FirstOrDefault(title => title.Title == titleTemp);
                                    address.Title = addressViewModel.AddressTitle;
                                    db.SaveChanges();
                                    AddressesCollection.Clear();
                                    db.Addresses.Where(login => login.User.Login == userViewModel.UserLogin).ToList().ForEach(a =>
                                    {
                                        AddressesCollection.Add(new Address { Title = a.Title });
                                    });
                            }
                            else
                            {
                                _messageService.ShowAsync("Такой адрес уже существует!", "Редактирование адреса");
                                AddressesCollection.Clear();
                                db.Addresses.Where(login => login.User.Login == userViewModel.UserLogin).ToList().ForEach(a =>
                                {
                                    AddressesCollection.Add(new Address { Title = a.Title });
                                });
                            }
                        };
                    });
                    
                }, () => SelectedAddress != null));
            }
        }
        private Command _removeCommand;
        public Command RemoveCommand
        {
            get
            {
                return _removeCommand ?? (_removeCommand = new Command(async () =>
                {
                    if (await _messageService.ShowAsync("Вы действительно хотите удалить адрес?", "Внимание!",
                        MessageButton.YesNo, MessageImage.Warning) != MessageResult.Yes)
                    {
                        return;
                    }

                    _pleaseWaitService.Show("Удаление адреса...");
                    using (var db = new PSDBContext())
                    {
                        var address = db.Addresses.FirstOrDefault(title => title.Title == SelectedAddress.Title);
                        db.Addresses.Remove(address);
                        db.SaveChanges();
                        AddressesCollection.Clear();
                        db.Addresses.Where(login => login.User.Login == userViewModel.UserLogin).ToList().ForEach(a =>
                        {
                            AddressesCollection.Add(new Address { Title = a.Title });
                        });
                    };
                    AddressesCollection.Remove(SelectedAddress);

                    _pleaseWaitService.Hide();
                },
                () => SelectedAddress != null));
            }
        }
    }
}
