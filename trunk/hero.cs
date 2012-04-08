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

    class hero : Character
    {
        SpriteFont font;

        int lives;
        Texture2D livesSprite;
        Vector2 livesOffset;

        int score;

        public hero(Game g, Vector2 position)
            : base(g, position)
        {
            lives = 5;
            score = 0;
        }
        LinkedList<stone> inventory = new LinkedList<stone>();

        public void getHit()
        {
            if (getHP() > 0)
            {
                inventory.RemoveFirst();
                inventory.First().mode = 2;
                if (getHP() > 4)
                {
                    inventory.ElementAt(4).mode = 3; //Set the 4th item to display again
                }
            }
            else
            {
                lives--;
                //Play death animation
                if (lives < 0)
                {
                    //Game over. 
                }
            }
        }

        public void pickUpStone(stone s)
        {
            score += 50; 
            if (getHP() > 0)
            {
                inventory.First().mode = 3;
                if (getHP() > 4)
                {
                    inventory.ElementAt(4).mode = 4; //Set the 4th item to be hidden
                }
            }

            inventory.AddFirst(s);
            s.mode = 2;
            s.Position = new Vector2(400, 40);

            switch (s.type)
                {
                    case 1:
                        typeCol = Color.White;
                        break;
                    case 2:
                        typeCol = new Color(255, 196, 196);
                        break;
                    default:
                        typeCol = Color.LightBlue;
                        break;
                }
        }

        public int getHP()
        {
            if (inventory == null) { return 0; }
            return inventory.Count();
        }


        public void drawInventory()
        {
            //Inventory
            for (int x = 1; x < getHP(); x++)
            {
                inventory.ElementAt(x).Position = new Vector2(410+(30*x), 40);
            }
            
        }




        protected override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("myFont");
            Sprite = Game.Content.Load<Texture2D>("mage");

            Vector2 offset = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
            Position = Position - offset;

            livesSprite = Game.Content.Load<Texture2D>("lives");
            livesOffset = new Vector2(livesSprite.Width / 2, livesSprite.Height / 2);


            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }
       
        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.Up))
            {
                if (ladder > 0) // if he's going up a ladder
                {
                    if ((step >= 0 && step <= 4) || (step < 0)) //if he had a walking sprite
                        step = 6;
                    Position -= new Vector2(0, elapsed) * 80;
                    climbed = 1;
                }
                else
                { //if he's jumping
                    jumping = 1;
                }
            }

            if (k.IsKeyDown(Keys.Down))
            {
                if (ladder > 0) // if he's going down a ladder
                {
                    if ((step >= 0 && step <= 4) || (step < 0)) //if he had a walking sprite
                        step = 6;
                    Position += new Vector2(0, elapsed) * 80;
                    climbed = 1;
                }

            }

            if (k.IsKeyDown(Keys.Left))
            {
                Position -= new Vector2(elapsed, 0) * 100;
                walked = -1;
                if (step > 0) //if he was walking right or was climbing
                {
                    step = -1;
                }
                Console.WriteLine(counter);
            }
            if (k.IsKeyDown(Keys.Right))
            {
                Position += new Vector2(elapsed, 0) * 100;
                walked = 1;
                if ((step < 0) || (step > 6)) //if he was walking left or was climbing
                {
                    step = 1;
                }
                Console.WriteLine(counter);

            }

            if (jumping == 1) { jump(gameTime); }


            boundingBox = new Rectangle((int)Position.X, (int)Position.Y, 64, 64);
            //Collision Detection (for stones)
            foreach (var c in Game.Components)
            {
                stone s = c as stone;

                if (s != null)
                {
                    if (boundingBox.Intersects(s.boundingBox))
                    {
                        pickUpStone(s);
                    }
                }

                treasure t = c as treasure;

                if (t != null)
                {
                    if (boundingBox.Intersects(t.boundingBox))
                    {
                        score += t.points;
                        t.mode = 0;
                        Console.WriteLine("Score = " + score);
                    }
                }
            }


            base.Update(gameTime);
        }

        Rectangle boundingBox;

        public override void Draw(GameTime gameTime)
        {
            drawInventory();

            if (jumping == 1)
            { //jumpSprite(); 
            }
            else
            {
                // Rectangle rec = walk(gameTime);
            }

            Rectangle rec = walk(gameTime);
            spriteBatch.Begin();
            //Draw lives
            spriteBatch.DrawString(font, "Lives:", new Vector2(10, 5), Color.White);
            for (int x = 0; x < lives; x++)
            {
                spriteBatch.Draw(livesSprite, new Vector2(10+(x*30), 30), Color.White);
            }
            //Draw Score
            spriteBatch.DrawString(font, "Score: "+score, new Vector2(600, 45), Color.White);

            //Draw Hero
            spriteBatch.Draw(Sprite, Position, rec, typeCol, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
