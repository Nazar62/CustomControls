using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arduino
{
    public partial class CustomFormBorderMini : UserControl
    {
        Button exit;
        Button minimize;
        Panel buttonPanel;
        Panel panel;
        int startX;
        int buttonsWidth;
        string formName = "Form1";
        int iconSize = 5;
        Color panelColor = Color.DimGray;
        Color buttonCollor = Color.DimGray;
        Color iconColor = Color.Black;

        [Category("Custom")]
        public string FormName { 
            get { return formName; } 
            set 
            { 
                formName = value; 
            } }
        [Category("Custom")]
        public Color PanelColor { 
            get { return panelColor; } 
            set 
            { 
                panelColor = value;
                panel.BackColor = panelColor;
                buttonPanel.BackColor = panelColor;
            } }
        [Category("Custom")]
        public Color ButtonCollor { 
            get { return buttonCollor; } 
            set 
            { 
                buttonCollor = value;
                exit.BackColor = buttonCollor;
                minimize.BackColor = buttonCollor;
            } }
        [Category("Custom")]
        public int IconSize { 
            get { return iconSize; } 
            set { 
                iconSize = value;
                exit.Invalidate();
                minimize.Invalidate();
            } }
        [Category("Custom")]
        public Color IconColor { 
            get { return iconColor; }
            set { 
                iconColor = value;
                exit.Invalidate();
                minimize.Invalidate();
            }
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr one, int two, int three, int four);

        private FormWindowState WindowState;
        public CustomFormBorderMini()
        {
            //InitializeComponent();


            drawButton();

            this.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
            this.Size = new Size(this.ClientSize.Width, 32);
            //this.MinimumSize = new Size(this.ClientSize.Width, 32);
            //this.MaximumSize = new Size(this.ClientSize.Width, 32);
            minimize.Click += Minimize_Click;
            minimize.Paint += Minimize_Paint;
            exit.Click += ExitBtn_Click;
            exit.Paint += Exit_Paint;
        }

        private void Exit_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(iconColor);
            g.DrawLine(pen, (exit.Width / 2) - iconSize, (exit.Height / 2) - iconSize, (exit.Width / 2) + iconSize, (exit.Height / 2) + iconSize);
            g.DrawLine(pen, (exit.Width / 2) - iconSize, (exit.Height / 2) + iconSize, (exit.Width / 2) + iconSize, (exit.Height / 2) - iconSize);
        }

       

        private void Minimize_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(iconColor);
            g.DrawLine(pen, (minimize.Width / 2) - iconSize, (minimize.Height / 2), (minimize.Width / 2) + iconSize, minimize.Height / 2);
        }

        private void drawButton()
        {
            panel = new Panel();

            panel.Width = this.ClientSize.Width;
            panel.Dock = DockStyle.Fill;
            panel.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
            //panel.SuspendLayout();
            panel.MouseDown += Panel_MouseDown;
            panel.ClientSizeChanged += Panel_ClientSizeChanged;
            this.ClientSizeChanged += CustomFormBorder_ClientSizeChanged;
            this.Controls.Add(panel);

            int buttonWidth = 46;
            int buttonHeight = 32;

            // Calculate the total width required for the buttons
            buttonsWidth = (buttonWidth * 2);

            // Calculate the starting X position for the buttons
            startX = this.ClientSize.Width - buttonsWidth;

            buttonPanel = new Panel();
            buttonPanel.Width = buttonsWidth;
            buttonPanel.Dock = DockStyle.Right;
            //buttonPanel.Location = new Point(startX, 0); 
            panel.Controls.Add(buttonPanel);

            minimize = new Button();
            minimize.Size = new Size(46, 32);
            minimize.BackColor = Color.DimGray;
            minimize.FlatStyle = FlatStyle.Flat;
            minimize.FlatAppearance.BorderSize = 0;
            minimize.Location = new Point(0, 0);
            buttonPanel.Controls.Add(minimize);

            exit = new Button();
            exit.BackColor = Color.DimGray;
            exit.Size = new Size(46, 32);
            exit.FlatStyle = FlatStyle.Flat;
            exit.FlatAppearance.BorderSize = 0;
            exit.Location = new Point(minimize.Right, 0);
            exit.FlatAppearance.MouseDownBackColor = Color.DarkRed;
            exit.FlatAppearance.MouseOverBackColor = Color.Red;
            buttonPanel.Controls.Add(exit);
        }

        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            Form form = Application.OpenForms[formName];
            ReleaseCapture();
            SendMessage(form.Handle, 0x112, 0xf012, 0);
        }

        private void CustomFormBorder_ClientSizeChanged(object sender, EventArgs e)
        {
            startX = this.ClientSize.Width - buttonsWidth;
            panel.Width = this.ClientSize.Width;
            buttonPanel.Location = new Point(startX -15, 0);
            exit.Height = this.ClientSize.Height;
            minimize.Height = this.ClientSize.Height;
            exit.Invalidate();
            minimize.Invalidate();
        }

        private void Panel_ClientSizeChanged(object sender, EventArgs e)
        {
            startX = this.ClientSize.Width - buttonsWidth;
            panel.Width = this.ClientSize.Width;
        }

        private void Maximize_Click(object sender, EventArgs e)
        {
            Form form = Application.OpenForms[formName];
            if (form.WindowState != FormWindowState.Maximized)
            {
                form.WindowState = FormWindowState.Maximized;
            } else
            {
                form.WindowState = FormWindowState.Normal;
            }
        }

        private void Minimize_Click(object sender, EventArgs e)
        {
            //WindowState = FormWindowState.Minimized;
            Form form = Application.OpenForms[formName];
            form.WindowState = FormWindowState.Minimized;

        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
// ідея перемістити кнопки в panel з totalwidth шириною і його вже засунути в інший panel