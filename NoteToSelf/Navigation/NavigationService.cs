using GalaSoft.MvvmLight;

namespace NoteToSelf.Navigation
{
    internal class NavigationService
    {
        private readonly INavigationBase @base;
        public static NavigationService Instance { get; private set; }

        private NavigationService(INavigationBase @base)
        {
            this.@base = @base;
        }

        public static void Initialize(INavigationBase @base) => Instance = new NavigationService(@base);

        public void NavigateTo(ViewModelBase viewModel) =>
            @base.NavigateToViewModel(viewModel);
    }
}
