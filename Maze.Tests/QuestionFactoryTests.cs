
using MazeRunnerWPF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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

            
            Queue<Question> questions =q.GetQuestions(fakeArgs, 20);

            foreach(Question question in questions)
            {
                Console.WriteLine($"Category: {question.Category} | Difficulty: {question.Difficulty}");
            }

        }

       

       




        

      






    }
}
