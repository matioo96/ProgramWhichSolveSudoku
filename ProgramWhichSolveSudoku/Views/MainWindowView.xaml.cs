using MahApps.Metro.Controls;
using ProgramWhichSolveSudoku.ViewModels;
using System;
using System.ComponentModel;

namespace ProgramWhichSolveSudoku.Views
{
    public partial class MainWindowView : MetroWindow
    {
        public MainWindowView()
        {
            InitializeComponent();
            DataContext = new MainWindowsViewModel(gridPanel, this);
        }
    }
}
