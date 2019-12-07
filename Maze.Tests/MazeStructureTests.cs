using MazeRunnerWPF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MazeTesting
{
    [TestClass]
    public class MazeStructureTests
    {

        [TestMethod]
        public void PrintTestMaze()
        {
            int n = 3;
            MazeStructure maze = new MazeStructure(n);
            //maze.testDraw();

            maze.testDraw();



        }

        [TestMethod]
        public void North_South_Questions_Are_The_Same_On_Either_Side_Of_Door()
        {
            int n = 3;
            MazeStructure maze = new MazeStructure(n);
            maze.testDraw();

            //maze.testDraw();

            for (int i = 1; i < maze.size; i++)
            {
                for (int j = 0; j < maze.size; j++)
                {
                    if (maze.ValidIndex(i - 1))
                    {
                        if (!maze.NorthWall[i, j])
                        {
                            Console.WriteLine($"({i},{j}) north question {maze._QuestionsList[maze.NorthQuestion[i, j]].CorrectAnswer} matches south question {maze._QuestionsList[maze.SouthQuestion[i - 1, j]].CorrectAnswer} of ({i - 1},{j})");
                            Assert.IsTrue(ReferenceEquals(maze._QuestionsList[maze.NorthQuestion[i, j]], maze._QuestionsList[maze.SouthQuestion[i - 1, j]]));

                        }
                    }
                }
            }





        }






        [TestMethod]
        public void East_West_Questions_Are_The_Same_On_Either_Side_Of_Door()
        {
            int n = 3;
            MazeStructure maze = new MazeStructure(n);
            //maze.testDraw();

            maze.testDraw();

            for (int i = 1; i < maze.size; i++)
            {
                for (int j = 0; j < maze.size; j++)
                {
                    if (maze.ValidIndex(j + 1))
                    {
                        if (!maze.EastWall[i, j])
                        {
                            Console.WriteLine($"({i},{j}) east question {maze._QuestionsList[maze.EastQuestion[i, j]].CorrectAnswer} matches west question {maze._QuestionsList[maze.WestQuestion[i, j + 1]].CorrectAnswer} of ({i},{j + 1})");
                            Assert.IsTrue(ReferenceEquals(maze._QuestionsList[maze.EastQuestion[i, j]], maze._QuestionsList[maze.WestQuestion[i, j + 1]]));

                        }
                    }
                }
            }





        }



        [TestMethod]
        public void QuestionCounterWorks()
        {
          




        }



    }
}
