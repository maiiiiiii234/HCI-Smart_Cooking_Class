using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HCI
{
    public partial class Form1 : Form
    {
        //variables
        Bitmap off;

        public Form1()
        {
            this.Load += Form1_Load;
            this.Paint += Form1_Paint;
            Timer t = new Timer();
            t.Start();
            t.Tick += T_Tick;
            this.MouseDown += Form1_MouseDown;
            this.WindowState = FormWindowState.Maximized;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
           
        }

        private void T_Tick(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Dubb(e.Graphics);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
        }


        void DrawScene(Graphics g)
        {
            g.Clear(Color.White);

            // Get the width and height of the client area (form)
            int width = this.ClientSize.Width;
            int height = this.ClientSize.Height;

            // Create a pen to draw the lines
            Pen pen = new Pen(Color.Black, 2);

            // Draw a vertical line dividing the screen into left and right halves
            g.DrawLine(pen, width / 2, 0, width / 2, height);

            // Draw a horizontal line dividing the screen into top and bottom halves
            g.DrawLine(pen, 0, height / 2, width, height / 2);


        }

        void Dubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }
    }
}
