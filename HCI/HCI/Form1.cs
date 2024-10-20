using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HCI
{

    public class something
    {
        // Properties
        public Bitmap Image { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        // Constructor to initialize the object
        public something(Bitmap image, int x, int y, int width, int height)
        {
            Image = image;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        // Method to draw the image on a graphics object
        //public void Draw(Graphics g)
        //{
        //    if (Image != null)
        //    {
        //        g.DrawImage(Image, X, Y, Width, Height);
        //    }
        //}


        // Method to display object details (optional)
        //public void ShowDetails()
        //{
        //    Console.WriteLine($"Position: ({X}, {Y}), Size: ({Width}x{Height})");
        //}
    }
    public partial class Form1 : Form
    {
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

       public class ingredients
        {
            // Properties
            public Bitmap Image { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }

            // Constructor to initialize the object
            public ingredients(Bitmap image, int x, int y, int width, int height)
            {
                Image = image;
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }
        }
        public Form1()
        {
            this.Load += Form1_Load;
            this.Paint += Form1_Paint;
            Timer t = new Timer();
            t.Start();
            t.Tick += T_Tick;
            this.WindowState = FormWindowState.Maximized;
            this.MouseDown += Form1_MouseDown;
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
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);

        }
        private void T_Tick(object sender, EventArgs e)
        {
            Dubb(this.CreateGraphics());
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
            g.Clear(Color.Black);
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


            // Add some objects to the list
            something ta2leeb_ma3la2a=new something(new Bitmap("ta2leeb (ma3la2a).png"), width / 2+55, height / 2+50, 180, 320);
            something ta2leeb_shoka=new something(new Bitmap("ta2leeb (shoka).png"), width / 2 +55 , height / 2 +50, 180, 320);

            something mafrash = new something(new Bitmap("mafrash.png"), width / 2 - 150, height / 2 - 400, 650, 400);
            something dish = new something(new Bitmap("dish.png"), width / 2 + 165, height / 2 - 300, 200, 200);
            something ma3la2a = new something(new Bitmap("ma3la2a.png"), width / 2 + 170, height / 2 - 300, 170, 170);
            something shoka = new something(new Bitmap("shoka.png"), width / 2 +190, height / 2 - 300, 170, 170);

            //something shoka = new something(new Bitmap("shoka.png"), width / 2 + 50, height / 2 - 280, 250, 250);
            something tasa = new something(new Bitmap("tasa.png"), width / 2 + 433, height / 2 - 480, 350, 350);
            something hala = new something(new Bitmap("7ala.png"), width / 2 + 485, height / 2 - 250, 250, 250);

            something knife = new something(new Bitmap("knife.png"), width -200, height / 2 +140, 150, 180);
            //something water = new something(new Bitmap("water.png"), width / 2 +20, height / 2 - 300, 200, 290);

            something board = new something(new Bitmap("board.png"), width / 2 + 180, height / 2-30 , 580, 480);


            // Create a pen to draw the lines
            Pen pen = new Pen(Color.Black, 2);

            // Draw a vertical line dividing the screen into left and right halves
            g.DrawLine(pen, width / 2, 0, width / 2, height);
            g.DrawLine(pen, 0, height / 2, width, height / 2);
            ingredients milk = new ingredients(new Bitmap("milk.png"), 500, -70, 350, 650);
            ingredients salt = new ingredients(new Bitmap("salt.png"), 560, 12, 100, 125);
            ingredients pepper = new ingredients(new Bitmap("pepper-removebg-preview.png"), 660, 20, 50, 112);
            ingredients oil = new ingredients(new Bitmap("oil.png"), 475, 20, 100, 112);
            ingredients water = new ingredients(new Bitmap("water.png"), 495, 145, 100, 250);
            ingredients taba2Top = new ingredients(new Bitmap("taba2.png"), 0, -25, 290, 270);
            ingredients taba2Bottom = new ingredients(new Bitmap("taba2.png"), 0, 170, 290, 270);
            ingredients pasta = new ingredients(new Bitmap("pasta.png"), 60, 35, 165, 165);
            ingredients gebna = new ingredients(new Bitmap("gebna.png"), 70, 235, 150, 140);
            ingredients taba2Bottomright = new ingredients(new Bitmap("taba2.png"), 200, 170, 290, 270);
            ingredients taba2upright = new ingredients(new Bitmap("taba2.png"), 200, -25, 290, 270);
            ingredients chicken = new ingredients(new Bitmap("chicken breast.png"), 245, 235, 200, 145);
            ingredients butter = new ingredients(new Bitmap("butterbs.png"), 225, 7, 250, 185);
            g.DrawImage(taba2Top.Image, taba2Top.X, taba2Top.Y, taba2Top.Width, taba2Top.Height);
            g.DrawImage(taba2Bottom.Image, taba2Bottom.X, taba2Bottom.Y, taba2Bottom.Width, taba2Bottom.Height);
            g.DrawImage(taba2upright.Image, taba2upright.X, taba2upright.Y, taba2upright.Width, taba2upright.Height);
            g.DrawImage(taba2Bottomright.Image, taba2Bottomright.X, taba2Bottomright.Y, taba2Bottomright.Width, taba2Bottomright.Height);
            g.DrawImage(pasta.Image, pasta.X, pasta.Y, pasta.Width, pasta.Height);
            g.DrawImage(milk.Image, milk.X, milk.Y, milk.Width, milk.Height);
            g.DrawImage(salt.Image, salt.X, salt.Y, salt.Width, salt.Height);
            g.DrawImage(pepper.Image, pepper.X, pepper.Y, pepper.Width, pepper.Height);
            g.DrawImage(chicken.Image, chicken.X, chicken.Y, chicken.Width, chicken.Height);
            g.DrawImage(butter.Image, butter.X, butter.Y, butter.Width, butter.Height);

            g.DrawImage(oil.Image, oil.X, oil.Y, oil.Width, oil.Height);
            g.DrawImage(water.Image, water.X, water.Y, water.Width, water.Height);
            g.DrawImage(gebna.Image, gebna.X, gebna.Y, gebna.Width, gebna.Height);
            g.DrawImage(ta2leeb_ma3la2a.Image, ta2leeb_ma3la2a.X, ta2leeb_ma3la2a.Y, ta2leeb_ma3la2a.Width, ta2leeb_ma3la2a.Height);
            g.DrawImage(ta2leeb_shoka.Image, ta2leeb_shoka.X, ta2leeb_shoka.Y, ta2leeb_shoka.Width, ta2leeb_shoka.Height);

            g.DrawImage(mafrash.Image, mafrash.X, mafrash.Y, mafrash.Width, mafrash.Height);
            g.DrawImage(dish.Image, dish.X, dish.Y, dish.Width, dish.Height);
            g.DrawImage(ma3la2a.Image, ma3la2a.X, ma3la2a.Y, ma3la2a.Width, ma3la2a.Height);
            g.DrawImage(shoka.Image, shoka.X, shoka.Y, shoka.Width, shoka.Height);

            g.DrawImage(tasa.Image, tasa.X, tasa.Y, tasa.Width, tasa.Height);
            g.DrawImage(hala.Image, hala.X, hala.Y, hala.Width, hala.Height);

            //  g.DrawImage(water.Image, water.X, water.Y, water.Width, water.Height);

            g.DrawImage(board.Image, board.X, board.Y, board.Width, board.Height);
            g.DrawImage(knife.Image, knife.X, knife.Y, knife.Width, knife.Height);
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