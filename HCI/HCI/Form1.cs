using System;
using System.Drawing;
using System.Windows.Forms;

namespace HCI
{
    public partial class Form1 : Form
    {
        //variables
        Bitmap off;
        bool isflame=false;
        Flame flameEinKbera = new Flame();
        Flame flameEinSo8ayara = new Flame();
        AinElbotagaz einElSo8ayara = new AinElbotagaz();
        AinElbotagaz einElKbera = new AinElbotagaz();



        public class AinElbotagaz
        {
            public int X, Y;
            public Bitmap img;
            public float angel;
        }

        public class Flame
        {
            public int X, Y;
            public Bitmap img;
        }
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
            if (!isflame)
            {
                ////flame 1 el3een elkbera
                //isflame = true;
                //flameEinKbera.X = 485;
                //flameEinKbera.Y = this.Height - 325;
                //flameEinKbera.img = new Bitmap("flame1.png");

                ////flame 1 el3een elso8ayara
                isflame = true;
                flameEinSo8ayara.X = 185;
                flameEinSo8ayara.Y = this.Height - 305;
                flameEinSo8ayara.img = new Bitmap("flame1.png");

                ////flame 2 el3een elkbera
                isflame = true;
                flameEinKbera.X = 466;
                flameEinKbera.Y = this.Height - 329;
                flameEinKbera.img = new Bitmap("flame2.png");

                ////flame 2 el3een elso8ayara
                //isflame = true;
                //flameEinSo8ayara.X = 173;
                //flameEinSo8ayara.Y = this.Height - 305;
                //flameEinSo8ayara.img = new Bitmap("flame2.png");
            }
        }

        private void T_Tick(object sender, EventArgs e)
        {
            Dubb(this.CreateGraphics());
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Dubb(e.Graphics);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            einElSo8ayara.X = 28;
            einElSo8ayara.Y = this.Height - 355;
            einElSo8ayara.img = new Bitmap("3eenElbotagaz0.png");

            einElKbera.X = 28;
            einElKbera.Y = this.Height - 183;
            einElKbera.img = new Bitmap("3eenElbotagaz45.png");
        }

        // The function where the drawing happens
        void DrawScene(Graphics g)
        {
            g.Clear(Color.White);
            Bitmap bg = new Bitmap("marbleBG.png");
            int width = this.ClientSize.Width;
            int height = this.ClientSize.Height;
            g.DrawImage(bg, 0, 0, width, height);
            Bitmap stove = new Bitmap("Stove1.png");
            g.DrawImage(stove, 0, height / 2, width / 2, height - 400);
            g.DrawImage(einElSo8ayara.img, einElSo8ayara.X, einElSo8ayara.Y, 55, 55);
            g.DrawImage(einElKbera.img, einElKbera.X, einElKbera.Y, 55, 55);

            if (isflame)
            {
                ////flame 1 el3een elkbera
                //g.DrawImage(flameEinKbera.img, flameEinKbera.X, flameEinKbera.Y, 190, 170);

                ////flame 1 el3een elso8ayara
                g.DrawImage(flameEinSo8ayara.img, flameEinSo8ayara.X, flameEinSo8ayara.Y, 150, 130);

                ////flame 2 el3een elkbera
                g.DrawImage(flameEinKbera.img, flameEinKbera.X, flameEinKbera.Y, 230, 185);

                //flame 2 el3een elso8ayara
                //g.DrawImage(flameEinSo8ayara.img, flameEinSo8ayara.X, flameEinSo8ayara.Y, 180, 140);
            }

            // Create a pen to draw the lines
            Pen pen = new Pen(Color.Black, 2);

            // Draw a vertical line dividing the screen into left and right halves
            g.DrawLine(pen, width / 2, 0, width / 2, height);

            // Draw a horizontal line dividing the screen into top and bottom halves
            g.DrawLine(pen, 0, height / 2, width, height / 2);

            // Dispose of the pen
            pen.Dispose();
        }

        // The function responsible for double buffering
        void Dubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2); // Call the DrawScene method to draw on the off-screen bitmap
            g.DrawImage(off, 0, 0); // Draw the off-screen bitmap to the form
        }
    }
}
