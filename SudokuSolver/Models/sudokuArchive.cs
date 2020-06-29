using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SudokuSolver.Models
{
    public class SudokuArchive
    {
        /// <summary>
        /// The sudoku from that point in time.
        /// </summary>
        public int[][] Sudoku { get; set; }

        /// <summary>
        /// The index that was guessed.
        /// </summary>
        public int IndexGuess { get; set; }

        /// <summary>
        /// The number options that can be guessed from.
        /// </summary>
        public int[] GuessOptions { get; set; }

        /// <summary>
        /// The index X of the guessed cell in the jagged array.
        /// </summary>
        public int CellX { get; set; }

        /// <summary>
        /// The index Y from the guessed cell in the jagged array.
        /// </summary>
        public int CellY { get; set; }
    }
}