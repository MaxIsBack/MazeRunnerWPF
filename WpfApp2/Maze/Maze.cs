using System;
using System.Collections.Generic;

namespace MazeRunnerWPF
{
    public class Maze
    {


        public int Size { get; private set; }
        public bool[,] NorthWall { get; private set; }    // is there a wall to north of cell i, j
        public bool[,] EastWall { get; private set; }
        public bool[,] SouthWall { get; private set; }
        public bool[,] WestWall { get; private set; }

        public int[,] NorthQuestion { get; private set; }    // contains the index that coresponds to the question in the Question List (MazeQuestions)
        public int[,] EastQuestion { get; private set; }
        public int[,] SouthQuestion { get; private set; }
        public int[,] WestQuestion { get; private set; }




        public (int x, int y) _EntranceCoordinates;
        public (int x, int y) _ExitCoordinates;

        public (int x, int y) PlayerLocation { get; set; }
        public bool[,] RoomDiscovered { get; private set; }

        private QuestionFactory _QuestionFactory = new QuestionFactory();

        internal void UnlockQuestion(int questionIndex)
        {
            Question temp = MazeQuestions[questionIndex];
            temp.Unlock();
            MazeQuestions[questionIndex]=temp;
        }

        public List<Question> MazeQuestions { get; private set; } = new List<Question>();




        private Random randomInt = new Random();

        public Maze(int size, params string[] questionArgs)
        {
            if (size < 3)
            {
                throw new ArgumentException("size must be more than 2", nameof(size));
            }

            this.Size = size;

            MazeStructure mazeStructure = new MazeStructure(Size, questionArgs);
            MazeQuestions = mazeStructure._QuestionsList;

            CopyMazeStructure(mazeStructure);

            mazeStructure = null; // clean up memory.

            setExits();

        }

    

        //returns true if locked false if not
        internal bool QuestionStatus(int questionIndex)
        {
            return MazeQuestions[questionIndex].Locked();
        }

        private void CopyMazeStructure(MazeStructure mazeStructure)
        {
            NorthQuestion = mazeStructure.NorthQuestion;
            EastQuestion = mazeStructure.EastQuestion;
            SouthQuestion = mazeStructure.SouthQuestion;
            WestQuestion = mazeStructure.WestQuestion;

            NorthWall = mazeStructure.NorthWall;
            EastWall = mazeStructure.EastWall;
            SouthWall = mazeStructure.SouthWall;
            WestWall = mazeStructure.WestWall;



        }

        

        public void ChangeQuestion( int QuestionIndex, params string[] questionArgs)
        {
            MazeQuestions[QuestionIndex] = _QuestionFactory.getQuestions(questionArgs, 1).Dequeue();
        }



        public void ChangeAllQuestionsInMaze((int x, int y) location, params string[] questionParams)
        {
            Queue<Question> newQuestions = _QuestionFactory.getQuestions(questionParams, Size * Size * 4);
            MazeQuestions = new List<Question>();

            InitializeQuestionLocationArraysWithDefaultQuestionIndex();



            if (!NorthWall[location.x, location.y])

            {

                if (ValidIndex(location.x - 1) && SouthQuestion[location.x - 1, location.y] != -1)
                {

                    NorthQuestion[location.x, location.y] = SouthQuestion[location.x - 1, location.y];

                }
                else
                {
                    MazeQuestions.Add(newQuestions.Dequeue());
                    NorthQuestion[location.x, location.y] = MazeQuestions.Count-1;
                }

            }

            if (!SouthWall[location.x, location.y])

            {

                if (ValidIndex(location.x + 1) && NorthQuestion[location.x + 1, location.y] != -1)
                {

                    SouthQuestion[location.x, location.y] = NorthQuestion[location.x + 1, location.y];

                }
                else
                {
                    MazeQuestions.Add(newQuestions.Dequeue());
                    SouthQuestion[location.x, location.y] = MazeQuestions.Count-1;
                }

            }

            if (!EastWall[location.x, location.y])

            {

                if (ValidIndex(location.y + 1) && WestQuestion[location.x, location.y + 1] != -1)
                {

                    EastQuestion[location.x, location.y] = WestQuestion[location.x, location.y + 1];

                }
                else
                {
                    MazeQuestions.Add(newQuestions.Dequeue());
                    EastQuestion[location.x, location.y] = MazeQuestions.Count-1;
                }

            }

            if (WestWall[location.x, location.y])

            {

                if (ValidIndex(location.y - 1) && EastQuestion[location.x, location.y - 1] != -1)
                {

                    WestQuestion[location.x, location.y] = EastQuestion[location.x, location.y - 1];

                }
                else
                {

                    MazeQuestions.Add(newQuestions.Dequeue());
                    WestQuestion[location.x, location.y] = MazeQuestions.Count-1;
                }

            }
        }

        private void InitializeQuestionLocationArraysWithDefaultQuestionIndex()
        {


            NorthQuestion = new int[Size, Size];
            EastQuestion = new int[Size, Size];
            SouthQuestion = new int[Size, Size];
            WestQuestion = new int[Size, Size];

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {

                    NorthQuestion[i, j] = -1;
                    EastQuestion[i, j] = -1;
                    SouthQuestion[i, j] = -1;
                    WestQuestion[i, j] = -1;
                }

            }



        }

        public void QuestionAnsweredCorrectly(Question question)
        {
            question.Unlock();
        }

      /*  public void QuestionAnsweredIncorrectly(int x, int y, int[,] questionList)
        {
            //questionList[y, x] = -1;
            questionList[x, y] = -1;
        }*/



        private void setExits()
        {
            _EntranceCoordinates.x = randomInt.Next(Size / 2);
            _EntranceCoordinates.y = randomInt.Next(Size / 2);

            int boundaryFactor;
            if (Size % 2 == 0)
            {
                boundaryFactor = (Size / 2) - 1;
            }
            else boundaryFactor = Size / 2;
            int exitX;
            int exitY = randomInt.Next(Size / 2) + boundaryFactor;
            do
            {
                exitX = randomInt.Next(Size / 2) + boundaryFactor;
            }
            while (exitX == _EntranceCoordinates.x);

            _ExitCoordinates.x = exitX;
            _ExitCoordinates.y = exitY;

        }

        public (int x, int y) GetEntrance()
        {
            return _EntranceCoordinates;
        }

        public (int x, int y) GetExit()
        {
            return _ExitCoordinates;
        }


        public int getSize()
        {
            return Size;
        }


        //all locked questions in that room will be changed
        public void ChangeAllQuestionAtLocation((int x, int y) location)
        {

            if (NorthQuestion[location.x, location.y] != -1 && MazeQuestions[ NorthQuestion[location.x, location.y]].Locked()) { ChangeQuestion(NorthQuestion[location.x, location.y]); }

            if (SouthQuestion[location.x, location.y] != -1 && MazeQuestions[ SouthQuestion[location.x, location.y]].Locked()) { ChangeQuestion(SouthQuestion[location.x, location.y]); }

            if (WestQuestion[location.x, location.y] != -1 && MazeQuestions[ WestQuestion[location.x, location.y]].Locked()) { ChangeQuestion( WestQuestion[location.x, location.y]); }

            if (EastQuestion[location.x, location.y] != -1 && MazeQuestions[ EastQuestion[location.x, location.y]].Locked()) { ChangeQuestion( EastQuestion[location.x, location.y]); }

        }

        public void ChangeAllUnlockedQuestionAtLocation((int x, int y) location)
        {

            if (NorthQuestion[location.x, location.y] != -1 && !MazeQuestions[NorthQuestion[location.x, location.y]].Locked()) { ChangeQuestion(NorthQuestion[location.x, location.y]); }

            if (SouthQuestion[location.x, location.y] != -1 && !MazeQuestions[SouthQuestion[location.x, location.y]].Locked()) { ChangeQuestion(SouthQuestion[location.x, location.y]); }

            if (WestQuestion[location.x, location.y] != -1 && !MazeQuestions[WestQuestion[location.x, location.y]].Locked()) { ChangeQuestion(WestQuestion[location.x, location.y]); }

            if (EastQuestion[location.x, location.y] != -1 && !MazeQuestions[EastQuestion[location.x, location.y]].Locked()) { ChangeQuestion(EastQuestion[location.x, location.y]); }

        }

        public bool ValidIndex(int index)
        {
            return index >= 0 && index < Size;
        }

        public Question GetQuestion(int index) {

            return MazeQuestions[index];
        }




        public void ChangeAllUnlockedQuestionsInMaze((int x, int y) location, params string[] questionParams)
        {

            ChangeAllQuestionAtLocation(location);

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {

                    ChangeAllUnlockedQuestionAtLocation((x, y));
                    
                }
            }
        }





        public int[,] NorthQuestionCounting { get; private set; }     // contains the index that coresponds to the question in the Question List (MazeQuestions)
        public int[,] EastQuestionCounting { get; private set; }
        public int[,] SouthQuestionCounting { get; private set; }
        public int[,] WestQuestionCounting { get; private set; }
        public int CountLockedQuestions()
        {

            NorthQuestionCounting = new int[Size, Size];
            EastQuestionCounting = new int[Size, Size];
            SouthQuestionCounting = new int[Size, Size];
            WestQuestionCounting = new int[Size, Size];


            SetQuestionToDefault(Size);


            int count = 0;
            string[] questionArgs = { "e" };

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {

                    if (!NorthWall[x, y])

                    {

                        if (ValidIndex(x - 1) && SouthQuestionCounting[x - 1, y] != -1)
                        {
                            NorthQuestion[x, y] = SouthQuestion[x - 1, y];


                        }
                        else
                        {
                            
                            NorthQuestion[x, y] = 1;
                            count++;
                        }

                    }

                    if (!SouthWall[x, y])

                    {

                        if (ValidIndex(x + 1) && NorthQuestionCounting[x + 1, y] != 0)
                        {

                            SouthQuestionCounting[x, y] = NorthQuestionCounting[x + 1, y];

                        }
                        else
                        {
                            // MazeQuestions.Add(newQuestions.Dequeue());
                            SouthQuestionCounting[x, y] = 1;
                            count++;
                        }

                    }

                    if (!EastWall[x, y])

                    {

                        if (ValidIndex(y + 1) && WestQuestionCounting[x, y + 1] != 0)
                        {

                            EastQuestionCounting[x, y] = WestQuestionCounting[x, y + 1];

                        }
                        else
                        {
                            //MazeQuestions.Add(newQuestions.Dequeue());
                            EastQuestionCounting[x, y] = 1;
                            count++;
                        }

                    }

                    if (WestWall[x, y])

                    {

                        if (ValidIndex(y - 1) && EastQuestionCounting[x, y - 1] != 0)
                        {

                            WestQuestionCounting[x, y] = EastQuestionCounting[x, y - 1];

                        }
                        else
                        {

                            //MazeQuestions.Add(newQuestions.Dequeue());
                            WestQuestionCounting[x, y] = 1;
                            count++;
                        }

                    }

                }
            }

            Console.WriteLine($"Count= {count}");
            return count;

        }

        private void SetQuestionToDefault(int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {

                    NorthQuestionCounting[i, j] = 0;
                    EastQuestionCounting[i, j] = 0;
                    SouthQuestionCounting[i, j] = 0;
                    WestQuestionCounting[i, j] = 0;
                }

            }

        }


      






    }


    public static class Direction
    {
        public const int East = 0;
        public const int West = 1;
        public const int North = 2;
        public const int South = 3;
        
    }



   

}
