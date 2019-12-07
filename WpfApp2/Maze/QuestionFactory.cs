
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text.RegularExpressions;

namespace MazeRunnerWPF
{
    public class QuestionFactory
    {
        private int[] _EasyMode = new int[] { 70, 90, 100 };
        private int[] _MediumMode = new int[] { 40, 85, 100 };
        private int[] _HardMode = new int[] { 20, 70, 100 };
        private int[] _LegendaryMode = new int[] { 0, 10, 100 };

        private int [] _GameMode;
        public int [] GameMode { get { return _GameMode; } }


        public void SetGameMode(string[] questionArgs) {

            if (questionArgs != null && questionArgs.Length > 0)
            {
                string args = string.Join("", questionArgs);
                if (args.Contains("0"))
                {
                   _GameMode = _EasyMode;
                    

                }
                else if (args.Contains("1"))
                {
                    _GameMode = _MediumMode;
                   

                }
                else if (args.Contains("2"))
                {
                    _GameMode = _HardMode;
                   

                }

            }



        }

        public int[] _IndexCounters; // keeps track of what questions have been already used from database.
        private int[] _NumberOfQuestionsPerTable = new int[3]; //allows for resetting the IndexCounters to 0.
        private string _QuestionIndexTrackerFile = @"C:\Users\saffron\source\repos\MazeRunnerWPF\WpfApp2\QuestionDatabase\QuestionIndexTracker.txt";
        string[] _Tables = new string[] { "EasyQuestions", "MediumQuestions", "HardQuestions" };
        private enum _EnumTable
        {
            EasyQuestions = 0,
            MediumQuestions = 1,
            HardQuestions = 2
        }

        string _Database = @"Data Source=QuestionDatabase\QuestionsForMazeRunner.db; Version=3;";


        public QuestionFactory()
        {

            GetNumberOfQuestionsInDatabase();
            LoadIndexCounters();
        }

        private void GetNumberOfQuestionsInDatabase()
        {
            using (SQLiteConnection sql_conn = new SQLiteConnection(_Database))
            {


                Random randomInt = new Random();

                sql_conn.Open();
              
                using (SQLiteCommand cmd = sql_conn.CreateCommand())
                {

                    for (int i = 0; i < _NumberOfQuestionsPerTable.Length; i++)
                    {
                        cmd.CommandText = $"select Count (*) from {_Tables[i]}";
                        cmd.CommandType = System.Data.CommandType.Text;

                        _NumberOfQuestionsPerTable[i] = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                }
            }
        }

        private void LoadIndexCounters()
        {


            if (File.Exists(_QuestionIndexTrackerFile))
            {

                string[] indexCountersString = File.ReadAllText(_QuestionIndexTrackerFile).Split(',');

                _IndexCounters = new int[indexCountersString.Length];
                for (int i = 0; i < indexCountersString.Length; i++)
                {

                    _IndexCounters[i] = Int32.Parse(indexCountersString[i]);

                }



            }
            else
            {
                _IndexCounters = new int[] { 1, 1, 1 };
            }


        }
        private void SaveIndexCounters()
        {
            string indexCountersAsString = string.Join(",", _IndexCounters);

            File.WriteAllText(_QuestionIndexTrackerFile, indexCountersAsString);




        }
        // will return questions based on the game mode unless given params for a specific type of question
        public Queue<Question> getQuestions( int numberOfQuestionsToReturn, params string[] questionArgs)
        {

            bool getRandomQuestionsBasedOnLevel = false;

            int currentTableToGetFrom = 0;

            EnsureEnoughQuestionsRemainingInDatabase(numberOfQuestionsToReturn);


            //use game mode unless there are params to just get a certain type of question
            int[] currentLevel = _GameMode;



            if (questionArgs != null && questionArgs.Length > 0)
            {
                string args = string.Join("", questionArgs);

                if (args.Contains("e"))
                {
                    currentTableToGetFrom = (int)_EnumTable.EasyQuestions;


                }
                else if (args.Contains("m"))
                {
                    currentTableToGetFrom = (int)_EnumTable.MediumQuestions;

                }
                else if (args.Contains("h"))
                {
                    currentTableToGetFrom = (int)_EnumTable.HardQuestions;

                }
                /* if (args.Contains("0"))
                {
                    currentLevel = _EasyMode;
                   

                }
                else if (args.Contains("1"))
                {
                    currentLevel = _MediumMode;
                    getRandomQuestionsBasedOnLevel = true;

                }
                else if (args.Contains("2"))
                {
                    currentLevel = _HardMode;
                    getRandomQuestionsBasedOnLevel = true;

                }*/
            }
            else {
                getRandomQuestionsBasedOnLevel = true;
            }






            // question args are for type of question. perhaps make enum. like sports+difficult. 
            // what sort of questions to inlude in the returned list.
            var questions = new Queue<Question>();

            //using SQLiteConnection sql_conn = new SQLiteConnection(_Database);
            using (SQLiteConnection sql_conn = new SQLiteConnection(_Database))
            {


                Random randomInt = new Random();

                sql_conn.Open();
                //var questions = new Queue<Question>();



                using (SQLiteCommand cmd = sql_conn.CreateCommand())
                {
                    SQLiteDataReader reader;
                    for (int i = 0; i < numberOfQuestionsToReturn; i++)
                    {

                        // gets questions based on percentage of difficulty
                        if (getRandomQuestionsBasedOnLevel == true&& _GameMode!=null)
                        {
                            int random = randomInt.Next(100) + 1;

                            if (random > 0 && random <= currentLevel[0])
                            {
                                currentTableToGetFrom = 0;
                            }
                            if (random > currentLevel[0] && random <= currentLevel[1])
                            {
                                currentTableToGetFrom = 1;
                            }
                            if (random > currentLevel[(int)_EnumTable.MediumQuestions] && random <= currentLevel[(int)_EnumTable.HardQuestions])
                            {
                                currentTableToGetFrom = 2;
                            }



                        }

                       /* int count = GetQuestionCount(sql_conn, currentTableToGetFrom);
                        while (_IndexCounters[currentTableToGetFrom] > count)
                            _IndexCounters[currentTableToGetFrom] -= count;*/


                        cmd.CommandText = $"select * from {_Tables[currentTableToGetFrom]} where ID=" + _IndexCounters[currentTableToGetFrom];
                        cmd.CommandType = System.Data.CommandType.Text;
                        reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {

                            int ID = Convert.ToInt32(reader["ID"]);
                            string type = (reader["Type"].ToString());
                            string category = (reader["Category"].ToString());
                            string difficulty = (reader["Difficulty"].ToString());
                            string question = (System.Web.HttpUtility.HtmlDecode(reader["Question"].ToString()));
                            string correctAnswer = (reader["CorrectAnswer"].ToString());
                            //string[] incorrectAnswers = reader["IncorrectAnswers"].ToString().Split('|');

                            string[] incorrectAnswers = reader["IncorrectAnswers"].ToString().Split('|');
                            string pattern = "(?<=\")(.*?)(?=\")";

                            for (int j = 0; j < incorrectAnswers.Length; j++)
                            {
                                string cleanedAnswer = Regex.Match(incorrectAnswers[j], pattern).ToString();
                                incorrectAnswers[j] = cleanedAnswer;
                            }





                            questions.Enqueue(new Question(difficulty, category, type, question, correctAnswer, incorrectAnswers));

                        }
                        reader.Close();

                        _IndexCounters[currentTableToGetFrom]++;
                    }
                }


                sql_conn.Close();
            }

            SaveIndexCounters();

            return questions;
        }

        private void EnsureEnoughQuestionsRemainingInDatabase(int numberOfQuestionsToReturn)
        {

            for (int i = 0; i < _NumberOfQuestionsPerTable.Length; i++)
            {
                if (_IndexCounters[i] >= _NumberOfQuestionsPerTable[i] - numberOfQuestionsToReturn) {
                    _IndexCounters[i] = 1;
                }
            }
            
        }

        private int GetQuestionCount(SQLiteConnection sql_conn, int currentTableToGetFrom)
        {
            using (SQLiteCommand cmd = sql_conn.CreateCommand())
            {
                cmd.CommandText = $"select count(1) from {_Tables[currentTableToGetFrom]}";
                cmd.CommandType = System.Data.CommandType.Text;
                SQLiteDataReader reader = cmd.ExecuteReader();

                int count = -1;
                if (reader.Read())
                {
                    count = Convert.ToInt32(reader[0]);
                }
                reader.Close();

                return count;
            }
        }
    }
}