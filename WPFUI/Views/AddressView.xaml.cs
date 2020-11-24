using Catel.Windows;
using WPFUI.ViewModels;

namespace WPFUI.Views
{
    public partial class AddressView : DataWindow
    {
        public AddressView()
        {
            InitializeComponent();
        }

        public AddressView(AddressViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

    }
}