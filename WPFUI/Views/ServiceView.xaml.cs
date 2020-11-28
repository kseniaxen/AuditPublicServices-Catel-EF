using Catel.Windows;
using WPFUI.ViewModels;

namespace WPFUI.Views
{
    public partial class ServiceView : DataWindow
    {
        public ServiceView()
        {
            InitializeComponent();
        }

        public ServiceView(ServiceViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

    }
}