using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Labb_3.Model
{
    public enum Difficulty { Easy, Medium, Hard };
    internal class QuestionPack
    {
        public string Name {  get; set; }
        public Difficulty Difficulty {  get; set; }
        public int TimeLimit { get; set; } //in seconds
        public ObservableCollection<Question> Questions {  get; set; }

        public QuestionPack(string name, Difficulty difficulty = Difficulty.Medium, int timeLimit = 30)
        {
            this.Name = name;
            this.Difficulty = difficulty;
            this.TimeLimit = timeLimit;
            Questions = new ObservableCollection<Question>();
        }
        public QuestionPack()
        {
            Name = "Default Pack";
            Difficulty = Difficulty.Medium;
            TimeLimit = 30;
            Questions = new ObservableCollection<Question>();
        }
    }
}
