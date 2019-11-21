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




            // string input = Gui.getInput();

            int input = Convert.ToInt32(Console.ReadLine());

            bool correctlyAnswered = false;

            switch (input)
            {
                case 0:
                    correctlyAnswered = true;
                    break;

                default:
                    GamePlay.TheMaze.ChangeQuestion(questionIndex);
                    break;
            }


            if (correctlyAnswered)
            {
                Console.WriteLine("nice work");
            }


            return correctlyAnswered;



            //check answer


            /*


                        if (correct)
                        {
                            theQuestion.Unlock;
                            return true;
                        }
                        else
                            return false;


                    }*/

        }
    }
}
