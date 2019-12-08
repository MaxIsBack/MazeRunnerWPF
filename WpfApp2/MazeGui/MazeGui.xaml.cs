using MazeRunnerWPF.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

namespace MazeRunnerWPF.MazeGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MazeGui : Page, IGuiPage
    {
        public const int THREAD_SLEEP = 1000 / 90;  // 90 fps for removing stuttering
        private MazeGuiBuilder mazeBuilder;
        private (int x, int y) currentLocation;
        private CardinalDirs currentDir;

        private bool isWaitingOnSetup;

        public MazeGui()
        {
            acceptInput = true;
            InitializeComponent();
            isWaitingOnSetup = true;
        }

        private void SetupMaze(int size, int difficulty)
        {
            mazeBuilder = new MazeGuiBuilder(size, difficulty);
            currentLocation = mazeBuilder.GetPlayerLoc();
            currentAngle = targetAngle = GetLookRotation();
            currentDir = CardinalDirs.NORTH;
            BuildCurrentLocation();
        }

        private void LoadMazeFromSaveFile()
        {
            mazeBuilder = new MazeGuiBuilder();
            currentLocation = mazeBuilder.GetPlayerLoc();
            currentDir = mazeBuilder.GetPlayerDir();
            currentAngle = targetAngle = GetLookRotation(currentDir);
            BuildCurrentLocation();
        }

        private void MoveRoomsAuto()
        {
            switch (currentDir)
            {
                case CardinalDirs.NORTH:
                    currentLocation.y--;
                    break;
                case CardinalDirs.SOUTH:
                    currentLocation.y++;
                    break;
                case CardinalDirs.EAST:
                    currentLocation.x++;
                    break;
                case CardinalDirs.WEST:
                    currentLocation.x--;
                    break;
            }
            mazeBuilder.UpdatePlayerLoc(currentLocation);
        }

        private delegate void TriggerBuildCurrentLocation();
        private void BuildCurrentLocation()
        {
            UpdateIfCanMove();
            mazeBuilder.BuildRoomTextures(
                currentLocation.x,
                currentLocation.y,
                ref matDiffuseNorth,
                ref matDiffuseSouth,
                ref matDiffuseWest,
                ref matDiffuseEast,
                ref matDiffuseFloor,
                ref matDiffuseCeiling
            );
        }



        private bool acceptInput;

        private void btnTurnLeft_Click(object sender, RoutedEventArgs e) { TurnLeft(); }
        private void btnTurnRight_Click(object sender, RoutedEventArgs e) { TurnRight(); }
        private void btnAction_Click(object sender, RoutedEventArgs e) { DoAction(); }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            MessageBox.Show(window, "Game Saved! Click OK to continue.", "Game Saved", MessageBoxButton.OK, MessageBoxImage.Information);
            mazeBuilder.SaveMaze();
            
        }

        public void OnShown(object passingObj)
        {
            Console.WriteLine("Added keydown events");
            var window = Window.GetWindow(this);
            window.KeyDown += Page_KeyDown;

            if (isWaitingOnQuestion)
            {
                isWaitingOnQuestion = false;

                bool answeredCorrectly = (((bool, int))passingObj).Item1;
                int questionId = (((bool, int))passingObj).Item2;
                if (answeredCorrectly)
                {
                    mazeBuilder.UnlockQuestion(questionId);
                    BuildCurrentLocation();     // TODO: May be heavy???
                    MoveToZ(1);
                }
                else
                {
                    SystemSounds.Beep.Play();
                    mazeBuilder.ShuffleAllQuestions(questionId);


                    BuildCurrentLocation();

                    acceptInput = true;
                    
                }
            }
            else if (isWaitingOnSetup)
            {
                isWaitingOnSetup = false;
                (bool newGame, int difficulty) parameters = ((bool, int))passingObj;
                if (parameters.newGame)
                {
                    SetupMaze(4, parameters.difficulty);
                }
                else
                {
                    LoadMazeFromSaveFile();
                }
            }
        }

        public void OnDisappeared()
        {
            Console.WriteLine("Removed keydown events");
            var window = Window.GetWindow(this);
            window.KeyDown -= Page_KeyDown;
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left) TurnLeft();
            else if (e.Key == Key.Right) TurnRight();
            else if (e.Key == Key.Up) DoAction();
        }



        private void UpdateIfCanMove()
        {
            btnAction.IsEnabled = IsDoor();
        }

        private bool IsDoor()
        {
            return !mazeBuilder.IsWall(currentLocation.x, currentLocation.y, currentDir);
        }

        private bool IsDoorPermalocked()
        {
            return mazeBuilder.IsDoorPermalocked(currentLocation.x, currentLocation.y, currentDir);
        }

        private bool IsDoorLocked()
        {
            int doorQ = mazeBuilder.GetQuestionId(
                            currentLocation.x,
                            currentLocation.y,
                            currentDir
                        );
            return mazeBuilder.IsQuestionLocked(doorQ);
        }

        private void TurnLeft()
        {
            if (!acceptInput) return;
            acceptInput = false;

            currentDir = CardinalDirsUtils.TurnLeft(currentDir);
            mazeBuilder.UpdatePlayerDir(currentDir);
            UpdateIfCanMove();
            Turn(-90);
        }

        private void TurnRight()
        {
            if (!acceptInput) return;
            acceptInput = false;

            currentDir = CardinalDirsUtils.TurnRight(currentDir);
            mazeBuilder.UpdatePlayerDir(currentDir);
            UpdateIfCanMove();
            Turn(90);
        }

        private bool isWaitingOnQuestion;
        private void DoAction()
        {
            isWaitingOnQuestion = false;
            if (IsDoor())
            {
                if (!acceptInput) return;
                acceptInput = false;

                if (IsDoorPermalocked())
                {
                    // Chew the player out for even thinking that they could have another chance!!! ;(
                    acceptInput = true;
                }
                else if (IsDoorLocked())
                {
                    isWaitingOnQuestion = true;
                    GuiMediator.Instance.ShowQuestionGui(
                        mazeBuilder.GetQuestionId(
                            currentLocation.x,
                            currentLocation.y,
                            currentDir
                        )
                    );
                }
                else
                {
                    MoveToZ(1);
                }
            }
        }


        private double targetAngle;
        private void Turn(double angle)
        {
            targetAngle += angle;
            currentAngle = GetLookRotation();
            new Thread(new ThreadStart(TurnAnimateAsync)).Start();
        }

        private delegate void UpdateSetLookRotation(double angle);
        private void SetLookRotation(double angle)
        {
            lookRotation.Angle = angle;
        }

        private double GetLookRotation(CardinalDirs direction)
        {
            switch (currentDir)
            {
                case CardinalDirs.NORTH:
                    SetLookRotation(0);
                    break;
                case CardinalDirs.SOUTH:
                    SetLookRotation(180);
                    break;
                case CardinalDirs.EAST:
                    SetLookRotation(90);
                    break;
                case CardinalDirs.WEST:
                    SetLookRotation(270);
                    break;
            }
            return GetLookRotation();
        }

        private double GetLookRotation()
        {
            return lookRotation.Angle;
        }

        private double currentAngle;
        private void TurnAnimateAsync()
        {
            int ticks = 30;
            double turnDelta = (targetAngle - currentAngle) / ticks;
            for (int i = 0; i < ticks; i++)
            {
                Thread.Sleep(THREAD_SLEEP);
                currentAngle += turnDelta;
                Dispatcher.Invoke(
                    new UpdateSetLookRotation(this.SetLookRotation),
                    new object[] { currentAngle }
                );
            }

            acceptInput = true;
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
            MoveRoomsAuto();
            Dispatcher.Invoke(
                new TriggerBuildCurrentLocation(this.BuildCurrentLocation),
                new object[] { }
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

            acceptInput = true;
            Dispatcher.Invoke(
                new UpdateCheckIfWonMaze(this.CheckIfWonMaze),
                new object[] { }
            );
        }

        private delegate void UpdateSetZPos(double z);

        private void SetZPos(double z)
        {
            var pt = camMain.Position;
            pt.Z = z;
            camMain.Position = pt;
        }

        private delegate void UpdateCheckIfWonMaze();

        private void CheckIfWonMaze()
        {
            if (currentLocation == mazeBuilder.GetGoalLoc())
            {
                Console.WriteLine("Yayyyy! You won!");
                GuiMediator.Instance.ShowWinningGui(null);
            }
        }
    }
}

