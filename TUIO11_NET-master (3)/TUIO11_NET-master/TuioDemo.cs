
using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System.IO;
using System.Drawing.Drawing2D;
using TUIO;

	public class TuioDemo : Form , TuioListener
{
    Bitmap off;
    bool isflame = false;
    Flame flameEinKbera = new Flame();
    Flame flameEinSo8ayara = new Flame();
    AinElbotagaz einElSo8ayara = new AinElbotagaz();
    AinElbotagaz einElKbera = new AinElbotagaz();
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

		private TuioClient client;
		private Dictionary<long,TuioObject> objectList;
		private Dictionary<long,TuioCursor> cursorList;
		private Dictionary<long,TuioBlob> blobList;

		public static int width, height;
		private int window_width =  640;
		private int window_height = 480;
		private int window_left = 0;
		private int window_top = 0;
		private int screen_width = Screen.PrimaryScreen.Bounds.Width;
		private int screen_height = Screen.PrimaryScreen.Bounds.Height;
	    public string objectImagePath;
		public string backgroundImagePath;
	    public int fire = -1 , fireID=0,KnifeID=1,ChickenID=2,chknflag=0;
		public	TuioObject knifeObject;
		public  TuioObject chickenObject;

	    private bool fullscreen;
		private bool verbose;

		Font font = new Font("Arial", 10.0f);
		SolidBrush fntBrush = new SolidBrush(Color.White);
		SolidBrush bgrBrush = new SolidBrush(Color.FromArgb(0,0,64));
		SolidBrush curBrush = new SolidBrush(Color.FromArgb(192, 0, 192));
		SolidBrush objBrush = new SolidBrush(Color.FromArgb(64, 0, 0));
		SolidBrush blbBrush = new SolidBrush(Color.FromArgb(64, 64, 64));
		Pen curPen = new Pen(new SolidBrush(Color.Blue), 1);

		public TuioDemo(int port) {
		
			verbose = false;
			fullscreen = false;
			width = window_width;
			height = window_height;
			this.Load += TuioDemo_Load;
			this.Paint += TuioDemo_Paint;

			//Timer t = new Timer();
			//t.Start();
			//t.Tick += T_Tick;
			this.WindowState = FormWindowState.Maximized;
			//this.MouseDown += Form1_MouseDown;
			// this.ClientSize = new System.Drawing.Size(width, height);
			this.Name = "TuioDemo";
			this.Text = "TuioDemo";
			
			this.Closing+=new CancelEventHandler(Form_Closing);
			this.KeyDown+=new KeyEventHandler(Form_KeyDown);

			this.SetStyle( ControlStyles.AllPaintingInWmPaint |
							ControlStyles.UserPaint |
							ControlStyles.DoubleBuffer, true);

			objectList = new Dictionary<long,TuioObject>(128);
			cursorList = new Dictionary<long,TuioCursor>(128);
			blobList   = new Dictionary<long,TuioBlob>(128);
			
			client = new TuioClient(port);
			client.addTuioListener(this);

			client.connect();
		}
		private void TuioDemo_Paint(object sender, PaintEventArgs e)
    {
        DrawScene(e.Graphics);
    }
		private void TuioDemo_Load(object sender, EventArgs e)
    {
        off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
        einElSo8ayara.X = 28;
        einElSo8ayara.Y = this.Height - 355;
        einElSo8ayara.img = new Bitmap("3eenElbotagaz0.png");

        einElKbera.X = 28;
        einElKbera.Y = this.Height - 183;
        einElKbera.img = new Bitmap("3eenElbotagaz45.png");
    }

		private void Form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {

 			if ( e.KeyData == Keys.F1) {
	 			if (fullscreen == false) {

					width = screen_width;
					height = screen_height;

					window_left = this.Left;
					window_top = this.Top;

					this.FormBorderStyle = FormBorderStyle.None;
		 			this.Left = 0;
		 			this.Top = 0;
		 			this.Width = screen_width;
		 			this.Height = screen_height;

		 			fullscreen = true;
	 			} else {

					width = window_width;
					height = window_height;

		 			this.FormBorderStyle = FormBorderStyle.Sizable;
		 			this.Left = window_left;
		 			this.Top = window_top;
		 			this.Width = window_width;
		 			this.Height = window_height;

		 			fullscreen = false;
	 			}
 			} else if ( e.KeyData == Keys.Escape) {
				this.Close();

 			} else if ( e.KeyData == Keys.V ) {
 				verbose=!verbose;
 			}

 		}

		private void Form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			client.removeTuioListener(this);

			client.disconnect();
			System.Environment.Exit(0);
		}

		public void addTuioObject(TuioObject o) {
			lock(objectList) {
				objectList.Add(o.SessionID,o);
			} if (verbose) Console.WriteLine("add obj "+o.SymbolID+" ("+o.SessionID+") "+o.X+" "+o.Y+" "+o.Angle);
		}

		public void updateTuioObject(TuioObject o) {

		if(o.SymbolID==fireID)
        { 
		// Convert the angle from radians to degrees
		double angleInDegrees = o.Angle * (180.0 / Math.PI);

		// Normalize the angle (ensure it's between 0 and 360 degrees)
		if (angleInDegrees < 0) angleInDegrees += 360;

		// Flag to detect if the object is rotated near 90 degrees
		bool rotatedRight = (Math.Abs(angleInDegrees - 90) < 5);

		// Flag to detect if the object is rotated back to 0 degrees
		bool rotatedToZero = (Math.Abs(angleInDegrees - 0) < 5 || Math.Abs(angleInDegrees - 360) < 5);

		// Set the fire flag based on the rotation state
		if (rotatedRight)
		{
			if (o.SymbolID == 0)
			{
				fire = 1;  // Set fire flag to 1 when rotated to the right (90 degrees)
				Console.WriteLine("Fire ON - Object rotated to the right (90 degrees)");
			}
		}
		else if (rotatedToZero)
		{
			if (o.SymbolID == 0)
			{
				fire = 0;  // Set fire flag to 1 when rotated to the right (90 degrees)
				Console.WriteLine("Fire ON - Object rotated to the right (90 degrees)");
			}
		}
		}

	 	if (o.SymbolID == KnifeID) {
              knifeObject = o;
         }
        if (o.SymbolID == ChickenID) {
            chickenObject = o;
        }
     
        if (knifeObject != null && chickenObject != null) {
            checkKnifeSlicing(knifeObject, chickenObject);
        }

		if (verbose) Console.WriteLine("set obj "+o.SymbolID+" "+o.SessionID+" "+o.X+" "+o.Y+" "+o.Angle+" "+o.MotionSpeed+" "+o.RotationSpeed+" "+o.MotionAccel+" "+o.RotationAccel);
		}
	    public void checkKnifeSlicing(TuioObject knife, TuioObject chicken) {
 
          float xrange = 0.05f; 
          float yrange = 0.05f;

        // slice vertically
        if (Math.Abs(knife.X - chicken.X) < xrange) {

            if ( Math.Abs(knife.Y - chicken.Y)<yrange) {
                // Knife has "sliced" the chicken
                Console.WriteLine("Chicken sliced!");
	     			chknflag=1;
                
            }
        }

    }


		public void removeTuioObject(TuioObject o) {
			lock(objectList) {
				objectList.Remove(o.SessionID);
			}
			if (verbose) Console.WriteLine("del obj "+o.SymbolID+" ("+o.SessionID+")");
		}

		public void addTuioCursor(TuioCursor c) {
			lock(cursorList) {
				cursorList.Add(c.SessionID,c);
			}
			if (verbose) Console.WriteLine("add cur "+c.CursorID + " ("+c.SessionID+") "+c.X+" "+c.Y);
		}

		public void updateTuioCursor(TuioCursor c) {
			if (verbose) Console.WriteLine("set cur "+c.CursorID + " ("+c.SessionID+") "+c.X+" "+c.Y+" "+c.MotionSpeed+" "+c.MotionAccel);
		}

		public void removeTuioCursor(TuioCursor c) {
			lock(cursorList) {
				cursorList.Remove(c.SessionID);
			}
			if (verbose) Console.WriteLine("del cur "+c.CursorID + " ("+c.SessionID+")");
 		}

		public void addTuioBlob(TuioBlob b) {
			lock(blobList) {
				blobList.Add(b.SessionID,b);
			}
			if (verbose) Console.WriteLine("add blb "+b.BlobID + " ("+b.SessionID+") "+b.X+" "+b.Y+" "+b.Angle+" "+b.Width+" "+b.Height+" "+b.Area);
		}

		public void updateTuioBlob(TuioBlob b) {
		
			if (verbose) Console.WriteLine("set blb "+b.BlobID + " ("+b.SessionID+") "+b.X+" "+b.Y+" "+b.Angle+" "+b.Width+" "+b.Height+" "+b.Area+" "+b.MotionSpeed+" "+b.RotationSpeed+" "+b.MotionAccel+" "+b.RotationAccel);
		}

		public void removeTuioBlob(TuioBlob b) {
			lock(blobList) {
				blobList.Remove(b.SessionID);
			}
			if (verbose) Console.WriteLine("del blb "+b.BlobID + " ("+b.SessionID+")");
		}

		public void refresh(TuioTime frameTime) {
			Invalidate();
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)//drawscene
		{
			// Getting the graphics object
			Graphics g = pevent.Graphics;
			g.FillRectangle(bgrBrush, new Rectangle(0,0,width,height));
			backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "kitback.jpg");
		    // Draw background image without rotation
			if (File.Exists(backgroundImagePath))
			{
				using (Image bgImage = Image.FromFile(backgroundImagePath))
				{
					g.DrawImage(bgImage, new Rectangle(new Point(0, 0), new Size(width, height)));
				}
			}
			else
			{
				Console.WriteLine($"Background image not found: {backgroundImagePath}");
			}
			// draw the cursor path
			if (cursorList.Count > 0) {
 			 lock(cursorList) {
			 foreach (TuioCursor tcur in cursorList.Values) {
					List<TuioPoint> path = tcur.Path;
					TuioPoint current_point = path[0];

					for (int i = 0; i < path.Count; i++) {
						TuioPoint next_point = path[i];
						g.DrawLine(curPen, current_point.getScreenX(width), current_point.getScreenY(height), next_point.getScreenX(width), next_point.getScreenY(height));
						current_point = next_point;
					}
					g.FillEllipse(curBrush, current_point.getScreenX(width) - height / 100, current_point.getScreenY(height) - height / 100, height / 50, height / 50);
					g.DrawString(tcur.CursorID + "", font, fntBrush, new PointF(tcur.getScreenX(width) - 10, tcur.getScreenY(height) - 10));
				}
			}
		 }

			// draw the objects
			if (objectList.Count > 0) {
 				lock(objectList) {
					foreach (TuioObject tobj in objectList.Values) {
						int ox = tobj.getScreenX(width);
						int oy = tobj.getScreenY(height);
						int size = height / 10;
					switch (tobj.SymbolID)
					{
                        case 0: //fire
                            objectImagePath = Path.Combine(Environment.CurrentDirectory, "firepic.png");
                            break;
                        case 1: //knife
							objectImagePath = Path.Combine(Environment.CurrentDirectory, "knife.jfif");
							break;
						case 2: //chicken
							if(chknflag==0)
                            { 
								objectImagePath = Path.Combine(Environment.CurrentDirectory, "chkn.jfif");
							}
                            else if(chknflag==1)
                            {
								objectImagePath = Path.Combine(Environment.CurrentDirectory, "slicedChkn.jfif");
                            }
							break;
                        case 3: //spoon5ashab

                        case 4: //pasta

                        case 5: //butter

                        case 6: //mozzrella

                        case 7: //oil

                        case 8: //water

						case 9: //salt

						case 10: //pepper

						case 11: //milk

						case 12: //3eenKbera
							isflame = true;
							flameEinKbera.X = 485;
							flameEinKbera.Y = this.Height - 325;
							flameEinKbera.img = new Bitmap("flame1.png");
							break;

						case 13: //3eenSo8ayara
                            isflame = true;
                            flameEinSo8ayara.X = 185;
                            flameEinSo8ayara.Y = this.Height - 305;
                            flameEinSo8ayara.img = new Bitmap("flame1.png");
							break;

                        case 14: //sho3laKbera

						case 15: //sho3laSo8ayara

						case 16: //shoka5ashab

						case 17: //cuttingBoard

						case 18: //pot

						case 19: //tasa

						case 20: //spoonAkl

						case 21: //shokaAkl

						case 22: //plate

                        default:
                            // Use default rectangle for other IDs
                            g.FillRectangle(objBrush, new Rectangle(ox - size / 2, oy - size / 2, size, size));
                            g.DrawString(tobj.SymbolID + "", font, fntBrush, new PointF(ox - 10, oy - 10));
                            continue;
                    }

					try
					{


						// Draw object image with rotation
						if (File.Exists(objectImagePath) )
						{
							using (Image objectImage = Image.FromFile(objectImagePath))
							{
								if (fire == 1 && tobj.SymbolID==fireID)
								{
									// Save the current state of the graphics object
									GraphicsState state = g.Save();

									// Apply transformations for rotation
									g.TranslateTransform(ox, oy);
									g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
									g.TranslateTransform(-ox, -oy);

									// Draw the rotated object
									g.DrawImage(objectImage, new Rectangle(ox - size / 2, oy - size / 2, size, size));

									// Restore the graphics state
									g.Restore(state);
								}
                                else if(tobj.SymbolID != fireID)

                                {
									// Save the current state of the graphics object
									GraphicsState state = g.Save();

									// Apply transformations for rotation
									g.TranslateTransform(ox, oy);
									g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
									g.TranslateTransform(-ox, -oy);

									// Draw the rotated object
									g.DrawImage(objectImage, new Rectangle(ox - size / 2, oy - size / 2, size, size));

									// Restore the graphics state
									g.Restore(state);
								}
							}
						}
						else
						{
							Console.WriteLine($"Object image not found: {objectImagePath}");
							// Fall back to drawing a rectangle
							g.FillRectangle(objBrush, new Rectangle(ox - size / 2, oy - size / 2, size, size));
						}
					}
                    catch { }

						//g.TranslateTransform(ox, oy);
						//g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
						//g.TranslateTransform(-ox, -oy);

						//g.FillRectangle(objBrush, new Rectangle(ox - size / 2, oy - size / 2, size, size));

						//g.TranslateTransform(ox, oy);
						//g.RotateTransform(-1 * (float)(tobj.Angle / Math.PI * 180.0f));
						//g.TranslateTransform(-ox, -oy);

						//g.DrawString(tobj.SymbolID + "", font, fntBrush, new PointF(ox - 10, oy - 10));
					}
				}
			}

			// draw the blobs
			if (blobList.Count > 0) {
				lock(blobList) {
					foreach (TuioBlob tblb in blobList.Values) {
						int bx = tblb.getScreenX(width);
						int by = tblb.getScreenY(height);
						float bw = tblb.Width*width;
						float bh = tblb.Height*height;

						g.TranslateTransform(bx, by);
						g.RotateTransform((float)(tblb.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-bx, -by);

						g.FillEllipse(blbBrush, bx - bw / 2, by - bh / 2, bw, bh);

						g.TranslateTransform(bx, by);
						g.RotateTransform(-1 * (float)(tblb.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-bx, -by);
						
						g.DrawString(tblb.BlobID + "", font, fntBrush, new PointF(bx, by));
					}
				}
			}
		}

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
        something ta2leeb_ma3la2a = new something(new Bitmap("ta2leeb (ma3la2a).png"), width / 2 + 55, height / 2 + 50, 180, 320);
        something ta2leeb_shoka = new something(new Bitmap("ta2leeb (shoka).png"), width / 2 + 55, height / 2 + 50, 180, 320);

        something mafrash = new something(new Bitmap("mafrash.png"), width / 2 - 150, height / 2 - 400, 650, 400);
        something dish = new something(new Bitmap("dish.png"), width / 2 + 165, height / 2 - 300, 200, 200);
        something ma3la2a = new something(new Bitmap("ma3la2a.png"), width / 2 + 170, height / 2 - 300, 170, 170);
        something shoka = new something(new Bitmap("shoka.png"), width / 2 + 190, height / 2 - 300, 170, 170);

        //something shoka = new something(new Bitmap("shoka.png"), width / 2 + 50, height / 2 - 280, 250, 250);
        something tasa = new something(new Bitmap("tasa.png"), width / 2 + 433, height / 2 - 480, 350, 350);
        something hala = new something(new Bitmap("7ala.png"), width / 2 + 485, height / 2 - 250, 250, 250);

        something knife = new something(new Bitmap("knife.png"), width - 200, height / 2 + 140, 150, 180);
        //something water = new something(new Bitmap("water.png"), width / 2 +20, height / 2 - 300, 200, 290);

        something board = new something(new Bitmap("board.png"), width / 2 + 180, height / 2 - 30, 580, 480);


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
		public static void Main(String[] argv) {
	 		int port = 0;
			switch (argv.Length) {
				case 1:
					port = int.Parse(argv[0],null);
					if(port==0) goto default;
					break;
				case 0:
					port = 3333;
					break;
				default:
					Console.WriteLine("usage: mono TuioDemo [port]");
					System.Environment.Exit(0);
					break;
			}
			
			TuioDemo app = new TuioDemo(port);
			Application.Run(app);
		}
	}
