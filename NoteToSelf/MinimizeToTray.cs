using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NoteToSelf
{
    // Thanks! https://github.com/julienadam/AttachR
    public static class MinimizeToTray
    {
        static MinimizeToTrayInstance instance;

        /// Enables "minimize to tray" behavior for the specified Window.
        /// <param name="window">Window to enable the behavior for.</param>
        public static void Enable(MainWindow window)
        {
            instance = new MinimizeToTrayInstance(window);
        }

        public static void Maximize()
        {
            instance.BackToNormal();
        }

        /// Class implementing "minimize to tray" functionality for a Window instance.
        private class MinimizeToTrayInstance
        {
            private MainWindow _window;
            private NotifyIcon _notifyIcon;

            /// Initializes a new instance of the MinimizeToTrayInstance class.
            /// <param name="window">Window instance to attach to.</param>
            public MinimizeToTrayInstance(MainWindow window)
            {
                _window = window;
                _window.StateChanged += new EventHandler(HandleStateChanged);
            }

            /// Handles the Window's StateChanged event.
            /// <param name="sender">Event source.</param>
            /// <param name="e">Event arguments.</param>
            private void HandleStateChanged(object sender, EventArgs e)
            {
                if (_notifyIcon == null)
                {
                    // Initialize NotifyIcon instance "on demand"
                    _notifyIcon = new NotifyIcon();
                    Stream iconStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/Resources/logo.ico")).Stream;
                    _notifyIcon.Icon = new Icon(iconStream);
                    _notifyIcon.MouseClick += new MouseEventHandler(HandleNotifyIconClicked);
                }
                // Update copy of Window Title in case it has changed
                _notifyIcon.Text = _window.Title;

                // Show/hide Window and NotifyIcon
                var minimized = (_window.WindowState == System.Windows.WindowState.Minimized);
                _window.ShowInTaskbar = !minimized;
                _notifyIcon.Visible = minimized;
            }

            /// Handles a click on the notify icon or.
            /// <param name="sender">Event source.</param>
            /// <param name="e">Event arguments.</param>
            private void HandleNotifyIconClicked(object sender, EventArgs e)
            {
                // Restore the Window
                BackToNormal();
            }

            public void BackToNormal()
            {
                _window.WindowState = System.Windows.WindowState.Normal;
            }
        }
    }
}
