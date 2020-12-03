using Catel.Windows;
using WPFUI.ViewModels;

namespace WPFUI.Views
{
    public partial class VolumeIndicationView : DataWindow
    {
        public VolumeIndicationView()
        {
            InitializeComponent();
        }

        public VolumeIndicationView(VolumeIndicationViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

    }
}
