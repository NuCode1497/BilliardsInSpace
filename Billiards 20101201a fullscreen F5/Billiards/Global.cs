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
    public class Global
    {
        static public GraphicsDeviceManager graphics;
        static public DisplayMode displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
        static public int defaultWindowWidth = 800;
        static public int defaultWindowHeight = 600;
        static public int windowWidth
        {
            get
            {
                return Global.graphics.PreferredBackBufferWidth;
            }
            set
            {
                Global.graphics.PreferredBackBufferWidth = value;
            }
        }
        static public int windowHeight
        {
            get
            {
                return Global.graphics.PreferredBackBufferHeight;
            }
            set
            {
                Global.graphics.PreferredBackBufferHeight = value;
            }
        }
        static public int screenWidth
        {
            get
            {
                return displayMode.Width;
            }
        }
        static public int screenHeight
        {
            get
            {
                return displayMode.Height;
            }
        }
        public static float accuracy = 0.0001f; //accuracy of quicksort loops
        public static float PiOver8 = MathHelper.PiOver4 / 2;
        static public Game1 currentGame;
        static public Player currentPlayer;
        static public Table currentTable;
        static public Random rand = new Random();

        static public int score = 0;
        static public int highscore;
        static public int totalSolids;
        static public int totalStripes;
        static public int pocketedSolids;
        static public int pocketedStripes;

        #region textures
        static public Texture2D instructionsImage;
        static public Texture2D bluePlanetImage;
        static public Texture2D starImage;
        static public Texture2D NoiseBallImage;
        static public Texture2D AimStickImage;
        static public Texture2D CrosshairImage;
        static public Texture2D CueBallImage;
        static public Texture2D Ball1Image;
        static public Texture2D Ball2Image;
        static public Texture2D Ball3Image;
        static public Texture2D Ball4Image;
        static public Texture2D Ball5Image;
        static public Texture2D Ball6Image;
        static public Texture2D Ball7Image;
        static public Texture2D Ball8Image;
        static public Texture2D Ball9Image;
        static public Texture2D Ball10Image;
        static public Texture2D Ball11Image;
        static public Texture2D Ball12Image;
        static public Texture2D Ball13Image;
        static public Texture2D Ball14Image;
        static public Texture2D Ball15Image;
        static public Texture2D TableImage;
        #endregion

        #region Avatar
        static public GifAnimation Avatar_Run_N;
        static public GifAnimation Avatar_Run_E;
        static public GifAnimation Avatar_Run_S;
        static public GifAnimation Avatar_Run_W;
        static public GifAnimation Avatar_Run_NE;
        static public GifAnimation Avatar_Run_SE;
        static public GifAnimation Avatar_Run_NW;
        static public GifAnimation Avatar_Run_SW;
        static public GifAnimation Avatar_Ready_N;
        static public GifAnimation Avatar_Ready_E;
        static public GifAnimation Avatar_Ready_S;
        static public GifAnimation Avatar_Ready_W;
        static public GifAnimation Avatar_Ready_NE;
        static public GifAnimation Avatar_Ready_SE;
        static public GifAnimation Avatar_Ready_NW;
        static public GifAnimation Avatar_Ready_SW;
        static public GifAnimation Avatar_Aim_N;
        static public GifAnimation Avatar_Aim_E;
        static public GifAnimation Avatar_Aim_S;
        static public GifAnimation Avatar_Aim_W;
        static public GifAnimation Avatar_Aim_NE;
        static public GifAnimation Avatar_Aim_SE;
        static public GifAnimation Avatar_Aim_NW;
        static public GifAnimation Avatar_Aim_SW;
        static public GifAnimation Avatar_Fire_N;
        static public GifAnimation Avatar_Fire_E;
        static public GifAnimation Avatar_Fire_S;
        static public GifAnimation Avatar_Fire_W;
        static public GifAnimation Avatar_Fire_NE;
        static public GifAnimation Avatar_Fire_SE;
        static public GifAnimation Avatar_Fire_NW;
        static public GifAnimation Avatar_Fire_SW;
        static public GifAnimation Avatar_Hold_N;
        static public GifAnimation Avatar_Hold_E;
        static public GifAnimation Avatar_Hold_S;
        static public GifAnimation Avatar_Hold_W;
        static public GifAnimation Avatar_Hold_NE;
        static public GifAnimation Avatar_Hold_SE;
        static public GifAnimation Avatar_Hold_NW;
        static public GifAnimation Avatar_Hold_SW;
        #endregion

        #region sounds
        static public SoundEffect ChargeSound;
        static public SoundEffect ChargeLoopSound;
        static public SoundEffect FireGunSound;
        static public SoundEffect FlakCannonSound;
        static public SoundEffect ClackSound;
        static public SoundEffect PocketSound;
        static public SoundEffect ThwopSound;
        static public SoundEffect PlopSound;
        static public SoundEffect BumperSound;
        #endregion

        static public SpriteFont ArielFont;
        static public SpriteFont BodoniFont;

        static public KeyboardState keyboardState;
        static public MouseState mouseState;
        static public void ToggleFullscreen()
        {
            if (!graphics.IsFullScreen)
            {
                graphics.IsFullScreen = true;
                Global.windowHeight = Global.screenHeight;
                Global.windowWidth = Global.screenWidth;
                graphics.ApplyChanges();
            }
            else
            {
                graphics.IsFullScreen = false;
                Global.windowHeight = Global.defaultWindowHeight;
                Global.windowWidth = Global.defaultWindowWidth;
                graphics.ApplyChanges();
            }
        }
        static public void Load(ContentManager Content)
        {
            #region Avatar
            Avatar_Run_N = Content.Load<GifAnimation>(@"Avatar\Avatar_Run_N");
            Avatar_Run_E = Content.Load<GifAnimation>(@"Avatar\Avatar_Run_E");
            Avatar_Run_S = Content.Load<GifAnimation>(@"Avatar\Avatar_Run_S");
            Avatar_Run_W = Content.Load<GifAnimation>(@"Avatar\Avatar_Run_W");
            Avatar_Run_NE = Content.Load<GifAnimation>(@"Avatar\Avatar_Run_NE");
            Avatar_Run_SE = Content.Load<GifAnimation>(@"Avatar\Avatar_Run_SE");
            Avatar_Run_NW = Content.Load<GifAnimation>(@"Avatar\Avatar_Run_NW");
            Avatar_Run_SW = Content.Load<GifAnimation>(@"Avatar\Avatar_Run_SW");
            Avatar_Ready_N = Content.Load<GifAnimation>(@"Avatar\Avatar_Ready_N");
            Avatar_Ready_E = Content.Load<GifAnimation>(@"Avatar\Avatar_Ready_E");
            Avatar_Ready_S = Content.Load<GifAnimation>(@"Avatar\Avatar_Ready_S");
            Avatar_Ready_W = Content.Load<GifAnimation>(@"Avatar\Avatar_Ready_W");
            Avatar_Ready_NE = Content.Load<GifAnimation>(@"Avatar\Avatar_Ready_NE");
            Avatar_Ready_SE = Content.Load<GifAnimation>(@"Avatar\Avatar_Ready_SE");
            Avatar_Ready_NW = Content.Load<GifAnimation>(@"Avatar\Avatar_Ready_NW");
            Avatar_Ready_SW = Content.Load<GifAnimation>(@"Avatar\Avatar_Ready_SW");
            Avatar_Aim_N = Content.Load<GifAnimation>(@"Avatar\Avatar_Aim_N");
            Avatar_Aim_E = Content.Load<GifAnimation>(@"Avatar\Avatar_Aim_E");
            Avatar_Aim_S = Content.Load<GifAnimation>(@"Avatar\Avatar_Aim_S");
            Avatar_Aim_W = Content.Load<GifAnimation>(@"Avatar\Avatar_Aim_W");
            Avatar_Aim_NE = Content.Load<GifAnimation>(@"Avatar\Avatar_Aim_NE");
            Avatar_Aim_SE = Content.Load<GifAnimation>(@"Avatar\Avatar_Aim_SE");
            Avatar_Aim_NW = Content.Load<GifAnimation>(@"Avatar\Avatar_Aim_NW");
            Avatar_Aim_SW = Content.Load<GifAnimation>(@"Avatar\Avatar_Aim_SW");
            Avatar_Fire_N = Content.Load<GifAnimation>(@"Avatar\Avatar_Fire_N");
            Avatar_Fire_E = Content.Load<GifAnimation>(@"Avatar\Avatar_Fire_E");
            Avatar_Fire_S = Content.Load<GifAnimation>(@"Avatar\Avatar_Fire_S");
            Avatar_Fire_W = Content.Load<GifAnimation>(@"Avatar\Avatar_Fire_W");
            Avatar_Fire_NE = Content.Load<GifAnimation>(@"Avatar\Avatar_Fire_NE");
            Avatar_Fire_SE = Content.Load<GifAnimation>(@"Avatar\Avatar_Fire_SE");
            Avatar_Fire_NW = Content.Load<GifAnimation>(@"Avatar\Avatar_Fire_NW");
            Avatar_Fire_SW = Content.Load<GifAnimation>(@"Avatar\Avatar_Fire_SW");
            Avatar_Hold_N = Content.Load<GifAnimation>(@"Avatar\Avatar_Hold_N");
            Avatar_Hold_E = Content.Load<GifAnimation>(@"Avatar\Avatar_Hold_E");
            Avatar_Hold_S = Content.Load<GifAnimation>(@"Avatar\Avatar_Hold_S");
            Avatar_Hold_W = Content.Load<GifAnimation>(@"Avatar\Avatar_Hold_W");
            Avatar_Hold_NE = Content.Load<GifAnimation>(@"Avatar\Avatar_Hold_NE");
            Avatar_Hold_SE = Content.Load<GifAnimation>(@"Avatar\Avatar_Hold_SE");
            Avatar_Hold_NW = Content.Load<GifAnimation>(@"Avatar\Avatar_Hold_NW");
            Avatar_Hold_SW = Content.Load<GifAnimation>(@"Avatar\Avatar_Hold_SW");
            #endregion Avatar

            #region sounds
            ChargeSound = Content.Load<SoundEffect>(@"Sounds\Charge");
            ChargeLoopSound = Content.Load<SoundEffect>(@"Sounds\ChargeLoop");
            FireGunSound = Content.Load<SoundEffect>(@"Sounds\Fire");
            ClackSound = Content.Load<SoundEffect>(@"Sounds\clack");
            PocketSound = Content.Load<SoundEffect>(@"Sounds\pocket");
            ThwopSound = Content.Load<SoundEffect>(@"Sounds\thwop");
            PlopSound = Content.Load<SoundEffect>(@"Sounds\FingerPlop4");
            BumperSound = Content.Load<SoundEffect>(@"Sounds\dup");
            #endregion

            #region textures
            instructionsImage = Content.Load<Texture2D>(@"Instructions");
            starImage = Content.Load<Texture2D>(@"star");
            AimStickImage = Content.Load<Texture2D>("AimStick");
            CrosshairImage = Content.Load<Texture2D>("crosshair");
            bluePlanetImage = Content.Load<Texture2D>(@"Balls\bluePlanet");
            NoiseBallImage = Content.Load<Texture2D>(@"Balls\NoiseBall");
            CueBallImage = Content.Load<Texture2D>(@"Balls\cueball");
            Ball1Image = Content.Load<Texture2D>(@"Balls\1ball");
            Ball2Image = Content.Load<Texture2D>(@"Balls\2ball");
            Ball3Image = Content.Load<Texture2D>(@"Balls\3ball");
            Ball4Image = Content.Load<Texture2D>(@"Balls\4ball");
            Ball5Image = Content.Load<Texture2D>(@"Balls\5ball");
            Ball6Image = Content.Load<Texture2D>(@"Balls\6ball");
            Ball7Image = Content.Load<Texture2D>(@"Balls\7ball");
            Ball8Image = Content.Load<Texture2D>(@"Balls\8ball");
            Ball9Image = Content.Load<Texture2D>(@"Balls\9ball");
            Ball10Image = Content.Load<Texture2D>(@"Balls\10ball");
            Ball11Image = Content.Load<Texture2D>(@"Balls\11ball");
            Ball12Image = Content.Load<Texture2D>(@"Balls\12ball");
            Ball13Image = Content.Load<Texture2D>(@"Balls\13ball");
            Ball14Image = Content.Load<Texture2D>(@"Balls\14ball");
            Ball15Image = Content.Load<Texture2D>(@"Balls\15ball");
            TableImage = Content.Load<Texture2D>(@"Table");
            #endregion

            ArielFont = Content.Load<SpriteFont>("SpriteFont1");
            BodoniFont = Content.Load<SpriteFont>("SpriteFont2");
        }
        static public Texture2D randomBallImage()
        {
            Texture2D image = null;
            int x = rand.Next(1, 18);
            switch (x)
            {
                case 1:
                    image = Ball1Image;
                    break;
                case 2:
                    image = Ball2Image;
                    break;
                case 3:
                    image = Ball3Image;
                    break;
                case 4:
                    image = Ball4Image;
                    break;
                case 5:
                    image = Ball5Image;
                    break;
                case 6:
                    image = Ball6Image;
                    break;
                case 7:
                    image = Ball7Image;
                    break;
                case 8:
                    image = Ball8Image;
                    break;
                case 9:
                    image = Ball9Image;
                    break;
                case 10:
                    image = Ball10Image;
                    break;
                case 11:
                    image = Ball11Image;
                    break;
                case 12:
                    image = Ball12Image;
                    break;
                case 13:
                    image = Ball13Image;
                    break;
                case 14:
                    image = Ball14Image;
                    break;
                case 15:
                    image = Ball15Image;
                    break;
                case 16:
                    image = CueBallImage;
                    break;
                case 17:
                    image = NoiseBallImage;
                    break;
            }
            return image;
        }
        static public float randFloat()
        {
            return (float)rand.NextDouble();
        }
        static public float randFloat(float max)
        {
            return (float)rand.NextDouble() * max;
        }
        static public float randFloat(float min, float max)
        {
            return ((float)rand.NextDouble() * (max - min)) + min;
        }
        static public void randomizeColor(ref Color c)
        {
            c.A = 255;
            c.R = (byte)rand.Next(256);
            c.G = (byte)rand.Next(256);
            c.B = (byte)rand.Next(256);
        }
        static public void randomizeBrightColor(ref Color c)
        {
            c.A = 255;
            c.R = (byte)rand.Next(240, 256);
            c.G = (byte)rand.Next(240, 256);
            c.B = (byte)rand.Next(240, 256);

        }
        static public float randAngle()
        {
            return (float)(rand.NextDouble() * MathHelper.TwoPi);
        }
        static public Vector2 randomTableSpot()
        {//does not take into account ball radius
            if (currentGame.Table != null)
            {
                Vector2 spot = new Vector2();

                spot.X = currentGame.Table.location.X + (float)((randFloat(-1325, 1325)));
                spot.Y = currentGame.Table.location.Y + (float)((randFloat(-615, 615)));
                return spot;
            }
            return Vector2.Zero;
        }
        static public Vector2 findRackSpot(int type)
        {
            Vector2 spot = new Vector2();
            switch (type)
            {
                case BilliardBall.TYPE_8:
                    spot = new Vector2(2241, 711);
                    break;
                case BilliardBall.TYPE_CUE:
                    spot = new Vector2(711, 711);
                    break;
                case BilliardBall.TYPE_SOLID:
                    switch (rand.Next(1,7))
                    {
                        case 1:
                            spot = new Vector2(2131, 711);
                    break;
                        case 2:
                            spot = new Vector2(2186, 680);
                    break;
                        case 3:
                            spot = new Vector2(2241, 773);
                    break;
                        case 4:
                            spot = new Vector2(2296, 618);
                    break;
                        case 5:
                            spot = new Vector2(2296, 742);
                    break;
                        case 6:
                            spot = new Vector2(2351, 649);
                    break;
                        case 7:
                            spot = new Vector2(2351, 835);
                    break;
                    }
                    break;
                case BilliardBall.TYPE_STRIPE:
                    switch (rand.Next(1, 7))
                    {
                        case 1:
                            spot = new Vector2(2186, 742);
                    break;
                        case 2:
                            spot = new Vector2(2241, 649);
                    break;
                        case 3:
                            spot = new Vector2(2296, 680);
                    break;
                        case 4:
                            spot = new Vector2(2296, 804);
                    break;
                        case 5:
                            spot = new Vector2(2351, 587);
                    break;
                        case 6:
                            spot = new Vector2(2351, 711);
                    break;
                        case 7:
                            spot = new Vector2(2351, 773);
                    break;
                    }
                    break;
            }
            spot += currentTable.topLeftCorner;
            return spot;
        }
        static public Vector2 randomScreenSpot()
        {//not working correctly?
            if (currentGame.Table != null)
            {
                Vector2 spot = new Vector2();

                spot.X = randFloat(0, windowWidth);
                spot.Y = randFloat(0, windowHeight);
                return spot;
            }
            return Vector2.Zero;
        }
        static public bool probability(double p)
        {
            return rand.NextDouble() < p;
        }
        static public bool probabilityPerSecond(double p)
        {
            double frameRate = 60.0;
            return rand.NextDouble() < (p / frameRate);
        }

        static public Vector2 intersection(Line L1, Line L2)
        {//unused
            //http://thirdpartyninjas.com/blog/2008/10/07/line-segment-intersection/
            //2010-11-20

            float ua = (L2.P2.X - L2.P1.X) * (L1.P1.Y - L2.P1.Y) - (L2.P2.Y - L2.P1.Y) * (L1.P1.X - L2.P1.X);
            float ub = (L1.P2.X - L1.P1.X) * (L1.P1.Y - L2.P1.Y) - (L1.P2.Y - L1.P1.Y) * (L1.P1.X - L2.P1.X);
            float denominator = (L2.P2.Y - L2.P1.Y) * (L1.P2.X - L1.P1.X) - (L2.P2.X - L2.P1.X) * (L1.P2.Y - L1.P1.Y);
            Vector2 intersectionPoint = new Vector2(float.NaN, float.NaN);

            if (Math.Abs(denominator) <= 0.00001f)
            {
                if (Math.Abs(ua) <= 0.00001f && Math.Abs(ub) <= 0.00001f)
                {
                    intersectionPoint = (L1.P1 + L1.P2) / 2;
                }
            }
            else
            {
                ua /= denominator;
                ub /= denominator;

                if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
                {
                    intersectionPoint.X = L1.P1.X + ua * (L1.P2.X - L1.P1.X);
                    intersectionPoint.Y = L1.P1.Y + ua * (L1.P2.Y - L1.P1.Y);
                }
            }
            return intersectionPoint;
        }
    }
}
