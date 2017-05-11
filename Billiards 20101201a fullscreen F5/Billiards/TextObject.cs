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
    public class TextObject : ScreenObject
    {
        public string text = "";

        SpriteFont spriteFont = Global.ArielFont;

        public override void Setup()
        {
            changeState(STATE_ACTIVE);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            switch (state)
            {
                case STATE_INACTIVE:
                    break;
                case STATE_ACTIVE:
                    break;
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (state)
            {
                case STATE_INACTIVE:
                    break;
                case STATE_ACTIVE:
                    spriteBatch.DrawString(spriteFont, text, absoluteLocation, color, rotation, origin, scale, spriteEffects, layer);
                    break;
            }
        }

    }
}
