using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Labb_3.Command;
using Labb_3.Model;
using Labb_3.View;

namespace Labb_3.WiewModel
{
    internal class ConfigViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        public DelegateCommand AddNewQuestionCommand { get; }
        public DelegateCommand ObliterateQuestionCommand { get; }
        public DelegateCommand NewPackCommand { get; }
        public DelegateCommand CurrentPackOptionsCommand { get; }

        private Question? _activeQuestion;
        private bool _configVisibility;
        public bool ConfigVisibility
        {
            get => _configVisibility;
            set
            {
                _configVisibility = value;
                RaisePropertyChanged();
            }
        }
        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel?.ActivePack; }
        public Question? ActiveQuestion
        {
            get => _activeQuestion;
            set
            {
                _activeQuestion = value;
                RaisePropertyChanged();
                ObliterateQuestionCommand.RaiseCanExecuteChanged();
            }
        }
        public string PackName
        {
            get => ActivePack.Name;
            set
            {
                ActivePack.Name = value;
                RaisePropertyChanged();
            }
        }
        public int PackTimeLimit
        {
            get => ActivePack.TimeLimit;
            set
            {
                ActivePack.TimeLimit = value;
                RaisePropertyChanged();
            }
        }
        public Difficulty PackDifficulty
        {
            get => ActivePack.Difficulty;
            set
            {
                ActivePack.Difficulty = value;
            }
        }
        public ConfigViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            AddNewQuestionCommand = new DelegateCommand(addNewQuestion);
            ObliterateQuestionCommand = new DelegateCommand(ObliterateQuestion, CanObliterateQuestion);
            NewPackCommand = new DelegateCommand(NewPack);
            CurrentPackOptionsCommand = new DelegateCommand(CurrentPackOptions);
            ConfigVisibility = true;
        }

        private bool CanObliterateQuestion(object? arg)
        {
            return ActiveQuestion != null;
        }

        private void addNewQuestion(object parameter)
        {
            Question question = new Question();
            ActivePack.questions.Add(question);
            ActiveQuestion = question;

        }
        private void ObliterateQuestion(object parameter)
        {
            ActivePack.questions.Remove(ActiveQuestion);
            ActiveQuestion = ActivePack.questions.FirstOrDefault();
        }
        public void NewPack(object parameter)
        {
            QuestionPackViewModel newPack = new QuestionPackViewModel(new QuestionPack("New Question Pack"));
            mainWindowViewModel.PutThisPackIntoThePackListPlease(newPack);
            mainWindowViewModel.ActivePack = newPack;
            OpenPackOptions();
        }
        public void CurrentPackOptions(object parameter)
        {
            OpenPackOptions();
        }
        public void OpenPackOptions()
        {
            OptionsWindow optionsWindow = new OptionsWindow();
            optionsWindow.DataContext = this;
            optionsWindow.ShowDialog();
        }
    }
}
