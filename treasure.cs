using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Pokemon
{
    class treasure : DrawableGameComponent
    {
        public int points { get; set; } //1 collectable, 2 current, 3 in inventory, 4 far back in inventory
        public int mode { get; set; }
        public Vector2 Position { get; set; }
        Texture2D Sprite;
        SpriteBatch spriteBatch;
        public Rectangle boundingBox { get; set; }

        public treasure(int p, Game g, Vector2 position)
            : base(g)
        {
            points = p;
            Position = position;
            mode = 1;
        }

        protected override void LoadContent()
        {
            Sprite = Game.Content.Load<Texture2D>("chest");
            
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void Draw(GameTime gameTime)
        {
            //Set bounding box for collision detection
            //Possibly throw this into update, but then again, stones don't move...


            if (mode == 1)
            {
                boundingBox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
                Vector2 offset = new Vector2(Sprite.Width / 2, Sprite.Height / 2);

                spriteBatch.Begin();
                spriteBatch.Draw(Sprite, Position - offset, Color.White);
                spriteBatch.End();
            }
            else
            {
                boundingBox = new Rectangle(0, 0, 0, 0);
            }

            base.Draw(gameTime);
        }



       


    }
    
}
