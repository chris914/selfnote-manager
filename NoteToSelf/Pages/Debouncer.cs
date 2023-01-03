using System.Threading;

namespace NoteToSelf.Pages
{
    /// <summary>
    /// Debouncer class to help delay events.
    /// </summary>
    public class Debouncer<T> where T : class
    {
        public delegate void EventHandlerDelegate(T parameter);
        public event EventHandlerDelegate Idled;

        int waiting;
        Timer timer;
        T parameter;

        public Debouncer(int waiting = 300)
        {
            this.waiting = waiting;
            timer = new Timer(x =>
            {
                Idled(parameter);
            });
        }

        public void OnEvent(T parameter = null)
        {
            this.parameter = parameter;
            timer.Change(waiting, Timeout.Infinite);
        }
    }

    /// <summary>
    /// Parameterless debouncer to help delay events.
    /// </summary>
    public class Debouncer
    {
        public delegate void EventHandlerDelegate();
        public event EventHandlerDelegate Idled;

        int waiting;
        Timer timer;

        public Debouncer(int waiting = 300)
        {
            this.waiting = waiting;
            timer = new Timer(x =>
            {
                Idled();
            });
        }

        public void OnEvent()
        {
            timer.Change(waiting, Timeout.Infinite);
        }
    }
}
