using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace MazeRunnerWPF.MazeGui
{
    public interface IGuiPage
    {
        void OnShown(object passingObj);
        void OnDisappeared();
    }

    public enum TextureType
    {
        WALL, DOOR_UNLOCKED, DOOR_LOCKED, DOOR_PERMALOCKED, FLOOR, CEILING
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

        public MazeGuiBuilder(int size, int difficulty)
        {
            Controller.MazeController.CreateMaze(size, difficulty);
            mazeStruct = Controller.MazeController.getMaze();
        }

        public (int x, int y) GetEntranceLoc()
        {
            return mazeStruct.GetEntrance();
        }

        public (int x, int y) GetGoalLoc()
        {
            return mazeStruct.GetExit();
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
                case TextureType.DOOR_PERMALOCKED:
                    texPath = @"./Assets/stu.png";      // TODO: Change this to an actual texture
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

        public void UnlockQuestion(int questionId)
        {
            mazeStruct.UnlockQuestion(questionId);
        }

        public void ShuffleAllQuestions((int x, int y) location)
        {
            mazeStruct.ChangeAllQuestionAtLocation((location.y, location.x));
        }

        public int GetQuestionId(int x, int y, CardinalDirs facingDirection)
        {
            (bool[,] walls, int[,] questions) refWalls = GetDirectionalWallInfo(facingDirection);
            return refWalls.questions[y, x];
        }

        public void LockDoorWhenQuestionAnsweredIncorrectly(int x, int y, CardinalDirs facingDirection)
        {
            (bool[,] _, int[,] questions) refWalls = GetDirectionalWallInfo(facingDirection);
            mazeStruct.QuestionAnsweredIncorrectly(x, y, refWalls.questions);
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

        public bool IsQuestionLocked(int questionId)
        {
            return mazeStruct.GetQuestion(questionId).Locked();
        }

        public bool IsDoorPermalocked(int x, int y, CardinalDirs facingDirection)
        {
            return GetTexTypeFromLocationDirection(x, y, facingDirection) == TextureType.DOOR_PERMALOCKED;
        }

        private TextureType GetTexTypeFromLocationDirection(int x, int y, CardinalDirs direction)
        {
            (bool[,] walls, int[,] questions) refWalls = GetDirectionalWallInfo(direction);

            if (refWalls.walls[y, x])
                return TextureType.WALL;
            else
            {
                int questionId = refWalls.questions[y, x];
                if (questionId < 0)
                {
                    return TextureType.DOOR_PERMALOCKED;
                }
                else if (IsQuestionLocked(questionId))
                {
                    return TextureType.DOOR_LOCKED;
                }
                return TextureType.DOOR_UNLOCKED;
            }
        }
    }
}
