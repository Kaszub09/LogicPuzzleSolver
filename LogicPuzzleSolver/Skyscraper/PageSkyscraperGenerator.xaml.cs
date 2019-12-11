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

namespace LogicPuzzleSolver.Skyscraper
{
    /// <summary>
    /// Logika interakcji dla klasy PageSkyscraperGenerator.xaml
    /// </summary>
    public partial class PageSkyscraperGenerator : Page
    {
        int size;
        Control[,] listOfControls;
        SkyscraperGenerator skyscrapers;
        bool showBoard = true;
        public PageSkyscraperGenerator()
        {
            InitializeComponent();
            size = 0;
            ButtonIncrease_Click(null, null);
        }


        private void ButtonDecrease_Click(object sender, RoutedEventArgs e)
        {
            if (size > 1)
            {
                size--;
                RedrawPanel();
                UpdateLabel();
                skyscrapers = new SkyscraperGenerator(size);
                UpdateBoard();
            }
        }

        private void ButtonIncrease_Click(object sender, RoutedEventArgs e)
        {
            size++;
            RedrawPanel();
            UpdateLabel();
            skyscrapers = new SkyscraperGenerator(size);
            UpdateBoard();

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
                        b.Click += RemoveRestoreNumber;
                        b.Content = "";
                        b.SetBinding(Button.FontSizeProperty, binding);
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
                            b.Click += RemoveRestoreNumber;
                            b.Content = "";
                            b.SetBinding(Button.FontSizeProperty, binding);
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

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    ((Label)listOfControls[i + 1, j + 1]).Content = showBoard == true ? skyscrapers.GetField(i, j).ToString() : null;
                }
            }


            for (int i = 0; i < size; i++)
            {
                ((Button)listOfControls[0, i + 1]).Content = skyscrapers.GetNumber(i, 0) == 0 ? "" : skyscrapers.GetNumber(i, 0).ToString();
                ((Button)listOfControls[i + 1, size + 1]).Content = skyscrapers.GetNumber(i, 1) == 0 ? "" : skyscrapers.GetNumber(i, 1).ToString();
                ((Button)listOfControls[size + 1, i + 1]).Content = skyscrapers.GetNumber(i, 2) == 0 ? "" : skyscrapers.GetNumber(i, 2).ToString();
                ((Button)listOfControls[i + 1, 0]).Content = skyscrapers.GetNumber(i, 3) == 0 ? "" : skyscrapers.GetNumber(i, 3).ToString();

            }

        }



        private void ButtonIncreaseNumbersSize_Click(object sender, RoutedEventArgs e)
        {
            foreach (Control c in listOfControls)
            {
                if (c != null)
                {
                    c.FontSize += 2;
                }
            }
        }

        private void ButtonDecreasNumbersSize_Click(object sender, RoutedEventArgs e)
        {
            foreach (Control c in listOfControls)
            {
                if (c != null)
                {
                    if (c.FontSize > 3)
                        c.FontSize -= 2;
                }
            }

        }

        private void ButtonBackPermutate_Click(object sender, RoutedEventArgs e)
        {
            skyscrapers.BackLastPermutation();
            UpdateBoard();
        }

        private void ButtonBackNumber_Click(object sender, RoutedEventArgs e)
        {
            skyscrapers.RestorDeletedNumber();
            UpdateBoard();
        }

        private void ButtonPermutateCol_Click(object sender, RoutedEventArgs e)
        {
            skyscrapers.Permutate(true);
            UpdateBoard();
        }

        private void ButtonPermutateRow_Click(object sender, RoutedEventArgs e)
        {
            skyscrapers.Permutate(false);
            UpdateBoard();
        }

        private void ButtonDeleteNumber_Click(object sender, RoutedEventArgs e)
        {
            skyscrapers.DeleteRandomNumber();
            UpdateBoard();
        }

        private void RemoveRestoreNumber(object sender, RoutedEventArgs e)
        {

            int y = Grid.GetColumn((Button)sender);
            int x = Grid.GetRow((Button)sender);
            if (x == 0 || x == size + 1)
            {
                if (((Button)sender).Content != null)
                {
                    skyscrapers.RemoveNumber(y - 1, x == 0 ? 0 : 2);
                }
                else
                {
                    skyscrapers.RestoreNumber(y - 1, x == 0 ? 0 : 2);
                }
                UpdateBoard();
            }
            else
            {
                if (((Button)sender).Content != null)
                {
                    skyscrapers.RemoveNumber(x - 1, y == 0 ? 3 : 1);
                }
                else
                {
                    skyscrapers.RestoreNumber(x - 1, y == 0 ? 3 : 1);
                }
                UpdateBoard();
            }

        }

        private void ButtonClearRememberedPermutations_Click(object sender, RoutedEventArgs e)
        {
            skyscrapers.ClearListOfOperations();
        }


        private void CheckboxShowBoard_Click_1(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                showBoard = true;
            }
            else
            {
                showBoard = false;
            }

            UpdateBoard();
        }

    }
}
