using MazeRunnerWPF.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MazeRunnerWPF.MazeGui
{
    /// <summary>
    /// Interaction logic for QuestionGui.xaml
    /// </summary>
    public partial class QuestionGui : Page, IGuiPage
    {
        public QuestionGui()
        {
            InitializeComponent();
        }

        private List<(string answer, bool correct)> answerChoices;
        private int questionId;
        public void OnShown(object passingObj)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += Page_KeyDown;

            questionId = (int)passingObj;

            rbOption1.IsChecked = rbOption2.IsChecked = rbOption3.IsChecked = rbOption4.IsChecked = false;
            rbOption1.Visibility = rbOption2.Visibility = rbOption3.Visibility = rbOption4.Visibility = Visibility.Visible;

            Question trebekPls = Controller.MazeController.Questioner(questionId);
            lblQuestion.Content = trebekPls.QuestionPrompt;

            answerChoices =
                MazeRunnerWPF.Trebek.PrepareAnswers(
                    trebekPls
                );

            switch(trebekPls.Type)
            {
                case "multiple":
                    lblQuestionType.Content = "Multiple Choice:";
                    rbOption1.Content = answerChoices[0].answer;
                    rbOption2.Content = answerChoices[1].answer;
                    rbOption3.Content = answerChoices[2].answer;
                    rbOption4.Content = answerChoices[3].answer;
                    break;
                case "boolean":
                    lblQuestionType.Content = "True/False:";
                    rbOption1.Content = answerChoices[0].answer;
                    rbOption2.Content = answerChoices[1].answer;
                    rbOption3.Visibility = Visibility.Hidden;
                    rbOption4.Visibility = Visibility.Hidden;
                    break;
            }
        }

        public void OnDisappeared()
        {
            var window = Window.GetWindow(this);
            window.KeyDown -= Page_KeyDown;
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C)
            {
                Console.WriteLine("Toggle Cheats!!!");
                btnSubmitBadChoice.Visibility = btnSubmitBadChoice.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
                btnSubmitAnswerRight.Visibility = btnSubmitAnswerRight.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            }
        }

        private static bool QuestionableBoolToBool(bool? q)
        {
            return q == null ? false : (bool)q;
        }

        private int GetSelectedAnswer()
        {
            if (QuestionableBoolToBool(rbOption1.IsChecked))
            {
                return 0;
            }
            else if (QuestionableBoolToBool(rbOption2.IsChecked))
            {
                return 1;
            }
            else if (QuestionableBoolToBool(rbOption3.IsChecked))
            {
                return 2;
            }
            else if (QuestionableBoolToBool(rbOption4.IsChecked))
            {
                return 3;
            }

            return -1;
        }

        private void btnSubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            int selectedAnswer = GetSelectedAnswer();
            if (selectedAnswer < 0) return;

            GuiMediator.Instance.ShowMazeGui(
                (answerChoices[selectedAnswer].correct, questionId)
            );
        }

        private void btnSubmitAnswerGoodChoice_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Zoo wee mama!");
            GuiMediator.Instance.ShowMazeGui((true, questionId));
        }

        private void btnSubmitBadChoice_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Aye aye aye!");
            GuiMediator.Instance.ShowMazeGui((false, questionId));
        }
    }
}
