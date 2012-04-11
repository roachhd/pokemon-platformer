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
	public int type; 
	public int sprW, sprH;
	public Rectangle rec;

        public treasure(int p, int t, Game g, Vector2 position)
            : base(g)
        {
            type = t;
            points = p;
            Position = position;
            mode = 1;

		sprW = Sprite.Width / 14;
		sprH = Sprite.Height / 2;
		if (t > 27 || t < 0) {t = 24;}
		rec = new Rectangle(sprW*(t%14), sprH*((int)(t/14)), sprW, sprH);
        }

        protected override void LoadContent()
        {
            Sprite = Game.Content.Load<Texture2D>("food");
            
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
		sprW = Sprite.Width / 14;
		sprH = Sprite.Height / 2;
		if (t > 27 || t < 0) {t = 24;}
		rec = new Rectangle(sprW*(t%14), sprH*((int)(t/14)), sprW, sprH);
        }

        public override void Draw(GameTime gameTime)
        {
            //Set bounding box for collision detection
            //Possibly throw this into update, but then again, stones don't move...


            if (mode == 1)
            {
                boundingBox = new Rectangle((int)Position.X, (int)Position.Y, sprW, sprH);
                Vector2 offset = new Vector2(sprW / 2, sprH / 2);

                spriteBatch.Begin();
                spriteBatch.Draw(Sprite, Position - offset, rec, Color.White);
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