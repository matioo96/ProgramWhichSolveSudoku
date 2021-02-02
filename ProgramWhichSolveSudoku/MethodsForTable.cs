using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramWhichSolveSudoku
{
    public static class MethodsForTable
    {
        public static void SetNumbers(int[,,] sudokuTable)
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    for (int k = 1; k <= 9; k++)
                        sudokuTable[i, j, k] = k;
        }
        public static void DeleteOfFields(int[,,] sudokuTable)
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    if (sudokuTable[i, j, 0] != 0)
                    {
                        for (int x = 0; x < 9; x++)
                        {
                            sudokuTable[i, j, x + 1] = 0;
                            sudokuTable[x, j, sudokuTable[i, j, 0]] = 0; //X
                            sudokuTable[i, x, sudokuTable[i, j, 0]] = 0; //Y
                        }
                        if (i < 3 & j < 3)  //1
                            for (int x = 0; x < 3; x++)
                                for (int y = 0; y < 3; y++)
                                    sudokuTable[x, y, sudokuTable[i, j, 0]] = 0;
                        else if (i >= 3 & j < 3 & i < 6) //2
                            for (int x = 3; x < 6; x++)
                                for (int y = 0; y < 3; y++)
                                    sudokuTable[x, y, sudokuTable[i, j, 0]] = 0;
                        else if (i >= 6 & j < 3) //3
                            for (int x = 6; x < 9; x++)
                                for (int y = 0; y < 3; y++)
                                    sudokuTable[x, y, sudokuTable[i, j, 0]] = 0;
                        else if (i < 3 & j >= 3 & j < 6) //4
                            for (int x = 0; x < 3; x++)
                                for (int y = 3; y < 6; y++)
                                    sudokuTable[x, y, sudokuTable[i, j, 0]] = 0;
                        else if (i >= 3 & i < 6 & j >= 3 & j < 6) //5
                            for (int x = 3; x < 6; x++)
                                for (int y = 3; y < 6; y++)
                                    sudokuTable[x, y, sudokuTable[i, j, 0]] = 0;
                        else if (i >= 6 & j >= 3 & j < 6) //6
                            for (int x = 6; x < 9; x++)
                                for (int y = 3; y < 6; y++)
                                    sudokuTable[x, y, sudokuTable[i, j, 0]] = 0;
                        else if (i < 3 & j >= 6) //7
                            for (int x = 0; x < 3; x++)
                                for (int y = 6; y < 9; y++)
                                    sudokuTable[x, y, sudokuTable[i, j, 0]] = 0;
                        else if (i >= 3 & i < 6 & j >= 6) //8
                            for (int x = 3; x < 6; x++)
                                for (int y = 6; y < 9; y++)
                                    sudokuTable[x, y, sudokuTable[i, j, 0]] = 0;
                        else if (i >= 6 & j >= 6) //9
                            for (int x = 6; x < 9; x++)
                                for (int y = 6; y < 9; y++)
                                    sudokuTable[x, y, sudokuTable[i, j, 0]] = 0;
                    }
                }
        }

        public static int[,,] GetCopiesTable(int[,,] table)
        {
            var newTab = new int[9, 9, 10];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        newTab[i, j, k] = table[i, j, k];
                    }
                }
            }
            return newTab;
        }
    }
}
