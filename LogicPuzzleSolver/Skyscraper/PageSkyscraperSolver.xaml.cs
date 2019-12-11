using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace LogicPuzzleSolver.Skyscraper
{
    /// <summary>
    /// Logika interakcji dla klasy PageSkyscraperSolver.xaml
    /// </summary>
    public partial class PageSkyscraperSolver : Page
    {
        int size;
        Control[,] listOfControls;
        SkyscraperSolver skyscrapers;
        CancellationTokenSource cancellationTokenSource;
        bool wasCancelled;
        public PageSkyscraperSolver()
        {
            InitializeComponent();
            size = 0;
            ButtonIncrease_Click(null, null);
            buttonCancel.IsEnabled = false;
        }

        private void ButtonDecrease_Click(object sender, RoutedEventArgs e)
        {
            if (size > 1)
            {
                size--;
                RedrawPanel();
                UpdateLabel();
            }
        }

        private void ButtonIncrease_Click(object sender, RoutedEventArgs e)
        {
            size++;
            RedrawPanel();
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            labelSize.Content = size.ToString() + " x " + size.ToString();
        }

        private void RedrawPanel()
        {
            gridMainPanel.Children.Clear();
            gridMainPanel.ColumnDefinitions.Clear();
            gridMainPanel.RowDefinitions.Clear();
            listOfControls = new Control[size + 2, size + 2];
            Binding binding = new Binding("boardFontSize");
            binding.Source = Properties.Settings.Default;
            for (int i = 0; i <= size + 1; i++)
            {
                gridMainPanel.RowDefinitions.Add(new RowDefinition());
                gridMainPanel.ColumnDefinitions.Add(new ColumnDefinition());
                if (i == 0 || i == size + 1)
                {
                    for (int j = 1; j <= size; j++)
                    {
                        Button b = new Button();
                        b.Click += Utilities.ExtraMethods.ChooseNumberWindow(size);
                        b.SetBinding(Button.FontSizeProperty, binding);
                        b.Content = "";
                        Grid.SetColumn(b, j);
                        Grid.SetRow(b, i);
                        gridMainPanel.Children.Add(b);
                        listOfControls[i, j] = b;
                    }
                }
                else
                {
                    for (int j = 0; j <= size + 1; j++)
                    {
                        if (j == 0 || j == size + 1)
                        {
                            Button b = new Button();
                            b.Click += Utilities.ExtraMethods.ChooseNumberWindow(size);
                            b.SetBinding(Button.FontSizeProperty, binding);
                            b.Content = "";
                            Grid.SetColumn(b, j);
                            Grid.SetRow(b, i);
                            gridMainPanel.Children.Add(b);
                            listOfControls[i, j] = b;
                        }
                        else
                        {
                            Label l = new Label();
                            Grid.SetColumn(l, j);
                            Grid.SetRow(l, i);
                            l.SetBinding(Button.FontSizeProperty, binding);

                            l.BorderBrush = Brushes.Black;
                            l.BorderThickness = new Thickness(0, 0, 1, 1);
                            l.HorizontalContentAlignment = HorizontalAlignment.Center;
                            l.VerticalContentAlignment = VerticalAlignment.Center;
                            l.Content = " ";
                            gridMainPanel.Children.Add(l);
                            listOfControls[i, j] = l;
                        }
                    }
                }
            }
        }


        private void UpdateBoard()
        {
            if (skyscrapers != null)
            {
                int[,] board = skyscrapers.GetBoard();
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        ((Label)listOfControls[i + 1, j + 1]).Content = board[i, j] == 0 ? " " : board[i, j].ToString();
                    }
                }
            }
        }

        private async void ButtonSolve_Click(object sender, RoutedEventArgs e)
        {
            int[] rowTop, rowBottom, rowLeft, rowRight;
            rowTop = new int[size];
            rowLeft = new int[size];
            rowRight = new int[size];
            rowBottom = new int[size];
            for (int i = 0; i < size; i++)
            {
                rowTop[i] = ((Button)listOfControls[0, i + 1]).Content != "" ?
                    int.Parse(((Button)listOfControls[0, i + 1]).Content.ToString()) : 0;
                rowLeft[i] = ((Button)listOfControls[i + 1, 0]).Content != "" ?
                    int.Parse(((Button)listOfControls[i + 1, 0]).Content.ToString()) : 0;
                rowRight[i] = ((Button)listOfControls[i + 1, size + 1]).Content != "" ?
                    int.Parse(((Button)listOfControls[i + 1, size + 1]).Content.ToString()) : 0;
                rowBottom[i] = ((Button)listOfControls[size + 1, i + 1]).Content != "" ?
                    int.Parse(((Button)listOfControls[size + 1, i + 1]).Content.ToString()) : 0;
            }

            skyscrapers = new SkyscraperSolver(size, rowTop, rowBottom, rowLeft, rowRight);
            bool solved;
            wasCancelled = false;
            cancellationTokenSource = new CancellationTokenSource();
            skyscrapers.token = cancellationTokenSource.Token;
            textBoxInfo.Text = "Solving...";
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            ChangeButtonState(false);
            var watch = System.Diagnostics.Stopwatch.StartNew();

            solved = await Task.Factory.StartNew(() => skyscrapers.Solve());

            watch.Stop();
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            ChangeButtonState(true);
            if (solved)
            {
                textBoxInfo.Text = watch.Elapsed.ToString();
            }
            else
            {
                if (wasCancelled)
                {
                    textBoxInfo.Text = "Cancelled";
                }
                else
                {
                    textBoxInfo.Text = "It's impossible to solve!";
                }
            }
            UpdateBoard();
        }

        private void ChangeButtonState(bool state)
        {
            buttonDecrease.IsEnabled = state;
            buttonIncrease.IsEnabled = state;
            buttonSolve.IsEnabled = state;
            buttonCancel.IsEnabled = !state;
        }



        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource.Cancel();
            wasCancelled = true;
        }

    }
}
