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

namespace WpfApp2
{
    struct _Room
    {
        bool NorthDoor, SouthDoor, EastDoor, WestDoor;
        public _Room(bool northD, bool southD, bool eastD, bool westD)
        {
            NorthDoor = northD;
            SouthDoor = southD;
            EastDoor = eastD;
            WestDoor = westD;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var sampleRoom = new _Room(true, false, true, true);
            CreateRoom(20, 20, sampleRoom);
        }

        private static Rectangle ColoredRectInit(Color border, Color fill)
        {
            Rectangle rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(border);
            rect.Fill = new SolidColorBrush(fill);
            return rect;
        }

        private void CreateRoom(int x, int y, _Room room)
        {
            const int RM_SIZE = 200;
            const int DR_LONG = 50;
            const int DR_SKINNY = 20;
            Rectangle rect;

            // Create main rect
            rect = ColoredRectInit(Colors.Black, Colors.Beige);
            rect.Width = RM_SIZE;
            rect.Height = RM_SIZE;
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            mainCanvas.Children.Add(rect);


            // Create door (N)
            rect = ColoredRectInit(Colors.DarkOrange, Colors.Orange);
            rect.Width = DR_LONG;
            rect.Height = DR_SKINNY;
            Canvas.SetLeft(rect, x + RM_SIZE / 2 - rect.Width / 2);
            Canvas.SetTop(rect, y - rect.Height / 2);
            mainCanvas.Children.Add(rect);

            // Create door (S)
            rect = ColoredRectInit(Colors.DarkOrange, Colors.Orange);
            rect.Width = DR_LONG;
            rect.Height = DR_SKINNY;
            Canvas.SetLeft(rect, x + RM_SIZE / 2 - rect.Width / 2);
            Canvas.SetTop(rect, y + RM_SIZE - rect.Height / 2);
            mainCanvas.Children.Add(rect);

            // Create door (E)
            rect = ColoredRectInit(Colors.DarkOrange, Colors.Orange);
            rect.Width = DR_SKINNY;
            rect.Height = DR_LONG;
            Canvas.SetLeft(rect, x + RM_SIZE - rect.Width / 2);
            Canvas.SetTop(rect, y + RM_SIZE / 2 - rect.Height / 2);
            mainCanvas.Children.Add(rect);

            // Create door (W)
            rect = ColoredRectInit(Colors.DarkOrange, Colors.Orange);
            rect.Width = DR_SKINNY;
            rect.Height = DR_LONG;
            Canvas.SetLeft(rect, x - rect.Width / 2);
            Canvas.SetTop(rect, y + RM_SIZE / 2 - rect.Height / 2);
            mainCanvas.Children.Add(rect);
        }
    }
}
