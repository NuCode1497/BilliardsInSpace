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
    public class ListOfWalls : ScreenObject
    {
        public const int STATE_DRAW_LINES = 3;
        
        public List<Wall> WallList = new List<Wall>();

        public virtual void Clear()
        {
            WallList.Clear();
        }

        public override void Setup()
        {
            changeState(STATE_ACTIVE);
        }

        public void Add(Wall wall, bool hasParent)
        {
            if (hasParent)
            {
                wall.parent = this;
            }
            else
            {
                wall.parent = null;
            }
            WallList.Add(wall);
        }
        public void AddList(ListOfWalls list, bool hasParent)
        {
            foreach (Wall wall in list.WallList)
            {
                if (hasParent)
                {
                    wall.parent = this;
                }
                else
                {
                    wall.parent = null;
                }
                WallList.Add(wall);
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            switch (state)
            {
                case STATE_INACTIVE:
                    break;
                case STATE_DRAW_LINES:
                case STATE_ACTIVE:
                    scale = Global.currentGame.Table.scale;
                    foreach (Wall wall in WallList)
                    {
                        wall.Update(gameTime);
                    }
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
                    break;
                case STATE_DRAW_LINES:
                    foreach (Wall so in WallList)
                    {
                        so.Draw(spriteBatch);
                    }
                    break;
            }
        }

    }
}
