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

public class TuioDemo : Form, TuioListener
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
    private Dictionary<long, TuioObject> objectList;
    private Dictionary<long, TuioCursor> cursorList;
    private Dictionary<long, TuioBlob> blobList;

    public static int width, height;
    private int window_width = 640;
    private int window_height = 480;
    private int window_left = 0;
    private int window_top = 0;
    private int screen_width = Screen.PrimaryScreen.Bounds.Width;
    private int screen_height = Screen.PrimaryScreen.Bounds.Height;
    public string objectImagePath;
    public string backgroundImagePath;
    public int fire = -1, fire2 = -1, fireID = 0, fireID2 = 12, KnifeID = 1, ChickenID = 2, chknflag = 0, stir = 0, stepFlag = 0;
    public TuioObject knifeObject;
    public TuioObject chickenObject;
    public TuioObject spoonObject;
    public TuioObject foodObject;
    int size, sizeh;

    private bool fullscreen;
    private bool verbose;

    Font font = new Font("Arial", 10.0f);
    SolidBrush fntBrush = new SolidBrush(Color.White);
    SolidBrush bgrBrush = new SolidBrush(Color.FromArgb(0, 0, 64));
    SolidBrush curBrush = new SolidBrush(Color.FromArgb(192, 0, 192));
    SolidBrush objBrush = new SolidBrush(Color.FromArgb(64, 0, 0));
    SolidBrush blbBrush = new SolidBrush(Color.FromArgb(64, 64, 64));
    Pen curPen = new Pen(new SolidBrush(Color.Blue), 1);
    Form messageForm = new Form();
    Label messageLabel = new Label();
    double checkXStart=0.400, checkXEnd=0.450;
    double checkXStart2 = 0.200, checkXEnd2 = 0.250;

    public TuioDemo(int port)
    {

        verbose = false;
        fullscreen = false;
        width = window_width;
        height = window_height;
        this.Load += TuioDemo_Load;
        this.Paint += TuioDemo_Paint;

        //t.Start();
        //t.Tick += T_Tick;
        this.WindowState = FormWindowState.Maximized;
        //this.MouseDown += Form1_MouseDown;
        // this.ClientSize = new System.Drawing.Size(width, height);
        this.Name = "TuioDemo";
        this.Text = "TuioDemo";

        this.Closing += new CancelEventHandler(Form_Closing);
        this.KeyDown += new KeyEventHandler(Form_KeyDown);

        this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                        ControlStyles.UserPaint |
                        ControlStyles.DoubleBuffer, true);

        objectList = new Dictionary<long, TuioObject>(128);
        cursorList = new Dictionary<long, TuioCursor>(128);
        blobList = new Dictionary<long, TuioBlob>(128);

        client = new TuioClient(port);
        client.addTuioListener(this);

        client.connect();
    }
    // Store the current message form and timer
    //private Form messageForm;
    System.Windows.Forms.Timer timer;
    private void ShowMessage(string message)
    {
        // Check if a message form is already open
        if (messageForm != null && messageForm.Visible)
        {
            messageForm.Close(); // Close the existing message form
        }

        // Create a new form and label
        messageForm = new Form();
        messageForm.StartPosition = FormStartPosition.CenterScreen;
        messageForm.Size = new Size(300, 150); // Adjust the size as needed

        Label messageLabel = new Label();
        messageLabel.Text = message;
        messageLabel.Dock = DockStyle.Fill; // Fill the form
        messageLabel.TextAlign = ContentAlignment.MiddleCenter; // Center text

        messageForm.Controls.Add(messageLabel);
        messageForm.Show(); // Show the form

        // Set up a timer to close the form after the specified duration
        timer = new System.Windows.Forms.Timer();
        timer.Interval = 3*1000; // Convert to milliseconds
        timer.Tick += (sender, e) =>
        {
            messageForm.Close(); // Close the form
            timer.Stop(); // Stop the timer
            timer.Dispose(); // Clean up the timer
        };
        timer.Start();
    }
    private void TuioDemo_Paint(object sender, PaintEventArgs e)
    {
        //DrawScene(e.Graphics);
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

    private void Form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
    {

        if (e.KeyData == Keys.F1)
        {
            if (fullscreen == false)
            {

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
            }
            else
            {

                width = window_width;
                height = window_height;

                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.Left = window_left;
                this.Top = window_top;
                this.Width = window_width;
                this.Height = window_height;

                fullscreen = false;
            }
        }
        else if (e.KeyData == Keys.Escape)
        {
            this.Close();

        }
        else if (e.KeyData == Keys.V)
        {
            verbose = !verbose;
        }

    }

    private void Form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        client.removeTuioListener(this);

        client.disconnect();
        System.Environment.Exit(0);
    }

    public void addTuioObject(TuioObject o)
    {
        lock (objectList)
        {
            objectList.Add(o.SessionID, o);
        }
        if (verbose) Console.WriteLine("add obj " + o.SymbolID + " (" + o.SessionID + ") " + o.X + " " + o.Y + " " + o.Angle);
    }

    public void updateTuioObject(TuioObject o)
    {
        if (o.SymbolID == fireID)
        {
            // Convert the angle from radians to degrees
            double angleInDegrees = o.Angle * (180.0 / Math.PI);

            // Normalize the angle (ensure it's between 0 and 360 degrees)
            if (angleInDegrees < 0) angleInDegrees += 360;

            // Flag to detect if the object is rotated near 90 degrees
            bool rotatedRight = (Math.Abs(angleInDegrees - 90) < 5);
            bool rotated22 = (Math.Abs(angleInDegrees - 180) < 5);


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
            if (rotated22)
            {
                if (o.SymbolID == 0)
                {
                    fire = 2;  // Set fire flag to 1 when rotated to the right (90 degrees)
                    Console.WriteLine("Fire ON - Object rotated to the right (90 degrees)");
                    if (stepFlag == 7)
                    {
                        stepFlag = 8;
                        this.Invoke((MethodInvoker)delegate {
                            ShowMessage("Step 8: Put the pan on the stove.");
                        });

                    }
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
        if (o.SymbolID == fireID2)
        {
            // Convert the angle from radians to degrees
            double angleInDegrees = o.Angle * (180.0 / Math.PI);

            // Normalize the angle (ensure it's between 0 and 360 degrees)
            if (angleInDegrees < 0) angleInDegrees += 360;

            // Flag to detect if the object is rotated near 90 degrees
            bool rotatedRight = (Math.Abs(angleInDegrees - 90) < 5);
            bool rotated22 = (Math.Abs(angleInDegrees - 180) < 5);

            // Flag to detect if the object is rotated back to 0 degrees
            bool rotatedToZero = (Math.Abs(angleInDegrees - 0) < 5 || Math.Abs(angleInDegrees - 360) < 5);

            // Set the fire flag based on the rotation state
            if (rotatedRight)
            {
                if (o.SymbolID == 12)
                {
                    fire2 = 1;  // Set fire flag to 1 when rotated to the right (90 degrees)
                    Console.WriteLine("Fire ON - Object rotated to the right (90 degrees)");
                }
            }
            if (rotated22)
            {
                if (o.SymbolID == 12)
                {
                    fire2 = 2;  // Set fire flag to 1 when rotated to the right (90 degrees)
                    if (stepFlag == 1)
                    {
                        stepFlag = 2;
                        this.Invoke((MethodInvoker)delegate {
                            ShowMessage("Step2: Put the pot on the stove.");
                        });

                    }
                    //Console.WriteLine("Fire ON - Object rotated to the right (90 degrees)");
                }
            }
            else if (rotatedToZero)
            {
                if (o.SymbolID == 12)
                {
                    fire2 = 0;  // Set fire flag to 1 when rotated to the right (90 degrees)
                    Console.WriteLine("Fire ON - Object rotated to the right (90 degrees)");
                }
            }
        }
        if (o.SymbolID == 7)//oil
        {
            if (o.X <= checkXEnd2 && o.X >= checkXStart2 && stepFlag == 9)
            {
                stepFlag = 10;
                this.Invoke((MethodInvoker)delegate
                {
                    ShowMessage("Step10: Cut the chicken.");
                });
            }
        }
        if (o.SymbolID == 2 && chknflag==1)//slicedshkn
        {
            if (o.X <= checkXEnd2 && o.X >= checkXStart2 && stepFlag == 11)
            {
                backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "stepflag11.jpg");
                stepFlag = 12;
                this.Invoke((MethodInvoker)delegate
                {
                    ShowMessage("Step12: Put the milk in the pan.");
                });
            }
        }
        if (o.SymbolID == 11)//milk
        {

            if (o.X <= checkXEnd2 && o.X >= checkXStart2 && stepFlag == 12)
            {
                stepFlag = 13;
                this.Invoke((MethodInvoker)delegate
                {
                    ShowMessage("Step13: Put the butter in the pan.");
                });
            }
        }
        if (o.SymbolID == 5)//butter
        {
            if (o.X <= checkXEnd2 && o.X >= checkXStart2 && stepFlag == 13)
            {
                stepFlag = 14;
                this.Invoke((MethodInvoker)delegate
                {
                    ShowMessage("Step14: Put the mozzarila in the pan.");
                });
            }
        }
        if (o.SymbolID == 6)//mozza
        {
            if (o.X <= checkXEnd2&& o.X >= checkXStart2 && stepFlag == 14)
            {
                stepFlag = 15;
                this.Invoke((MethodInvoker)delegate
                {
                    ShowMessage("Step15: Put what is in the pan into the pot.");
                });
            }
        }
        if (o.SymbolID == 13 && stepFlag==15)//panagain
        {
            if (o.X <= checkXEnd && o.X >= checkXStart && stepFlag == 15)
            {
                stepFlag = 16;
                this.Invoke((MethodInvoker)delegate
                {
                    ShowMessage("FINISHED !!!");
                });
            }
        }
        if (o.SymbolID == 13)//pan
        {
            if (o.X <= checkXEnd2 && o.X >= checkXStart2 && stepFlag == 8)
            {
                stepFlag = 9;
                this.Invoke((MethodInvoker)delegate
                {
                    ShowMessage("Step9: Put the oil in the pan.");
                });
            }
        }
        if (o.SymbolID == 10)//pepper
        {
            if (o.X <= checkXEnd && o.X >= checkXStart && stepFlag == 6)
            {
                stepFlag = 7;
                this.Invoke((MethodInvoker)delegate
                {
                    ShowMessage("Step7: Open the stove with the highest flame.");
                });
            }
        }
        if (o.SymbolID == 9)//salt
        {
            if (o.X <= checkXEnd && o.X >= checkXStart && stepFlag == 5)
            {
                stepFlag = 6;
                this.Invoke((MethodInvoker)delegate
                {
                    ShowMessage("Step6: put the pepper.");
                });
            }
        }
        if (o.SymbolID == 15)//pasta
        {
            if (o.X <= checkXEnd && o.X >= checkXStart && stepFlag == 4)
            {
                stepFlag = 5;
                this.Invoke((MethodInvoker)delegate
                {
                    ShowMessage("Step5: put the salt.");
                });
            }
        }
        if (o.SymbolID == 8)//water
        {
            if (o.X <= checkXEnd && o.X >= checkXStart && stepFlag == 3)
            {
                stepFlag = 4;
                this.Invoke((MethodInvoker)delegate
                {
                    ShowMessage("Step4: put the pasta.");
                });
            }
        }
        if (o.SymbolID == 14)//pot
        {
            if (o.X <= checkXEnd && o.X >= checkXStart && stepFlag == 2)
            {
                stepFlag = 3;
                this.Invoke((MethodInvoker)delegate
                {
                    ShowMessage("step3: put the water inside the pot.");
                });
            }
        }
        if (o.SymbolID == KnifeID)
        {
            knifeObject = o;
        }
        if (o.SymbolID == ChickenID)
        {
            chickenObject = o;
        }
        if (knifeObject != null && chickenObject != null)
        {
            checkKnifeSlicing(knifeObject, chickenObject);
        }
        if (spoonObject != null && foodObject != null)
        {
            float xx = 0.5f;
            float yy = 0.5f;

            if (Math.Abs(spoonObject.X - foodObject.X) < xx)
            {
                if (Math.Abs(spoonObject.Y - foodObject.Y) < yy)
                {
                    double angleInDegrees = o.Angle * (180.0 / Math.PI);

                    if (angleInDegrees < 0) angleInDegrees += 360;


                    bool rotated1 = (Math.Abs(angleInDegrees - 90) < 5);
                    bool rotated2 = (Math.Abs(angleInDegrees - 180) < 5);

                    if (rotated1)
                    {
                        if (o.SymbolID == 3)
                        {
                            stir = 1;
                        }
                    }
                    if (rotated2)
                    {
                        if (o.SymbolID == 3)
                        {
                            stir = 2;
                        }
                    }
                }
            }
        }

        if (verbose) Console.WriteLine("set obj " + o.SymbolID + " " + o.SessionID + " " + o.X + " " + o.Y + " " + o.Angle + " " + o.MotionSpeed + " " + o.RotationSpeed + " " + o.MotionAccel + " " + o.RotationAccel);
    }
    public void checkKnifeSlicing(TuioObject knife, TuioObject chicken)
    {
        float xrange = 0.05f;
        float yrange = 0.05f;

        // slice vertically
        if (Math.Abs(knife.X - chicken.X) < xrange)
        {
            if (Math.Abs(knife.Y - chicken.Y) < yrange)
            {
                stepFlag = 11;
                // Knife has "sliced" the chicken
                Console.WriteLine("Chicken sliced!");
                chknflag = 1;
                this.Invoke((MethodInvoker)delegate
                {
                    ShowMessage("Step11: Put the chicken in the pan.");
                });
            }
        }
    }

    public void removeTuioObject(TuioObject o)
    {
        lock (objectList)
        {
            objectList.Remove(o.SessionID);
        }
        if (verbose) Console.WriteLine("del obj " + o.SymbolID + " (" + o.SessionID + ")");
    }

    public void addTuioCursor(TuioCursor c)
    {
        lock (cursorList)
        {
            cursorList.Add(c.SessionID, c);
        }
        if (verbose) Console.WriteLine("add cur " + c.CursorID + " (" + c.SessionID + ") " + c.X + " " + c.Y);
    }

    public void updateTuioCursor(TuioCursor c)
    {
        if (verbose) Console.WriteLine("set cur " + c.CursorID + " (" + c.SessionID + ") " + c.X + " " + c.Y + " " + c.MotionSpeed + " " + c.MotionAccel);
    }

    public void removeTuioCursor(TuioCursor c)
    {
        lock (cursorList)
        {
            cursorList.Remove(c.SessionID);
        }
        if (verbose) Console.WriteLine("del cur " + c.CursorID + " (" + c.SessionID + ")");
    }

    public void addTuioBlob(TuioBlob b)
    {
        lock (blobList)
        {
            blobList.Add(b.SessionID, b);
        }
        if (verbose) Console.WriteLine("add blb " + b.BlobID + " (" + b.SessionID + ") " + b.X + " " + b.Y + " " + b.Angle + " " + b.Width + " " + b.Height + " " + b.Area);
    }

    public void updateTuioBlob(TuioBlob b)
    {

        if (verbose) Console.WriteLine("set blb " + b.BlobID + " (" + b.SessionID + ") " + b.X + " " + b.Y + " " + b.Angle + " " + b.Width + " " + b.Height + " " + b.Area + " " + b.MotionSpeed + " " + b.RotationSpeed + " " + b.MotionAccel + " " + b.RotationAccel);
    }

    public void removeTuioBlob(TuioBlob b)
    {
        lock (blobList)
        {
            blobList.Remove(b.SessionID);
        }
        if (verbose) Console.WriteLine("del blb " + b.BlobID + " (" + b.SessionID + ")");
    }

    public void refresh(TuioTime frameTime)
    {
        Invalidate();
    }
    private bool isTasaDrawn = false;

    protected override void OnPaintBackground(PaintEventArgs pevent)//drawscene
    {
        // Getting the graphics object
        Graphics g = pevent.Graphics;
        g.FillRectangle(bgrBrush, new Rectangle(0, 0, this.Width, this.Height));

        //backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "BG.jpg");
        

        // draw the cursor path
        if (cursorList.Count > 0)
        {
            lock (cursorList)
            {
                foreach (TuioCursor tcur in cursorList.Values)
                {
                    List<TuioPoint> path = tcur.Path;
                    TuioPoint current_point = path[0];

                    for (int i = 0; i < path.Count; i++)
                    {
                        TuioPoint next_point = path[i];
                        g.DrawLine(curPen, current_point.getScreenX(this.Width), current_point.getScreenY(height), next_point.getScreenX(this.Width), next_point.getScreenY(height));
                        current_point = next_point;
                    }
                    g.FillEllipse(curBrush, current_point.getScreenX(this.Width) - height / 100, current_point.getScreenY(height) - height / 100, height / 50, height / 50);
                    g.DrawString(tcur.CursorID + "", font, fntBrush, new PointF(tcur.getScreenX(this.Width) - 10, tcur.getScreenY(height) - 10));
                }
            }
        }
        if (stepFlag == 0)
        {
            backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "BG.jpg");
            stepFlag = 1;
            ShowMessage("Step1: Open the stove with the highest flame.");
        }
        if (stepFlag == 16)
        {
            backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "stepflag16.jpg");

        }
        if (File.Exists(backgroundImagePath))
        {
            using (Image bgImage = Image.FromFile(backgroundImagePath))
            {
                g.DrawImage(bgImage, new Rectangle(new Point(0, 0), new Size(this.Width, this.Height)));
            }

        }
        else
        {
            Console.WriteLine($"Background image not found: {backgroundImagePath}");
        }
        // draw the objects
        if (objectList.Count > 0)
        {
            lock (objectList)
            {
                foreach (TuioObject tobj in objectList.Values)
                {
                    int ox = tobj.getScreenX(this.Width);
                    int oy = tobj.getScreenY(this.Height);

                    switch (tobj.SymbolID)
                    {
                        case 0: //3ensoghyra
                            if (stepFlag == 7)
                            {
                                backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "stepflag6.jpg");
                                objectImagePath = Path.Combine(Environment.CurrentDirectory, "3eenElbotagaz0.png");
                                size = 150;
                                sizeh = 250;
                            }
                            break;
                        case 1: //knife
                            if (stepFlag==10)
                            {
                                backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "stepflag10.jpg");
                                objectImagePath = Path.Combine(Environment.CurrentDirectory, "knife.png");
                                size = 150;
                                sizeh = 250;
                            }
                            break;
                        case 2: //chicken
                            if (chknflag == 0 && stepFlag ==10)
                            {
                                backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "stepflag10.jpg");
                                objectImagePath = Path.Combine(Environment.CurrentDirectory, "chicken breast.png");
                            }
                            else if (chknflag == 1)
                            {
                                objectImagePath = Path.Combine(Environment.CurrentDirectory, "slicedChkn.png");
                            }
                            size = 280;
                            sizeh = 250;
                            break;
                        case 3: //spoon
                            objectImagePath = Path.Combine(Environment.CurrentDirectory, "ta2leeb (ma3la2a)2.png");
                            size = 280;
                            sizeh = 250;
                            break;
                        case 4: // food
                            if (stir == 0)
                            {
                                objectImagePath = Path.Combine(Environment.CurrentDirectory, "1.png");
                            }
                            else if (stir == 1)
                            {
                                objectImagePath = Path.Combine(Environment.CurrentDirectory, "2.png");
                            }
                            else if (stir == 2)
                            {
                                objectImagePath = Path.Combine(Environment.CurrentDirectory, "3.png");
                            }
                            break;
                        case 5: //butter
                            if (stepFlag == 13)
                            {
                                backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "stepflag13.jpg");
                                objectImagePath = Path.Combine(Environment.CurrentDirectory, "butterbs.png");
                                size = 280;
                                sizeh = 250;
                            }
                            break;
                        case 6: //mozzrella
                            if (stepFlag == 14)
                            {
                                backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "stepflag14.jpg");
                                objectImagePath = Path.Combine(Environment.CurrentDirectory, "gebna.png");
                                size = 220;
                                sizeh = 200;
                            }
                            break;
                        case 7: //oil
                            if (stepFlag == 9)
                            {
                                backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "stepflag9.jpg");
                                objectImagePath = Path.Combine(Environment.CurrentDirectory, "oil.png");
                                size = 280;
                                sizeh = 250;
                            }
                            break;
                        case 8: //water
                            if (stepFlag == 3)
                            {
                                backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "stepflag3.jpg");
                                objectImagePath = Path.Combine(Environment.CurrentDirectory, "water.png");
                                size = 280;
                                sizeh = 250;
                            }
                            break;
                        case 9: //salt
                            if (stepFlag == 5)
                            {
                                backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "stepflag5.jpg");
                                objectImagePath = Path.Combine(Environment.CurrentDirectory, "salt.png");
                                size = 280;
                                sizeh = 250;
                            }
                            break;
                        case 10: //pepper
                            if (stepFlag == 6)
                            {
                                backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "stepflag6.jpg");
                                objectImagePath = Path.Combine(Environment.CurrentDirectory, "pepper.png");
                                size = 120;
                                sizeh = 235;
                            }
                            break;
                        case 11: //milk
                            if (stepFlag == 12)
                            {
                                backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "stepflag12.jpg");
                                objectImagePath = Path.Combine(Environment.CurrentDirectory, "milk.png");
                                size = 350;
                                sizeh = 550;
                            }
                            break;
                        case 12: //3eenKbera
                            if (stepFlag == 1)
                            {
                                backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "BG.jpg");
                                objectImagePath = Path.Combine(Environment.CurrentDirectory, "3eenElbotagaz0.png");
                                size = 150;
                                sizeh = 250;
                            }
                            break;
                        case 13: //tasa
                            if (stepFlag == 8 || stepFlag == 15)
                            {
                                if(stepFlag == 8)
                                {
                                    backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "stepflag8.jpg");
                                }
                                if (stepFlag == 15)
                                {
                                    backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "stepflag15.jpg");
                                }
                                objectImagePath = Path.Combine(Environment.CurrentDirectory, "tasa.png");
                                size = 280;
                                sizeh = 250;
                            }
                            break;
                            
                        case 14: //7ala
                            if (stepFlag == 2)
                            {
                                backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "stepflag2.jpg");
                                objectImagePath = Path.Combine(Environment.CurrentDirectory, "7ala.png");
                                size = 300;
                                sizeh = 270;
                            }
                            break;
                        case 15: //pasta
                            if (stepFlag == 4)
                            {
                                backgroundImagePath = Path.Combine(Environment.CurrentDirectory, "stepflag4.jpg");
                                objectImagePath = Path.Combine(Environment.CurrentDirectory, "pasta.png");
                                size = 300;
                                sizeh = 270;
                            }
                            break;
                        default:
                            // Use default rectangle for other IDs
                            g.FillRectangle(objBrush, new Rectangle(ox - size / 2, oy - size / 2, size, size));
                            g.DrawString(tobj.SymbolID + "", font, fntBrush, new PointF(ox - 10, oy - 10));
                            continue;
                    }
                    try
                    {
                        
                        // Draw object image with rotation
                        if (File.Exists(objectImagePath))
                        {
                            using (Image objectImage = Image.FromFile(objectImagePath))
                            {
                                if (fire == 1 && tobj.SymbolID == fireID)
                                {
                                    // Save the current state of the graphics object
                                    GraphicsState state = g.Save();
                                    flameEinSo8ayara.X = 230;
                                    flameEinSo8ayara.Y = this.Height - 370;
                                    flameEinSo8ayara.img = new Bitmap("flame1.png");
                                    g.DrawImage(flameEinSo8ayara.img, flameEinSo8ayara.X, flameEinSo8ayara.Y, 210, 190);
                                }
                                else if (fire == 2 && tobj.SymbolID == fireID)
                                {
                                    GraphicsState state = g.Save();
                                    flameEinSo8ayara.X = 230;
                                    flameEinSo8ayara.Y = this.Height - 355;
                                    flameEinSo8ayara.img = new Bitmap("flame2.png");
                                    g.DrawImage(flameEinSo8ayara.img, flameEinSo8ayara.X, flameEinSo8ayara.Y, 200, 160);
                                }
                                else if (fire2 == 1 && tobj.SymbolID == fireID2)
                                {
                                    GraphicsState state = g.Save();
                                    flameEinSo8ayara.X = 600;
                                    flameEinSo8ayara.Y = this.Height - 390;
                                    flameEinSo8ayara.img = new Bitmap("flame1.png");
                                    g.DrawImage(flameEinSo8ayara.img, flameEinSo8ayara.X, flameEinSo8ayara.Y, 260, 230);
                                }
                                else if (fire2 == 2 && tobj.SymbolID == fireID2)
                                {
                                    GraphicsState state = g.Save();
                                    flameEinSo8ayara.X = 600;
                                    flameEinSo8ayara.Y = this.Height - 385;
                                    flameEinSo8ayara.img = new Bitmap("flame2.png");
                                    g.DrawImage(flameEinSo8ayara.img, flameEinSo8ayara.X, flameEinSo8ayara.Y, 250, 220);
                                }
                                else if (tobj.SymbolID != fireID && tobj.SymbolID != fireID2)
                                {
                                    // Save the current state of the graphics object
                                    GraphicsState state = g.Save();

                                    // Apply transformations for rotation
                                    g.TranslateTransform(ox, oy);
                                    g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
                                    g.TranslateTransform(-ox, -oy);

                                    // Draw the rotated object
                                    g.DrawImage(objectImage, new Rectangle(ox - size / 2, oy - size / 2, size, sizeh));

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
        if (blobList.Count > 0)
        {
            lock (blobList)
            {
                foreach (TuioBlob tblb in blobList.Values)
                {
                    int bx = tblb.getScreenX(this.Width);
                    int by = tblb.getScreenY(this.Height);
                    float bw = tblb.Width * width;
                    float bh = tblb.Height * height;

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
    public static void Main(String[] argv)
    {
        int port = 0;
        switch (argv.Length)
        {
            case 1:
                port = int.Parse(argv[0], null);
                if (port == 0) goto default;
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