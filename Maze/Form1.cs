using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Maze
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            playerBoundsUpdate();
            formOriginalSize = this.Size;

            int i = 0;
            foreach (Panel obj in this.Controls.OfType<Panel>())
                originalPanels[++i] = new Rectangle(obj.Location, obj.Size);

            pb_Rect = new Rectangle(pb_Player.Location, pb_Player.Size);
            winRect = new Rectangle(winLabel.Location, winLabel.Size);

            pb_OriginalLocation = pb_Player.Location;

            xRatio = (float)(this.Width) / (float)(formOriginalSize.Width);
            yRatio = (float)(this.ClientRectangle.Height) / (float)(formOriginalSize.Height);
        }

        #region Variables

        int movementSpeed = 3;

        static Size formOriginalSize;
        float xRatio;
        float yRatio;
        static Rectangle[] originalPanels = new Rectangle[200];
        static Rectangle pb_Rect;
        static Rectangle winRect;
        Point pb_OriginalLocation;

        bool up = false;
        bool down = false;
        bool left = false;
        bool right = false;

        static Rectangle pb_Bounds_Up;
        static Rectangle pb_Bounds_Down;
        static Rectangle pb_Bounds_Left;
        static Rectangle pb_Bounds_Right;

        static int collisionUp;
        static int collisionDown;
        static int collisionLeft;
        static int collisionRight;

        static Panel[] WorldObjects = new Panel[2];

        #endregion

        #region Functions

        private void playerBoundsUpdate()
        {
            pb_Bounds_Up = new Rectangle(pb_Player.Left, pb_Player.Top - movementSpeed, pb_Player.Width, 1);
            pb_Bounds_Down = new Rectangle(pb_Player.Left, pb_Player.Bottom + movementSpeed, pb_Player.Width, 1);
            pb_Bounds_Left = new Rectangle(pb_Player.Left - movementSpeed, pb_Player.Top, 1, pb_Player.Height);
            pb_Bounds_Right = new Rectangle(pb_Player.Right + movementSpeed, pb_Player.Top, 1, pb_Player.Height);

            collisionUp = -1;
            collisionDown = -1;
            collisionLeft = -1;
            collisionRight = -1;
        }

        private void rezisePanel(Panel actual, Rectangle rect)
        {
            float xRatio = (float)(this.Width) / (float)(formOriginalSize.Width);
            float yRatio = (float)(this.ClientRectangle.Height) / (float)(formOriginalSize.Height);

            int newX = (int)(rect.Location.X * xRatio);
            int newY = (int)(rect.Location.Y * yRatio);

            int newWidth = (int)(rect.Size.Width * xRatio);
            int newHeight = (int)(rect.Size.Height * yRatio);

            actual.Location = new Point(newX, newY);
            actual.Size = new Size(newWidth, newHeight);
        }

        #region Rezise
        private void Form1_ResizeEnd(object sender, EventArgs e)
        {

            int i = 1;
            xRatio = (float)(this.Width) / (float)(formOriginalSize.Width);
            yRatio = (float)(this.ClientRectangle.Height) / (float)(formOriginalSize.Height);

            foreach (Panel obj in this.Controls.OfType<Panel>())
            {
                int newX = (int)(originalPanels[i].Location.X * xRatio);
                int newY = (int)(originalPanels[i].Location.Y * yRatio);

                int newWidth = (int)(originalPanels[i].Size.Width * xRatio);
                int newHeight = (int)(originalPanels[i++].Size.Height * yRatio);

                obj.Location = new Point(newX, newY);
                obj.Size = new Size(newWidth, newHeight);
            }


            int pb_newX = (int)(pb_Rect.Location.X * xRatio);
            int pb_newY = (int)(pb_Rect.Location.Y * yRatio);

            int pb_newWidth = (int)(pb_Rect.Size.Width * xRatio);
            int pb_newHeight = (int)(pb_Rect.Size.Height * yRatio);

            winLabel.Location = new Point((int)(winRect.Location.X * xRatio), (int)(winRect.Location.Y * yRatio));
            winLabel.Size = new Size((int)(winRect.Width * xRatio), (int)(winRect.Height * yRatio));


            pb_Player.Location = new Point(pb_newX, pb_newY);
            pb_Player.Size = new Size(pb_newWidth, pb_newHeight);
        }

        #endregion

        #region Movement

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.Up:
                    up = true;
                    break;
                case Keys.S:
                case Keys.Down:
                    down = true;
                    break;
                case Keys.A:
                case Keys.Left:
                    left = true;
                    break;
                case Keys.D:
                case Keys.Right:
                    right = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.Up:
                    up = false;
                    break;
                case Keys.S:
                case Keys.Down:
                    down = false;
                    break;
                case Keys.A:
                case Keys.Left:
                    left = false;
                    break;
                case Keys.D:
                case Keys.Right:
                    right = false;
                    break;
            }
        }

        private void movementUpdate_Tick(object sender, EventArgs e)
        {
            if (pb_Bounds_Up.IntersectsWith(winLabel.Bounds))
            {
                pb_Rect.Location = pb_OriginalLocation;
                pb_Player.Location = pb_OriginalLocation;
                playerBoundsUpdate();
                up = down = left = right = false;
                MessageBox.Show("You won!", "Congratulations!");
            }
            foreach (var pan in this.Controls.OfType<Panel>())
            {
                if (pb_Bounds_Up.IntersectsWith(pan.Bounds))
                    collisionUp = pb_Player.Top - pan.Bottom;

                if (pb_Bounds_Down.IntersectsWith(pan.Bounds))
                    collisionDown = pan.Top - pb_Player.Bottom;

                if (pb_Bounds_Left.IntersectsWith(pan.Bounds))
                    collisionLeft = pb_Player.Left - pan.Right;

                if (pb_Bounds_Right.IntersectsWith(pan.Bounds))
                    collisionRight = pan.Left - pb_Player.Right;
            }

            if (up)
                if (pb_Player.Top >= movementSpeed)
                    if (collisionUp == -1)
                        pb_Player.Top -= movementSpeed;
                    else
                        pb_Player.Top -= collisionUp;
                else
                    pb_Player.Top = 0;

            if (down)
                if (pb_Player.Bottom <= this.ClientRectangle.Height - movementSpeed)
                    if (collisionDown == -1)
                        pb_Player.Top += movementSpeed;
                    else
                        pb_Player.Top += collisionDown;
                else
                    pb_Player.Top = this.ClientRectangle.Bottom - pb_Player.Height;


            if (left)
                if (pb_Player.Left >= movementSpeed)
                    if (collisionLeft == -1)
                        pb_Player.Left -= movementSpeed;
                    else
                        pb_Player.Left -= collisionLeft;
                else
                    pb_Player.Left = 0;


            if (right)
                if (pb_Player.Right <= this.ClientRectangle.Width - movementSpeed)
                    if (collisionRight == -1)
                        pb_Player.Left += movementSpeed;
                    else
                        pb_Player.Left += collisionRight;
                else
                    pb_Player.Left = this.ClientRectangle.Right - pb_Player.Width;

            pb_Rect.Location = new Point((int)(pb_Player.Location.X / xRatio),(int)(pb_Player.Location.Y / yRatio));
            playerBoundsUpdate();
        }



        #endregion

        #endregion
    }
}
