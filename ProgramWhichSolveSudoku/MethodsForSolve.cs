using ProgramWhichSolveSudoku.Models;
using ProgramWhichSolveSudoku.ViewModels;
using System.Collections.Generic;

namespace ProgramWhichSolveSudoku
{
    public static class MethodsForSolve
    {
        private static List<NumberToGrid> _numbersToGrid;
        public static void BruteForceSolveMethod(int[,,] newSudokuTable)
        {
            _numbersToGrid = new List<NumberToGrid>();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (newSudokuTable[i, j, 0] == 0)
                        _numbersToGrid.Add(new NumberToGrid()
                        {
                            Coordinates = new Coordinates()
                            { Row = i, Column = j }
                        });
                }
            }
            var iteration = 0;
            if (_numbersToGrid.Count == 0)
                return;
            do
            {
                MethodsForTable.SetNumbers(newSudokuTable);
                MethodsForTable.DeleteOfFields(newSudokuTable);
                var numberTogrid = _numbersToGrid[iteration];
                var good = false;
                for (int k = numberTogrid.Number + 1; k < 10; k++)
                {
                    if (newSudokuTable[numberTogrid.Coordinates.Row, numberTogrid.Coordinates.Column, k] == 0)
                        continue;
                    good = true;
                    var number = newSudokuTable[numberTogrid.Coordinates.Row, numberTogrid.Coordinates.Column, k];
                    _numbersToGrid[iteration].Number = number;
                    newSudokuTable[numberTogrid.Coordinates.Row, numberTogrid.Coordinates.Column, 0] = number;
                    break;
                }
                if (!good)
                {
                    newSudokuTable[numberTogrid.Coordinates.Row, numberTogrid.Coordinates.Column, 0] = 0;
                    if (iteration > 0)
                        iteration--;
                    numberTogrid = _numbersToGrid[iteration];
                    newSudokuTable[numberTogrid.Coordinates.Row, numberTogrid.Coordinates.Column, 0] = 0;
                    for (int i = iteration + 1; i < _numbersToGrid.Count - 1; i++)
                    {
                        _numbersToGrid[i].Number = 0;
                    }
                }
                else
                {
                    iteration++;
                }
            } while (_numbersToGrid[_numbersToGrid.Count - 1].Number == 0);
            foreach (var number in _numbersToGrid)
            {
                MainWindowsViewModel.numbersToGrid.Add(number);
            }
        }
    }
}
