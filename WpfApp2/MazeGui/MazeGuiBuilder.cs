using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MazeRunnerWPF.MazeGui
{
    public class MazeGuiBuilder
    {
        private MazeStructure mazeStruct;

        public MazeGuiBuilder(int size)
        {
             mazeStruct = new MazeStructure(size);
        }

        public PointCollection BuildRoomTextureCoordinates(int x, int y)
        {
            // TODO: cycle thru north south east west and see if the door should be a locked tex coord or not
            return null;
        }

        private Point GetTexCoordFromType(int type)
        {
            // TODO: This will get the texture coord if it's a wall, or a door, or an unlocked door, etc.
            return new Point();
        }
    }
}
