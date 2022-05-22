using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Convolutional_Neural_Network
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 11; i++)
            {
                for (int q = 0; q < 11; q++)
                {
                    var newButton = new Button()
                    {
                        Name = $"b{i}{q}",
                        Width = 37,
                        Height = 37,
                        Background = (Brush)(new BrushConverter().ConvertFrom("#FFFFFFFF"))
                    };
                    newButton.Click += new RoutedEventHandler(Button_Click);
                    Grid.SetRow(newButton, i);
                    Grid.SetColumn(newButton, q);
                    Main.Children.Add(newButton);
                }
            }
        }
        private readonly double[,] list = new double[11, 11];
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = (Button)sender;
            b.Background = (Brush)(new BrushConverter().ConvertFrom("Black"));
            int x = int.Parse(b.Name[1].ToString());
            int y = int.Parse(b.Name[2].ToString());
            list[x, y] = 1;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 11; i++)
            {
                for (int w = 0; w < 11; w++)
                {
                    if (list[i, w]==0)
                        list[i, w] = -1;
                }
            }
            MessageBox.Show("The shape is " + Action.Dicaver(list));
            new MainWindow().Show();
            Close();
        }
    }
}
