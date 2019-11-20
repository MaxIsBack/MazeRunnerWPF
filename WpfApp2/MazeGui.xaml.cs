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

namespace MazeRunnerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CurrentAngle = targetAngle = GetLookRotation();
        }

        private void btnTurnLeft_Click(object sender, RoutedEventArgs e) { TurnLeft(); }
        private void btnTurnRight_Click(object sender, RoutedEventArgs e) { TurnRight(); }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left) TurnLeft();
            else if (e.Key == Key.Right) TurnRight();
        }




        private void TurnLeft()
        {
            Turn(-90);
        }

        private void TurnRight()
        {
            Turn(90);
        }

        private double targetAngle;
        private void Turn(double angle)
        {
            targetAngle += angle;
            CurrentAngle = GetLookRotation();
            new Thread(new ThreadStart(TurnAnimateAsync)).Start();
        }

        private delegate void UpdateSetLookRotation(double angle);
        private void SetLookRotation(double angle)
        {
            lookRotation.Angle = angle;
        }

        private double GetLookRotation()
        {
            return lookRotation.Angle;
        }

        private double CurrentAngle;
        private void TurnAnimateAsync()
        {
            int ticks = 30;
            double turnDelta = (targetAngle - CurrentAngle) / ticks;
            for (int i = 0; i < ticks; i++)
            {
                Thread.Sleep(1000 / 60);
                CurrentAngle += turnDelta;
                Console.WriteLine(CurrentAngle);
                Dispatcher.Invoke(
                    new UpdateSetLookRotation(this.SetLookRotation),
                    new object[] { CurrentAngle }
                );
            }
        }
    }
}

