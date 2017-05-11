// (c) Copyright 2010 Cody Neuburger
//        All rights reserved.
//Avatar models were created from World of Warcraft by Blizzard Entertainment
//using WoW Model Viewer
//I am in no way affiliated with Blizzard
//Some sounds are from http://www.freesound.org

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using XNAGifAnimationLibrary;

namespace Billiards
{
    public class Player : ListOfScreenObjects
    {
        public bool autoMode = false;

        public SpriteObject Crosshair;
        public Avatar avatar;

        public int paradigm;

        public const int STATE_WIN = 3;
        public const int STATE_LOSE = 4;
        public const int STATE_NEXTPLAYER = 5;

        public override void Setup()
        {
            collideType = COLLIDE_TYPE_MAN;
            paradigm = BilliardBall.TYPE_CUE; //solids
            location = Vector2.Zero;
            velocity = Vector2.Zero;
            rotation = 0.0f;
            angularVelocity = 0.0f;
            angularAcceleration = 0.0f;
            friction = 0.85f;
            layer = 0.5f;
            radius = 20;
            scale = 2 * Global.currentGame.Table.scale;
            mass = .001f * (float)(Math.Pow(10, 1));

            Crosshair = new SpriteObject();
            Crosshair.image = Global.CrosshairImage;
            Crosshair.Setup();
            Crosshair.layer = 0.61f;
            Crosshair.state = ScreenObject.STATE_ACTIVE;

            //avatar modifies player list, must be updated seperatly
            avatar = new Avatar();
            avatar.Setup();
            avatar.parent = this;

            base.Setup();
        }
        public override void Update(GameTime gameTime)
        {
            switch (state)
            {
                case STATE_ACTIVE:
                    break;
                case STATE_WIN:
                    break;
                case STATE_LOSE:
                    break;
                case STATE_INACTIVE:
                    break;
            }

            scale = 2 * Global.currentGame.Table.scale;
            if (autoMode) avatar.autoMode = true;
            else avatar.autoMode = false;

            avatar.Update(gameTime);
            base.Update(gameTime);
            BounceOnWalls();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Crosshair.Draw(spriteBatch);
            avatar.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
