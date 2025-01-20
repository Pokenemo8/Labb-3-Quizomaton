using Labb_3.Command;
using Labb_3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Labb_3.WiewModel
{
    internal class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        private DispatcherTimer timer;
        private bool ableToPressButtons;
        private int _timeLeft;
        private bool _playerVisibility;
        public int score;
        List<Question> questionList;
        private Question activeQuestion;
        private string? _activeQuestionText;
        private string _endScreenText;
        public string EndScreenText
        {
            get => _endScreenText;
            set
            {
                _endScreenText = value;
                RaisePropertyChanged();
            }

        }
        public string? ActiveQuestionText
        {
            get => _activeQuestionText;
            set
            {
                 _activeQuestionText = value;
                 RaisePropertyChanged();
            }
        }
        private string? _scoreText;
        public string? ScoreText
        {
            get => _scoreText;
            set
            {
                _scoreText = value;
                RaisePropertyChanged();
            }
        }
        private string? _questionsleftText;
        public string? QuestionsleftText
        {
            get => _questionsleftText;
            set
            {
                _questionsleftText = value;
                RaisePropertyChanged();
            }
        }
        private string? _button1Text;
        public string? Button1Text
        {
            get => _button1Text;
            set
            {
                _button1Text = value;
                RaisePropertyChanged();
            }
        }
        private string? _button2Text;
        public string? Button2Text
        {
            get => _button2Text;
            set
            {
                _button2Text = value;
                RaisePropertyChanged();
            }
        }
        private string? _button3Text;
        public string? Button3Text
        {
            get => _button3Text;
            set
            {
                _button3Text = value;
                RaisePropertyChanged();
            }
        }
        private string? _button4Text;
        public string? Button4Text
        {
            get => _button4Text;
            set
            {
                _button4Text = value;
                RaisePropertyChanged();
            }
        }
        private SolidColorBrush _button1Color;
        public SolidColorBrush Button1Color
        {
            get => _button1Color;
            set
            {
                _button1Color = value;
                RaisePropertyChanged();
            }
        }
        private SolidColorBrush _button2Color;
        public SolidColorBrush Button2Color
        {
            get => _button2Color;
            set
            {
                _button2Color = value;
                RaisePropertyChanged();
            }
        }
        private SolidColorBrush _button3Color;
        public SolidColorBrush Button3Color
        {
            get => _button3Color;
            set
            {
                _button3Color = value;
                RaisePropertyChanged();
            }
        }
        private SolidColorBrush _button4Color;
        public SolidColorBrush Button4Color
        {
            get => _button4Color;
            set
            {
                _button4Color = value;
                RaisePropertyChanged();
            }
        }
        private SolidColorBrush buttonDefaultColor;
        private SolidColorBrush buttonCorrectColor;
        private SolidColorBrush buttonIncorrectColor;
        private int pressedButtonNumber;
        readonly Random random = new Random();
        public bool PlayerVisibility
        {
            get => _playerVisibility;
            set
            {
                _playerVisibility = value;
                RaisePropertyChanged();

            }
        }
        private Visibility _gameVisibility;
        public Visibility GameVisibility
        {
            get => _gameVisibility;
            set
            {
                _gameVisibility = value;
                RaisePropertyChanged();
            }
        }
        private Visibility _endScreenVisibility;
        public Visibility EndScreenVisibility
        {
            get => _endScreenVisibility;
            set
            {
                _endScreenVisibility = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand Button1PressedCommand { get; }
        public DelegateCommand Button2PressedCommand { get; }
        public DelegateCommand Button3PressedCommand { get; }
        public DelegateCommand Button4PressedCommand { get; }

        public int TimeLeft
        {
            get => _timeLeft;
            private set
            {
                _timeLeft = value;
                RaisePropertyChanged();
            }
        }
        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            TimeLeft = 30;
            PlayerVisibility = false;
            Button1PressedCommand = new DelegateCommand(Button1Pressed);
            Button2PressedCommand = new DelegateCommand(Button2Pressed);
            Button3PressedCommand = new DelegateCommand(Button3Pressed);
            Button4PressedCommand = new DelegateCommand(Button4Pressed);
            buttonDefaultColor = new SolidColorBrush(Colors.LightGray);
            buttonCorrectColor = new SolidColorBrush(Colors.LightGreen);
            buttonIncorrectColor = new SolidColorBrush(Colors.Red);
            Button1Color = buttonDefaultColor;
            Button2Color = buttonDefaultColor;
            Button3Color = buttonDefaultColor;
            Button4Color = buttonDefaultColor;
            GameVisibility = Visibility.Visible;
            EndScreenVisibility = Visibility.Hidden;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeLeft--;
            if (TimeLeft <= 0)
            {
                timer.Stop();
                if (ableToPressButtons)
                {
                    ableToPressButtons = false;
                    CheckAnswer(null);
                }
                else
                {
                    nextQuestion();
                }
            }
        }
        
        public void startGame(QuestionPackViewModel activeQuestionPack)
        {
            ClearButtonColors();
            questionList = activeQuestionPack.questions.ToList();
            questionList = questionList.OrderBy(x => random.Next()).ToList();
            score = 0;
            GameVisibility = Visibility.Visible;
            EndScreenVisibility = Visibility.Hidden;
            updateScoreText();
            nextQuestion();
        }
        private void updateScoreText()
        {
            ScoreText = "Score: " + score + "/" + mainWindowViewModel.ActivePack.questions.Count;
        }
        private void UpdateQuestionsleftText()
        {
            QuestionsleftText = "Question " + (mainWindowViewModel.ActivePack.questions.Count - questionList.Count) + "/" + mainWindowViewModel.ActivePack.questions.Count;
        }
        //Everything below is the worst code i have ever written
        //seriously what is this i hate myself
        //I should have just figured out how to access the buttons themselves but i am in too deep now
        public void nextQuestion()
        {
            if(questionList.Count > 0)
            {
                activeQuestion = questionList[0];
                questionList.RemoveAt(0);
                ActiveQuestionText = activeQuestion.QuestionText;
                ClearButtonColors();
                ableToPressButtons = true;
                UpdateQuestionsleftText();
                List<int> questionTexts = new List<int> { 1, 2, 3, 4 };
                questionTexts = questionTexts.OrderBy(x => random.Next()).ToList();
                Button1Text = GiveQuestionText(questionTexts[0]);
                questionTexts.RemoveAt(0);
                Button2Text = GiveQuestionText(questionTexts[0]);
                questionTexts.RemoveAt(0);
                Button3Text = GiveQuestionText(questionTexts[0]);
                questionTexts.RemoveAt(0);
                Button4Text = GiveQuestionText(questionTexts[0]);
                questionTexts.RemoveAt(0);
                ableToPressButtons = true;
                TimeLeft = mainWindowViewModel.ActivePack.TimeLimit;
                timer.Start();
            }
            else
            {
                GameVisibility = Visibility.Hidden;
                EndScreenVisibility = Visibility.Visible;
                EndScreenText = "You got " + score + " Points out of " + mainWindowViewModel.ActivePack.questions.Count + " Questions!";
            }
        }
        private string GiveQuestionText(int questionId)
        {
            return questionId switch
            {
                1 => activeQuestion.CorrectAnswer,
                2 => activeQuestion.IncorrectAnswers[0],
                3 => activeQuestion.IncorrectAnswers[1],
                4 => activeQuestion.IncorrectAnswers[2],
                _ => "How did you get here",
            };
        }
        private void Button1Pressed(object parameter)
        {
            
            if (ableToPressButtons)
            {
                ableToPressButtons = false;
                pressedButtonNumber = 1;
                CheckAnswer(Button1Text);
            }
            
        }
        private void Button2Pressed(object parameter)
        {
            if (ableToPressButtons)
            {
                pressedButtonNumber = 2;
                ableToPressButtons = false;
                CheckAnswer(Button2Text);
            }
        }
        private void Button3Pressed(object parameter)
        {
            if (ableToPressButtons)
            {
                pressedButtonNumber = 3;
                ableToPressButtons = false;
                CheckAnswer(Button3Text);
            }
        }
        private void Button4Pressed(object parameter)
        {
            if (ableToPressButtons)
            {
                pressedButtonNumber = 4;
                ableToPressButtons = false;
                CheckAnswer(Button4Text);
            }
            else
            {
                nextQuestion();
            }
        }
        private void CheckAnswer(string answer)
        {
            if (answer != null)
            {
                if (answer == activeQuestion.CorrectAnswer)
                {
                    score++;
                    updateScoreText();
                    ChangeButtonColor(pressedButtonNumber, buttonCorrectColor);
                }
                else
                {
                    ChangeButtonColor(pressedButtonNumber, buttonIncorrectColor);
                    ChangeButtonColor(GetCorrectButtonNumber(), buttonCorrectColor);
                }
            }
            else
            {
                ChangeButtonColor(GetCorrectButtonNumber(), buttonCorrectColor);
            }
            TimeLeft = 3;
            timer.Start();
        }
        private int GetCorrectButtonNumber()
        {
            if(Button1Text == activeQuestion.CorrectAnswer)
            {
                return 1;
            }
            else if (Button2Text == activeQuestion.CorrectAnswer)
            {
                return 2;
            }
            else if (Button3Text == activeQuestion.CorrectAnswer)
            {
                return 3;
            }
            else if (Button4Text == activeQuestion.CorrectAnswer)
            {
                return 4;
            }
            else
            {
                return 0;
            }
        }
        private void ChangeButtonColor(int buttonNumber,SolidColorBrush color)
        {
            switch(buttonNumber)
            {
                case 1: Button1Color = color; break;
                case 2: Button2Color = color; break;
                case 3: Button3Color = color; break;
                case 4: Button4Color = color; break;
                default: break;
            };
        }
        private void ClearButtonColors()
        {
            Button1Color = buttonDefaultColor;
            Button2Color = buttonDefaultColor;
            Button3Color = buttonDefaultColor;
            Button4Color = buttonDefaultColor;
        }
    }
}
