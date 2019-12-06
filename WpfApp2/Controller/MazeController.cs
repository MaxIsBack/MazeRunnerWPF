using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunnerWPF.Controller
{
     static class MazeController
    {

        private static volatile Maze theMaze;

        internal static void createMaze(int size, int difficulty)
        {
            if (theMaze == null)
            {
                theMaze = new Maze(size, difficulty.ToString());
            }
        }
        internal static Maze MazeStruct()
        {
            return theMaze;
        }
        internal static Question Questioner(int questionIndex)
        {
            return theMaze.GetQuestion(questionIndex);
        }
        public static Maze getMaze()
        {
            return theMaze;
        }

    }
}
