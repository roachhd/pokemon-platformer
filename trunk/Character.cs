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
        protected Rectangle boundingBox;
        protected int step = 1;
        protected int walked = 0, climbed = 0;
        protected int counter = 0;
        protected int ladder = 1;
        protected int jumper = 0;
        protected int percent = 400;
        protected int got;


        public Vector2 Position { get; set; }
        protected Texture2D Sprite;
        protected SpriteBatch spriteBatch;

        protected Color typeCol = Color.White;


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
            if (step == 1) rec = new Rectangle(0, 0, sprwid, sprhei);
            else if (step == 2) rec = new Rectangle(sprwid, 0, sprwid, sprhei);
            else if (step == 3) rec = new Rectangle(sprwid*2, 0, sprwid, sprhei);
            else if (step == 4) rec = new Rectangle(sprwid, 0, sprwid, sprhei); //second again
            else if (step == -1) rec = new Rectangle(0, sprhei, sprwid, sprhei);
            else if (step == -2) rec = new Rectangle(sprwid, sprhei, sprwid, sprhei);
            else if (step == -3) rec = new Rectangle(sprwid * 2, sprhei, sprwid, sprhei);
            else if (step == -4) rec = new Rectangle(sprwid, sprhei, sprwid, sprhei);
            //climbing steps
            else if (step == 6) rec = new Rectangle(0, sprhei * 2, sprwid, sprhei);
            else if (step == 7) rec = new Rectangle(sprwid, sprhei * 2, sprwid, sprhei);
            else if (step == 8) rec = new Rectangle(sprwid * 2, sprhei * 2, sprwid, sprhei);
            else rec = new Rectangle(sprwid, sprhei * 2, sprwid, sprhei);

            //death sprite
            if (step == 10) rec = new Rectangle(sprwid*2, sprhei * 2, sprwid, sprhei);

            return rec;
        }

        protected int jumping;

        int sprwid = 50, sprhei = 50;

        protected override void LoadContent()
        {

            Sprite = Game.Content.Load<Texture2D>("mage");
            sprwid = 50; sprhei = 50;
            Vector2 offset = new Vector2(sprwid / 2, sprhei / 2);
            Position = Position - offset;


            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }



    }




}
