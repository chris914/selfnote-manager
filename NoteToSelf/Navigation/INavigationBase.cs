using GalaSoft.MvvmLight;

namespace NoteToSelf.Navigation
{
    internal interface INavigationBase
    {
        void NavigateToViewModel(ViewModelBase viewModel);
    }
}
