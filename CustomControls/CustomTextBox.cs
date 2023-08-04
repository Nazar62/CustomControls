using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomControlsTest
{
    public partial class CustomTextBox : UserControl
    {

        //Fields
        Color backColor = Color.DimGray;
        Color lineColor = Color.Black;
        int lineHeight = 5;
        Color placeholderColor = Color.Silver;
        Color textColor = Color.White;
        string placeholderText;
        bool passwordChar = false;
        bool isPlaceholder = false;

        //Items
        TextBox textBox;

        [Category("Custom")]
        public new Color BackColor { 
            get { return backColor; } 
            set {
                backColor = value; 
            } }
        [Category("Custom")]
        public Color LineColor { 
            get {return lineColor;} 
            set {
                lineColor = value;
                this.Invalidate();
            } }
        [Category("Custom")]
        public int LineHeight { 
            get {return lineHeight;} 
            set {
                lineHeight = value;
                this.Invalidate();
            } }
        [Category("Custom")]
        public Color PlaceholderColor { 
            get {return placeholderColor;} 
            set {
                placeholderColor = value;
                textBox.ForeColor = value;
            } }
        [Category("Custom")]
        public Color TextColor { 
            get {return textColor;} 
            set {
                textColor = value; 
            } }
        [Category("Custom")]
        public string PlaceholderText { 
            get {return placeholderText;} 
            set {
                placeholderText = value;
                textBox.Text = placeholderText;
                SetPlaceholder();
            } }
        [Category("Custom")]
        public bool PasswordChar { 
            get {return passwordChar;} 
            set {
                passwordChar = value;
                textBox.UseSystemPasswordChar = passwordChar;
            } }
        [Category("Custom")]
        public string Texts
        {
            get { if (isPlaceholder) return "";
                else return textBox.Text;
                }
            set
            {
                textBox.Text = value;
                SetPlaceholder();
            }
        }
        public CustomTextBox()
        {
            textBox = new TextBox();
            textBox.Width = this.Width - 10;
            textBox.Height = this.Height;
            //textBox.Dock = DockStyle.Left;
            textBox.BorderStyle = BorderStyle.None;
            textBox.BackColor = backColor;
            //textBox.Text = "hello";
            textBox.ForeColor = placeholderColor;
            textBox.Location = new Point(10,0);
            textBox.Enter += TextBox_Enter;
            textBox.Leave += TextBox_Leave;
            this.Size = new Size(100, 20);
            this.Controls.Add(textBox);
            this.Paint += CustomTextBox_Paint;
            SetPlaceholder();
        }

        private void CustomTextBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            using (Pen pen = new Pen(lineColor))
            {
                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                for (int i = 1; i < lineHeight; i++)
                {
                    g.DrawLine(pen, 0, this.Height - i, this.Width, this.Height - i);
                }
            }  
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            SetPlaceholder();
        }
        private void SetPlaceholder()
        {
            if (string.IsNullOrWhiteSpace(textBox.Text) && placeholderText != "")
            {
                isPlaceholder = true;
                textBox.Text = placeholderText;
                textBox.ForeColor = placeholderColor;
                if (passwordChar)
                {
                    textBox.UseSystemPasswordChar = false;
                }
            }
        }
        private void RemovePlacehoder()
        {
            if (placeholderText != "" && isPlaceholder)
            {
                isPlaceholder = false;
                textBox.ForeColor = textColor;
                textBox.Text = "";
                if (passwordChar)
                {
                    textBox.UseSystemPasswordChar = true;
                }
            }
        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            RemovePlacehoder();
        }
    }
}
