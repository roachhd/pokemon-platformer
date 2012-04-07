using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon
{
    class Ground : DrawableGameComponent
    {
        public Vector2 position { get; set; }
        Texture2D sprite;
        SpriteBatch spritebatch;
        public int type;
        public Rectangle bb
        {
            get
            {
                return new Rectangle(
                (int)position.X,
                (int)position.Y,
                sprite.Width,
                sprite.Height);


            }
        }
        //methods

        public Ground(Game g, Vector2 pos, int tp)
            : base(g)
        {
            position = pos;
            type = tp;
        }

        public override void Draw(GameTime gameTime)
        {
            if (this.type == 0) return;
            if (sprite == null) return;
                    Vector2 offset = new Vector2(sprite.Width / 2, sprite.Height / 2);

                    spritebatch.Begin();
                    spritebatch.Draw(sprite,
                    position - offset,
                    Color.White);
                    spritebatch.End();
              
        }



        protected override void LoadContent()
        {
            switch(type){
                case 0:
                    sprite = Game.Content.Load<Texture2D>("test");
                    break;
                case 1:
                    sprite = Game.Content.Load<Texture2D>("test");
                    break;
                case 2:
                    sprite = Game.Content.Load<Texture2D>("ground test");
                    break;
                case 3:
                    sprite = Game.Content.Load<Texture2D>("withit");
                    break;

            }

            spritebatch = new SpriteBatch(Game.GraphicsDevice);
        }

    }
}