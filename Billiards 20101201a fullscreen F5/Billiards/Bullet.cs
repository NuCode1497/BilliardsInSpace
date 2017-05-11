// (c) Copyright 2010 Cody Neuburger
//        All rights reserved.
//Avatar models were created from World of Warcraft by Blizzard Entertainment
//using WoW Model Viewer
//I am in no way affiliated with Blizzard
//Some sounds are from http://www.freesound.org

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
    public class Bullet : SpriteObject
    {
        public const int STATE_FIRED = 5;
        public const int STATE_CHARGING = 3;
        public const int STATE_FIRING = 4;
        float bulletSpeed;
        public double facingAngle;
        float massScale;
        float speedScale;
        float charger = .001f;

        SoundEffectInstance hitSound;
        public override void Setup()
        {
            collideType = COLLIDE_TYPE_BALL;
            facingAngle = 0;
            hit = false;
            hitWall = false;
            hitSound = Global.PlopSound.CreateInstance();
            image = Global.NoiseBallImage;
            angularVelocity = 0f;
            angularAcceleration = 0f;
            velocity = Vector2.Zero;
            location = Vector2.Zero;
            absoluteLocation = Vector2.Zero;
            friction = 1f;
            radius = 30.75f;
            sRadius = 30.75f;
            layer = 0.6f;
            scale = 0.005f;
            massScale = scale;
            speedScale = 2;
            base.Setup();
            changeState(STATE_INACTIVE);
        }
        public override void Update(GameTime gameTime)
        {
            switch (state)
            {
                case STATE_INACTIVE:
                    break;
                case STATE_ACTIVE:
                    if (hit)
                    {
                        changeState(STATE_KILL);
                    }
                    base.Update(gameTime);
                    break;
                case STATE_CHARGING:
                    if (parent == null) break;
                    base.Update(gameTime);
                    collideType = COLLIDE_TYPE_NONE;
                    rotation = Global.randFloat((float)MathHelper.PiOver4);
                    if (charger < 0.05f) charger *= 1.05f;
                    else if (charger < 0.1f) charger += .01f;
                    else if (charger < 1.5f) charger += .005f;
                    scale = (float)(30.75 / 137) * Global.currentGame.Table.scale;
                    scale *= charger;
                    sRadius = radius * charger;

                    //play with these numbers to adjust striker
                    massScale = scale * 3;
                    mass = 2000 * (massScale);
                    speedScale += .1f;
                    bulletSpeed = speedScale;

                    float d = 137 + parent.radius;
                    location.X = (float)(d * Math.Cos(facingAngle));
                    location.Y = (float)(d * Math.Sin(facingAngle));
                    oldLocation = location;
                    break;
                case STATE_FIRING:
                    charger = 0.0001f;
                    if (parent == null) break;
                    collideType = COLLIDE_TYPE_BALL;
                    int x = this.state;//debug
                    base.Update(gameTime);
                    rotation = 0;
                    hit = false;
                    hitWall = false;
                    velocity.X = bulletSpeed * (float)Math.Cos(facingAngle);
                    velocity.Y = bulletSpeed * (float)Math.Sin(facingAngle);
                    oldVelocity = velocity;
                    changeParent(parent.parent);
                    changeState(STATE_FIRED);
                    break;
                case STATE_FIRED:
                    if (parent == null) break;
                    base.Update(gameTime);
                    BounceOnTableWalls();
                    BounceOnTableWallObjects();
                    DieInPockets();
                    if (hitWall)
                    {
                        hitSound.Play();
                        changeState(STATE_KILL);
                    }
                    else if (hit)
                    {
                        changeState(STATE_DEATH);
                    }
                    break;
                case STATE_DEATH:
                    base.Update(gameTime);
                    if (color.A < 10)
                    {
                        changeState(STATE_KILL);
                    }
                    else
                    {
                        color.A = (byte)(color.A - (byte)(Global.rand.Next(5, 10)));
                    }
                    break;
                case STATE_KILL:
                    //return to pool
                    parent = null;
                    velocity = Vector2.Zero;
                    location = Vector2.Zero;
                    absoluteLocation = Vector2.Zero;
                    acceleration = Vector2.Zero;
                    scale = 0.005f;
                    massScale = scale;
                    speedScale = 2;
                    hit = false;
                    hitWall = false;
                    color = Color.White;
                    changeState(STATE_INACTIVE);
                    break;
            }
        }
        public void DieInPockets()
        {
            //if it hasnt hit a wall, check for hitting outside wall to register a pocket
            if (location.X + sRadius > Global.currentGame.Table.location.X + (1359))
            {
                location.X = Global.currentGame.Table.location.X + (1359) - sRadius;
                velocity.X = Math.Abs(velocity.X) * -1;
                hitWall = true;
            }
            if (location.X - sRadius < Global.currentGame.Table.location.X + (-1359))
            {
                location.X = Global.currentGame.Table.location.X + (-1359) + sRadius;
                velocity.X = Math.Abs(velocity.X);
                hitWall = true;
            }
            if (location.Y + sRadius > Global.currentGame.Table.location.Y + (649))
            {
                location.Y = Global.currentGame.Table.location.Y + (649) - sRadius;
                velocity.Y = Math.Abs(velocity.Y) * -1;
                hitWall = true;
            }
            if (location.Y - sRadius < Global.currentGame.Table.location.Y + (-649))
            {
                location.Y = Global.currentGame.Table.location.Y + (-649) + sRadius;
                velocity.Y = Math.Abs(velocity.Y);
                hitWall = true;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

    }
}
