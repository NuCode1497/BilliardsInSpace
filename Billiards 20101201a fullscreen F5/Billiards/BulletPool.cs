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
    public class BulletPool : PoolOfScreenObjects
    {
        public override void Setup()
        {
            for (int i = 0; i < numberOfObjects; i++)
            {
                Bullet bullet = new Bullet();
                Add(bullet, false);
            }
            base.Setup();
        }

        public Bullet getBullet()
        {
            if (ActivateObject())
            {
                return (Bullet)GetLastActivatedObject();
            }
            else
            {
                return null;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
