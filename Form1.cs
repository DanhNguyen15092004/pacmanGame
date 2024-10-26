using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ConvertImgToBitMap
{
    public partial class Form1 : Form
    {
        private Pacman pacman;
        private Ghost ghost;
        private System.Threading.Timer gameTimer; // Use System.Threading.Timer
        private Map gameMap;

        public Form1()
        {
            gameMap = new Map();
            InitializeComponent();
            this.DoubleBuffered = true;
            InitializeGame();
        }

        private void InitializeGame()
        {
            pacman = new Pacman(new Point(50, 50), new Size(50, 50), @"D:\ConvertImgToBitMap\ConvertImgToBitMap\static\pacman.png", 10);
            ghost = new Ghost(new Point(100, 100), new Size(50, 50), @"D:\ConvertImgToBitMap\ConvertImgToBitMap\static\pinky.png", 10);

            // Initialize the System.Threading.Timer
            gameTimer = new System.Threading.Timer(GameLoop, null, 0, 100); // 100 ms interval

            this.DoubleBuffered = true;
            this.KeyDown += GameForm_KeyDown;
        }

        private void GameLoop(object state)
        {
            if (this.IsHandleCreated)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    Invalidate();

                });
            }
           

            pacman.Update();
            ghost.Update();

            // Use Invoke to call Invalidate on the UI thread
            if (this.IsDisposed || this.Disposing)
            {
                return;
            }
        }

        protected override async void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Ensure RenderMap is asynchronous if needed
            await gameMap.RenderMap(e.Graphics);
            pacman.Draw(e.Graphics);
            ghost.Draw(e.Graphics);
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            Point direction = Point.Empty;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    direction = new Point(0, -1);
                    break;
                case Keys.Down:
                    direction = new Point(0, 1);
                    break;
                case Keys.Left:
                    direction = new Point(-1, 0);
                    break;
                case Keys.Right:
                    direction = new Point(1, 0);
                    break;
            }

            pacman.SetDirection(direction);
            Invalidate(); // Call OnPaint to repaint
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Dispose of the timer on form close
            gameTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
