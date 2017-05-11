// (c) Copyright 2010 Cody Neuburger
//        All rights reserved.
//Avatar models were created from World of Warcraft by Blizzard Entertainment
//extracted from WoW Model Viewer
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
    public class Table : SpriteObject
    {
        public override void Setup()
        {
            layer = 0.1f;
            scale = 0.3f;
            location.X = 0f;
            location.Y = 0f;
            image = Global.TableImage;
            changeState(STATE_ACTIVE);
            base.Setup();
        }
        public override void Update(GameTime gameTime)
        {
            scale = parent.scale;
            base.Update(gameTime);
        }

        public float left
        {
            get
            {
                return location.X - image.Width / 2;
            }
        }
        public float right
        {
            get
            {
                return location.X + image.Width / 2;
            }
        }
        public float top
        {
            get
            {
                return location.Y - image.Height / 2;
            }
        }
        public float bottom
        {
            get
            {
                return location.Y + image.Height / 2;
            }
        }
        public Vector2 topLeftCorner
        {
            get
            {
                return new Vector2(left, top);
            }
        }
        public Vector2 bottomLeftCorner
        {
            get
            {
                return new Vector2(left, bottom);
            }
        }
        public Vector2 topRightCorner
        {
            get
            {
                return new Vector2(right, top);
            }
        }
        public Vector2 bottomRightCorner
        {
            get
            {
                return new Vector2(right, bottom);
            }
        }
    }
}
