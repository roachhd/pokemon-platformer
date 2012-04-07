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

        public Vector2 Position { get; set; }
        protected Texture2D Sprite;
        protected SpriteBatch spriteBatch;

        protected Color typeCol = Color.White;
        

        public Character(Game g, Vector2 position) : base(g) { Position = position; }

        protected override void LoadContent()
        {

            Sprite = Game.Content.Load<Texture2D>("mage");

            Vector2 offset = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
            Position = Position - offset;


            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }
        
       




    }




}
