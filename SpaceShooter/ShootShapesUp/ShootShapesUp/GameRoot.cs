using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Linq;

namespace ShootShapesUp
{
    public class GameRoot : Game
    {

        // some helpful static properties
        public static GameRoot Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        public static GameTime GameTime { get; private set; }

        public static Texture2D Player { get; private set; }
        public static Texture2D Wanderer { get; private set; }
        public static Texture2D Seeker { get; private set; }
        public static Texture2D Bullet { get; private set; }
        public static Texture2D Pointer { get; private set; }
        public static Texture2D puShield { get; private set; }
        public static Texture2D hpBar { get; private set; }
        public static Texture2D hpBarInner { get; private set; }
        public static Texture2D Boss { get; private set; }
        public static Texture2D dungeonBG { get; private set; }
        public static int level { get; private set; }

        public static SpriteFont Font { get; private set; }

        public static Song Music { get; private set; }
      

        private static readonly Random rand = new Random();
        

        private static SoundEffect[] explosions;
        // return a random explosion sound
        public static SoundEffect Explosion { get { return explosions[rand.Next(explosions.Length)]; } }

        private static SoundEffect[] shots;
        public static SoundEffect Shot { get { return shots[rand.Next(shots.Length)]; } }

        private static SoundEffect[] spawns;
        public static SoundEffect Spawn { get { return spawns[rand.Next(spawns.Length)]; } }

        public static SoundEffect pickUpPowerUp { get; private set; }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private PowerUpBar powerUpBar;
        private Parralax dungeonBackG;

        public GameRoot()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = @"Content";

            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 800;
            this.IsMouseVisible = true;

            
        }

        protected override void Initialize()
        {
            base.Initialize();

            EntityManager.Add(PlayerShip.Instance);

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(GameRoot.Music);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Player = Content.Load<Texture2D>("Art/Player");
            Seeker = Content.Load<Texture2D>("Art/Seeker");
            Bullet = Content.Load<Texture2D>("Art/Bullet");
            Pointer = Content.Load<Texture2D>("Art/Pointer");
            puShield = Content.Load<Texture2D>("Art/pu_Shield");
            hpBar = Content.Load<Texture2D>("Art/hpBar");
            hpBarInner = Content.Load<Texture2D>("Art/hpbar_inner");
            Wanderer = Content.Load<Texture2D>("Art/Wanderer");
            Boss = Content.Load<Texture2D>("Art/Boss");

            powerUpBar = new PowerUpBar(Content);
            dungeonBackG = new Parralax();
            
            Font = Content.Load<SpriteFont>("Font");
            
            dungeonBG = Content.Load<Texture2D>("Art/dungeonBG");
            dungeonBackG.Load(GraphicsDevice, dungeonBG);


            Music = Content.Load<Song>("Sound/Music");

            // These linq expressions are just a fancy way loading all sounds of each category into an array.
            explosions = Enumerable.Range(1, 8).Select(x => Content.Load<SoundEffect>("Sound/explosion-0" + x)).ToArray();
            shots = Enumerable.Range(1, 4).Select(x => Content.Load<SoundEffect>("Sound/shoot-0" + x)).ToArray();
            spawns = Enumerable.Range(1, 8).Select(x => Content.Load<SoundEffect>("Sound/spawn-0" + x)).ToArray();
            pickUpPowerUp = Content.Load<SoundEffect>("Sound/Pickup_Coin");
        }

        protected override void Update(GameTime gameTime)
        {

            GameTime = gameTime;
            Input.Update();

            // Allows the game to exit
            if (Input.WasButtonPressed(Buttons.Back) || Input.WasKeyPressed(Keys.Escape))
                this.Exit();
            
            EntityManager.Update();
            EnemySpawner.Update();
            powerUpBar.Update();

            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            dungeonBackG.Update(timeElapsed * 200);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Draw entities. Sort by texture for better batching.
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            EntityManager.Draw(spriteBatch);
            DrawRightAlignedString("Level: " + PlayerShip.Instance.level, 770);
            DrawRightAlignedString("Enemies Killed : " + PlayerShip.Instance.enemiesKilled, 750);
            if (PlayerShip.Instance.bossDead)
            {
                DrawVictoryText("YOU WIN");
            }
            spriteBatch.End();

            
            // Draw user interface
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            powerUpBar.Draw(spriteBatch);
            dungeonBackG.Draw(spriteBatch);
            spriteBatch.End();


            base.Draw(gameTime);
        }


        private void DrawVictoryText(string text)
        {
            spriteBatch.DrawString(GameRoot.Font, text , new Vector2(ScreenSize.X / 2, ScreenSize.Y / 2), Color.White);
        }
        private void DrawRightAlignedString(string text, float y)
        {
            var textWidth = GameRoot.Font.MeasureString(text).X;
            spriteBatch.DrawString(GameRoot.Font, text, new Vector2(ScreenSize.X - textWidth - 5, y), Color.White);
        }
    }
}
