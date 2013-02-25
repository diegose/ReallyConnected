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
        static int Interval = 10 * 1000;

        [STAThread]
        static void Main()
        {
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
            try
            {
                var status = new Ping().Send(Host).Status;
                switch (status)
                {
                    case IPStatus.Success:
                        Connected();
                        break;
                    default:
                        Disconnected(status.ToString());
                        break;
                }
            }
            catch (PingException e)
            {
                Disconnected(e.Message);
            }
        }

        private static void Disconnected(string reason)
        {
            NotifyIcon.Icon = Properties.Resources.Disconnected;
            NotifyIcon.Text = reason;
        }

        private static void Connected()
        {
            NotifyIcon.Icon = Properties.Resources.Connected;
            NotifyIcon.Text = "Connected";
        }
    }
}
