using LogicPuzzleSolver.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace LogicPuzzleSolver.Sudoku
{
    /// <summary>
    /// Logika interakcji dla klasy PageSudokuSolver.xaml
    /// </summary>
    public partial class PageSudokuSolver : Page
    {
        Button[,] buttonBoard;
        SudokuSolver sudoku;
        List<int[,]> solutions;
        int solutionNumber;
        public PageSudokuSolver()
        {
            InitializeComponent();
            buttonBoard = new Button[9, 9];

            for (int i = 0; i < 9; i++)
            {
                    gridBoard.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(5.0, GridUnitType.Star) });
                    gridBoard.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(5.0, GridUnitType.Star) });
                    if ((i+1) % 3 == 0)
                    {
                        gridBoard.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Star) });
                        gridBoard.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1.0, GridUnitType.Star) });
                    }
            }

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (!(i == 3 || i == 7 || j == 3 || j == 7))
                    {
                        Button b = new Button();
                        b.Click += ExtraMethods.ChooseNumberWindow(9);
                        Binding binding = new Binding("boardFontSize");
                        binding.Source = Properties.Settings.Default ;
                        b.SetBinding(Button.FontSizeProperty, binding);
                        b.Content = "";
                        Grid.SetRow(b, i );
                        Grid.SetColumn(b, j );
                        gridBoard.Children.Add(b);
                        buttonBoard[i-i/4, j-j/4] = b;
                    }
                }
            }

        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Txt files (*.txt) | *.txt"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                int[,] tempBoard = new int[9, 9];
                using (System.IO.StreamReader file = new System.IO.StreamReader(openFileDialog.FileName))
                {
                    string temp;                 
                    for (int i = 0; i < 9; i++)
                    {
                        temp = file.ReadLine();
                        for (int j = 0; j < 9; j++)
                        {
                            tempBoard[i, j] =  temp[j]-'0';
                        }
                    }
                }
                UpdateBoard(tempBoard);
            }    
        }    

        private void UpdateBoard(int [,] tempBoard)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    buttonBoard[i, j].Content = tempBoard[i, j] == 0 ? "" : tempBoard[i, j].ToString();
                }
            }
        }

        private int[,] GetBoard()
        {
            int[,] tempBoard = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    tempBoard[i, j] = buttonBoard[i, j].Content.ToString() == "" ? 0 : int.Parse(buttonBoard[i, j].Content.ToString());
                }
            }
            return tempBoard;
        }

        private void ButtonClearBoard_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    buttonBoard[i, j].Content = "";
                }
            }
            TextBoxElapsedTime.Text = " ";
            ButtonNextSolution.IsEnabled = false;
        }

        private void ButtonSolveWithoutGuessing_Click(object sender, RoutedEventArgs e)
        {
            sudoku = new SudokuSolver(GetBoard());
            UpdateBoard(sudoku.SolveWithoutGuessing());
            ButtonNextSolution.IsEnabled = false;
        }

        private void ButtonSolve_Click(object sender, RoutedEventArgs e)
        {
            sudoku = new SudokuSolver(GetBoard());
            solutions = sudoku.Solve((bool)CheckBoxAll.IsChecked?true:false);
            if (solutions.Count > 0)
            {
                TextBoxElapsedTime.Text = "Solved in "+sudoku.solvingTime.ToString();
                UpdateBoard(solutions[0]);
                if(solutions.Count > 1)
                {
                    ButtonNextSolution.IsEnabled = true;
                    solutionNumber = 0;
                }
                else
                {
                    ButtonNextSolution.IsEnabled = false;
                }
            }
            else
            {
                TextBoxElapsedTime.Text = "There are no solutions :(";
                ButtonNextSolution.IsEnabled = false;
            }               
        }

        private void ButtonNextSolution_Click(object sender, RoutedEventArgs e)
        {
            solutionNumber = (solutionNumber + 1) % solutions.Count;
            UpdateBoard(solutions[solutionNumber]);
        }
    }
}
