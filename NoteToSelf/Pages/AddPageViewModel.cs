using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NoteToSelf.Model;
using NoteToSelf.Navigation;
using System;
using System.Windows;

namespace NoteToSelf.Pages
{
    public class AddPageViewModel : ViewModelBase
    {
        private bool isReadOnly;
        public bool IsReadOnly
        {
            get => isReadOnly;
            set => Set(ref isReadOnly, value);
        }

        private string title;
        public string Title
        {
            get => title;
            set => Set(ref title, value);
        }

        private string tags;
        public string Tags
        {
            get => tags;
            set => Set(ref tags, value);
        }

        private string description;
        public string Description
        {
            get => description;
            set => Set(ref description, value);
        }

        private bool isNoteCategory;
        public bool IsNoteCategory
        {
            get => isNoteCategory;
            set => Set(ref isNoteCategory, value);
        }

        private Note note;
        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand SwitchNoteCategoryCommand { get; }

        private System.Windows.Forms.Timer timer;
        public AddPageViewModel(string description = null, bool isSilent = false)
        {
            Description = description;
            IsReadOnly = false;
            IsNoteCategory = false;
            if (isSilent)
            {
                StartTimer();
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            SwitchNoteCategoryCommand = new RelayCommand(SwitchNoteCategory);
        }

        public void StartTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 2500;
            timer.Tick += (sender, args) => { timer.Stop(); Save(); };
            timer.Start();

            Application.Current.MainWindow.StateChanged += (sender, args) =>
            {
                if (Application.Current.MainWindow.WindowState == WindowState.Maximized || Application.Current.MainWindow.WindowState == WindowState.Normal)
                {
                    timer.Stop();
                }
            };
        }

        public AddPageViewModel(Note note, bool isReadOnly, bool isSilent = false, string extraText = null)
        {
            this.note = note;
            Title = note.Title;
            Description = note.Description;
            if (!string.IsNullOrEmpty(extraText))
            {
                Description += "\r\n";
                Description += extraText;
            }

            IsNoteCategory = note.IsNoteCategory;

            if (note.Tags != null)
                Tags = string.Join(" ,", note.Tags);

            IsReadOnly = isReadOnly;

            if (isSilent)
            {
                StartTimer();
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            SwitchNoteCategoryCommand = new RelayCommand(SwitchNoteCategory);
        }

        private void SwitchNoteCategory()
        {
            if (this.note != null)
            {
                IsNoteCategory = !IsNoteCategory;
                note.IsNoteCategory = !note.IsNoteCategory;
            }
        }

        private void Cancel()
        {
            NavigationService.Instance.NavigateTo(new NotesPageViewModel());
        }

        private void Save()
        {
            if (this.note == null)
            {
                var newNote = new Note(Title, Description, DateTime.Now, DateTime.Now, Tags);
                NotesManager.Instance.AddNote(newNote);
            }
            else
            {
                note.Title = Title;
                note.Description = Description;
                note.Tags = NoteHelper.GetTagsFromString(Tags);
                note.LastModified = DateTime.Now;
                NotesManager.Instance.EditNote(note);
            }

            NavigationService.Instance.NavigateTo(new NotesPageViewModel());
        }
    }
}
