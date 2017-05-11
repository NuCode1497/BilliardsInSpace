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
    public class BilliardBall : SpriteObject
    {
        public const int STATE_WELCOME = 3;
        public const int STATE_POCKETED = 4;
        public const int STATE_TEST_1 = 5;
        public const int STATE_TEST_2 = 6;
        public const int STATE_TEST_3 = 7;
        public const int STATE_TEST_4 = 8;
        public const int STATE_TEST_4_2 = 9;

        public const int TYPE_8 = 0;
        public const int TYPE_CUE = 1;
        public const int TYPE_SOLID = 2;
        public const int TYPE_STRIPE = 3;
        public const int TYPE_TEST_5 = 4;
        public int type;

        public Particle TEST_P3 = new Particle();
        public Particle TEST_P4 = new Particle();
        public Particle TEST_P5 = new Particle();
        public Particle TEST_P6 = new Particle();

        int test4 = 0;

        float newVol;
        public bool first = true;

        public override void Setup()
        {
            collideType = COLLIDE_TYPE_BALL;
            type = TYPE_SOLID;
            image = Global.CueBallImage;
            location = Vector2.Zero;
            velocity.X = (float)Global.rand.NextDouble() * .01f;
            velocity.Y = (float)Global.rand.NextDouble() * .01f;
            mass = 1f * (float)(Math.Pow(10, 3));
            scale = (float)(30.75 / 137);
            sRadius = 30.75f; //this value should remain constant, it is the regulation ball size relative to table
            layer = 0.4f;
            friction = .997f;
            fCritVel = .2f;
            changeState(STATE_ACTIVE);
            base.Setup();
        }
        public override void Update(GameTime gameTime)
        {
            hittingCorner = false;

            switch (state)
            {
                #region Tests
                case STATE_TEST_3:
                    base.Update(gameTime);
                    BounceOnScreenWalls();
                    BounceOnWallObjects();
                    if (hit)
                    {
                        SoundEffectInstance clack = Global.ClackSound.CreateInstance();
                        clack.Volume = 0.5f;
                        newVol = clack.Volume * velocity.Length() / 4;
                        if (newVol > 1) newVol = 1;
                        clack.Volume = newVol;
                        clack.Play();
                        hit = false;
                    }
                    if (hitWall)
                    {
                        SoundEffectInstance thump = Global.BumperSound.CreateInstance();
                        thump.Volume = 0.5f;
                        newVol = thump.Volume * velocity.Length();
                        if (newVol > 1) newVol = 1;
                        thump.Volume = newVol;
                        thump.Play();
                        hitWall = false;

                    }
                    p = Global.currentGame.particlePool.getParticle();
                    if (p != null)
                    {
                        p.location.X = location.X;
                        p.location.Y = location.Y;
                        p.velocity.X = 0;
                        p.velocity.Y = 0;
                        p.scale = 1;
                        p.color.A = 200;
                        p.color.B = (byte)(50 + 200 - 40 * Math.Abs(velocity.Length()));
                        p.color.R = (byte)(50 + 40 * Math.Abs(velocity.Length()));
                        p.color.G = 0;
                        p.layer = .9f + Math.Abs(velocity.Length()) / 1000;
                        p.changeState(Particle.STATE_TEST_1);
                    }
                    break;
                case STATE_TEST_4:
                    base.Update(gameTime);
                    BounceOnScreenWalls();
                    BounceOnWallObjects();
                    if (hit)
                    {
                        SoundEffectInstance clack = Global.ClackSound.CreateInstance();
                        clack.Volume = 0.5f;
                        newVol = clack.Volume * velocity.Length() / 4;
                        if (newVol > 1) newVol = 1;
                        clack.Volume = newVol;
                        clack.Play();
                        hit = false;
                    }
                    if (hitWall)
                    {
                        test4++;
                        if (test4 == 2)
                        {
                            test4 = 0;
                            Global.currentGame.TEST_actives--;
                            changeState(STATE_KILL);
                        }
                        SoundEffectInstance thump = Global.BumperSound.CreateInstance();
                        thump.Volume = 0.5f;
                        newVol = thump.Volume * velocity.Length();
                        if (newVol > 1) newVol = 1;
                        thump.Volume = newVol;
                        thump.Play();
                        hitWall = false;

                    }
                    p = Global.currentGame.particlePool.getParticle();
                    if (p != null)
                    {
                        p.location.X = location.X;
                        p.location.Y = location.Y;
                        p.velocity.X = 0;
                        p.velocity.Y = 0;
                        p.scale = 1;
                        p.color.A = 200;
                        p.color.B = (byte)(50 + 200 - 2 * Math.Abs(velocity.Length()));
                        p.color.R = (byte)(50 + 2 * Math.Abs(velocity.Length()));
                        p.color.G = 0;
                        p.layer = .9f + Math.Abs(velocity.Length()) / 1000;
                        p.changeState(Particle.STATE_TEST_1);
                    }
                    break;
                case STATE_TEST_4_2:
                    base.Update(gameTime);
                    BounceOnScreenWalls();
                    BounceOnWallObjects();
                    if (hit)
                    {
                        SoundEffectInstance clack = Global.ClackSound.CreateInstance();
                        clack.Volume = 0.5f;
                        newVol = clack.Volume * velocity.Length() / 4;
                        if (newVol > 1) newVol = 1;
                        clack.Volume = newVol;
                        clack.Play();
                        hit = false;
                    }
                    if (hitWall)
                    {
                        test4++;
                        if (test4 == 4)
                        {
                            test4 = 0;
                            Global.currentGame.TEST_actives--;
                            changeState(STATE_KILL);
                        }
                        SoundEffectInstance thump = Global.BumperSound.CreateInstance();
                        thump.Volume = 0.5f;
                        newVol = thump.Volume * velocity.Length();
                        if (newVol > 1) newVol = 1;
                        thump.Volume = newVol;
                        thump.Play();
                        hitWall = false;

                    }
                    p = Global.currentGame.particlePool.getParticle();
                    if (p != null)
                    {
                        p.location.X = location.X;
                        p.location.Y = location.Y;
                        p.velocity.X = 0;
                        p.velocity.Y = 0;
                        p.scale = 1;
                        p.color.A = 200;
                        p.color.B = (byte)(50 + 200 - 2 * Math.Abs(velocity.Length()));
                        p.color.R = (byte)(50 + 2 * Math.Abs(velocity.Length()));
                        p.color.G = 0;
                        p.layer = .9f + Math.Abs(velocity.Length()) / 1000;
                        p.changeState(Particle.STATE_TEST_1);
                    }
                    break;
                case STATE_TEST_2:
                case STATE_TEST_1:
                    base.Update(gameTime);
                    BounceOnScreenWalls();
                    if (hit)
                    {
                        SoundEffectInstance clack = Global.ClackSound.CreateInstance();
                        clack.Volume = 0.5f;
                        newVol = clack.Volume * velocity.Length() / 4;
                        if (newVol > 1) newVol = 1;
                        clack.Volume = newVol;
                        clack.Play();
                        hit = false;
                    }
                    if (hitWall)
                    {
                        Global.currentGame.TEST_actives--;
                        changeState(STATE_KILL);
                        SoundEffectInstance thump = Global.BumperSound.CreateInstance();
                        thump.Volume = 0.5f;
                        newVol = thump.Volume * velocity.Length();
                        if (newVol > 1) newVol = 1;
                        thump.Volume = newVol;
                        thump.Play();
                        hitWall = false;

                    }
                    p = Global.currentGame.particlePool.getParticle();
                    if (p != null)
                    {
                        p.location.X = location.X;
                        p.location.Y = location.Y;
                        p.velocity.X = 0;
                        p.velocity.Y = 0;
                        p.scale = 1;
                        p.color.A = 200;
                        p.color.B = (byte)(50 + 200 - 2 * Math.Abs(velocity.Length()));
                        p.color.R = (byte)(50 + 2 * Math.Abs(velocity.Length()));
                        p.color.G = 0;
                        p.layer = .9f + Math.Abs(velocity.Length()) / 1000;
                        p.changeState(Particle.STATE_TEST_1);
                    }
                    break;
                #endregion
                #region inactive
                case STATE_KILL:
                    collideType = COLLIDE_TYPE_BALL;
                    type = TYPE_SOLID;
                    image = Global.CueBallImage;
                    location = Vector2.Zero;
                    absoluteLocation = Vector2.Zero;
                    velocity.X = (float)Global.rand.NextDouble() * .01f;
                    velocity.Y = (float)Global.rand.NextDouble() * .01f;
                    mass = 1f * (float)(Math.Pow(10, 3));
                    scale = (float)(30.75 / 137);
                    sRadius = 30.75f; //this value should remain constant, it is the regulation ball size relative to table
                    layer = 0.4f;
                    friction = .998f;
                    fCritVel = .2f;
                    color.A = 255;
                    changeState(STATE_INACTIVE);
                    TEST_P3.changeState(Particle.STATE_KILL);
                    TEST_P4.changeState(Particle.STATE_KILL);
                    TEST_P5.changeState(Particle.STATE_KILL);
                    TEST_P6.changeState(Particle.STATE_KILL);
                    break;
                case STATE_INACTIVE:
                    break;
                #endregion
                #region active
                case STATE_ACTIVE:
                    scale = (float)(30.75 / 137) * Global.currentGame.Table.scale; //move to Game1 when mouse wheel
                    base.Update(gameTime);
                    BounceOnTableWalls();
                    BounceOnTableWallObjects();
                    BounceInsidePockets();
                    DieInPockets();
                    if (hit)
                    {
                        SoundEffectInstance clack = Global.ClackSound.CreateInstance();
                        clack.Volume = 0.5f;
                        newVol = clack.Volume * velocity.Length() / 4;
                        if (newVol > 1) newVol = 1;
                        clack.Volume = newVol;
                        clack.Play();
                        hit = false;
                    }
                    if (hitWall)
                    {
                        SoundEffectInstance thump = Global.BumperSound.CreateInstance();
                        thump.Volume = 0.5f;
                        newVol = thump.Volume * velocity.Length();
                        if (newVol > 1) newVol = 1;
                        thump.Volume = newVol;
                        thump.Play();
                        hitWall = false;
                    }
                    break;
                case STATE_WELCOME:
                    base.Update(gameTime);
                    DieOnDistantWalls();
                    if (hit)
                    {
                        hit = false;
                    }
                    if (hitWall)
                    {
                        changeState(STATE_INACTIVE);
                    }
                    break;
                #endregion
                #region pocketed
                case STATE_DEATH:
                    base.Update(gameTime);
                    //BounceInsidePocket();
                    if (color.A < 10)
                    {
                        changeState(STATE_KILL);
                    }
                    velocity *= .9f;
                    color.A = (byte)(color.A - 10);
                    break;
                case STATE_POCKETED:
                    base.Update(gameTime);
                    switch (type)
                    {
                        case TYPE_TEST_5:
                            reset();
                            break;
                        case TYPE_CUE:
                            reset();
                            Global.currentPlayer.changeState(Player.STATE_NEXTPLAYER);
                            break;
                        case TYPE_SOLID:
                            Global.pocketedSolids++;
                            switch (Global.currentPlayer.paradigm)
                            {
                                case TYPE_8:
                                    Global.currentPlayer.changeState(Player.STATE_LOSE);
                                    break;
                                case TYPE_STRIPE:
                                    break;
                                case TYPE_SOLID:
                                    break;
                                case TYPE_CUE:
                                    Global.currentPlayer.paradigm = TYPE_SOLID;
                                    break;
                            }
                            changeState(STATE_DEATH);
                            break;
                        case TYPE_STRIPE:
                            Global.pocketedStripes++;
                            switch (Global.currentPlayer.paradigm)
                            {
                                case TYPE_8:
                                    Global.currentPlayer.changeState(Player.STATE_LOSE);
                                    break;
                                case TYPE_STRIPE:
                                    break;
                                case TYPE_SOLID:
                                    break;
                                case TYPE_CUE:
                                    Global.currentPlayer.paradigm = TYPE_STRIPE;
                                    break;
                            }
                            changeState(STATE_DEATH);
                            break;
                        case TYPE_8:
                            switch (Global.currentPlayer.paradigm)
                            {
                                case TYPE_8:
                                    Global.currentPlayer.changeState(Player.STATE_WIN);
                                    break;
                                case TYPE_STRIPE:
                                    Global.currentPlayer.changeState(Player.STATE_LOSE);
                                    break;
                                case TYPE_SOLID:
                                    Global.currentPlayer.changeState(Player.STATE_LOSE);
                                    break;
                                case TYPE_CUE:
                                    Global.currentPlayer.changeState(Player.STATE_LOSE);
                                    break;
                            }
                            changeState(STATE_DEATH);
                            break;
                    }

                    SoundEffectInstance clunk = Global.PocketSound.CreateInstance();
                    clunk.Volume = 0.5f;
                    newVol = clunk.Volume + clunk.Volume * velocity.Length();
                    if (newVol > 1) newVol = 1;
                    clunk.Volume = newVol;
                    clunk.Play();
                    break;
                #endregion
            }
        }
        public void DieInPockets()
        {
            //if it hasnt hit a wall, check for hitting outside wall to register a pocket
            if (location.X + sRadius > Global.currentGame.Table.location.X + (1359))
            {
                location.X = Global.currentGame.Table.location.X + (1359) - sRadius;
                velocity.X = Math.Abs(velocity.X) * -1;
                changeState(STATE_POCKETED);
            }
            if (location.X - sRadius < Global.currentGame.Table.location.X + (-1359))
            {
                location.X = Global.currentGame.Table.location.X + (-1359) + sRadius;
                velocity.X = Math.Abs(velocity.X);
                changeState(STATE_POCKETED);
            }
            if (location.Y + sRadius > Global.currentGame.Table.location.Y + (649))
            {
                location.Y = Global.currentGame.Table.location.Y + (649) - sRadius;
                velocity.Y = Math.Abs(velocity.Y) * -1;
                changeState(STATE_POCKETED);
            }
            if (location.Y - sRadius < Global.currentGame.Table.location.Y + (-649))
            {
                location.Y = Global.currentGame.Table.location.Y + (-649) + sRadius;
                velocity.Y = Math.Abs(velocity.Y);
                changeState(STATE_POCKETED);
            }
        }
        public void BounceInsidePockets()
        {
            bool foundPocket = false;
            foreach (Vector2 pocket in Global.currentGame.pockets)
            {
                float d = Vector2.Distance(pocket, location);
                if (d < 36)
                {
                    foundPocket = true;
                    if (d < 5 && state != STATE_POCKETED) changeState(STATE_POCKETED);

                    #region collision
                    #region vars

                    float accuracy = Global.currentGame.TEST_accuracy;
                    float tpivot1;
                    double difference;
                    double phi; //angle of collision
                    Vector2 Fg12 = Vector2.Zero; //gravitational force by particle 1 on particle 2
                    double dmin; //r1 + r2
                    float axis; //flag telling collision axis points NESW or NWSE 
                    //ball 1
                    Vector2 displacement1 = Vector2.Zero;
                    Vector2 v12; //velocity on collision axis
                    Vector2 a12; //acceleration on collision axis
                    double v12mag;
                    Vector2 v11; //velocity not on collision axis
                    double v12fmag; //result of 1d collision along axis of collision
                    Vector2 v12f;
                    Vector2 v1f;
                    double vAlignment1; //alignment of collision to velocity
                    double cvtheta1; //angle between collision and velocity
                    double vtheta1; //angle of velocity
                    double atheta1;
                    double catheta1;
                    double aAlignment1;
                    double a12mag;

                    #endregion
                    d = Vector2.Distance(pocket, location);
                    dmin = sRadius + 36;
                    difference = dmin - d;

                    if (difference < accuracy) //bounce inside pocket circle
                    {
                        #region displacement correction
                        tpivot1 = t;
                        while (!(difference > -accuracy && difference < accuracy) && tpivot1 > accuracy)
                        {   //quicksort to t when objs first contact
                            tpivot1 /= 2;
                            if (difference > accuracy)
                            {
                                t -= tpivot1;
                            }
                            else
                            {
                                t += tpivot1;
                            }

                            ModifiedUpdate();

                            d = Vector2.Distance(pocket, location);

                            difference = dmin - d;
                        }

                        phi = Math.Atan2((pocket.Y - location.Y), (pocket.X - location.X));

                        #endregion

                        if (Math.Abs(difference) < accuracy)
                        {
                            #region circular collision detection

                            axis = (float)Math.Tan(phi);
                            axis /= Math.Abs(axis);
                            if (float.IsNaN(axis)) axis = -1;

                            //ball 1
                            vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                            cvtheta1 = phi - vtheta1;
                            vAlignment1 = Math.Cos(cvtheta1);
                            v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi)));
                            v11 = velocity - v12; //split velocity into 2 vectors
                            if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                            else v12mag = v12.Length();
                            v12fmag = -v12mag;
                            v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi)))); //unflatten
                            v1f = v12f + v11;
                            velocity = v1f;

                            #endregion
                        }
                        else
                        {
                            #region old displacement correction
                            difference = dmin;
                            difference -= d;
                            Vector2 dx = new Vector2((float)(difference * Math.Cos(phi)), (float)(difference * Math.Sin(phi)));
                            location -= dx;

                            d = Vector2.Distance(pocket, location);
                            #endregion

                            #region old circular collision detection

                            axis = (float)Math.Tan(phi);
                            axis /= Math.Abs(axis);
                            if (float.IsNaN(axis)) axis = -1;

                            //ball 1
                            vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                            cvtheta1 = phi - vtheta1;
                            vAlignment1 = Math.Cos(cvtheta1);
                            v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi)));
                            v11 = velocity - v12; //split velocity into 2 vectors
                            if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                            else v12mag = v12.Length();
                            //velocity correction
                            if (oldAcceleration.Length() > 0)
                            {
                                atheta1 = Math.Atan2(oldAcceleration.Y, oldAcceleration.X);
                                catheta1 = phi - atheta1;
                                aAlignment1 = Math.Cos(catheta1);
                                a12 = new Vector2((float)((oldAcceleration.Length() * aAlignment1) * Math.Cos(phi)), (float)((oldAcceleration.Length() * aAlignment1) * Math.Sin(phi)));
                                if (a12.X < 0) a12mag = a12.Length() * -1;
                                else a12mag = a12.Length();
                                if (v12mag < 0)
                                {
                                    if (displacement1.X < 0) v12mag = -(Math.Sqrt((v12mag * v12mag) - 2 * a12mag * displacement1.Length())); //Vxf^2 = Vxi^2 + 2 * a * (xf - xi)
                                    else v12mag = -(Math.Sqrt((v12mag * v12mag) + 2 * a12mag * displacement1.Length()));
                                }
                                else
                                {
                                    if (displacement1.X < 0) v12mag = Math.Sqrt((v12mag * v12mag) - 2 * a12mag * displacement1.Length()); //Vxf^2 = Vxi^2 + 2 * a * (xf - xi)
                                    else v12mag = Math.Sqrt((v12mag * v12mag) + 2 * a12mag * displacement1.Length());
                                }
                                if (double.IsNaN(v12mag)) v12mag = 0;
                            }
                            v12fmag = -v12mag;
                            v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi)))); //unflatten
                            v1f = v12f + v11;
                            velocity = v1f;
                            #endregion
                        }

                        oldLocation = location;
                        oldVelocity = velocity;
                    }
                    else
                    {
                        phi = Math.Atan2((pocket.Y - location.Y), (pocket.X - location.X));
                    }
                    #endregion
                    #region gravity
                    if (d != 0)
                    {
                        float Fg12mag;
                        Fg12mag = (10000000 / (d * d));
                        Fg12.X = (float)(Fg12mag * Math.Cos(phi));
                        Fg12.Y = (float)(Fg12mag * Math.Sin(phi));
                        if (Fg12.Length() != 0) acceleration += -Fg12 / mass;
                    }
                    oldAcceleration = acceleration;
                    #endregion
                }
                if (foundPocket) break;
            }
        }
        public override void reset()
        {
            changeState(STATE_POCKETED);
            while (state == STATE_POCKETED)
            {
                location = Global.randomTableSpot();
                velocity.X = (float)Global.rand.NextDouble() * .01f;
                velocity.Y = (float)Global.rand.NextDouble() * .01f;
                BounceOnTableWalls();
                changeState(BilliardBall.STATE_ACTIVE);
            }
            oldLocation = location;
            oldVelocity = velocity;

            if (!first)
            {
                SoundEffectInstance plop = Global.PlopSound.CreateInstance();
                plop.Volume = 0.1f;
                float newVol = plop.Volume;
                if (newVol > 1) newVol = 1;
                plop.Volume = newVol;
                plop.Play();
            }
            first = false;
            switch (type)
            {
                case TYPE_TEST_5:
                case TYPE_CUE:
                    break;
                case TYPE_SOLID:
                    Global.pocketedSolids--;
                    break;
                case TYPE_STRIPE:
                    Global.pocketedStripes--;
                    break;
                case TYPE_8:
                    break;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (state)
            {
                case STATE_INACTIVE:
                case STATE_KILL:
                case STATE_POCKETED:
                    break;
                case STATE_TEST_1:
                case STATE_TEST_2:
                case STATE_TEST_3:
                case STATE_TEST_4:
                case STATE_TEST_4_2:
                    TEST_P3.Draw(spriteBatch);
                    TEST_P4.Draw(spriteBatch);
                    TEST_P5.Draw(spriteBatch);
                    TEST_P6.Draw(spriteBatch);
                    spriteBatch.Draw(image, absoluteLocation, null, color, rotation, origin, scale, spriteEffects, layer);
                    break;
                case STATE_DEATH:
                case STATE_WELCOME:
                case STATE_ACTIVE:
                    spriteBatch.Draw(image, absoluteLocation, null, color, rotation, origin, scale, spriteEffects, layer);
                    break;
            }
        }
    }
}