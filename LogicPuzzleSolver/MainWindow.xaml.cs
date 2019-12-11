using LogicPuzzleSolver.Nonogram;
using LogicPuzzleSolver.Skyscraper;
using LogicPuzzleSolver.Sudoku;
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

namespace LogicPuzzleSolver
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, Page> PageList;
        ResourceDictionary resDictDSIII;
        ResourceDictionary resDictBright;
        public MainWindow()
        {
            InitializeComponent();
            PageList = new Dictionary<string, Page>();
            resDictDSIII = new ResourceDictionary
            {
                Source = new Uri("Utilities/DSIII.xaml", UriKind.RelativeOrAbsolute)
            };
            resDictBright = new ResourceDictionary
            {
                Source = new Uri("Utilities/Bright.xaml", UriKind.RelativeOrAbsolute)
            };
            Application.Current.Resources.MergedDictionaries.Add(resDictDSIII);
        }

        private void ButtonFontSizeMinus_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.fontSize > 8)
            {
                Properties.Settings.Default.fontSize -= 2;
                Properties.Settings.Default.Save();
            }
        }

        private void ButtonFontSizePlus_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.fontSize < 20)
            {
                Properties.Settings.Default.fontSize += 2;
                Properties.Settings.Default.Save();
            }
        }

        private void MenuStyleDSIII_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resDictDSIII);
        }

        private void MenuStyle_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resDictBright);
        }

        private void MenuSudokuSolver_Click(object sender, RoutedEventArgs e)
        {
            if (!PageList.ContainsKey("SudokuSolver"))
            {
                PageList.Add("SudokuSolver", new PageSudokuSolver());                
            }
            FrameMain.Navigate(PageList["SudokuSolver"]);
        }

        private void MenuSkyscraperSolver_Click(object sender, RoutedEventArgs e)
        {
            if (!PageList.ContainsKey("SkyscraperSolver"))
            {
                PageList.Add("SkyscraperSolver", new PageSkyscraperSolver());
            }
            FrameMain.Navigate(PageList["SkyscraperSolver"]);
        }

        private void MenuNonogramSolver_Click(object sender, RoutedEventArgs e)
        {
            if (!PageList.ContainsKey("NonogramSolver"))
            {
                PageList.Add("NonogramSolver", new PageNonogramSolver());
            }
            FrameMain.Navigate(PageList["NonogramSolver"]);
        }

        private void MenuSkyscraperGenerator_Click(object sender, RoutedEventArgs e)
        {
            if (!PageList.ContainsKey("SkyscraperGenerator"))
            {
                PageList.Add("SkyscraperGenerator", new PageSkyscraperGenerator());
            }
            FrameMain.Navigate(PageList["SkyscraperGenerator"]);
        }
    }
}
