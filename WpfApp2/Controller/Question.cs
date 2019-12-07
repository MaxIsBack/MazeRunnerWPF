using System;

namespace MazeRunnerWPF
{
    public class Question
    {
        private int number;
        private bool _Locked;

        public string Difficulty { get; private set; }
        public string Category { get; private set; }
        public string Type { get; private set; }
        public string QuestionPrompt { get; private set; }
        public string CorrectAnswer { get; private set; }
        public string [] IncorrectAnswers { get; private set; }

        public Question(string difficulty, string category, string type, string questionPrompt, string correctAnswer, string [] incorrectAnswers) {
            Difficulty = difficulty;
            Category = category;
            Type = type;
            QuestionPrompt = questionPrompt;
            CorrectAnswer = correctAnswer;
            IncorrectAnswers = incorrectAnswers;
            _Locked = true;

          
        }

        

        public Question(int num) {
            number = num;
            _Locked = true;

        }

        internal bool Locked()
        {
            return _Locked;
        }

        internal void Unlock()
        {
            _Locked = false;
        }
    }
}