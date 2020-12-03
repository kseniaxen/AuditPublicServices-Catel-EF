using Catel.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WPFUI.Models
{
    public class VolumeIndication : ValidatableModelBase
    {
        public Address SelectedAddress 
        { 
            get { return GetValue<Address>(SelectedAddressProperty); }
            set { SetValue(SelectedAddressProperty, value); }
        }
        public static readonly PropertyData SelectedAddressProperty = RegisterProperty("SelectedAddress", typeof(Address));

        public ObservableCollection<Address> AddressesCollection { get; set; }

        public Service SelectedService
        {
            get { return GetValue<Service>(SelectedServiceProperty); }
            set { SetValue(SelectedServiceProperty, value); }
        }
        public static readonly PropertyData SelectedServiceProperty = RegisterProperty("SelectedService", typeof(Service));

        public ObservableCollection<Service> ServicesCollection { get; set; }

        public VolumeIndication(ObservableCollection<Address> addCol, ObservableCollection<Service> serCol)
        {
            AddressesCollection = addCol;
            ServicesCollection = serCol;
        }

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (SelectedAddress == null)
            {
                validationResults.Add(FieldValidationResult.CreateError(SelectedAddressProperty, "Не указан адрес!"));
            }
            if (SelectedService == null)
            {
                validationResults.Add(FieldValidationResult.CreateError(SelectedServiceProperty, "Не указана услуга!"));
            }
        }

    }
}
