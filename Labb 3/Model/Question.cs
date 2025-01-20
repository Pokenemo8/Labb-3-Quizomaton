using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Labb_3.Model
{
    internal class Question
    {
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public string[] IncorrectAnswers { get; set; }
        public Question(string questionText = "Insert question here", string correctAnswer = "", string incorrectAnswer1 = "", string incorrectAnswer2 = "", string incorrectAnswer3 = "")
        {
            this.QuestionText = questionText;
            this.CorrectAnswer = correctAnswer;
            IncorrectAnswers = [incorrectAnswer1, incorrectAnswer2, incorrectAnswer3];
        }
        public Question()
        {
            QuestionText = "New Question";
            CorrectAnswer = string.Empty;
            IncorrectAnswers = new string[3];
        }
        
        [JsonConstructor]
        public Question(string questionText, string correctAnswer, string[] incorrectAnswers)
        {
            QuestionText = questionText;
            CorrectAnswer = correctAnswer;
            IncorrectAnswers = incorrectAnswers ?? new string[3];
        }
        public override string ToString()
        {
            return QuestionText;
        }
    }
}
