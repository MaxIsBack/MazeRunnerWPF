using System;

namespace MazeRunnerWPF
{
    public class Question
    {
        public int number;
        private bool _Locked;

        public string Difficulty { get; }
        public string Category { get; }
        public string Type { get; }
        public string QuestionPrompt { get; }
        public string CorrectAnswer { get; }
        public string [] IncorrectAnswers { get; }

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
            _Locked = true;
        }
    }
}