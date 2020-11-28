using System;
using WPFUI.ViewModels;

namespace WPFUI.Views
{
    using Catel.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class ControlRatesView
    {
        public ControlRatesView()
        {
            InitializeComponent();
        }

        public ControlRatesView(ControlRatesViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

    }
}

