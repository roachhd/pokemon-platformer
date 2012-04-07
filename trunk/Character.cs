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
    class Character : DrawableGameComponent
    {
    
        Vector2 Position { get; set; }
        Texture2D Sprite;
        SpriteBatch spriteBatch;

        int step=1;
        int walked = 0;
        int counter=0;

        private Rectangle walk(GameTime gameTime)
        {

            float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Rectangle rec;
            counter++;
            
            if (counter > 5)
            {
                counter = 0;
                if (walked == 1)
                {
                    if (step == 4)
                    {
                        step = 1;
                    }
                    else { step++; }
                }
                if (walked == -1)
                {
                    if (step == -4)
                    {
                        step = -1;
                    }
                    else { step--; }
                }
            }
            walked = 0;
            if (step==1) rec = new Rectangle(0, 0, 64, 64);
            else if (step==2) rec = new Rectangle(64, 0, 64, 64);
            else if (step == 3) rec = new Rectangle(128, 0, 64, 64);
            else if (step == 4) rec = new Rectangle(64, 0, 64, 64);
            else if (step == -1) rec = new Rectangle(0, 64, 64,64);
            else if (step == -2) rec = new Rectangle(64, 64, 64, 64);
            else if (step == -3) rec = new Rectangle(128, 64, 64, 64);
            else rec = new Rectangle(64, 64, 64, 64);

            return rec;
        }

        public Character(Game g, Vector2 position) : base(g) { Position = position; }

        protected override void LoadContent()
        {

            Sprite = Game.Content.Load<Texture2D>("mage");

            Vector2 offset = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
            Position = Position - offset;

            counter = 0;

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }
        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.Up))
            {
                //accel += elapsed;

            }

            if (k.IsKeyDown(Keys.Left))
            {
                Position -= new Vector2(elapsed, 0) * 100;
                walked = -1;
                if (step > 0)
                {
                    step = -1;
                }
                Console.WriteLine(counter);
            }
            if (k.IsKeyDown(Keys.Right))
            {
                //angle += elapsed;
                Position += new Vector2(elapsed, 0) * 100;
                walked = 1;
                if (step < 0)
                {
                    step = 1;
                }
                Console.WriteLine(counter);

            }

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            Rectangle rec=walk(gameTime);


            spriteBatch.Begin();
            //spriteBatch.Draw(Sprite, Position, Color.White);
            spriteBatch.Draw(Sprite,Position,rec,Color.White,0,Vector2.Zero,1,SpriteEffects.None,0);

            spriteBatch.End();

            base.Draw(gameTime);
        }




    }

    


}
