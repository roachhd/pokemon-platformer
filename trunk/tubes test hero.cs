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


        public hero(Game g, Vector2 position) : base(g, position) { }
        LinkedList<stone> inventory = new LinkedList<stone>();
        public void getHit()
        {
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

        public void pickUpStone(stone s)
        {
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
            }
            if (k.IsKeyDown(Keys.Right))
            {
                Position += new Vector2(elapsed, 0) * 100;
                walked = 1;
                if ((step < 0) || (step > 6)) //if he was walking left or was climbing
                {
                    step = 1;
                }

            }

            if (jumping == 1) { jump(gameTime); }


            
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
            }


            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {

            if (jumping == 1)
            { //jumpSprite(); 
            }
            else
            {
                // Rectangle rec = walk(gameTime);
            }

            Rectangle rec = walk(gameTime);
            spriteBatch.Begin();
            //spriteBatch.Draw(Sprite, Position, Color.White);
            spriteBatch.Draw(Sprite, Position, rec, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}