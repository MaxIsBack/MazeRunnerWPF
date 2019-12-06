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

        internal static void CreateMaze(int size)
        {
            if (theMaze == null)
            {
                theMaze = new Maze(size);
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
