using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MazeRunnerWPF.MazeGui
{
    public enum TextureType
    {
        CEILING, FLOOR, WALL, DOOR_UNLOCKED, DOOR_LOCKED
    }

    enum CardinalDirs
    {
        NORTH, SOUTH, EAST, WEST
    }

    public class MazeGuiBuilder
    {
        private Maze mazeStruct;

        public MazeGuiBuilder(int size)
        {
            mazeStruct = new Maze(size);
        }

        public (int x, int y) GetEntranceLoc()
        {
            return mazeStruct.getEntrance();
        }

        public PointCollection BuildRoomTextureCoordinates(int x, int y)
        {
            var ptCollect = new PointCollection();

            // Order: Front, back, left, right, bottom, top
            AddArrayPointsToCollection(ref ptCollect, GetTexCoordFromType(GetTexTypeFromLocationDirection(x, y, CardinalDirs.NORTH)));
            AddArrayPointsToCollection(ref ptCollect, GetTexCoordFromType(GetTexTypeFromLocationDirection(x, y, CardinalDirs.SOUTH)));
            AddArrayPointsToCollection(ref ptCollect, GetTexCoordFromType(GetTexTypeFromLocationDirection(x, y, CardinalDirs.WEST)));
            AddArrayPointsToCollection(ref ptCollect, GetTexCoordFromType(GetTexTypeFromLocationDirection(x, y, CardinalDirs.EAST)));
            AddArrayPointsToCollection(ref ptCollect, GetTexCoordFromType(TextureType.FLOOR));
            AddArrayPointsToCollection(ref ptCollect, GetTexCoordFromType(TextureType.CEILING));

            return ptCollect;
        }

        private void AddArrayPointsToCollection(ref PointCollection ptCollect, Point[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                ptCollect.Add(points[i]);
            }
        }

        private TextureType GetTexTypeFromLocationDirection(int x, int y, CardinalDirs direction)
        {
            bool[,] refWalls = null;

            switch (direction)
            {
                case CardinalDirs.NORTH:
                    refWalls = mazeStruct.NorthWall;
                    break;
                case CardinalDirs.SOUTH:
                    refWalls = mazeStruct.SouthWall;
                    break;
                case CardinalDirs.EAST:
                    refWalls = mazeStruct.EastWall;
                    break;
                case CardinalDirs.WEST:
                    refWalls = mazeStruct.WestWall;
                    break;
            }

            if (refWalls[x, y])
                return TextureType.WALL;
            else
                return TextureType.DOOR_LOCKED;
        }

        private Point[] GetTexCoordFromType(TextureType type)
        {
            double texUnitU = 1;
            double texUnitV = 4;

            Point[] points = new Point[4];
            points[0] = new Point(0, 0);
            points[1] = new Point(texUnitU, 0);
            points[2] = new Point(0, texUnitV);
            points[3] = new Point(texUnitU, texUnitV);

            double offsetU = 0, offsetV = 0;
            switch (type)
            {
                case TextureType.CEILING:
                    break;
                case TextureType.FLOOR:
                    break;
                case TextureType.WALL:
                    offsetU = 0;
                    offsetV = 0;
                    break;
                case TextureType.DOOR_UNLOCKED:
                    break;
                case TextureType.DOOR_LOCKED:
                    offsetU = 1;
                    offsetV = 0;
                    break;
            }

            for (int i = 0; i < points.Length; i++)
            {
                points[i].X += offsetU;
                points[i].Y += offsetV;
            }
            return points;
        }
    }
}
