
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
    public class PoolOfScreenObjects : ListOfScreenObjects
    {
        public int numberOfObjects = 100;
        public int lastActivatedObject = 0;
        public override void Setup()
        {
            foreach (ScreenObject so in ScreenObjectList)
            {
                so.Setup();
                so.changeState(ScreenObject.STATE_INACTIVE);
            }
            changeState(ScreenObject.STATE_ACTIVE);
        }

        public bool ActivateObject()
        {
            int count = 0;
            lastActivatedObject = (lastActivatedObject + 1) % ScreenObjectList.Count();
            while (ScreenObjectList[lastActivatedObject].state > STATE_INACTIVE)
            {
                count++;
                if (count > ScreenObjectList.Count()) return false;
            }
            ScreenObjectList[lastActivatedObject].changeState(STATE_ACTIVE);
            return true;
        }

        public ScreenObject GetLastActivatedObject()
        {
            return ScreenObjectList[lastActivatedObject];
        }
        public override void Clear()
        {
            foreach (ScreenObject ball in ScreenObjectList)
            {
                ball.changeState(ScreenObject.STATE_KILL);
            }
        }
    }
}
