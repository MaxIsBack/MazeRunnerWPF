


using System;
using System.Collections.Generic;
using System.Text;

namespace MazeRunnerWPF
{

    public class MazeStructure
    {
        public int size;                 // dimension of maze

        //---------------- Algorithm variables -------------
        private bool[,] _NorthWall;     // is there a wall to north of cell i, j
        private bool[,] _EastWall;
        private bool[,] _SouthWall;
        private bool[,] _WestWall;

        private bool[,] visited;
        //------------------end Algorithm variables----------------------

        public bool[,] NorthWall { get; private set; }    // is there a wall to north of cell i, j
        public bool[,] EastWall { get; private set; }
        public bool[,] SouthWall { get; private set; }
        public bool[,] WestWall { get; private set; }

        public int[,] NorthQuestion { get; private set; }    // is there a Question north of cell i, j 
        public int[,] EastQuestion { get; private set; }
        public int[,] SouthQuestion { get; private set; }
        public int[,] WestQuestion { get; private set; }

        public List<Question> _QuestionsList { get; } = new List<Question>();



        private int[] entranceCoodinates = new int[2];
        private int[] exitCoordinates = new int[2];
        private QuestionFactory questionFactory = new QuestionFactory();
        private Queue<Question> _QuestionQueue;



        public string[,] testMaze;

        //gui neeeds to know:
        //walls, questions, location.


        public MazeStructure(int size, params string[] questionArgs)
        {
            if (size < 3)
            {
                throw new ArgumentException("size must be more than 2", nameof(size));
            }



            this.size = size;
            /*StdDraw.setXscale(0, n+2);
            StdDraw.setYscale(0, n+2);*/
            questionFactory.SetGameMode(questionArgs);
            _QuestionQueue = questionFactory.getQuestions(size*size*3);
            testMaze = new string[size + 2, size + 2];

            Initialize();

            generate();
            getWalls();
          



            SetExits();

        }


        public int[] getEntrance()
        {
            return this.entranceCoodinates;
        }
        
        public int[] getExit()
        {
            return this.exitCoordinates;
        }

        private void Initialize()
        {
            // initialize border cells as already visited
            visited = new bool[size + 2, size + 2];
            for (int x = 0; x < size + 2; x++)
            {
                visited[x, 0] = true;
                visited[x, size + 1] = true;
            }
            for (int y = 0; y < size + 2; y++)
            {
                visited[0, y] = true;
                visited[size + 1, y] = true;
            }



            // initialze all walls as present
            _NorthWall = new bool[size + 2, size + 2];
            _EastWall = new bool[size + 2, size + 2];
            _SouthWall = new bool[size + 2, size + 2];
            _WestWall = new bool[size + 2, size + 2];
            for (int x = 0; x < size + 2; x++)
            {
                for (int y = 0; y < size + 2; y++)
                {
                    _NorthWall[x, y] = true;
                    _EastWall[x, y] = true;
                    _SouthWall[x, y] = true;
                    _WestWall[x, y] = true;
                }
            }
        }


        // generate the maze
        private void Generate(int x, int y)
        {
            visited[x, y] = true;
            Random rand = new Random();

            // while there is an unvisited neighbor
            while (!visited[x, y + 1] || !visited[x + 1, y] || !visited[x, y - 1] || !visited[x - 1, y])
            {

                // pick random neighbor (could use Knuth's trick instead)
                Random random = new Random();
                while (true)
                {
                    // double r = StdRandom.uniform(4);
                    int r = random.Next(4);
                    if (r == 0 && !visited[x, y + 1])
                    {
                        _NorthWall[x, y] = false;
                        _SouthWall[x, y + 1] = false;
                        Generate(x, y + 1);
                        break;
                    }
                    else if (r == 1 && !visited[x + 1, y])
                    {
                        _EastWall[x, y] = false;
                        _WestWall[x + 1, y] = false;
                        Generate(x + 1, y);
                        break;
                    }
                    else if (r == 2 && !visited[x, y - 1])
                    {
                        _SouthWall[x, y] = false;
                        _NorthWall[x, y - 1] = false;
                        Generate(x, y - 1);
                        break;
                    }
                    else if (r == 3 && !visited[x - 1, y])
                    {
                        _WestWall[x, y] = false;
                        _EastWall[x - 1, y] = false;
                        Generate(x - 1, y);
                        break;
                    }
                }
            }

        }

        // generate the maze starting from lower left
        private void Generate()
        {
            Generate(1, 1);

            Random random = new Random();
            //delete some random walls
            for (int i = 0; i < size; i++)
            {
                int x = 1 + random.Next(size - 1);
                int y = 1 + random.Next(size - 1);
                _NorthWall[x, y] = _SouthWall[x, y + 1] = false;
            }

        }

        private void SetExits()
        {
            Random randomInt = new Random();
            entranceCoodinates[0] = randomInt.Next(size / 2);
            entranceCoodinates[1] = randomInt.Next(size / 2);

            int boundaryFactor;
            if (size % 2 == 0)
            {
                boundaryFactor = (size / 2) - 1;
            }
            else boundaryFactor = size / 2;
            int exitX;
            int exitY = randomInt.Next(size / 2) + boundaryFactor;
            do
            {
                exitX = randomInt.Next(size / 2) + boundaryFactor;
            }
            while (exitX == entranceCoodinates[0]);

            exitCoordinates[0] = exitX;
            exitCoordinates[1] = exitY;
        }

        // draw the maze a reference method to how we might do the gui draw.
        public string[,] Draw()
        {
            /* StdDraw.setPenColor(StdDraw.RED);
             StdDraw.filledCircle(n/2.0 + 0.5, n/2.0 + 0.5, 0.375);
             StdDraw.filledCircle(1.5, 1.5, 0.375);


             StdDraw.setPenColor(StdDraw.BLACK);*/

            string[,] wallLocations = new string[size, size];
            // for (int row =0; row <n; row++){
            for (int x = 1; x <= size; x++)
            {
                //for (int col= 0; col<n;col++){
                for (int y = 1; y <= size; y++)
                {
                    StringBuilder wallLocationsstring = new StringBuilder();
                    if (_SouthWall[x, y])
                    {//StdDraw.line(x, y, x+1, y);
                        wallLocationsstring.Append('S');
                    }
                    if (_NorthWall[x, y])
                    {//StdDraw.line(x, y+1, x+1, y+1);
                        wallLocationsstring.Append('N');
                    }
                    if (_WestWall[x, y])
                    {//StdDraw.line(x, y, x, y+1);
                        wallLocationsstring.Append('W');
                    }
                    if (_EastWall[x, y])
                    {
                        //StdDraw.line(x + 1, y, x + 1, y + 1);
                        wallLocationsstring.Append('E');
                    }
                    //System.out.println(wallLocationsstring.tostring().toCharArray());
                    wallLocations[x - 1, y - 1] = wallLocationsstring.ToString();

                }
                // }
            }
            //}

            //System.out.println(Arrays.deepTostring(wallLocations));

            //System.out.println(theTestDungeon);
            /* StdDraw.show();
             StdDraw.pause(1000);*/
            return wallLocations;
        }


        public void TestDraw()
        {
            /* StdDraw.setPenColor(StdDraw.RED);
             StdDraw.filledCircle(n/2.0 + 0.5, n/2.0 + 0.5, 0.375);
             StdDraw.filledCircle(1.5, 1.5, 0.375);


             StdDraw.setPenColor(StdDraw.BLACK);*/


            // for (int row =0; row <n; row++){
            for (int x = 0; x < size; x++)
            {
                //for (int col= 0; col<n;col++){
                for (int y = 0; y < size; y++)
                {
                    Console.Write($"({ x },{y}) has: ");
                    if (SouthWall[x, y])
                    {//StdDraw.line(x, y, x+1, y);
                        //testMaze[x - 1, y] = "*";
                        Console.Write("southWall, ");
                    }
                    else
                    {
                        //testMaze[x - 1, y] = "?";
                        Console.Write("south ?, ");
                    }


                    if (NorthWall[x, y])
                    {//StdDraw.line(x, y+1, x+1, y+1);
                        //testMaze[x + 1, y] = "*";
                        Console.Write("northWall, ");
                    }
                    else
                    {
                        Console.Write("north ?, ");
                    }


                    if (WestWall[x, y])
                    {//StdDraw.line(x, y, x, y+1);
                        //testMaze[x, y - 1] = "*";
                        Console.Write("westWall, ");
                    }
                    else
                    {
                        //testMaze[x, y - 1] = "?";
                        Console.Write("west ?, ");
                    }


                    if (EastWall[x, y])
                    {
                        //StdDraw.line(x + 1, y, x + 1, y + 1);
                        //testMaze[x, y + 1] = "*";
                        Console.Write("eastWall, ");
                    }
                    else
                    {
                        // testMaze[x, y + 1] = "?";
                        Console.Write("east ?, ");
                    }


                }
                Console.WriteLine();
                // }
            }

            //Print2DArray(testMaze);
        }




        // a test client


        private string[,] ConvertAlgorithmMazeToRowColFormat(string[,] wallLocations)
        {

            List<string> formatted = new List<string>();

            for (int colUnformatted = size - 1; colUnformatted >= 0; colUnformatted--)
            {
                for (int rowUnformatted = 0; rowUnformatted < size; rowUnformatted++)
                {
                    formatted.Add(wallLocations[rowUnformatted, colUnformatted]);
                }
            }

            string[,] formattedWallLocations = new string[size, size];

            int i = 0;
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    formattedWallLocations[row, col] = formatted[i];
                    i++;
                }
            }

            return formattedWallLocations;

        }

        public string[,] GetWalls()
        {
            string[,] wallLocations = this.Draw();
            string[,] wallLocationsRCformat = this.ConvertAlgorithmMazeToRowColFormat(wallLocations);
           
            SetUpMazeForGui(wallLocationsRCformat);
            Print2DArray(wallLocationsRCformat);
            return wallLocationsRCformat;
        }

        private void SetUpMazeForGui(string[,] wallLocationsRCformat)
        {
            NorthWall = new bool[size, size];
            EastWall = new bool[size, size];
            SouthWall = new bool[size, size];
            WestWall = new bool[size, size];

            InitializeQuestionLocationArraysWithDefaultQuestionIndex(size);

            int noQuestionPlacedDefault = -1;


            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {

                    if (wallLocationsRCformat[i, j].Contains("N")) { NorthWall[i, j] = true; }
                    else
                    {

                        if (ValidIndex(i - 1) && SouthQuestion[i - 1, j] != noQuestionPlacedDefault)
                        {

                            NorthQuestion[i, j] = SouthQuestion[i - 1, j];

                        }
                        else
                        {
                            _QuestionsList.Add(_QuestionQueue.Dequeue());

                            NorthQuestion[i, j] = _QuestionsList.Count - 1;
                        }

                    }

                    if (wallLocationsRCformat[i, j].Contains("S")) { SouthWall[i, j] = true; }
                    else
                    {

                        if (ValidIndex(i + 1) && NorthQuestion[i + 1, j] != noQuestionPlacedDefault)
                        {

                            SouthQuestion[i, j] = NorthQuestion[i + 1, j];

                        }
                        else
                        {

                            _QuestionsList.Add(_QuestionQueue.Dequeue());



                            SouthQuestion[i, j] = _QuestionsList.Count - 1; ;
                        }

                    }

                    if (wallLocationsRCformat[i, j].Contains("E")) { EastWall[i, j] = true; }
                    else
                    {

                        if (ValidIndex(j + 1) && WestQuestion[i, j + 1] != noQuestionPlacedDefault)
                        {

                            EastQuestion[i, j] = WestQuestion[i, j + 1];

                        }
                        else
                        {
                            _QuestionsList.Add(_QuestionQueue.Dequeue());


                            EastQuestion[i, j] = _QuestionsList.Count - 1;
                        }

                    }

                    if (wallLocationsRCformat[i, j].Contains("W")) { WestWall[i, j] = true; }
                    else
                    {

                        if (ValidIndex(j - 1) && EastQuestion[i, j - 1] != noQuestionPlacedDefault)
                        {

                            WestQuestion[i, j] = EastQuestion[i, j - 1];

                        }
                        else
                        {
                            _QuestionsList.Add(_QuestionQueue.Dequeue());

                            WestQuestion[i, j] = _QuestionsList.Count - 1;
                        }

                    }

                }

            }

        }

        private void InitializeQuestionLocationArraysWithDefaultQuestionIndex(int size)
        {


            NorthQuestion = new int[size, size];
            EastQuestion = new int[size, size];
            SouthQuestion = new int[size, size];
            WestQuestion = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {

                    NorthQuestion[i, j] = -1;
                    EastQuestion[i, j] = -1;
                    SouthQuestion[i, j] = -1;
                    WestQuestion[i, j] = -1;
                }

            }



        }

        public bool ValidIndex(int index)
        {
            return index >= 0 && index < size;
        }

        public static void Print2DArray<T>(T[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }


      
    }
}