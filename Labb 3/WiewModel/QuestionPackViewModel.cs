using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Labb_3.Model;

namespace Labb_3.WiewModel
{
    internal class QuestionPackViewModel : ViewModelBase
    {
        public readonly QuestionPack model;
        public ObservableCollection<Question> questions 
        {  
            get => model.Questions;
            set 
            { 
                model.Questions = value;
                RaisePropertyChanged();
            }

        }
        public QuestionPackViewModel(QuestionPack model)
        {
            this.model = model;
            questions = new ObservableCollection<Question>(model.Questions);
        }
        public QuestionPackViewModel()
        {
            model = new QuestionPack("Name");
            questions = new ObservableCollection<Question>(model.Questions);
        }
        public string Name 
        { 
            get => model.Name; 
            set 
            {
                model.Name = value;
                RaisePropertyChanged();
            } 
        }
        public Difficulty Difficulty
        {
            get => model.Difficulty;
            set
            {
                model.Difficulty = value;
                RaisePropertyChanged();
            }
        }
        public int TimeLimit
        {
            get => model.TimeLimit;
            set
            {
                model.TimeLimit = value;
                RaisePropertyChanged();
            }
        }

        
    }
}
