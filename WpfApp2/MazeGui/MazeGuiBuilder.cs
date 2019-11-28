using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace MazeRunnerWPF.MazeGui
{
    public interface IGuiPage
    {
        void OnShown(object passingObj);
        object OnDisappeared();
    }

    public enum TextureType
    {
        WALL, DOOR_UNLOCKED, DOOR_LOCKED, FLOOR, CEILING
    }

    public enum CardinalDirs
    {
        NORTH = 0, SOUTH = 1, EAST = 2, WEST = 3
    }

    public static class CardinalDirsUtils
    {
        public static CardinalDirs TurnLeft(CardinalDirs direction)
        {
            switch (direction)
            {
                case CardinalDirs.NORTH:
                    return CardinalDirs.WEST;
                case CardinalDirs.SOUTH:
                    return CardinalDirs.EAST;
                case CardinalDirs.EAST:
                    return CardinalDirs.NORTH;
                case CardinalDirs.WEST:
                    return CardinalDirs.SOUTH;
            }
            return CardinalDirs.NORTH;
        }

        public static CardinalDirs TurnRight(CardinalDirs direction)
        {
            switch (direction)
            {
                case CardinalDirs.NORTH:
                    return CardinalDirs.EAST;
                case CardinalDirs.SOUTH:
                    return CardinalDirs.WEST;
                case CardinalDirs.EAST:
                    return CardinalDirs.SOUTH;
                case CardinalDirs.WEST:
                    return CardinalDirs.NORTH;
            }
            return CardinalDirs.NORTH;
        }
    }

    public class MazeGuiBuilder
    {
        private Maze mazeStruct;

        public MazeGuiBuilder(int size)
        {

            Controller.MazeController.createMaze(3);
            mazeStruct = Controller.MazeController.getMaze();
        }

        public (int x, int y) GetEntranceLoc()
        {
            return mazeStruct.GetEntrance();
        }

        public void BuildRoomTextures(
            int gridX,
            int gridY,
            ref DiffuseMaterial northTex,
            ref DiffuseMaterial southTex,
            ref DiffuseMaterial westTex,
            ref DiffuseMaterial eastTex,
            ref DiffuseMaterial floorTex,
            ref DiffuseMaterial ceilTex)
        {
            AssignTexture(ref northTex, GetTexTypeFromLocationDirection(gridX, gridY, CardinalDirs.NORTH));
            AssignTexture(ref southTex, GetTexTypeFromLocationDirection(gridX, gridY, CardinalDirs.SOUTH));
            AssignTexture(ref westTex, GetTexTypeFromLocationDirection(gridX, gridY, CardinalDirs.WEST));
            AssignTexture(ref eastTex, GetTexTypeFromLocationDirection(gridX, gridY, CardinalDirs.EAST));
            AssignTexture(ref floorTex, TextureType.FLOOR);
            AssignTexture(ref ceilTex, TextureType.CEILING);
        }

        private void AssignTexture(ref DiffuseMaterial diffuseMaterial, TextureType type)
        {
            string texPath = @"./Assets/tex.png";
            switch (type)
            {
                case TextureType.WALL:
                    texPath = @"./Assets/tex.png";
                    break;
                case TextureType.DOOR_UNLOCKED:
                    texPath = @"./Assets/door_tex.png";
                    break;
                case TextureType.DOOR_LOCKED:
                    texPath = @"./Assets/door_tex_locked.png";
                    break;
                case TextureType.FLOOR:
                    texPath = @"./Assets/floor.png";
                    break;
                case TextureType.CEILING:
                    texPath = @"./Assets/ceil.png";
                    break;
            }

            diffuseMaterial.Brush =
                new ImageBrush(
                    new BitmapImage(new Uri(texPath, UriKind.Relative))
                );
        }

        public bool IsWall(int x, int y, CardinalDirs facingDirection)
        {
            TextureType type = GetTexTypeFromLocationDirection(x, y, facingDirection);
            return type == TextureType.WALL;
        }

        private (bool[,], int[,]) GetDirectionalWallInfo(CardinalDirs direction)
        {
            switch (direction)
            {
                case CardinalDirs.NORTH:
                    return (mazeStruct.NorthWall, mazeStruct.NorthQuestion);
                case CardinalDirs.SOUTH:
                    return (mazeStruct.SouthWall, mazeStruct.SouthQuestion);
                case CardinalDirs.EAST:
                    return (mazeStruct.EastWall, mazeStruct.EastQuestion);
                case CardinalDirs.WEST:
                    return (mazeStruct.WestWall, mazeStruct.WestQuestion);
            }

            return (null, null);
        }

        private bool IsQuestionLocked(int qIndex)
        {
            return mazeStruct.GetQuestion(qIndex).Locked();
        }

        private TextureType GetTexTypeFromLocationDirection(int x, int y, CardinalDirs direction)
        {
            (bool[,] walls, int[,] questions) refWalls = GetDirectionalWallInfo(direction);

            if (refWalls.walls[y, x])
                return TextureType.WALL;
            else
            {
                if (IsQuestionLocked(
                        refWalls.questions[y, x]))
                {
                    return TextureType.DOOR_LOCKED;
                }
                return TextureType.DOOR_UNLOCKED;
            }
        }
    }
}
