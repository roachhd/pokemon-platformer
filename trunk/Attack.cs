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

        public Attack(Game g,Vector2 position, int type, int dir) : base(g){
            Type = type;
            Dir = dir;
            if (Dir < 0) Position = position + new Vector2(-10, 40);
            else Position = position + new Vector2(65, 40);
            

        }

        protected override void LoadContent()
        {
            if (Type == 1)
            {
                Sprite = Game.Content.Load<Texture2D>("red");
                spriteBatch = new SpriteBatch(Game.GraphicsDevice);
                
            }
            else if (Type == 2)
            {
                Sprite = Game.Content.Load<Texture2D>("blue");
                spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            }
            else {
                Sprite = Game.Content.Load<Texture2D>("yellow");
                spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            }

            Vector2 offset = new Vector2(Sprite.Height / 2, Sprite.Width / 2); //is this ok??? or first width then height
            Position = Position - offset;
            
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Console.WriteLine("dir: " + Dir);
            if (Dir < 0)
            { //left
                Position -= new Vector2(elapsed, 0) * 100;
            }
            else {
                Position += new Vector2(elapsed, 0) * 100;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            
            spriteBatch.Begin();
            spriteBatch.Draw(Sprite, Position, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }





    }
}
