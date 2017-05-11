// (c) Copyright 2010 Dr. Thomas Fernandez
//          All rights reserved.

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
    public class SpriteObject : ScreenObject
    {
        public Texture2D image = Global.CueBallImage;

        public override void Setup()
        {
            origin.X = image.Width / 2.0f;
            origin.Y = image.Height / 2.0f;
            base.Setup();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (state > STATE_INACTIVE)
            {
                spriteBatch.Draw(image, absoluteLocation, null, color, rotation, origin, scale, spriteEffects, layer);
            }
        }

    }
}
