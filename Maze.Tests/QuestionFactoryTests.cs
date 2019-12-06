
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
            string [] fakeArgs = new string[] {"1"};

            
            Queue<Question> questions =q.getQuestions(fakeArgs, 20);

            foreach(Question question in questions)
            {
                Console.WriteLine($"Category: {question.Category} | Difficulty: {question.Difficulty}");
            }

        }


        [TestMethod]
        public void Question_Factory_Loads_NewQuestions_EachGame()
        {

            QuestionFactory q = new QuestionFactory();
            int originalCount = q._IndexCounters[0];
            string[] fakeArgs = new string[] { "e" };
            Queue<Question> questions = q.getQuestions(fakeArgs, 20);

            QuestionFactory q2 = new QuestionFactory();

            



            Assert.IsTrue(q2._IndexCounters.Length == 3);
            Assert.IsTrue(q2._IndexCounters[0] == originalCount+20);

        }

















    }
}
