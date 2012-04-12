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

namespace pokemon
{
    class Enemy : Character
    {
        int sprwid=64, sprhei=64;

        hero mage;
        public Rectangle boundingBox { get; set; }

        public Enemy(Game g, Vector2 position, hero h) : base(g, position) { Position = position; mage = h; }

        protected override void LoadContent()
        {

            Sprite = Game.Content.Load<Texture2D>("enemy");

            Vector2 offset = new Vector2(sprwid / 2, sprhei / 2);
            Position = Position - offset;

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }


        public override void Update(GameTime gameTime)
        {

            boundingBox = new Rectangle((int)Position.X, (int)Position.Y, 50, 50);
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState k = Keyboard.GetState();


            foreach (var c in Game.Components)
            {
                Ground b = c as Ground;
                //put this in the enemy class
                if (b != null)
                {
                    if (boundingBox.Intersects(b.bb) && ((b.type == 1) || (b.type == 2) || (b.type == 3)))
                    {
                        if ((Position.Y > (b.bb.Top - 39)) && (Position.Y < (b.bb.Bottom - 39))) // if the main player's position is between the top and the bottom of the bounding box
                        {
                            //it's a side collision.  check if position is to the left or right, and handle appropriately
                            if (Position.X < b.position.X) //it's on the left
                            {
                                Position = new Vector2(b.bb.Left - boundingBox.Width, Position.Y);
                            }
                            else Position = new Vector2(b.bb.Right, Position.Y);
                        }
                        else if (Position.Y < b.bb.Top) // if the position of the player is above the top of the box, it's an above collision
                        {
                            Position = new Vector2(Position.X, b.bb.Top - boundingBox.Height + 1);
                        }
                        else Position = new Vector2(Position.X, b.bb.Bottom); //else it hit the bottom, but this should never happen.
                    }
                    //ladders
                    /*if (boundingBox.Contains(b.bb) || (boundingBox.Intersects(b.bb)) && (b.type == 4))
                    {
                        if (k.IsKeyDown(Keys.Up))
                        {
                            if ((step >= 0 && step <= 4) || (step < 0)) //if he had a walking sprite
                                step = 6;
                            Position -= new Vector2(0, elapsed) * 80;
                            climbed = 1;
                        }
                        if (k.IsKeyDown(Keys.Down))
                        {
                            if ((step >= 0 && step <= 4) || (step < 0)) //if he had a walking sprite
                                step = 6;
                            Position += new Vector2(0, elapsed) * 80;
                            climbed = 1;
                        }
                    }*/
                    if (boundingBox.Intersects(b.bb) && (b.type == 0) && (b.position.Y > (Position.Y - 64)))
                    {
                        Position += new Vector2(0, elapsed) * 80;

                    }
                }

            }


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
