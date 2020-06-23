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
        public int[][] Solve(int[][] sudoku)
        {
            return sudoku;
        }

        public int[][] SolveGuessing(int[][] sudoku)
        {
            return sudoku;
        }

        public int[][] Create(int[][] sudoku)
        {
            return sudoku;
        }

        #region Sudoku Dissection/Itiration
        private void IterateLoop(int [][] sudoku)
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
        private void CheckCell(int x, int y)
        {
            for (int i = 0; i < N2; i++)
            {
                //Do something with.
                //sudoku[x][i] for checking one column.
                //sudoku[i][x] for checking one row.
            }
            int a = WhichBlock(x);
            int b = WhichBlock(y);
            
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

        private void DissectSudoku()
        {
            ///For Itirating through blocks.
            /// i and j each go through N long steps. Checking a block N by N.
            /// x adds x * N onto i to check the next two blocks over.
            /// Whilst y adds y * N onto j each loop to start one block down after the first row of blocks was tested.
            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N; x++)
                {
                    for (int i = (0 + (x * N)); i < (N + (N * x)); i++)
                    {
                        for (int j = (0 + (y * N)); j < (N + (N * y)); j++)
                        {

                        }
                    }
                }
            }
        }

        #endregion
    }
}