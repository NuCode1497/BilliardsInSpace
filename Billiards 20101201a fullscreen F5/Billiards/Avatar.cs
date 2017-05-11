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
    public class Avatar : AnimatedObject
    {
        SoundEffectInstance ChargeLoopSound;
        SoundEffectInstance ChargeSound;
        Bullet curBullet = null;

        public const int DIRECTION_N = 0;
        public const int DIRECTION_E = 1;
        public const int DIRECTION_S = 2;
        public const int DIRECTION_W = 3;
        public const int DIRECTION_NE = 4;
        public const int DIRECTION_SE = 5;
        public const int DIRECTION_NW = 6;
        public const int DIRECTION_SW = 7;
        public const int STATE_RUN = 13;
        public const int STATE_READY0 = 3;
        public const int STATE_READY1 = 12;
        public const int STATE_AIM0 = 4;
        public const int STATE_AIM1 = 5;
        public const int STATE_AIM2 = 11;
        public const int STATE_FIRE0 = 6;
        public const int STATE_FIRE1 = 7;
        public const int STATE_HOLD0 = 8;
        public const int STATE_HOLD1 = 9;
        public const int STATE_CANCEL = 10;

        public bool autoMode = false;
        public bool autoKeyW = false;
        public bool autoKeyA = false;
        public bool autoKeyS = false;
        public bool autoKeyD = false;
        public bool autoMouse = false;
        public bool autoMouse2 = false;
        public double autoAngle = 0;
        public double autoAngleInc = 0;

        int facingDirection;
        double facingAngle;

        public override void Setup()
        {
            collideType = COLLIDE_TYPE_MAN;
            speedControl = 8;
            ChargeLoopSound = Global.ChargeLoopSound.CreateInstance();
            ChargeLoopSound.IsLooped = true;
            ChargeSound = Global.ChargeSound.CreateInstance();

            facingAngle = 0;
            facingDirection = DIRECTION_W;
            animation = Global.Avatar_Run_W;
            location = Vector2.Zero;
            velocity = Vector2.Zero;
            rotation = 0;
            angularVelocity = 0.0f;
            angularAcceleration = 0.0f;
            friction = 0.85f;
            layer = 0.5f;
            radius = 20;
            scale = 2 * Global.currentGame.Table.scale;
            mass = .001f * (float)(Math.Pow(10, 1));
            changeState(STATE_ACTIVE);
            base.Setup();
        }

        public override void Update(GameTime gameTime)
        {
            scale = parent.scale;
            base.Update(gameTime);

            switch (state)
            {
                case STATE_KILL:
                    changeState(STATE_DEATH);
                    break;
                case STATE_DEATH:
                    changeState(STATE_INACTIVE);
                    break;
                case STATE_INACTIVE:
                    break;
                case STATE_ACTIVE:
                    changeState(STATE_READY0);
                    break;
                case STATE_RUN:
                    #region RUN
                    GuideWithKeyboard();
                    if (autoMode)
                    {
                        if (Global.probabilityPerSecond(.5))
                        {
                            parent.velocity = Vector2.Zero;
                            changeState(STATE_AIM0);
                            break;
                        }

                        if (Global.probabilityPerSecond(.01))
                        {
                            changeState(STATE_READY0);
                        }
                    }
                    else
                    {
                        if (Global.mouseState.LeftButton == ButtonState.Pressed)
                        {
                            parent.velocity = Vector2.Zero;
                            changeState(STATE_AIM0);
                            break;
                        }
                    }
                    switch (facingDirection)
                    {
                        case DIRECTION_N:
                            animation = Global.Avatar_Run_N;
                            break;
                        case DIRECTION_E:
                            animation = Global.Avatar_Run_E;
                            break;
                        case DIRECTION_S:
                            animation = Global.Avatar_Run_S;
                            break;
                        case DIRECTION_W:
                            animation = Global.Avatar_Run_W;
                            break;
                        case DIRECTION_NE:
                            animation = Global.Avatar_Run_NE;
                            break;
                        case DIRECTION_SE:
                            animation = Global.Avatar_Run_SE;
                            break;
                        case DIRECTION_NW:
                            animation = Global.Avatar_Run_NW;
                            break;
                        case DIRECTION_SW:
                            animation = Global.Avatar_Run_SW;
                            break;
                    }
                    #endregion
                    break;
                case STATE_READY0:
                    #region READY
                    GuideWithKeyboard();
                    if (Global.mouseState.LeftButton == ButtonState.Pressed)
                    {
                        parent.velocity = Vector2.Zero;
                        changeState(STATE_AIM0);
                        break;
                    }

                    if (Global.keyboardState.IsKeyDown(Keys.E))
                    {
                        changeState(STATE_READY1);
                        parent.location = Global.randomTableSpot();
                        break;
                    }
                    switch (facingDirection)
                    {
                        case DIRECTION_N:
                            animation = Global.Avatar_Ready_N;
                            break;
                        case DIRECTION_E:
                            animation = Global.Avatar_Ready_E;
                            break;
                        case DIRECTION_S:
                            animation = Global.Avatar_Ready_S;
                            break;
                        case DIRECTION_W:
                            animation = Global.Avatar_Ready_W;
                            break;
                        case DIRECTION_NE:
                            animation = Global.Avatar_Ready_NE;
                            break;
                        case DIRECTION_SE:
                            animation = Global.Avatar_Ready_SE;
                            break;
                        case DIRECTION_NW:
                            animation = Global.Avatar_Ready_NW;
                            break;
                        case DIRECTION_SW:
                            animation = Global.Avatar_Ready_SW;
                            break;
                    }
                    #endregion
                    break;
                case STATE_READY1:
                    #region READY
                    GuideWithKeyboard();
                    if (autoMode)
                    {
                        if (Global.probabilityPerSecond(5))
                        {
                            parent.velocity = Vector2.Zero;
                            changeState(STATE_AIM0);
                            break;
                        }

                        if (Global.probabilityPerSecond(.01))
                        {
                            changeState(STATE_READY0);
                        }
                    }
                    else
                    {
                        if (Global.mouseState.LeftButton == ButtonState.Pressed)
                        {
                            parent.velocity = Vector2.Zero;
                            changeState(STATE_AIM0);
                            break;
                        }

                        if (Global.keyboardState.IsKeyUp(Keys.E))
                        {
                            changeState(STATE_READY0);
                        }
                    }
                    switch (facingDirection)
                    {
                        case DIRECTION_N:
                            animation = Global.Avatar_Ready_N;
                            break;
                        case DIRECTION_E:
                            animation = Global.Avatar_Ready_E;
                            break;
                        case DIRECTION_S:
                            animation = Global.Avatar_Ready_S;
                            break;
                        case DIRECTION_W:
                            animation = Global.Avatar_Ready_W;
                            break;
                        case DIRECTION_NE:
                            animation = Global.Avatar_Ready_NE;
                            break;
                        case DIRECTION_SE:
                            animation = Global.Avatar_Ready_SE;
                            break;
                        case DIRECTION_NW:
                            animation = Global.Avatar_Ready_NW;
                            break;
                        case DIRECTION_SW:
                            animation = Global.Avatar_Ready_SW;
                            break;
                    }
                    #endregion
                    break;
                case STATE_AIM0:
                    #region AIM0
                    if (parent.hit)
                    {
                        parent.hit = false;
                        changeState(STATE_CANCEL);
                        break;
                    }
                    if (Global.mouseState.RightButton == ButtonState.Pressed)
                    {
                        changeState(STATE_CANCEL);
                        break;
                    }

                    //call parent.getBullet() to add to parent list and subsequently make it a collidable
                    curBullet = Global.currentGame.bulletPool.getBullet();
                    if (curBullet != null)
                    {
                        curBullet.color = Color.LightCyan;
                        curBullet.changeState(Bullet.STATE_CHARGING);
                        curBullet.parent = this.parent;
                        curBullet.location = new Vector2(50000, 50000); //so it doesnt run into the player
                    }
                    else
                    {
                        //out of ammo
                        changeState(STATE_CANCEL);
                        break;
                    }
                    AimWithMouse();
                    switch (facingDirection)
                    {
                        case DIRECTION_N:
                            animation = Global.Avatar_Aim_N;
                            break;
                        case DIRECTION_E:
                            animation = Global.Avatar_Aim_E;
                            break;
                        case DIRECTION_S:
                            animation = Global.Avatar_Aim_S;
                            break;
                        case DIRECTION_W:
                            animation = Global.Avatar_Aim_W;
                            break;
                        case DIRECTION_NE:
                            animation = Global.Avatar_Aim_NE;
                            break;
                        case DIRECTION_SE:
                            animation = Global.Avatar_Aim_SE;
                            break;
                        case DIRECTION_NW:
                            animation = Global.Avatar_Aim_NW;
                            break;
                        case DIRECTION_SW:
                            animation = Global.Avatar_Aim_SW;
                            break;
                    }
                    Global.currentGame.AimStick.state = ScreenObject.STATE_ACTIVE;
                    Global.currentGame.AimStick.location = parent.location;
                    animation.Stop();
                    animation.Play();
                    ChargeSound.Play();
                    changeState(STATE_AIM1);
                    #endregion
                    break;
                case STATE_AIM1:
                    #region AIM1
                    if (parent.hit)
                    {
                        parent.hit = false;
                        changeState(STATE_CANCEL);
                        break;
                    }
                    if (Global.mouseState.RightButton == ButtonState.Pressed || curBullet.state == ScreenObject.STATE_INACTIVE)
                    {
                        changeState(STATE_CANCEL);
                        break;
                    }

                    if (ChargeSound.State == SoundState.Stopped && ChargeLoopSound.State == SoundState.Stopped)
                    {
                        ChargeLoopSound.Play();
                    }

                    AimWithMouse();
                    if (animation.CurrentFrame >= animation.FrameCount - 1)
                    {
                        changeState(STATE_HOLD0);
                        break;
                    }
                    if (Global.mouseState.LeftButton == ButtonState.Released)
                    {
                        changeState(STATE_AIM2);
                        break;
                    }
                    #endregion
                    break;
                case STATE_AIM2:
                    #region AIM2
                    if (parent.hit)
                    {
                        parent.hit = false;
                        changeState(STATE_CANCEL);
                        break;
                    }
                    if (Global.mouseState.RightButton == ButtonState.Pressed || curBullet.state == ScreenObject.STATE_INACTIVE)
                    {
                        changeState(STATE_CANCEL);
                        break;
                    }

                    if (ChargeSound.State == SoundState.Stopped && ChargeLoopSound.State == SoundState.Stopped)
                    {
                        ChargeLoopSound.Play();
                    }

                    AimWithMouse();
                    if (animation.CurrentFrame >= animation.FrameCount - 1)
                    {
                        changeState(STATE_HOLD0);
                        break;
                    }

                    if (autoMode)
                    {
                        if (Global.probabilityPerSecond(.3))
                        {
                            changeState(STATE_FIRE0);
                            break;
                        }
                    }
                    else
                    {
                        if (Global.mouseState.LeftButton == ButtonState.Pressed)
                        {
                            changeState(STATE_FIRE0);
                            break;
                        }
                    }
                    #endregion
                    break;
                case STATE_HOLD0:
                    #region HOLD0
                    if (parent.hit)
                    {
                        parent.hit = false;
                        changeState(STATE_CANCEL);
                        break;
                    }
                    if (Global.mouseState.RightButton == ButtonState.Pressed || curBullet.state == ScreenObject.STATE_INACTIVE)
                    {
                        changeState(STATE_CANCEL);
                        break;
                    }

                    if (ChargeSound.State == SoundState.Stopped && ChargeLoopSound.State == SoundState.Stopped)
                    {
                        ChargeLoopSound.Play();
                    }

                    AimWithMouse();
                    switch (facingDirection)
                    {
                        case DIRECTION_N:
                            animation = Global.Avatar_Hold_N;
                            break;
                        case DIRECTION_E:
                            animation = Global.Avatar_Hold_E;
                            break;
                        case DIRECTION_S:
                            animation = Global.Avatar_Hold_S;
                            break;
                        case DIRECTION_W:
                            animation = Global.Avatar_Hold_W;
                            break;
                        case DIRECTION_NE:
                            animation = Global.Avatar_Hold_NE;
                            break;
                        case DIRECTION_SE:
                            animation = Global.Avatar_Hold_SE;
                            break;
                        case DIRECTION_NW:
                            animation = Global.Avatar_Hold_NW;
                            break;
                        case DIRECTION_SW:
                            animation = Global.Avatar_Hold_SW;
                            break;
                    }

                    if (Global.mouseState.LeftButton == ButtonState.Released)
                    {
                        changeState(STATE_HOLD1);
                        break;
                    }
                    #endregion
                    break;
                case STATE_HOLD1:
                    #region HOLD1
                    if (parent.hit)
                    {
                        parent.hit = false;
                        changeState(STATE_CANCEL);
                        break;
                    }
                    if (Global.mouseState.RightButton == ButtonState.Pressed || curBullet.state == ScreenObject.STATE_INACTIVE)
                    {
                        changeState(STATE_CANCEL);
                        break;
                    }

                    if (ChargeSound.State == SoundState.Stopped && ChargeLoopSound.State == SoundState.Stopped)
                    {
                        ChargeLoopSound.Play();
                    }

                    AimWithMouse();
                    switch (facingDirection)
                    {
                        case DIRECTION_N:
                            animation = Global.Avatar_Hold_N;
                            break;
                        case DIRECTION_E:
                            animation = Global.Avatar_Hold_E;
                            break;
                        case DIRECTION_S:
                            animation = Global.Avatar_Hold_S;
                            break;
                        case DIRECTION_W:
                            animation = Global.Avatar_Hold_W;
                            break;
                        case DIRECTION_NE:
                            animation = Global.Avatar_Hold_NE;
                            break;
                        case DIRECTION_SE:
                            animation = Global.Avatar_Hold_SE;
                            break;
                        case DIRECTION_NW:
                            animation = Global.Avatar_Hold_NW;
                            break;
                        case DIRECTION_SW:
                            animation = Global.Avatar_Hold_SW;
                            break;
                    }

                    if (autoMode)
                    {
                        if (Global.probabilityPerSecond(1))
                        {
                            changeState(STATE_FIRE0);
                            break;
                        }
                    }
                    else
                    {
                        if (Global.mouseState.LeftButton == ButtonState.Pressed)
                        {
                            changeState(STATE_FIRE0);
                            break;
                        }
                    }
                    #endregion
                    break;
                case STATE_FIRE0:
                    #region FIRE0
                    AimWithMouse();
                    Global.currentGame.AimStick.state = ScreenObject.STATE_INACTIVE;
                    ChargeLoopSound.Stop();
                    ChargeSound.Stop();
                    Global.FireGunSound.Play();
                    curBullet.state = Bullet.STATE_FIRING;

                    switch (facingDirection)
                    {
                        case DIRECTION_N:
                            animation = Global.Avatar_Fire_N;
                            break;
                        case DIRECTION_E:
                            animation = Global.Avatar_Fire_E;
                            break;
                        case DIRECTION_S:
                            animation = Global.Avatar_Fire_S;
                            break;
                        case DIRECTION_W:
                            animation = Global.Avatar_Fire_W;
                            break;
                        case DIRECTION_NE:
                            animation = Global.Avatar_Fire_NE;
                            break;
                        case DIRECTION_SE:
                            animation = Global.Avatar_Fire_SE;
                            break;
                        case DIRECTION_NW:
                            animation = Global.Avatar_Fire_NW;
                            break;
                        case DIRECTION_SW:
                            animation = Global.Avatar_Fire_SW;
                            break;
                    }

                    animation.Stop();
                    animation.Play();
                    changeState(STATE_FIRE1);
                    #endregion
                    break;
                case STATE_FIRE1:
                    #region FIRE1
                    if (animation.CurrentFrame >= animation.FrameCount - 1)
                    {
                        changeState(STATE_READY0);
                    }
                    #endregion
                    break;
                case STATE_CANCEL:
                    #region CANCEL
                    Global.currentGame.AimStick.state = ScreenObject.STATE_INACTIVE;
                    ChargeSound.Stop();
                    ChargeLoopSound.Stop();
                    if (curBullet != null)
                    {
                        curBullet.state = ScreenObject.STATE_KILL;
                    }
                    if (Global.mouseState.RightButton == ButtonState.Released)
                    {
                        changeState(STATE_READY0);
                        break;
                    }
                    break;
                    #endregion
            }
        }
        private void AimWithMouse()
        {
            int sem = 0;

            #region auto
            if (autoMode)
            {
                if (Global.probabilityPerSecond(1))
                {
                    autoMouse = !autoMouse;
                }
                if (Global.probabilityPerSecond(1))
                {
                    autoMouse2 = !autoMouse2;
                }
                if (autoMouse)
                {
                    autoAngleInc = Global.rand.NextDouble() * .5 - .25;
                }
                if (autoMouse2)
                {
                    autoAngle += autoAngleInc;
                }
                facingAngle = autoAngle;
            }
            #endregion
            else
            {
                facingAngle = Math.Atan2((parent.absoluteLocation.Y - Global.currentPlayer.Crosshair.absoluteLocation.Y), 
                                         (parent.absoluteLocation.X - Global.currentPlayer.Crosshair.absoluteLocation.X));
            }

            if (facingAngle < 0)
            {
                facingAngle = MathHelper.TwoPi + facingAngle;
            }
            curBullet.facingAngle = facingAngle + Math.PI;

            Global.currentGame.AimStick.rotation = (float)(facingAngle + Math.PI);

            if (state != STATE_AIM0 && state != STATE_AIM1 && state != STATE_AIM2)
            {
                sem = (int)(Math.Floor((facingAngle + Math.PI / 8) / MathHelper.PiOver4)); //get octant
                switch (sem)
                {
                    case 0:
                        facingDirection = DIRECTION_E;
                        rotation = (float)(facingAngle - Math.PI);
                        break;
                    case 1:
                        facingDirection = DIRECTION_NE;
                        rotation = (float)(facingAngle - 3 * MathHelper.PiOver4);
                        break;
                    case 2:
                        facingDirection = DIRECTION_N;
                        rotation = (float)(facingAngle - MathHelper.PiOver2);
                        break;
                    case 3:
                        facingDirection = DIRECTION_NW;
                        rotation = (float)(facingAngle - MathHelper.PiOver4);
                        break;
                    case 4:
                        facingDirection = DIRECTION_W;
                        rotation = (float)(facingAngle);
                        break;
                    case 5:
                        facingDirection = DIRECTION_SW;
                        rotation = (float)(facingAngle - 7 * MathHelper.PiOver4);
                        break;
                    case 6:
                        facingDirection = DIRECTION_S;
                        rotation = (float)(facingAngle - 3 * MathHelper.PiOver2);
                        break;
                    case 7:
                        facingDirection = DIRECTION_SE;
                        rotation = (float)(facingAngle - 5 * MathHelper.PiOver4);
                        break;
                    case 8:
                        facingDirection = DIRECTION_E;
                        rotation = (float)(facingAngle - Math.PI);
                        break;
                }
            }
            else
            {
                switch (facingDirection)
                {
                    case DIRECTION_E:
                        rotation = (float)(facingAngle - Math.PI);
                        break;
                    case DIRECTION_NE:
                        rotation = (float)(facingAngle - 3 * MathHelper.PiOver4);
                        break;
                    case DIRECTION_N:
                        rotation = (float)(facingAngle - MathHelper.PiOver2);
                        break;
                    case DIRECTION_NW:
                        rotation = (float)(facingAngle - MathHelper.PiOver4);
                        break;
                    case DIRECTION_W:
                        rotation = (float)(facingAngle);
                        break;
                    case DIRECTION_SW:
                        rotation = (float)(facingAngle - 7 * MathHelper.PiOver4);
                        break;
                    case DIRECTION_S:
                        rotation = (float)(facingAngle - 3 * MathHelper.PiOver2);
                        break;
                    case DIRECTION_SE:
                        rotation = (float)(facingAngle - 5 * MathHelper.PiOver4);
                        break;
                }
            }
            
        }
        private void GuideWithKeyboard()
        {
            int sem = 0;
            parent.velocity = Vector2.Zero;

            if (autoMode)
            {
                if (Global.probabilityPerSecond(1))
                {
                    autoKeyW = !autoKeyW;
                }
                if (autoKeyW)
                {
                    sem += 8;
                    parent.velocity.Y -= speedControl;
                }
                if (Global.probabilityPerSecond(1))
                {
                    autoKeyD = !autoKeyD;
                }
                if (autoKeyD)
                {
                    sem += 4;
                    parent.velocity.X += speedControl;
                }
                if (Global.probabilityPerSecond(1))
                {
                    autoKeyS = !autoKeyS;
                }
                if (autoKeyS)
                {
                    sem += 2;
                    parent.velocity.Y += speedControl;
                }
                if (Global.probabilityPerSecond(1))
                {
                    autoKeyA = !autoKeyA;
                }
                if (autoKeyA)
                {
                    sem += 1;
                    parent.velocity.X -= speedControl;
                }
            }
            else
            {
                if (Global.keyboardState.IsKeyDown(Keys.LeftShift))
                {
                    speedControl = 8;
                }
                else
                {
                    speedControl = 4;
                }
                if (Global.keyboardState.IsKeyDown(Keys.W))
                {
                    sem += 8;
                    parent.velocity.Y -= speedControl;
                }
                if (Global.keyboardState.IsKeyDown(Keys.D))
                {
                    sem += 4;
                    parent.velocity.X += speedControl;
                }
                if (Global.keyboardState.IsKeyDown(Keys.S))
                {
                    sem += 2;
                    parent.velocity.Y += speedControl;
                }
                if (Global.keyboardState.IsKeyDown(Keys.A))
                {
                    sem += 1;
                    parent.velocity.X -= speedControl;
                }
            }

            if (parent.velocity.Length() > 0)
            {
                rotation = 0;
                changeState(STATE_RUN);
                if (Math.Abs(parent.velocity.Y) > 0 && Math.Abs(parent.velocity.X) > 0)
                {
                    parent.velocity *= .707f;
                }
                switch (sem)
                {
                    case 0:
                        break;
                    case 1:
                        facingDirection = DIRECTION_W;
                        break;
                    case 2:
                        facingDirection = DIRECTION_S;
                        break;
                    case 3:
                        facingDirection = DIRECTION_SW;
                        break;
                    case 4:
                        facingDirection = DIRECTION_E;
                        break;
                    case 5:
                        break;
                    case 6:
                        facingDirection = DIRECTION_SE;
                        break;
                    case 7:
                        facingDirection = DIRECTION_S;
                        break;
                    case 8:
                        facingDirection = DIRECTION_N;
                        break;
                    case 9:
                        facingDirection = DIRECTION_NW;
                        break;
                    case 10:
                        break;
                    case 11:
                        facingDirection = DIRECTION_W;
                        break;
                    case 12:
                        facingDirection = DIRECTION_NE;
                        break;
                    case 13:
                        facingDirection = DIRECTION_N;
                        break;
                    case 14:
                        facingDirection = DIRECTION_E;
                        break;
                    case 15:
                        break;
                }
            }
            else
            {
                if (Global.keyboardState.IsKeyDown(Keys.E))
                {
                    changeState(STATE_READY1);
                }
                else
                {
                    changeState(STATE_READY0);
                }
            }
        }
        public override void reset()
        {
            location = Global.randomTableSpot();
            velocity.X = (float)Global.rand.NextDouble() * .01f;
            velocity.Y = (float)Global.rand.NextDouble() * .01f;
            BounceOnWalls();
            changeState(STATE_ACTIVE);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}