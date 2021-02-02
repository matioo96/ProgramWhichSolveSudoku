using RelayCommandLibrary.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace ProgramWhichSolveSudoku.ViewModels
{
    class ChosingNumberViewModel
    {
        private System.Drawing.Point _point;
        public ICommand SelectNumberCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ChosingNumberViewModel(System.Drawing.Point point)
        {
            _point = point;
            SelectNumberCommand = new RelayCommand(SelectNumber);
            RemoveCommand = new RelayCommand(Remove);
        }

        public double Left
        {
            get { return _point.X; }
            set { _point.X = Convert.ToInt32(value); }
        }
        public double Top
        {
            get { return _point.Y; }
            set { _point.Y = Convert.ToInt32(value); }
        }

        private static Window window;

        public static Window Window
        {
            get { return window; }
            set { window = value; }
        }

        private void SelectNumber(object obj)
        {
            MainWindowsViewModel.selectedNumber = obj.ToString();
            Window.Close();
        }

        private void Remove(object obj)
        {
            MainWindowsViewModel.selectedNumber = " ";
            Window.Close();
        }
    }
}
