using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunnerWPF
{
    public class Trebek
    {
        //returns true if question is correct;
        public static bool AskQuestion(int questionIndex)
        {

            Question theQuestion = Controller.MazeController.getMaze().GetQuestion(questionIndex);

            // gui knows how to display question. Trebek jst gets input and lets the gui if the player got the question right.
            //and then updates the question status.

            int correctAnswerPosition;

            List<string> answerChoices = PrepareAnswers(theQuestion, out correctAnswerPosition);

           /* Console.WriteLine("Trebek is Asking you a question");

            Console.WriteLine(theQuestion.QuestionPrompt);

            foreach(string answer in answerChoices)
            {
                Console.WriteLine(answer);
            } */
            // string input = Gui.getInput();

          /*  int input = Convert.ToInt32(Console.ReadLine());*/

            bool correctlyAnswered = false;
           

           




           /* switch (input)
            {
                case 0:

                    Controller.MazeController.getMaze().UnlockQuestion(questionIndex);
                    correctlyAnswered = true;
                    Console.WriteLine("nice work");
                    break;

                default:
                    Controller.MazeController.getMaze().ChangeQuestion(questionIndex);
                    break;
            }*/

/*
            if (correctlyAnswered)
            {
                //Console.WriteLine("nice work");
            }
*/

            return correctlyAnswered;


        }

        private static List<string> PrepareAnswers(Question question, out int jojos)
        {
            jojos = 1;
            return new List<string>();
        }

        public static List<(string, bool)> PrepareAnswers(Question theQuestion)
        {
            List<string> answers = theQuestion.IncorrectAnswers.ToList();
            List<(string, bool)> answerChoices = new List<(string, bool)>();
            foreach (string answer in answers)
            {
                answerChoices.Add((answer, false));
            }
            answerChoices.Add((theQuestion.CorrectAnswer, true));

            // Shuffle!!!
            answerChoices = answerChoices.OrderBy(a => Guid.NewGuid()).ToList();

            return answerChoices;
        }
    }
}
