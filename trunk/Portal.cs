using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Pokemon
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    class Portal : Ground
    {
        private bool Active;
        private bool UsePortal;
        private hero Mage;

        public Portal(Game g, hero m, Vector2 position)
            : base(g, position, 5)
        {
            UsePortal = false;
            Active = false;
            Mage = m;
        }

        public bool IsActive()
        {
            return Active;
        }

        public bool IsUsed()
        {
            return UsePortal;
        }

        public void SetActive(bool tf)
        {
            Active = tf;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                if (Mage.getBoundingBox().Intersects(this.bb))
                    UsePortal = true;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
