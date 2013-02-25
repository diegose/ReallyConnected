using System;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace ReallyConnected
{
    static class Program
    {
        static NotifyIcon NotifyIcon;
        static string Host = "google.com";
        static int Interval = 30000;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            NotifyIcon = new NotifyIcon
                         {
                             Visible = true,
                             ContextMenu = new ContextMenu {MenuItems = {new MenuItem("Exit", Exit)}}
                         };
            var timer = new Timer(Interval);
            timer.Elapsed += delegate { TestConnectivity(); };
            timer.Start();
            TestConnectivity();
            Application.Run();
        }

        static void Exit(object sender, EventArgs args)
        {
            NotifyIcon.Visible = false;
            Application.Exit();
        }

        static void TestConnectivity()
        {
            var status = new Ping().Send(Host).Status;
            switch (status)
            {
                case IPStatus.Success:
                    NotifyIcon.Icon = Properties.Resources.Connected;
                    NotifyIcon.Text = "Connected";
                    break;
                default:
                    NotifyIcon.Icon = Properties.Resources.Connected;
                    NotifyIcon.Text = status.ToString();
                    break;
            }
        }
    }
}
