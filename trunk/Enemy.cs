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
    class Enemy : Character
    {
        hero mage;
        public Rectangle boundingBox { get; set; }

        public Enemy(Game g, Vector2 position, hero h) : base(g, position) { Position = position; mage = h; }

        protected override void LoadContent()
        {

            Sprite = Game.Content.Load<Texture2D>("enemy");

            Vector2 offset = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
            Position = Position - offset;

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }


        public override void Update(GameTime gameTime)
        {

            boundingBox = new Rectangle((int)Position.X, (int)Position.Y, 64, 64);
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState k = Keyboard.GetState();

            if (Position.X > mage.Position.X)
            {
                Position -= new Vector2(elapsed, 0) * 40;
                if (walked == 1) { step = -1; }
                walked = -1;
            }

            if (Position.X < mage.Position.X)
            {
                Position += new Vector2(elapsed, 0) * 40;
                if (walked == -1) { step = 1; }
                walked = 1;
            }

            base.Update(gameTime);
        }




        public override void Draw(GameTime gameTime)
        {
            Rectangle rec = base.walk(gameTime);

            spriteBatch.Begin();
            spriteBatch.Draw(Sprite, Position, rec, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}