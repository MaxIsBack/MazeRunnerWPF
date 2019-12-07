
using MazeRunnerWPF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace MazeTesting
{
    [TestClass]
    public class QuestionFactoryTests
    {

        [TestMethod]
        public void Question_Factory_DB_Test()
        {

            QuestionFactory q = new QuestionFactory();
            string[] fakeArgs = new string[] { "0" };
            q.SetGameMode(fakeArgs);
           

            

            Queue<Question> questions =q.getQuestions(20);


            foreach(Question question in questions)
            {
                Console.WriteLine($"Question: {question.QuestionPrompt} | Category: {question.Category} | Difficulty: {question.Difficulty}");
            }

        }


        [TestMethod]
        public void Question_Factory_Loads_NewQuestions_EachGame()
        {

            QuestionFactory q = new QuestionFactory();
            string[] fakeArgs = new string[] { "0" };
            q.SetGameMode(fakeArgs);

            int easyOriginalCount = q._IndexCounters[0];
            int mediumOriginalCount = q._IndexCounters[1];
            int hardOriginalCount = q._IndexCounters[2];

            string[] efakeArgs = new string[] { "e" };
            string[] mfakeArgs = new string[] { "m" };
            string[] hfakeArgs = new string[] { "h" };

            int numOfQuestions = 5;
            //easy questions
            Queue<Question> questions = q.getQuestions( numOfQuestions, efakeArgs);

            QuestionFactory q2 = new QuestionFactory();
           
            Assert.IsTrue(q2._IndexCounters[0] == easyOriginalCount+numOfQuestions);


            //medium questions
            Queue<Question> questionsM = q.getQuestions( numOfQuestions, mfakeArgs);

            QuestionFactory qM = new QuestionFactory();

            Assert.IsTrue(qM._IndexCounters[1] == mediumOriginalCount + numOfQuestions);

            //hard questions
            Queue<Question> questionsH = q.getQuestions( numOfQuestions, hfakeArgs);

            QuestionFactory qH = new QuestionFactory();

            Assert.IsTrue(qH._IndexCounters[2] == hardOriginalCount + numOfQuestions);

        }

















    }
}
