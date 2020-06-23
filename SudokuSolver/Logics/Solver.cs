using SudokuSolver.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SudokuSolver.Logics
{
    public class Solver
    {
        /// <summary>
        /// N en N2 are stand-ins for the size of the sudoku. N2 being N^2. 
        /// A standard sudoku has N = 3.
        /// Having them as readonly numbers here and using them for itiration instead of hard coding would allow for 
        /// the code to work on sudoku's of other N sizes.
        /// </summary>
        private readonly int N = 3, N2 = 9;
        private int[] arrOptions = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public bool IsSolved;
        public int[][] Solve(int[][] sudoku)
        {
            do
            {
                IsSolved = true;
                sudoku = IterateSudokuSolve(sudoku);
            } while (!IsSolved);

            return sudoku;
        }

        public int[][] SolveGuessing(int[][] sudoku)
        {

                sudoku = IterateSudokuSolve(sudoku);
            return sudoku;
        }

        public int[][] Create(int[][] sudoku)
        {
            return sudoku;
        }
        #region Initialize Variables.
        /// <summary>
        /// Generates ArrOptions 1 to N2 to draw from.
        /// If one did not wish to have it hardcoded.
        /// </summary>
        private void InitializeArrOptions()
        {
            arrOptions = new int[9];
            for (int i = 0; i < N2; i++)
            {
                arrOptions[i] = i + 1;
            }
        }
        #endregion

        #region Sudoku Dissection/Iteration Base Methods

        #region check cell methods
        private void CheckCell(int x, int y)
        {
            for (int i = 0; i < N2; i++)
            {
                //Do something with.
                //sudoku[x][i] for checking one column.
                //sudoku[i][x] for checking one row.
            }
            //Checks which block the cell is in for both x and y and calls Iterateblock with the results.
            IterateBLock(WhichBlock(x), WhichBlock(y));

        }
        /// <summary>
        /// To calculate which block a cell is in.
        /// e.g. x = 5.     x % N = 2.      5 - 2 = 3.  3/3 = 1.
        /// The cell is 1 block to the right.
        /// </summary>
        /// <param name="a">Is the x or y index of the cell.</param>
        /// <returns>The block x or y index.</returns>
        private int WhichBlock(int a)
        {
            int i = (a - (a % N)) / N;
            return i;
        }
        #endregion

        #region Iteration Methods
        private void IterateLoop(int[][] sudoku)
        {
            for (int i = 0; i < N2; i++)
            {
                for (int j = 0; j < N2; j++)
                {
                    //Do something with.
                    //sudoku[i][j] for checking columns.
                    //sudoku[j][i] for checking rows.
                }
            }
        }

        /// <summary>
        /// x and y both loop N times.
        /// To check each block in an N by N Sudoku.
        /// Using IterateBlock method to deligate the actual cell checking.
        /// </summary>
        private void IterateAllBlocks()
        {
            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N; x++)
                {
                    IterateBLock(x, y);
                }
            }
        }

        /// <summary>
        /// For itirating through one singular block of size N by N.
        /// Adding x/y * N to i and j respectively to determine which block to check.
        /// </summary>
        /// <param name="x">Number of blocks horizontal.</param>
        /// <param name="y">Number of block vertical.</param>
        private void IterateBLock(int x, int y)
        {
            for (int i = (0 + (x * N)); i < (N + (N * x)); i++)
            {
                for (int j = (0 + (y * N)); j < (N + (N * y)); j++)
                {
                    //Do something Sudoku[i][j]
                }
            }
        }
        #endregion

        #endregion

        #region Solving methods
        private int[][] IterateSudokuSolve(int[][] sudoku)
        {
            sudoku = IterateLoopSolve(sudoku);
            sudoku = IterateAllBlocksSolve(sudoku);
            return sudoku;
        }

        #region Iteration
        private int[][] IterateLoopSolve(int[][] sudoku)
        {
            for (int i = 0; i < N2; i++)
            {
                for (int j = 0; j < N2; j++)
                {
                    sudoku = CheckSection(sudoku, i, j);
                    sudoku = CheckSection(sudoku, j, i);
                }
            }
            return sudoku;
        }

        /// <summary>
        /// x and y both loop N times.
        /// To check each block in an N by N Sudoku.
        /// Using IterateBlock method to deligate the actual cell checking.
        /// </summary>
        private int[][] IterateAllBlocksSolve(int[][] sudoku)
        {
            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N; x++)
                {
                   sudoku = IterateBLockSolve(sudoku, x, y);
                }
            }
            return sudoku;
        }

        /// <summary>
        /// For itirating through one singular block of size N by N.
        /// Adding x/y * N to i and j respectively to determine which block to check.
        /// </summary>
        /// <param name="x">Number of blocks horizontal.</param>
        /// <param name="y">Number of block vertical.</param>
        private int[][] IterateBLockSolve(int[][] sudoku, int x, int y)
        {
            for (int i = (0 + (x * N)); i < (N + (N * x)); i++)
            {
                for (int j = (0 + (y * N)); j < (N + (N * y)); j++)
                {
                    sudoku = CheckSection(sudoku, i, j);
                }
            }
            return sudoku;
        }
        #endregion

        private int[][] CheckSection(int[][] sudoku, int x, int y)
        {
            if (sudoku[x][y] == 0)
            {
                List<int> tempList = arrOptions.ToList();
                for (int i = 0; i < N2; i++)
                {
                    tempList.Remove(sudoku[x][i]);
                    tempList.Remove(sudoku[i][y]);
                }
                int a = WhichBlock(x);
                int b = WhichBlock(y);

                for (int i = (0 + (a * N)); i < (N + (a * N)); i++)
                {
                    for (int j = (0 + (b * N)); j < (N + (b * N)); j++)
                    {
                        tempList.Remove(sudoku[i][j]);
                    }
                }

                if (tempList.Count == 1)
                    sudoku[x][y] = tempList[0];
                else
                    IsSolved = false;
            }

            return sudoku;
        }
        #endregion  
    }
}