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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace FirstTest
{
    public class MovingObject
    {
        public Vector2 position;
        public float rotation;
        public float rotation_speed;
        public Vector2 speed;
        public int tex;
        public bool first;
        public bool marked = false;
        public int width;
        public int height;
        public int immune = 0;
    }

    public class Sprite1
    {
        SpriteBatch spriteBatch;
        List<MovingObject> SpriteList = new List<MovingObject>();
        List<MovingObject> SpriteList2 = new List<MovingObject>();
        Random RandGen = new Random();

        Texture2D sprite1;
        Texture2D sprite2;
        Texture2D sprite3;
        Texture2D sprite4;
        Texture2D sprite5;
        Texture2D sprite6;
        Texture2D sprite7;
        Texture2D sprite8;
        Texture2D sprite9;
        Texture2D sprite10;
        Texture2D MenuFrame;
        Texture2D Title;
        Texture2D sprite11;
        Texture2D Tree;

        SpriteFont Font1;
        SpriteFont Font3;
        Vector2 FontPos;

        Curve curveObject;

        bool draw_HUD = true;

        //float greyscale_color = 0.0f;
        long framecount = 0;
        double FPS_Count = 0.0f;
        Vector2 old_position = Vector2.Zero;
        SpriteEffects SEVal = SpriteEffects.None;

        MovingObject umbrella;

        // pre-calc the resolution ration for mouse movement in a resolution independant format
        float resolution_x_ratio;
        float resolution_y_ratio;

        // pre-cal the intersect parameters for the umbrella
        int umb_width;
        int umb_height;

   
        public bool Intersects(MovingObject a, MovingObject b)
        {

            BoundingSphere s1, s2;
            Vector3 acenter = new Vector3(a.position.X, a.position.Y , 0.0f);
            Vector3 bcenter = new Vector3(b.position.X, b.position.Y , 0.0f);
            
            // (w/2 + h/2)/3 = (w+h)/6
            float aradius = ((float)(a.width+a.height)/6.0f);
            float bradius = ((float)(b.width+b.height)/6.0f);

            //s1 = new BoundingBox(new Vector3(a.position.X + 30, a.position.Y + 30, 0.0f), new Vector3(a.position.X + sprite5.Width - 30, a.position.Y + sprite5.Height - 30, 0.0f));
            //s2 = new BoundingBox(new Vector3(b.position.X + 30, b.position.Y + 30, 0.0f), new Vector3(b.position.X + sprite5.Width - 30, b.position.Y + sprite5.Height - 30, 0.0f));
            s1 = new BoundingSphere(acenter, aradius);
            s2 = new BoundingSphere(bcenter, bradius);
            return (s1.Intersects(s2));

        }

        public void Initialize(Game1 game, int max_X, int max_Y)
        {
            //game.IsMouseVisible = true; 
            int i = RandGen.Next(Constants.MAX_RAND_BEES) + Constants.MIN_BEES;

            // pre-calc the resolution ration for mouse movement in a resolution independant format
            resolution_x_ratio = (float)((float)game.TargetResolutionX / (float)game.GraphicsDevice.Viewport.Width);
            resolution_y_ratio = (float)((float)game.TargetResolutionY / (float)game.GraphicsDevice.Viewport.Height);

            SpriteList.Clear();
            SpriteList2.Clear();

            for (int j = 0; j < i; j++)
            {
                MovingObject placeholder = new MovingObject();

                placeholder.rotation_speed = (float)RandGen.NextDouble() / 10;
                placeholder.speed = new Vector2(100 + (float)RandGen.NextDouble() * 100.0f, 100 + (float)RandGen.NextDouble() * 100.0f);
                placeholder.tex = 4;// RandGen.Next(3);
                placeholder.width = sprite5.Width;
                placeholder.height = sprite5.Height;

                bool collided = false;
                do
                {
                    collided = false;
                    placeholder.position = new Vector2(max_X * (float)RandGen.NextDouble(), max_Y * (float)RandGen.NextDouble());
                    foreach (MovingObject a in SpriteList)
                    {
                        if (Intersects(a, placeholder))
                        {
                            collided = true;
                            break;
                        }
                    }
                } while (collided);
                //placeholder.rotation = (float)RandGen.NextDouble()*RandGen.Next(50);

                if (j == 0) placeholder.first = true; else placeholder.first = false;

                SpriteList.Add(placeholder);
            }

            for (i = 0; i < 100; i++)
            {
                MovingObject placeholder = new MovingObject();
                placeholder.width = sprite7.Width;
                placeholder.height = sprite7.Height;
                placeholder.position = new Vector2(max_X * (float)RandGen.NextDouble(), max_Y * (float)RandGen.NextDouble());
                placeholder.speed = new Vector2(10 + (50.0f * (float)Math.Sin(MathHelper.ToRadians((float)(360 * RandGen.NextDouble())))), 10 + (50.0f * (float)Math.Sin(MathHelper.ToRadians((float)(360 * RandGen.NextDouble())))));
                SpriteList2.Add(placeholder);
            }
        }

        public void LoadContent(Game1 game, GraphicsDevice g)
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(g);

            sprite1 = game.Content.Load<Texture2D>("sprite1");
            sprite2 = game.Content.Load<Texture2D>("sprite2");
            sprite3 = game.Content.Load<Texture2D>("sprite3");
            sprite4 = game.Content.Load<Texture2D>("sprite4");
            sprite5 = game.Content.Load<Texture2D>("sprite5");
            sprite6 = game.Content.Load<Texture2D>("sprite6");
            sprite7 = game.Content.Load<Texture2D>("sprite7");
            sprite8 = game.Content.Load<Texture2D>("sprite8");
            sprite9 = game.Content.Load<Texture2D>("sprite9");
            sprite10 = game.Content.Load<Texture2D>("sprite10");
            MenuFrame = game.Content.Load<Texture2D>("OldMenuFrame");
            Title = game.Content.Load<Texture2D>("Title");
            sprite11 = game.Content.Load<Texture2D>("umbrella");
            Font1 = game.Content.Load<SpriteFont>("Arial");
            Font3 = game.Content.Load<SpriteFont>("Courier New Big");
            Tree = game.Content.Load<Texture2D>("Tree");

            curveObject = game.Content.Load<Curve>("TitleBounce");
            
            // pre-cal the intersect parameters for the umbrella
            umb_width = sprite11.Width / 4;
            umb_height = sprite11.Height / 2;
        }

        int tree_start_x = 100;
        int tree_start_y = 0;
        public void DrawTree(int screen_height)
        {
            tree_start_y++;
            if (tree_start_y < Tree.Height - screen_height)
            {
                Rectangle r = new Rectangle(0, tree_start_y, Tree.Width, tree_start_y + screen_height);
                spriteBatch.Draw(Tree, new Vector2(tree_start_x, 0), r, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            }
            else
            {
                Rectangle r1 = new Rectangle(0, tree_start_y, Tree.Width, Tree.Height-tree_start_y);
                Rectangle r2 = new Rectangle(0, 0, Tree.Width, (tree_start_y+screen_height)-Tree.Height);
                spriteBatch.Draw(Tree, new Vector2(tree_start_x, 0), r1, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(Tree, new Vector2(tree_start_x, Tree.Height - tree_start_y), r2, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            }

            if (tree_start_y > Tree.Height)
                tree_start_y = 0;

        }

        public void ShuffleSprites()
        {
            foreach (MovingObject a in SpriteList)
            {
                a.position = a.position + a.speed + new Vector2((float)RandGen.NextDouble() * 100, (float)RandGen.NextDouble() * 100);
            }

        }

        public void ToggleHUD()
        {
            draw_HUD = !draw_HUD;
        }

        public bool OutOfFlowers()
        {
            if (SpriteList2.Count() == 0)
                return true;
            else
                return false;
        }

        public void SpawnUmbrella(Game1 game, int max_X, int max_Y, Sound1 so)
        {
            if (umbrella == null)
            {
                umbrella = new MovingObject();
                umbrella.width = sprite11.Width;
                umbrella.height = sprite11.Height;
                umbrella.position = new Vector2(max_X * (float)RandGen.NextDouble(), max_Y * (float)RandGen.NextDouble());
                umbrella.speed = Vector2.Zero;
                so.PlayUmbrellaSpawn(game);
            }
        }

        public void SpawnFlowers(int max_X, int max_Y)
        {
            for (int i = 0; i < 100; i++)
            {
                MovingObject placeholder = new MovingObject();
                placeholder.width = sprite7.Width;
                placeholder.height = sprite7.Height;
                placeholder.position = new Vector2(max_X * (float)RandGen.NextDouble(), max_Y * (float)RandGen.NextDouble());
                placeholder.speed = new Vector2(10 + (50.0f * (float)Math.Sin(MathHelper.ToRadians((float)(360 * RandGen.NextDouble())))), 10 + (50.0f * (float)Math.Sin(MathHelper.ToRadians((float)(360 * RandGen.NextDouble())))));
                SpriteList2.Add(placeholder);
            }

            for (int j = 0; j < 3; j++)
            {
                MovingObject placeholder = new MovingObject();
                bool collided = false;
                placeholder.width = sprite5.Width;
                placeholder.height = sprite5.Height;
                do
                {
                    collided = false;
                    placeholder.position = new Vector2(max_X * (float)RandGen.NextDouble(), max_Y * (float)RandGen.NextDouble());
                    foreach (MovingObject a in SpriteList)
                    {
                        if (Intersects(a, placeholder))
                        {
                            collided = true;
                            break;
                        }
                    }
                } while (collided);
                placeholder.speed = new Vector2(100 + (float)RandGen.NextDouble() * 100.0f, 100 + (float)RandGen.NextDouble() * 100.0f);
                placeholder.tex = 4;

                SpriteList.Add(placeholder);
            }
        }

        public void UpdatePlayerSprite(Game1 game, PlayerObject pl, Sound1 so)
        {
            MouseState current_mouse = Mouse.GetState();

            old_position = SpriteList.First().position;
            if (current_mouse.X > game.GraphicsDevice.Viewport.Width || current_mouse.Y > game.GraphicsDevice.Viewport.Height || current_mouse.X < 0 || current_mouse.Y < 0)
            {
                // out of bounds
            }
            else
                SpriteList.First().position = new Vector2(current_mouse.X * resolution_x_ratio, current_mouse.Y * resolution_y_ratio);

            // check for intersection with umbrella
            if (umbrella != null)
            {
                MovingObject umb_intersect = umbrella;
                umb_intersect.width = umb_width;
                umb_intersect.height = umb_height;
                if (Intersects(SpriteList.First(), umb_intersect))
                {
                    so.PlayUmbrellaPickUp(game);
                    pl.umbrella++;
                    umbrella=null;
                }
            }
        }

        public void UpdateSprite(Game1 game, GameTime gameTime, int max_X, int max_Y, Sound1 so)
        {
            List<MovingObject> cleanup = new List<MovingObject>();
            Vector2 old_position;

            if (!game.isGameOver())
            {
                foreach (MovingObject a in SpriteList)
                {
                    // Move the sprite by speed, scaled by elapsed time.
                    // First sprite is the player sprite
                    if (!a.first)
                    {
                        old_position = a.position;

                        a.position += a.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (a.immune > 0) a.immune--;

                        foreach (MovingObject b in SpriteList)
                        {
                            if (a != b)
                            {

                                if (Intersects(a, b))
                                {
                                    if (b.first)
                                    {
                                        if (a.immune == 0)
                                        {
                                            game.PlayerHit();
                                            a.speed = new Vector2(100 + (50.0f * (float)Math.Sin(MathHelper.ToRadians((float)(360 * RandGen.NextDouble())))), 100 + (50.0f * (float)Math.Sin(MathHelper.ToRadians((float)(360 * RandGen.NextDouble()))))); ;
                                            a.immune = 30;
                                        }
                                    }
                                    else
                                    {
                                        Vector2 tmp = a.speed;
                                        a.speed = b.speed;
                                        b.speed = tmp;
                                        a.position = old_position;
                                    }
                                }
                            }
                        }

                        int MaxX = max_X - sprite5.Width;
                        int MinX = 0;
                        int MaxY = max_Y - sprite5.Height;
                        int MinY = 0;

                        //a.rotation += a.rotation_speed;

                        // Check for bounce.
                        if ((a.position.X - sprite5.Width) > MaxX)
                        {
                            Vector2 dir;
                            dir = Vector2.Normalize(a.speed);
                            a.speed.X = 100 + ((float)RandGen.NextDouble() * 100.0f);
                            a.speed.X *= -dir.X;
                            a.position.X = MaxX + sprite5.Width;
                        }

                        else if (a.position.X < MinX)
                        {
                            Vector2 dir;
                            dir = Vector2.Normalize(a.speed);
                            a.speed.X = 100 + ((float)RandGen.NextDouble() * 100.0f);
                            a.speed.X *= -dir.X;
                            a.position.X = MinX;
                        }

                        if ((a.position.Y - sprite5.Height) > MaxY)
                        {
                            Vector2 dir;
                            dir = Vector2.Normalize(a.speed);
                            a.speed.Y = 100 + ((float)RandGen.NextDouble() * 100.0f);
                            a.speed.Y *= -dir.Y;
                            a.position.Y = MaxY + sprite5.Height;
                        }

                        else if (a.position.Y < MinY)
                        {
                            Vector2 dir;
                            dir = Vector2.Normalize(a.speed);
                            a.speed.Y = 100 + ((float)RandGen.NextDouble() * 100.0f);
                            a.speed.Y *= -dir.Y;
                            a.position.Y = MinY;
                        }
                    }
                }
            }

            foreach (MovingObject a in SpriteList2)
            {

                // Move the sprite by speed, scaled by elapsed time.
                a.position += a.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                int MaxX = max_X - sprite7.Width;
                int MinX = 0;
                int MaxY = max_Y - sprite7.Height;
                int MinY = 0;

                a.rotation += 0.001f;

                // Check for bounce.
                if ((a.position.X - sprite7.Width) > MaxX)
                {
                    Vector2 dir;
                    dir = Vector2.Normalize(a.speed);
                    a.speed.X = 10 + ((float)RandGen.NextDouble() * 20.0f);
                    a.speed.X *= -dir.X;
                    a.position.X = MaxX + sprite7.Width;
                }

                else if (a.position.X < MinX)
                {
                    Vector2 dir;
                    dir = Vector2.Normalize(a.speed);
                    a.speed.X = 10 + ((float)RandGen.NextDouble() * 20.0f);
                    a.speed.X *= -dir.X;
                    a.position.X = MinX;
                }

                if ((a.position.Y - sprite7.Height) > MaxY)
                {
                    Vector2 dir;
                    dir = Vector2.Normalize(a.speed);
                    a.speed.Y = 10 + ((float)RandGen.NextDouble() * 20.0f);
                    a.speed.Y *= -dir.Y;
                    a.position.Y = MaxY + sprite7.Height;
                }

                else if (a.position.Y < MinY)
                {
                    Vector2 dir;
                    dir = Vector2.Normalize(a.speed);
                    a.speed.Y = 10 + ((float)RandGen.NextDouble() * 20.0f);
                    a.speed.Y *= -dir.Y;
                    a.position.Y = MinY;
                }

                if (!game.isGameOver() && Intersects(SpriteList.First(), a))
                {
                    if (!game.isPlayerImmune())
                    {
                        game.IncrementFlowerCount();
                        cleanup.Add(a);
                    }
                }
            }

            if (!game.isGameOver())
            {
                foreach (MovingObject a in cleanup)
                {
                    so.PlayFlowerPick(game);
                    SpriteList2.Remove(a);
                }
            }
        }

        public void Draw(Game1 game, GameTime gameTime, int max_X, int max_Y, Sound1 so, int LevelRefresh, PlayerObject pl)
        {
            string output;
            Vector2 FontOrigin;

            //greyscale_color = (float)Math.Abs(Math.Sin(MathHelper.ToRadians((float)(gameTime.TotalGameTime.TotalSeconds * 10))));

            // TODO: Add your drawing code here
            // Draw the sprite.
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            int xTiles = max_X / sprite4.Width;
            int yTiles = max_Y / sprite4.Height;

            // background image
            for (int i = 0; i <= xTiles; i++)
            {
                for (int j = 0; j <= yTiles; j++)
                {
                    //spriteBatch.Draw(sprite4, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, new Vector2(graphics.GraphicsDevice.Viewport.Width / (float)sprite4.Width, graphics.GraphicsDevice.Viewport.Height / (float)sprite4.Height), SpriteEffects.None, 0);
                    //spriteBatch.Draw(sprite4, new Vector2(i * sprite4.Width, j * sprite4.Height), new Color(1.0f, 1.0f, 1.0f, MathHelper.Clamp(greyscale_color,0.2f,1.0f)));
                    spriteBatch.Draw(sprite4, new Vector2(i * sprite4.Width, j * sprite4.Height), new Color(1.0f, 1.0f, 1.0f));
                }
            }

            // tree
            DrawTree(max_Y);

            // flowers
            float half_w = sprite7.Width / 2;
            float half_h = sprite7.Height / 2;
            foreach (MovingObject a in SpriteList2)
            {
                Vector2 center;
//                float new_scale = (game.CorrectX() + game.CorrectY())/2;
                center = new Vector2(half_w, half_h);
//                center = new Vector2(sprite7.Width / 2 * game.CorrectX(), sprite7.Height / 2 * game.CorrectY());
                spriteBatch.Draw(sprite7, a.position, null, Color.White, a.rotation, center, 1.0f, SpriteEffects.None, 0.0f);
//                spriteBatch.Draw(sprite7, new Vector2(a.position.X * game.CorrectX(), a.position.Y * game.CorrectY()), null, Color.White, a.rotation, center, new_scale, SpriteEffects.None, 0.0f);
            }

            // beehive
            spriteBatch.Draw(sprite6, new Vector2(max_X * 4 / 7, 2), Color.White);

            // bees
            if (!game.isGameOver())
            {
                Vector2 center;
                
                // umbrella
                if (umbrella != null)
                {
                    Rectangle r = new Rectangle(0, 0, sprite11.Width/2, sprite11.Height);
                    center = new Vector2(umb_width, umb_height);
                    spriteBatch.Draw(sprite11, umbrella.position, r, Color.White, 0.0f, center, 1.0f, SpriteEffects.None, 0.0f);
                }

                foreach (MovingObject a in SpriteList)
                {
                    switch (a.tex)
                    {
                        case 0:
                            center = new Vector2(sprite1.Width / 2, sprite1.Height / 2);
                            spriteBatch.Draw(sprite1, a.position, null, Color.White, a.rotation, center, 1.0f, SpriteEffects.None, 0.0f);
                            output = a.position.ToString();
                            FontOrigin = Font1.MeasureString(output) / 2;
                            FontPos = a.position;
                            //spriteBatch.DrawString(Font1, output, FontPos, Color.LightGreen, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                            break;
                        case 1:
                            center = new Vector2(sprite2.Width / 2, sprite2.Height / 2);
                            spriteBatch.Draw(sprite2, a.position, null, Color.White, a.rotation, center, 1.0f, SpriteEffects.None, 0.0f);
                            output = a.position.ToString();
                            FontOrigin = Font1.MeasureString(output) / 2;
                            FontPos = a.position;
                            //spriteBatch.DrawString(Font1, output, FontPos, Color.LightGreen, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                            break;
                        case 2:
                            center = new Vector2(sprite3.Width / 2, sprite3.Height / 2);
                            spriteBatch.Draw(sprite3, a.position, null, Color.White, a.rotation, center, 1.0f, SpriteEffects.None, 0.0f);
                            output = a.position.ToString();
                            FontOrigin = Font1.MeasureString(output) / 2;
                            FontPos = a.position;
                            //spriteBatch.DrawString(Font1, output, FontPos, Color.LightGreen, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                            break;
                        case 3:
                            center = new Vector2(sprite4.Width / 2, sprite4.Height / 2);
                            spriteBatch.Draw(sprite4, a.position, null, Color.White, a.rotation, center, 1.0f, SpriteEffects.None, 0.0f);
                            output = a.position.ToString();
                            FontOrigin = Font1.MeasureString(output) / 2;
                            FontPos = a.position;
                            //spriteBatch.DrawString(Font1, output, FontPos, Color.LightGreen, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                            break;
                        case 4:
                            center = new Vector2(sprite5.Width / 2, sprite5.Height / 2);
                            Color draw_color = Color.White;
                            Texture2D current_sprite = sprite5;

                            if (a.first)
                            {
                                current_sprite = sprite10;
                                if (old_position.X > a.position.X )
                                {
                                    SEVal = SpriteEffects.FlipHorizontally;
                                }
                                else
                                if (old_position.X < a.position.X )
                                {
                                    SEVal = SpriteEffects.None;
                                }

                                if (game.GetPlayerHit())
                                    draw_color = Color.Crimson;
                                else
                                    draw_color = Color.White;

                                spriteBatch.Draw(current_sprite, a.position, null, draw_color, a.rotation, center, 1.0f, SEVal, 0.0f);

                                if(pl.umbrella_active>0)
                                {
                                    Rectangle r=new Rectangle(sprite11.Width / 2, 0, sprite11.Width, sprite11.Height);;
                                    center = new Vector2(umb_width, umb_height);
                                    if(SEVal==SpriteEffects.None)
                                        spriteBatch.Draw(sprite11, new Vector2(a.position.X + 30,a.position.Y - 10), r, Color.White, 0.0f, center, 1.0f, SpriteEffects.None, 0.0f);
                                    else
                                        spriteBatch.Draw(sprite11, new Vector2(a.position.X - 30, a.position.Y - 10), r, Color.White, 0.0f, center, 1.0f, SpriteEffects.None, 0.0f);
                                }
                            }
                            else
                            if (a.speed.X < 0)
                                spriteBatch.Draw(current_sprite, a.position, null, draw_color, a.rotation, center, 1.0f, SpriteEffects.FlipHorizontally, 0.0f);
                            else
                                spriteBatch.Draw(current_sprite, a.position, null, draw_color, a.rotation, center, 1.0f, SpriteEffects.None, 0.0f);

                            /*output = a.speed.ToString();
                            FontOrigin = Font1.MeasureString(output) / 2;
                            FontPos = new Vector2(a.position.X,a.position.Y+70);
                            spriteBatch.DrawString(Font1, output, FontPos, Color.Black, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                            */
                            break;
                    }

                }
            }
            // Game Over
            else
            {
                float float_effect = 0.75f + MathHelper.Clamp((float)Math.Abs(Math.Sin((gameTime.TotalGameTime.TotalSeconds) * 4 * Math.PI / 180)), 0.0f, 1.0f);
                //float float_effect = 0.75f + (float)Math.Sin((gameTime.TotalGameTime.TotalSeconds)*180 * Math.PI / 180);
                spriteBatch.Draw(sprite9, new Vector2(max_X / 2, max_Y / 2), null, Color.White, 0.0f, new Vector2(sprite9.Width / 2, sprite9.Height / 2), float_effect, SpriteEffects.None, 1.0f);
                so.PlayGameOver(game);
            }

            if (LevelRefresh > 0)
            {
                int Count = LevelRefresh / 60 + 1;
                output = Count.ToString();

                FontOrigin = Font3.MeasureString(output) / 2;
                FontPos = new Vector2(max_X / 2, max_Y / 2);
                spriteBatch.DrawString(Font3, output, FontPos * 1.01f, Color.Black, 0, FontOrigin, 1.5f, SpriteEffects.None, 1.0f);
                spriteBatch.DrawString(Font3, output, FontPos, Color.Wheat, 0, FontOrigin, 1.5f, SpriteEffects.None, 1.0f);
            }

            if (draw_HUD)
            {
                // Draw Hello World
                double val = Math.Sin((gameTime.TotalGameTime.TotalSeconds * 180) * Math.PI / 180);
                double val2 = max_X / (float)sprite4.Height;
                FPS_Count += (double)gameTime.ElapsedGameTime.TotalSeconds;//MovingObject val3 = SpriteList.First();

                framecount++;

                // HUD background
                spriteBatch.Draw(sprite8, Vector2.Zero, Color.White);

                double FPS = framecount/FPS_Count;
                output = "FPS: " + FPS.ToString("F04");
                FontOrigin = Font1.MeasureString(output) / 2;
                FontPos = new Vector2(max_X-100, max_Y-50);
                spriteBatch.DrawString(Font1, output, FontPos * 1.002f, Color.Black, 0, FontOrigin, 1.0f, SpriteEffects.None, 1.0f);
                spriteBatch.DrawString(Font1, output, FontPos, Color.White, 0, FontOrigin, 1.0f, SpriteEffects.None, 1.0f);
                
                output = "LEVEL: " + game.GetLevel() + "     " + "BEES: " + SpriteList.Count();
                FontOrigin = Font1.MeasureString(output) / 2;
                FontPos = new Vector2(FontOrigin.X + 40, 30);
                spriteBatch.DrawString(Font1, output, new Vector2(FontPos.X * 1.02f, FontPos.Y *1.06f), Color.Black, 0, FontOrigin, 1.0f, SpriteEffects.None, 1.0f);
                spriteBatch.DrawString(Font1, output, FontPos, Color.White, 0, FontOrigin, 1.0f, SpriteEffects.None, 1.0f);

                output = "LIVES: " + game.GetLives();
                FontOrigin = Font1.MeasureString(output) / 2;
                FontPos = new Vector2(FontOrigin.X + 40, 60);
                spriteBatch.DrawString(Font1, output, FontPos * 1.04f, Color.Black, 0, FontOrigin, 1.0f, SpriteEffects.None, 1.0f);
                spriteBatch.DrawString(Font1, output, FontPos, Color.White, 0, FontOrigin, 1.0f, SpriteEffects.None, 1.0f);
                /*
                output = "SINE: " + val.ToString("F04");
                FontOrigin = Font1.MeasureString(output) / 2;
                FontPos = new Vector2(FontOrigin.X + 40, 30);
                spriteBatch.DrawString(Font1, output, FontPos * 1.04f, Color.Black, 0, FontOrigin, 1.0f, SpriteEffects.None, 1.0f);
                spriteBatch.DrawString(Font1, output, FontPos, Color.White, 0, FontOrigin, 1.0f, SpriteEffects.None, 1.0f);
                */
                output = "FLOWERS: " + game.GetFlowerCount();
                FontOrigin = Font1.MeasureString(output) / 2;
                FontPos = new Vector2(FontOrigin.X + 40, 90);
                spriteBatch.DrawString(Font1, output, FontPos * 1.03f, Color.Black, 0, FontOrigin, 1.0f, SpriteEffects.None, 1.0f);
                spriteBatch.DrawString(Font1, output, FontPos, Color.White, 0, FontOrigin, 1.0f, SpriteEffects.None, 1.0f);

                //output = "FPS: " + framecount / FPS_Count;
                output = "UMBRELLAS: " + pl.umbrella; // +" Timeleft: " + pl.umbrella_active;
                FontOrigin = Font1.MeasureString(output) / 2;
                FontPos = new Vector2(FontOrigin.X + 40, 120);
                spriteBatch.DrawString(Font1, output, FontPos * 1.02f, Color.Black, 0, FontOrigin, 1.0f, SpriteEffects.None, 1.0f);
                spriteBatch.DrawString(Font1, output, FontPos, Color.White, 0, FontOrigin, 1.0f, SpriteEffects.None, 1.0f);
            }
            spriteBatch.End();
        }

        int start_delay = 2 * 60;
        public void DoStart(Game1 game, GameTime gameTime, int max_X, int max_Y)
        {
            start_delay--;

            if (start_delay <= 0)
                this.DrawMenu(game, gameTime, max_X, max_Y);
            else
                this.DrawLoading(game, max_X, max_Y);
        }

        public void DrawLoading(Game1 game, int max_X, int max_Y)
        {
            string output;
            Vector2 FontOrigin;

            //int corrected_max_X = (int)(max_X / game.CorrectX());
            //int corrected_max_Y = (int)(max_Y / game.CorrectY());

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            output = "LOADING...";
            FontOrigin = Font3.MeasureString(output) / 2;
            FontPos = new Vector2(max_X - FontOrigin.X, (max_Y - FontOrigin.Y));
            spriteBatch.Draw(Title, new Vector2((max_X - Title.Width) / 2, (max_Y - Title.Height - 400) / 2), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
//            FontOrigin = new Vector2(FontOrigin.X * game.CorrectX(), FontOrigin.Y * game.CorrectY());
//            FontPos = new Vector2((corrected_max_X - FontOrigin.X), (corrected_max_Y - FontOrigin.Y));
//            spriteBatch.Draw(Title, new Vector2(((max_X - Title.Width) / 2) * game.CorrectX(), ((max_Y - Title.Height - 400) / 2) * game.CorrectY()), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(Font3, output, FontPos * 1.008f, Color.Black, 0, FontOrigin * 1.008f, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(Font3, output, FontPos, Color.BurlyWood, 0, FontOrigin, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.End();
        }

        int final_Y = 0;
        int current_Y = 0;
        int frames = 0;

        public void DrawMenu(Game1 game, GameTime gameTime, int max_X, int max_Y)
        {
            
            final_Y = (max_Y - MenuFrame.Height) / 2;

            current_Y = (int)((float)final_Y * curveObject.Evaluate((float)frames/75));
            frames++;

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
//            spriteBatch.Draw(Title, new Vector2(((max_X - Title.Width) / 2) * game.CorrectX(), ((max_Y - Title.Height - 400) / 2) * game.CorrectY()), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
//            spriteBatch.Draw(MenuFrame, new Vector2(((max_X - MenuFrame.Width) / 2) * game.CorrectX(), current_Y * game.CorrectY()), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(Title, new Vector2((max_X - Title.Width) / 2, (max_Y - Title.Height - 400) / 2), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(MenuFrame, new Vector2((max_X - MenuFrame.Width) / 2, current_Y), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }

    }
}
