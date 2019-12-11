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
using System.Windows.Shapes;

namespace LogicPuzzleSolver.Utilities
{
    /// <summary>
    /// Logika interakcji dla klasy WindowChooseNumber.xaml
    /// </summary>
    public partial class WindowChooseNumber : Window
    {

        public int result { get; private set; }
        const int numberPixels = 40;
        int maxNumber;
        public WindowChooseNumber(int maxNumber)
        {
            InitializeComponent();

            this.maxNumber = maxNumber;
            int width = (int)Math.Ceiling(Math.Sqrt(maxNumber));
            int height = (int)Math.Ceiling((double)maxNumber / width);
            for (int i = 0; i <= height; i++)
            {
                gridMain.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < width; i++)
            {
                gridMain.ColumnDefinitions.Add(new ColumnDefinition());
            }

            int temp = 0;
            while (temp < maxNumber)
            {
                Button buttonNumber = new Button();
                buttonNumber.Click += ReturnValue;
                buttonNumber.Content = (temp + 1).ToString();
                Grid.SetColumn(buttonNumber, temp % width);
                Grid.SetRow(buttonNumber, temp / width);
                gridMain.Children.Add(buttonNumber);
                temp++;
            }
            Button buttonEmpty = new Button();
            buttonEmpty.Click += ReturnValue;
            Grid.SetColumnSpan(buttonEmpty, width);
            Grid.SetColumn(buttonEmpty, 0);
            Grid.SetRow(buttonEmpty, height + 1);
            gridMain.Children.Add(buttonEmpty);

            result = 0;
            this.Height = numberPixels * (height + 1);
            this.Width = numberPixels * width;
            this.PreviewKeyDown += new KeyEventHandler(NumberIsPressed);

            Point point = Mouse.GetPosition(Application.Current.MainWindow);
            this.Top = point.Y + Application.Current.MainWindow.Top + SystemParameters.WindowCaptionHeight;
            this.Left = point.X + Application.Current.MainWindow.Left ;
        }

        private void NumberIsPressed(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                result = Int32.Parse((e.Key.ToString()).Substring(1, 1));
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                result = Int32.Parse((e.Key.ToString()).Substring(6, 1));
            }
            if (result > maxNumber)
            {
                result = maxNumber;
            }
            Close();
        }

        private void ReturnValue(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content != null)
                result = int.Parse(((Button)sender).Content.ToString());
            else
                result = 0;
            Close();
        }
    }
}
