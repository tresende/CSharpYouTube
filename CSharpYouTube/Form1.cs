﻿using CSharpYouTube.Properties;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CSharpYouTubePlayer
{
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
            this.axShockwaveFlash1.Size = new Size(409, 316);
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

        public string validaUri(string url)
        {

            if (url.IndexOf("watch?v=") >= 0)
            {
                url = url.Replace("watch?", "");
                url = url.Replace("=", "/");
            }
            return url;
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

        private void rodaVídeoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string urlValida = "";
            if (txtUrl.Text != string.Empty)
            {
                urlValida = validaUri(txtUrl.Text.Trim());
                urlValida = urlValida + "&autoplay=1";
            }
            axShockwaveFlash1.Movie = urlValida;
        }


    }
}