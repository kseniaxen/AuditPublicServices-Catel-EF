using Catel.Windows;
using WPFUI.ViewModels;

namespace WPFUI.Views
{
    public partial class RateView : DataWindow
    {
        public RateView()
        {
            InitializeComponent();
        }

        public RateView(RateViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

    }
}