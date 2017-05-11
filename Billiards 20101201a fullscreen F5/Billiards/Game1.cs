// (c) Copyright 2010 Cody Neuburger
//        All rights reserved.
//Avatar models were created from World of Warcraft by Blizzard Entertainment
//using WoW Model Viewer
//I am in no way affiliated with Blizzard
//Some sounds are from http://www.freesound.org

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
using XNAGifAnimationLibrary;

namespace Billiards
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region variables
        SpriteBatch spriteBatch;

        public const int STATE_WELCOME = 0;
        public const int STATE_W_FULL = 34;
        public const int STATE_TITLE = 1;
        public const int STATE_MENU = 2;
        public const int STATE_HIGHSCORES = 3;
        public const int STATE_INSTRUCTIONS0 = 4;
        public const int STATE_INSTRUCTIONS1 = 17;
        public const int STATE_GAMESTART = 5;
        public const int STATE_OPTIONS = 6;
        public const int STATE_GAMEPLAY = 7;
        public const int STATE_G_FULL = 35;
        public const int STATE_PAUSING = 8;
        public const int STATE_PAUSE = 9;
        public const int STATE_UNPAUSING = 10;
        public const int STATE_WIN = 11;
        public const int STATE_LOSE = 12;
        public const int STATE_STATS = 13;
        public const int STATE_NEWHIGHSCORE = 14;
        public const int STATE_GAMEOVER = 15;
        public const int STATE_AUTO = 16;
        public const int STATE_TEST_1 = 18;
        public const int STATE_TEST_2 = 19;
        public const int STATE_TEST_2_aUp = 20;
        public const int STATE_TEST_2_aDown = 21;
        public const int STATE_TEST_3 = 22;
        public const int STATE_TEST_3_CLEAR = 23;
        public const int STATE_TEST_3_1 = 24;
        public const int STATE_TEST_3_N = 33;
        public const int STATE_TEST_4 = 25;
        public const int STATE_TEST_4_CLEAR = 30;
        public const int STATE_TEST_4_aUp = 26;
        public const int STATE_TEST_4_aDown = 27;
        public const int STATE_TEST_4_1 = 31;
        public const int STATE_TEST_4_N = 32;
        public const int STATE_TEST_5_1 = 28;
        public const int STATE_TEST_5_2 = 29;
        public int state = 0;
        int stateframes = 0;

        public Line line1;

        public Vector2 Pocket1 = new Vector2(1420, 60);
        public Vector2 Pocket2 = new Vector2(84, 84);
        public Vector2 Pocket3 = new Vector2(84, 1335);
        public Vector2 Pocket4 = new Vector2(1420, 1360);
        public Vector2 Pocket5 = new Vector2(2756, 1335);
        public Vector2 Pocket6 = new Vector2(2756, 84);
        public Vector2 oldPocket1 = new Vector2(1420, 60);
        public Vector2 oldPocket2 = new Vector2(84, 84);
        public Vector2 oldPocket3 = new Vector2(84, 1335);
        public Vector2 oldPocket4 = new Vector2(1420, 1360);
        public Vector2 oldPocket5 = new Vector2(2756, 1335);
        public Vector2 oldPocket6 = new Vector2(2756, 84);
        public List<Vector2> pockets = new List<Vector2>();

        public ListOfWalls walls;
        public PoolOfParticles particlePool;
        public BackGround backGround;
        public Table Table;
        public Player player1;
        public SpriteObject AimStick;
        public SpriteObject Crosshair;
        public BulletPool bulletPool;
        public PoolOfBalls ballPool;
        public ListOfCollidables collidables;
        public BilliardBall cueball;
        public BilliardBall ball1;
        public BilliardBall ball2;
        public BilliardBall ball3;
        public BilliardBall ball4;
        public BilliardBall ball5;
        public BilliardBall ball6;
        public BilliardBall ball7;
        public BilliardBall ball8;
        public BilliardBall ball9;
        public BilliardBall ball10;
        public BilliardBall ball11;
        public BilliardBall ball12;
        public BilliardBall ball13;
        public BilliardBall ball14;
        public BilliardBall ball15;
        BilliardBall TESTball1;
        BilliardBall TESTball2;

        public SpriteObject screenFiller;

        Wall TESTwall1;
        Wall TESTwall2;
        Wall TESTwall3;
        Wall TESTwall4;
        Wall TESTwall5;
        Wall TESTwall6;
        Wall TESTwall7;
        Wall TESTwall8;
        Wall TESTwall9;

        //instructions
        AnimatedObject runner;
        AnimatedObject shooter;
        AnimatedObject aimer;
        AnimatedObject readyer;
        SpriteObject instructions;

        //camera
        Vector2 man = new Vector2();
        Vector2 mouse = new Vector2();
        int scrollWheelValue;
        int prevScrollWheelValue;

        int frameCount = 0;
        double averageFrameRate = 60.0;
        int totalMillisecs = 0;

        ScoreTextObject player1ScoreText;
        ScoreTextObject player2ScoreText;
        TitleTextObject TitleText;
        TextObject textObject;
        TextObject textObject2;
        TextObject textObject3;
        TextObject WinText;
        TextObject LoseText;
        TextObject GameOverText;
        TextObject Note;
        TextObject Note2;
        TextObject Note3;
        Line lineA = new Line();
        Line lineB = new Line();
        public ListOfScreenObjects topList = new ListOfScreenObjects();
        #endregion

        public Game1()
        {
            Global.graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Global.currentGame = this;
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            scrollWheelValue = 0;
            prevScrollWheelValue = 0;
            IsMouseVisible = false;
            Window.Title = "Billiards In Space - Cody Neuburger";
            base.Initialize();
        }
        protected override void LoadContent()
        {
            pockets.Add(Pocket1);
            pockets.Add(Pocket2);
            pockets.Add(Pocket3);
            pockets.Add(Pocket4);
            pockets.Add(Pocket5);
            pockets.Add(Pocket6);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Global.Load(Content);

            line1 = new Line();
            line1.Setup();

            //setup pools
            #region pools
            bulletPool = new BulletPool();
            bulletPool.numberOfObjects = 20;
            bulletPool.Setup();

            ballPool = new PoolOfBalls();
            ballPool.numberOfObjects = 30;
            ballPool.Setup();

            particlePool = new PoolOfParticles();
            particlePool.numberOfObjects = 20000;
            particlePool.Setup();

            backGround = new BackGround();
            backGround.Setup();
            #endregion

            //setup table
            Table = new Table();
            Table.Setup();
            Table.scale = 0.5f;
            Global.currentTable = Table;

            #region player stuff
            //place objects on table
            AimStick = new SpriteObject();
            AimStick.image = Global.AimStickImage;
            AimStick.Setup();
            AimStick.origin.X = 0;
            AimStick.layer = 0.6f;

            Crosshair = new SpriteObject();
            Crosshair.image = Global.CrosshairImage;
            Crosshair.Setup();
            Crosshair.layer = 0.61f;

            player1 = new Player();
            player1.Setup();
            #endregion

            Global.totalSolids = 7;
            Global.totalStripes = 7;

            walls = new ListOfWalls();
            walls.Setup();

            collidables = new ListOfCollidables();
            collidables.Setup();
            #region balls
            cueball = new BilliardBall();
            cueball.Setup();
            cueball.image = Global.CueBallImage;
            cueball.type = BilliardBall.TYPE_CUE;
            ball1 = new BilliardBall();
            ball1.Setup();
            ball1.image = Global.Ball1Image;
            ball1.type = BilliardBall.TYPE_SOLID;
            ball2 = new BilliardBall();
            ball2.Setup();
            ball2.image = Global.Ball2Image;
            ball2.type = BilliardBall.TYPE_SOLID;
            ball3 = new BilliardBall();
            ball3.Setup();
            ball3.image = Global.Ball3Image;
            ball3.type = BilliardBall.TYPE_SOLID;
            ball4 = new BilliardBall();
            ball4.Setup();
            ball4.image = Global.Ball4Image;
            ball4.type = BilliardBall.TYPE_SOLID;
            ball5 = new BilliardBall();
            ball5.Setup();
            ball5.image = Global.Ball5Image;
            ball5.type = BilliardBall.TYPE_SOLID;
            ball6 = new BilliardBall();
            ball6.Setup();
            ball6.image = Global.Ball6Image;
            ball6.type = BilliardBall.TYPE_SOLID;
            ball7 = new BilliardBall();
            ball7.Setup();
            ball7.image = Global.Ball7Image;
            ball7.type = BilliardBall.TYPE_SOLID;
            ball8 = new BilliardBall();
            ball8.Setup();
            ball8.image = Global.Ball8Image;
            ball8.type = BilliardBall.TYPE_8;
            ball9 = new BilliardBall();
            ball9.Setup();
            ball9.image = Global.Ball9Image;
            ball9.type = BilliardBall.TYPE_STRIPE;
            ball10 = new BilliardBall();
            ball10.Setup();
            ball10.image = Global.Ball10Image;
            ball10.type = BilliardBall.TYPE_STRIPE;
            ball11 = new BilliardBall();
            ball11.Setup();
            ball11.image = Global.Ball11Image;
            ball11.type = BilliardBall.TYPE_STRIPE;
            ball12 = new BilliardBall();
            ball12.Setup();
            ball12.image = Global.Ball12Image;
            ball12.type = BilliardBall.TYPE_STRIPE;
            ball13 = new BilliardBall();
            ball13.Setup();
            ball13.image = Global.Ball13Image;
            ball13.type = BilliardBall.TYPE_STRIPE;
            ball14 = new BilliardBall();
            ball14.Setup();
            ball14.image = Global.Ball14Image;
            ball14.type = BilliardBall.TYPE_STRIPE;
            ball15 = new BilliardBall();
            ball15.Setup();
            ball15.image = Global.Ball15Image;
            ball15.type = BilliardBall.TYPE_STRIPE;
            cueball.location = new Vector2(711, 711);
            ball8.location = new Vector2(2184, 711);
            #endregion
            #region test
            TESTwall1 = new Wall();
            TESTwall1.P1 = new Vector2(600, 600);
            TESTwall1.P2 = new Vector2(800, 400);
            TESTwall1.side = Wall.SIDE_RIGHT;
            TESTwall1.type = Wall.TYPE_FLOOR;
            TESTwall1.Setup();

            TESTwall2 = new Wall();
            TESTwall2.P1 = new Vector2(0, 400);
            TESTwall2.P2 = new Vector2(200, 600);
            TESTwall2.side = Wall.SIDE_LEFT;
            TESTwall2.type = Wall.TYPE_FLOOR;
            TESTwall2.Setup();

            TESTwall3 = new Wall();
            TESTwall3.P1 = new Vector2(0, 200);
            TESTwall3.P2 = new Vector2(200, 0);
            TESTwall3.side = Wall.SIDE_LEFT;
            TESTwall3.type = Wall.TYPE_CEILING;
            TESTwall3.Setup();

            TESTwall4 = new Wall();
            TESTwall4.P1 = new Vector2(600, 0);
            TESTwall4.P2 = new Vector2(800, 200);
            TESTwall4.side = Wall.SIDE_RIGHT;
            TESTwall4.type = Wall.TYPE_CEILING;
            TESTwall4.Setup();

            TESTwall5 = new Wall();
            TESTwall5.P1 = new Vector2(600, 400);
            TESTwall5.P2 = new Vector2(400, 200);
            TESTwall5.side = Wall.SIDE_RIGHT;
            TESTwall5.type = Wall.TYPE_CEILING;
            TESTwall5.Setup();

            TESTwall6 = new Wall();
            TESTwall6.P1 = new Vector2(300, 300);
            TESTwall6.P2 = new Vector2(400, 200);
            TESTwall6.side = Wall.SIDE_RIGHT;
            TESTwall6.type = Wall.TYPE_FLOOR;
            TESTwall6.Setup();

            TESTwall7 = new Wall();
            TESTwall7.P1 = new Vector2(500, 300);
            TESTwall7.P2 = new Vector2(400, 200);
            TESTwall7.side = Wall.SIDE_LEFT;
            TESTwall7.type = Wall.TYPE_FLOOR;
            TESTwall7.Setup();

            TESTwall8 = new Wall();
            TESTwall8.P1 = new Vector2(500, 300);
            TESTwall8.P2 = new Vector2(400, 400);
            TESTwall8.side = Wall.SIDE_LEFT;
            TESTwall8.type = Wall.TYPE_CEILING;
            TESTwall8.Setup();

            TESTwall9 = new Wall();
            TESTwall9.P1 = new Vector2(400, 400);
            TESTwall9.P2 = new Vector2(300, 300);
            TESTwall9.side = Wall.SIDE_RIGHT;
            TESTwall9.type = Wall.TYPE_CEILING;
            TESTwall9.Setup();
            #endregion
            #region HUD stuff
            //instructions
            runner = new AnimatedObject();
            runner.Setup();
            runner.animation = Global.Avatar_Run_SE;
            runner.speedControl = 4;
            runner.location.X = 118;
            runner.location.Y = 128;
            runner.state = AnimatedObject.STATE_ACTIVE;
            runner.layer = .97f;

            shooter = new AnimatedObject();
            shooter.Setup();
            shooter.animation = Global.Avatar_Fire_E;
            runner.speedControl = 4;
            shooter.location.X = 118;
            shooter.location.Y = 188;
            shooter.state = AnimatedObject.STATE_ACTIVE;
            shooter.layer = .97f;

            aimer = new AnimatedObject();
            aimer.Setup();
            aimer.animation = Global.Avatar_Aim_W;
            runner.speedControl = 4;
            aimer.location.X = 118;
            aimer.location.Y = 246;
            aimer.state = AnimatedObject.STATE_ACTIVE;
            aimer.layer = .97f;

            readyer = new AnimatedObject();
            readyer.Setup();
            readyer.animation = Global.Avatar_Ready_SE;
            readyer.location.X = 118;
            readyer.location.Y = 306;
            readyer.state = AnimatedObject.STATE_ACTIVE;
            readyer.layer = .97f;

            instructions = new SpriteObject();
            instructions.Setup();
            instructions.image = Global.instructionsImage;
            instructions.location.X = 150;
            instructions.location.Y = 150;
            instructions.layer = .96f;
            instructions.scale = 1f;
            instructions.state = SpriteObject.STATE_ACTIVE;

            screenFiller = new SpriteObject();
            screenFiller.Setup();
            screenFiller.state = ScreenObject.STATE_ACTIVE;
            screenFiller.image = Global.CueBallImage;
            screenFiller.scale = 10f;
            screenFiller.absoluteLocation.X = Global.windowWidth / 2;
            screenFiller.absoluteLocation.Y = Global.windowHeight / 2;
            screenFiller.layer = .94f;
            screenFiller.color = new Color(255, 100, 100, 200);

            player1ScoreText = new ScoreTextObject();
            player1ScoreText.Setup();
            player1ScoreText.layer = .95f;
            player1ScoreText.location.X = 10;
            player1ScoreText.location.Y = 10;

            player2ScoreText = new ScoreTextObject();
            player2ScoreText.Setup();
            player2ScoreText.layer = .95f;
            player2ScoreText.location.X = Global.windowWidth - 200f;
            player2ScoreText.location.Y = 10f;

            textObject = new TextObject();
            textObject.Setup();
            textObject.text = "Billiards";
            textObject.location.X = 100;
            textObject.location.Y = 200;
            textObject.scale = 3.0f;
            textObject.layer = .9f;

            textObject2 = new TextObject();
            textObject2.Setup();
            textObject2.text = "In Space!";
            textObject2.location.X = 120;
            textObject2.location.Y = 280;
            textObject2.scale = 3.0f;
            textObject2.layer = .9f;

            textObject3 = new TextObject();
            textObject3.Setup();
            textObject3.text = "Press F1";
            textObject3.location.X = Global.windowHeight - 100f;
            textObject3.location.Y = Global.windowHeight - 50f;
            textObject3.scale = 2.0f;
            textObject3.layer = .96f;

            WinText = new TextObject();
            WinText.Setup();
            WinText.text = "You Win!";
            WinText.location.X = Global.windowHeight / 2 - 100f;
            WinText.location.Y = Global.windowHeight / 2 - 50f;
            WinText.scale = 4.0f;
            WinText.layer = .95f;

            LoseText = new TextObject();
            LoseText.Setup();
            LoseText.text = "You Lose!";
            LoseText.location.X = Global.windowHeight / 2 - 100f;
            LoseText.location.Y = Global.windowHeight / 2 - 50f;
            LoseText.scale = 4.0f;
            LoseText.layer = .95f;

            GameOverText = new TextObject();
            GameOverText.Setup();
            GameOverText.text = "GAME OVER";
            GameOverText.location.X = Global.windowHeight / 2 - 100f;
            GameOverText.location.Y = Global.windowHeight / 2 - 50f;
            GameOverText.scale = 4.0f;
            GameOverText.layer = .95f;

            Note = new TextObject();
            Note.Setup();
            Note.text = "Note";
            Note.location.X = 50;
            Note.location.Y = 50;
            Note.scale = 1f;
            Note.layer = .99f;

            Note2 = new TextObject();
            Note2.Setup();
            Note2.text = "Note";
            Note2.location.X = 50;
            Note2.location.Y = 100;
            Note2.scale = 1f;
            Note2.layer = .99f;

            Note3 = new TextObject();
            Note3.Setup();
            Note3.text = "Note";
            Note3.location.X = 50;
            Note3.location.Y = 125;
            Note3.scale = 1f;
            Note3.layer = .99f;

            TitleText = new TitleTextObject();
            TitleText.Setup();
            TitleText.layer = .03f;
            TitleText.color.A = 130;
            #endregion

            topList.Setup();
            changeState(STATE_WELCOME);
        }
        void calcPockets()
        {
            Pocket1 = oldPocket1 * topList.scale + topList.absoluteLocation;
            Pocket2 = oldPocket2 * topList.scale + topList.absoluteLocation;
            Pocket3 = oldPocket3 * topList.scale + topList.absoluteLocation;
            Pocket4 = oldPocket4 * topList.scale + topList.absoluteLocation;
            Pocket5 = oldPocket5 * topList.scale + topList.absoluteLocation;
            Pocket6 = oldPocket6 * topList.scale + topList.absoluteLocation;
        }
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            Global.keyboardState = Keyboard.GetState();
            Global.mouseState = Mouse.GetState();

            switch (state)
            {
                #region tests
                case STATE_TEST_4_1:
                case STATE_TEST_4:
                    if (Global.keyboardState.IsKeyDown(Keys.Escape))
                    {
                        changeState(STATE_WELCOME);
                        break;
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.Up))
                    {
                        TEST_accuracy /= 10; // 0.001 may be all that is needed
                        if (TEST_accuracy < 0.00000001f) TEST_accuracy = 0.00000001f;
                        changeState(STATE_TEST_4_aUp);
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.Down))
                    {
                        TEST_accuracy *= 10;
                        if (TEST_accuracy > .1f) TEST_accuracy = .1f;
                        changeState(STATE_TEST_4_aDown);
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.Enter))
                    {
                        changeState(STATE_TEST_4_CLEAR);
                        break;
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.N))
                    {
                        TEST_part++;
                        changeState(STATE_TEST_4_N);
                    }
                    TEST_4();
                    break;
                case STATE_TEST_4_aUp:
                    if (Global.keyboardState.IsKeyDown(Keys.Escape))
                    {
                        changeState(STATE_WELCOME);
                        break;
                    }
                    if (Global.keyboardState.IsKeyUp(Keys.Up))
                    {
                        changeState(STATE_TEST_4_1);
                    }
                    TEST_4();
                    break;
                case STATE_TEST_4_aDown:
                    if (Global.keyboardState.IsKeyDown(Keys.Escape))
                    {
                        changeState(STATE_WELCOME);
                        break;
                    }
                    if (Global.keyboardState.IsKeyUp(Keys.Down))
                    {
                        changeState(STATE_TEST_4_1);
                    }
                    TEST_4();
                    break;
                case STATE_TEST_4_N:
                    if (Global.keyboardState.IsKeyDown(Keys.Escape))
                    {
                        changeState(STATE_WELCOME);
                        break;
                    }
                    if (Global.keyboardState.IsKeyUp(Keys.N))
                    {
                        changeState(STATE_TEST_4_1);
                    }
                    TEST_4();
                    break;
                case STATE_TEST_4_CLEAR:
                    if (Global.keyboardState.IsKeyDown(Keys.Escape))
                    {
                        changeState(STATE_WELCOME);
                        break;
                    }
                    if (Global.keyboardState.IsKeyUp(Keys.Enter))
                    {
                        changeState(STATE_TEST_4_1);
                        break;
                    }
                    TEST_4();
                    break;
                case STATE_TEST_3_1:
                case STATE_TEST_3:
                    if (Global.keyboardState.IsKeyDown(Keys.Escape))
                    {
                        changeState(STATE_WELCOME);
                        break;
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.Enter))
                    {
                        changeState(STATE_TEST_3_CLEAR);
                        break;
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.N))
                    {
                        TEST_part++;
                        changeState(STATE_TEST_3_N);
                        break;
                    }
                    TEST_3();
                    break;
                case STATE_TEST_3_CLEAR:
                    if (Global.keyboardState.IsKeyDown(Keys.Escape))
                    {
                        changeState(STATE_WELCOME);
                        break;
                    }
                    if (Global.keyboardState.IsKeyUp(Keys.Enter))
                    {
                        changeState(STATE_TEST_3_1);
                        break;
                    }
                    TEST_3();
                    break;
                case STATE_TEST_3_N:
                    if (Global.keyboardState.IsKeyDown(Keys.Escape))
                    {
                        changeState(STATE_WELCOME);
                        break;
                    }
                    if (Global.keyboardState.IsKeyUp(Keys.N))
                    {
                        changeState(STATE_TEST_3_1);
                        break;
                    }
                    TEST_3();
                    break;
                case STATE_TEST_1:
                    if (Global.keyboardState.IsKeyDown(Keys.Escape))
                    {
                        changeState(STATE_WELCOME);
                        break;
                    }
                    TEST_1();
                    break;
                case STATE_TEST_2:
                    if (Global.keyboardState.IsKeyDown(Keys.Escape))
                    {
                        changeState(STATE_WELCOME);
                        break;
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.Up))
                    {
                        TEST_accuracy /= 10; // 0.001 may be all that is needed
                        if (TEST_accuracy < 0.00000001f) TEST_accuracy = 0.00000001f;
                        changeState(STATE_TEST_2_aUp);
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.Down))
                    {
                        TEST_accuracy *= 10;
                        if (TEST_accuracy > .1f) TEST_accuracy = .1f;
                        changeState(STATE_TEST_2_aDown);
                    }
                    TEST_2();
                    break;
                case STATE_TEST_2_aUp:
                    if (Global.keyboardState.IsKeyDown(Keys.Escape))
                    {
                        changeState(STATE_WELCOME);
                        break;
                    }
                    if (Global.keyboardState.IsKeyUp(Keys.Up))
                    {
                        changeState(STATE_TEST_2);
                    }
                    TEST_2();
                    break;
                case STATE_TEST_2_aDown:
                    if (Global.keyboardState.IsKeyDown(Keys.Escape))
                    {
                        changeState(STATE_WELCOME);
                        break;
                    }
                    if (Global.keyboardState.IsKeyUp(Keys.Down))
                    {
                        changeState(STATE_TEST_2);
                    }
                    TEST_2();
                    break;
                case STATE_TEST_5_1:
                case STATE_TEST_5_2:
                    break;
                #endregion
                #region title
                case STATE_WELCOME:
                    //Welcome screen
                    //Logos
                    if (Global.keyboardState.IsKeyDown(Keys.F1) || stateframes > 300)
                    {
                        changeState(STATE_TITLE);
                        break;
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.F5))
                    {
                        Global.ToggleFullscreen();
                        changeState(STATE_W_FULL);
                        break;
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.F9))
                    {
                        changeState(STATE_TEST_1);
                        break;
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.F10))
                    {
                        changeState(STATE_TEST_2);
                        break;
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.F11))
                    {
                        changeState(STATE_TEST_3);
                        break;
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.F12))
                    {
                        changeState(STATE_TEST_4);
                        break;
                    }
                    TossBallsAround();
                    break;
                case STATE_W_FULL:
                    if (Global.keyboardState.IsKeyUp(Keys.F5))
                    {
                        changeState(STATE_WELCOME);
                        break;
                    }
                    break;
                case STATE_TITLE:
                    //Title Screen
                    //Instructions
                    //Play
                    //Options
                    if (Global.keyboardState.IsKeyDown(Keys.F2))
                    {
                        changeState(STATE_INSTRUCTIONS0);
                        break;
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.F9))
                    {
                        changeState(STATE_TEST_1);
                        break;
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.F10))
                    {
                        changeState(STATE_TEST_2);
                        break;
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.F11))
                    {
                        changeState(STATE_TEST_3);
                        break;
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.F12))
                    {
                        changeState(STATE_TEST_4);
                        break;
                    }
                    TossBallsAround();
                    break;
                case STATE_MENU:
                    break;
                case STATE_HIGHSCORES:
                    break;
                case STATE_INSTRUCTIONS0:
                    if (Global.keyboardState.IsKeyUp(Keys.F2))
                    {
                        changeState(STATE_INSTRUCTIONS1);
                        break;
                    }
                    break;
                case STATE_INSTRUCTIONS1:
                    if (Global.keyboardState.IsKeyDown(Keys.F2))
                    {
                        changeState(STATE_GAMESTART);
                        break;
                    }
                    break;
                #endregion
                #region game
                case STATE_G_FULL:
                    if (Global.keyboardState.IsKeyUp(Keys.F5))
                    {
                        changeState(STATE_GAMEPLAY);
                        break;
                    }
                    break;
                case STATE_GAMESTART:
                    changeState(STATE_GAMEPLAY);
                    break;
                case STATE_OPTIONS:
                    break;
                case STATE_GAMEPLAY:
                    HandleGameKeys();
                    HandleGameLogic();
                    if ((Global.keyboardState.IsKeyDown(Keys.LeftAlt) && Global.keyboardState.IsKeyDown(Keys.A)) ||
                        (Global.keyboardState.IsKeyDown(Keys.RightAlt) && Global.keyboardState.IsKeyDown(Keys.A)))
                    {
                        changeState(STATE_AUTO);
                        break;
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.Escape))
                    {
                        changeState(STATE_WELCOME);
                        break;
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.F5))
                    {
                        Global.ToggleFullscreen();
                        changeState(STATE_G_FULL);
                        break;
                    }
                    break;
                case STATE_AUTO:
                    HandleGameKeys();
                    HandleGameLogic();
                    if ((Global.keyboardState.IsKeyDown(Keys.LeftAlt) && Global.keyboardState.IsKeyDown(Keys.M)) ||
                        (Global.keyboardState.IsKeyDown(Keys.RightAlt) && Global.keyboardState.IsKeyDown(Keys.M)))
                    {
                        changeState(STATE_GAMESTART);
                        break;
                    }
                    if (Global.keyboardState.IsKeyDown(Keys.Escape))
                    {
                        changeState(STATE_WELCOME);
                        break;
                    }
                    break;
                case STATE_WIN:
                    if (Global.keyboardState.IsKeyDown(Keys.F1) || stateframes > 400)
                    {
                        changeState(STATE_GAMEOVER);
                        break;
                    }
                    break;
                case STATE_LOSE:
                    if (Global.keyboardState.IsKeyDown(Keys.F1) || stateframes > 400)
                    {
                        changeState(STATE_GAMEOVER);
                        break;
                    }
                    break;
                case STATE_GAMEOVER:
                    if (Global.keyboardState.IsKeyDown(Keys.F2) || stateframes > 900)
                    {
                        changeState(STATE_WELCOME);
                    }

                    break;
                #endregion
            }

            stateframes++;
            //P for slow motion
            if(Global.keyboardState.IsKeyDown(Keys.P))
            {
                if (stateframes % 30 == 0)
                {
                    topList.Update(gameTime);
                    stateframes = 0;
                }
            }
            else
            {
                topList.Update(gameTime);
            }
            PanCamera(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GetFrameRate(gameTime);

            switch (state)
            {
                case STATE_TEST_4_N:
                case STATE_TEST_4_1:
                case STATE_TEST_4_CLEAR:
                case STATE_TEST_4:
                case STATE_TEST_4_aUp:
                case STATE_TEST_4_aDown:
                case STATE_TEST_3:
                case STATE_TEST_3_1:
                case STATE_TEST_3_N:
                case STATE_TEST_3_CLEAR:
                case STATE_TEST_2:
                case STATE_TEST_2_aUp:
                case STATE_TEST_2_aDown:
                case STATE_TEST_1:
                    GraphicsDevice.Clear(Color.DarkGray);
                    break;
                case STATE_MENU:
                case STATE_HIGHSCORES:
                case STATE_OPTIONS:
                    GraphicsDevice.Clear(new Color(50, 100, 50));
                    break;
                case STATE_TEST_5_1:
                case STATE_TEST_5_2:
                case STATE_WELCOME:
                case STATE_W_FULL:
                case STATE_TITLE:
                case STATE_INSTRUCTIONS0:
                case STATE_INSTRUCTIONS1:
                case STATE_GAMEOVER:
                case STATE_AUTO:
                case STATE_WIN:
                case STATE_LOSE:
                case STATE_NEWHIGHSCORE:
                case STATE_STATS:
                case STATE_GAMESTART:
                case STATE_GAMEPLAY:
                case STATE_G_FULL:
                    GraphicsDevice.Clear(Color.Black);
                    break; ;

            }

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.SaveState);

            topList.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
        public void changeState(int nuState)
        {
            Crosshair.state = ScreenObject.STATE_INACTIVE;
            stateframes = 0;
            state = nuState;
            switch (state)
            {
                #region test 4
                case STATE_TEST_4_1:
                    break;
                case STATE_TEST_4_CLEAR:
                    particlePool.Clear();
                    break;
                case STATE_TEST_4_N:
                    TEST_actives = 0;
                    TEST_i = 0;
                    particlePool.Clear();
                    ballPool.Clear();
                    topList.Clear();
                    collidables.Clear();
                    walls.Clear();
                    Global.currentPlayer = null;
                    textObject3.text = "Press Esc";
                    topList.Add(textObject3, false);
                    collidables.AddList(ballPool, false);
                    topList.Add(collidables, true);
                    topList.Add(particlePool, false);
                    walls.state = ListOfWalls.STATE_DRAW_LINES;
                    walls.Add(TESTwall5, false);
                    topList.Add(walls, false);
                    Note.text = "Test 4\nPress Enter to Clear";
                    topList.Add(Note, false);
                    Note2.text = "Ball 1 Velocity: 0";
                    topList.Add(Note2, false);
                    Note3.text = "Precision: .01";
                    topList.Add(Note3, false);
                    break;
                case STATE_TEST_4_aUp:
                case STATE_TEST_4_aDown:
                case STATE_TEST_4:
                    TEST_part = 3;
                    TEST_actives = 0;
                    TEST_i = 0;
                    particlePool.Clear();
                    ballPool.Clear();
                    topList.Clear();
                    collidables.Clear();
                    walls.Clear();
                    Global.currentPlayer = null;
                    textObject3.text = "Press Esc";
                    topList.Add(textObject3, false);
                    collidables.AddList(ballPool, false);
                    topList.Add(collidables, true);
                    topList.Add(particlePool, false);
                    walls.state = ListOfWalls.STATE_DRAW_LINES;
                    walls.Add(TESTwall5, false);
                    topList.Add(walls, false);
                    Note.text = "Test 4\nPress Enter to Clear";
                    topList.Add(Note, false);
                    Note2.text = "Ball 1 Velocity: 0";
                    topList.Add(Note2, false);
                    Note3.text = "Precision: .01";
                    topList.Add(Note3, false);
                    break;
                #endregion
                #region test 3
                case STATE_TEST_3:
                    TESTwall6 = new Wall();
                    TESTwall6.P1 = new Vector2(300, 300);
                    TESTwall6.P2 = new Vector2(400, 200);
                    TESTwall6.side = Wall.SIDE_RIGHT;
                    TESTwall6.type = Wall.TYPE_FLOOR;
                    TESTwall6.Setup();

                    TESTwall7 = new Wall();
                    TESTwall7.P1 = new Vector2(500, 300);
                    TESTwall7.P2 = new Vector2(400, 200);
                    TESTwall7.side = Wall.SIDE_LEFT;
                    TESTwall7.type = Wall.TYPE_FLOOR;
                    TESTwall7.Setup();

                    TESTwall8 = new Wall();
                    TESTwall8.P1 = new Vector2(500, 300);
                    TESTwall8.P2 = new Vector2(400, 400);
                    TESTwall8.side = Wall.SIDE_LEFT;
                    TESTwall8.type = Wall.TYPE_CEILING;
                    TESTwall8.Setup();

                    TESTwall9 = new Wall();
                    TESTwall9.P1 = new Vector2(400, 400);
                    TESTwall9.P2 = new Vector2(300, 300);
                    TESTwall9.side = Wall.SIDE_RIGHT;
                    TESTwall9.type = Wall.TYPE_CEILING;
                    TESTwall9.Setup();
                    TEST_actives = 0;
                    TEST_i = 0;
                    TEST_part = 1;
                    particlePool.Clear();
                    ballPool.Clear();
                    topList.Clear();
                    collidables.Clear();
                    walls.Clear();
                    Global.currentPlayer = null;
                    textObject3.text = "Press Esc";
                    topList.Add(textObject3, false);
                    collidables.AddList(ballPool, false);
                    topList.Add(collidables, true);
                    topList.Add(particlePool, false);
                    walls.state = ListOfWalls.STATE_DRAW_LINES;
                    walls.Add(TESTwall1, false);
                    walls.Add(TESTwall2, false);
                    walls.Add(TESTwall3, false);
                    walls.Add(TESTwall4, false);
                    walls.Add(TESTwall6, false);
                    walls.Add(TESTwall7, false);
                    walls.Add(TESTwall8, false);
                    walls.Add(TESTwall9, false);
                    topList.Add(walls, false);
                    Note.text = "Test 3\nPress Enter to Clear";
                    topList.Add(Note, false);
                    Note2.text = "Ball 1 Velocity: 0";
                    Note3.text = "Ball 2 Velocity: 0";
                    topList.Add(Note2, false);
                    topList.Add(Note3, false);
                    break;
                case STATE_TEST_3_1:
                    break;
                case STATE_TEST_3_CLEAR:
                    particlePool.Clear();
                    break;
                case STATE_TEST_3_N:
                    particlePool.Clear();
                    ballPool.Clear();
                    TEST_actives = 0;
                    TEST_i = 0;
                    break;
                #endregion
                #region test 2
                case STATE_TEST_2_aUp:
                case STATE_TEST_2_aDown:
                case STATE_TEST_2:
                    TEST_actives = 0;
                    TEST_i = 0;
                    particlePool.Clear();
                    ballPool.Clear();
                    topList.Clear();
                    collidables.Clear();
                    walls.Clear();
                    collidables.changeState(ListOfCollidables.STATE_TEST_2);
                    Global.currentPlayer = null;
                    textObject3.text = "Press Esc";
                    topList.Add(textObject3, false);
                    collidables.AddList(ballPool, false);
                    topList.Add(collidables, true);
                    topList.Add(particlePool, false);
                    Note.text = "This demonstrates fixing the\ndisplacement by convergence";
                    topList.Add(Note, false);
                    Note2.text = "Velocity: 0";
                    topList.Add(Note2, false);
                    Note3.text = "Precision: .01";
                    topList.Add(Note3, false);
                    break;
                #endregion
                #region test 1
                case STATE_TEST_1:
                    TEST_actives = 0;
                    TEST_i = 0;
                    particlePool.Clear();
                    ballPool.Clear();
                    topList.Clear();
                    collidables.Clear();
                    walls.Clear();
                    collidables.changeState(ListOfCollidables.STATE_TEST_1);
                    Global.currentPlayer = null;
                    textObject3.text = "Press Esc";
                    topList.Add(textObject3, false);
                    collidables.AddList(ballPool, false);
                    topList.Add(collidables, true);
                    topList.Add(particlePool, false);
                    Note.text = "This demonstrates the flaw\nof updating position with frames";
                    topList.Add(Note, false);
                    Note2.text = "Velocity: 0";
                    topList.Add(Note2, false);
                    break;
                #endregion
                #region welcome
                case STATE_W_FULL:
                    break;
                case STATE_WELCOME:
                    topList.Clear();
                    topList.scale = 1f;
                    particlePool.Clear();
                    ballPool.Clear();
                    topList.Clear();
                    collidables.Clear();
                    walls.Clear();
                    collidables.changeState(ListOfCollidables.STATE_ACTIVE);
                    Global.currentPlayer = null;
                    textObject3.text = "Press F1";
                    topList.Add(backGround, true);
                    topList.Add(textObject, true);
                    topList.Add(TitleText, false);
                    topList.Add(textObject3, false);
                    collidables.AddList(ballPool, true);
                    topList.Add(collidables, true);
                    topList.Add(particlePool, false);
                    break;
                #endregion
                #region title
                case STATE_TITLE:
                    topList.Clear();
                    collidables.Clear();
                    walls.Clear();
                    collidables.changeState(ListOfCollidables.STATE_ACTIVE);
                    Global.currentPlayer = null;
                    textObject2.text = "In Space!";
                    topList.Add(textObject2, false);
                    textObject3.text = "Press F2";
                    topList.Add(textObject3, false);
                    topList.Add(backGround, true);
                    topList.Add(textObject, true);
                    topList.Add(TitleText, false);
                    collidables.AddList(ballPool, true);
                    topList.Add(collidables, true);
                    topList.Add(particlePool, false);
                    break;
                #endregion
                #region options
                case STATE_MENU:
                    break;
                case STATE_HIGHSCORES:
                    break;
                case STATE_INSTRUCTIONS0:
                    topList.Clear();
                    collidables.Clear();
                    walls.Clear();
                    topList.Add(instructions, false);
                    topList.Add(runner, false);
                    topList.Add(shooter, false);
                    topList.Add(readyer, false);
                    topList.Add(aimer, false);
                    textObject3.text = "Press F2";
                    topList.Add(textObject3, false);
                    topList.Add(backGround, true);
                    topList.Add(particlePool, false);
                    topList.Add(screenFiller, false);
                    screenFiller.color = new Color(100, 100, 100, 100);
                    break;
                case STATE_INSTRUCTIONS1:
                    topList.Clear();
                    collidables.Clear();
                    walls.Clear();
                    topList.Add(instructions, false);
                    topList.Add(runner, false);
                    topList.Add(shooter, false);
                    topList.Add(readyer, false);
                    topList.Add(aimer, false);
                    textObject3.text = "Press F2";
                    topList.Add(textObject3, false);
                    topList.Add(backGround, true);
                    topList.Add(particlePool, false);
                    topList.Add(screenFiller, false);
                    screenFiller.color = new Color(100, 100, 100, 100);
                    break;
                #endregion
                #region game
                case STATE_GAMESTART:
                    topList.Clear();
                    collidables.Clear();
                    walls.Clear();
                    setupWalls();
                    topList.scale = 0.3f;
                    Crosshair.state = ScreenObject.STATE_ACTIVE;
                    Global.currentPlayer = player1;
                    player1.avatar.state = Avatar.STATE_CANCEL;
                    player1.Setup();
                    player1.autoMode = false;
                    topList.Add(backGround, false);
                    topList.Add(player1ScoreText, false);
                    topList.Add(player2ScoreText, false);
                    topList.Add(TitleText, false);
                    topList.Add(AimStick, true);
                    topList.Add(Table, true);
                    collidables.AddList(bulletPool, true);
                    collidables.Add(player1, true);
                    collidables.Add(ball1, true);
                    collidables.Add(ball2, true);
                    collidables.Add(ball3, true);
                    collidables.Add(ball4, true);
                    collidables.Add(ball5, true);
                    collidables.Add(ball6, true);
                    collidables.Add(ball7, true);
                    collidables.Add(ball8, true);
                    collidables.Add(ball9, true);
                    collidables.Add(ball10, true);
                    collidables.Add(ball11, true);
                    collidables.Add(ball12, true);
                    collidables.Add(ball13, true);
                    collidables.Add(ball14, true);
                    collidables.Add(ball15, true);
                    collidables.Add(cueball, true);
                    topList.Add(collidables, true);
                    topList.Add(particlePool, false);
                    walls.state = ListOfWalls.STATE_ACTIVE;
                    #region test
                    //TESTwall6 = new Wall();
                    //TESTwall6.P1 = new Vector2(600, 600);
                    //TESTwall6.P2 = new Vector2(800, 400);
                    //TESTwall6.P1 += Global.currentTable.topLeftCorner;
                    //TESTwall6.P2 += Global.currentTable.topLeftCorner;
                    //TESTwall6.side = Wall.SIDE_RIGHT;
                    //TESTwall6.type = Wall.TYPE_FLOOR;
                    //TESTwall6.Setup();

                    //TESTwall7 = new Wall();
                    //TESTwall7.P1 = new Vector2(1000, 600);
                    //TESTwall7.P2 = new Vector2(800, 400);
                    //TESTwall7.P1 += Global.currentTable.topLeftCorner;
                    //TESTwall7.P2 += Global.currentTable.topLeftCorner;
                    //TESTwall7.side = Wall.SIDE_LEFT;
                    //TESTwall7.type = Wall.TYPE_FLOOR;
                    //TESTwall7.Setup();

                    //TESTwall8 = new Wall();
                    //TESTwall8.P1 = new Vector2(1000, 600);
                    //TESTwall8.P2 = new Vector2(800, 800);
                    //TESTwall8.P1 += Global.currentTable.topLeftCorner;
                    //TESTwall8.P2 += Global.currentTable.topLeftCorner;
                    //TESTwall8.side = Wall.SIDE_LEFT;
                    //TESTwall8.type = Wall.TYPE_CEILING;
                    //TESTwall8.Setup();

                    //TESTwall9 = new Wall();
                    //TESTwall9.P1 = new Vector2(800, 800);
                    //TESTwall9.P2 = new Vector2(600, 600);
                    //TESTwall9.P1 += Global.currentTable.topLeftCorner;
                    //TESTwall9.P2 += Global.currentTable.topLeftCorner;
                    //TESTwall9.side = Wall.SIDE_RIGHT;
                    //TESTwall9.type = Wall.TYPE_CEILING;
                    //TESTwall9.Setup();
                    //walls.Add(TESTwall6, true);
                    //walls.Add(TESTwall7, true);
                    //walls.Add(TESTwall8, true);
                    //walls.Add(TESTwall9, true);
                    #endregion
                    topList.Add(walls, true);
                    collidables.RackBalls();
                    break;
                case STATE_OPTIONS:
                    break;
                case STATE_GAMEPLAY:
                    break;
                case STATE_G_FULL:
                    break;
                case STATE_AUTO:
                    topList.Clear();
                    collidables.Clear();
                    walls.Clear();
                    setupWalls();
                    Crosshair.state = ScreenObject.STATE_ACTIVE;
                    Global.currentPlayer = player1;
                    player1.avatar.state = Avatar.STATE_CANCEL;
                    player1.autoMode = true;
                    topList.Add(backGround, true);
                    topList.Add(player1ScoreText, false);
                    topList.Add(player2ScoreText, false);
                    topList.Add(TitleText, false);
                    topList.Add(AimStick, true);
                    topList.Add(Table, true);
                    collidables.AddList(bulletPool, true);
                    collidables.Add(player1, true);
                    collidables.Add(ball1, true);
                    collidables.Add(ball2, true);
                    collidables.Add(ball3, true);
                    collidables.Add(ball4, true);
                    collidables.Add(ball5, true);
                    collidables.Add(ball6, true);
                    collidables.Add(ball7, true);
                    collidables.Add(ball8, true);
                    collidables.Add(ball9, true);
                    collidables.Add(ball10, true);
                    collidables.Add(ball11, true);
                    collidables.Add(ball12, true);
                    collidables.Add(ball13, true);
                    collidables.Add(ball14, true);
                    collidables.Add(ball15, true);
                    collidables.Add(cueball, true);
                    topList.Add(collidables, true);
                    topList.Add(particlePool, false);
                    topList.Add(walls, true);
                    collidables.PlaceOnTable();
                    break;
                #endregion
                #region end game
                case STATE_WIN:
                    topList.Add(TitleText, false);
                    topList.Add(screenFiller, false);
                    screenFiller.color = new Color(100, 200, 100, 200);
                    topList.Add(WinText, false);
                    LoseText.text = "You Won!";
                    topList.Add(AimStick, true);
                    textObject3.text = "Press F1";
                    topList.Add(textObject3, false);
                    break;
                case STATE_LOSE:
                    topList.Add(TitleText, false);
                    topList.Add(screenFiller, false);
                    screenFiller.color = new Color(255, 100, 100, 200);
                    topList.Add(LoseText, false);
                    LoseText.text = "You Lost!";
                    textObject3.text = "Press F1";
                    topList.Add(textObject3, false);
                    break;
                case STATE_NEWHIGHSCORE:
                    break;
                case STATE_GAMEOVER:
                    topList.Clear();
                    Global.currentPlayer = player1;
                    player1.avatar.state = Avatar.STATE_CANCEL;
                    textObject3.text = "Press F2";
                    topList.Add(textObject3, false);
                    topList.Add(GameOverText, false);
                    GameOverText.text = "GAME OVER";
                    topList.Add(backGround, true);
                    topList.Add(GameOverText, false);
                    topList.Add(particlePool, false);
                    walls.state = ListOfWalls.STATE_ACTIVE;
                    topList.Add(walls, true);
                    break;
                #endregion
            }
        }
        public int TEST_actives = 0;
        public int TEST_i = 0;
        public int TEST_part = 0;
        public float TEST_accuracy = 0.001f;
        private void TEST_1()
        {
            if (TEST_actives == 0)
            {
                TEST_actives = 2;
                TEST_i++;
                BilliardBall ball;
                ball = ballPool.getBall();
                if (ball != null)
                {
                    ball.friction = 1f;
                    ball.scale = 0.4f;
                    ball.sRadius = ball.radius * ball.scale;
                    ball.image = Global.randomBallImage();
                    ball.state = BilliardBall.STATE_TEST_1;
                    ball.location.X = 200;
                    ball.location.Y = 250;
                    ball.velocity.X = TEST_i;
                    ball.velocity.Y = 0;
                }
                ball = ballPool.getBall();
                if (ball != null)
                {
                    ball.friction = 1f;
                    ball.scale = 0.4f;
                    ball.sRadius = ball.radius * ball.scale;
                    ball.image = Global.randomBallImage();
                    ball.state = BilliardBall.STATE_TEST_1;
                    ball.location.X = 600;
                    ball.location.Y = 350;
                    ball.velocity.X = -TEST_i;
                    ball.velocity.Y = 0;
                }

                Note2.text = "Velocity: " + ball.velocity.Length();

                if (TEST_i >= 100)
                {
                    //particlePool.Clear();
                    TEST_i = 0;
                }
            }
        }
        private void TEST_2()
        {
            if (TEST_actives == 0)
            {
                TEST_actives = 2;
                TEST_i++;
                BilliardBall ball;
                ball = ballPool.getBall();
                if (ball != null)
                {
                    ball.friction = 1f;
                    ball.scale = 0.4f;
                    ball.sRadius = ball.radius * ball.scale;
                    ball.image = Global.randomBallImage();
                    ball.state = BilliardBall.STATE_TEST_2;
                    ball.location.X = 200;
                    ball.location.Y = 250;
                    ball.velocity.X = TEST_i;
                    ball.velocity.Y = 0;
                }
                ball = ballPool.getBall();
                if (ball != null)
                {
                    ball.friction = 1f;
                    ball.scale = 0.4f;
                    ball.sRadius = ball.radius * ball.scale;
                    ball.image = Global.randomBallImage();
                    ball.state = BilliardBall.STATE_TEST_2;
                    ball.location.X = 600;
                    ball.location.Y = 350;
                    ball.velocity.X = -TEST_i;
                    ball.velocity.Y = 0;
                }

                Note2.text = "Velocity: " + ball.velocity.Length();
                Note3.text = "Accuracy: " + TEST_accuracy;

                if (TEST_i >= 100)
                {
                    //particlePool.Clear();
                    TEST_i = 0;
                }
            }
        }
        private void TEST_3()
        {
            if (TEST_part > 1) TEST_part = 0;
            switch (TEST_part)
            {
                case 0:
                    if (TEST_actives == 0)
                    {
                        TEST_actives = 2;
                        TESTball1 = ballPool.getBall();
                        if (TESTball1 != null)
                        {
                            TESTball1.friction = 1f;
                            TESTball1.scale = 0.4f;
                            TESTball1.sRadius = TESTball1.radius * TESTball1.scale;
                            TESTball1.image = Global.Ball1Image;
                            TESTball1.state = BilliardBall.STATE_TEST_3;
                            TESTball1.location.X = 200;
                            TESTball1.location.Y = 300;
                            TESTball1.velocity.X = Global.randFloat(-3f, 3f);
                            TESTball1.velocity.Y = Global.randFloat(-3f, 3f);
                            TESTball1.oldLocation = TESTball1.location;
                            TESTball1.oldVelocity = TESTball1.velocity;
                        }
                        TESTball2 = ballPool.getBall();
                        if (TESTball2 != null)
                        {
                            TESTball2.friction = 1f;
                            TESTball2.scale = 0.4f;
                            TESTball2.sRadius = TESTball2.radius * TESTball2.scale;
                            TESTball2.image = Global.Ball2Image;
                            TESTball2.state = BilliardBall.STATE_TEST_3;
                            TESTball2.location.X = 600;
                            TESTball2.location.Y = 300;
                            TESTball2.velocity.X = Global.randFloat(-3f, 3f);
                            TESTball2.velocity.Y = Global.randFloat(-3f, 3f);
                            TESTball2.oldLocation = TESTball2.location;
                            TESTball2.oldVelocity = TESTball2.velocity;
                        }

                        TESTwall6 = new Wall();
                        TESTwall6.P1 = new Vector2(300, 300);
                        TESTwall6.P2 = new Vector2(400, 200);
                        TESTwall6.side = Wall.SIDE_RIGHT;
                        TESTwall6.type = Wall.TYPE_FLOOR;
                        TESTwall6.Setup();

                        TESTwall7 = new Wall();
                        TESTwall7.P1 = new Vector2(500, 300);
                        TESTwall7.P2 = new Vector2(400, 200);
                        TESTwall7.side = Wall.SIDE_LEFT;
                        TESTwall7.type = Wall.TYPE_FLOOR;
                        TESTwall7.Setup();

                        TESTwall8 = new Wall();
                        TESTwall8.P1 = new Vector2(500, 300);
                        TESTwall8.P2 = new Vector2(400, 400);
                        TESTwall8.side = Wall.SIDE_LEFT;
                        TESTwall8.type = Wall.TYPE_CEILING;
                        TESTwall8.Setup();

                        TESTwall9 = new Wall();
                        TESTwall9.P1 = new Vector2(400, 400);
                        TESTwall9.P2 = new Vector2(300, 300);
                        TESTwall9.side = Wall.SIDE_RIGHT;
                        TESTwall9.type = Wall.TYPE_CEILING;
                        TESTwall9.Setup();
                    }
                    Note2.text = "Ball 1 Velocity: " + TESTball1.velocity.Length();
                    Note3.text = "Ball 2 Velocity: " + TESTball2.velocity.Length();
                    break;
                case 1:
                    if (TEST_actives == 0)
                    {
                        TEST_actives = 1;
                        TEST_i++;
                        BilliardBall ball;
                        ball = ballPool.getBall();
                        if (ball != null)
                        {
                            ball.friction = 1f;
                            ball.scale = 0.4f;
                            ball.sRadius = ball.radius * ball.scale;
                            ball.image = Global.randomBallImage();
                            ball.state = BilliardBall.STATE_TEST_4;
                            ball.location.X = 200;
                            ball.location.Y = 240 + TEST_i * 5;
                            ball.velocity.X = 5;
                            ball.velocity.Y = 0;
                        }

                        TESTwall5.P1 = new Vector2(600, 400);
                        TESTwall5.P2 = new Vector2(400, 200);
                        TESTwall5.side = Wall.SIDE_RIGHT;
                        TESTwall5.type = Wall.TYPE_CEILING;
                        TESTwall5.Setup();

                        Note2.text = "Velocity: " + ball.velocity.Length();
                        Note3.text = "Accuracy: " + TEST_accuracy;
                        if (TEST_i >= 30)
                        {
                            //particlePool.Clear();
                            TEST_i = 0;
                            TEST_part = 0;
                            TEST_actives = 0;
                        }
                    }
                    break;
            }
        }
        private void TEST_4()
        {
            switch (TEST_part)
            {
                case 0:
                    if (TEST_actives == 0)
                    {
                        TEST_actives = 1;
                        TEST_i++;
                        BilliardBall ball;
                        ball = ballPool.getBall();
                        if (ball != null)
                        {
                            ball.friction = 1f;
                            ball.scale = 0.4f;
                            ball.sRadius = ball.radius * ball.scale;
                            ball.image = Global.randomBallImage();
                            ball.state = BilliardBall.STATE_TEST_4;
                            ball.location.X = 200;
                            ball.location.Y = 300;
                            ball.velocity.X = TEST_i;
                            ball.velocity.Y = 0;
                        }

                        TESTwall5.P1 = new Vector2(600, 400);
                        TESTwall5.P2 = new Vector2(400, 200);
                        TESTwall5.side = Wall.SIDE_RIGHT;
                        TESTwall5.type = Wall.TYPE_CEILING;
                        TESTwall5.Setup();

                        Note2.text = "Velocity: " + ball.velocity.Length();
                        Note3.text = "Accuracy: " + TEST_accuracy;
                        if (TEST_i >= 100)
                        {
                            //particlePool.Clear();
                            TEST_i = 0;
                            TEST_part = 1;
                        }
                    }
                    break;
                case 1:
                    if (TEST_actives == 0)
                    {
                        TEST_actives = 1;
                        TEST_i++;
                        BilliardBall ball;
                        ball = ballPool.getBall();
                        if (ball != null)
                        {
                            ball.friction = 1f;
                            ball.scale = 0.4f;
                            ball.sRadius = ball.radius * ball.scale;
                            ball.image = Global.randomBallImage();
                            ball.state = BilliardBall.STATE_TEST_4_2;
                            ball.location.X = 200;
                            ball.location.Y = 300;
                            ball.velocity.X = 5;
                            ball.velocity.Y = TEST_i / 4f;
                        }

                        TESTwall5.P1 = new Vector2(600, 400);
                        TESTwall5.P2 = new Vector2(400, 200);
                        TESTwall5.side = Wall.SIDE_RIGHT;
                        TESTwall5.type = Wall.TYPE_CEILING;
                        TESTwall5.Setup();

                        Note2.text = "Velocity: " + ball.velocity.Length();
                        Note3.text = "Accuracy: " + TEST_accuracy;
                        if (TEST_i >= 8)
                        {
                            //particlePool.Clear();
                            TEST_i = 0;
                            TEST_part = 2;
                        }
                    }
                    break;
                case 2:
                    if (TEST_actives == 0)
                    {
                        TEST_actives = 1;
                        TEST_i++;
                        BilliardBall ball;
                        ball = ballPool.getBall();
                        if (ball != null)
                        {
                            ball.friction = 1f;
                            ball.scale = 0.4f;
                            ball.sRadius = ball.radius * ball.scale;
                            ball.image = Global.randomBallImage();
                            ball.state = BilliardBall.STATE_TEST_4;
                            ball.location.X = 200;
                            ball.location.Y = 300;
                            ball.velocity.X = 5;
                            ball.velocity.Y = 0f;
                        }

                        
                        TESTwall5._P2.X += 8;

                        Note2.text = "Velocity: " + ball.velocity.Length();
                        Note3.text = "Accuracy: " + TEST_accuracy;
                        if (TEST_i >= 20)
                        {
                            //particlePool.Clear();
                            TEST_i = 0;
                            TEST_part = 3;
                            TESTwall5.P2.X = 400;
                        }
                    }
                    break;
                case 3:
                    if (TEST_actives == 0)
                    {
                        TEST_actives = 1;
                        TEST_i++;
                        BilliardBall ball;
                        ball = ballPool.getBall();
                        if (ball != null)
                        {
                            ball.friction = 1f;
                            ball.scale = 0.4f;
                            ball.sRadius = ball.radius * ball.scale;
                            ball.image = Global.randomBallImage();
                            ball.state = BilliardBall.STATE_TEST_4;
                            ball.location.X = 200;
                            ball.location.Y = 420 + TEST_i * 5;
                            ball.velocity.X = 3;
                            ball.velocity.Y = 0f;
                        }

                        TESTwall5.P1 = new Vector2(600, 400);
                        TESTwall5.P2 = new Vector2(400, 200);
                        TESTwall5.side = Wall.SIDE_RIGHT;
                        TESTwall5.type = Wall.TYPE_CEILING;
                        TESTwall5.Setup();

                        Note2.text = "Velocity: " + ball.velocity.Length();
                        Note3.text = "Accuracy: " + TEST_accuracy;
                        if (TEST_i >= 10)
                        {
                            //particlePool.Clear();
                            TEST_i = 0;
                            TEST_part = 0;
                        }
                    }
                    break;
                case 4:
                    if (TEST_actives == 0)
                    {
                        TEST_actives = 1;
                        TEST_i++;
                        BilliardBall ball;
                        ball = ballPool.getBall();
                        if (ball != null)
                        {
                            ball.friction = 1f;
                            ball.scale = 0.4f;
                            ball.sRadius = ball.radius * ball.scale;
                            ball.image = Global.randomBallImage();
                            ball.state = BilliardBall.STATE_TEST_4;
                            ball.location.X = 600;
                            ball.location.Y = 420 + TEST_i * 5;
                            ball.velocity.X = -3;
                            ball.velocity.Y = 0f;
                        }

                        TESTwall5.P1 = new Vector2(200, 400);
                        TESTwall5.P2 = new Vector2(400, 200);
                        TESTwall5.side = Wall.SIDE_LEFT;
                        TESTwall5.type = Wall.TYPE_CEILING;
                        TESTwall5.Setup();

                        Note2.text = "Velocity: " + ball.velocity.Length();
                        Note3.text = "Accuracy: " + TEST_accuracy;
                        if (TEST_i >= 10)
                        {
                            //particlePool.Clear();
                            TEST_i = 0;
                            TEST_part = 0;
                        }
                    }
                    break;
                case 5:
                    if (TEST_actives == 0)
                    {
                        TEST_actives = 1;
                        TEST_i++;
                        BilliardBall ball;
                        ball = ballPool.getBall();
                        if (ball != null)
                        {
                            ball.friction = 1f;
                            ball.scale = 0.4f;
                            ball.sRadius = ball.radius * ball.scale;
                            ball.image = Global.randomBallImage();
                            ball.state = BilliardBall.STATE_TEST_4;
                            ball.location.X = 600;
                            ball.location.Y = 350 + TEST_i * 5;
                            ball.velocity.X = -3;
                            ball.velocity.Y = 0f;
                        }

                        TESTwall5.P1 = new Vector2(400, 400);
                        TESTwall5.P2 = new Vector2(200, 200);
                        TESTwall5.side = Wall.SIDE_LEFT;
                        TESTwall5.type = Wall.TYPE_FLOOR;
                        TESTwall5.Setup();

                        Note2.text = "Velocity: " + ball.velocity.Length();
                        Note3.text = "Accuracy: " + TEST_accuracy;
                        if (TEST_i >= 10)
                        {
                            //particlePool.Clear();
                            TEST_i = 0;
                            TEST_part = 0;
                        }
                    }
                    break;
                default:
                    TEST_part = 0;
                    break;
            }
        }
        private void TossBallsAround()
        {
            BilliardBall ball = ballPool.getBall();
            if (ball != null)
            {
                ball.scale = 0.4f;
                ball.image = Global.randomBallImage();
                ball.state = BilliardBall.STATE_WELCOME;
                int x = Global.rand.Next(1, 3);
                if (x == 2) x = -1;
                else x = 1;
                ball.velocity.X = 3 * x;
                ball.velocity.Y = 3 * x;
                ball.friction = 1f;
                ball.sRadius = ball.radius * ball.scale;
                float sRadius = ball.sRadius;
                int side = Global.rand.Next(1, 5);
                switch (side)
                {
                    case 1:
                        ball.location.X = Global.randFloat(-sRadius * 2, Global.windowWidth + sRadius * 2);
                        ball.location.Y = -sRadius * 2;
                        break;
                    case 2:
                        ball.location.X = Global.randFloat(-sRadius * 2, Global.windowWidth + sRadius * 2);
                        ball.location.Y = Global.windowHeight + sRadius * 2;
                        break;
                    case 3:
                        ball.location.Y = Global.randFloat(-sRadius * 2, Global.windowHeight + sRadius * 2);
                        ball.location.X = -sRadius * 2;
                        break;
                    case 4:
                        ball.location.Y = Global.randFloat(-sRadius * 2, Global.windowHeight + sRadius * 2);
                        ball.location.X = Global.windowWidth + sRadius * 2;
                        break;
                }
                ball.oldLocation = ball.location;
                ball.oldVelocity = ball.velocity;
                ball.updateAbsLoc();
            }
        }
        private void HandleGameKeys()
        {
            scrollWheelValue = Global.mouseState.ScrollWheelValue;
            int diff = scrollWheelValue - prevScrollWheelValue;
            float differ;
            if (diff != 0)
            {
                differ = (diff / 120) * .01f;
                topList.scale += differ;
            }
            prevScrollWheelValue = scrollWheelValue;
        }
        public void HandleGameLogic()
        {
            calcPockets();

            switch (Global.currentPlayer.paradigm)
            {
                case BilliardBall.TYPE_CUE:
                    player1ScoreText.text = "Player1:\nChoosing Paradigm";
                    player2ScoreText.text = "Player2:\nChoosing Paradigm";
                    break;
                case BilliardBall.TYPE_8:
                    player1ScoreText.text = "Player1: 8 ball\n";
                    if (Global.pocketedSolids < Global.totalSolids)
                        player2ScoreText.text = "Player2: Solids\n" + (Global.totalSolids - Global.pocketedSolids) + " balls left";
                    else if (Global.pocketedStripes < Global.totalStripes)
                        player2ScoreText.text = "Player2: Stripes\n" + (Global.totalStripes - Global.pocketedStripes) + " balls left";
                    else
                        player2ScoreText.text = "Player2: 8 ball\n";
                    break;
                case BilliardBall.TYPE_SOLID:
                    if (Global.pocketedSolids == Global.totalSolids)
                    {
                        Global.currentPlayer.paradigm = BilliardBall.TYPE_8;
                    }
                    player1ScoreText.text = "Player1: Solids\n" + (Global.totalSolids - Global.pocketedSolids) + " balls left";
                    player2ScoreText.text = "Player2: Stripes\n" + (Global.totalStripes - Global.pocketedStripes) + " balls left";
                    break;
                case BilliardBall.TYPE_STRIPE:
                    if (Global.pocketedStripes == Global.totalStripes)
                    {
                        Global.currentPlayer.paradigm = BilliardBall.TYPE_8;
                    }
                    player2ScoreText.text = "Player2: Solids\n" + (Global.totalSolids - Global.pocketedSolids) + " balls left";
                    player1ScoreText.text = "Player1: Stripes\n" + (Global.totalStripes - Global.pocketedStripes) + " balls left";
                    break;
            }
            switch (Global.currentPlayer.state)
            {
                case Player.STATE_ACTIVE:
                    break;
                case Player.STATE_WIN:
                    changeState(STATE_WIN);
                    break;
                case Player.STATE_LOSE:
                    changeState(STATE_LOSE);
                    break;
            }
        }
        private void PanCamera(GameTime gameTime)
        {
            if (Global.currentPlayer != null)
            {
                man = Global.currentPlayer.absoluteLocation;
                mouse.X = Global.mouseState.X;
                mouse.Y = Global.mouseState.Y;

                if (mouse.X > Global.windowWidth - Global.currentPlayer.sRadius) mouse.X = Global.windowWidth - Global.currentPlayer.sRadius;
                if (mouse.Y > Global.windowHeight - Global.currentPlayer.sRadius) mouse.Y = Global.windowHeight - Global.currentPlayer.sRadius;
                if (mouse.X < 0 + Global.currentPlayer.sRadius) mouse.X = 0 + Global.currentPlayer.sRadius;
                if (mouse.Y < 0 + Global.currentPlayer.sRadius) mouse.Y = 0 + Global.currentPlayer.sRadius;

                topList.location.X = topList.location.X - (mouse.X + man.X * 2) / 3 + Global.windowWidth / 2;
                topList.location.Y = topList.location.Y - (mouse.Y + man.Y * 2) / 3 + Global.windowHeight / 2;

                Global.currentPlayer.Crosshair.absoluteLocation = mouse;
                Global.currentPlayer.Crosshair.location = mouse - topList.location;
            }
            else
            {
                topList.location = Vector2.Zero;
            }
        }
        private void GetFrameRate(GameTime gameTime)
        {
            totalMillisecs += gameTime.ElapsedRealTime.Milliseconds;
            frameCount++;
            if (frameCount >= 60)
            {
                averageFrameRate = Math.Floor(1.0 / (((double)totalMillisecs + 1.0) / 6000000.0)) / 100.0;
                totalMillisecs = 0;
                frameCount = 0;
            }
        }
        private void setupWalls()
        {
            //TODO make this part of the table object
            Wall wall;
            //west top
            wall = new Wall();
            wall.P1 = new Vector2(61, 111);
            wall.P2 = new Vector2(95, 160);
            wall.P1 += Global.currentTable.topLeftCorner;
            wall.P2 += Global.currentTable.topLeftCorner;
            wall.side = Wall.SIDE_LEFT;
            wall.type = Wall.TYPE_FLOOR;
            wall.Setup();
            walls.Add(wall, true);
            //west bottom
            wall = new Wall();
            wall.P1 = new Vector2(61, 1308);
            wall.P2 = new Vector2(95, 1260);
            wall.P1 += Global.currentTable.topLeftCorner;
            wall.P2 += Global.currentTable.topLeftCorner;
            wall.side = Wall.SIDE_LEFT;
            wall.type = Wall.TYPE_CEILING;
            wall.Setup();
            walls.Add(wall, true);
            //south west left
            wall = new Wall();
            wall.P1 = new Vector2(160, 1325);
            wall.P2 = new Vector2(111, 1359);
            wall.P1 += Global.currentTable.topLeftCorner;
            wall.P2 += Global.currentTable.topLeftCorner;
            wall.side = Wall.SIDE_RIGHT;
            wall.type = Wall.TYPE_FLOOR;
            wall.Setup();
            walls.Add(wall, true);
            //south west right
            wall = new Wall();
            wall.P1 = new Vector2(1354, 1325);
            wall.P2 = new Vector2(1384, 1359);
            wall.P1 += Global.currentTable.topLeftCorner;
            wall.P2 += Global.currentTable.topLeftCorner;
            wall.side = Wall.SIDE_LEFT;
            wall.type = Wall.TYPE_FLOOR;
            wall.Setup();
            walls.Add(wall, true);
            //south east left
            wall = new Wall();
            wall.P1 = new Vector2(1484, 1325);
            wall.P2 = new Vector2(1454, 1359);
            wall.P1 += Global.currentTable.topLeftCorner;
            wall.P2 += Global.currentTable.topLeftCorner;
            wall.side = Wall.SIDE_RIGHT;
            wall.type = Wall.TYPE_FLOOR;
            wall.Setup();
            walls.Add(wall, true);
            //south east right
            wall = new Wall();
            wall.P1 = new Vector2(2680, 1325);
            wall.P2 = new Vector2(2730, 1359);
            wall.P1 += Global.currentTable.topLeftCorner;
            wall.P2 += Global.currentTable.topLeftCorner;
            wall.side = Wall.SIDE_LEFT;
            wall.type = Wall.TYPE_FLOOR;
            wall.Setup();
            walls.Add(wall, true);
            //east bottom
            wall = new Wall();
            wall.P1 = new Vector2(2745, 1260);
            wall.P2 = new Vector2(2779, 1309);
            wall.P1 += Global.currentTable.topLeftCorner;
            wall.P2 += Global.currentTable.topLeftCorner;
            wall.side = Wall.SIDE_RIGHT;
            wall.type = Wall.TYPE_CEILING;
            wall.Setup();
            walls.Add(wall, true);
            //east top
            wall = new Wall();
            wall.P1 = new Vector2(2745, 160);
            wall.P2 = new Vector2(2779, 109);
            wall.P1 += Global.currentTable.topLeftCorner;
            wall.P2 += Global.currentTable.topLeftCorner;
            wall.side = Wall.SIDE_RIGHT;
            wall.type = Wall.TYPE_FLOOR;
            wall.Setup();
            walls.Add(wall, true);
            //north east right
            wall = new Wall();
            wall.P1 = new Vector2(2678, 95);
            wall.P2 = new Vector2(2729, 61);
            wall.P1 += Global.currentTable.topLeftCorner;
            wall.P2 += Global.currentTable.topLeftCorner;
            wall.side = Wall.SIDE_LEFT;
            wall.type = Wall.TYPE_CEILING;
            wall.Setup();
            walls.Add(wall, true);
            //north east left
            wall = new Wall();
            wall.P1 = new Vector2(1484, 95);
            wall.P2 = new Vector2(1455, 61);
            wall.P1 += Global.currentTable.topLeftCorner;
            wall.P2 += Global.currentTable.topLeftCorner;
            wall.side = Wall.SIDE_RIGHT;
            wall.type = Wall.TYPE_CEILING;
            wall.Setup();
            walls.Add(wall, true);
            //north west right
            wall = new Wall();
            wall.P1 = new Vector2(1354, 95);
            wall.P2 = new Vector2(1384, 61);
            wall.P1 += Global.currentTable.topLeftCorner;
            wall.P2 += Global.currentTable.topLeftCorner;
            wall.side = Wall.SIDE_LEFT;
            wall.type = Wall.TYPE_CEILING;
            wall.Setup();
            walls.Add(wall, true);
            //north west left
            wall = new Wall();
            wall.P1 = new Vector2(162, 95);
            wall.P2 = new Vector2(111, 61);
            wall.P1 += Global.currentTable.topLeftCorner;
            wall.P2 += Global.currentTable.topLeftCorner;
            wall.side = Wall.SIDE_RIGHT;
            wall.type = Wall.TYPE_CEILING;
            wall.Setup();
            walls.Add(wall, true);
            ////TEST bumper
            //wall = new Wall();
            //wall.P1 = new Vector2(2245, 760);
            //wall.P2 = new Vector2(2779, 109);
            //wall.type = Wall.TYPE_RIGHT;
            //wall.side = Wall.SIDE_BOTTOM;
            //wall.Setup();
            //walls.Add(wall, true);
        }
    }
}
