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
using System.Xml.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Labb_3.WiewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
        public PlayerViewModel PlayerViewModel { get; }
        public ConfigViewModel ConfigViewModel { get; }
        public DelegateCommand ChangeViewConfigCommand { get; }
        public DelegateCommand ChangeViewPlayerCommand { get; }
        public DelegateCommand NewQuestionPackCommand { get; }
        public DelegateCommand SetActivePackCommand { get; }
        public DelegateCommand DestroyQuestionPackCommand { get; }
        public DelegateCommand FullscreenCommand { get; }
        public DelegateCommand ExitCommand { get; }
        private WindowState _windowState;
        public WindowState WindowState
        {
            get => _windowState;
            set
            {
                _windowState = value;
                RaisePropertyChanged();
            }
        }
        private WindowStyle _windowStyle;
        public WindowStyle WindowStyle
        {
            get => _windowStyle;
            set 
            { 
                _windowStyle = value;
                RaisePropertyChanged();
            }
        }

        private QuestionPackViewModel? _activePack;

        public QuestionPackViewModel? ActivePack
        {
            get => _activePack;
            set
            {
                _activePack = value;
                RaisePropertyChanged();
                ConfigViewModel?.RaisePropertyChanged("ActivePack");
            }
        }
        public Action CloseWindowAction { get; set; }

        public MainWindowViewModel()
        {
            
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                IncludeFields = true,
                IgnoreReadOnlyProperties = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
            if (File.Exists(GetSaveFileLocation()))
            {
                string jsonString = File.ReadAllText(GetSaveFileLocation());
                Packs = JsonSerializer.Deserialize<ObservableCollection<QuestionPackViewModel>>(jsonString, options);
                
                
            }
            
            if (Packs.Count == 0)
            {
                QuestionPackViewModel newpack = new QuestionPackViewModel(new QuestionPack("My Question Pack"));
                Packs.Add(newpack);
                ActivePack = newpack;
                ActivePack.questions.Add(new Question());
                ActivePack.questions.Add(new Question("Question 2"));
            }
            else
            {
                ActivePack = Packs[0];
            }
            
            PlayerViewModel = new PlayerViewModel(this);
            ConfigViewModel = new ConfigViewModel(this);
            ChangeViewConfigCommand = new DelegateCommand(ChangeViewConfig, CanChangeViewConfig);
            ChangeViewPlayerCommand = new DelegateCommand(ChangeViewPlayer, CanChangeViewPlayer);
            NewQuestionPackCommand = new DelegateCommand(NewQuestionPack);
            SetActivePackCommand = new DelegateCommand(SetActivePack);
            DestroyQuestionPackCommand = new DelegateCommand(DestroyQuestionPack);
            FullscreenCommand = new DelegateCommand(Fullscreen);
            ExitCommand = new DelegateCommand(IWantToLeavePls);
            WindowState = WindowState.Normal;
            WindowStyle = WindowStyle.SingleBorderWindow;
        }

        

        private bool CanChangeViewConfig(object? arg)
        {
            return PlayerViewModel.PlayerVisibility;
        }
        private bool CanChangeViewPlayer(object? arg)
        {
            return ConfigViewModel.ConfigVisibility;
        }
        public void ChangeViewConfig(object parameter)
        {
            ConfigViewModel.ConfigVisibility = true;
            PlayerViewModel.PlayerVisibility = false;
            ChangeViewConfigCommand.RaiseCanExecuteChanged();
            ChangeViewPlayerCommand.RaiseCanExecuteChanged();
        }
        public void ChangeViewPlayer(object parameter)
        {

            ConfigViewModel.ConfigVisibility = false;
            PlayerViewModel.PlayerVisibility = true;
            ChangeViewConfigCommand.RaiseCanExecuteChanged();
            ChangeViewPlayerCommand.RaiseCanExecuteChanged();
            PlayerViewModel.startGame(ActivePack);
        }
        public void NewQuestionPack(object parameter)
        {
            SavePacks();
        }
        public async Task SavePacks()
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                IncludeFields = true,
                IgnoreReadOnlyProperties = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
            string json = JsonSerializer.Serialize(Packs, options);
            await File.WriteAllTextAsync(GetSaveFileLocation(), json);
        }
        private string GetSaveFileLocation()
        {

            string pafth = @"\Quizomaton\pax.json";
            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Quizomaton"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Quizomaton");
            }
            
            
            
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + pafth;
        }
        public void PutThisPackIntoThePackListPlease(QuestionPackViewModel pack)
        {
            Packs.Add(pack);
        }
        public void SetActivePack(object selectedPack)
        {
            if (selectedPack is QuestionPackViewModel questionPack)
            {
                ActivePack = questionPack;
            }
            
        }
        public void DestroyQuestionPack(object parameter)
        {
            Packs.Remove(ActivePack);
            ActivePack = Packs.FirstOrDefault();
        }

        public void Fullscreen(object parameter)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
            }
        }
        public void IWantToLeavePls(object parameter)
        {
            SavePacks();
            Application.Current.Shutdown();
        }
    }
}
