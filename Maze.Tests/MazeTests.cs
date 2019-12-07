
using MazeRunnerWPF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MazeTesting
{
    [TestClass]
    public class MazeTests
    {

        [TestMethod]
        public void PrintTestMaze()
        {
           
        }

        [TestMethod]
        public void North_South_Questions_Are_The_Same_On_Either_Side_Of_Door()
        {
            int n = 3;
            Maze maze = new Maze(n);
            //maze.testDraw();

            //maze.testDraw();

            for (int i = 1; i < maze.Size; i++)
            {
                for (int j = 0; j < maze.Size; j++)
                {
                    if (maze.ValidIndex(i - 1))
                    {
                        if (!maze.NorthWall[i, j])
                        {
                            Console.WriteLine($"({i},{j}) north question {maze.GetQuestion(maze.NorthQuestion[i, j]).CorrectAnswer} matches south question {maze.MazeQuestions[maze.SouthQuestion[i - 1, j]].CorrectAnswer} of ({i - 1},{j})");
                            Assert.IsTrue(ReferenceEquals(maze.GetQuestion(maze.NorthQuestion[i, j]), maze.GetQuestion(maze.SouthQuestion[i - 1, j])));

                        }
                    }
                }
            }


        }

        [TestMethod]
        public void North_South_Questions_Are_The_Same_On_Either_Side_Of_Door_After_Changing_A_Question()
        {
            int n = 3;
            Maze maze = new Maze(n);
            //maze.testDraw();

            //maze.testDraw();

            for (int i = 1; i < maze.Size; i++)
            {
                for (int j = 0; j < maze.Size; j++)
                {
                    if (maze.ValidIndex(i - 1))
                    {
                        if (!maze.NorthWall[i, j])
                        {
                            maze.ChangeQuestion(maze.NorthQuestion[i, j]);

                            Console.WriteLine($"({i},{j}) north question -{maze.MazeQuestions[maze.NorthQuestion[i, j]].CorrectAnswer}- matches south question {maze.MazeQuestions[maze.SouthQuestion[i - 1, j]].CorrectAnswer} of ({i - 1},{j})");
                            Assert.IsTrue(ReferenceEquals(maze.GetQuestion(maze.NorthQuestion[i, j]), maze.GetQuestion(maze.SouthQuestion[i - 1, j])));

                        }
                    }
                }
            }





        }

        [TestMethod]
        public void Change_All_Questions_In_A_Room_Changes_Adjacent_Rooms()
        {
            Maze maze = new Maze(3);
            (int x, int y) location1 = (0, 0);
            (int x, int y) location2 = (0, 1);


            maze.ChangeAllQuestionAtLocation(location1);
            maze.ChangeAllQuestionAtLocation(location2);


            for (int i = 1; i < maze.Size; i++)
            {
                for (int j = 0; j < maze.Size; j++)
                {
                    if (maze.ValidIndex(i - 1))
                    {
                        if (!maze.NorthWall[i, j])
                        {
                            Console.WriteLine($"({i},{j}) north question {maze.GetQuestion(maze.NorthQuestion[i, j]).CorrectAnswer} matches south question {maze.MazeQuestions[maze.SouthQuestion[i - 1, j]].CorrectAnswer} of ({i - 1},{j})");
                            Assert.IsTrue(ReferenceEquals(maze.GetQuestion(maze.NorthQuestion[i, j]), maze.GetQuestion(maze.SouthQuestion[i - 1, j])));

                        }
                    }
                }
            }


        }



        [TestMethod]
        public void Change_All_Locked_Questions_In_A_Maze_Changes_Adjacent_Rooms()
        {
            Maze maze = new Maze(3);
            (int x, int y) location1 = (0, 0);
            //(int x, int y) location2 = (0, 1);


            maze.ChangeAllUnlockedQuestionsInMaze(location1);
            //maze.ChangeAllQuestionAtLocation(location2);


            for (int i = 1; i < maze.Size; i++)
            {
                for (int j = 0; j < maze.Size; j++)
                {
                    if (maze.ValidIndex(i - 1))
                    {
                        if (!maze.NorthWall[i, j])
                        {
                            Console.WriteLine($"({i},{j}) north question {maze.GetQuestion(maze.NorthQuestion[i, j]).CorrectAnswer} matches south question {maze.MazeQuestions[maze.SouthQuestion[i - 1, j]].CorrectAnswer} of ({i - 1},{j})");
                            Assert.IsTrue(ReferenceEquals(maze.GetQuestion(maze.NorthQuestion[i, j]), maze.GetQuestion(maze.SouthQuestion[i - 1, j])));

                        }
                    }
                }
            }


        }




    }
}
