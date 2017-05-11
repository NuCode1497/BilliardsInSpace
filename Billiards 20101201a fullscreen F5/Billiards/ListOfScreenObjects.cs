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
    public class ListOfScreenObjects : ScreenObject
    {
        public List<ScreenObject> ScreenObjectList = new List<ScreenObject>();


        public virtual void Clear()
        {
            ScreenObjectList.Clear();
        }

        public override void Setup()
        {
            changeState(ScreenObject.STATE_ACTIVE);
        }

        public void Add(ScreenObject so, bool hasParent)
        {
            if (hasParent)
            {
                so.parent = this;
            }
            else
            {
                so.parent = null;
            }
            ScreenObjectList.Add(so);
        }
        public void AddList(ListOfScreenObjects list, bool hasParent)
        {
            foreach (ScreenObject so in list.ScreenObjectList)
            {
                if (hasParent)
                {
                    so.parent = this;
                }
                else
                {
                    so.parent = null;
                }
                ScreenObjectList.Add(so);
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(state > STATE_INACTIVE)
            {
                if (parent != null)
                {
                    scale = parent.scale;
                }
                foreach (ScreenObject so in ScreenObjectList)
                {
                    so.Update(gameTime);
                }
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (state)
            {
                case STATE_INACTIVE:
                    break;
                case STATE_ACTIVE:
                    foreach (ScreenObject so in ScreenObjectList)
                    {
                        so.Draw(spriteBatch);
                    }
                    break;
            }
        }

    }
}
