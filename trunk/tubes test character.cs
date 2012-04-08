using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pokemon
{
    class Character : DrawableGameComponent
    {
        protected int step = 1;
        protected int walked = 0, climbed = 0;
        protected int counter = 0;
        protected int ladder = 1;
        protected int jumper = 0;
        protected int percent = 400;
        protected int got = 0;


        public Vector2 Position { get; set; }
        protected Texture2D Sprite;
        protected SpriteBatch spriteBatch;

        protected Color typeCol = Color.White;
        public Rectangle boundingBox
        {
            get
            {
                return new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                64,
                64);
            }
        }


        public Character(Game g, Vector2 position) : base(g) { Position = position; }

        protected bool collisionWithFloor() { return false; }

        protected void jump(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position -= new Vector2(0, elapsed) * percent;
            percent -= 10;

            if (collisionWithFloor())
            {
                jumping = 0;
                percent = 400;
            }

        }

        /*
         * Steps:
         * 1 to 4: walking right
         * -1 to -4: walking left
         * 6 to 9: climbing up
         * -6 to -9: climbing down
         */

        protected Rectangle walk(GameTime gameTime)
        {

            float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Rectangle rec;
            counter++;

            if (counter > 5)
            {
                counter = 0;
                if (walked == 1)
                {
                    walked = 0;
                    if (step == 4) { step = 1; }
                    else { step++; }
                }
                else if (walked == -1)
                {
                    walked = 0;
                    if (step == -4) { step = -1; }
                    else { step--; }
                }
                if (climbed == 1)
                {
                    climbed = 0;
                    if (step == 9) { step = 6; }
                    else { step++; }
                }
            }

            //walking steps
            if (step == 1) rec = new Rectangle(0, 0, 64, 64);
            else if (step == 2) rec = new Rectangle(64, 0, 64, 64);
            else if (step == 3) rec = new Rectangle(128, 0, 64, 64);
            else if (step == 4) rec = new Rectangle(64, 0, 64, 64);
            else if (step == -1) rec = new Rectangle(0, 64, 64, 64);
            else if (step == -2) rec = new Rectangle(64, 64, 64, 64);
            else if (step == -3) rec = new Rectangle(128, 64, 64, 64);
            else if (step == -4) rec = new Rectangle(64, 64, 64, 64);
            //climbing steps
            else if (step == 6) rec = new Rectangle(0, 128, 64, 64);
            else if (step == 7) rec = new Rectangle(64, 128, 64, 64);
            else if (step == 8) rec = new Rectangle(128, 128, 64, 64);
            else rec = new Rectangle(64, 128, 64, 64);

            return rec;
        }

        protected int jumping;

        protected override void LoadContent()
        {

            Sprite = Game.Content.Load<Texture2D>("mage");

            Vector2 offset = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
            Position = Position - offset;


            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }


        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.Up))
            {
                if (ladder > 0) // if he's going up a ladder
                {
                    if ((step >= 0 && step <= 4) || (step < 0)) //if he had a walking sprite
                        step = 6;
                    Position -= new Vector2(0, elapsed) * 80;
                    climbed = 1;
                }
                else
                { //if he's jumping
                    jumping = 1;
                }
            }

            if (k.IsKeyDown(Keys.Down))
            {
                if (ladder > 0) // if he's going down a ladder
                {
                    if ((step >= 0 && step <= 4) || (step < 0)) //if he had a walking sprite
                        step = 6;
                    Position += new Vector2(0, elapsed) * 80;
                    climbed = 1;
                }

            }

            if (k.IsKeyDown(Keys.Left))
            {
                Position -= new Vector2(elapsed, 0) * 100;
                walked = -1;
                if (step > 0) //if he was walking right or was climbing
                {
                    step = -1;
                }

            }
            if (k.IsKeyDown(Keys.Right))
            {
                Position += new Vector2(elapsed, 0) * 100;
                walked = 1;
                if ((step < 0) || (step > 6)) //if he was walking left or was climbing
                {
                    step = 1;
                }

            }

            if (jumping == 1) { jump(gameTime); }



            //Collision Detection (for the ground)
            foreach (var c in Game.Components)
            {
                Ground s = c as Ground;

                if (s != null)
                {
                    /*if (boundingBox.Intersects(s.bb) && (boundingBox.Left < s.bb.Right))
                    {
                        Position = new Vector2(s.bb.Right, Position.Y);
                    }*/
                        
                    /*if (boundingBox.Intersects(s.bb) && (boundingBox.Right > s.bb.Left)) //you're on the left
                        {
                            Position = new Vector2(s.bb.Left - boundingBox.Width, Position.Y);
                        }*/
                        if (boundingBox.Intersects(s.bb) && (boundingBox.Bottom > s.bb.Top)) //top
                        {
                            Position = new Vector2(Position.X, s.bb.Top - boundingBox.Height);
                        }
                        /*if (boundingBox.Intersects(s.bb) && (boundingBox.Top < s.bb.Bottom)) //bottom
                        {
                            Position = new Vector2(Position.X, s.bb.Bottom - s.bb.Height/2);

                        }*/
                           
                    }
                }
            }



        }



    }




