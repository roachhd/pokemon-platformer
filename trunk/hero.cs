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
        Texture2D uiFrame;

        int lives;
        Texture2D livesSprite;
        Vector2 livesOffset;
        Vector2 start;

        int score;

        

        int lastWalk = 1; int attacked = 0;
        Rectangle boundingBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, sprwid - 2, sprhei);
            }
        }
        Game g;

        public Rectangle getBoundingBox()
        {
            return this.boundingBox;
        }


        public hero(Game G, Vector2 position, Vector2 st)
            : base(G, position)
        {
            start = st;
            lives = 5;
            score = 0;
            g = G;
        }
        LinkedList<stone> inventory = new LinkedList<stone>();

        public LinkedList<stone> giveInventory()
        {
            return inventory;
        }

        public stone getHit()
        {
            stone removeMe = null;

            if (getHP() > 0)
            {
                //lives--;
                removeMe = inventory.First();
                inventory.RemoveFirst();
                if (getHP() > 0)
                {
                    inventory.First().mode = 2;
                    if (getHP() > 4)
                    {
                        inventory.ElementAt(4).mode = 3; //Set the 4th item to display again
                    }
                }
            }
            else
            {
                lives--;
                Position = start;
                //Play death animation
                if (lives < 0)
                {
                    //Game over. 
                }
            }

            return removeMe;
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
            for (int x = 0; x < getHP(); x++)
            {
                if (x == 0)
                    inventory.ElementAt(x).Position = new Vector2(410, 40);
                else
                    inventory.ElementAt(x).Position = new Vector2(420 + (30 * x), 30);
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
            if (lives == 0)
            {
                Game1.Screen = Game1.GameState.GameOver;
            }
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.Space))
            {
                if (inventory.First != null)
                {
                    if (attacked == 0)
                    {
                        Attack a = new Attack(g, this.Position, inventory.First().type, lastWalk);
                        Game.Components.Add(a);
                        attacked = 1;
                    }
                }
            }
            if (k.IsKeyUp(Keys.Space))
            {
                attacked = 0;
            }



            if (k.IsKeyDown(Keys.Left))
            {
                Position -= new Vector2(elapsed, 0) * 100;
                walked = -1;
                lastWalk = walked;
                if (step > 0) //if he was walking right or was climbing
                {
                    step = -1;
                }
            }
            if (k.IsKeyDown(Keys.Right))
            {
                Position += new Vector2(elapsed, 0) * 100;
                walked = 1;
                lastWalk = walked;
                if ((step < 0) || (step > 6)) //if he was walking left or was climbing
                {
                    step = 1;
                }

            }

            if (jumping == 1) { jump(gameTime); }

            IGameComponent removeMe = null;
            IGameComponent removeMe2 = null;

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

                Ground b = c as Ground;
                if (b != null)
                {
                    int diff = sprhei - 25; //25 = Ground height
                    if (boundingBox.Intersects(b.bb) && ((b.type == 1) || (b.type == 2) || (b.type == 3)))
                    {
                        if ((Position.Y > (b.bb.Top - diff)) && (Position.Y < (b.bb.Bottom - diff)))
                        // if the main player's position is between the top and the bottom of the bounding box
                        {
                            //it's a side collision.  check if position is to the left or right, and handle appropriately
                            if (Position.X < b.position.X) //it's on the left
                            {
                                Position = new Vector2(b.bb.Left - boundingBox.Width, Position.Y);
                            }
                            else Position = new Vector2(b.bb.Right, Position.Y);
                        }
                        else if (Position.Y < b.bb.Top)
                        // if the position of the player is above the top of the box, it's an above collision
                        {
                            Position = new Vector2(Position.X, b.bb.Top - boundingBox.Height + 1);
                        }

                        else
                            Position = new Vector2(Position.X, b.bb.Bottom);
                        //else it hit the bottom, but this should never happen.
                    }

                    if (boundingBox.Top < 50) Position = new Vector2(Position.X, 50);
                    if (boundingBox.Left < 25) Position = new Vector2(25, Position.Y);
                    if (boundingBox.Right >= 775) Position = new Vector2(725, Position.Y);
                    //if (boundingBox.Top < 40) Position = new Vector2(Position.X, 40 + boundingBox.Height / 2);
                    if ((boundingBox.Contains(b.bb) || (boundingBox.Intersects(b.bb))) && (b.type == 4))
                    //    if (boundingBox.Contains(b.bb) && (b.type == 4))
                    {
                        Position = new Vector2(Position.X, Position.Y - 2);
                        if (k.IsKeyDown(Keys.Up))
                        {
                            if ((step >= 0 && step <= 4) || (step < 0)) //if he had a walking sprite
                                step = 6;
                            Position -= new Vector2(0, elapsed) * 100;
                            climbed = 1;
                        }
                        if (k.IsKeyDown(Keys.Down))
                        {
                            if ((step >= 0 && step <= 4) || (step < 0)) //if he had a walking sprite
                                step = 6;
                            Position += new Vector2(0, elapsed) * 80;
                            climbed = 1;
                        }
                    }
                    if (boundingBox.Intersects(b.bb) && (b.type == 0) && (b.position.Y > (Position.Y - 64)))
                    {
                        Position += new Vector2(0, elapsed) * 80;

                    }
                }

                treasure t = c as treasure;

                if (t != null)
                {
                    if (boundingBox.Intersects(t.boundingBox))
                    {
                        score += t.points;
                        removeMe = t;
                        Game1.foodCount--;

                    }
                }

                Enemy e = c as Enemy;
                if (e != null)
                {
                    if (boundingBox.Intersects(e.boundingBox))
                    {
                        removeMe2 = getHit();
                        removeMe = e;
                    }
                }
                Portal p = c as Portal;

                if (p != null)
                {
                    if (Game1.foodCount <= 0)
                        p.SetActive(true); 
                    if(p.IsUsed())
                        Game1.Screen = Game1.GameState.LevelComplete;
                }
            }


            if (removeMe != null)
            {
                Game.Components.Remove(removeMe);
                if (removeMe2 != null)
                {
                    Game.Components.Remove(removeMe2);
                }
            }
            base.Update(gameTime);
        }



        public override void Draw(GameTime gameTime)
        {

            drawInventory();
            /*if (jumping == 1)
            { //jumpSprite(); 
            }
            else
            {
                // Rectangle rec = walk(gameTime);
            }*/

            Rectangle rec = walk(gameTime);
            spriteBatch.Begin();
            //Draw lives

            spriteBatch.DrawString(font, "Lives:", new Vector2(10, 2), Color.White);
            spriteBatch.DrawString(font, "Inventory:", new Vector2(300, 2), Color.White);
            for (int x = 0; x < lives; x++)
            {
                spriteBatch.Draw(livesSprite, new Vector2(10 + (x * 30), 30), Color.White);
            }
            //Draw Score
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(600, 5), Color.White);

            //Draw Hero
            spriteBatch.Draw(Sprite, Position, rec, typeCol, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}