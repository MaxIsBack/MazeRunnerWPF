using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunnerWPF.MazeGui
{
    sealed class GuiContentManager
    {
        private static GuiContentManager instance = null;
        private static readonly object padlock = new object();

        public static GuiContentManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                        instance = new GuiContentManager();
                }

                return instance;
            }
        }

        private GuiContentManager()
        {
            previousContent = new QuestionGui();
        }

        private object previousContent = null;

        private bool currentlyGame = true;
        

        public object SwitchGuis(object currentContent)
        {
            var curCon= currentContent;
            currentlyGame = !currentlyGame;
            var prevCon = previousContent;
            previousContent = curCon;
            return prevCon;
        }
    }
}
