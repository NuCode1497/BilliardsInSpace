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
    public class Particle : SpriteObject
    {
        public const int STATE_FADEOUT = 3;
        public const int STATE_TEST_1 = 4;

        public override void Setup()
        {
            image = Global.starImage;
            location.X = 0;
            location.Y = 0;
            mass = 0;
            scale = 1f;
            layer = 0.01f;
            friction = 1f;
            base.Setup();
        }

        public override void Update(GameTime gameTime)
        {
            switch (state)
            {
                case STATE_TEST_1:
                    base.Update(gameTime);
                    break;
                case STATE_INACTIVE:
                    break;
                case STATE_ACTIVE:
                    base.Update(gameTime);
                    BounceOnScreen();
                    if (hitWall)
                    {
                        changeState(STATE_INACTIVE);
                    }
                    else if (Global.probabilityPerSecond(.1))
                    {
                        changeState(STATE_FADEOUT);
                    }
                    break;
                case STATE_FADEOUT:
                    base.Update(gameTime);
                    BounceOnScreen();
                    if (hitWall)
                    {
                        changeState(STATE_INACTIVE);
                    }
                    if (color.A < 10)
                    {
                        changeState(STATE_INACTIVE);
                    }
                    else
                    {
                        color.A = (byte)(color.A - (byte)(Global.rand.Next(0, 5)));
                    }
                    break;
            }
        }
    }
}
