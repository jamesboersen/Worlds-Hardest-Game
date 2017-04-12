using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoxField
{
    public partial class GameScreen : UserControl
    {
        //player1 button control keys - DO NOT CHANGE
        Boolean leftArrowDown, downArrowDown, rightArrowDown, upArrowDown, bDown, nDown, mDown, spaceDown;

        //used to draw boxes on screen
        SolidBrush boxBrush = new SolidBrush(Color.White);
        //draw character brush
        SolidBrush circleBrush = new SolidBrush(Color.Red);
        //victory box draw brush
        SolidBrush victoryBrush = new SolidBrush(Color.Yellow);
        //outline pen
        Pen outlinePen = new Pen(Color.Black, 20);

        //create a list to hold a column of boxes        
        List<Box> boxes = new List<Box>();

        //box values
        int boxSize, boxSpeed;

        //hero character
        Box hero;
        int heroSpeed;

        //victory box
        Box victory;

        public GameScreen()
        {
            InitializeComponent();
            OnStart();
        }

        /// <summary>
        /// Set initial game values here
        /// </summary>
        public void OnStart()
        {
            //set game start values
            boxSize = 30;
            boxSpeed = 5;
            //list of boxes that move
            Box b1 = new Box(375, -0, boxSize, boxSpeed);
            boxes.Add(b1);

            Box b2 = new Box(525, this.Height - 30, boxSize, boxSpeed);
            boxes.Add(b2);

            Box b3 = new Box(675, -0, boxSize, boxSpeed);
            boxes.Add(b3);

            Box b4 = new Box(225, this.Height - 30, boxSize, boxSpeed);
            boxes.Add(b4);

            Box b5 = new Box(300, 0, boxSize - 10, boxSpeed - 2);
            boxes.Add(b5);

            Box b6 = new Box(450, this.Height - 30, boxSize - 10, boxSpeed - 2);
            boxes.Add(b6);

            Box b7 = new Box(600, 0, boxSize - 10, boxSpeed - 2);
            boxes.Add(b7);

            //set hero values at start of game
            heroSpeed = 4;
            hero = new Box(100, this.Height/2, boxSize, heroSpeed);
            
            //victory box value
            victory = new Box(800, this.Height / 2, boxSize, 0);
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player 1 button presses
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.B:
                    bDown = true;
                    break;
                case Keys.N:
                    nDown = true;
                    break;
                case Keys.M:
                    mDown = true;
                    break;
                case Keys.Space:
                    spaceDown = true;
                    break;
                default:
                    break;
            }
        }

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //player 1 button releases
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.B:
                    bDown = false;
                    break;
                case Keys.N:
                    nDown = false;
                    break;
                case Keys.M:
                    mDown = false;
                    break;
                case Keys.Space:
                    spaceDown = false;
                    break;
                default:
                    break;
            }
        }

        private void gameLoop_Tick(object sender, EventArgs e)
        {
            //update location of all boxes (drop down screen)
            foreach (Box b in boxes)
            {
                b.Move();
                if (b.y > this.Height - b.size)
                {
                    b.speed = -b.speed;
                }
                if (b.y < 0)
                {
                    b.speed = -b.speed;
                }
            }


            //move our hero
            if (leftArrowDown == true && hero.x > 10)
            {
                hero.Move("left");
            }
            if (rightArrowDown == true && hero.x < this.Width -boxSize - 10)
            {
                hero.Move("right");
            }
            if (downArrowDown == true && hero.y < this.Height - boxSize - 10)
            {
                hero.Move("down");
            }
            if (upArrowDown == true && hero.y > 10)
            {
                hero.Move("up");
            }

            //check for collision between hero and boxes
            foreach (Box b in boxes)
            {
                Boolean hasCollided = false;

                hasCollided = hero.Collision(b);

                if (hasCollided == true)
                {
                    gameLoop.Stop();

                    Application.Restart();
                }       
            }
            //check if hero collided with victory box

            Boolean victoryCollided = false;

            victoryCollided = hero.victoryCollision(victory);

            if (victoryCollided == true)
            {
                gameLoop.Enabled = false;

                Form f = this.FindForm();
                f.Controls.Remove(this);

                VictoryScreen vs = new VictoryScreen();
                f.Controls.Add(vs);
            }

            Refresh();
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //draw boxes to screen
            foreach (Box b in boxes)
            {
                boxBrush.Color = Color.FromArgb(0, 0, 255);
                e.Graphics.FillRectangle(boxBrush, b.x, b.y, b.size, b.size);
            }
            //draw the character
            e.Graphics.FillEllipse(circleBrush, hero.x, hero.y, hero.size, hero.size);
            //draw the victory box
            e.Graphics.FillRectangle(victoryBrush, 800, this.Height / 2, boxSize, boxSize);
            //draw the outline
            e.Graphics.DrawRectangle(outlinePen, 0, 0, this.Width - 2, this.Height - 2);
        }
    }
}
