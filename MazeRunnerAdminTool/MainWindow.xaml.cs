using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

namespace MazeRunnerAdminTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            cbType.SelectionChanged += cbType_SelectionChanged;
        }

        string _Database = null;

        private void btnOpenDb_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "SQLite database files (*.db)|*.db"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                lblDispFname.Content = openFileDialog.FileName;
                _Database = @"Data Source=" + openFileDialog.FileName + "; Version=3;";
            }
        }

        private void btnAddQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (_Database == null)
            {
                MessageBox.Show(
                    "Please select database file to write to.",
                    "Sanity Check",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            switch (cbType.SelectedIndex)
            {
                case 0:
                    // Multiple choice
                    AddQuestionMultipleChoice(
                        (Difficulty)cbDifficulty.SelectedIndex,
                        txtQuestion.Text,
                        txtAnswerCorrect.Text,
                        new string[] {
                            txtAnswerIncorrect1.Text,
                            txtAnswerIncorrect2.Text,
                            txtAnswerIncorrect3.Text
                        });
                    break;
                case 1:
                    // True/False
                    AddQuestionTF(
                        (Difficulty)cbDifficulty.SelectedIndex,
                        txtQuestion.Text,
                        cbTFCorrectAns.SelectedIndex == 0);
                    break;
            }
        }

        private void cbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbType.SelectedIndex)
            {
                case 0:
                    // Multiple choice
                    lblIncorrectAns1.Visibility =
                        lblIncorrectAns2.Visibility =
                        lblIncorrectAns3.Visibility =
                        txtAnswerCorrect.Visibility =
                        txtAnswerIncorrect1.Visibility =
                        txtAnswerIncorrect2.Visibility =
                        txtAnswerIncorrect3.Visibility =
                            Visibility.Visible;
                    cbTFCorrectAns.Visibility = Visibility.Hidden;
                    break;
                case 1:
                    // True/False
                    lblIncorrectAns1.Visibility =
                        lblIncorrectAns2.Visibility =
                        lblIncorrectAns3.Visibility =
                        txtAnswerCorrect.Visibility =
                        txtAnswerIncorrect1.Visibility =
                        txtAnswerIncorrect2.Visibility =
                        txtAnswerIncorrect3.Visibility =
                            Visibility.Hidden;
                    cbTFCorrectAns.Visibility = Visibility.Visible;
                    break;
            }
        }


        private void AddQuestionMultipleChoice(Difficulty difficulty, string question, string correctAns, string[] incorrectAnswers)
        {
            CommitToDB(difficulty, "multiple", question, correctAns, incorrectAnswers);
        }

        private void AddQuestionTF(Difficulty difficulty, string question, bool correctAns)
        {
            CommitToDB(difficulty, "boolean", question, correctAns.ToString(), new string[] { (!correctAns).ToString() });
        }

        private void CommitToDB(Difficulty difficulty, string type, string question, string correctAns, string[] incorrectAnswers)
        {
            string _tabeName = GetDBName(difficulty);
            string _type = type;
            string _category = "User made";
            string _difficulty = difficulty.ToString().ToLower();
            string _question = question;
            string _correctAns = correctAns;
            string _incorrectAns = SerializeIncorrectAnswers(incorrectAnswers);

            using (SQLiteConnection sql_conn = new SQLiteConnection(_Database))
            {
                sql_conn.Open();

                using (SQLiteCommand cmd = sql_conn.CreateCommand())
                {
                    cmd.CommandText = $"insert into {_tabeName} (Type, Category, Difficulty, Question, CorrectAnswer, IncorrectAnswers) " +
                        $"values ('{_type}','{_category}','{_difficulty}','{_question}','{_correctAns}','{_incorrectAns}')";
                    cmd.CommandType = System.Data.CommandType.Text;

                    int rows = 0;
                    try
                    {
                        rows = cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(
                            e.Message,
                            "Failed",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }

                    if (rows > 0)
                    {
                        MessageBox.Show(
                            "Question added to Database!!!",
                            "Success",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                }
            }
        }

        private string GetDBName(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.EASY:
                    return "EasyQuestions";
                case Difficulty.MEDIUM:
                    return "MediumQuestions";
                case Difficulty.HARD:
                    return "HardQuestions";
            }
            return null;
        }

        private string SerializeIncorrectAnswers(string[] incorrectAnswers)
        {
            string all = "";
            for (int i = 0; i < incorrectAnswers.Length; i++)
            {
                if (i > 0)
                {
                    all += "|";
                }
                all += incorrectAnswers[i];
            }
            return all;
        }
    }

    enum Difficulty
    {
        EASY = 0, MEDIUM = 1, HARD = 2
    }
}
