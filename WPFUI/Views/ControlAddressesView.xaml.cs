using System;
using WPFUI.ViewModels;

namespace WPFUI.Views
{
    using Catel.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class ControlAddressesView
    {
        public ControlAddressesView()
        {
            InitializeComponent();
        }

        public ControlAddressesView(ControlAddressesViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

    }
}

