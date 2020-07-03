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
        private static readonly int N = 3, N2 = 9;

        /// <summary>
        /// countG is to keep track of at which number of potential numbers to guess the computer can "guess" one.
        /// Setting countG to highest (9) before attempting to Solve.
        /// </summary>
        private int countG;
        /// <summary>
        /// Tto keep track of which index was already guessed in combination with the sudoku archives.
        /// </summary>
        private int indexG;
        private int[] arrOptions = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        /// <summary>
        /// Notes if a new archive needs to be pulled from list.
        /// Based on if a cell is found that needs to be filled but has no options.
        /// </summary>
        public bool IsSolved, IsNewGuess, IsValidArchive;
        public bool NewArchive = false;
        public List<SudokuArchive> sudokuArchives;

        public int[][] Solve(int[][] sudoku)
        {
            //Set count to 1 to not allow for guessing.
            countG = 1;
            int attempts = 0;
            do
            {
                sudoku = IterateSudokuSolve(sudoku);
                CheckIsSolved(sudoku);

                //Keep track of attempts and break out of loop after more than 3.
                attempts++;
                if (attempts > 3)
                    break;
            } while (!IsSolved);

            return sudoku;
        }

        public int[][] SolveGuessing(int[][] sudoku)
        {
                //Make new Archives list to aid with guessing.
                sudokuArchives = new List<SudokuArchive>();
                //Reset indexG to 0
                indexG = 0;

            do
            {
                if (!NewArchive)
                    sudoku = IterateSudokuGuess(sudoku);
                if (IsNewGuess && !NewArchive)
                    sudoku = IterateSudokuSolve(sudoku);
                else
                {
                    //If no new guess number can be found that means all options have been tried and the sudoku is not solved.
                    //Hence go to the last entry of sudoku archives and make the sudoku that sudoku and change the index guess.
                    do
                    {
                        int tempIndex = sudokuArchives.Count - 1;
                        Debug.WriteLine("Archives Count: " + sudokuArchives.Count);

                        //Add 1 to the IndexGuess and set IndexG to current IndexG.
                        sudokuArchives[tempIndex].IndexGuess++;
                        indexG = sudokuArchives[tempIndex].IndexGuess;

                 

                        //If indexG is equal or bigger to GuessOptions that means all guesses have been tried.
                        //Meaning a prior archived sudoku should be used instead.
                        if (indexG >= sudokuArchives[tempIndex].GuessOptions.Length)
                        {
                            IsValidArchive = false;
                            //Remove archive from list.
                            sudokuArchives.RemoveAt(tempIndex);
                        }
                        else
                        {
                            //If it is a valid archive change bool and set sudoku to archived sudoku.
                            IsValidArchive = true;
                            NewArchive = false;
                            sudoku = sudokuArchives[tempIndex].Sudoku;

                            //Set the cell from the archives that was guessed to the next possible option.
                            sudoku[sudokuArchives[tempIndex].CellX][sudokuArchives[tempIndex].CellY] = sudokuArchives[tempIndex].GuessOptions[indexG];

                            //Set indexG back to 0 in case new guesses are made after this archive point.
                            indexG = 0;
                        }
                    } while (!IsValidArchive);
                }
                CheckIsSolved(sudoku);
            }while (!IsSolved);
  
            return sudoku;
        }

        //Variables needed for creation.
        private Random Rnd = new Random();

        public int[][] Create(int[][] sudoku)
        {
            sudoku = CreateFilledSudoku(sudoku);
            sudoku = RemoveCells(sudoku);
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

        #region support methods
        /// <summary>
        /// To calculate which block a cell is in.
        /// e.g. x = 5.     x % N = 2.      5 - 2 = 3.  3/3 = 1.
        /// The cell is 1 block to the right.
        /// </summary>
        /// <param name="a">Is the x or y index of the cell.</param>
        /// <returns>The block x or y index.</returns>
        static int WhichBlock(int a)
        {
            int i = (a - (a % N)) / N;
            return i;
        }

        /// <summary>
        /// Returns new jagged array copied from input value.
        /// </summary>
        /// <param name="sourceArr">The array to be copied.</param>
        /// <returns></returns>
        static int[][] CopyArrayJagged(int[][] sourceArr)
        {
            int length = sourceArr.Length;
            int[][] outputArr = new int[length][];

            for (var x = 0; x < length; x++)
            {
                int[] innerArr = sourceArr[x];
                int inLength = innerArr.Length;
                int[] newInnerArr = new int[inLength];
                Array.Copy(innerArr, newInnerArr, inLength);
                outputArr[x] = newInnerArr;
            }

            return outputArr;
        }

        private void CheckIsSolved(int[][] sudoku)
        {
            IsSolved = true;
            for (int i = 0; i < N2; i++)
            {
                for (int j = 0; j < N2; j++)
                {
                    if (sudoku[i][j] == 0)
                        IsSolved = false;
                }
            }
        }

        private void EmptySudoku(int[][] sudoku)
        {
            for (int i = 0; i < N2; i++)
            {
                for (int j = 0; j < N2; j++)
                {
                    sudoku[i][j] = 0;
                }
            }
        }
        #endregion

        #region Creation Methods
        private int[][] CreateFilledSudoku(int[][] sudoku)
        {
                EmptySudoku(sudoku);
                //Create list 1 to 9 to place 9 random numbers at 9 random heights.
                List<int> tempListA = arrOptions.ToList();
                for (int j = 0; j < N2; j++)
                {
                    int tempInt = Rnd.Next(0, tempListA.Count);
                    int tempIntB = Rnd.Next(0, N2);
                    sudoku[tempIntB][j] = tempListA[tempInt];
                    tempListA.RemoveAt(tempInt);
                }
                //Then call SolveGuessing to fill in remainder of Sudoku.
                sudoku = SolveGuessing(sudoku);

            return sudoku;
        }

        private int[][] RemoveCells(int[][] sudoku)
        {
            int[][] backupSudoku, newSudoku;
            int rndNum, rndIndex, attempt = 0, count = 0;
            List<int> columnsList = arrOptions.ToList();

            //Remove cells, check if solveable. Until 3 attempts were it was not solvable anymore.
            //Then return sudoku.
            do
            {
                //Makes backup of sudoku before removal.
                backupSudoku = CopyArrayJagged(sudoku);

                //First 4 attempts. Set one randomly selected column all to 0.
                if (attempt < 4)
                {
                    //Generate rndIndex based on length rowslist. 
                    rndIndex= Rnd.Next(0, columnsList.Count);
                    //rndNum is number at index in rowslist - 1 to adjust number to correct indexing position for array.
                    rndNum = columnsList[rndIndex] - 1;
                    //Remove selected column from list using rndIndex.
                    columnsList.RemoveAt(rndIndex);

                    //Set each cell 1 to 9 for the selected row to 0.
                    for (int j = 0; j < N2; j++)
                    {
                        sudoku[rndNum][j] = 0;
                    }
                }
                else if (attempt < 15)
                {
                    //For each column, randomly remove 9 cells.
                    for (int i = 0; i < N2; i++)
                    {
                        for (int j = 0; j < N2; j++)
                        {
                            rndIndex = Rnd.Next(0, N2);
                            sudoku[i][rndIndex] = 0;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < N2; i++)
                    {
                        List<int> tempListA = sudoku[i].Where(x => x != 0).ToList();
                        List<int> tempListB = sudoku[i].ToList();
                        for (int j = 0; j < (tempListA.Count - 1); j++)
                        {
                            rndIndex = Rnd.Next(0, tempListA.Count);
                            rndNum = tempListA[rndIndex];

                            sudoku[i][tempListB.FindIndex(x => x == rndNum)] = 0;
                            tempListA.RemoveAt(rndIndex);
                        }
                    }
                }
                newSudoku = CopyArrayJagged(sudoku);

                //Attempt to solve using logic.
                CheckIsSolved(Solve(sudoku));
                //If it was not able to be solve. Revert to back-up.
                if (!IsSolved)
                {
                    sudoku = backupSudoku;
                    attempt++;
                    Debug.WriteLine("Attempt: " + attempt + "Count: " + count);
                }
                else
                    sudoku = newSudoku;

                count = 0;
                //Count all numbers still present in sudoku that are not 0.
                for (int i = 0; i < N2; i++)
                {
                    count += sudoku[i].Where(x => x != 0).Count();
                }
                if (count < 20)
                    break;

            } while (attempt < 500);
                return sudoku;
        }

        #region First draft Methods.
        /*
       private int[][] CreateFilledSudoku(int[][] sudoku)
       {
           for (int i = 0; i < N2; i++)
           {
               List<int> tempList = arrOptions.ToList();

               for (int j = 0; j < N2; j++)
               {
                   AddNumber(sudoku, i, j, tempList);
               }

           }
           return sudoku;
       }

       private void AddNumber(int[][] sudoku, int x, int y, List<int> optionList)
       {
           var tempList = GiveCellOptions(sudoku, x, y, optionList);

           if (tempList.Count != 0)
           {
               int tempInt = Rnd.Next(0, tempList.Count);
               sudoku[x][y] = tempList[tempInt];
               optionList.Remove(tempList[tempInt]);
           }
       }

       private List<int> GiveCellOptions(int[][] sudoku, int x, int y, List<int> optionList)
       {
           //Make new tempList 1 to 9.
           List<int> tempList = arrOptions.ToList();
           //Remove all options that are not allowed.
           for (int i = 0; i < N2; i++)
           {
               tempList.Remove(sudoku[x][i]);
               tempList.Remove(sudoku[i][y]);
           }

           //Check if the tempList isn't bigger than 0. There is no need to check the block also.
           if (tempList.Count > 0)
           {
               int a = WhichBlock(x);
               int b = WhichBlock(y);

               for (int i = (0 + (a * N)); i < (N + (a * N)); i++)
               {
                   for (int j = (0 + (b * N)); j < (N + (b * N)); j++)
                   {
                       tempList.Remove(sudoku[i][j]);
                   }
               }
           }

           //Interesect optionList with tempList to determine which actual options the cell has.
           return optionList.Intersect(tempList).ToList();
       }  
       */
        #endregion

        #endregion

        #region Iteration Methods
        private int[][] IterateLoop(int[][] sudoku, Func<int[][], int, int, int[][]> func)
        {
            for (int i = 0; i < N2; i++)
            {
                for (int j = 0; j < N2; j++)
                {
                    sudoku = func(sudoku, i, j);
                    if (NewArchive)
                        break;
                }
                if (NewArchive)
                    break;
            }
            return sudoku;
        }

        /// <summary>
        /// x and y both loop N times.
        /// To check each block in an N by N Sudoku.
        /// Using IterateBlock method to deligate the actual cell checking.
        /// </summary>
        private int[][] IterateAllBlocks(int[][] sudoku, Func<int[][], int, int, int[][]> func)
        {
            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N; x++)
                {
                    sudoku = IterateBLock(sudoku, x, y, func);
                    if (NewArchive)
                        break;
                }
                if (NewArchive)
                    break;
            }
            return sudoku;
        }

        /// <summary>
        /// For itirating through one singular block of size N by N.
        /// Adding x/y * N to i and j respectively to determine which block to check.
        /// </summary>
        /// <param name="x">Number of blocks horizontal.</param>
        /// <param name="y">Number of block vertical.</param>
        private int[][] IterateBLock(int[][] sudoku, int x, int y, Func<int[][], int, int, int[][]> func)
        {
            for (int i = (0 + (x * N)); i < (N + (N * x)); i++)
            {
                for (int j = (0 + (y * N)); j < (N + (N * y)); j++)
                {
                    sudoku = func(sudoku, i, j);
                    if (NewArchive)
                        break;
                }
                if (NewArchive)
                    break;
            }
            return sudoku;
        }
        #endregion

        #region Solving methods
        private int[][] IterateSudokuSolve(int[][] sudoku)
        {
            sudoku = IterateLoop(sudoku, CheckCell);
            sudoku = IterateAllBlocks(sudoku, CheckCell);
            return sudoku;
        }

        private int[][] IterateSudokuGuess(int[][] sudoku)
        {
            //Set countG to highest possible. Reset IsNewGuess;
            countG = N2 + 1;
            IsNewGuess = false;
            NewArchive = false;

            //Iterate through Sudoku to check for the lowest list count. Make that new countG;
            sudoku = IterateLoop(sudoku, CheckGuessCount);

            //If a NewArchive is needed do not bother checking.
            if (!NewArchive)
                sudoku = IterateAllBlocks(sudoku, CheckGuessCount);

            return sudoku;
        }

        private int[][] CheckCell(int[][] sudoku, int x, int y)
        {
            if (sudoku[x][y] == 0)
            {
                //Make temporary list.
                List<int> tempList = arrOptions.ToList();

                //Check row and column cell is in by keeping either x or y consistent and iterating through 9 cells.
                //Remove any numbers that are present from the temporary list.
                for (int i = 0; i < N2; i++)
                {
                    tempList.Remove(sudoku[x][i]);
                    tempList.Remove(sudoku[i][y]);
                }

                //Check if the tempList is bigger than 0. If it isn't then no need to check the block also.
                if (tempList.Count > 0)
                {
                    //Check which block in the x and y direction the cell is in.
                    int a = WhichBlock(x);
                    int b = WhichBlock(y);

                    //For said cell. Use the block knowledge to check the block it is in for numbers and remove from temp list.
                    for (int i = (0 + (a * N)); i < (N + (a * N)); i++)
                    {
                        for (int j = (0 + (b * N)); j < (N + (b * N)); j++)
                        {
                            tempList.Remove(sudoku[i][j]);
                        }
                    }
                }

                if (tempList.Count == 1)
                    sudoku[x][y] = tempList[0];
                else if (tempList.Count == 0)
                {
                    //If the tempList.Count == 0 that means there are no possible options and hence the sudoku can't be solved.
                    NewArchive = true;
                }
                else if (tempList.Count <= countG)
                {
                    //Make new Archive when a number is guessed, before the actual guess took place.
                    sudokuArchives.Add(new SudokuArchive{Sudoku = CopyArrayJagged(sudoku), IndexGuess = indexG , GuessOptions = tempList.ToArray(), CellX = x, CellY = y});

                    //Try the first of the count options.
                    sudoku[x][y] = tempList[indexG];

                    //Set countG to 1 to not allow furhter guessing at first.
                    countG = 1;
                }

            }

            return sudoku;
        }

        /// <summary>
        /// Adjusts countG to be as low as possible by checking all cells for their lowest Count.
        /// </summary>
        /// <param name="sudoku"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private int[][] CheckGuessCount(int[][] sudoku, int x, int y)
        {
            if (sudoku[x][y] == 0)
            {
                List<int> tempList = arrOptions.ToList();
                for (int i = 0; i < N2; i++)
                {
                    tempList.Remove(sudoku[x][i]);
                    tempList.Remove(sudoku[i][y]);
                }

                //Check block areas
                //If the tempList isn't bigger than 1. There is no need to check the block also.
                if (tempList.Count > 0)
                {
                    int a = WhichBlock(x);
                    int b = WhichBlock(y);

                    for (int i = (0 + (a * N)); i < (N + (a * N)); i += 2)
                    {
                        for (int j = (0 + (b * N)); j < (N + (b * N)); j += 2)
                        {
                            tempList.Remove(sudoku[i][j]);
                        }
                    }
                }
                if (tempList.Count == 0)
                {
                    NewArchive = true;
                }
                else if (tempList.Count < countG)
                {
                    countG = tempList.Count;
                    IsNewGuess = true;
                }

              
            }
            return sudoku;
        }
        #endregion

 
    }
}