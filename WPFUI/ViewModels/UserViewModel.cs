using Catel.Data;
using Catel.MVVM;
using System;
using System.Collections.Generic;
using System.Text;
using WPFUI.Models;

namespace WPFUI.ViewModels
{
    public class UserViewModel: ViewModelBase
    {
        [Model]
        public User UserObject
        {
            get { return GetValue<User>(UserObjectProperty); }
            set { SetValue(UserObjectProperty, value); }
        }
        public static readonly PropertyData UserObjectProperty = RegisterProperty("UserObject", typeof(User));


        [ViewModelToModel("UserObject", "Login")]
        public string UserLogin
        {
            get { return GetValue<string>(LoginProperty); }
            set { SetValue(LoginProperty, value); }
        }
        public static readonly PropertyData LoginProperty = RegisterProperty("UserLogin", typeof(string));


        [ViewModelToModel("UserObject", "Password")]
        public string UserPassword
        {
            get { return GetValue<string>(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public static readonly PropertyData PasswordProperty = RegisterProperty("UserPassword", typeof(string));
        public override string Title { get { return "Audit Public Services"; } }
        public UserViewModel(User user = null)
        {
            UserObject = user ?? new User();
        }
    }
}
