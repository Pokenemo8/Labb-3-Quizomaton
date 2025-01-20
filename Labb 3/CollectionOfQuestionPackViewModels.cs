using Labb_3.WiewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_3
{
    class CollectionOfQuestionPackViewModels
    {
        public ObservableCollection<QuestionPackViewModel> questionPacks {  get; set; }
        public CollectionOfQuestionPackViewModels(ObservableCollection<QuestionPackViewModel> questionPacks)
        {
            this.questionPacks = questionPacks;
        }
    }
}
