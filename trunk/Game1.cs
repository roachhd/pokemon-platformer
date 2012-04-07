using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Vector2[] TheMatrix;
        DrawableGameComponent[] myWorld = new DrawableGameComponent[3072];
        public static int[] level;
        private StreamReader readLevel;
        

        public Vector2[] MatrixInit()
        {   int counter=0;
            int h = 2400;
            int w = 800;
            int x = 25; //something
            /*Vector2[] result*/
            TheMatrix = new Vector2[3072];
            for (int i = 0; i < h; i+=x)
            {
                for (int j = 0; j < w; j += x)
                {
                    TheMatrix[counter] = new Vector2(i + (x / 2), j + (x / 2));
                    counter++;
                }
            }
            return TheMatrix;
        }

        public void LevelRead()
        {
            FileInfo myFile = new FileInfo("worstlevelever.txt");
            level = new int[3072];
            readLevel = myFile.OpenText();
            int myCurrentInt;

            for (int i = 0; i < level.Length; i++)
            {
                myCurrentInt = readLevel.Read();

                if (myCurrentInt != 13 && myCurrentInt != 10)
                level[i] = readLevel.Read() - 48;
            }

            readLevel.Close();
        }



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            MatrixInit();
            LevelRead();
            for (int i = 0; i < TheMatrix.Length; i++)
            {
                Components.Add(
                myWorld[i] = new Ground(this, TheMatrix[i], level[i])
                );
            }
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);


            base.Draw(gameTime);
        }
    }
}
