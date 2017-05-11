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
    public class Wall : ScreenObject
    {
        public const int SIDE_LEFT = 0; //wall pushes objects in +X direction
        public const int SIDE_RIGHT = 1; //wall pushes objects in -X direction
        public const int TYPE_CEILING = 0; // wall is -Y relative to object
        public const int TYPE_FLOOR = 1; // wall is +Y relative to object
        public int type;
        public int side;

        private Line line = new Line();
        private Line lineT = new Line();
        private Line lineB = new Line();
        private Line lineL = new Line();
        private Line lineR = new Line();
        public Vector2 _P1; //original scaled
        public Vector2 _P2;
        public Vector2 P1 = new Vector2();
        public Vector2 P2 = new Vector2();
        public Vector2 sP1 = new Vector2();
        public Vector2 sP2 = new Vector2();

        public override void Setup()
        {
            line.color = Color.Blue;
            line.P1 = P1;
            line.P2 = P2;
            line.width = 2;
            line.layer = .99f;
            line.Setup();

            lineB.color = Color.PaleGoldenrod;
            lineB.width = 2;
            lineB.layer = .99f;
            lineB.Setup();

            lineT.color = Color.PaleGoldenrod;
            lineT.width = 2;
            lineT.layer = .99f;
            lineT.Setup();

            lineL.color = Color.PaleGoldenrod;
            lineL.width = 2;
            lineL.layer = .99f;
            lineL.Setup();

            lineR.color = Color.PaleGoldenrod;
            lineR.width = 2;
            lineR.layer = .99f;
            lineR.Setup();



            collideType = COLLIDE_TYPE_WALL;
            _P1 = P1;
            _P2 = P2;
            sP1 = P1;
            sP2 = P2;
            changeState(STATE_ACTIVE);
            base.Setup();
        }

        // -pi/2 < theta < pi/2
        public double theta
        {
            get { return Math.Atan2(rightPoint.Y - leftPoint.Y, rightPoint.X - leftPoint.X); }
        }
        public double phi
        {
            get
            {
                switch (type)
                {
                    case TYPE_FLOOR:
                        return MathHelper.PiOver2 + theta;
                    case TYPE_CEILING:
                        return -MathHelper.PiOver2 + theta;
                    default:
                        return 0;
                }
            }
        }
        public float m
        {
            get { return ((rightPoint.Y - leftPoint.Y) / (rightPoint.X - leftPoint.X)); }
        }
        public Vector2 leftPoint
        {
            get
            {
                if (P1.X < P2.X) return P1;
                else return P2;
            }
        }
        public Vector2 rightPoint
        {
            get
            {
                if (P1.X > P2.X) return P1;
                else return P2;
            }
        }
        public Vector2 topPoint
        {
            get
            {
                if (P1.Y < P2.Y) return P1;
                else return P2;
            }
        }
        public Vector2 bottomPoint
        {
            get
            {
                if (P1.Y > P2.Y) return P1;
                else return P2;
            }
        }
        public float left
        {
            get { return Math.Min(P1.X, P2.X); }
        }
        public float right
        {
            get { return Math.Max(P1.X, P2.X); }
        }
        public float top
        {
            get { return Math.Min(P1.Y, P2.Y); }
        }
        public float bottom
        {
            get { return Math.Max(P1.Y, P2.Y); }
        }
        public Vector2 sleftPoint
        {
            get
            {
                if (sP1.X < sP2.X) return sP1;
                else return sP2;
            }
        }
        public Vector2 srightPoint
        {
            get
            {
                if (sP1.X > sP2.X) return sP1;
                else return sP2;
            }
        }
        public Vector2 stopPoint
        {
            get
            {
                if (sP1.Y < sP2.Y) return sP1;
                else return sP2;
            }
        }
        public Vector2 sbottomPoint
        {
            get
            {
                if (sP1.Y > sP2.Y) return sP1;
                else return sP2;
            }
        }
        public float sleft
        {
            get { return Math.Min(sP1.X, sP2.X); }
        }
        public float sright
        {
            get { return Math.Max(sP1.X, sP2.X); }
        }
        public float stop
        {
            get { return Math.Min(sP1.Y, sP2.Y); }
        }
        public float sbottom
        {
            get { return Math.Max(sP1.Y, sP2.Y); }
        }
        public override void Update(GameTime gameTime)
        {
            if (parent != null) scale = parent.scale;
            base.Update(gameTime);
 
            switch (Global.currentGame.state)
            {
                case Game1.STATE_GAMEPLAY:
                    P1 = (_P1 + location);
                    P2 = (_P2 + location);
                    sP1 = P1 * parent.scale + parent.absoluteLocation;
                    sP2 = P2 * parent.scale + parent.absoluteLocation;
                    break;
                case Game1.STATE_TEST_4:
                case Game1.STATE_TEST_4_aDown:
                case Game1.STATE_TEST_4_aUp:
                    
                break;
            }
            line.P1 = sP1;
            line.P2 = sP2;
            lineT.P1.X = sleft;
            lineT.P1.Y = stop;
            lineT.P2.X = sright;
            lineT.P2.Y = stop;
            lineL.P1.X = sleft;
            lineL.P1.Y = stop;
            lineL.P2.X = sleft;
            lineL.P2.Y = sbottom;
            lineR.P1.X = sright;
            lineR.P1.Y = stop;
            lineR.P2.X = sright;
            lineR.P2.Y = sbottom;
            lineB.P1.X = sleft;
            lineB.P1.Y = sbottom;
            lineB.P2.X = sright;
            lineB.P2.Y = sbottom;

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            line.Draw(spriteBatch);
            lineB.Draw(spriteBatch);
            lineT.Draw(spriteBatch);
            lineL.Draw(spriteBatch);
            lineR.Draw(spriteBatch);
        }
    }
}
