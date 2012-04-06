using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WindowsGame1
{
    class stone: DrawableGameComponent
    {
        public int mode { get; set; } //1 collectable, 2 current, 3 in inventory, 4 far back in inventory
        public int type { get; set; } //1 red, 2 blue, 3 yellow
        public Vector2 Position { get; set; }
        Texture2D Sprite;
        SpriteBatch spriteBatch;

        public stone(int t, int m, Game g, Vector2 position):base(g)
        {
            type = t;
            mode = m;
            Position = position;
        }

        protected override void LoadContent()
        {
            switch (type)
            {
                case 1:
                    Sprite = Game.Content.Load<Texture2D>("firestone");
                    break;

                case 2:
                    Sprite = Game.Content.Load<Texture2D>("waterstone");
                    break;

                case 3:
                    Sprite = Game.Content.Load<Texture2D>("leafstone");
                    break;
            }
            
            
            spriteBatch = new SpriteBatch(Game.GraphicsDevice); 
        }

        public override void Draw(GameTime gameTime)
        {

            Vector2 offset = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
            //Check mode then draw
            spriteBatch.Begin();
            spriteBatch.Draw(Sprite, Position - offset, Color.White);
            spriteBatch.End();

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
