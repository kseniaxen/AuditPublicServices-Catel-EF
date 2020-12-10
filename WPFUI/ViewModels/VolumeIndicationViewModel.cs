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

        [ViewModelToModel("VIObject", "Id")]
        public int VIId
        {
            get; set;
        }

        [ViewModelToModel("VIObject", "SelectedAddress")]
        public Address VISelectedAddress
        {
            get { return GetValue<Address>(VISelectedAddressProperty); }
            set { SetValue(VISelectedAddressProperty, value); }
        }
        public static readonly PropertyData VISelectedAddressProperty = RegisterProperty("VISelectedAddress", typeof(Address));

        public ObservableCollection<Address> VIAddressesCollection { get; set; }


        [ViewModelToModel("VIObject", "SelectedService")]
        public Service VISelectedService
        {
            get { return GetValue<Service>(VISelectedServiceProperty); }
            set { SetValue(VISelectedServiceProperty, value); }
        }
        public static readonly PropertyData VISelectedServiceProperty = RegisterProperty("VISelectedService", typeof(Service));

        public ObservableCollection<Service> VIServicesCollection { get; set; }


        [ViewModelToModel("VIObject", "SelectedRate")]
        public Rate VISelectedRate
        {
            get { return GetValue<Rate>(VISelectedRateProperty); }
            set { SetValue(VISelectedRateProperty, value); }
        }
        public static readonly PropertyData VISelectedRateProperty = RegisterProperty("VISelectedRate", typeof(Rate));

        public ObservableCollection<Rate> VIRatesCollection { get; set; }

        [ViewModelToModel("VIObject", "PrevIndication")]
        public string VIPrevIndication
        {
            get { return GetValue<string>(PrevIndicationProperty); }
            set { SetValue(PrevIndicationProperty, value); }
        }
        public static readonly PropertyData PrevIndicationProperty = RegisterProperty("VIPrevIndication", typeof(string));

        [ViewModelToModel("VIObject", "CurIndication")]
        public string VICurIndication
        {
            get { return GetValue<string>(CurIndicationProperty); }
            set { SetValue(CurIndicationProperty, value); }
        }
        public static readonly PropertyData CurIndicationProperty = RegisterProperty("VICurIndication", typeof(string));

        [ViewModelToModel("VIObject", "Total")]
        public string VITotal
        {
            get;set;
        }

        [ViewModelToModel("VIObject", "SelectedDate")]
        public DateTime VISelectedDate
        {
            get { return GetValue<DateTime>(SelectedDateProperty); }
            set {
                SetValue(SelectedDateProperty, value); }
        }
        public static readonly PropertyData SelectedDateProperty = RegisterProperty("VISelectedDate", typeof(DateTime));
        public override string Title { get { return "Indication Management"; } }
        public VolumeIndicationViewModel(VolumeIndication volumeIndication = null, ObservableCollection<Address> addressCol=null, ObservableCollection<Service> serviceCol = null, ObservableCollection<Rate> rateCol = null)
        {
            VIObject = volumeIndication ?? new VolumeIndication();
            VIAddressesCollection = addressCol;
            VIServicesCollection = serviceCol;
            VIRatesCollection = rateCol;
            if(volumeIndication == null) VISelectedDate = DateTime.Now;
        }
    }
}
