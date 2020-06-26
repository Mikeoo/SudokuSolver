using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SudokuSolver.Models
{
    public class SudokuArchive
    {
        public int[][] Sudoku { get; set; }
        public int IndexGuess { get; set; }
        public int GuessOptions { get; set; }
    }
}