using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameTesting
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static SpriteFont testFont;
        public static MouseState mouseState = Mouse.GetState();
        public static MouseState oldMouseState = mouseState;
        public static int mouseX;
        public static int mouseY;
        public static Sprite shopText;
        public static Sprite cursorTexture;
        public static Sprite blockTexture;
        public static Sprite charBounds;
        public static Sprite ground;
        public static Sprite background;
        public static Sprite testAlpha;
        public static Sprite textBox;
        public static Sprite levelTexture;
        public static Sprite menuGround;
        public static Sprite startTexture;
        public static Sprite stairs;
        public static KeyboardState keyBoardState = Keyboard.GetState();
        public static KeyboardState oldKeyBoardState = keyBoardState;
        public static Vector2 oldObjPos;
        public static Vector2 objPos = new Vector2(400,480);
        public static float gravity = 0.68f;
        public static Vector2 objVel = new Vector2(0,0);
        public static bool inAir = false;
        public static int dir = -1;
        public static Vector2 shopPos = new Vector2(720, 494);
        public static bool talking = false;
        public static Vector2 talkingPos;
        public static List<Sprite> collidableSprites = new List<Sprite>();
        public static List<GameButton> buttons = new List<GameButton>();
        public static List<GameButton> menuButtons = new List<GameButton>();
        public static Rectangle groundRect = new Rectangle(0, 648, 1280, 72);
        public static int jumpframes = 0;
        public static int releaseFrame = 0;
        public static Vector2 cameraPos = new Vector2(0, 0);
        public static Vector2 playerSize = new Vector2(56, 63);
        public Vector2 compA = new Vector2(0, 0);
        public static string loadStatus = "Started Program";
        public static bool editing = false;
        public static List<Texture2D> editTiles = new List<Texture2D>();
        public static List<Vector2> editPos = new List<Vector2>();
        GameButton EditButton = new GameButton(new Vector2(10, 190), ">Start Editing", true, testFont);
        public static bool inMenu = true;
        public static int menuOffset = 188;
        public static bool levelLoaded = false;
        public static bool seeCollisions = false;
        public static List<Texture2D> collisionBoxes = new List<Texture2D>();
        Texture2D cBoxT;
        public static int printW;
        public static int printH;
        GameButton Collisions;
        public static int maxDepth = 1024;
        public static Vector2 startLocation;
        public static Vector2 objStart = new Vector2(400, 480);
        GameMenu ContainerMenu = new GameMenu();
        GameMenu TalkingMenu = new GameMenu();
        public static bool jumpReleased = false;
        public static bool canDestroy = false;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
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
            Main.testFont = base.Content.Load<SpriteFont>("Font" + Path.DirectorySeparatorChar + "Mouse_Text");
            Main.cursorTexture = new Sprite((base.Content.Load<Texture2D>("Images" + Path.DirectorySeparatorChar + "Cursor")), default(Vector2), false);
            Main.blockTexture = new Sprite((base.Content.Load<Texture2D>(/*"Images" + Path.DirectorySeparatorChar + */@"CharBounds")), default(Vector2), true);
            Main.charBounds = new Sprite((base.Content.Load<Texture2D>(/*"Images" + Path.DirectorySeparatorChar + */@"CharBounds")), default(Vector2), true);
            //Main.ground = base.Content.Load<Texture2D>("Images" + Path.DirectorySeparatorChar + "Ground");
            //createCollidableAlpha(ref testAlpha, "Images" + Path.DirectorySeparatorChar + "TransTest", new Vector2(650, 360));
            createCollidableAlpha(ref ground, @"ObstacleCourse", new Vector2(-128, 616 + 64 - 1280));
            //Main.background = new Sprite((base.Content.Load<Texture2D>("Images" + Path.DirectorySeparatorChar + "Background")), default(Vector2), false);
            Main.background = new Sprite((new Texture2D(graphics.GraphicsDevice, 1280, 720)), default(Vector2), true);
            Color[] backData = new Color[1280 * 720];
            for (int i = 0; i < 1280 * 720; i++)
                backData[i] = Color.LightGray;
            Main.background.Texture.SetData<Color>(backData);
            Main.shopText = new Sprite((base.Content.Load<Texture2D>("Images" + Path.DirectorySeparatorChar + "Shopkeeper")), shopPos, true);
            Main.textBox = new Sprite((base.Content.Load<Texture2D>(@"Textbox")), default(Vector2), true);
            Main.startTexture = new Sprite((base.Content.Load<Texture2D>(@"StartImage")), default(Vector2), true);
            GameButton LoadDemo = new GameButton(new Vector2(10, 130), ">Load Demo", true, testFont);
            GameButton LevelOne = new GameButton(new Vector2(10, 150), ">Level One", true, testFont);
            GameButton StartLevelOne = new GameButton(new Vector2(10, 170), ">Save Edit", true, testFont);
            Collisions = new GameButton(new Vector2(10, 210), ">Test Destruction", true, testFont);
            GameButton stairButton = new GameButton(new Vector2(10, 230), ">Test Stairs", true, testFont);
            GameButton menuStart = new GameButton(new Vector2(640 - testFont.MeasureString("Start").X / 2, 360), "Start", true, testFont);

            Main.menuGround = new Sprite((new Texture2D(graphics.GraphicsDevice, 1280, 72)), default(Vector2), true);
            Color[] tData = new Color[1280 * 72];
            for (int i = 0; i < 1280*72; i++)
                tData[i] = Color.Green;

            EditButton.Font = testFont;
            buttons.Add(LevelOne);
            buttons.Add(StartLevelOne);
            buttons.Add(EditButton);
            buttons.Add(Collisions);
            buttons.Add(LoadDemo);
            buttons.Add(stairButton);
            menuButtons.Add(menuStart);

            cBoxT = new Texture2D(graphics.GraphicsDevice, 32, 32);
            Color[] cBoxColor = new Color[32 * 32];
            for (int i = 0; i < cBoxColor.Length; i++ )
                cBoxColor[i] = new Color(180, 80, 80, 180);
            cBoxT.SetData(cBoxColor);

            for ( int i = 0; i < 20; i++)
                ContainerMenu.Add(textBox.Texture, new Vector2( (i % 5) * 56, ( i / 5 * 56)));
            ContainerMenu.Add("Container Test", new Vector2(0, -30));
            ContainerMenu.Position = new Vector2( 640 - (5*28), 360 - (4*28));

            TalkingMenu.Add(textBox.Texture, new Vector2(0, 0));
            //Main.collidableSprites.Add(groundRect);
            //Main.collidableSprites.Add(new Rectangle(45, 360, 150, 50));
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unloade
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

            

            oldMouseState = mouseState;
            Main.mouseState = Mouse.GetState();
            Main.mouseX = Main.mouseState.X;
            Main.mouseY = Main.mouseState.Y;

            if (!inMenu)
            {
                foreach (GameButton b in buttons)
                    if (new Rectangle((int)b.Position.X, (int)b.Position.Y + 4, (int)testFont.MeasureString(b.Contents).X + (int)((b.Hover) ? 5 : 0), (int)testFont.MeasureString(b.Contents).Y - 13 + (int)((b.Hover) ? 5 : 0)).Contains(mouseX, mouseY))
                        b.Hover = true;
                    else
                        b.Hover = false;
            }
            else
            {
                foreach (GameButton b in menuButtons)
                    if (new Rectangle((int)b.Position.X, (int)b.Position.Y + 4, (int)testFont.MeasureString(b.Contents).X + (int)((b.Hover) ? 5 : 0), (int)testFont.MeasureString(b.Contents).Y - 13 + (int)((b.Hover) ? 5 : 0)).Contains(mouseX, mouseY))
                        b.Hover = true;
                    else
                        b.Hover = false;
            }
            oldKeyBoardState = keyBoardState;
            Main.keyBoardState = Keyboard.GetState();

            MovementUpdate(keyBoardState, gameTime, collidableSprites); 
            oldObjPos = objPos;
            objPos += objVel;
            cameraPos = new Vector2(objPos.X - 640 + 32, objPos.Y - 360 + playerSize.X / 2);

            if (keyBoardState.IsKeyDown(Keys.Tab) && oldKeyBoardState.IsKeyUp(Keys.Tab))
            {
                ContainerMenu.canDraw = !ContainerMenu.canDraw;
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (editing)
                {
                    float x = (mouseX + cameraPos.X) - (mouseX + cameraPos.X) % 32;
                    float y = (mouseY + cameraPos.Y) - (mouseY + cameraPos.Y) % 32;
                    if ((mouseX + cameraPos.X) < 0) x -= 32;
                    if ((mouseY + cameraPos.Y) < 0) y -= 32;
                    if (!editPos.Exists((Vector2 v) => { return ((v.X == x) && (v.Y == y)); }))
                    {
                        editPos.Add(new Vector2(x, y));
                        Texture2D t = new Texture2D(graphics.GraphicsDevice, 32, 32);
                        Color[] tData = new Color[32 * 32];
                        for (int i = 0; i < 32 * 32; i++)
                            tData[i] = Color.White;
                        t.SetData(tData);
                        editTiles.Add(t);

                        collidableSprites.Add(new Sprite(t, new Vector2(x, y), true));
                    }
                }
                if (oldMouseState.LeftButton == ButtonState.Released)
                {
                    if (!inMenu)
                    {
                        foreach (GameButton b in buttons)
                            if (b.Hover)
                                switch (b.Contents)
                                {
                                    case ">Level One":
                                        loadLevel(ref levelTexture, "Content" + Path.DirectorySeparatorChar + "Levels" + Path.DirectorySeparatorChar + "LevelOne.lvl");
                                        break;
                                    case ">Load Demo":
                                        collidableSprites = new List<Sprite>();
                                        loadLevel(ref levelTexture, "Content" + Path.DirectorySeparatorChar + "Levels" + Path.DirectorySeparatorChar + "LevelDemo.lvl");
                                        editTiles = new List<Texture2D>();
                                        editPos = new List<Vector2>();
                                        break;
                                    case ">Save Edit":
                                        //createLevel(ground, "Content" + Path.DirectorySeparatorChar + "Levels" + Path.DirectorySeparatorChar + "LevelOne.lvl", "Level One", 400, 0);
                                        saveEdit("Content" + Path.DirectorySeparatorChar + "Levels" + Path.DirectorySeparatorChar + "LevelOne.lvl", "Level One");
                                        break;
                                    case ">Start Editing":
                                        editing = true;
                                        EditButton.Contents = ">Stop Editing";
                                        break;
                                    case ">Stop Editing":
                                        editing = false;
                                        EditButton.Contents = ">Start Editing";
                                        break;
                                    case ">See Collisions":
                                        seeCollisions = true;
                                        Collisions.Contents = ">Stop Collisions";
                                        break;
                                    case ">Stop Collisions":
                                        seeCollisions = false;
                                        Collisions.Contents = ">See Collisions";
                                        break;
                                    case ">Test Destruction":
                                        collidableSprites = new List<Sprite>();
                                        createCollidableAlpha(ref ground, @"Destroy", new Vector2(0, 616));
                                        //loadLevel(ref levelTexture, "Content" + Path.DirectorySeparatorChar + "Levels" + Path.DirectorySeparatorChar + "DestructDemo.lvl");
                                        canDestroy = true;
                                        objPos = startLocation;
                                        break;
                                    case ">Test Stairs":
                                        createCollidableAlpha(ref stairs, @"Stairs", new Vector2(ground.Texture.Width/2, 616 - 128 - 128 + 32));
                                        break;
                                    default: break;
                                }
                    }
                    else
                    {
                        foreach (GameButton b in menuButtons)
                            if (b.Hover)
                                switch (b.Contents)
                                {
                                    case "Start":
                                        inMenu = false;
                                        break;
                                    default:
                                        break;
                                }
                    }
                    if ((new Rectangle((int)(shopPos.X - cameraPos.X), (int)(shopPos.Y - cameraPos.Y), 138, 166)).Contains(mouseX, mouseY))
                    {
                        if (!talking)
                        {
                            talking = true;
                            talkingPos = shopPos;
                            TalkingMenu.Position = shopPos - new Vector2(0, 50);
                            TalkingMenu.Add("Menu text.", new Vector2(20, 10));
                        }
                        else
                        {
                            talking = false;
                            TalkingMenu.RemoveAllStrings();
                        }
                    }
                }
            }

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                if (editing)
                {
                    float x = (mouseX + cameraPos.X) - (mouseX + cameraPos.X) % 32;
                    float y = (mouseY + cameraPos.Y) - (mouseY + cameraPos.Y) % 32;
                    if ((mouseX + cameraPos.X) < 0) x -= 32;
                    if ((mouseY + cameraPos.Y) < 0) y -= 32;

                    int count = editPos.Count;

                    editPos.Remove(editPos.Find((Vector2 v) => { return ((v.X == x) && (v.Y == y)); }));
                    if (count == editPos.Count + 1)
                    {
                        editTiles.Remove(editTiles.Find((Texture2D t) => { return true; }));
                        collidableSprites.Remove(collidableSprites.Find((Sprite r) => { return ((r.Rectangle.X == x) && (r.Rectangle.Y == y)); }));
                    }
                }
                else if (canDestroy && oldMouseState.RightButton == ButtonState.Released)
                {
                    int rad = 64;
                    List<Sprite> destroyList = new List<Sprite>();
                    for (int i = (int)(mouseX + cameraPos.X - rad); i < (int)(mouseX + cameraPos.X + 2*rad); i += 16)
                        for (int j = (int)(mouseY + cameraPos.Y - rad); j < (int)(mouseY + cameraPos.Y +2*rad); j += 16)
                        {
                            Sprite sp = collidableSprites.Find((Sprite s) => { return ModRect.toRect(s.Rectangle).Contains(i, j); });
                            if (sp != null && !destroyList.Contains(sp))
                                destroyList.Add(sp);
                        }
                    
                    loadStatus = "Number of sprites destroyed: " + destroyList.Count;
                    foreach( Sprite s in destroyList)
                        if (s != null)
                            s.DestroyAt((int)(mouseX + cameraPos.X), (int)(mouseY + cameraPos.Y), rad);
                }
            }

            if (mouseState.MiddleButton == ButtonState.Pressed)
            {
                if (oldMouseState.MiddleButton == ButtonState.Released)
                {
                    if ( editing )
                        startLocation = new Vector2(mouseX + cameraPos.X, mouseY + cameraPos.Y);
                }
            }

            if (Math.Sqrt((objPos.X - talkingPos.X) * (objPos.X - talkingPos.X) + (objPos.Y - talkingPos.Y) * (objPos.Y - talkingPos.Y)) > 1000)
                talking = false;
            // TODO: Add your update logic here

            if ( talking && collidableSprites.Count > 0)
            {
                Sprite rect = collidableSprites.Find((Sprite r) => { return ((r.Rectangle.X == 650) && (r.Rectangle.Y == 360)); });
                if (rect != null) rect.Rectangle.passable = 0xA;
            }
            else if (!talking && collidableSprites.Count > 0)
            {
                Sprite rect = collidableSprites.Find((Sprite r) => { return ((r.Rectangle.X == 650) && (r.Rectangle.Y == 360)); });
                if (rect != null) rect.Rectangle.passable = 0x0;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            this.spriteBatch.Draw(background.Texture, new Rectangle(0, 0, 1280, 720), Color.White);
            //this.spriteBatch.Draw(ground, new Rectangle(0-(int)cameraPos.X, 648-(int)cameraPos.Y, 1280, 72), new Rectangle?(new Rectangle(0, 648, 1280, 72)), new Color(0f, 180f, 0f));
            //if (!levelLoaded)
            //    DrawCamera(ground.Texture, new Vector2(0, 616), Color.White);
            //else
            //    DrawCamera(levelTexture.Texture, new Vector2(0, 616), Color.White);
            //DrawCamera((levelLoaded) ? levelTexture : ground, Color.White);
            foreach (Sprite s in collidableSprites)
                DrawCamera(s, Color.White);
            for (int i = 0; i < editTiles.Count; i++)
                DrawCamera(editTiles[i], editPos[i], Color.White);
            if (startLocation != default(Vector2))
                DrawCamera(startTexture.Texture, new Vector2(startLocation.X - 32, startLocation.Y - 128), Color.White);
            //if (seeCollisions)
            //    foreach (Sprite r in collidableSprites)
            //        DrawCamera(r, Color.White);
            //this.spriteBatch.Draw(ground, new Rectangle(500-(int)cameraPos.X, 250-(int)cameraPos.Y, 150, 50), null, Color.Green, 1.57f, default(Vector2), SpriteEffects.None, 0f);
            //DrawCamera(ground, new Rectangle(45, 360, 150, 50), Color.Green);
            //DrawCamera(testAlpha, new Vector2(650, 360), Color.White);
            //this.spriteBatch.Draw(shopText, shopPos - cameraPos, Color.White);
            DrawCamera(shopText, Color.White);
            this.spriteBatch.DrawString(testFont, "v0.0.1.2", new Vector2(1280 - 80, 10), Color.White);
            DrawCamera(charBounds.Texture, new Vector2((int)objPos.X, (int)(objPos.Y + ((inMenu) ? menuOffset : 0))), Color.White);
            this.spriteBatch.Draw(blockTexture.Texture, new Rectangle((int)(objPos.X - cameraPos.X), (int)(objPos.Y - cameraPos.Y + ((inMenu) ? menuOffset : 0)), (int)playerSize.X, (int)playerSize.Y), null, Color.White, 0f, new Vector2(0, 0), (dir == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            
            Texture2D t = new Texture2D(graphics.GraphicsDevice,2,2);
            Color[] tC = new Color[4];
            tC[0] = tC[1] = tC[2] = tC[3] = Color.White;
            t.SetData(tC);
            DrawCamera(t, new Vector2(objPos.X + playerSize.X / 2 + objVel.X + ((dir == -1) ? -24 : 24), objPos.Y + playerSize.Y - 2), Color.White);

            if (!inMenu)
            {
                this.spriteBatch.DrawString(testFont, "GameTesting", new Vector2(10f, 10f), new Color(255f, 255f, 255f));
                this.spriteBatch.DrawString(testFont, "Cursor at X: " + Main.mouseX + ", Y: " + Main.mouseY, new Vector2(10f, 30f), new Color(255f, 255f, 255f));
                this.spriteBatch.DrawString(testFont, "Current Tile Coord X: " + ((mouseX + cameraPos.X) - (mouseX + cameraPos.X) % 32) + ", Y: " + ((mouseY + cameraPos.Y) - (mouseY + cameraPos.Y) % 32), new Vector2(10f, 50f), new Color(255f, 255f, 255f));
                this.spriteBatch.DrawString(testFont, "inAir: " + inAir, new Vector2(10f, 70f), new Color(255f, 255f, 255f));
                this.spriteBatch.DrawString(testFont, "Object at X: " + Main.objPos.X + ", Y: " + Main.objPos.Y, new Vector2(10f, 90f), new Color(255f, 255f, 255f));
                this.spriteBatch.DrawString(testFont, loadStatus, new Vector2(10, 720 - 30), Color.White, 0f, default(Vector2), 0.8f, SpriteEffects.None, 0f);

                foreach (GameButton b in buttons)
                    if (b.Enabled)
                        this.spriteBatch.DrawString(b.Font, b.Contents, b.Position, Color.White, 0f, default(Vector2), (b.Hover) ? 1.1f : 1f, SpriteEffects.None, 0f);

                if (talking)
                {
                    List<Sprite> gr = TalkingMenu.getGraphics();
                    List<String> str = TalkingMenu.getStrings();
                    List<Vector2> strpos = TalkingMenu.getStringPos();
                    for (int i = 0; i < gr.Count(); i++)
                        this.spriteBatch.Draw(gr[i].Texture, TalkingMenu.Position + gr[i].Position - cameraPos, Color.White);
                    for (int i = 0; i < str.Count(); i++)
                        this.spriteBatch.DrawString(testFont, str[i], TalkingMenu.Position + strpos[i] - cameraPos, Color.Green);
                }

                if (ContainerMenu.canDraw)
                {
                    List<Sprite> gr = ContainerMenu.getGraphics();
                    List<String> str = ContainerMenu.getStrings();
                    List<Vector2> strpos = ContainerMenu.getStringPos();
                    for (int i = 0; i < gr.Count(); i++)
                        this.spriteBatch.Draw(gr[i].Texture, new Rectangle((int)(ContainerMenu.Position.X + gr[i].Position.X), (int)(ContainerMenu.Position.Y + gr[i].Position.Y), 48, 48), null, Color.White);
                    for (int i = 0; i < str.Count(); i++)
                        this.spriteBatch.DrawString(testFont, str[i], ContainerMenu.Position + strpos[i], Color.White);
                }
            }
            else
            {
                this.spriteBatch.DrawString(testFont, "THIS IS GAME", new Vector2(640 - testFont.MeasureString("THIS IS GAME").X / 2 * 5, 30), Color.White, 0f, default(Vector2), 5f, SpriteEffects.None, 0f);
                foreach (GameButton b in menuButtons)
                    if (b.Enabled)
                        this.spriteBatch.DrawString(b.Font, b.Contents, b.Position, Color.White, 0f, default(Vector2), (b.Hover) ? 1.1f : 1f, SpriteEffects.None, 0f);

            }
            //this.spriteBatch.DrawString(testFont, "# of Collidables: " + collidableSprites.Count, new Vector2(10f, 90f), new Color(255f, 255f, 255f));
            this.spriteBatch.Draw(cursorTexture.Texture, new Vector2(Main.mouseX, Main.mouseY), new Color(200f, 0f, 0f));
            spriteBatch.End();
            // TODO: Add your drawing code here
            base.Draw(gameTime);

        }

        private void DrawCamera(Sprite s, Color c)
        {
            this.spriteBatch.Draw(s.Texture, new Rectangle((int)(s.Position.X - cameraPos.X), (int)(s.Position.Y - cameraPos.Y) + ((inMenu) ? menuOffset : 0), s.Texture.Width, s.Texture.Height), c);
        }

        private void DrawCamera(Texture2D t, Rectangle r, Color c)
        {
            this.spriteBatch.Draw(t, new Rectangle(r.Location.X - (int)cameraPos.X, r.Location.Y - (int)cameraPos.Y + ((inMenu) ? menuOffset:0), r.Width, r.Height), c);
        }

        private void DrawCamera(Texture2D t, Rectangle r, Rectangle? sR, Color c, SpriteEffects sE)
        {
            this.spriteBatch.Draw(t, new Rectangle(r.Location.X - (int)cameraPos.X, r.Location.Y - (int)cameraPos.Y + ((inMenu) ? menuOffset : 0), r.Width, r.Height), sR, c, 0f, default(Vector2), sE, 0f);
        }

        private void DrawCamera(Texture2D t, Vector2 v, Color c)
        {
            this.spriteBatch.Draw(t, new Vector2(v.X - cameraPos.X,v.Y - cameraPos.Y + ((inMenu) ? menuOffset:0)), c);
        }

        private void MovementUpdate(KeyboardState ks, GameTime gametime, List<Sprite> rList)
        {
            Vector2 upcomingPos = new Vector2();
            if (!inAir)
            {
                if (!(keyBoardState.IsKeyDown(Keys.A) && keyBoardState.IsKeyDown(Keys.D)))
                {
                    if (keyBoardState.IsKeyDown(Keys.A))
                    {
                        objVel.X = -7;
                    }
                    if (keyBoardState.IsKeyDown(Keys.D))
                    {
                        objVel.X = 7;
                    }
                }
                else if (keyBoardState.IsKeyDown(Keys.A) && keyBoardState.IsKeyDown(Keys.D))
                {
                    objVel.X = 0;
                }
                if (keyBoardState.IsKeyUp(Keys.A) && keyBoardState.IsKeyUp(Keys.D))
                    objVel.X = 0;
            }
            else
            {
                if (!(keyBoardState.IsKeyDown(Keys.A) && keyBoardState.IsKeyDown(Keys.D)))
                {
                    if (keyBoardState.IsKeyDown(Keys.A))
                    {
                        objVel.X -= 1f;
                    }
                    else if (keyBoardState.IsKeyDown(Keys.D))
                    {
                        objVel.X += 1f;
                    }
                    if (objVel.X > 7) objVel.X = 7;
                    if (objVel.X < -7) objVel.X = -7;
                }

                if (keyBoardState.IsKeyUp(Keys.A) && keyBoardState.IsKeyUp(Keys.D))
                    objVel.X *= 0.9f;
            }

            if (objVel.X > 0)
                dir = 1;
            else if (objVel.X < 0)
                dir = -1;

            if (keyBoardState.IsKeyUp(Keys.Space))
            {
                if ( !inAir )
                    jumpReleased = true;
                if (jumpframes > 4)
                {
                    objVel.Y *= (float) 4 / jumpframes;
                    releaseFrame = jumpframes;
                }
                jumpframes = 0;
            }
            else
            {
                if (inAir)
                {
                    jumpframes--;
                    if (keyBoardState.IsKeyDown(Keys.D) && oldKeyBoardState.IsKeyUp(Keys.Space))
                    {
                        foreach( Sprite r in collidableSprites)
                            if (new Rectangle((int)objPos.X + 5, (int)objPos.Y, (int)playerSize.X, (int)playerSize.Y).Intersects(ModRect.toRect(r.Rectangle)))
                            {
                                objVel.X += -7;
                                objVel.Y = -14;
                            }
                    }
                    else if (keyBoardState.IsKeyDown(Keys.A) && oldKeyBoardState.IsKeyUp(Keys.Space))
                    {
                        foreach (Sprite r in collidableSprites)
                            if (new Rectangle((int)(objPos.X - 5), (int)objPos.Y, (int)playerSize.X, (int)playerSize.Y).Intersects(ModRect.toRect(r.Rectangle)))
                            {
                                objVel.X += 7;
                                objVel.Y = -14;
                            }
                    }
                }
                else
                {
                    if (jumpReleased)
                    {
                        objVel.Y -= 17;
                        inAir = true;
                        jumpframes = 19;
                        jumpReleased = false;
                    }
                }
            }
            bool collided = false;
            foreach (Sprite r in collidableSprites)
                if (new Rectangle((int)(objPos.X - 5), (int)objPos.Y, (int)playerSize.X + 10, (int)playerSize.Y).Intersects(ModRect.toRect(r.Rectangle)))
                {
                    collided = true;
                    break;
                }
            if (collided && objVel.Y > 0)
                objVel.Y += gravity / 4;
            else
                objVel.Y += gravity;

            upcomingPos = objPos + objVel;
            Vector2 firstFoot = new Vector2(objPos.X + playerSize.X / 2 + objVel.X + ((dir == -1) ? -24:22), objPos.Y + playerSize.Y - 2);
            Vector2 upcomingPosX = new Vector2(objPos.X + objVel.X, objPos.Y);
            Vector2 upcomingPosY = new Vector2(objPos.X, objPos.Y + objVel.Y);
            //Rectangle newPosRect = new Rectangle((int)upcomingPos.X, (int)upcomingPos.Y, (int)playerSize.X, (int)playerSize.Y);
            Rectangle newPosRectX = new Rectangle((int)upcomingPos.X, (int)objPos.Y, (int)playerSize.X, (int)playerSize.Y);
            Rectangle newPosRectY = new Rectangle((int)objPos.X, (int)upcomingPos.Y, (int)playerSize.X, (int)playerSize.Y);
            foreach (Sprite r in rList)
            {
                if (newPosRectX.Intersects(ModRect.toRect(r.Rectangle)))
                {
                        bool blocked = false;
                        Color[] tData = new Color[r.pixelCount];
                        r.Texture.GetData(tData);
                        for (int i = (int)(firstFoot.Y - playerSize.Y); i < (int)firstFoot.Y - 12 && !blocked; i++)
                            if (ModRect.toRect(r.Rectangle).Contains((int)firstFoot.X, (int)i) && tData[(int)(firstFoot.X - r.Position.X) + (int)(i - r.Position.Y) * r.Texture.Width].A != 0)
                            {
                                objVel.X = 0;
                                blocked = true;
                            }
                        if (ModRect.toRect(r.Rectangle).Contains((int)firstFoot.X, (int)firstFoot.Y) && !blocked)
                        {
                            if (PerPixelCollision(r, new Sprite(charBounds.Texture, upcomingPosX, true)))
                            {
                                Sprite tempSprite = r;
                                Color[] wData = new Color[tempSprite.pixelCount];
                                tempSprite.Texture.GetData(wData);
                                for (int i = 0; i < tempSprite.pixelCount; i++)
                                    wData[i] = (wData[i].A != 0) ? Color.Red : Color.Transparent;
                                tempSprite.Texture.SetData(wData);
                                if (wData[(int)(firstFoot.X - r.Position.X) + (int)(firstFoot.Y - r.Position.Y) * r.Texture.Width].A != 0)
                                {
                                    int teleUp = 1;
                                    while (tempSprite != null && teleUp < 12 && ((int)(firstFoot.Y - teleUp) >= tempSprite.Position.Y) && wData[(int)(firstFoot.X - tempSprite.Position.X) + (int)(firstFoot.Y - tempSprite.Position.Y - teleUp) * tempSprite.Texture.Width].A != 0)
                                    {
                                        teleUp++;
                                        if ((int)(firstFoot.Y - teleUp) < tempSprite.Position.Y)
                                        {
                                            tempSprite = collidableSprites.Find((Sprite s) => { return ModRect.toRect(s.Rectangle).Contains((int)firstFoot.X, (int)firstFoot.Y - teleUp); });
                                            if (tempSprite != null)
                                                tempSprite.Texture.GetData(wData);
                                        }
                                    }
                                    if (teleUp < 12)
                                    {
                                        firstFoot.Y -= teleUp;
                                        Vector2 tempPos = new Vector2(firstFoot.X - playerSize.X / 2 - ((dir == -1) ? -24 : 22), firstFoot.Y - playerSize.Y);
                                        if (!PerPixelCollision(tempSprite, new Sprite(charBounds.Texture, tempPos, true)))
                                            objPos = tempPos;

                                    }
                                    objVel.X = 0;

                                    if (gametime.TotalGameTime.Milliseconds % 1000 == 0)
                                        loadStatus = "teleUp: " + teleUp;

                                }
                            }
                        }
                }
                /*
                if (newPosRectX.Intersects(ModRect.toRect(r.Rectangle)) )
                {
                    if (PerPixelCollision(r, new Sprite(charBounds.Texture, upcomingPosX, true)))
                    {
                        int teleUp = 0;
                        Color[] wData = new Color[r.pixelCount];
                        r.Texture.GetData(wData);
                        while (((int)(firstFoot.Y) < r.Position.Y + r.Texture.Height) && ((int)(firstFoot.X) < r.Position.X + r.Texture.Width) && ((int)(firstFoot.Y - teleUp) > r.Position.Y) && wData[(int)(firstFoot.X - r.Position.X) + (int)(firstFoot.Y - r.Position.Y - teleUp) * r.Texture.Width].A != 0)
                            teleUp++;
                        if (teleUp < 9 && teleUp != 0)
                        {
                            firstFoot.Y -= teleUp;
                            objPos = firstFoot;
                            objVel.X = 0;
                        }
                        else
                            objVel.X = 0;
                    }
                    else
                        objVel.X = 0;
                    //compA.X = r.Rectangle.X - objPos.X + ((r.Rectangle.X > objPos.X) ? -playerSize.X : r.Rectangle.Width);
                    //if (objVel.X > 0 && !getFlag(r.Rectangle.passable, 3))
                    //    objVel.X = r.Rectangle.X - objPos.X + ((r.Rectangle.X > objPos.X) ? -playerSize.X : r.Rectangle.Width);
                    //else if (objVel.X < 0 && !getFlag(r.Rectangle.passable, 1))
                    //    objVel.X = r.Rectangle.X - objPos.X + ((r.Rectangle.X > objPos.X) ? -playerSize.X : r.Rectangle.Width);
                    //if (Math.Abs(objVel.X) < 1) objVel.X = 0;
                }*/
                if (newPosRectY.Intersects(ModRect.toRect(r.Rectangle)))
                {
                    objVel.Y = (PerPixelCollision(r, new Sprite(charBounds.Texture, upcomingPosY, true))) ? 0 : objVel.Y;
                    if (r.Rectangle.Y > newPosRectY.Location.Y)
                        inAir = false;
                    //compA.Y = r.Rectangle.Y - objPos.Y + ((r.Rectangle.Y > objPos.Y) ? -playerSize.Y : r.Rectangle.Height);
                    //if (objVel.Y > 0 && !getFlag(r.Rectangle.passable, 0))
                    //    objVel.Y = r.Rectangle.Y - objPos.Y + ((r.Rectangle.Y > objPos.Y) ? -playerSize.Y : r.Rectangle.Height);
                    //if (objVel.Y < 0 && !getFlag(r.Rectangle.passable, 2))
                    //    objVel.Y = r.Rectangle.Y - objPos.Y + ((r.Rectangle.Y > objPos.Y) ? -playerSize.Y : r.Rectangle.Height);
                    //if (Math.Abs(objVel.Y) < 1) objVel.Y = 0;
                }
                else
                    if (objVel.Y > gravity)
                    {
                        inAir = true;
                        jumpReleased = false;
                    }
            }

            if (objPos.Y > maxDepth)
            {
                objPos = objStart;
                objVel = default(Vector2);
            }
        }

        public bool PerPixelCollision(Sprite a, Sprite b)
        {
            if (a == null || b == null)
                return false;
            // Get Color data of each Texture
            Color[] bitsA = new Color[a.Texture.Width * a.Texture.Height];
            a.Texture.GetData(bitsA);
            Color[] bitsB = new Color[b.Texture.Width * b.Texture.Height];
            b.Texture.GetData(bitsB);

            // Calculate the intersecting rectangle
            int x1 = Math.Max(a.Rectangle.X, b.Rectangle.X);
            int x2 = Math.Min(a.Rectangle.X + a.Rectangle.Width, b.Rectangle.X + b.Rectangle.Width);

            int y1 = Math.Max(a.Rectangle.Y, b.Rectangle.Y);
            int y2 = Math.Min(a.Rectangle.Y + a.Rectangle.Height, b.Rectangle.Y + b.Rectangle.Height);

            // For each single pixel in the intersecting rectangle
            for (int y = y1; y < y2; ++y)
            {
                for (int x = x1; x < x2; ++x)
                {
                    // Get the color from each texture
                    Color aC = bitsA[(x - a.Rectangle.X) + (y - a.Rectangle.Y) * a.Texture.Width];
                    Color bC = bitsB[(x - b.Rectangle.X) + (y - b.Rectangle.Y) * b.Texture.Width];

                    if (aC.A != 0 && bC.A != 0) // If both colors are not transparent (the alpha channel is not 0), then there is a collision
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void createCollidableAlpha(ref Sprite s, string loc, Vector2 pos, byte passable = 0x0)
        {
            if ( loc != null )
                s = new Sprite((base.Content.Load<Texture2D>(loc)), pos, true);

            Texture2D t = s.Texture;
            Color[] tColors = new Color[t.Height * t.Width];
            t.GetData<Color>(tColors);
            bool hasPixels = false;
            for (int i = 0; i < (t.Width / 32); i++)
                for (int j = 0; j < (t.Height / 32); j++)
                {
                    hasPixels = false;
                    for (int k = 32 * i; k < 32 * i + 32 && !hasPixels; k++)
                        for (int l = 32 * j; l < 32 * j + 32 && !hasPixels; l++)
                            if (tColors[k + l * t.Width].A != 0)
                                hasPixels = true;
                    if (hasPixels)
                    {
                        Color[] tempBox = new Color[32 * 32];
                        Texture2D tempText = new Texture2D(graphics.GraphicsDevice, 32, 32);
                        //for (int m = 0; m < 32 * 32; m++)
                        //    tempBox[m] = Color.White;
                        for (int k = 32 * i; k < 32 * i + 32; k++)
                            for (int l = 32 * j; l < 32 * j + 32; l++)
                                tempBox[(k - 32 * i) + (l - 32 * j) * 32] = tColors[k + l * t.Width];

                        tempText.SetData<Color>(tempBox);

                        collidableSprites.Add(new Sprite(tempText, new Vector2((int)pos.X + 32 * i, (int)pos.Y + 32 * j), true));
                    }
                }
        }

        private bool getFlag(byte boolByte, int pos)
        {
            return ((boolByte >> pos) & 0x1) == 1;
        }

        private void loadLevel(ref Sprite sprite, string loc)
        {
            try
            {
                FileStream level = File.Open(loc, FileMode.Open);
                BinaryReader br = new BinaryReader(level);

                int nameSize = br.ReadByte();
                string name = String.Empty;
                for (int i = 0; i < nameSize; i++)
                    name += (char)br.ReadByte();

                int sizeX = br.ReadInt16();
                int sizeY = br.ReadInt16();
                Color[] textureA = new Color[sizeX * sizeY];
                for (int i = 0; i < sizeX; i++)
                    for (int j = 0; j < sizeY; j++)
                    {
                        textureA[i + j * sizeX] = new Color(br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte());
                        textureA[i + j * sizeX].A *= 2;
                    }
                Texture2D texture = new Texture2D(graphics.GraphicsDevice, sizeX, sizeY);
                texture.SetData<Color>(textureA);
                sprite = new Sprite(texture, new Vector2(0,616), true);

                objPos.X = br.ReadInt16();
                objPos.Y = br.ReadInt16() + 616;
                objStart = new Vector2(objPos.X, objPos.Y);

                br.Close();
                level.Close();

                collidableSprites = new List<Sprite>();
                editTiles = new List<Texture2D>();
                editPos = new List<Vector2>();
                startLocation = default(Vector2);

                createCollidableAlpha(ref sprite, null, new Vector2(0, 616));
                loadStatus = "Loaded " + name;

                maxDepth = 616 + levelTexture.Texture.Height - 128;
                levelLoaded = true;
            }
            catch (FileNotFoundException e)
            {
                loadStatus = "File " + loc + " not found.";
            }
            catch (Exception e)
            {
                loadStatus = "There was an error loading file";
            }

        }

        private void createLevel(Texture2D t, string loc, string name, int startX, int startY)
        {
            FileStream newLevel = File.Create(loc);
            BinaryWriter bw = new BinaryWriter(newLevel);
            
            bw.Write((byte)name.Length);
            bw.Write(name.ToCharArray());
            
            bw.Write((short)t.Width);
            bw.Write((short)t.Height);
            
            Color[] tData = new Color[t.Width * t.Height];
            t.GetData(tData);
            for (int i = 0; i < t.Width; i++)
                for (int j = 0; j < t.Height; j++)
                {
                    bw.Write((byte)tData[i + j * t.Width].R);
                    bw.Write((byte)tData[i + j * t.Width].G);
                    bw.Write((byte)tData[i + j * t.Width].B);
                    tData[i + j * t.Height].A /= 2;
                    bw.Write((byte)tData[i + j * t.Width].A);
                }

            bw.Write((short) startX);
            bw.Write((short) startY);

            bw.Close();
            newLevel.Close();
            loadStatus = "File create at " + loc;
        }

        private void saveEdit(string loc, string name)
        {
            if (editPos.Count > 0)
            {
                if (startLocation != default(Vector2))
                {
                    FileStream newLevel = File.Create(loc);
                    BinaryWriter bw = new BinaryWriter(newLevel);

                    bw.Write((byte)name.Length);
                    bw.Write(name.ToCharArray());

                    int minX = (int)editPos[0].X;
                    int minY = (int)editPos[0].Y;
                    int maxX = minX;
                    int maxY = minY;
                    foreach (Vector2 r in editPos)
                    {
                        minX = (int)Math.Min(minX, r.X);
                        minY = (int)Math.Min(minY, r.Y);
                        maxX = (int)Math.Max(maxX, r.X);
                        maxY = (int)Math.Max(maxY, r.Y);
                    }
                    int w = maxX - minX + 32;
                    int h = maxY - minY + 32;
                    printW = w;
                    printH = h;

                    bw.Write((short)w);
                    bw.Write((short)h);

                    Texture2D t = new Texture2D(graphics.GraphicsDevice, w, h);
                    Color[] tData = new Color[w * h];
                    foreach (Vector2 r in editPos)
                    {
                        for (int j = (int)r.Y; j < (int)(r.Y + 32); j++)
                            for (int i = (int)r.X; i < (int)(r.X + 32); i++)
                                tData[(i - minX) * h + (j - minY)] = Color.White;
                    }

                    for (int i = 0; i < tData.Length; i++)
                    {
                        bw.Write((byte)tData[i].R);
                        bw.Write((byte)tData[i].G);
                        bw.Write((byte)tData[i].B);
                        tData[i].A /= 2;
                        bw.Write((byte)tData[i].A);
                    }

                    bw.Write((short)(startLocation.X - minX));
                    bw.Write((short)(startLocation.Y - minY));

                    bw.Close();
                    newLevel.Close();
                    loadStatus = "Edit saved at " + loc;
                }
                else
                    loadStatus = "No start location specified. Use Middle Button to create a startPoint.";
            }
            else
                loadStatus = "No edits were created. Save aborted.";
        }
    }
}
