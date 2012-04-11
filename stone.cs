using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace pokemon
{
    class stone: DrawableGameComponent
    {
        public int mode { get; set; } //1 collectable, 2 current, 3 in inventory, 4 far back in inventory
        public int type { get; set; } //1 red, 2 blue, 3 yellow
        public Vector2 Position { get; set; }
        Texture2D Sprite;
        SpriteBatch spriteBatch;
        public Rectangle boundingBox { get; set; } 
	int sprW, sprH;
	Rectangle rec;

        public stone(int t, int m, Game g, Vector2 position):base(g)
        {
            type = t;
            mode = m;
            Position = position;
        }

        protected override void LoadContent()
        {
		Sprite = Game.Content.Load<Texture2D>("cannons");
		sprW = Sprite.Width / 3;
		sprH = Sprite.Height;
            switch (type)
            {
                case 1: //Fire
			rec = new Rectangle(sprW*0, sprH*0, sprW, sprH);
                    break;

                case 2: //Water
			rec = new Rectangle(sprW*1, sprH*0, sprW, sprH);
                    break;

                case 3: //Lightning
                    rec = new Rectangle(sprW*2, sprH*0, sprW, sprH);
                    break;
            }
            
            

            spriteBatch = new SpriteBatch(Game.GraphicsDevice); 
        }

        public override void Draw(GameTime gameTime)
        {
            //Set bounding box for collision detection
            //Possibly throw this into update, but then again, stones don't move...
            boundingBox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);


            Vector2 offset = new Vector2(sprW / 2, sprH / 2);
            //Check mode then draw
            if (mode < 4)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(Sprite, Position - offset, rec, Color.White);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }



        public float damageMultiplier(stone opposing)
        {
            switch (type)
            {
                case 1: //Red beats blue, weak agaisnt yellow
                    if (opposing.type == 3)
                        return 0.25F;
                    else if (opposing.type == 2)
                        return 2.0F;
                break;
                case 2: //Blue beats yellow, weak agaisnt red
                if (opposing.type == 1)
                    return 0.25F;
                else if (opposing.type == 3)
                    return 2.0F;
                break;
                case 3: //yellow beats red, weak agaisnt blue
                if (opposing.type == 2)
                    return 0.25F;
                else if (opposing.type == 1)
                    return 2.0F;
                break;
            }
            return 1;
        }


    }
}