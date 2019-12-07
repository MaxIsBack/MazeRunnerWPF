using MazeRunnerWPF.Controller;
using System;

namespace MazeRunnerWPF
{

    public class GamePlay
    {
        private static Maze theMaze;
        static int MazeSize { get; set; } = 4;


        public void MazeRunnerMain(string[] args)
        {

            Console.WriteLine("Welcome to Maze Runner. select your level");

            //get input selection from gui
            int levelSelection = 0;
            string[] mazeArgs;

            switch (levelSelection)
            {
                case 0:
                    mazeArgs = new string[] { "0" };
                    break;
                default:
                    mazeArgs = new string[] { "0" };
                    break;
            }

            theMaze =  MazeController.getMaze();
            // Maze(MazeSize, mazeArgs);

            theMaze.SetPlayerLocation(theMaze.GetEntrance().x, theMaze.GetEntrance().y);

            Play();



        }

        private static void Play()
        {

            DisplayQuestionOptions();



        }

        public Maze GetMaze()
        {
            return theMaze;
        }

        private static void DisplayQuestionOptions()
        {

            while (true)
            {

                int x = theMaze.PlayerLocation.x;
                int y = theMaze.PlayerLocation.y;

                Console.WriteLine($"current location: {x},{y}");


                //display possible ways to go
                if (theMaze.EastQuestion[x, y] != -1)
                {
                    Console.WriteLine("Move East = 0");
                }
                if (theMaze.WestQuestion[x, y] != -1)
                {
                    Console.WriteLine("Move West = 1");
                }
                if (theMaze.NorthQuestion[x, y] != -1)
                {
                    Console.WriteLine("Move North = 2");
                }
                if (theMaze.SouthQuestion[x, y] != -1)
                {
                    Console.WriteLine("Move South = 3");
                }

                int choice = Convert.ToInt32(Console.ReadLine());



                //ask question if question is locked and move if succesfull
                bool correctAnswer = false;
                switch (choice)
                {
                    case Direction.East:
                        if (theMaze.QuestionStatus(theMaze.EastQuestion[x, y]))
                        {
                            if (Trebek.AskQuestion(theMaze.EastQuestion[x, y]))
                            {
                                MovePlayer(Direction.East);
                                correctAnswer = true;
                            }

                        }
                        else
                        {
                            MovePlayer(Direction.East);
                        }
                        break;
                    case Direction.West:
                        if (theMaze.QuestionStatus(theMaze.WestQuestion[x, y]))
                        {
                            if (Trebek.AskQuestion(theMaze.WestQuestion[x, y]))
                            {
                                MovePlayer(Direction.West);
                                correctAnswer = true;
                            }
                        }
                        else
                        {
                            MovePlayer(Direction.West);
                        }
                        break;
                    case Direction.North:
                        if (theMaze.QuestionStatus(theMaze.NorthQuestion[x, y]))
                        {
                            if (Trebek.AskQuestion(theMaze.NorthQuestion[x, y]))
                            {
                                MovePlayer(Direction.North);
                                correctAnswer = true;
                            }
                        }
                        else
                        {
                            MovePlayer(Direction.North);
                        }
                        break;
                    case Direction.South:
                        if (theMaze.QuestionStatus(theMaze.SouthQuestion[x, y]))
                        {
                            if (Trebek.AskQuestion(theMaze.SouthQuestion[x, y]))
                            {
                                MovePlayer(Direction.South);
                                correctAnswer = true;
                            }
                        }
                        else
                        {
                            MovePlayer(Direction.South);
                        }

                        break;

                    default:
                        break;
                }

                if (!correctAnswer)
                {
                    //dont move rooms!
                }

                if (theMaze.PlayerLocation.x == theMaze.GetExit().x && theMaze.PlayerLocation.y == theMaze.GetExit().y)
                {
                    Console.WriteLine("YouWin");
                    return;
                }

            }

        }



        internal static void MovePlayer(int direction)
        {
            int x = theMaze.PlayerLocation.x;
            int y = theMaze.PlayerLocation.y;

            switch (direction)
            {
                case Direction.East:
                    theMaze.SetPlayerLocation(x, y + 1);
                    break;
                case Direction.West:
                    theMaze.SetPlayerLocation(x, y - 1);
                    break;
                case Direction.North:
                    theMaze.SetPlayerLocation(x - 1, y);
                    break;
                case Direction.South:
                    theMaze.SetPlayerLocation(x + 1, y);
                    break;
                default:
                    break;
            }
        }
    }
}
