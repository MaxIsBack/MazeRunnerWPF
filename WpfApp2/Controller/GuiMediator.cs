using MazeRunnerWPF.MazeGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MazeRunnerWPF.Controller
{
    sealed class GuiMediator
    {
        private static GuiMediator instance = null;
        private static readonly object padlock = new object();

        public static GuiMediator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                        instance = new GuiMediator();
                }

                return instance;
            }
        }

        private GuiMediator()
        {
        }

        private MainWindow mainWindow;
        private IGuiPage mazeGuiContent;
        private IGuiPage questionGuiContent;
        private IGuiPage winningGuiContent;
        private IGuiPage previous;

        public void SetMainWindow(MainWindow window)
        {
            mainWindow = window;
        }

        public void ShowMazeGui(object passingObj)
        {
            if (mazeGuiContent == null)
            {
                Console.WriteLine("New Maze Gui created");
                mazeGuiContent = new MazeRunnerWPF.MazeGui.MazeGui();
            }
            SetUpContent(mazeGuiContent, passingObj);
        }

        public void ShowQuestionGui(object passingObj)
        {
            if (questionGuiContent == null)
            {
                Console.WriteLine("New Question Gui created");
                questionGuiContent = new MazeRunnerWPF.MazeGui.QuestionGui();
            }
            SetUpContent(questionGuiContent, passingObj);
        }

        public void ShowWinningGui(object passingObj)
        {
            if (winningGuiContent == null)
            {
                Console.WriteLine("New Winning Gui created");
                winningGuiContent = new MazeRunnerWPF.MazeGui.WinningGui();
            }
            SetUpContent(winningGuiContent, passingObj);
        }

        private void SetUpContent(IGuiPage page, object passingObj)
        {
            if (previous != null)
            {
                previous.OnDisappeared();
            }

            mainWindow.SetContent(page);
            previous = page;
            previous.OnShown(passingObj);
        }
    }
}
