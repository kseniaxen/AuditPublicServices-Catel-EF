using Catel.Data;
using System.Collections.Generic;

namespace WPFUI.Models
{
    public class User : ValidatableModelBase
    {
        public string Login
        {
            get { return GetValue<string>(LoginProperty); }
            set { SetValue(LoginProperty, value); }
        }
        public static readonly PropertyData LoginProperty = RegisterProperty("Login", typeof(string), string.Empty);


        public string Password
        {
            get { return GetValue<string>(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public static readonly PropertyData PasswordProperty = RegisterProperty("Password", typeof(string), string.Empty);

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrEmpty(Login) || Login.Length > 20)
            {
                validationResults.Add(FieldValidationResult.CreateError(LoginProperty, "Неверный формат ввода логина!"));
            }

            if (string.IsNullOrEmpty(Password))
            {
                validationResults.Add(FieldValidationResult.CreateError(PasswordProperty, "Неверный формат ввода пароля!"));
            }
        }
    }
}
