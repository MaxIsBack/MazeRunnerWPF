
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace MazeRunnerWPF
{
    public class QuestionFactory
    {
        private int[] _EasyMode = new int[] { 70, 90, 100 };
        private int[] _MediumMode = new int[] { 40, 85, 100 };
        private int[] _HardMode = new int[] { 20, 70, 100 };
        private int[] _LegendaryMode = new int[] { 0, 10, 100 };

        private int[] _IndexCounters = new int[] { 1, 1, 1 }; // keeps track of what questions have been already used from database.
        string[] _Tables = new string[] { "EasyQuestions", "MediumQuestions", "HardQuestions" };
        private enum _EnumTable
        {
            EasyQuestions = 0,
            MediumQuestions = 1,
            HardQuestions = 2
        }


        // for testing.
        string _Database = @"Data Source=C:\Users\saffron\Source\Repos\MazeRunnerWPF\WpfApp2\QuestionDatabase\QuestionsForMazeRunner.db; Version=3;";

        //for the app
        //private string _Database = Path.Combine("Data Source=", System.AppDomain.CurrentDomain.BaseDirectory, @"QuestionDatabase\QuestionsForMazeRunner.db", "; Version=3;");
        
        


        public Queue<Question> getQuestions(string[] questionArgs, int numberOfQuestionsToReturn)
        {
            
            bool getRandomQuestionsBasedOnLevel = false;



            int currentTableToGetFrom = 0;

            //default level
            int[] currentLevel = _EasyMode;

            if (questionArgs != null && questionArgs.Length > 0)
            {
                string args = string.Join("", questionArgs);
                if (args.Contains("e"))
                {
                    currentTableToGetFrom = (int)_EnumTable.EasyQuestions;


                }
                if (args.Contains("m"))
                {
                    currentTableToGetFrom = (int)_EnumTable.MediumQuestions;

                }
                if (args.Contains("0"))
                {
                    currentLevel = _EasyMode;
                    getRandomQuestionsBasedOnLevel = true;

                }
                if (args.Contains("1"))
                {
                    currentLevel = _MediumMode;
                    getRandomQuestionsBasedOnLevel = true;

                }
                if (args.Contains("2"))
                {
                    currentLevel = _MediumMode;
                    getRandomQuestionsBasedOnLevel = true;

                }
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
                        if (getRandomQuestionsBasedOnLevel == true)
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



                        cmd.CommandText = $"select * from {_Tables[currentTableToGetFrom]} where ID=" + _IndexCounters[currentTableToGetFrom];
                        cmd.CommandType = System.Data.CommandType.Text;
                        reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {

                            int ID = Convert.ToInt32(reader["ID"]);
                            string type = (reader["Type"].ToString());
                            string category = (reader["Category"].ToString());
                            Console.WriteLine(category);
                            string difficulty = (reader["Difficulty"].ToString());
                            string question = (System.Web.HttpUtility.HtmlDecode(reader["Question"].ToString()));
                            string correctAnswer = (reader["CorrectAnswer"].ToString());
                            string[] incorrectAnswers = reader["IncorrectAnswers"].ToString().Split('|');
                           
                           
                            questions.Enqueue(new Question(difficulty, category, type, question, correctAnswer, incorrectAnswers));

                        }

                        reader.Close();
                        _IndexCounters[currentTableToGetFrom]++;
                    }




                }


                sql_conn.Close();
            }

            return questions;


        }


    }
}