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
        public const int THREAD_SLEEP = 1000 / 90;  // 90 fps for removing stuttering
        private MazeGui.MazeGuiBuilder mazeBuilder;
        private (int x, int y) currentLocation;

        public MainWindow()
        {
            InitializeComponent();
            mazeBuilder = new MazeGui.MazeGuiBuilder(3);
            currentLocation = mazeBuilder.GetEntranceLoc();
            Console.WriteLine();
            CurrentAngle = targetAngle = GetLookRotation();
            BuildCurrentLocation();
        }

        private void BuildCurrentLocation()
        {
            var collect = mazeBuilder.BuildRoomTextureCoordinates(currentLocation.x, currentLocation.y);
            meshMainRoom.TextureCoordinates = collect;
            Console.WriteLine(meshMainRoom.TextureCoordinates);
        }

        private void btnTurnLeft_Click(object sender, RoutedEventArgs e) { TurnLeft(); }
        private void btnTurnRight_Click(object sender, RoutedEventArgs e) { TurnRight(); }
        private void btnAction_Click(object sender, RoutedEventArgs e) { DoAction(); }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left) TurnLeft();
            else if (e.Key == Key.Right) TurnRight();
            else if (e.Key == Key.Up) DoAction();
        }




        private void TurnLeft()
        {
            Turn(-90);
        }

        private void TurnRight()
        {
            Turn(90);
        }

        private void DoAction()
        {
            MoveToZ(1);
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
                Thread.Sleep(THREAD_SLEEP);
                CurrentAngle += turnDelta;
                Dispatcher.Invoke(
                    new UpdateSetLookRotation(this.SetLookRotation),
                    new object[] { CurrentAngle }
                );
            }
        }


        private double targetZ, currentZ;
        private void MoveToZ(double newZ)
        {
            targetZ = newZ;
            currentZ = camMain.Position.Z;
            new Thread(new ThreadStart(MoveAnimateAsync)).Start();
        }

        private void MoveAnimateAsync()
        {
            int ticks = 30;
            double moveDelta = (targetZ - currentZ) / ticks;
            for (int i = 0; i < ticks; i++)
            {
                Thread.Sleep(THREAD_SLEEP);
                currentZ += moveDelta;
                Dispatcher.Invoke(
                    new UpdateSetZPos(this.SetZPos),
                    new object[] { currentZ }
                );
            }

            targetZ = -2;
            currentZ = -5;
            Dispatcher.Invoke(
                new UpdateSetZPos(this.SetZPos),
                new object[] { currentZ }
            );

            moveDelta = (targetZ - currentZ) / ticks;
            for (int i = 0; i < ticks; i++)
            {
                Thread.Sleep(THREAD_SLEEP);
                currentZ += moveDelta;
                Dispatcher.Invoke(
                    new UpdateSetZPos(this.SetZPos),
                    new object[] { currentZ }
                );
            }
        }

        private delegate void UpdateSetZPos(double z);
        private void SetZPos(double z)
        {
            var pt = camMain.Position;
            pt.Z = z;
            camMain.Position = pt;
        }
    }
}

