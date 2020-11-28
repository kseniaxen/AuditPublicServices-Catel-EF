using Catel.Data;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace WPFUI.Models
{
    public class Rate : ValidatableModelBase
    {
        public string Title
        {
            get { return GetValue<string>(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly PropertyData TitleProperty = RegisterProperty("Title", typeof(string), string.Empty);
        public string MeasureTitle
        {
            get { return GetValue<string>(MeasureTitleProperty); }
            set { SetValue(MeasureTitleProperty, value); }
        }
        public static readonly PropertyData MeasureTitleProperty = RegisterProperty("MeasureTitle", typeof(string), string.Empty);
        public string Price
        {
            get { return GetValue<string>(PriceProperty); }
            set { SetValue(PriceProperty, value); }
        }
        public static readonly PropertyData PriceProperty = RegisterProperty("Price", typeof(string), string.Empty);

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrEmpty(Title))
            {
                validationResults.Add(FieldValidationResult.CreateError(TitleProperty, "Неверный формат ввода названия!"));
            }

            if (string.IsNullOrEmpty(MeasureTitle))
            {
                validationResults.Add(FieldValidationResult.CreateError(MeasureTitleProperty, "Неверный формат ввода единицы измерения!"));
            }
            decimal output;
            if (!Decimal.TryParse(Price,out output))
            {
                validationResults.Add(FieldValidationResult.CreateError(PriceProperty, "Неверный формат ввода цены!"));
            }
        }
    }
}
