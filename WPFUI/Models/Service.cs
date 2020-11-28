using Catel.Data;
using System.Collections.Generic;

namespace WPFUI.Models
{
    public class Service : ValidatableModelBase
    {
        public string Title
        {
            get { return GetValue<string>(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly PropertyData TitleProperty = RegisterProperty("Title", typeof(string), string.Empty);

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrEmpty(Title))
            {
                validationResults.Add(FieldValidationResult.CreateError(TitleProperty, "Неверный формат ввода услуги!"));
            }
        }
    }
}
