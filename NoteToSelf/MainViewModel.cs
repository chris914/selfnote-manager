using GalaSoft.MvvmLight;
using NoteToSelf.Model;
using NoteToSelf.Navigation;
using NoteToSelf.Pages;

namespace NoteToSelf
{
    internal class MainViewModel : ViewModelBase, INavigationBase
    {
        private ViewModelBase currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => currentViewModel;
            private set => Set(ref currentViewModel, value);
        }

        public MainViewModel()
        {
            NavigationService.Initialize(this);
            NotesManager.Instance.LoadNotes();

            NavigateToViewModel(new NotesPageViewModel());
        }

        public void NavigateToViewModel(ViewModelBase viewModel)
        {
            CurrentViewModel = viewModel;
        }

        internal void NavigateToAdd(string selectedText, bool isSilent)
        {
            var viewModel = new AddPageViewModel(selectedText, isSilent);
            NavigateToViewModel(viewModel);
        }

        internal void NavigateToAdd(Note note, string selectedText, bool isSilent)
        {
            var viewModel = new AddPageViewModel(note, false, isSilent, selectedText);
            NavigateToViewModel(viewModel);
        }
    }
}
