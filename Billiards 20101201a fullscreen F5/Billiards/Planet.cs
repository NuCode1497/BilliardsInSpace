//
// Copyright (c) 2010 Cody Neuburger
// All rights reserved.
//

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
    public class Planet : SpriteObject
    {
        public const int STATE_PREPARING_TO_FLY = 10;
        public const int STATE_FLYING = 11;


        public override void Setup()
        {
            state = STATE_FLYING;
            location.X = Global.randFloat() * Global.windowWidth;
            location.Y = Global.randFloat() * Global.windowHeight;
            velocity.X = Global.randFloat(-0.05f, 0.05f);
            velocity.Y = Global.randFloat(-0.05f, 0.05f);
            rotation = Global.randAngle();
            angularVelocity = Global.randFloat(-0.0003f, 0.0003f);
            layer = .02f;
            scale = 2.5f;


            friction = 1.0f;

            image = Global.bluePlanetImage;

            base.Setup();

            //color.A = 140;

        }





        public override void Update(GameTime gameTime)
        {

            switch (state)
            {
                case STATE_INACTIVE:
                    break;
                case STATE_PREPARING_TO_FLY:
                    Setup();
                    changeState(STATE_FLYING);
                    break;
                case STATE_FLYING:
                    break;
            }
            base.Update(gameTime);
            BounceOnScreen();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (state)
            {
                case STATE_INACTIVE:
                case STATE_PREPARING_TO_FLY:
                    break;
                case STATE_FLYING:
                    base.Draw(spriteBatch);
                    break;
            }

        }

    }
}
