// (c) Copyright 2010 Dr. Thomas Fernandez
//          All rights reserved.

// http://www.xnawiki.com/index.php?title=Drawing_2D_lines_without_using_primitives
// 2010-11-15

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


namespace Billiards
{
    public class Line : GameObject
    {
        Texture2D blank;
        public Color color;
        public float width;
        public Vector2 P1;
        public Vector2 P2;
        public float layer;

        public override void Setup()
        {
            blank = new Texture2D(Global.currentGame.GraphicsDevice, 1, 1, 1, TextureUsage.None, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            base.Setup();
        }

        public override void Draw(SpriteBatch batch)
        {
            float angle = (float)Math.Atan2(P2.Y - P1.Y, P2.X - P1.X);
            float length = Vector2.Distance(P1, P2);

            batch.Draw(blank, P1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, layer);
        }
        public double theta
        {
            get { return Math.Atan2(rightPoint.Y - leftPoint.Y, rightPoint.X - leftPoint.X); }
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
    }
}
