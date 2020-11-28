using Catel.Data;
using Catel.MVVM;
using WPFUI.Models;

namespace WPFUI.ViewModels
{
    public class ServiceViewModel : ViewModelBase
    {
        [Model]
        public Service ServiceObject
        {
            get { return GetValue<Service>(ServiceObjectProperty); }
            set { SetValue(ServiceObjectProperty, value); }
        }
        public static readonly PropertyData ServiceObjectProperty = RegisterProperty("ServiceObject", typeof(Service));


        [ViewModelToModel("ServiceObject", "Title")]
        public string ServiceTitle
        {
            get { return GetValue<string>(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly PropertyData TitleProperty = RegisterProperty("ServiceTitle", typeof(string));
        public ServiceViewModel(Service service = null)
        {
            ServiceObject = service ?? new Service();
        }
    }
}
