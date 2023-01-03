using GalaSoft.MvvmLight.Command;
using NoteToSelf.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace NoteToSelf
{
    /// <summary>
    /// Interaction logic for SelectionWindow.xaml
    /// </summary>
    public partial class SelectionWindow : Window, INotifyPropertyChanged
    {
        public List<NoteCategory> noteCategories;
        public List<NoteCategory> NoteCategories
        {
            get => noteCategories;
            set
            {
                noteCategories = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoteCategories)));
            }
        }

        private int selection = 0;
        public Note selectedNote;

        public SelectionWindow()
        {
            InitializeComponent();
            DataContext = this;

            LoadNoteCategories();

            NoteCategoryMouseEnterCommand = new RelayCommand<NoteCategory>(NoteCategoryMouseEnter);
        }

        public RelayCommand<NoteCategory> NoteCategoryMouseEnterCommand { get; }

        //public bool ShowOrReturn(string text)
        //{
        //    this.ShowDialog();

        //    if (!Keyboard.IsKeyDown(Key.Q) && !Keyboard.IsKeyDown(Key.LeftCtrl))
        //        return false;

        //    LoadNoteCategories();
        //    return true;
        //}

        private void LoadNoteCategories()
        {
            var categories = new List<NoteCategory>();
            categories.Add(new NoteCategory(null) { Title = "New Category", IsSelected = true });
            foreach (var item in NotesManager.Instance.GetNotes().Where(x => x.IsNoteCategory).ToList())
            {
                categories.Add(new NoteCategory(item)
                {
                    Title = item.Title,
                    IsSelected = false,
                });
            }

            NoteCategories = categories;
        }

        private void SelectNext()
        {
            foreach (var item in NoteCategories)
                item.IsSelected = false;

            selection++;
            if (selection > NoteCategories.Count - 1)
                selection = 0;

            NoteCategories[selection].IsSelected = true;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Q)
            //{
            //    SelectNext();
            //}
            //if (e.Key == Key.Enter)
            //{
            //    selectedNote = NoteCategories[selection].Note;
            //    this.Close();
            //}
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Timer t;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Keyboard.IsKeyDown(Key.CapsLock))
            {
                this.Close();
            }
        }

        private bool close = false;
        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (close)
            {
                t.Stop();
                selectedNote = NoteCategories[selection].Note;
                try
                {
                    Dispatcher.Invoke(() => this.Close());
                }
                catch (Exception exc)
                {
                    Debug.WriteLine(exc.Message);
                }

            }

            try
            {
                Dispatcher.Invoke(() =>
                   {
                       var kCtrlDown = Keyboard.IsKeyDown(Key.CapsLock);
                       var kQDown = Keyboard.IsKeyDown(Key.Q);
                       Debug.WriteLine(kCtrlDown);
                       Debug.WriteLine(kQDown);
                       if (!kCtrlDown && !kQDown)
                           close = true;
                   });
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        private void NoteCategoryMouseEnter(NoteCategory obj)
        {
            if (obj.IsSelected)
                return;

            foreach (var item in NoteCategories)
                item.IsSelected = false;

            obj.IsSelected = true;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.CapsLock)
            {
                (Application.Current.MainWindow as MainWindow).locked = false;
                selectedNote = NoteCategories.FirstOrDefault(x => x.IsSelected).Note;
                this.Close();
                close = true;
                //selectedNote = NoteCategories[selection].Note;
                //if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.Q))
                //    this.Close();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Q)
            {
                //SelectNext();
            }
        }
    }

    public class NoteCategory : INotifyPropertyChanged
    {
        private Note note;
        public Note Note { get => note; }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }

        private string title;

        public NoteCategory(Note note)
        {
            this.note = note;
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
    }
}
