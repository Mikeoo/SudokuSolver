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

        #region Sudoku Dissection
        private void DissectSudoku()
        {
            ///For Itirating through blocks.
            /// i and j each go through N long steps. Checking a block N by N.
            /// x adds x * N onto i to check the next two blocks over.
            /// Whilst y adds y * N onto j each loop to start one block down after the first row of blocks was tested.
            for (int y = 0; x < N; y++)
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