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

        private Window mainWindow;
        private object mazeGuiContent;
        private object questionGuiContent;

        public void SetMainWindow(Window window)
        {
            mainWindow = window;
        }

        public void ShowMazeGui()
        {
            if (mazeGuiContent == null)
            {
                Console.WriteLine("New Maze Gui created");
                questionGuiContent = new MainWindow();
            }

            mainWindow.Content = mazeGuiContent;
        }

        public void ShowQuestionGui()
        {
            if (questionGuiContent == null)
            {
                Console.WriteLine("New Question Gui created");
                questionGuiContent = new QuestionGui();
            }

            mainWindow.Content = questionGuiContent;
        }
    }
}
