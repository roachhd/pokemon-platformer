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
        //int sprwid = 50, sprhei = 50;

        hero mage;
        public Rectangle boundingBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, sprwid, sprhei);
            }
        }
        int Type;
        int life;


        //1-red,2-blue,3-yellow
        public Enemy(Game g, Vector2 position, hero h, int t, int l) : base(g, position) { Position = position; mage = h; Type = t; life = l; }

        protected override void LoadContent()
        {
            deathanim = Game.Content.Load<Texture2D>("attacked");
            if (Type == 1)
            {//Red
                sprType = 3;
                Sprite = Game.Content.Load<Texture2D>("enemyred");

            }
            else if (Type == 2)
            {//Blue
                sprType = 1;
                Sprite = Game.Content.Load<Texture2D>("enemyblue");
            }
            else
            {//Yellow
                sprType = 2;
                Sprite = Game.Content.Load<Texture2D>("enemyyellow");
            }


            Vector2 offset = new Vector2(sprwid / 2, sprhei / 2);
            Position = Position - offset;

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        int calculateDamage(int tattack, int tdefense)
        {

            if (tattack == 1)
            { //red
                if (tdefense == 1) { return 50; }
                else if (tdefense == 2) { return 25; } //blue
                else { return 100; }
            }
            else if (tattack == 2)
            { //blue
                if (tdefense == 1) { return 100; }
                else if (tdefense == 2) { return 50; } //blue
                else { return 25; }
            }
            else
            {
                if (tdefense == 1) { return 25; }
                else if (tdefense == 2) { return 100; } //blue
                else { return 50; }
            }
        }

        int died = 0;
        int dying = 0;
        int deathAnimation = -1;
        Attack at = null;
        Rectangle deathrec;
        Texture2D deathanim;
        public override void Update(GameTime gameTime)
        {


            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState k = Keyboard.GetState();


            if (died == 0)
            {

                foreach (var c in Game.Components)
                {
                    if (mage != null)
                    {
                        Attack a = c as Attack;
                        //put this in the enemy class
                        if (a != null)
                        {
                            if (boundingBox.Intersects(a.boundingBox) && boundingBox.Left < a.boundingBox.Left)
                            {
                                life -= calculateDamage(mage.giveInventory().First().type, this.Type);
                                Console.WriteLine("monster life: " + life);
                                at = (Attack)c;
                                if (life <= 0)
                                {


                                    at = (Attack)c;
                                    died = 1;

                                    if (at.Type == 1) deathrec = new Rectangle(0, 0, 33, 33);//red
                                    else if (at.Type == 2) deathrec = new Rectangle(33, 0, 33, 33);//blue
                                    else deathrec = new Rectangle(66, 0, 33, 33);//yellow
                                }
                            }
                        }
                    }

                    Ground b = c as Ground;
                    //put this in the enemy class
                    if (boundingBox.Left < 25) Position = new Vector2(25, Position.Y);
                    if (boundingBox.Right >= 775) Position = new Vector2(725, Position.Y);
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
                        //ladders
                        if (boundingBox.Contains(b.bb) || (boundingBox.Intersects(b.bb)) && (b.type == 4))
                        {
                            if (Position.Y > mage.Position.Y)
                            {
                                if ((step >= 0 && step <= 4) || (step < 0)) //if he had a walking sprite
                                    step = 6;
                                Position -= new Vector2(0, elapsed) * 40;
                                climbed = 1;
                            }
                            if (Position.Y < mage.Position.Y)
                            {
                                if ((step >= 0 && step <= 4) || (step < 0)) //if he had a walking sprite
                                    step = 6;
                                Position += new Vector2(0, elapsed) * 40;
                                climbed = 1;
                            }
                        }
                        if (boundingBox.Intersects(b.bb) && (b.type == 0) && (b.position.Y > (Position.Y - 50)))
                        {
                            Position += new Vector2(0, elapsed) * 40;

                        }
                    }

                }

                if (mage != null)
                {
                    if (Position.X > mage.Position.X)
                    {
                        Position -= new Vector2(elapsed, 0) * 40;
                        if (walked == 1)
                        {
                            step = -1;
                        }
                        walked = -1;
                    }

                    if (Position.X < mage.Position.X)
                    {
                        Position += new Vector2(elapsed, 0) * 40;
                        if (walked == -1)
                        {
                            step = 1;
                        }
                        walked = 1;
                    }
                }
            }
            else
            {

                if (dying < 60)
                {
                    if (dying % 10 == 0)
                    {
                        //monster dying animation

                        //((Attack)at).Type = 1;

                        deathAnimation *= -1;
                    }

                    dying++;
                }
                else
                {
                    Game.Components.Remove(this);
                }


            }


            if (at != null)
            {
                Game.Components.Remove((IGameComponent)at);

            }

            base.Update(gameTime);
        }





        public override void Draw(GameTime gameTime)
        {
            Rectangle rec = base.walk(gameTime);
            spriteBatch.Begin();
            spriteBatch.Draw(Sprite, Position, rec, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();


            if (deathAnimation == 1)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                spriteBatch.Draw(deathanim, Position, deathrec, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}

