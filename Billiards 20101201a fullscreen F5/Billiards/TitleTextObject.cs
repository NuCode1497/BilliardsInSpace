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
    public class TitleTextObject : TextObject
    {

        public override void Setup()
        {
            location.X = 50f;
            location.Y = Global.windowHeight - 50f;
            text = "Cody Neuburger";
            color = Color.RosyBrown;
            scale = 2.0f;
            base.Setup();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

    }
}
