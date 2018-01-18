using CSharpYouTube.Properties;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;
using Microsoft.Win32;
using System.IO;
using System.Security;

namespace CSharpYouTubePlayer
{

    public enum BrowserEmulationVersion
    {
        Default = 0,
        Version7 = 7000,
        Version8 = 8000,
        Version8Standards = 8888,
        Version9 = 9000,
        Version9Standards = 9999,
        Version10 = 10000,
        Version10Standards = 10001,
        Version11 = 11000,
        Version11Edge = 11001
    }

    public partial class Form1 : Form
    {

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        public Form1()
        {
            this.Opacity = 0.77;
            InitializeComponent();
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Destravar", Unlock);
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Player Thiago";
            trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            trayIcon.Icon = new System.Drawing.Icon("Icon2.ico");
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.WindowState = FormWindowState.Maximized;
            this.MinimizeBox = this.MaximizeBox = false;
            this.MinimumSize = this.MaximumSize = this.Size;
            this.trackBar1.Value = 10;
            //this.axShockwaveFlash1.Size = new Size(409, 316);
            this.TopMost = false;
            this.BackColor = Color.White;
            this.trackBar1.Visible = true;
            this.txtUrl.Visible = true;
            this.menuStrip1.Visible = true;
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
        }

        private void Unlock(object sender, EventArgs e)
        {
            int wl = GetWindowLong(this.Handle, GWL.ExStyle);
            wl = 327688;
            SetWindowLong(this.Handle, GWL.ExStyle, wl);
            this.TopMost = false;
            this.BackColor = Color.White;
            this.trackBar1.Visible = true;
            this.menuStrip1.Visible = true;
            this.txtUrl.Visible = true;
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
        }

        //public string validaUri(string url)
        //{

        //    if (url.IndexOf("v=") >= 0)
        //    {
        //        url = url.Replace("watch?", "");
        //        url = url.Replace("=", "/");
        //    }
        //    return url;
        //}

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.Opacity = trackBar1.Value * 0.1;
            }
            catch
            {

            }

        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED
                return cp;
            }
        }

        private int WM_MOUSEACTIVATE = 0x0021, MA_NOACTIVATE = 0x0003;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEACTIVATE)
            {
                m.Result = (IntPtr)0x80000;
                return;
            }
            base.WndProc(ref m);
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        public enum GWL
        {
            ExStyle = -20
        }

        public enum WS_EX
        {
            Transparent = 0x20,
            Layered = 0x80000
        }

        public enum LWA
        {
            ColorKey = 0x1,
            Alpha = 0x2
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong(IntPtr hWnd, GWL nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, int crKey, byte alpha, LWA dwFlags);

        private void travarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TransparencyKey = this.BackColor = Color.Red; // the color key to transparent, choose a color that you don't use
            this.FormBorderStyle = FormBorderStyle.None;
            int wl = GetWindowLong(this.Handle, GWL.ExStyle);
            wl = wl | 0x80000 | 0x20;
            SetWindowLong(this.Handle, GWL.ExStyle, wl);
            this.TopMost = true;
            this.menuStrip1.Visible = false;
            this.trackBar1.Visible = false;
            this.txtUrl.Visible = false;
        }


        public static BrowserEmulationVersion GetBrowserEmulationVersion()
        {
            BrowserEmulationVersion result;

            result = BrowserEmulationVersion.Default;

            try
            {
                RegistryKey key;

                key = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);
                if (key != null)
                {
                    string programName;
                    object value;

                    programName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
                    value = key.GetValue(programName, null);

                    if (value != null)
                    {
                        result = (BrowserEmulationVersion)Convert.ToInt32(value);
                    }
                }
            }
            catch (SecurityException)
            {
                // The user does not have the permissions required to read from the registry key.
            }
            catch (UnauthorizedAccessException)
            {
                // The user does not have the necessary registry rights.
            }

            return result;
        }


        public static bool IsBrowserEmulationSet()
        {
            return GetBrowserEmulationVersion() != BrowserEmulationVersion.Default;
        }

        private void rodaVídeoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string urlValida = "";
            if (txtUrl.Text != string.Empty)
            {

                string source = txtUrl.Text.Trim();
                string[] stringSeparators = new string[] { "v=" };
                var result = source.Split(stringSeparators, StringSplitOptions.None);
                urlValida = "https://www.youtube.com/embed/" + result[1];
            }
            if (!IsBrowserEmulationSet())
            {
                SetBrowserEmulationVersion();
            }
            this.webBrowser1.Navigate(urlValida);

        }

        private const string InternetExplorerRootKey = @"Software\Microsoft\Internet Explorer";
        private const string BrowserEmulationKey = InternetExplorerRootKey + @"\Main\FeatureControl\FEATURE_BROWSER_EMULATION";


        public static bool SetBrowserEmulationVersion(BrowserEmulationVersion browserEmulationVersion)
        {
            bool result;

            result = false;

            try
            {
                RegistryKey key;

                key = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);

                if (key != null)
                {
                    string programName;

                    programName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);

                    if (browserEmulationVersion != BrowserEmulationVersion.Default)
                    {
                        // if it's a valid value, update or create the value
                        key.SetValue(programName, (int)browserEmulationVersion, RegistryValueKind.DWord);
                    }
                    else
                    {
                        // otherwise, remove the existing value
                        key.DeleteValue(programName, false);
                    }

                    result = true;
                }
            }
            catch (SecurityException)
            {
                // The user does not have the permissions required to read from the registry key.
            }
            catch (UnauthorizedAccessException)
            {
                // The user does not have the necessary registry rights.
            }

            return result;
        }


        public static int GetInternetExplorerMajorVersion()
        {
            int result;

            result = 0;

            try
            {
                RegistryKey key;

                key = Registry.LocalMachine.OpenSubKey(InternetExplorerRootKey);

                if (key != null)
                {
                    object value;

                    value = key.GetValue("svcVersion", null) ?? key.GetValue("Version", null);

                    if (value != null)
                    {
                        string version;
                        int separator;

                        version = value.ToString();
                        separator = version.IndexOf('.');
                        if (separator != -1)
                        {
                            int.TryParse(version.Substring(0, separator), out result);
                        }
                    }
                }
            }
            catch (SecurityException)
            {
                // The user does not have the permissions required to read from the registry key.
            }
            catch (UnauthorizedAccessException)
            {
                // The user does not have the necessary registry rights.
            }

            return result;
        }


        public static bool SetBrowserEmulationVersion()
        {
            int ieVersion;
            BrowserEmulationVersion emulationCode;

            ieVersion = GetInternetExplorerMajorVersion();

            if (ieVersion >= 11)
            {
                emulationCode = BrowserEmulationVersion.Version11;
            }
            else
            {
                switch (ieVersion)
                {
                    case 10:
                        emulationCode = BrowserEmulationVersion.Version10;
                        break;
                    case 9:
                        emulationCode = BrowserEmulationVersion.Version9;
                        break;
                    case 8:
                        emulationCode = BrowserEmulationVersion.Version8;
                        break;
                    default:
                        emulationCode = BrowserEmulationVersion.Version7;
                        break;
                }
            }

            return SetBrowserEmulationVersion(emulationCode);
        }

    }
}