using System;

namespace MazeRunnerWPF
{

    public class GamePlay
    {
        public static Maze TheMaze { get; private set; }
        static int MazeSize { get; set; } = 4;


        public static void MazeRunnerMain(string[] args)
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

            TheMaze = new Maze(MazeSize, mazeArgs);

            TheMaze.PlayerLocation = TheMaze._EntranceCoordinates;

            Play();



        }

        private static void Play()
        {

            DisplayQuestionOptions();



        }


        private static void DisplayQuestionOptions()
        {

            while (true)
            {

                int x = TheMaze.PlayerLocation.x;
                int y = TheMaze.PlayerLocation.y;

                Console.WriteLine($"current location: {x},{y}");


                //display possible ways to go
                if (TheMaze.EastQuestion[x, y] != -1)
                {
                    Console.WriteLine("Move East = 0");
                }
                if (TheMaze.WestQuestion[x, y] != -1)
                {
                    Console.WriteLine("Move West = 1");
                }
                if (TheMaze.NorthQuestion[x, y] != -1)
                {
                    Console.WriteLine("Move North = 2");
                }
                if (TheMaze.SouthQuestion[x, y] != -1)
                {
                    Console.WriteLine("Move South = 3");
                }

                int choice = Convert.ToInt32(Console.ReadLine());



                //ask question if question is locked and move if succesfull
                bool correctAnswer = false;
                switch (choice)
                {
                    case Direction.East:
                        if (TheMaze.QuestionStatus(TheMaze.EastQuestion[x, y]))
                        {
                            if (Trebek.AskQuestion(TheMaze.EastQuestion[x, y]))
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
                        if (TheMaze.QuestionStatus(TheMaze.WestQuestion[x, y]))
                        {
                            if (Trebek.AskQuestion(TheMaze.WestQuestion[x, y]))
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
                        if (TheMaze.QuestionStatus(TheMaze.NorthQuestion[x, y]))
                        {
                            if (Trebek.AskQuestion(TheMaze.NorthQuestion[x, y]))
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
                        if (TheMaze.QuestionStatus(TheMaze.SouthQuestion[x, y]))
                        {
                            if (Trebek.AskQuestion(TheMaze.SouthQuestion[x, y]))
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

                if (TheMaze.PlayerLocation.x == TheMaze._ExitCoordinates.x && TheMaze.PlayerLocation.y == TheMaze._ExitCoordinates.y)
                {
                    Console.WriteLine("YouWin");
                    return;
                }

            }

        }



        static void MovePlayer(int direction)
        {
            int x = TheMaze.PlayerLocation.x;
            int y = TheMaze.PlayerLocation.y;

            switch (direction)
            {
                case Direction.East:
                    TheMaze.PlayerLocation = (x, y + 1);
                    break;
                case Direction.West:
                    TheMaze.PlayerLocation = (x, y - 1);
                    break;
                case Direction.North:
                    TheMaze.PlayerLocation = (x - 1, y);
                    break;
                case Direction.South:
                    TheMaze.PlayerLocation = (x + 1, y);
                    break;
                default:
                    break;
            }
        }
    }
}
