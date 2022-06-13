using System.Collections.ObjectModel;
using Diploma.Resources.Strings;
using Prism.Mvvm;

namespace Diploma.Models
{
    public class TaskBindableModel : BindableBase
    {
        #region -- Public properties --

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private int _lessonId;
        public int LessonId
        {
            get => _lessonId;
            set => SetProperty(ref _lessonId, value);
        }

        private string _question;
        public string Question
        {
            get => _question;
            set => SetProperty(ref _question, value);
        }

        private string _answer;
        public string Answer
        {
            get => _answer;
            set => SetProperty(ref _answer, value);
        }

        private string _type;
        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        private int _selectedTypeIndex;
        public int SelectedTypeIndex
        {
            get => _selectedTypeIndex;
            set => SetProperty(ref _selectedTypeIndex, value);
        }

        private string _selectedValue;
        public string SelectedValue
        {
            get => _selectedValue;
            set => SetProperty(ref _selectedValue, value);
        }

        public ObservableCollection<string> Types => new() { Strings.OneAnswer, Strings.MultipleAnswers, Strings.DirectAnswer };

        private string _possibleAnswersText;
        public string PossibleAnswersText
        {
            get => _possibleAnswersText;
            set => SetProperty(ref _possibleAnswersText, value);
        }

        private ObservableCollection<string> _possibleAnswers;
        public ObservableCollection<string> PossibleAnswers
        {
            get => _possibleAnswers;
            set => SetProperty(ref _possibleAnswers, value);
        }

        #endregion
    }
}
