using ProgramWhichSolveSudoku.Models;
using ProgramWhichSolveSudoku.Views;
using RelayCommandLibrary.Commands;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace ProgramWhichSolveSudoku.ViewModels
{
    class MainWindowsViewModel
    {
        public static string selectedNumber;
        public static List<NumberToGrid> numbersToGrid = new List<NumberToGrid>();

        private int[,,] _sudokuTable;
        private ChosingNumberView _lastChosingNumberView;
        private bool solving = false;
        private static Timer _timer;

        public ICommand SolveCommand { get; set; }
        public ICommand NewSudokuCommand { get; set; }        
        public ICommand RemoveAllCommand { get; set; }

        public MainWindowsViewModel(Grid gridPanel, MainWindowView mainWindowView)
        {
            Grid = gridPanel;
            _sudokuTable = new int[9, 9, 10];
            SolveCommand = new RelayCommand(Solve);
            NewSudokuCommand = new RelayCommand(NewSudoku, CanRestart);
            RemoveAllCommand = new RelayCommand(RemoveAll);
            mainWindowView.Closing += OnApplicationExit;

            for (int i = 0; i < 9; i++)
            {
                Grid.ColumnDefinitions.Add(new ColumnDefinition());
                Grid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    System.Windows.Controls.Button button = new System.Windows.Controls.Button();
                    if ((i == 3 && j == 3) || (i == 3 && j == 6) || (i == 6 && j == 3) ||
                        (i == 6 && j == 6))
                        button.Margin = new Thickness(5, 5, 0, 0);
                    else if (i == 3 || i == 6)
                        button.Margin = new Thickness(5, 0, 0, 0);
                    else if (j == 3 || j == 6)
                        button.Margin = new Thickness(0, 5, 0, 0);
                    else
                        button.Margin = new Thickness(0);
                    button.Tag = new Coordinates { Row = i, Column = j };
                    button.Click += new RoutedEventHandler(ChosingNumber);
                    button.Background = Brushes.DarkGray;
                    button.Opacity = 0.8;
                    button.Height = 50;
                    button.Width = 50;
                    button.FontSize = 25;
                    Grid.Children.Add(button);
                    Grid.SetColumn(button, i);
                    Grid.SetRow(button, j);
                }
            }
        }

        private Grid grid;

        public Grid Grid
        {
            get { return grid; }
            set { grid = value; }
        }

        private System.Windows.Controls.Button button;

        public System.Windows.Controls.Button Button
        {
            get { return button; }
            set { button = value; }
        }

        private void ChosingNumber(object sender, RoutedEventArgs e)
        {
            if (_lastChosingNumberView != null)
            {
                _lastChosingNumberView.Close();
                _lastChosingNumberView = null;
            }
            selectedNumber = string.Empty;
            Button = sender as System.Windows.Controls.Button;
            var t = System.Windows.Forms.Cursor.Position;
            var chosingNumberView = new ChosingNumberView(t);
            _lastChosingNumberView = chosingNumberView;
            chosingNumberView.Closed += chosingNumberView_Closed;
            chosingNumberView.Show();
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            if (_lastChosingNumberView != null)
            {
                _lastChosingNumberView.Close();
                _lastChosingNumberView = null;
            }
        }

        private void RemoveAll(object obj)
        {
            foreach (var children in Grid.Children)
            {
                var button = children as System.Windows.Controls.Button;
                button.Content = null;
            }
        }

        private void Solve(object obj)
        {
            solving = true;
            TurnOffAllButtons();
            FillInTable();
            var numbersInTab = CalculateQuantityNumbersInTab();
            var quantity = 0;
            while (true)
            {
                MethodsForTable.DeleteOfFields(_sudokuTable);
                FindSingleNumbers();

                MethodsForTable.DeleteOfFields(_sudokuTable);
                FindSingleNumbersIn3x3();

                MethodsForTable.DeleteOfFields(_sudokuTable);
                FindSingleNumbersInLine();

                if (numbersInTab + numbersToGrid.Count == 81 || 
                    numbersInTab + numbersToGrid.Count == quantity)
                    break;
                quantity = numbersInTab + numbersToGrid.Count;
            }
            if (numbersInTab + numbersToGrid.Count != 81)
            {
                var newsudokuTable = MethodsForTable.GetCopiesTable(_sudokuTable);
                MethodsForSolve.BruteForceSolveMethod(newsudokuTable);
            }
            _timer = new Timer
            {
                Interval = 250
            };
            _timer.Tick += new EventHandler(Fill);
            _timer.Start();            
        }

        private void Fill(object sender, EventArgs e)
        {
            var random = new Random();
            var index = random.Next(0, numbersToGrid.Count - 1);
            foreach (var children in Grid.Children)
            {
                var button = children as System.Windows.Controls.Button;
                Coordinates coordinates = button.Tag as Coordinates;
                var number = numbersToGrid[index];
                if (coordinates.Column == number.Coordinates.Column && coordinates.Row == number.Coordinates.Row)
                {
                    button.Content = number.Number;
                    System.Windows.Forms.Application.DoEvents();
                }
            }
            numbersToGrid.RemoveAt(index);
            if (numbersToGrid.Count == 0)
            {
                _timer.Stop();
                solving = false;
                System.Windows.Forms.Application.DoEvents();
            }
        }

        private void NewSudoku(object obj)
        {
            TurnOnAllButtons();
        }

        private bool CanRestart(object obj)
        {
            return !solving;
        }

        private int CalculateQuantityNumbersInTab()
        {
            var quantity = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (_sudokuTable[i, j, 0] > 0)
                        quantity++;
                }
            }
            return quantity;
        }

        private void FindSingleNumbersInLine()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int k = 1; k <= 9; k++)
                {
                    var count = 0;
                    var number = 0;
                    var coordinates = new Coordinates();
                    for (int x = 0; x < 9; x++)
                    {
                        if (_sudokuTable[i, x, k] == k)
                        {
                            count++;
                            number = _sudokuTable[i, x, k];
                            coordinates = new Coordinates() { Row = i, Column = x };
                        }
                    }
                    if (count == 1)
                        SetNumber(coordinates, number);
                }
            }
            for (int j = 0; j < 9; j++)
            {
                for (int k = 1; k <= 9; k++)
                {
                    var count = 0;
                    var number = 0;
                    var coordinates = new Coordinates();
                    for (int x = 0; x < 9; x++)
                    {
                        if (_sudokuTable[x, j, k] == k)
                        {
                            count++;
                            number = _sudokuTable[x, j, k];
                            coordinates = new Coordinates() { Row = x, Column = j };
                        }
                    }
                    if (count == 1)
                        SetNumber(coordinates, number);
                }
            }
        }

        private void FindSingleNumbersIn3x3()
        {
            for (int x = 1; x <= 9; x++)
            {
                // 1
                var count = 0;
                var number = 0;
                var coordinates = new Coordinates();
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                    {
                        if (_sudokuTable[i, j, x] > 0)
                        {
                            count++;
                            number = _sudokuTable[i, j, x];
                            coordinates = new Coordinates() { Row = i, Column = j };
                        }
                    }
                if (count == 1)
                    SetNumber(coordinates, number);
                // 2
                count = 0;
                for (int i = 3; i < 6; i++)
                    for (int j = 0; j < 3; j++)
                    {
                        if (_sudokuTable[i, j, x] > 0)
                        {
                            count++;
                            number = _sudokuTable[i, j, x];
                            coordinates = new Coordinates() { Row = i, Column = j };
                        }
                    }
                if (count == 1)
                    SetNumber(coordinates, number);
                //3
                count = 0;
                for (int i = 6; i < 9; i++)
                    for (int j = 0; j < 3; j++)
                    {
                        if (_sudokuTable[i, j, x] > 0)
                        {
                            count++;
                            number = _sudokuTable[i, j, x];
                            coordinates = new Coordinates() { Row = i, Column = j };
                        }
                    }
                if (count == 1)
                    SetNumber(coordinates, number);
                // 4
                count = 0;
                for (int i = 0; i < 3; i++)
                    for (int j = 3; j < 6; j++)
                    {
                        if (_sudokuTable[i, j, x] > 0)
                        {
                            count++;
                            number = _sudokuTable[i, j, x];
                            coordinates = new Coordinates() { Row = i, Column = j };
                        }
                    }
                if (count == 1)
                    SetNumber(coordinates, number);
                // 5
                count = 0;
                for (int i = 3; i < 6; i++)
                    for (int j = 3; j < 6; j++)
                    {
                        if (_sudokuTable[i, j, x] > 0)
                        {
                            count++;
                            number = _sudokuTable[i, j, x];
                            coordinates = new Coordinates() { Row = i, Column = j };
                        }
                    }
                if (count == 1)
                    SetNumber(coordinates, number);
                // 6
                count = 0;
                for (int i = 6; i < 9; i++)
                    for (int j = 3; j < 6; j++)
                    {
                        if (_sudokuTable[i, j, x] > 0)
                        {
                            count++;
                            number = _sudokuTable[i, j, x];
                            coordinates = new Coordinates() { Row = i, Column = j };
                        }
                    }
                if (count == 1)
                    SetNumber(coordinates, number);
                // 7
                count = 0;
                for (int i = 0; i < 3; i++)
                    for (int j = 6; j < 9; j++)
                    {
                        if (_sudokuTable[i, j, x] > 0)
                        {
                            count++;
                            number = _sudokuTable[i, j, x];
                            coordinates = new Coordinates() { Row = i, Column = j };
                        }
                    }
                if (count == 1)
                    SetNumber(coordinates, number);
                // 8
                count = 0;
                for (int i = 3; i < 6; i++)
                    for (int j = 6; j < 9; j++)
                    {
                        if (_sudokuTable[i, j, x] > 0)
                        {
                            count++;
                            number = _sudokuTable[i, j, x];
                            coordinates = new Coordinates() { Row = i, Column = j };
                        }
                    }
                if (count == 1)
                    SetNumber(coordinates, number);
                // 9
                count = 0;
                for (int i = 6; i < 9; i++)
                    for (int j = 6; j < 9; j++)
                    {
                        if (_sudokuTable[i, j, x] > 0)
                        {
                            count++;
                            number = _sudokuTable[i, j, x];
                            coordinates = new Coordinates() { Row = i, Column = j };
                        }
                    }
                if (count == 1)
                    SetNumber(coordinates, number);
            }
        }

        private void FindSingleNumbers()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    var quantity = 0;
                    int number = 0;
                    if (_sudokuTable[i, j, 0] == 0)
                    {
                        for (int k = 1; k <= 9; k++)
                        {
                            if (_sudokuTable[i, j, k] != 0)
                            {
                                quantity++;
                                number = _sudokuTable[i, j, k];
                            }
                        }
                    }
                    if (quantity == 1)
                        SetNumber(new Coordinates() { Row = i, Column = j }, number);
                }
            }
        }

        private void SetNumber(Coordinates coordinates, int number)
        {
            _sudokuTable[coordinates.Row, coordinates.Column, 0] = number;
            numbersToGrid.Add(new NumberToGrid()
            {
                Coordinates = coordinates,
                Number = number
            });
        }

        private void FillInTable()
        {
            foreach (var children in Grid.Children)
            {
                var button = children as System.Windows.Controls.Button;
                Coordinates coordinates = button.Tag as Coordinates;
                _sudokuTable[coordinates.Row, coordinates.Column, 0] = Convert.ToInt32(button.Content);
            }
            MethodsForTable.SetNumbers(_sudokuTable);
        }

        private void TurnOffAllButtons()
        {
            foreach (var children in Grid.Children)
            {
                var button = children as System.Windows.Controls.Button;
                button.IsEnabled = false;
                button.Foreground = Brushes.Black;
            }
        }

        private void TurnOnAllButtons()
        {
            _sudokuTable = new int[9, 9, 10];
            foreach (var children in Grid.Children)
            {
                var button = children as System.Windows.Controls.Button;
                button.IsEnabled = true;
                button.Foreground = Brushes.White;
                button.Content = null;
            }
        }

        private void chosingNumberView_Closed(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedNumber))
            {
                if (selectedNumber == " ")
                    Button.Content = null;
                else
                    Button.Content = selectedNumber;
                selectedNumber = string.Empty;
            }
        }
    }
}
