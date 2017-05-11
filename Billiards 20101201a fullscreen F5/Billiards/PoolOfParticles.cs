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
    public class PoolOfParticles : PoolOfScreenObjects
    {
        public override void Setup()
        {
            for (int i = 0; i < numberOfObjects; i++)
            {
                Particle particle = new Particle();
                Add(particle, true);
            }
            base.Setup();
        }

        public Particle getParticle()
        {
            if (ActivateObject())
            {
                return (Particle)GetLastActivatedObject();
            }
            else
            {
                return null;
            }
        }

        public void MultiActivateExplosion(Vector2 loc, int howMany)
        {
            for (int i = 0; i < howMany; i++)
            {
                Particle p = getParticle();
                if (p != null)
                {
                    float r = Global.randFloat(0.05f, 12.0f);
                    float theta = Global.randAngle();
                    p.location = loc;
                    p.velocity.X = r * (float)Math.Cos(theta);
                    p.velocity.Y = r * (float)Math.Sin(theta);
                    p.changeState(Particle.STATE_ACTIVE);
                }
                else
                {
                    break;
                }
            }
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
