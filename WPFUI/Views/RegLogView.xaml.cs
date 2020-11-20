using System;
using WPFUI.ViewModels;

namespace WPFUI.Views
{
    using Catel.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class RegLogView
    {
        public RegLogView()
        {
            InitializeComponent();
        }

        public RegLogView(RegLogViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

    }
}
