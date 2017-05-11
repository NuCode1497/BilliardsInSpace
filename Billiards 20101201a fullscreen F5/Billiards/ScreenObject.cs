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
    public abstract class ScreenObject : GameObject
    {
        public static Vector2 P3 = new Vector2();
        public static Vector2 P4 = new Vector2();
        public static Vector2 P5 = new Vector2();
        public static Vector2 P6 = new Vector2();
        public static double dx;

        Vector2 Fg12 = new Vector2();
        public const int STATE_KILL = -2;
        public const int STATE_DEATH = 2;
        public const int STATE_INACTIVE = 0;
        public const int STATE_ACTIVE = 1;
        private int _state;
        public int state
        {
            get { return _state; }
            set { changeState(value); }
        }

        public const int COLLIDE_TYPE_NONE = 0;
        public const int COLLIDE_TYPE_BALL = 1;
        public const int COLLIDE_TYPE_MAN = 2;
        public const int COLLIDE_TYPE_WALL = 3;
        public int collideType = 0;

        protected bool hittingCorner = false;
        public float t; //frame time 0 <= t <= 1
        public static Particle p;
        public int stateFrames = 0;
        public ScreenObject parent = null;
        public Vector2 origin = new Vector2();
        public float angularAcceleration = 0.0f;
        public float angularVelocity = 0.0f;
        public float rotation = 0.0f;
        public Vector2 absoluteLocation = new Vector2();
        public Vector2 location = new Vector2(0, 0);
        public Vector2 oldLocation = new Vector2(0, 0);
        public Vector2 velocity = new Vector2(0f, 0f);
        public Vector2 oldVelocity = new Vector2(0f, 0f);
        public Vector2 acceleration = new Vector2(0f, 0f);
        public Vector2 oldAcceleration = new Vector2(0f, 0f);
        public float mass;
        public float radius = 137;
        public float sRadius;
        public float scale = 1.0f;
        public float layer = 0.5f;
        public Color color = Color.White;
        public float friction = 1f;
        public float fCritVel = 0f;
        public bool hit = false;
        public bool hitWall = false;
        public SpriteEffects spriteEffects = SpriteEffects.None;

        public void changeState(int newState)
        {
            _state = newState;
            stateFrames = 0;
        }
        public void changeParent(ScreenObject obj)
        {
            location += parent.location;
            parent = obj;
        }
        public void RandomizeSpriteEffects()
        {
            int picEffect = Global.rand.Next(3);
            switch (picEffect)
            {
                case 0:
                    spriteEffects = SpriteEffects.None;
                    break;
                case 1:
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    break;
                case 2:
                    spriteEffects = SpriteEffects.FlipVertically;
                    break;
            }
        }
        public override void Setup()
        {
            oldLocation = location;
            oldVelocity = velocity;
            oldAcceleration = acceleration;
        }
        public override void Update(GameTime gameTime)
        {
            if (state != STATE_INACTIVE)
            {
                t = 1;

                oldLocation = location;
                oldVelocity = velocity;
                oldAcceleration = acceleration;

                angularVelocity += angularAcceleration;
                rotation += angularVelocity;
                velocity += acceleration;
                acceleration = Vector2.Zero;
                velocity *= friction;

                if (velocity.Length() < fCritVel) velocity = Vector2.Zero; //static friction
                location += velocity;

                updateAbsLoc();
                stateFrames++;
            }
        }
        public virtual void BounceOnScreen()
        {
            hitWall = false;
            if (location.X > Global.windowWidth)
            {
                velocity.X = Math.Abs(velocity.X) * -1;
                hitWall = true;
            }
            if (location.X < 0)
            {
                velocity.X = Math.Abs(velocity.X);
                hitWall = true;
            }

            if (location.Y > Global.windowHeight)
            {
                velocity.Y = Math.Abs(velocity.X) * -1;
                hitWall = true;
            }
            if (location.Y < 0)
            {
                velocity.Y = Math.Abs(velocity.Y);
                hitWall = true;
            }
        }
        public virtual void BounceOnScreenWalls()
        {
            //Commenting out the while loops can be demonstrated in test 2
            //for cardinal walls and test 4 for angle walls.

            //An issue with the quicksort method is that it will desync the colliding object.
            //Instead of t = 1 per frame, t is changed to time of collision.
            float tpivot;
            float difference;

            hitWall = false;
            #region cardinal bumpers
            difference = (location.X + sRadius) - Global.windowWidth;
            if (difference > Global.accuracy)
            {
                tpivot = t;
                while (tpivot > Global.accuracy)
                {   //quicksort to t when objs first contact
                    tpivot /= 2;
                    if (difference > Global.accuracy) t -= tpivot;
                    else t += tpivot;

                    velocity = oldVelocity + oldAcceleration * t;
                    location = oldLocation + velocity * t;

                    difference = (location.X + sRadius) - Global.windowWidth;
                }
                if (difference > Global.accuracy)
                {
                    location.X = Global.windowWidth - sRadius;
                }

                velocity.X = Math.Abs(velocity.X) * -1;
                hitWall = true;
            }

            difference = sRadius - location.X; //0 - (location.X - sRadius);
            if (difference > Global.accuracy)
            {
                tpivot = t;
                while (tpivot > Global.accuracy)
                {   //quicksort to t when objs first contact
                    tpivot /= 2;
                    if (difference > Global.accuracy) t -= tpivot;
                    else t += tpivot;

                    velocity = oldVelocity + oldAcceleration * t;
                    location = oldLocation + velocity * t;

                    difference = sRadius - location.X;
                }
                if (difference > Global.accuracy)
                {
                    location.X = sRadius;
                }

                velocity.X = Math.Abs(velocity.X);
                hitWall = true;
            }

            difference = (location.Y + sRadius) - Global.windowHeight;
            if (difference > Global.accuracy)
            {
                tpivot = t;
                while (tpivot > Global.accuracy)
                {   //quicksort to t when objs first contact
                    tpivot /= 2;
                    if (difference > Global.accuracy) t -= tpivot;
                    else t += tpivot;

                    velocity = oldVelocity + oldAcceleration * t;
                    location = oldLocation + velocity * t;

                    difference = (location.Y + sRadius) - Global.windowHeight;
                }
                if (difference > Global.accuracy)
                {
                    location.Y = Global.windowHeight - sRadius;
                }

                velocity.Y = Math.Abs(velocity.Y) * -1;
                hitWall = true;
            }

            difference = sRadius - location.Y; //0 - (location.Y - sRadius);
            if (difference > Global.accuracy)
            {
                tpivot = t;
                while (tpivot > Global.accuracy)
                {   //quicksort to t when objs first contact
                    tpivot /= 2;
                    if (difference > Global.accuracy) t -= tpivot;
                    else t += tpivot;

                    velocity = oldVelocity + oldAcceleration * t;
                    location = oldLocation + velocity * t;

                    difference = sRadius - location.Y;
                }
                if (difference > Global.accuracy)
                {
                    location.Y = sRadius;
                }

                velocity.Y = Math.Abs(velocity.Y);
                hitWall = true;
            }
            #endregion
        }
        protected void calcWallPoints(Wall wall)
        {
            #region points
            P4.X = (float)(location.X + sRadius * Math.Cos(wall.phi));
            P4.Y = (float)(location.Y + sRadius * Math.Sin(wall.phi));

            P6.X = (float)(location.X + sRadius * Math.Cos(wall.phi - MathHelper.PiOver2));
            P6.Y = (float)(location.Y + sRadius * Math.Sin(wall.phi - MathHelper.PiOver2));

            P3.X = P4.X;
            P3.Y = wall.m * (P3.X - wall.leftPoint.X) + wall.leftPoint.Y;

            dx = (P4.Y - P3.Y) * Math.Cos(wall.theta);
            P5.X = (float)(dx * Math.Cos(wall.theta + MathHelper.PiOver2));
            P5.Y = (float)(dx * Math.Sin(wall.theta + MathHelper.PiOver2));
            #endregion
        }
        public virtual void BounceOnWalls()
        {
            float tpivot;
            float difference;

            hitWall = false;
            difference = (location.X + radius) - (Global.currentGame.Table.location.X + (1325));
            if (difference > 0)
            {
                tpivot = t;
                while (tpivot > Global.accuracy)
                {   //quicksort to t when objs first contact
                    tpivot /= 2;
                    if (difference > 0) t -= tpivot;
                    else t += tpivot;

                    ModifiedUpdate();

                    difference = (location.X + radius) - (Global.currentGame.Table.location.X + (1325));
                }
                if (difference > Global.accuracy)
                {
                    location.X = Global.currentGame.Table.location.X + (1325) - radius;
                    updateAbsLoc();
                }

                velocity.X = Math.Abs(velocity.X) * -1;
                hitWall = true;
            }

            difference = (Global.currentGame.Table.location.X + (-1325)) - (location.X - radius);
            if (difference > 0)
            {
                tpivot = t;
                while (tpivot > Global.accuracy)
                {   //quicksort to t when objs first contact
                    tpivot /= 2;
                    if (difference > 0) t -= tpivot;
                    else t += tpivot;

                    ModifiedUpdate();

                    difference = (Global.currentGame.Table.location.X + (-1325)) - (location.X - radius);
                }
                if (difference > Global.accuracy)
                {
                    location.X = Global.currentGame.Table.location.X + (-1325) + radius;
                    updateAbsLoc();
                }

                velocity.X = Math.Abs(velocity.X);
                hitWall = true;
            }

            difference = (location.Y + radius) - (Global.currentGame.Table.location.Y + (615));
            if (difference > 0)
            {
                tpivot = t;
                while (tpivot > Global.accuracy)
                {   //quicksort to t when objs first contact
                    tpivot /= 2;
                    if (difference > 0) t -= tpivot;
                    else t += tpivot;

                    ModifiedUpdate();

                    difference = (location.Y + radius) - (Global.currentGame.Table.location.Y + (615));
                }
                if (difference > Global.accuracy)
                {
                    location.Y = Global.currentGame.Table.location.Y + (615) - radius;
                    updateAbsLoc();
                }
                velocity.Y = Math.Abs(velocity.Y) * -1;
                hitWall = true;
            }

            difference = (Global.currentGame.Table.location.Y + (-615)) - (location.Y - radius);
            if (difference > 0)
            {
                tpivot = t;
                while (tpivot > Global.accuracy)
                {   //quicksort to t when objs first contact
                    tpivot /= 2;
                    if (difference > 0) t -= tpivot;
                    else t += tpivot;

                    ModifiedUpdate();

                    difference = (Global.currentGame.Table.location.Y + (-615)) - (location.Y - radius);
                }
                if (difference > Global.accuracy)
                {
                    location.Y = Global.currentGame.Table.location.Y + (-615) + radius;
                    updateAbsLoc();
                }
                velocity.Y = Math.Abs(velocity.Y);
                hitWall = true;
            }
        }
        public virtual void DieOnDistantWalls()
        {
            hitWall = false;
            if (location.X > Global.windowWidth + 200)
            {
                hitWall = true;
            }
            if (location.X < -200)
            {
                hitWall = true;
            }
            if (location.Y > Global.windowHeight + 200)
            {
                hitWall = true;
            }
            if (location.Y < -200)
            {
                hitWall = true;
            }
        }
        public virtual void BounceOnDistantWalls()
        {
            hitWall = false;
            if (location.X > Global.windowWidth + 200)
            {
                location.X = Global.windowWidth + 200;
                velocity.X = Math.Abs(velocity.X) * -1;
                hitWall = true;
            }
            if (location.X < -200)
            {
                location.X = -200;
                velocity.X = Math.Abs(velocity.X);
                hitWall = true;
            }
            if (location.Y > Global.windowHeight + 200)
            {
                location.Y = Global.windowHeight + 200;
                velocity.Y = Math.Abs(velocity.Y) * -1;
                hitWall = true;
            }
            if (location.Y < -200)
            {
                location.Y = -200;
                velocity.Y = Math.Abs(velocity.Y);
                hitWall = true;
            }
        }
        public void BounceOnTableWalls()
        {
            //Table.location is at center of table
            //dimensions: 2840 x 1420 px
            //center: (1420, 710)
            //north bumpers: 95
            //south bumpers: 1325
            //west bumper: 95
            //east bumper: 2745

            hitWall = false;

            #region cardinal bumpers
            BounceOnRightBumper();
            BounceOnLeftBumper();
            BounceOnBottomBumpers();
            BounceOnTopBumpers();
            #endregion
        }
        public void BounceOnTableWallObjects()
        {
            #region angle bumpers
            foreach (Wall wall in Global.currentGame.walls.WallList)
            {
                TableWallCollision(wall);
            }

            #endregion
        }
        private void TableWallCollision(Wall wall)
        {
            float difference = 0;
            float tpivot;

            calcWallPoints(wall);
            #region difference
            //do not refactor, there are many laden return statements to shortcut code
            //this region 
            switch (wall.side)
            {
                #region RIGHT WALL
                case Wall.SIDE_RIGHT:
                    //check for hitting corners
                    if (!(P4.X >= wall.left && P4.Y <= wall.bottom
                        && P4.Y >= wall.top && P4.X <= wall.right))
                    {
                        float diff;
                        #region check bottom corner
                        //if ball is overlapping corner but P4 is oob,
                        //then do a point collision at corner
                        diff = sRadius - Vector2.Distance(wall.bottomPoint, location);
                        if (diff > Global.accuracy)
                        {
                            //check if it is actually hitting the face of another wall
                            if (hittingCorner) return;
                            hittingCorner = true;
                            BounceOnTableWalls();
                            if (!hittingCorner) return;
                            calcWallPoints(wall);

                            hitWall = true;
                            #region cornerCollision
                            #region vars
                            double phi1; //angle of collision
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

                            #region displacement correction
                            //see ListOfCollidables.BallCollision

                            tpivot = t;
                            while (tpivot > Global.accuracy
                                && P4.Y > wall.bottom
                                && !(diff > -Global.accuracy
                                    && diff < Global.accuracy))
                            {   //quicksort to t when objs first contact
                                tpivot /= 2;
                                if (diff > 0) t -= tpivot;
                                else t += tpivot;

                                //ModifiedUpdate();
                                calcWallPoints(wall);
                                diff = sRadius - Vector2.Distance(wall.bottomPoint, location);
                            }
                            #endregion

                            phi1 = Math.Atan2((wall.bottomPoint.Y - location.Y), (wall.bottomPoint.X - location.X));

                            if (diff > -Global.accuracy && diff < Global.accuracy)
                            {
                                #region circular collision detection

                                axis = (float)Math.Tan(phi1);
                                axis /= Math.Abs(axis);
                                if (float.IsNaN(axis)) axis = -1;

                                //ball 1
                                vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                cvtheta1 = phi1 - vtheta1;
                                vAlignment1 = Math.Cos(cvtheta1);
                                v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                v11 = velocity - v12; //split velocity into 2 vectors
                                if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                else v12mag = v12.Length();

                                v12fmag = -v12mag;

                                v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                v1f = v12f + v11;

                                velocity = v1f;
                                #endregion
                            }
                            else
                            {
                                #region old displacement correction
                                Vector2 dx = new Vector2((float)(diff * Math.Cos(phi1)), (float)(diff * Math.Sin(phi1)));
                                displacement1 = -dx;
                                //location += displacement1;
                                updateAbsLoc();
                                #endregion
                                #region old circular collision detection

                                axis = (float)Math.Tan(phi1);
                                axis /= Math.Abs(axis);
                                if (float.IsNaN(axis)) axis = -1;

                                //ball 1
                                vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                cvtheta1 = phi1 - vtheta1;
                                vAlignment1 = Math.Cos(cvtheta1);
                                v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                v11 = velocity - v12; //split velocity into 2 vectors
                                if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                else v12mag = v12.Length();
                                //velocity correction
                                if (oldAcceleration.Length() > 0)
                                {
                                    atheta1 = Math.Atan2(oldAcceleration.Y, oldAcceleration.X);
                                    catheta1 = phi1 - atheta1;
                                    aAlignment1 = Math.Cos(catheta1);
                                    a12 = new Vector2((float)((oldAcceleration.Length() * aAlignment1) * Math.Cos(phi1)), (float)((oldAcceleration.Length() * aAlignment1) * Math.Sin(phi1)));
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

                                v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                v1f = v12f + v11;

                                velocity = v1f;
                                #endregion
                            }
                            oldLocation = location;
                            oldVelocity = velocity;
                            #endregion
                            return;
                        }
                        #endregion
                        #region check top corner
                        diff = sRadius - Vector2.Distance(wall.topPoint, location);
                        if (diff > Global.accuracy)
                        {
                            //check if it is actually hitting the face of another wall
                            if (hittingCorner) return;
                            hittingCorner = true;
                            BounceOnTableWalls();
                            if (!hittingCorner) return;
                            calcWallPoints(wall);

                            hitWall = true;
                            #region cornerCollision
                            #region vars
                            double phi1; //angle of collision
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

                            #region displacement correction
                            //see ListOfCollidables.BallCollision

                            tpivot = t;
                            while (tpivot > Global.accuracy
                                && P4.Y > wall.bottom
                                && !(diff > -Global.accuracy
                                    && diff < Global.accuracy))
                            {   //quicksort to t when objs first contact
                                tpivot /= 2;
                                if (diff > 0) t -= tpivot;
                                else t += tpivot;

                                ModifiedUpdate();
                                calcWallPoints(wall);
                                diff = sRadius - Vector2.Distance(wall.topPoint, location);
                            }
                            #endregion

                            phi1 = Math.Atan2((wall.topPoint.Y - location.Y), (wall.topPoint.X - location.X));

                            if (diff > -Global.accuracy && diff < Global.accuracy)
                            {
                                #region circular collision detection

                                axis = (float)Math.Tan(phi1);
                                axis /= Math.Abs(axis);
                                if (float.IsNaN(axis)) axis = -1;

                                //ball 1
                                vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                cvtheta1 = phi1 - vtheta1;
                                vAlignment1 = Math.Cos(cvtheta1);
                                v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                v11 = velocity - v12; //split velocity into 2 vectors
                                if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                else v12mag = v12.Length();

                                v12fmag = -v12mag;

                                v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                v1f = v12f + v11;

                                velocity = v1f;
                                #endregion
                            }
                            else
                            {
                                #region old displacement correction
                                Vector2 dx = new Vector2((float)(diff * Math.Cos(phi1)), (float)(diff * Math.Sin(phi1)));
                                displacement1 = -dx;
                                location += displacement1;
                                updateAbsLoc();
                                #endregion
                                #region old circular collision detection

                                axis = (float)Math.Tan(phi1);
                                axis /= Math.Abs(axis);
                                if (float.IsNaN(axis)) axis = -1;

                                //ball 1
                                vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                cvtheta1 = phi1 - vtheta1;
                                vAlignment1 = Math.Cos(cvtheta1);
                                v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                v11 = velocity - v12; //split velocity into 2 vectors
                                if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                else v12mag = v12.Length();
                                //velocity correction
                                if (oldAcceleration.Length() > 0)
                                {
                                    atheta1 = Math.Atan2(oldAcceleration.Y, oldAcceleration.X);
                                    catheta1 = phi1 - atheta1;
                                    aAlignment1 = Math.Cos(catheta1);
                                    a12 = new Vector2((float)((oldAcceleration.Length() * aAlignment1) * Math.Cos(phi1)), (float)((oldAcceleration.Length() * aAlignment1) * Math.Sin(phi1)));
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

                                v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                v1f = v12f + v11;

                                velocity = v1f;
                                #endregion
                            }
                            oldLocation = location;
                            oldVelocity = velocity;
                            #endregion
                            return;
                        }
                        #endregion
                        return;
                    }
                    //corners have not been hit
                    difference = P5.Length() * Math.Sign(P5.X);
                    break;
                #endregion
                #region LEFT WALL
                case Wall.SIDE_LEFT:
                    //check for hitting corners
                    if (!(P4.X >= wall.left && P4.Y <= wall.bottom
                        && P4.Y >= wall.top && P4.X <= wall.right))
                    {
                        double diff;
                        #region check bottom corner
                        //when ball is overlapping corner but P4 is oob
                        diff = sRadius - Vector2.Distance(wall.bottomPoint, location);
                        if (diff > Global.accuracy)
                        {
                            //check if it is actually hitting the face of another wall
                            if (hittingCorner) return;
                            hittingCorner = true;
                            BounceOnTableWalls();
                            if (!hittingCorner) return;
                            calcWallPoints(wall);

                            hitWall = true;
                            #region cornerCollision
                            #region vars
                            double phi1; //angle of collision
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

                            #region displacement correction
                            //see ListOfCollidables.BallCollision

                            tpivot = t;
                            while (tpivot > Global.accuracy
                                && P4.Y > wall.bottom
                                && !(diff > -Global.accuracy
                                    && diff < Global.accuracy))
                            {   //quicksort to t when objs first contact
                                tpivot /= 2;
                                if (diff > 0) t -= tpivot;
                                else t += tpivot;

                                ModifiedUpdate();
                                calcWallPoints(wall);
                                diff = Math.Abs(sRadius - Vector2.Distance(wall.bottomPoint, location));
                            }
                            #endregion

                            phi1 = Math.Atan2((wall.bottomPoint.Y - location.Y), (wall.bottomPoint.X - location.X));

                            if (diff > -Global.accuracy && diff < Global.accuracy)
                            {
                                #region circular collision detection

                                axis = (float)Math.Tan(phi1);
                                axis /= Math.Abs(axis);
                                if (float.IsNaN(axis)) axis = -1;

                                //ball 1
                                vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                cvtheta1 = phi1 - vtheta1;
                                vAlignment1 = Math.Cos(cvtheta1);
                                v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                v11 = velocity - v12; //split velocity into 2 vectors
                                if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                else v12mag = v12.Length();

                                v12fmag = -v12mag;

                                v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                v1f = v12f + v11;

                                velocity = v1f;
                                #endregion
                            }
                            else
                            {
                                #region old displacement correction
                                Vector2 dx = new Vector2((float)(diff * Math.Cos(phi1)), (float)(diff * Math.Sin(phi1)));
                                displacement1 = -dx;
                                location += displacement1;
                                updateAbsLoc();
                                #endregion
                                #region old circular collision detection

                                axis = (float)Math.Tan(phi1);
                                axis /= Math.Abs(axis);
                                if (float.IsNaN(axis)) axis = -1;

                                //ball 1
                                vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                cvtheta1 = phi1 - vtheta1;
                                vAlignment1 = Math.Cos(cvtheta1);
                                v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                v11 = velocity - v12; //split velocity into 2 vectors
                                if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                else v12mag = v12.Length();
                                //velocity correction
                                if (oldAcceleration.Length() > 0)
                                {
                                    atheta1 = Math.Atan2(oldAcceleration.Y, oldAcceleration.X);
                                    catheta1 = phi1 - atheta1;
                                    aAlignment1 = Math.Cos(catheta1);
                                    a12 = new Vector2((float)((oldAcceleration.Length() * aAlignment1) * Math.Cos(phi1)), (float)((oldAcceleration.Length() * aAlignment1) * Math.Sin(phi1)));
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

                                v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                v1f = v12f + v11;

                                velocity = v1f;
                                #endregion
                            }
                            oldLocation = location;
                            oldVelocity = velocity;
                            #endregion
                            return;
                        }
                        #endregion
                        #region check top corner
                        diff = sRadius - Vector2.Distance(wall.topPoint, location);
                        if (diff > Global.accuracy)
                        {
                            //check if it is actually hitting the face of another wall
                            if (hittingCorner) return;
                            hittingCorner = true;
                            BounceOnTableWalls();
                            if (!hittingCorner) return;
                            calcWallPoints(wall);

                            hitWall = true;
                            #region cornerCollision
                            #region vars
                            double phi1; //angle of collision
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

                            #region displacement correction
                            //see ListOfCollidables.BallCollision

                            tpivot = t;
                            while (tpivot > Global.accuracy
                                && P4.Y > wall.bottom
                                && !(diff > -Global.accuracy
                                    && diff < Global.accuracy))
                            {   //quicksort to t when objs first contact
                                tpivot /= 2;
                                if (diff > 0) t -= tpivot;
                                else t += tpivot;

                                ModifiedUpdate();
                                calcWallPoints(wall);
                                diff = Math.Abs(sRadius - Vector2.Distance(wall.topPoint, location));
                            }
                            #endregion

                            phi1 = Math.Atan2((wall.topPoint.Y - location.Y), (wall.topPoint.X - location.X));

                            if (diff > -Global.accuracy && diff < Global.accuracy)
                            {
                                #region circular collision detection

                                axis = (float)Math.Tan(phi1);
                                axis /= Math.Abs(axis);
                                if (float.IsNaN(axis)) axis = -1;

                                //ball 1
                                vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                cvtheta1 = phi1 - vtheta1;
                                vAlignment1 = Math.Cos(cvtheta1);
                                v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                v11 = velocity - v12; //split velocity into 2 vectors
                                if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                else v12mag = v12.Length();

                                v12fmag = -v12mag;

                                v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                v1f = v12f + v11;

                                velocity = v1f;
                                #endregion
                            }
                            else
                            {
                                #region old displacement correction
                                Vector2 dx = new Vector2((float)(diff * Math.Cos(phi1)), (float)(diff * Math.Sin(phi1)));
                                displacement1 = -dx;
                                location += displacement1;
                                updateAbsLoc();
                                #endregion
                                #region old circular collision detection

                                axis = (float)Math.Tan(phi1);
                                axis /= Math.Abs(axis);
                                if (float.IsNaN(axis)) axis = -1;

                                //ball 1
                                vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                cvtheta1 = phi1 - vtheta1;
                                vAlignment1 = Math.Cos(cvtheta1);
                                v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                v11 = velocity - v12; //split velocity into 2 vectors
                                if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                else v12mag = v12.Length();
                                //velocity correction
                                if (oldAcceleration.Length() > 0)
                                {
                                    atheta1 = Math.Atan2(oldAcceleration.Y, oldAcceleration.X);
                                    catheta1 = phi1 - atheta1;
                                    aAlignment1 = Math.Cos(catheta1);
                                    a12 = new Vector2((float)((oldAcceleration.Length() * aAlignment1) * Math.Cos(phi1)), (float)((oldAcceleration.Length() * aAlignment1) * Math.Sin(phi1)));
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

                                v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                v1f = v12f + v11;

                                velocity = v1f;
                                #endregion
                            }
                            oldLocation = location;
                            oldVelocity = velocity;
                            #endregion
                            return;
                        }
                        #endregion
                        return;
                    }
                    //corners have not been hit
                    difference = -(P5.Length() * Math.Sign(P5.X));
                    break;
                #endregion
            }
            #endregion
            if (difference > Global.accuracy)
            {
                //hitting face of wall
                hittingCorner = false;
                tpivot = t;
                //the result of commenting out this while loop can be seen in test 4 (f12)
                //tpivot probably doesnt need Global.accuracy beyond 0.001f
                while (!(difference > -Global.accuracy && difference < Global.accuracy) && tpivot > Global.accuracy)
                {
                    //quicksort to t when objs first contact
                    tpivot /= 2;
                    if (difference > Global.accuracy) t -= tpivot;
                    else t += tpivot;

                    ModifiedUpdate();
                    calcWallPoints(wall);
                    #region difference
                    switch (wall.side)
                    {
                        #region RIGHT WALL
                        case Wall.SIDE_RIGHT:
                            //check for hitting corners
                            if (!(P4.X >= wall.left && P4.Y <= wall.bottom
                                && P4.Y >= wall.top && P4.X <= wall.right))
                            {
                                float diff;
                                #region check bottom corner
                                //if ball is overlapping corner but P4 is oob,
                                //then do a point collision at corner
                                diff = sRadius - Vector2.Distance(wall.bottomPoint, location);
                                if (diff > Global.accuracy)
                                {
                                    //check if it is actually hitting the face of another wall
                                    if (hittingCorner) return;
                                    hittingCorner = true;
                                    BounceOnTableWalls();
                                    if (!hittingCorner) return;
                                    calcWallPoints(wall);

                                    hitWall = true;
                                    #region cornerCollision
                                    #region vars
                                    double phi1; //angle of collision
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

                                    #region displacement correction
                                    //see ListOfCollidables.BallCollision

                                    tpivot = t;
                                    while (tpivot > Global.accuracy
                                        && P4.Y > wall.bottom
                                        && !(diff > -Global.accuracy
                                            && diff < Global.accuracy))
                                    {   //quicksort to t when objs first contact
                                        tpivot /= 2;
                                        if (diff > 0) t -= tpivot;
                                        else t += tpivot;

                                        ModifiedUpdate();
                                        calcWallPoints(wall);
                                        diff = sRadius - Vector2.Distance(wall.bottomPoint, location);
                                    }
                                    #endregion

                                    phi1 = Math.Atan2((wall.bottomPoint.Y - location.Y), (wall.bottomPoint.X - location.X));

                                    if (diff > -Global.accuracy && diff < Global.accuracy)
                                    {
                                        #region circular collision detection

                                        axis = (float)Math.Tan(phi1);
                                        axis /= Math.Abs(axis);
                                        if (float.IsNaN(axis)) axis = -1;

                                        //ball 1
                                        vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                        cvtheta1 = phi1 - vtheta1;
                                        vAlignment1 = Math.Cos(cvtheta1);
                                        v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                        v11 = velocity - v12; //split velocity into 2 vectors
                                        if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                        else v12mag = v12.Length();

                                        v12fmag = -v12mag;

                                        v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                        v1f = v12f + v11;

                                        velocity = v1f;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region old displacement correction
                                        Vector2 dx = new Vector2((float)(diff * Math.Cos(phi1)), (float)(diff * Math.Sin(phi1)));
                                        displacement1 = -dx;
                                        location += displacement1;
                                        updateAbsLoc();
                                        #endregion
                                        #region old circular collision detection

                                        axis = (float)Math.Tan(phi1);
                                        axis /= Math.Abs(axis);
                                        if (float.IsNaN(axis)) axis = -1;

                                        //ball 1
                                        vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                        cvtheta1 = phi1 - vtheta1;
                                        vAlignment1 = Math.Cos(cvtheta1);
                                        v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                        v11 = velocity - v12; //split velocity into 2 vectors
                                        if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                        else v12mag = v12.Length();
                                        //velocity correction
                                        if (oldAcceleration.Length() > 0)
                                        {
                                            atheta1 = Math.Atan2(oldAcceleration.Y, oldAcceleration.X);
                                            catheta1 = phi1 - atheta1;
                                            aAlignment1 = Math.Cos(catheta1);
                                            a12 = new Vector2((float)((oldAcceleration.Length() * aAlignment1) * Math.Cos(phi1)), (float)((oldAcceleration.Length() * aAlignment1) * Math.Sin(phi1)));
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

                                        v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                        v1f = v12f + v11;

                                        velocity = v1f;
                                        #endregion
                                    }
                                    oldLocation = location;
                                    oldVelocity = velocity;
                                    #endregion
                                    return;
                                }
                                #endregion
                                #region check top corner
                                diff = sRadius - Vector2.Distance(wall.topPoint, location);
                                if (diff > Global.accuracy)
                                {
                                    //check if it is actually hitting the face of another wall
                                    if (hittingCorner) return;
                                    hittingCorner = true;
                                    BounceOnTableWalls();
                                    if (!hittingCorner) return;
                                    calcWallPoints(wall);

                                    hitWall = true;
                                    #region cornerCollision
                                    #region vars
                                    double phi1; //angle of collision
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

                                    #region displacement correction
                                    //see ListOfCollidables.BallCollision

                                    tpivot = t;
                                    while (tpivot > Global.accuracy
                                        && P4.Y > wall.bottom
                                        && !(diff > -Global.accuracy
                                            && diff < Global.accuracy))
                                    {   //quicksort to t when objs first contact
                                        tpivot /= 2;
                                        if (diff > 0) t -= tpivot;
                                        else t += tpivot;

                                        ModifiedUpdate();
                                        calcWallPoints(wall);
                                        diff = sRadius - Vector2.Distance(wall.topPoint, location);
                                    }
                                    #endregion

                                    phi1 = Math.Atan2((wall.topPoint.Y - location.Y), (wall.topPoint.X - location.X));

                                    if (diff > -Global.accuracy && diff < Global.accuracy)
                                    {
                                        #region circular collision detection

                                        axis = (float)Math.Tan(phi1);
                                        axis /= Math.Abs(axis);
                                        if (float.IsNaN(axis)) axis = -1;

                                        //ball 1
                                        vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                        cvtheta1 = phi1 - vtheta1;
                                        vAlignment1 = Math.Cos(cvtheta1);
                                        v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                        v11 = velocity - v12; //split velocity into 2 vectors
                                        if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                        else v12mag = v12.Length();

                                        v12fmag = -v12mag;

                                        v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                        v1f = v12f + v11;

                                        velocity = v1f;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region old displacement correction
                                        Vector2 dx = new Vector2((float)(diff * Math.Cos(phi1)), (float)(diff * Math.Sin(phi1)));
                                        displacement1 = -dx;
                                        location += displacement1;
                                        updateAbsLoc();
                                        #endregion
                                        #region old circular collision detection

                                        axis = (float)Math.Tan(phi1);
                                        axis /= Math.Abs(axis);
                                        if (float.IsNaN(axis)) axis = -1;

                                        //ball 1
                                        vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                        cvtheta1 = phi1 - vtheta1;
                                        vAlignment1 = Math.Cos(cvtheta1);
                                        v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                        v11 = velocity - v12; //split velocity into 2 vectors
                                        if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                        else v12mag = v12.Length();
                                        //velocity correction
                                        if (oldAcceleration.Length() > 0)
                                        {
                                            atheta1 = Math.Atan2(oldAcceleration.Y, oldAcceleration.X);
                                            catheta1 = phi1 - atheta1;
                                            aAlignment1 = Math.Cos(catheta1);
                                            a12 = new Vector2((float)((oldAcceleration.Length() * aAlignment1) * Math.Cos(phi1)), (float)((oldAcceleration.Length() * aAlignment1) * Math.Sin(phi1)));
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

                                        v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                        v1f = v12f + v11;

                                        velocity = v1f;
                                        #endregion
                                    }
                                    oldLocation = location;
                                    oldVelocity = velocity;
                                    #endregion
                                    return;
                                }
                                #endregion
                                return;
                            }
                            //corners have not been hit
                            difference = P5.Length() * Math.Sign(P5.X);
                            break;
                        #endregion
                        #region LEFT WALL
                        case Wall.SIDE_LEFT:
                            //check for hitting corners
                            if (!(P4.X >= wall.left && P4.Y <= wall.bottom
                                && P4.Y >= wall.top && P4.X <= wall.right))
                            {
                                double diff;
                                #region check bottom corner
                                //when ball is overlapping corner but P4 is oob
                                diff = sRadius - Vector2.Distance(wall.bottomPoint, location);
                                if (diff > Global.accuracy)
                                {
                                    //check if it is actually hitting the face of another wall
                                    if (hittingCorner) return;
                                    hittingCorner = true;
                                    BounceOnTableWalls();
                                    if (!hittingCorner) return;
                                    calcWallPoints(wall);

                                    hitWall = true;
                                    #region cornerCollision
                                    #region vars
                                    double phi1; //angle of collision
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

                                    #region displacement correction
                                    //see ListOfCollidables.BallCollision

                                    tpivot = t;
                                    while (tpivot > Global.accuracy
                                        && P4.Y > wall.bottom
                                        && !(diff > -Global.accuracy
                                            && diff < Global.accuracy))
                                    {   //quicksort to t when objs first contact
                                        tpivot /= 2;
                                        if (diff > 0) t -= tpivot;
                                        else t += tpivot;

                                        ModifiedUpdate();
                                        calcWallPoints(wall);
                                        diff = Math.Abs(sRadius - Vector2.Distance(wall.bottomPoint, location));
                                    }
                                    #endregion

                                    phi1 = Math.Atan2((wall.bottomPoint.Y - location.Y), (wall.bottomPoint.X - location.X));

                                    if (diff > -Global.accuracy && diff < Global.accuracy)
                                    {
                                        #region circular collision detection

                                        axis = (float)Math.Tan(phi1);
                                        axis /= Math.Abs(axis);
                                        if (float.IsNaN(axis)) axis = -1;

                                        //ball 1
                                        vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                        cvtheta1 = phi1 - vtheta1;
                                        vAlignment1 = Math.Cos(cvtheta1);
                                        v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                        v11 = velocity - v12; //split velocity into 2 vectors
                                        if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                        else v12mag = v12.Length();

                                        v12fmag = -v12mag;

                                        v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                        v1f = v12f + v11;

                                        velocity = v1f;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region old displacement correction
                                        Vector2 dx = new Vector2((float)(diff * Math.Cos(phi1)), (float)(diff * Math.Sin(phi1)));
                                        displacement1 = -dx;
                                        location += displacement1;
                                        updateAbsLoc();
                                        #endregion
                                        #region old circular collision detection

                                        axis = (float)Math.Tan(phi1);
                                        axis /= Math.Abs(axis);
                                        if (float.IsNaN(axis)) axis = -1;

                                        //ball 1
                                        vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                        cvtheta1 = phi1 - vtheta1;
                                        vAlignment1 = Math.Cos(cvtheta1);
                                        v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                        v11 = velocity - v12; //split velocity into 2 vectors
                                        if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                        else v12mag = v12.Length();
                                        //velocity correction
                                        if (oldAcceleration.Length() > 0)
                                        {
                                            atheta1 = Math.Atan2(oldAcceleration.Y, oldAcceleration.X);
                                            catheta1 = phi1 - atheta1;
                                            aAlignment1 = Math.Cos(catheta1);
                                            a12 = new Vector2((float)((oldAcceleration.Length() * aAlignment1) * Math.Cos(phi1)), (float)((oldAcceleration.Length() * aAlignment1) * Math.Sin(phi1)));
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

                                        v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                        v1f = v12f + v11;

                                        velocity = v1f;
                                        #endregion
                                    }
                                    oldLocation = location;
                                    oldVelocity = velocity;
                                    #endregion
                                    return;
                                }
                                #endregion
                                #region check top corner
                                diff = sRadius - Vector2.Distance(wall.topPoint, location);
                                if (diff > Global.accuracy)
                                {
                                    //check if it is actually hitting the face of another wall
                                    if (hittingCorner) return;
                                    hittingCorner = true;
                                    BounceOnTableWalls();
                                    if (!hittingCorner) return;
                                    calcWallPoints(wall);

                                    hitWall = true;
                                    #region cornerCollision
                                    #region vars
                                    double phi1; //angle of collision
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

                                    #region displacement correction
                                    //see ListOfCollidables.BallCollision

                                    tpivot = t;
                                    while (tpivot > Global.accuracy
                                        && P4.Y > wall.bottom
                                        && !(diff > -Global.accuracy
                                            && diff < Global.accuracy))
                                    {   //quicksort to t when objs first contact
                                        tpivot /= 2;
                                        if (diff > 0) t -= tpivot;
                                        else t += tpivot;

                                        ModifiedUpdate();
                                        calcWallPoints(wall);
                                        diff = Math.Abs(sRadius - Vector2.Distance(wall.topPoint, location));
                                    }
                                    #endregion

                                    phi1 = Math.Atan2((wall.topPoint.Y - location.Y), (wall.topPoint.X - location.X));

                                    if (diff > -Global.accuracy && diff < Global.accuracy)
                                    {
                                        #region circular collision detection

                                        axis = (float)Math.Tan(phi1);
                                        axis /= Math.Abs(axis);
                                        if (float.IsNaN(axis)) axis = -1;

                                        //ball 1
                                        vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                        cvtheta1 = phi1 - vtheta1;
                                        vAlignment1 = Math.Cos(cvtheta1);
                                        v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                        v11 = velocity - v12; //split velocity into 2 vectors
                                        if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                        else v12mag = v12.Length();

                                        v12fmag = -v12mag;

                                        v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                        v1f = v12f + v11;

                                        velocity = v1f;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region old displacement correction
                                        Vector2 dx = new Vector2((float)(diff * Math.Cos(phi1)), (float)(diff * Math.Sin(phi1)));
                                        displacement1 = -dx;
                                        location += displacement1;
                                        updateAbsLoc();
                                        #endregion
                                        #region old circular collision detection

                                        axis = (float)Math.Tan(phi1);
                                        axis /= Math.Abs(axis);
                                        if (float.IsNaN(axis)) axis = -1;

                                        //ball 1
                                        vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                        cvtheta1 = phi1 - vtheta1;
                                        vAlignment1 = Math.Cos(cvtheta1);
                                        v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                        v11 = velocity - v12; //split velocity into 2 vectors
                                        if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                        else v12mag = v12.Length();
                                        //velocity correction
                                        if (oldAcceleration.Length() > 0)
                                        {
                                            atheta1 = Math.Atan2(oldAcceleration.Y, oldAcceleration.X);
                                            catheta1 = phi1 - atheta1;
                                            aAlignment1 = Math.Cos(catheta1);
                                            a12 = new Vector2((float)((oldAcceleration.Length() * aAlignment1) * Math.Cos(phi1)), (float)((oldAcceleration.Length() * aAlignment1) * Math.Sin(phi1)));
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

                                        v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                        v1f = v12f + v11;

                                        velocity = v1f;
                                        #endregion
                                    }
                                    oldLocation = location;
                                    oldVelocity = velocity;
                                    #endregion
                                    return;
                                }
                                #endregion
                                return;
                            }
                            //corners have not been hit
                            difference = -(P5.Length() * Math.Sign(P5.X));
                            break;
                        #endregion
                    }
                    #endregion
                }

                //rebound
                hitWall = true;
                double vtheta = Math.Atan2(velocity.Y, velocity.X);
                double alpha = 0; //new v angle
                double gamma = 0; //angle between v and wall
                float vLen = velocity.Length();
                alpha = 2 * wall.theta - vtheta;
                gamma = vtheta - wall.theta;
                velocity.X = (float)(vLen * Math.Cos(alpha));
                velocity.Y = (float)(vLen * Math.Sin(alpha));

                //if ball was already inside wall at oldLocation
                //or if the ball hits at a shallow angle
                if (difference > Global.accuracy ||
                    Math.Abs(gamma) < Global.PiOver8)
                {
                    location -= P5;
                    updateAbsLoc();
                }
            }
        }
        public void BounceOnWallObjects()
        {
            #region angle bumpers
            foreach (Wall wall in Global.currentGame.walls.WallList)
            {
                WallCollision(wall);
            }

            #endregion
        }
        private void WallCollision(Wall wall)
        {
            float difference = 0;
            float tpivot;

            calcWallPoints(wall);
            #region difference
            //do not refactor, there are many laden return statements to shortcut code
            //this region 
            switch (wall.side)
            {
                #region RIGHT WALL
                case Wall.SIDE_RIGHT:
                    //check for hitting corners
                    if (!(P4.X >= wall.left && P4.Y <= wall.bottom
                        && P4.Y >= wall.top && P4.X <= wall.right))
                    {
                        float diff;
                        #region check bottom corner
                        //if ball is overlapping corner but P4 is oob,
                        //then do a point collision at corner
                        diff = sRadius - Vector2.Distance(wall.bottomPoint, location);
                        if (diff > Global.accuracy)
                        {
                            //check if it is actually hitting the face of another wall
                            if (hittingCorner) return;
                            hittingCorner = true;
                            BounceOnScreenWalls();
                            if (!hittingCorner) return;
                            calcWallPoints(wall);

                            hitWall = true;
                            #region cornerCollision
                            #region vars
                            double phi1; //angle of collision
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

                            #region displacement correction
                            //see ListOfCollidables.BallCollision

                            tpivot = t;
                            while (tpivot > Global.accuracy
                                && P4.Y > wall.bottom
                                && !(diff > -Global.accuracy
                                    && diff < Global.accuracy))
                            {   //quicksort to t when objs first contact
                                tpivot /= 2;
                                if (diff > 0) t -= tpivot;
                                else t += tpivot;

                                //ModifiedUpdate();
                                calcWallPoints(wall);
                                diff = sRadius - Vector2.Distance(wall.bottomPoint, location);
                            }
                            #endregion

                            phi1 = Math.Atan2((wall.bottomPoint.Y - location.Y), (wall.bottomPoint.X - location.X));

                            if (diff > -Global.accuracy && diff < Global.accuracy)
                            {
                                #region circular collision detection

                                axis = (float)Math.Tan(phi1);
                                axis /= Math.Abs(axis);
                                if (float.IsNaN(axis)) axis = -1;

                                //ball 1
                                vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                cvtheta1 = phi1 - vtheta1;
                                vAlignment1 = Math.Cos(cvtheta1);
                                v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                v11 = velocity - v12; //split velocity into 2 vectors
                                if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                else v12mag = v12.Length();

                                v12fmag = -v12mag;

                                v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                v1f = v12f + v11;

                                velocity = v1f;
                                #endregion
                            }
                            else
                            {
                                #region old displacement correction
                                Vector2 dx = new Vector2((float)(diff * Math.Cos(phi1)), (float)(diff * Math.Sin(phi1)));
                                displacement1 = -dx;
                                //location += displacement1;
                                updateAbsLoc();
                                #endregion
                                #region old circular collision detection

                                axis = (float)Math.Tan(phi1);
                                axis /= Math.Abs(axis);
                                if (float.IsNaN(axis)) axis = -1;

                                //ball 1
                                vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                cvtheta1 = phi1 - vtheta1;
                                vAlignment1 = Math.Cos(cvtheta1);
                                v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                v11 = velocity - v12; //split velocity into 2 vectors
                                if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                else v12mag = v12.Length();
                                //velocity correction
                                if (oldAcceleration.Length() > 0)
                                {
                                    atheta1 = Math.Atan2(oldAcceleration.Y, oldAcceleration.X);
                                    catheta1 = phi1 - atheta1;
                                    aAlignment1 = Math.Cos(catheta1);
                                    a12 = new Vector2((float)((oldAcceleration.Length() * aAlignment1) * Math.Cos(phi1)), (float)((oldAcceleration.Length() * aAlignment1) * Math.Sin(phi1)));
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

                                v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                v1f = v12f + v11;

                                velocity = v1f;
                                #endregion
                            }
                            oldLocation = location;
                            oldVelocity = velocity;
                            #endregion
                            return;
                        }
                        #endregion
                        #region check top corner
                        diff = sRadius - Vector2.Distance(wall.topPoint, location);
                        if (diff > Global.accuracy)
                        {
                            //check if it is actually hitting the face of another wall
                            if (hittingCorner) return;
                            hittingCorner = true;
                            BounceOnScreenWalls();
                            if (!hittingCorner) return;
                            calcWallPoints(wall);

                            hitWall = true;
                            #region cornerCollision
                            #region vars
                            double phi1; //angle of collision
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

                            #region displacement correction
                            //see ListOfCollidables.BallCollision

                            tpivot = t;
                            while (tpivot > Global.accuracy
                                && P4.Y > wall.bottom
                                && !(diff > -Global.accuracy
                                    && diff < Global.accuracy))
                            {   //quicksort to t when objs first contact
                                tpivot /= 2;
                                if (diff > 0) t -= tpivot;
                                else t += tpivot;

                                ModifiedUpdate();
                                calcWallPoints(wall);
                                diff = sRadius - Vector2.Distance(wall.topPoint, location);
                            }
                            #endregion

                            phi1 = Math.Atan2((wall.topPoint.Y - location.Y), (wall.topPoint.X - location.X));

                            if (diff > -Global.accuracy && diff < Global.accuracy)
                            {
                                #region circular collision detection

                                axis = (float)Math.Tan(phi1);
                                axis /= Math.Abs(axis);
                                if (float.IsNaN(axis)) axis = -1;

                                //ball 1
                                vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                cvtheta1 = phi1 - vtheta1;
                                vAlignment1 = Math.Cos(cvtheta1);
                                v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                v11 = velocity - v12; //split velocity into 2 vectors
                                if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                else v12mag = v12.Length();

                                v12fmag = -v12mag;

                                v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                v1f = v12f + v11;

                                velocity = v1f;
                                #endregion
                            }
                            else
                            {
                                #region old displacement correction
                                Vector2 dx = new Vector2((float)(diff * Math.Cos(phi1)), (float)(diff * Math.Sin(phi1)));
                                displacement1 = -dx;
                                location += displacement1;
                                updateAbsLoc();
                                #endregion
                                #region old circular collision detection

                                axis = (float)Math.Tan(phi1);
                                axis /= Math.Abs(axis);
                                if (float.IsNaN(axis)) axis = -1;

                                //ball 1
                                vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                cvtheta1 = phi1 - vtheta1;
                                vAlignment1 = Math.Cos(cvtheta1);
                                v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                v11 = velocity - v12; //split velocity into 2 vectors
                                if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                else v12mag = v12.Length();
                                //velocity correction
                                if (oldAcceleration.Length() > 0)
                                {
                                    atheta1 = Math.Atan2(oldAcceleration.Y, oldAcceleration.X);
                                    catheta1 = phi1 - atheta1;
                                    aAlignment1 = Math.Cos(catheta1);
                                    a12 = new Vector2((float)((oldAcceleration.Length() * aAlignment1) * Math.Cos(phi1)), (float)((oldAcceleration.Length() * aAlignment1) * Math.Sin(phi1)));
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

                                v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                v1f = v12f + v11;

                                velocity = v1f;
                                #endregion
                            }
                            oldLocation = location;
                            oldVelocity = velocity;
                            #endregion
                            return;
                        }
                        #endregion
                        return;
                    }
                    //corners have not been hit
                    difference = P5.Length() * Math.Sign(P5.X);
                    break;
                #endregion
                #region LEFT WALL
                case Wall.SIDE_LEFT:
                    //check for hitting corners
                    if (!(P4.X >= wall.left && P4.Y <= wall.bottom
                        && P4.Y >= wall.top && P4.X <= wall.right))
                    {
                        double diff;
                        #region check bottom corner
                        //when ball is overlapping corner but P4 is oob
                        diff = sRadius - Vector2.Distance(wall.bottomPoint, location);
                        if (diff > Global.accuracy)
                        {
                            //check if it is actually hitting the face of another wall
                            if (hittingCorner) return;
                            hittingCorner = true;
                            BounceOnScreenWalls();
                            if (!hittingCorner) return;
                            calcWallPoints(wall);

                            hitWall = true;
                            #region cornerCollision
                            #region vars
                            double phi1; //angle of collision
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

                            #region displacement correction
                            //see ListOfCollidables.BallCollision

                            tpivot = t;
                            while (tpivot > Global.accuracy
                                && P4.Y > wall.bottom
                                && !(diff > -Global.accuracy
                                    && diff < Global.accuracy))
                            {   //quicksort to t when objs first contact
                                tpivot /= 2;
                                if (diff > 0) t -= tpivot;
                                else t += tpivot;

                                ModifiedUpdate();
                                calcWallPoints(wall);
                                diff = Math.Abs(sRadius - Vector2.Distance(wall.bottomPoint, location));
                            }
                            #endregion

                            phi1 = Math.Atan2((wall.bottomPoint.Y - location.Y), (wall.bottomPoint.X - location.X));

                            if (diff > -Global.accuracy && diff < Global.accuracy)
                            {
                                #region circular collision detection

                                axis = (float)Math.Tan(phi1);
                                axis /= Math.Abs(axis);
                                if (float.IsNaN(axis)) axis = -1;

                                //ball 1
                                vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                cvtheta1 = phi1 - vtheta1;
                                vAlignment1 = Math.Cos(cvtheta1);
                                v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                v11 = velocity - v12; //split velocity into 2 vectors
                                if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                else v12mag = v12.Length();

                                v12fmag = -v12mag;

                                v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                v1f = v12f + v11;

                                velocity = v1f;
                                #endregion
                            }
                            else
                            {
                                #region old displacement correction
                                Vector2 dx = new Vector2((float)(diff * Math.Cos(phi1)), (float)(diff * Math.Sin(phi1)));
                                displacement1 = -dx;
                                location += displacement1;
                                updateAbsLoc();
                                #endregion
                                #region old circular collision detection

                                axis = (float)Math.Tan(phi1);
                                axis /= Math.Abs(axis);
                                if (float.IsNaN(axis)) axis = -1;

                                //ball 1
                                vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                cvtheta1 = phi1 - vtheta1;
                                vAlignment1 = Math.Cos(cvtheta1);
                                v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                v11 = velocity - v12; //split velocity into 2 vectors
                                if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                else v12mag = v12.Length();
                                //velocity correction
                                if (oldAcceleration.Length() > 0)
                                {
                                    atheta1 = Math.Atan2(oldAcceleration.Y, oldAcceleration.X);
                                    catheta1 = phi1 - atheta1;
                                    aAlignment1 = Math.Cos(catheta1);
                                    a12 = new Vector2((float)((oldAcceleration.Length() * aAlignment1) * Math.Cos(phi1)), (float)((oldAcceleration.Length() * aAlignment1) * Math.Sin(phi1)));
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

                                v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                v1f = v12f + v11;

                                velocity = v1f;
                                #endregion
                            }
                            oldLocation = location;
                            oldVelocity = velocity;
                            #endregion
                            return;
                        }
                        #endregion
                        #region check top corner
                        diff = sRadius - Vector2.Distance(wall.topPoint, location);
                        if (diff > Global.accuracy)
                        {
                            //check if it is actually hitting the face of another wall
                            if (hittingCorner) return;
                            hittingCorner = true;
                            BounceOnScreenWalls();
                            if (!hittingCorner) return;
                            calcWallPoints(wall);

                            hitWall = true;
                            #region cornerCollision
                            #region vars
                            double phi1; //angle of collision
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

                            #region displacement correction
                            //see ListOfCollidables.BallCollision

                            tpivot = t;
                            while (tpivot > Global.accuracy
                                && P4.Y > wall.bottom
                                && !(diff > -Global.accuracy
                                    && diff < Global.accuracy))
                            {   //quicksort to t when objs first contact
                                tpivot /= 2;
                                if (diff > 0) t -= tpivot;
                                else t += tpivot;

                                ModifiedUpdate();
                                calcWallPoints(wall);
                                diff = Math.Abs(sRadius - Vector2.Distance(wall.topPoint, location));
                            }
                            #endregion

                            phi1 = Math.Atan2((wall.topPoint.Y - location.Y), (wall.topPoint.X - location.X));

                            if (diff > -Global.accuracy && diff < Global.accuracy)
                            {
                                #region circular collision detection

                                axis = (float)Math.Tan(phi1);
                                axis /= Math.Abs(axis);
                                if (float.IsNaN(axis)) axis = -1;

                                //ball 1
                                vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                cvtheta1 = phi1 - vtheta1;
                                vAlignment1 = Math.Cos(cvtheta1);
                                v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                v11 = velocity - v12; //split velocity into 2 vectors
                                if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                else v12mag = v12.Length();

                                v12fmag = -v12mag;

                                v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                v1f = v12f + v11;

                                velocity = v1f;
                                #endregion
                            }
                            else
                            {
                                #region old displacement correction
                                Vector2 dx = new Vector2((float)(diff * Math.Cos(phi1)), (float)(diff * Math.Sin(phi1)));
                                displacement1 = -dx;
                                location += displacement1;
                                updateAbsLoc();
                                #endregion
                                #region old circular collision detection

                                axis = (float)Math.Tan(phi1);
                                axis /= Math.Abs(axis);
                                if (float.IsNaN(axis)) axis = -1;

                                //ball 1
                                vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                cvtheta1 = phi1 - vtheta1;
                                vAlignment1 = Math.Cos(cvtheta1);
                                v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                v11 = velocity - v12; //split velocity into 2 vectors
                                if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                else v12mag = v12.Length();
                                //velocity correction
                                if (oldAcceleration.Length() > 0)
                                {
                                    atheta1 = Math.Atan2(oldAcceleration.Y, oldAcceleration.X);
                                    catheta1 = phi1 - atheta1;
                                    aAlignment1 = Math.Cos(catheta1);
                                    a12 = new Vector2((float)((oldAcceleration.Length() * aAlignment1) * Math.Cos(phi1)), (float)((oldAcceleration.Length() * aAlignment1) * Math.Sin(phi1)));
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

                                v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                v1f = v12f + v11;

                                velocity = v1f;
                                #endregion
                            }
                            oldLocation = location;
                            oldVelocity = velocity;
                            #endregion
                            return;
                        }
                        #endregion
                        return;
                    }
                    //corners have not been hit
                    difference = -(P5.Length() * Math.Sign(P5.X));
                    break;
                #endregion
            }
            #endregion
            if (difference > Global.accuracy)
            {
                //hitting face of wall
                hittingCorner = false;
                tpivot = t;
                //the result of commenting out this while loop can be seen in test 4 (f12)
                //tpivot probably doesnt need Global.accuracy beyond 0.001f
                while (!(difference > -Global.accuracy && difference < Global.accuracy) && tpivot > Global.accuracy)
                {
                    //quicksort to t when objs first contact
                    tpivot /= 2;
                    if (difference > Global.accuracy) t -= tpivot;
                    else t += tpivot;

                    ModifiedUpdate();
                    calcWallPoints(wall);
                    #region difference
                    switch (wall.side)
                    {
                        #region RIGHT WALL
                        case Wall.SIDE_RIGHT:
                            //check for hitting corners
                            if (!(P4.X >= wall.left && P4.Y <= wall.bottom
                                && P4.Y >= wall.top && P4.X <= wall.right))
                            {
                                float diff;
                                #region check bottom corner
                                //if ball is overlapping corner but P4 is oob,
                                //then do a point collision at corner
                                diff = sRadius - Vector2.Distance(wall.bottomPoint, location);
                                if (diff > Global.accuracy)
                                {
                                    //check if it is actually hitting the face of another wall
                                    if (hittingCorner) return;
                                    hittingCorner = true;
                                    BounceOnScreenWalls();
                                    if (!hittingCorner) return;
                                    calcWallPoints(wall);

                                    hitWall = true;
                                    #region cornerCollision
                                    #region vars
                                    double phi1; //angle of collision
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

                                    #region displacement correction
                                    //see ListOfCollidables.BallCollision

                                    tpivot = t;
                                    while (tpivot > Global.accuracy
                                        && P4.Y > wall.bottom
                                        && !(diff > -Global.accuracy
                                            && diff < Global.accuracy))
                                    {   //quicksort to t when objs first contact
                                        tpivot /= 2;
                                        if (diff > 0) t -= tpivot;
                                        else t += tpivot;

                                        ModifiedUpdate();
                                        calcWallPoints(wall);
                                        diff = sRadius - Vector2.Distance(wall.bottomPoint, location);
                                    }
                                    #endregion

                                    phi1 = Math.Atan2((wall.bottomPoint.Y - location.Y), (wall.bottomPoint.X - location.X));

                                    if (diff > -Global.accuracy && diff < Global.accuracy)
                                    {
                                        #region circular collision detection

                                        axis = (float)Math.Tan(phi1);
                                        axis /= Math.Abs(axis);
                                        if (float.IsNaN(axis)) axis = -1;

                                        //ball 1
                                        vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                        cvtheta1 = phi1 - vtheta1;
                                        vAlignment1 = Math.Cos(cvtheta1);
                                        v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                        v11 = velocity - v12; //split velocity into 2 vectors
                                        if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                        else v12mag = v12.Length();

                                        v12fmag = -v12mag;

                                        v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                        v1f = v12f + v11;

                                        velocity = v1f;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region old displacement correction
                                        Vector2 dx = new Vector2((float)(diff * Math.Cos(phi1)), (float)(diff * Math.Sin(phi1)));
                                        displacement1 = -dx;
                                        location += displacement1;
                                        updateAbsLoc();
                                        #endregion
                                        #region old circular collision detection

                                        axis = (float)Math.Tan(phi1);
                                        axis /= Math.Abs(axis);
                                        if (float.IsNaN(axis)) axis = -1;

                                        //ball 1
                                        vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                        cvtheta1 = phi1 - vtheta1;
                                        vAlignment1 = Math.Cos(cvtheta1);
                                        v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                        v11 = velocity - v12; //split velocity into 2 vectors
                                        if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                        else v12mag = v12.Length();
                                        //velocity correction
                                        if (oldAcceleration.Length() > 0)
                                        {
                                            atheta1 = Math.Atan2(oldAcceleration.Y, oldAcceleration.X);
                                            catheta1 = phi1 - atheta1;
                                            aAlignment1 = Math.Cos(catheta1);
                                            a12 = new Vector2((float)((oldAcceleration.Length() * aAlignment1) * Math.Cos(phi1)), (float)((oldAcceleration.Length() * aAlignment1) * Math.Sin(phi1)));
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

                                        v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                        v1f = v12f + v11;

                                        velocity = v1f;
                                        #endregion
                                    }
                                    oldLocation = location;
                                    oldVelocity = velocity;
                                    #endregion
                                    return;
                                }
                                #endregion
                                #region check top corner
                                diff = sRadius - Vector2.Distance(wall.topPoint, location);
                                if (diff > Global.accuracy)
                                {
                                    //check if it is actually hitting the face of another wall
                                    if (hittingCorner) return;
                                    hittingCorner = true;
                                    BounceOnScreenWalls();
                                    if (!hittingCorner) return;
                                    calcWallPoints(wall);

                                    hitWall = true;
                                    #region cornerCollision
                                    #region vars
                                    double phi1; //angle of collision
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

                                    #region displacement correction
                                    //see ListOfCollidables.BallCollision

                                    tpivot = t;
                                    while (tpivot > Global.accuracy
                                        && P4.Y > wall.bottom
                                        && !(diff > -Global.accuracy
                                            && diff < Global.accuracy))
                                    {   //quicksort to t when objs first contact
                                        tpivot /= 2;
                                        if (diff > 0) t -= tpivot;
                                        else t += tpivot;

                                        ModifiedUpdate();
                                        calcWallPoints(wall);
                                        diff = sRadius - Vector2.Distance(wall.topPoint, location);
                                    }
                                    #endregion

                                    phi1 = Math.Atan2((wall.topPoint.Y - location.Y), (wall.topPoint.X - location.X));

                                    if (diff > -Global.accuracy && diff < Global.accuracy)
                                    {
                                        #region circular collision detection

                                        axis = (float)Math.Tan(phi1);
                                        axis /= Math.Abs(axis);
                                        if (float.IsNaN(axis)) axis = -1;

                                        //ball 1
                                        vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                        cvtheta1 = phi1 - vtheta1;
                                        vAlignment1 = Math.Cos(cvtheta1);
                                        v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                        v11 = velocity - v12; //split velocity into 2 vectors
                                        if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                        else v12mag = v12.Length();

                                        v12fmag = -v12mag;

                                        v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                        v1f = v12f + v11;

                                        velocity = v1f;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region old displacement correction
                                        Vector2 dx = new Vector2((float)(diff * Math.Cos(phi1)), (float)(diff * Math.Sin(phi1)));
                                        displacement1 = -dx;
                                        location += displacement1;
                                        updateAbsLoc();
                                        #endregion
                                        #region old circular collision detection

                                        axis = (float)Math.Tan(phi1);
                                        axis /= Math.Abs(axis);
                                        if (float.IsNaN(axis)) axis = -1;

                                        //ball 1
                                        vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                        cvtheta1 = phi1 - vtheta1;
                                        vAlignment1 = Math.Cos(cvtheta1);
                                        v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                        v11 = velocity - v12; //split velocity into 2 vectors
                                        if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                        else v12mag = v12.Length();
                                        //velocity correction
                                        if (oldAcceleration.Length() > 0)
                                        {
                                            atheta1 = Math.Atan2(oldAcceleration.Y, oldAcceleration.X);
                                            catheta1 = phi1 - atheta1;
                                            aAlignment1 = Math.Cos(catheta1);
                                            a12 = new Vector2((float)((oldAcceleration.Length() * aAlignment1) * Math.Cos(phi1)), (float)((oldAcceleration.Length() * aAlignment1) * Math.Sin(phi1)));
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

                                        v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                        v1f = v12f + v11;

                                        velocity = v1f;
                                        #endregion
                                    }
                                    oldLocation = location;
                                    oldVelocity = velocity;
                                    #endregion
                                    return;
                                }
                                #endregion
                                return;
                            }
                            //corners have not been hit
                            difference = P5.Length() * Math.Sign(P5.X);
                            break;
                        #endregion
                        #region LEFT WALL
                        case Wall.SIDE_LEFT:
                            //check for hitting corners
                            if (!(P4.X >= wall.left && P4.Y <= wall.bottom
                                && P4.Y >= wall.top && P4.X <= wall.right))
                            {
                                double diff;
                                #region check bottom corner
                                //when ball is overlapping corner but P4 is oob
                                diff = sRadius - Vector2.Distance(wall.bottomPoint, location);
                                if (diff > Global.accuracy)
                                {
                                    //check if it is actually hitting the face of another wall
                                    if (hittingCorner) return;
                                    hittingCorner = true;
                                    BounceOnScreenWalls();
                                    if (!hittingCorner) return;
                                    calcWallPoints(wall);

                                    hitWall = true;
                                    #region cornerCollision
                                    #region vars
                                    double phi1; //angle of collision
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

                                    #region displacement correction
                                    //see ListOfCollidables.BallCollision

                                    tpivot = t;
                                    while (tpivot > Global.accuracy
                                        && P4.Y > wall.bottom
                                        && !(diff > -Global.accuracy
                                            && diff < Global.accuracy))
                                    {   //quicksort to t when objs first contact
                                        tpivot /= 2;
                                        if (diff > 0) t -= tpivot;
                                        else t += tpivot;

                                        ModifiedUpdate();
                                        calcWallPoints(wall);
                                        diff = Math.Abs(sRadius - Vector2.Distance(wall.bottomPoint, location));
                                    }
                                    #endregion

                                    phi1 = Math.Atan2((wall.bottomPoint.Y - location.Y), (wall.bottomPoint.X - location.X));

                                    if (diff > -Global.accuracy && diff < Global.accuracy)
                                    {
                                        #region circular collision detection

                                        axis = (float)Math.Tan(phi1);
                                        axis /= Math.Abs(axis);
                                        if (float.IsNaN(axis)) axis = -1;

                                        //ball 1
                                        vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                        cvtheta1 = phi1 - vtheta1;
                                        vAlignment1 = Math.Cos(cvtheta1);
                                        v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                        v11 = velocity - v12; //split velocity into 2 vectors
                                        if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                        else v12mag = v12.Length();

                                        v12fmag = -v12mag;

                                        v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                        v1f = v12f + v11;

                                        velocity = v1f;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region old displacement correction
                                        Vector2 dx = new Vector2((float)(diff * Math.Cos(phi1)), (float)(diff * Math.Sin(phi1)));
                                        displacement1 = -dx;
                                        location += displacement1;
                                        updateAbsLoc();
                                        #endregion
                                        #region old circular collision detection

                                        axis = (float)Math.Tan(phi1);
                                        axis /= Math.Abs(axis);
                                        if (float.IsNaN(axis)) axis = -1;

                                        //ball 1
                                        vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                        cvtheta1 = phi1 - vtheta1;
                                        vAlignment1 = Math.Cos(cvtheta1);
                                        v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                        v11 = velocity - v12; //split velocity into 2 vectors
                                        if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                        else v12mag = v12.Length();
                                        //velocity correction
                                        if (oldAcceleration.Length() > 0)
                                        {
                                            atheta1 = Math.Atan2(oldAcceleration.Y, oldAcceleration.X);
                                            catheta1 = phi1 - atheta1;
                                            aAlignment1 = Math.Cos(catheta1);
                                            a12 = new Vector2((float)((oldAcceleration.Length() * aAlignment1) * Math.Cos(phi1)), (float)((oldAcceleration.Length() * aAlignment1) * Math.Sin(phi1)));
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

                                        v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                        v1f = v12f + v11;

                                        velocity = v1f;
                                        #endregion
                                    }
                                    oldLocation = location;
                                    oldVelocity = velocity;
                                    #endregion
                                    return;
                                }
                                #endregion
                                #region check top corner
                                diff = sRadius - Vector2.Distance(wall.topPoint, location);
                                if (diff > Global.accuracy)
                                {
                                    //check if it is actually hitting the face of another wall
                                    if (hittingCorner) return;
                                    hittingCorner = true;
                                    BounceOnScreenWalls();
                                    if (!hittingCorner) return;
                                    calcWallPoints(wall);

                                    hitWall = true;
                                    #region cornerCollision
                                    #region vars
                                    double phi1; //angle of collision
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

                                    #region displacement correction
                                    //see ListOfCollidables.BallCollision

                                    tpivot = t;
                                    while (tpivot > Global.accuracy
                                        && P4.Y > wall.bottom
                                        && !(diff > -Global.accuracy
                                            && diff < Global.accuracy))
                                    {   //quicksort to t when objs first contact
                                        tpivot /= 2;
                                        if (diff > 0) t -= tpivot;
                                        else t += tpivot;

                                        ModifiedUpdate();
                                        calcWallPoints(wall);
                                        diff = Math.Abs(sRadius - Vector2.Distance(wall.topPoint, location));
                                    }
                                    #endregion

                                    phi1 = Math.Atan2((wall.topPoint.Y - location.Y), (wall.topPoint.X - location.X));

                                    if (diff > -Global.accuracy && diff < Global.accuracy)
                                    {
                                        #region circular collision detection

                                        axis = (float)Math.Tan(phi1);
                                        axis /= Math.Abs(axis);
                                        if (float.IsNaN(axis)) axis = -1;

                                        //ball 1
                                        vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                        cvtheta1 = phi1 - vtheta1;
                                        vAlignment1 = Math.Cos(cvtheta1);
                                        v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                        v11 = velocity - v12; //split velocity into 2 vectors
                                        if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                        else v12mag = v12.Length();

                                        v12fmag = -v12mag;

                                        v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                        v1f = v12f + v11;

                                        velocity = v1f;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region old displacement correction
                                        Vector2 dx = new Vector2((float)(diff * Math.Cos(phi1)), (float)(diff * Math.Sin(phi1)));
                                        displacement1 = -dx;
                                        location += displacement1;
                                        updateAbsLoc();
                                        #endregion
                                        #region old circular collision detection

                                        axis = (float)Math.Tan(phi1);
                                        axis /= Math.Abs(axis);
                                        if (float.IsNaN(axis)) axis = -1;

                                        //ball 1
                                        vtheta1 = Math.Atan2(velocity.Y, velocity.X);
                                        cvtheta1 = phi1 - vtheta1;
                                        vAlignment1 = Math.Cos(cvtheta1);
                                        v12 = new Vector2((float)((velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                                        v11 = velocity - v12; //split velocity into 2 vectors
                                        if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                                        else v12mag = v12.Length();
                                        //velocity correction
                                        if (oldAcceleration.Length() > 0)
                                        {
                                            atheta1 = Math.Atan2(oldAcceleration.Y, oldAcceleration.X);
                                            catheta1 = phi1 - atheta1;
                                            aAlignment1 = Math.Cos(catheta1);
                                            a12 = new Vector2((float)((oldAcceleration.Length() * aAlignment1) * Math.Cos(phi1)), (float)((oldAcceleration.Length() * aAlignment1) * Math.Sin(phi1)));
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

                                        v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                                        v1f = v12f + v11;

                                        velocity = v1f;
                                        #endregion
                                    }
                                    oldLocation = location;
                                    oldVelocity = velocity;
                                    #endregion
                                    return;
                                }
                                #endregion
                                return;
                            }
                            //corners have not been hit
                            difference = -(P5.Length() * Math.Sign(P5.X));
                            break;
                        #endregion
                    }
                    #endregion
                }

                //rebound
                hitWall = true;
                double vtheta = Math.Atan2(velocity.Y, velocity.X);
                double alpha = 0; //new v angle
                double gamma = 0; //angle between v and wall
                float vLen = velocity.Length();
                alpha = 2 * wall.theta - vtheta;
                gamma = vtheta - wall.theta;
                velocity.X = (float)(vLen * Math.Cos(alpha));
                velocity.Y = (float)(vLen * Math.Sin(alpha));

                //if ball was already inside wall at oldLocation
                //or if the ball hits at a shallow angle
                if (difference > Global.accuracy ||
                    Math.Abs(gamma) < Global.PiOver8)
                {
                    location -= P5;
                    updateAbsLoc();
                }
            }
        }
        public void BounceOnTopBumpers()
        {
            float tpivot;
            float difference;

            #region top wall
            difference = (Global.currentGame.Table.location.Y + (-615)) - (location.Y - sRadius);
            if (difference > Global.accuracy)
            {
                tpivot = t;
                while (tpivot > Global.accuracy)
                {
                    if (!(((location.X >= Global.currentGame.Table.location.X + (-1258)) &&
                        (location.X <= Global.currentGame.Table.location.X + (-66))) ||
                        ((location.X >= Global.currentGame.Table.location.X + (64)) &&
                        (location.X <= Global.currentGame.Table.location.X + (1258)))))
                    {
                        //out of bounds
                        return;
                    }

                    //quicksort to t when objs first contact
                    tpivot /= 2;
                    if (difference > Global.accuracy) t -= tpivot;
                    else t += tpivot;

                    ModifiedUpdate();

                    difference = (Global.currentGame.Table.location.Y + (-615)) - (location.Y - sRadius);
                }
                if (difference > Global.accuracy) //if ball was already inside wall at oldLocation
                {
                    location.Y = Global.currentGame.Table.location.Y + (-615) + sRadius;
                    updateAbsLoc();
                }
                velocity.Y = Math.Abs(velocity.Y);
                oldLocation = location;
                oldVelocity = velocity;
                hitWall = true;
                //hitting face of wall
                hittingCorner = false;
            }
            #endregion
        }
        public void BounceOnBottomBumpers()
        {
            float tpivot;
            float difference;

            #region bottom wall
            difference = (location.Y + sRadius) - (Global.currentGame.Table.location.Y + (615));
            if (difference > Global.accuracy)
            {
                tpivot = t;
                while (tpivot > Global.accuracy)
                {
                    if (!(((location.X >= Global.currentGame.Table.location.X + (-1260)) &&
                        (location.X <= Global.currentGame.Table.location.X + (-66))) ||
                        ((location.X >= Global.currentGame.Table.location.X + (64)) &&
                        (location.X <= Global.currentGame.Table.location.X + (1260)))))
                    {
                        //out of bounds
                        return;
                    }

                    //quicksort to t when objs first contact
                    tpivot /= 2;
                    if (difference > Global.accuracy) t -= tpivot;
                    else t += tpivot;

                    ModifiedUpdate();

                    difference = (location.Y + sRadius) - (Global.currentGame.Table.location.Y + (615));
                }
                if (difference > Global.accuracy) //if ball was already inside wall at oldLocation
                {
                    location.Y = Global.currentGame.Table.location.Y + (615) - sRadius;
                    updateAbsLoc();
                }
                velocity.Y = Math.Abs(velocity.Y) * -1;
                oldLocation = location;
                oldVelocity = velocity;
                hitWall = true;
                //hitting face of wall
                hittingCorner = false;
            }
            #endregion
        }
        public void BounceOnLeftBumper()
        {
            float tpivot;
            float difference;

            #region left wall
            difference = (Global.currentGame.Table.location.X + (-1325)) - (location.X - sRadius);
            if (difference > Global.accuracy)
            {
                tpivot = t;
                while (tpivot > Global.accuracy)
                {
                    if (!((location.Y >= Global.currentGame.Table.location.Y + (-550)) &&
                        (location.Y <= Global.currentGame.Table.location.Y + (550))))
                    {
                        //out of bounds
                        return;
                    }

                    //quicksort to t when objs first contact
                    tpivot /= 2;
                    if (difference > Global.accuracy) t -= tpivot;
                    else t += tpivot;

                    ModifiedUpdate();

                    difference = (Global.currentGame.Table.location.X + (-1325)) - (location.X - sRadius);
                }
                if (difference > Global.accuracy) //if ball was already inside wall at oldLocation
                {
                    location.X = Global.currentGame.Table.location.X + (-1325) + sRadius;
                    updateAbsLoc();
                }
                velocity.X = Math.Abs(velocity.X);
                oldLocation = location;
                oldVelocity = velocity;
                hitWall = true;
                //hitting face of wall
                hittingCorner = false;
            }
            #endregion
        }
        public void BounceOnRightBumper()
        {
            float tpivot;
            float difference;

            #region right wall
            difference = (location.X + sRadius) - (Global.currentGame.Table.location.X + (1325));
            if (difference > Global.accuracy)
            {
                tpivot = t;
                while (tpivot > Global.accuracy)
                {
                    if (!((location.Y >= Global.currentGame.Table.location.Y + (-550)) &&
                        (location.Y <= Global.currentGame.Table.location.Y + (550))))
                    {
                        //out of bounds
                        //this happens when the object goes around the wall instead of through it
                        return;
                    }

                    //quicksort to t when objs first contact
                    tpivot /= 2;
                    if (difference > Global.accuracy) t -= tpivot;
                    else t += tpivot;

                    ModifiedUpdate();

                    difference = (location.X + sRadius) - (Global.currentGame.Table.location.X + (1325));
                }
                if (difference > Global.accuracy) //if ball was already inside wall at oldLocation
                {
                    location.X = Global.currentGame.Table.location.X + (1325) - sRadius;
                    updateAbsLoc();
                }
                velocity.X = Math.Abs(velocity.X) * -1;
                oldLocation = location;
                oldVelocity = velocity;
                hitWall = true;
                //hitting face of wall
                hittingCorner = false;
            }
            #endregion
        }
        public void ModifiedUpdate()
        {
            velocity = oldVelocity + oldAcceleration * t;
            velocity *= friction;
            if (velocity.Length() < fCritVel) velocity = Vector2.Zero; //static friction
            location = oldLocation + velocity * t;
            updateAbsLoc();
        }
        public void updateAbsLoc()
        {
            if (parent == null)
            {
                absoluteLocation = location;
            }
            else
            {
                absoluteLocation = location * parent.scale + parent.absoluteLocation;
            }
        }
        public virtual void reset()
        {
            location = Vector2.Zero;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
