
using MazeRunnerWPF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


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


            maze.ChangeAllUnlockedQuestionAtLocation(location1);
            maze.ChangeAllUnlockedQuestionAtLocation(location2);


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


            maze.ResetUnlockedMazeQuestionsAndChangeWronglyAnsweredQuestion(0);
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

        [TestMethod]
        public void Maze_SavesCorrectly()
        {
            Maze maze = new Maze(3);

            Console.WriteLine($"Before serialization the object contains: size: {maze.Size}");

            string filePath = @"C:\Users\saffron\Desktop\mazeData.xml";

            // Opens a file and serializes the object into it in binary format.
            Stream stream = File.Open(filePath, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            //BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, maze);
            stream.Close();

            // Empties obj.
            maze = null;

            // Opens file "data.xml" and deserializes the object from it.
            stream = File.Open(filePath, FileMode.Open);
            formatter = new BinaryFormatter();

            //formatter = new BinaryFormatter();

            maze = (Maze)formatter.Deserialize(stream);
            stream.Close();

            Console.WriteLine("");
            Console.WriteLine($"After deserialization the object contains: size: {maze.Size} ");
            


        }

    }
}
