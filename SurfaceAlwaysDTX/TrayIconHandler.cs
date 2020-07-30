using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SurfaceAlwaysDTX
{
    public class TrayIconHandler : ApplicationContext
    {
        /// <summary>
        /// Tray icon the user can click on to force open the latch and disconnect the screen
        /// </summary>
        private NotifyIcon trayIcon;
        public TrayIconHandler()
        {
            trayIcon = new NotifyIcon();
            trayIcon.Click += (s, e) =>
            {
                // Unlock latch so it may be opened even with low battery
                LatchControler.UnlockLatch();

                // Open the latch to disconnect the screen
                LatchControler.OpenLatch();
            };
            trayIcon.Icon = Icon.FromHandle(Properties.Resources.TrayIcon.GetHicon());
            trayIcon.Text = "Surface Always Detach: Ready to Detach";
            trayIcon.Visible = true;
        }

    }
}
