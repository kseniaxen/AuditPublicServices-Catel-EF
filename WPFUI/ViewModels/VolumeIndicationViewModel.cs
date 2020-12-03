using Catel.Data;
using Catel.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WPFUI.Models;

namespace WPFUI.ViewModels
{
    public class VolumeIndicationViewModel: ViewModelBase
    {
        [Model]
        public VolumeIndication VIObject
        {
            get { return GetValue<VolumeIndication>(VIObjectProperty); }
            set { SetValue(VIObjectProperty, value); }
        }
        public static readonly PropertyData VIObjectProperty = RegisterProperty("VIObject", typeof(VolumeIndication));

        [ViewModelToModel("VIObject", "SelectedAddress")]
        public Address VISelectedAddress
        {
            get { return GetValue<Address>(VISelectedAddressProperty); }
            set { SetValue(VISelectedAddressProperty, value); }
        }
        public static readonly PropertyData VISelectedAddressProperty = RegisterProperty("VISelectedAddress", typeof(Address));

        [ViewModelToModel("VIObject", "AddressesCollection")]
        public ObservableCollection<Address> VIAddressesCollection { get; set; }

        [ViewModelToModel("VIObject", "SelectedService")]
        public Service VISelectedService
        {
            get { return GetValue<Service>(VISelectedServiceProperty); }
            set { SetValue(VISelectedServiceProperty, value); }
        }
        public static readonly PropertyData VISelectedServiceProperty = RegisterProperty("VISelectedService", typeof(Service));

        [ViewModelToModel("VIObject", "ServicesCollection")]
        public ObservableCollection<Service> VIServicesCollection { get; set; }
        public VolumeIndicationViewModel(VolumeIndication volumeIndication = null, ObservableCollection<Address> addressCol=null, ObservableCollection<Service> serviceCol = null)
        {
            VIObject = volumeIndication ?? new VolumeIndication(addressCol, serviceCol);
        }
    }
}
