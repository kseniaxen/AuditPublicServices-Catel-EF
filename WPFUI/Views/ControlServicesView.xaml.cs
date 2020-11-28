using System;
using WPFUI.ViewModels;

namespace WPFUI.Views
{
    using Catel.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class ControlServicesView
    {
        public ControlServicesView()
        {
            InitializeComponent();
        }

        public ControlServicesView(ControlServicesViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

    }
}

