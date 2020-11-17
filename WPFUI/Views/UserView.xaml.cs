using Catel.Windows;
using WPFUI.ViewModels;

namespace WPFUI.Views
{
    public partial class UserView : DataWindow
    {
        public UserView()
        {
            InitializeComponent();
        }

        public UserView(UserViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

    }
}
