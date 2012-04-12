using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace pokemon
{
    class Attack : DrawableGameComponent
    {
        /**
         * 1 - red
         * 2 - blue
         * 3 - yellow
         */
        int Type; int Dir;
        Vector2 Position;

        Texture2D Sprite;
        SpriteBatch spriteBatch;
        int sprW=33, sprH=33;
        public Rectangle boundingBox;
        Rectangle spr;

        public Attack(Game g, Vector2 position, int type, int dir)
            : base(g)
        {
            Type = type;
            Dir = dir;
            if (Dir < 0) Position = position + new Vector2(-10, 40);
            else Position = position + new Vector2(65, 40);


        }

        protected override void LoadContent()
        {
            Sprite = Game.Content.Load<Texture2D>("attack");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            if (Type == 1)
            {
                spr = new Rectangle(0,0,33,33);

            }
            else if (Type == 2)
            {
                spr = new Rectangle(33, 0, 33, 33);
            }
            else
            {
                spr = new Rectangle(66, 0, 33, 33);
            }

            Vector2 offset = new Vector2(sprW / 2, sprH / 2); //is this ok??? or first width then height
            Position = Position - offset;

            
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
             boundingBox=new Rectangle((int)Position.X, (int)Position.Y, sprW, sprH);
            //Console.WriteLine("dir: " + Dir);
            if (Dir < 0)
            { //left
                Position -= new Vector2(elapsed, 0) * 100;
            }
            else
            {
                Position += new Vector2(elapsed, 0) * 100;
            }
            base.Update(gameTime);
        }

        

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin(); 
            spriteBatch.Draw(Sprite, Position, spr, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();

            base.Draw(gameTime);
        }





    }
}
