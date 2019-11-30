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
            pb_OriginalLocation = pb_Player.Location;
        }

        #region Variables

        int movementSpeed = 3;

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

        #endregion

        #region Functions

        private void playerBoundsUpdate()
        {
            pb_Bounds_Up = new Rectangle(pb_Player.Left, pb_Player.Top - movementSpeed, pb_Player.Width, movementSpeed);
            pb_Bounds_Down = new Rectangle(pb_Player.Left, pb_Player.Bottom + movementSpeed, pb_Player.Width, movementSpeed);
            pb_Bounds_Left = new Rectangle(pb_Player.Left - movementSpeed, pb_Player.Top, movementSpeed, pb_Player.Height);
            pb_Bounds_Right = new Rectangle(pb_Player.Right + movementSpeed, pb_Player.Top, movementSpeed, pb_Player.Height);

            collisionUp = -1;
            collisionDown = -1;
            collisionLeft = -1;
            collisionRight = -1;
        }

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

            if (up && !down)
                if (pb_Player.Top >= movementSpeed)
                    if (collisionUp == -1)
                        pb_Player.Top -= movementSpeed;
                    else
                        pb_Player.Top -= collisionUp;
                else
                    pb_Player.Top = 0;

            if (down && !up)
                if (pb_Player.Bottom <= this.ClientRectangle.Height - movementSpeed)
                    if (collisionDown == -1)
                        pb_Player.Top += movementSpeed;
                    else
                        pb_Player.Top += collisionDown;
                else
                    pb_Player.Top = this.ClientRectangle.Bottom - pb_Player.Height;


            if (left && !right)
                if (pb_Player.Left >= movementSpeed)
                    if (collisionLeft == -1)
                        pb_Player.Left -= movementSpeed;
                    else
                        pb_Player.Left -= collisionLeft;
                else
                    pb_Player.Left = 0;


            if (right && !left)
                if (pb_Player.Right <= this.ClientRectangle.Width - movementSpeed)
                    if (collisionRight == -1)
                        pb_Player.Left += movementSpeed;
                    else
                        pb_Player.Left += collisionRight;
                else
                    pb_Player.Left = this.ClientRectangle.Right - pb_Player.Width;

            playerBoundsUpdate();
        }
    
        #endregion

        #endregion
    }
}
