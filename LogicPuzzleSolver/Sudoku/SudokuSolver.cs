using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicPuzzleSolver.Sudoku
{
    class SudokuSolver
    {
        int[,] board;
        int[,,] boardIsNumberPossible;
        List<int[,]> solutions;
        bool isSolvingFinished;
        bool allSolutions;
        public TimeSpan solvingTime { private set;  get; }

        //Expected format: 9 lines with 9 characters each, with 0 in empty spaces and 1-9 digit in non-empty
        public SudokuSolver(string location)
        {
            board = new int[9, 9];
            using (System.IO.StreamReader file = new System.IO.StreamReader(location))
            {
                string temp;
                for (int i = 0; i < 9; i++)
                {
                    temp = file.ReadLine();
                    for (int j = 0; j < 9; j++)
                    {
                        board[i, j] = temp[j] - '0';
                    }
                }
            }
            Setup();
        }

        public SudokuSolver(int [,] tempBoard)
        {
            board = new int[9, 9];
            board = tempBoard;
            Setup();
        }

        public SudokuSolver(int[] tempBoard)
        {
            board = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    board[i, j] = tempBoard[i * 9 + j];
                }
            }
            Setup();
        }

        //Return solutions or empty list if there are none
        public List<int[,]> Solve(bool allSolutions)
        {
            this.allSolutions = allSolutions;
            if (isSolvingFinished == false)
            {
                if (CheckIfBoardIsCorrect())
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    SolveWithoutGuessing();
                    SolveWithGuessing(0);
                    watch.Stop();
                    isSolvingFinished = true;
                    solvingTime = watch.Elapsed;
                }
                else
                {
                    isSolvingFinished = true;
                }

            }
            return solutions;
        }


        public int[,] SolveWithoutGuessing()
        {
            int oldSum = -1;
            int newSum = BoardSum();
            while (oldSum != newSum)
            {
                oldSum = newSum;
                TryIsOnlyPlacePossible();
                TryIsOnlyNumberPossible();               
                newSum = BoardSum();
            }
            return board;
        }

        private bool CheckIfBoardIsCorrect()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i, j] != 0)
                    {
                        int temp = board[i, j];
                        board[i, j] = 0;
                        boardIsNumberPossible[i, j, temp] = 1;
                        if (!CanPlaceNumber(i, j, temp))
                        {
                            board[i, j] = temp;
                            boardIsNumberPossible[i, j, temp] = 0;
                            return false;
                        }
                        board[i, j] = temp;
                        boardIsNumberPossible[i, j, temp] = 0;
                    }
                }
            }
            return true;
        }

        private void Setup()
        {
            solutions = new List<int[,]>();
            isSolvingFinished = false;
            allSolutions = false;

            boardIsNumberPossible = new int[9, 9, 10];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        boardIsNumberPossible[i, j, k] = 1;
                    }
                }
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i, j] != 0)
                    {
                        PlaceNumber(i, j, board[i, j]);
                    }
                }
            }
        }

        private void PlaceNumber(int x, int y, int number)
        {
            board[x, y] = number;
            for (int i = 0; i < 10; i++)
            {
                boardIsNumberPossible[x, y, i] = 0;
            }
            for (int i = 0; i < 9; i++)
            {
                boardIsNumberPossible[i, y, number] = 0;
                boardIsNumberPossible[x, i, number] = 0;
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    boardIsNumberPossible[(x / 3) * 3 +i, (y / 3) * 3 + j, number] = 0;
                }
            }
        }

        private int BoardSum()
        {
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    sum += board[i, j];
                }
            }
            return sum;
        }

        private void TryIsOnlyNumberPossible()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i, j] == 0)
                    {
                        TryIsOnlyNumberPossible(i, j);
                    }                
                }
            }
        }

        private void TryIsOnlyNumberPossible(int x, int y)
        {
            int counter = 0;
            int number = 0;
            for(int k = 1; k <= 9; k++)
            {
                if (boardIsNumberPossible[x, y, k] == 1)
                {
                    counter++;
                    number = k;
                }
            }
            if(counter == 1)
            {
                PlaceNumber(x, y, number);
            }
        }

        private void TryIsOnlyPlacePossible()
        {
            for(int number = 1; number <= 9; number++)
            {
                for (int i = 0; i < 9; i++)
                {
                    TryIsOnlyPlacePossibleInRow(i, number);
                    TryIsOnlyPlacePossibleInCol(i, number);
                    TryIsOnlyPlacePossibleInSquare(number);
                }
            }
        }

        private void TryIsOnlyPlacePossibleInRow(int row, int value)
        {
            int counter = 0;
            int col = 0;
            for (int j = 0; j < 9; j++)
            {
                if (boardIsNumberPossible[row, j, value] == 1)
                {
                    counter++;
                    col = j;
                }
            }
            if (counter == 1)
            {
                PlaceNumber(row, col, value);
            }

        }

        private void TryIsOnlyPlacePossibleInCol(int col, int value)
        {
            int counter = 0;
            int row=0;
            for (int i = 0; i < 9; i++)
            {
                if (boardIsNumberPossible[i, col, value] == 1)
                {
                    counter++;
                    row = i;
                }
            }
            if (counter == 1)
            {
                PlaceNumber(row, col, value);
            }
        }

        private void TryIsOnlyPlacePossibleInSquare(int value)
        {
            int counter;
            int tempI=0, tempJ=0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    counter = 0;
                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            if (boardIsNumberPossible[3*i + k, 3 * j + l, value] == 1)
                            {
                                counter++;
                                tempI = k;
                                tempJ = l;
                            }
                        }
                    }
                    if (counter == 1)
                    {
                        PlaceNumber(3 * i +tempI, 3 * j +tempJ, value);
                    }
                }
            }

        }

        private bool SolveWithGuessing(int z)
        {
            if (z == 81)
            {
                solutions.Add((int[,])board.Clone());
                if (allSolutions)
                    return false;
                else
                    return true;
            }
            else
            {
                int i = z / 9;
                int j = z % 9;
                if (board[i, j] == 0)
                {
                    for (int number = 1; number <= 9; number++)
                    {
                        if (CanPlaceNumber(i, j, number))
                        {
                            board[i, j] = number;
                            if (SolveWithGuessing(z + 1))
                                return true;
                        }
                    }
                    board[i, j] = 0;
                    return false;
                }
                else
                {
                    if (SolveWithGuessing(z + 1))
                        return true;
                    else
                        return false;
                }
            }
        }

        private bool CanPlaceNumber(int x, int y, int number)
        {
            if (boardIsNumberPossible[x, y, number] == 0)
                return false;

            for (int i = 0; i < 9; i++)
            {
                if (board[i, y] == number || board[x, i] == number)
                {
                    return false;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[(x / 3) * 3 + i, (y / 3) * 3 + j] == number)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
