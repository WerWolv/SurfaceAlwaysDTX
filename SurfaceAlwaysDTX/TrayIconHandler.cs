using System.Windows.Forms;

namespace SurfaceAlwaysDTX
{
    public class TrayIconHandler : ApplicationContext
    {
        /// <summary>
        /// Tray icon the user can click on to force open the latch and disconnect the screen
        /// </summary>
        private NotifyIcon trayIcon;
        private Timer themePollingTimer;

        public TrayIconHandler()
        {
            this.trayIcon = new NotifyIcon();
            this.trayIcon.Click += (s, e) =>
            {
                // Unlock latch so it may be opened even with low battery
                LatchControler.UnlockLatch();

                // Open the latch to disconnect the screen
                LatchControler.OpenLatch();
            };
            this.trayIcon.Icon = Properties.Resources.TrayIcon_White;
            this.trayIcon.Text = "Surface Always Detach: Ready to Detach";
            this.trayIcon.Visible = true;

            // Periodically check if the current Windows theme has changed and adjust the tray icon accordingly
            this.themePollingTimer = new Timer();
            this.themePollingTimer.Tick += (s, e) =>
            {
                bool isLightTheme = true;
                try
                {
                    var value = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", "1");
                    if (value != null && value.ToString() == "0")
                        isLightTheme = false;
                }
                catch 
                {
                    return;
                }

                if (isLightTheme)
                    this.trayIcon.Icon = Properties.Resources.TrayIcon_Black;
                else
                    this.trayIcon.Icon = Properties.Resources.TrayIcon_White;
            };
            this.themePollingTimer.Interval = 5000;
            this.themePollingTimer.Start();

        }
    }
}
