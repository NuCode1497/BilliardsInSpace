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
    public class BackGround : ListOfScreenObjects
    {

        public override void Setup()
        {
            friction = 1.0f;

            ScreenObject so;
            for (int i = 0; i < 1; i++)
            {
                so = new Planet();
                so.Setup();
                Add(so, false);
            }
            base.Setup();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            BounceOnScreen();
            if (Global.probability(0.15))
            {
                Particle p = Global.currentGame.particlePool.getParticle();
                if (p != null)
                {
                    p.location.X = Global.randFloat(Global.windowWidth);
                    p.location.Y = Global.randFloat(Global.windowHeight);
                    p.velocity.X = Global.randFloat(-.2f, -.05f);
                    p.velocity.Y = -.1f;
                    p.scale = Global.randFloat();
                    Global.randomizeBrightColor(ref p.color);
                    p.changeState(STATE_ACTIVE);
                }
            }
        }

    }
}
