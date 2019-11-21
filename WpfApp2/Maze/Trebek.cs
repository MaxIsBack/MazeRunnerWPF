using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunnerWPF
{
    class Trebek
    {
        //returns true if question is correct;
        public static bool AskQuestion(int questionIndex)
        {

            Question theQuestion = GamePlay.TheMaze.GetQuestion(questionIndex);

            // gui knows how to display question. Trebek jst gets input and lets the gui if the player got the question right.
            //and then updates the question status.

            int correctAnswerPosition;

            List<string> answerChoices = PrepareAnswers(theQuestion, out correctAnswerPosition);

            Console.WriteLine("Trebek is Asking you a question");

            Console.WriteLine(theQuestion.QuestionPrompt);

            Console.WriteLine(answerChoices[0]);
            // string input = Gui.getInput();

            int input = Convert.ToInt32(Console.ReadLine());

            bool correctlyAnswered = false;
           

           




            switch (input)
            {
                case 0:
                    
                    GamePlay.TheMaze.UnlockQuestion(questionIndex);
                    correctlyAnswered = true;
                    Console.WriteLine("nice work");
                    break;

                default:
                    GamePlay.TheMaze.ChangeQuestion(questionIndex);
                    break;
            }


            if (correctlyAnswered)
            {
                //Console.WriteLine("nice work");
            }


            return correctlyAnswered;


        }

        private static List<string> PrepareAnswers(Question theQuestion, out int correctAnswerPosition)
        {
            List<string> answerChoices = theQuestion.IncorrectAnswers.ToList();
            answerChoices.Add(theQuestion.CorrectAnswer);
            //shuffle
            correctAnswerPosition = 0;
            return answerChoices;

        }
    }
}
