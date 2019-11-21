using System;

namespace MazeRunnerWPF
{

    class GamePlay
    {
        public static Maze TheMaze { get; private set; }
        static int MazeSize { get; set; } = 4;


        static void MazeRunnerMain(string[] args)
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


                //get the question
                if (TheMaze.EastQuestion[x, y] != -1)
                {
                    Console.WriteLine("Attempt East = 0");
                }
                if (TheMaze.WestQuestion[x, y] != -1)
                {
                    Console.WriteLine("Attempt West = 1");
                }
                if (TheMaze.NorthQuestion[x, y] != -1)
                {
                    Console.WriteLine("Attempt North = 2");
                }
                if (TheMaze.SouthQuestion[x, y] != -1)
                {
                    Console.WriteLine("Attempt South = 3");
                }

                int choice = Convert.ToInt32(Console.ReadLine());



                //ask question and move if succesful
                bool correctAnswer = false;
                switch (choice)
                {
                    case Direction.East:
                        if (Trebek.AskQuestion(TheMaze.EastQuestion[x, y]))
                        {
                            MovePlayer(Direction.East);
                            correctAnswer = true;
                            
                        }
                        break;
                    case Direction.West:
                        if (Trebek.AskQuestion(TheMaze.WestQuestion[x, y]))
                        {
                            MovePlayer(Direction.West);
                            correctAnswer = true;
                        }
                        break;
                    case Direction.North:
                        if (Trebek.AskQuestion(TheMaze.NorthQuestion[x, y]))
                        {
                            MovePlayer(Direction.North);
                            correctAnswer = true;
                        }
                        break;
                    case Direction.South:
                        if (Trebek.AskQuestion(TheMaze.SouthQuestion[x, y]))
                        {
                            MovePlayer(Direction.South);
                            correctAnswer = true;
                        }
                       
                        break;

                    default:
                        break;
                }

                if (!correctAnswer)
                {
                    //dont move rooms!
                }

                if (TheMaze.PlayerLocation == TheMaze._ExitCoordinates)
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
