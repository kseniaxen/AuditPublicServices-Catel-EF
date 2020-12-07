using Catel.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace WPFUI.Models
{
    public class VolumeIndication : ValidatableModelBase
    {
        public int Id { get; set; }
        public Address SelectedAddress 
        { 
            get { return GetValue<Address>(SelectedAddressProperty); }
            set { SetValue(SelectedAddressProperty, value); }
        }
        public static readonly PropertyData SelectedAddressProperty = RegisterProperty("SelectedAddress", typeof(Address));

        public Service SelectedService
        {
            get { return GetValue<Service>(SelectedServiceProperty); }
            set { SetValue(SelectedServiceProperty, value); }
        }
        public static readonly PropertyData SelectedServiceProperty = RegisterProperty("SelectedService", typeof(Service));

        public Rate SelectedRate
        {
            get { return GetValue<Rate>(SelectedRateProperty); }
            set { SetValue(SelectedRateProperty, value); }
        }
        public static readonly PropertyData SelectedRateProperty = RegisterProperty("SelectedRate", typeof(Rate));

        public string PrevIndication
        {
            get { return GetValue<string>(PrevIndicationProperty); }
            set { SetValue(PrevIndicationProperty, value); }
        }
        public static readonly PropertyData PrevIndicationProperty = RegisterProperty("PrevIndication", typeof(string));

        public string CurIndication
        {
            get { return GetValue<string>(CurIndicationProperty); }
            set { SetValue(CurIndicationProperty, value); }
        }
        public static readonly PropertyData CurIndicationProperty = RegisterProperty("CurIndication", typeof(string));

        public string Total { get;set; }

        public DateTime SelectedDate
        {
            get { return GetValue<DateTime>(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }
        public static readonly PropertyData SelectedDateProperty = RegisterProperty("SelectedDate", typeof(DateTime));

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

            if (SelectedRate == null)
            {
                validationResults.Add(FieldValidationResult.CreateError(SelectedRateProperty, "Не указан тариф!"));
            }

            int output1;
            if(!Int32.TryParse(PrevIndication, out output1))
            {
                validationResults.Add(FieldValidationResult.CreateError(PrevIndicationProperty, "Неверный формат ввода пред. показания!"));
            }

            int output2;
            if (!Int32.TryParse(CurIndication, out output2))
            {
                validationResults.Add(FieldValidationResult.CreateError(CurIndicationProperty, "Неверный формат ввода текущ. показания!"));
            }

            if (DateTime.MinValue == SelectedDate)
            {
                validationResults.Add(FieldValidationResult.CreateError(SelectedDateProperty, "Не указана дата!"));
            }
        }

    }
}
