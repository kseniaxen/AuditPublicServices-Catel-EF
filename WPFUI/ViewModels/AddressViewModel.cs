using Catel.Data;
using Catel.MVVM;
using System;
using System.Collections.Generic;
using System.Text;
using WPFUI.Models;

namespace WPFUI.ViewModels
{
    public class AddressViewModel : ViewModelBase
    {
        [Model]
        public Address AddressObject
        {
            get { return GetValue<Address>(AddressObjectProperty); }
            set { SetValue(AddressObjectProperty, value); }
        }
        public static readonly PropertyData AddressObjectProperty = RegisterProperty("AddressObject", typeof(Address));


        [ViewModelToModel("AddressObject", "Title")]
        public string AddressTitle
        {
            get { return GetValue<string>(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly PropertyData TitleProperty = RegisterProperty("AddressTitle", typeof(string));
        public AddressViewModel(Address address = null)
        {
            AddressObject = address ?? new Address();
        }
    }
}
