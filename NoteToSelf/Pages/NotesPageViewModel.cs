using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NoteToSelf.Model;
using NoteToSelf.Navigation;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace NoteToSelf.Pages
{
    public class NotesPageViewModel : ViewModelBase
    {
        private readonly Debouncer<TextBox> debouncer = new Debouncer<TextBox>();
        private string searchText;

        private List<Note> notes;
        public List<Note> Notes
        {
            get => notes;
            set => Set(ref notes, value);
        }

        public RelayCommand<Note> EditCommand { get; }
        public RelayCommand<Note> OpenCommand { get; }
        public RelayCommand<Note> DeleteCommand { get; }
        public RelayCommand<TextBox> SearchTextChangedCommand { get; }

        public NotesPageViewModel()
        {
            EditCommand = new RelayCommand<Note>((note) => Edit(note));
            OpenCommand = new RelayCommand<Note>((note) => Open(note));
            DeleteCommand = new RelayCommand<Note>((note) => Delete(note));

            SearchTextChangedCommand = new RelayCommand<TextBox>(SearchTextChanged);
            debouncer.Idled += Debouncer_Idled;

            GetNotes();
        }

        private void GetNotes(string filterString = null)
        {
            searchText = filterString;
            Notes = NotesManager.Instance.GetNotes(filterString);
        }

        private void SearchTextChanged(TextBox textBox)
        {
            if (textBox.Text == string.Empty)
                GetNotes();
            else
                debouncer.OnEvent(textBox);
        }

        private void Debouncer_Idled(TextBox parameter)
        {
            Application.Current.Dispatcher.Invoke(() => GetNotes(parameter.Text));
        }

        private void Delete(Note note)
        {
            NotesManager.Instance.DeleteNote(note);
            GetNotes(searchText);
        }

        private void Edit(Note note)
        {
            var viewModel = new AddPageViewModel(note, false);
            NavigationService.Instance.NavigateTo(viewModel);
        }

        private void Open(Note note)
        {
            var viewModel = new AddPageViewModel(note, true);
            NavigationService.Instance.NavigateTo(viewModel);
        }
    }
}
