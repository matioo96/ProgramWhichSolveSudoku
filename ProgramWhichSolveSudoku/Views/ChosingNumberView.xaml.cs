using ProgramWhichSolveSudoku.ViewModels;
using System.Drawing;

namespace ProgramWhichSolveSudoku.Views
{
    public partial class ChosingNumberView
    {
        public ChosingNumberView(Point point)
        {
            InitializeComponent();
            DataContext = new ChosingNumberViewModel(point);
        }
    }
}
