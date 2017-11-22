using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace CSharpYouTube
{
    public partial class Form2 : Form
    {

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        public Form2()
        {
            InitializeComponent();

            this.TopMost = true; // make the form always on top
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D; // hidden border
            this.WindowState = FormWindowState.Maximized; // maximized
            this.MinimizeBox = this.MaximizeBox = false; // not allowed to be minimized
            this.MinimumSize = this.MaximumSize = this.Size; // not allowed to be resized
            this.TransparencyKey = this.BackColor = Color.Red; // the color key to transparent, choose a color that you don't use


            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Sair", OnExit);
            trayIcon = new NotifyIcon();
            trayIcon.Text = "MyTrayApp";
            trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            //trayIcon.Icon = new System.Drawing.Icon("Icon.ico");
            this.SuspendLayout();
            // 
            // SysTrayApp
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "SysTrayApp";
            this.ResumeLayout(false);
            //this.Resize += new System.EventHandler(this.SysTrayApp_Resize);
            //this.DoubleClick += new System.EventHandler(this.SysTrayApp_DoubleClick);

        }

        private void OnExit(object sender, EventArgs e)
        {
            int wl = GetWindowLong(this.Handle, GWL.ExStyle);
            wl = 327688;
            SetWindowLong(this.Handle, GWL.ExStyle, wl);
            //Application.Exit();

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

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            int wl = GetWindowLong(this.Handle, GWL.ExStyle);
            wl = wl | 0x80000 | 0x20;
            SetWindowLong(this.Handle, GWL.ExStyle, wl);
            //SetLayeredWindowAttributes(this.Handle, 0, 128, LWA.Alpha);
        }
    }
}
