// (c) Copyright 2010 Cody Neuburger
//        All rights reserved.
//Avatar models were created from World of Warcraft by Blizzard Entertainment
//using WoW Model Viewer
//I am in no way affiliated with Blizzard
//Some sounds are from http://www.freesound.org

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
using XNAGifAnimationLibrary;

namespace Billiards
{
    public class AnimatedObject : ScreenObject
    {
        public GifAnimation animation = Global.Avatar_Run_W;
        public int speedControl = 2;

        public override void Setup()
        {
            origin.X = animation.Width / 2.0f;
            origin.Y = animation.Height / 2.0f;
        }

        public override void Update(GameTime gameTime)
        {
            animation.Update(gameTime.ElapsedGameTime.Ticks * (int)(speedControl / 2));
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (state != STATE_INACTIVE)
            {
                spriteBatch.Draw(animation.GetTexture(), absoluteLocation, null, Color.White, rotation, origin, scale, spriteEffects, layer);
            }
        }

    }
}
