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
        DrawableGameComponent[] myWorld = new DrawableGameComponent[768];
        public static int[] level;
        public static int levelCounter = 2;
        private Texture2D background;
        private hero player;
        private bool playerInit = false;
        Random random = new Random();
        private Texture2D uiFrame;
        public static int foodCount;
        private Song jungle;
        private Song creepy1;
        //public static Layer[] layers;


        private enum StartMenu
        {
            NewGame,
            ViewHighScores,
            Exit
        };

        public enum GameState
        {
            Start,
            HighScores,
            InGame,
            GameOver,
            LevelComplete,
            PauseMenu
        };
        public static GameState Screen = GameState.Start;
        private StartMenu SelectedMenu = StartMenu.NewGame;

        private SpriteFont menuFont;



        public Vector2[] MatrixInit()
        {
            int counter = 0;
            int h = 600;
            int w = 800;
            int x = 25; //something
            /*Vector2[] result*/
            TheMatrix = new Vector2[768];
            for (int j = 0; j < h; j += x)
            {
                for (int i = 0; i < w; i += x)
                {
                    TheMatrix[counter] = new Vector2(i + (x / 2), j + (x / 2));
                    counter++;
                }
            }
            return TheMatrix;
        }

        public int[] LevelRead(string s)
        {
            StreamReader myFile = new StreamReader(s);
            level = new int[768];
            int counter = 0;
            string line;
            while ((line = myFile.ReadLine()) != null && counter < level.Length)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    level[counter] = line[i] - 48;
                    counter++;
                }

            }
            myFile.Close();
            return level;
        }



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Note, the different symbols in the .txt files correspond to various levels.
        /// We have: 0-Nothing/Sky,1-Ground,2-Hero,3-Enemy,4-StoneRed,5-StoneBlue, 6-StoneYellow, 7-Treasure, 8-Ladder
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            foodCount = 0;
            TheMatrix = MatrixInit();

            level = LevelRead("level" + levelCounter + ".txt");
            
            player = new hero(this, new Vector2(525, 525), new Vector2(525, 525));
            for (int i = 0; i < TheMatrix.Length; i++)
            {
                switch (level[i])
                {
                    case 0: // Sky
                        Components.Add(
                            myWorld[i] = new Ground(this, TheMatrix[i], level[i])
                            );
                        break;
                    case 1: // Ground
                        Components.Add(
                            myWorld[i] = new Ground(this, TheMatrix[i], level[i])
                            );
                        break;
                    case 2: // Player

                        Components.Add(myWorld[i] = player);
                        break;
                    case 3: // Enemy
                        Components.Add(myWorld[i] = new Enemy(this, TheMatrix[i], player, random.Next(0, 3), 100));
                        break;
                    case 4: // Ladder
                        Components.Add(myWorld[i] = new Ground(this, TheMatrix[i], level[i]));
                        break;

                    case 5: //Red Stone
                        Components.Add(myWorld[i] = new stone(1, 1, this, TheMatrix[i]));
                        break;

                    case 6: // Blue Stone
                        Components.Add(myWorld[i] = new stone(2, 1, this, TheMatrix[i]));
                        break;

                    case 7: // Yellow Stone
                        Components.Add(myWorld[i] = new stone(3, 1, this, TheMatrix[i]));
                        break;

                    case 8: // Treasure/Food
                        Components.Add(myWorld[i] = new treasure(1, random.Next(0, 27), this, TheMatrix[i]));
                        foodCount++;
                        break;
                    case 9:
                        // Add portal
                        Components.Add(myWorld[i] = new Portal(this, player, TheMatrix[i]));
                        break;
                }

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
            menuFont = Content.Load<SpriteFont>("myfont");
            jungle = Content.Load<Song>("jungle");
            creepy1 = Content.Load<Song>("creepy1");
            if (levelCounter == 1 || levelCounter == 3 || levelCounter == 5)
            {
                background = Content.Load<Texture2D>("backgroundjungle");
            }
            else background = Content.Load<Texture2D>("underwater");    //Eventually we need some nice pixel art here.

            uiFrame = Content.Load<Texture2D>("frame");
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
            KeyboardState buttonsPressed = Keyboard.GetState();
            switch (Screen)
            {
                case GameState.Start:
                    if (buttonsPressed.IsKeyDown(Keys.Enter))
                    {
                        if (levelCounter == 1 || levelCounter == 3 || levelCounter == 5)
                        {
                            MediaPlayer.Play(jungle);
                        }
                        else MediaPlayer.Play(creepy1);
                        Screen = GameState.InGame;
                    }
                    if (buttonsPressed.IsKeyDown(Keys.H))
                    {
                        Screen = GameState.HighScores;
                    }
                    if (buttonsPressed.IsKeyDown(Keys.Escape))
                        this.Exit();
                    break;

                case GameState.LevelComplete:
                    // Is there stuff to add here?
                    if (buttonsPressed.IsKeyDown(Keys.Enter))
                    {
                        MediaPlayer.Stop();
                        Components.Clear();
                        levelCounter++;
                        Screen = GameState.InGame;
                        Initialize();
                    }
                        // next level
                    break;

                case GameState.InGame:
                    base.Update(gameTime);
                    break;

                case GameState.GameOver:
                    if (buttonsPressed.IsKeyDown((Keys.R)))
                    {
                        Components.Clear();
                        Screen = GameState.InGame;
                        Initialize();
                    }
                    if (buttonsPressed.IsKeyDown((Keys.H)))
                    {
                        Screen = GameState.HighScores;
                    }

                    if (buttonsPressed.IsKeyDown(Keys.Escape))
                        this.Exit();
                    break;

                case GameState.HighScores:
                    break;
            }
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (buttonsPressed.IsKeyDown(Keys.Escape))
            {
                /*if (Screen == GameState.InGame)
                    Screen = GameState.PauseMenu;
                else if (Screen == GameState.Start)*/
                this.Exit();
            }
            // TODO: Add your update logic here

            //base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch (Screen)
            {
                case GameState.Start:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin();
                    const string title = "LEVELS FROM HELL"; // Insert mind-blowingly awesome game title here.
                    const string instruct = "Move around with arrow keys and collect the food.";
                    const string instruct2 = "Launch spells with Space. Get to the Portal when all the food is collected";
                    const string descript = "Enter = new game";
                    const string descript2 = "H = view high scores";
                    const string descript3 = "ESC = quit";

                    spriteBatch.DrawString(menuFont, title,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (menuFont.MeasureString(title).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (menuFont.MeasureString(title).Y / 2) - 40),
                        Color.Maroon);

                    spriteBatch.DrawString(menuFont, instruct,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (menuFont.MeasureString(instruct).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (menuFont.MeasureString(instruct).Y / 2)),
                        Color.Blue);
                    spriteBatch.DrawString(menuFont, instruct2,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (menuFont.MeasureString(instruct2).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (menuFont.MeasureString(instruct2).Y / 2) + 10),
                        Color.Blue);

                    spriteBatch.DrawString(menuFont, descript,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (menuFont.MeasureString(descript).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (menuFont.MeasureString(descript).Y / 2) + 20),
                        Color.BlueViolet);

                    spriteBatch.DrawString(menuFont, descript2,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (menuFont.MeasureString(descript2).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (menuFont.MeasureString(descript2).Y / 2) + 40),
                        Color.BlueViolet);

                    spriteBatch.DrawString(menuFont, descript3,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (menuFont.MeasureString(descript3).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (menuFont.MeasureString(descript3).Y / 2) + 60),
                        Color.BlueViolet);
                    spriteBatch.End();
                    break;

                case GameState.InGame:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin();
                    spriteBatch.Draw(background,
                        new Rectangle(0, 0, Window.ClientBounds.Width,
                            Window.ClientBounds.Height), null,
                            Color.White, 0, Vector2.Zero,
                            SpriteEffects.None, 0);
                    spriteBatch.Draw(uiFrame, new Vector2(0, 0), Color.White);
                    spriteBatch.End();
                    base.Draw(gameTime);
                    break;

                case GameState.LevelComplete:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin();
                    const string youwin = "Using wits and your mighty spells, you have conquered!";
                    const string next = "Press ENTER to teleport to the next dimension, or ESC to quit.";
                    
                    spriteBatch.DrawString(menuFont, youwin,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (menuFont.MeasureString(title).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (menuFont.MeasureString(youwin).Y / 2) - 10),
                        Color.Maroon);

                    spriteBatch.DrawString(menuFont, next,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (menuFont.MeasureString(descript).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (menuFont.MeasureString(next).Y / 2) + 20),
                        Color.BlueViolet);
                    spriteBatch.End();
                    break;
                case GameState.GameOver:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin();
                    const string endtitle = "Your spirit has departed the physical realm.";
                    const string describeyouroptions = "Press R to restart the level, N to start a new game, H to view high scores, or ESC to exit.";

                    spriteBatch.DrawString(menuFont, endtitle,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (menuFont.MeasureString(title).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (menuFont.MeasureString(endtitle).Y / 2) - 10),
                        Color.Maroon);

                    spriteBatch.DrawString(menuFont, describeyouroptions,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (menuFont.MeasureString(descript).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (menuFont.MeasureString(describeyouroptions).Y / 2) + 20),
                        Color.BlueViolet);
                    spriteBatch.End();
                    break;
                // Enter lots of fun code stuff here.
            }
            //GraphicsDevice.Clear(Color.White);


            //base.Draw(gameTime);
        }
    }
}