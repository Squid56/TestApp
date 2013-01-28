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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace FirstTest
{
    public class Constants
    {
        public static int MAX_LIVES = 5;
        public static int MAX_RAND_BEES = 5;
        public static int MIN_BEES = 10;
    }

    public class PlayerObject
    {
        public int Lives = Constants.MAX_LIVES;
        public bool hit = false;
        public int immune = 120;
        public int flowers = 0;
        public int umbrella = 0;
        public int umbrella_active = 0;
    }
    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public PlayerObject pl;
        bool GameOver = false;
        Sound1 soundObject= new Sound1();
        Sprite1 spriteObject= new Sprite1();
        int Level = 1;
        int LevelRefresh = 180;
        bool FirstStart = true;
        KeyboardState oldState;

        public int TargetResolutionX = 1920;
        public int TargetResolutionY = 1080;
        float ResolutionRatioX = 1.0f;
        float ResolutionRatioY = 1.0f;
        DepthStencilBuffer shadowDepthBuffer;
        RenderTarget2D rt;
        DepthStencilBuffer old;
        SpriteBatch resolution;

        Random RandGen = new Random();

        public Game1(string[] args)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            if (args.Count() == 0)
            {
                graphics.PreferredBackBufferWidth = TargetResolutionX;
                graphics.PreferredBackBufferHeight = TargetResolutionY;
            }
            else
            {
                graphics.PreferredBackBufferWidth = Convert.ToInt32(args[0]);
                graphics.PreferredBackBufferHeight = Convert.ToInt32(args[1]);
            }

            /*
             * Sound1 soundObject= new Sound1(this);
            Sprite1 spriteObject = new Sprite1(this);
            Components.Add(soundObject);
            Components.Add(spriteObject);
             */

            //graphics.ToggleFullScreen();
            //graphics.SynchronizeWithVerticalRetrace = false;
            //this.IsFixedTimeStep = false;
        }

        public bool isPlayerImmune()
        {
            if (pl.immune==0)
                return false;

            return true;
        }

        public float CorrectX()
        {
            return ResolutionRatioX;
        }

        public float CorrectY()
        {
            return ResolutionRatioY;
        }

        public bool isGameOver()
        {
            return GameOver;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        /// 
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            pl = new PlayerObject();
            spriteObject.Initialize(this, TargetResolutionX, TargetResolutionY);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            rt = new RenderTarget2D(graphics.GraphicsDevice, TargetResolutionX, TargetResolutionY, 1, GraphicsDevice.PresentationParameters.BackBufferFormat, graphics.GraphicsDevice.PresentationParameters.MultiSampleType, graphics.GraphicsDevice.PresentationParameters.MultiSampleQuality);
//            shadowDepthBuffer = CreateDepthStencil(rt, DepthFormat.Depth24Stencil8Single);
//            shadowDepthBuffer = CreateDepthStencil(rt);
            shadowDepthBuffer = new DepthStencilBuffer(rt.GraphicsDevice, rt.Width,
                rt.Height, rt.GraphicsDevice.DepthStencilBuffer.Format,
                rt.MultiSampleType, rt.MultiSampleQuality);

            resolution = new SpriteBatch(graphics.GraphicsDevice);
            spriteObject.LoadContent(this, graphics.GraphicsDevice);
            soundObject.LoadContent(this);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        protected void HandleKeyboardInput()
        {
            KeyboardState newState = Keyboard.GetState();

            if (isGameOver())
            {
                if ((newState != oldState))
                {
                    Level = 1;
                    pl.Lives = Constants.MAX_LIVES;
                    pl.flowers = 0;
                    GameOver = false;
                    spriteObject.Initialize(this, TargetResolutionX, TargetResolutionY);
                    soundObject.playonce = true;
                    LevelRefresh = 180;
                    FirstStart = true;
                    pl.hit = false;
                    soundObject.playsoundatstart = true;
                    pl.umbrella = 0;
                    pl.umbrella_active = 0;
                }
            }
            else
            {
                // Allows the game to exit
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();

                if (newState.IsKeyUp(Keys.Enter) && oldState.IsKeyDown(Keys.Enter))
                    graphics.ToggleFullScreen();

                /*
                 * if (newState.IsKeyUp(Keys.Space) && oldState.IsKeyDown(Keys.Space))
                {
                    spriteObject.ShuffleSprites();
                }

                
                 * if (newState.IsKeyUp(Keys.H) && oldState.IsKeyDown(Keys.H))
                {
                    spriteObject.ToggleHUD();
                }
                */
                if (newState.IsKeyUp(Keys.L) && oldState.IsKeyDown(Keys.L))
                {
                    pl.Lives = Constants.MAX_LIVES;
                }

                if (newState.IsKeyUp(Keys.S) && oldState.IsKeyDown(Keys.S))
                {
                    FirstStart = false;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    this.Exit();
            }

            oldState = newState;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            HandleKeyboardInput();

            if (FirstStart)
            {
                soundObject.PlayStartSound(this);
            }
            else
            {
                if (LevelRefresh > 0)
                {
                    LevelRefresh--;
                    soundObject.StopBuzzing(this);
                }
                else
                    soundObject.StartBuzzing(this);

                if (spriteObject.OutOfFlowers())
                {
                    Level++;
                    LevelRefresh = 180;
                    soundObject.PlayGong(this);
                    spriteObject.SpawnFlowers(TargetResolutionX, TargetResolutionY);
                }

                spriteObject.UpdatePlayerSprite(this,pl,soundObject);

                if (LevelRefresh == 0)
                {
                    spriteObject.UpdateSprite(this, gameTime, TargetResolutionX, TargetResolutionY, soundObject);

                    if (RandGen.NextDouble() < 0.001f)
                    {
                        spriteObject.SpawnUmbrella(this, TargetResolutionX, TargetResolutionY, soundObject);
                    }
                }

                if (pl.immune > 0)
                    pl.immune--;
                else
                    pl.hit = false;

                if (pl.umbrella_active > 0)
                    pl.umbrella_active--;
            }

            base.Update(gameTime);
        }

        public void IncrementFlowerCount()
        {
            pl.flowers++;
        }

        public void PlayerHit()
        {
            if (pl.umbrella > 0 && pl.umbrella_active <=0)
            {
                soundObject.PlayUmbrellaOpen(this);
                pl.umbrella--;
                pl.umbrella_active = 180;
            }
            else if (pl.immune <= 0 && pl.umbrella_active <= 0)
            {
                // don't play angry sound on last life
                if (pl.Lives != 1)
                    soundObject.PlayAngryBuzz(this);

                pl.hit = true;
                pl.immune = 120;
                pl.Lives--;
                if (pl.Lives <= 0)
                {
                    pl.Lives = 0;
                    GameOver = true;
                }
            }
        }

        public int GetFlowerCount()
        {
            return pl.flowers;
        }

        public int GetLevel()
        {
            return Level;
        }

        public int GetLives()
        {
            return pl.Lives;
        }

        public bool GetPlayerHit()
        {
            return pl.hit;
        }

        /*
        public RenderTarget2D CreateRenderTarget(GraphicsDevice device, int numberLevels,
            SurfaceFormat surface)
        {
            MultiSampleType type = device.PresentationParameters.MultiSampleType;

            // If the card can't use the surface format
            if (!GraphicsAdapter.DefaultAdapter.CheckDeviceFormat(DeviceType.Hardware,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Format, TextureUsage.None,
                QueryUsages.None, ResourceType.RenderTarget, surface))
            {
                // Fall back to current display format
                surface = device.DisplayMode.Format;
            }
            // Or it can't accept that surface format with the current AA settings
            else if (!GraphicsAdapter.DefaultAdapter.CheckDeviceMultiSampleType(DeviceType.Hardware,
                surface, device.PresentationParameters.IsFullScreen, type))
            {
                // Fall back to no antialiasing
                type = MultiSampleType.None;
            }

            int width, height;
            //int width=TargetResolutionX, height=TargetResolutionY;

            // See if we can use our buffer size as our texture
            CheckTextureSize(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight,
                out width, out height);

            // Create our render target
            return new RenderTarget2D(device,
                width, height, numberLevels, surface,
                type, 0);

        }

        public static bool CheckTextureSize(int width, int height, out int newwidth, out int newheight)
        {
            bool retval = false;

            GraphicsDeviceCapabilities Caps;
            Caps = GraphicsAdapter.DefaultAdapter.GetCapabilities(DeviceType.Hardware);

            if (Caps.TextureCapabilities.RequiresPower2)
            {
                retval = true;  // Return true to indicate the numbers changed

                // Find the nearest base two log of the current width, and go up to the next integer                
                double exp = Math.Ceiling(Math.Log(width) / Math.Log(2));
                // and use that as the exponent of the new width
                width = (int)Math.Pow(2, exp);
                // Repeat the process for height
                exp = Math.Ceiling(Math.Log(height) / Math.Log(2));
                height = (int)Math.Pow(2, exp);
            }
            if (Caps.TextureCapabilities.RequiresSquareOnly)
            {
                retval = true;  // Return true to indicate numbers changed
                width = Math.Max(width, height);
                height = width;
            }

            newwidth = Math.Min(Caps.MaxTextureWidth, width);
            newheight = Math.Min(Caps.MaxTextureHeight, height);
            return retval;
        }
        
        public static DepthStencilBuffer CreateDepthStencil(RenderTarget2D target)
        {
            return new DepthStencilBuffer(target.GraphicsDevice, target.Width,
                target.Height, target.GraphicsDevice.DepthStencilBuffer.Format,
                target.MultiSampleType, target.MultiSampleQuality);
        }
        
        public static DepthStencilBuffer CreateDepthStencil(RenderTarget2D target, DepthFormat depth)
        {
            if (GraphicsAdapter.DefaultAdapter.CheckDepthStencilMatch(DeviceType.Hardware,
               GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Format, target.Format,
                depth))
            {
                return new DepthStencilBuffer(target.GraphicsDevice, target.Width,
                    target.Height, depth, target.MultiSampleType, target.MultiSampleQuality);
            }
            else
                return CreateDepthStencil(target);
        }
        */

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Rectangle r1 = new Rectangle(0, 0, TargetResolutionX, TargetResolutionY);
            Rectangle r2 = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            graphics.GraphicsDevice.SetRenderTarget(0, rt);

            // Cache the current depth buffer
            old = GraphicsDevice.DepthStencilBuffer;
            // Set our custom depth buffer
            GraphicsDevice.DepthStencilBuffer = shadowDepthBuffer;
            
            graphics.GraphicsDevice.Clear(Color.Wheat);

            if (FirstStart)
                spriteObject.DoStart(this, gameTime, TargetResolutionX, TargetResolutionY);
            else
                spriteObject.Draw(this, gameTime, TargetResolutionX, TargetResolutionY, soundObject, LevelRefresh, pl);

            graphics.GraphicsDevice.SetRenderTarget(0, null);
            // Reset the depth buffer
            GraphicsDevice.DepthStencilBuffer = old;

            resolution.Begin(SpriteBlendMode.None);
            resolution.Draw(rt.GetTexture(),r2,r1,Color.White,0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);
            resolution.End();
            base.Draw(gameTime);
        }
    }
}
