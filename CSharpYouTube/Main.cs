using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CSharpYouTube;

namespace CSharpYouTubePlayer
{
    public partial class Main : Form
    {

        #region Private Properties

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong(IntPtr hWnd, GWL nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
        private static extern bool SetLayeredWindowAttributes(IntPtr hWnd, int crKey, byte alpha, LWA dwFlags);

        private int WM_MOUSEACTIVATE = 0x0021, MA_NOACTIVATE = 0x0003;

        #endregion

        #region Constructor

        public Main()
        {
            this.Opacity = 0.77;
            InitializeComponent();
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add(Resource.Unlock, Unlock);
            trayIcon = new NotifyIcon();
            trayIcon.Text = Resource.Title;
            trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            trayIcon.Icon = new System.Drawing.Icon("Icon2.ico");
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.WindowState = FormWindowState.Maximized;
            this.MinimizeBox = this.MaximizeBox = false;
            this.MinimumSize = this.MaximumSize = this.Size;
            this.trackBar1.Value = 10;
            this.TopMost = false;
            this.BackColor = Color.White;
            this.trackBar1.Visible = true;
            this.txtUrl.Visible = true;
            this.menuStrip1.Visible = true;
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
        }

        #endregion

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED
                return cp;
            }
        }

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

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEACTIVATE)
            {
                m.Result = (IntPtr)0x80000;
                return;
            }
            base.WndProc(ref m);
        }

        private void LockScreen_Click(object sender, EventArgs e)
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
        
        private void PlayVideo_Click(object sender, EventArgs e)
        {
            string urlValida = "";
            if (txtUrl.Text != string.Empty)
            {
                string source = txtUrl.Text.Trim();
                string[] stringSeparators = new string[] { "v=" };
                var result = source.Split(stringSeparators, StringSplitOptions.None);


                urlValida = string.Format("https://www.youtube.com/embed/{0}?autoplay=1", result[1]);
            }
            if (!InternetExplorerBrowserEmulation.IsBrowserEmulationSet())
            {
                InternetExplorerBrowserEmulation.SetBrowserEmulationVersion();
            }
            this.webBrowser1.Navigate(urlValida);
        }
    }
}