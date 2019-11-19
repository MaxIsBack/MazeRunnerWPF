using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunnerWPF
{
    class Trebek
    {

      public void AskQuestion(int questionIndex)
        {

            Question theQuestion = GamePlay._TheMaze.GetQuestion(questionIndex);

            // gui knows how to display question. Trebek jst gets input and lets the gui if the player got the question right.
            //and then updates the question status.

            string input = Gui.getInput();
            
            //validateinput


            if (correct)
            theQuestion.Unlock;

        }

    }
}
