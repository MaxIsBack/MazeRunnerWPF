using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunnerWPF.Controller
{
    static class MazeController
    {

        private static volatile Maze theMaze;

        internal static void CreateMaze(int size, int difficulty)
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

        internal static void SaveMaze(Maze mazeStruct)
        {
            string filePath = @"SavedGame\mazeData.xml";
            // Opens a file and serializes the object into it in binary format.
            Stream stream = File.Open(filePath, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

          

            formatter.Serialize(stream, mazeStruct);
            stream.Close();
        }

        internal static Maze LoadMaze(Maze mazeStruct)
        {
            string filePath = @"SavedGame\mazeData.xml";
             Stream stream = File.Open(filePath, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();

            

            theMaze = (Maze)formatter.Deserialize(stream);
            stream.Close();

            return theMaze;
        }
    }
}
