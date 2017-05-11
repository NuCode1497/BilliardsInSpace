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
    public class ListOfCollidables : ListOfScreenObjects
    {
        public const int STATE_TEST_1 = 3;
        public const int STATE_TEST_2 = 4;
        
        public static float G = (6.673f * (float)Math.Pow(10, -11)); //universal gravitational constant

        public override void Setup()
        {
            base.Setup();
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            int last;
            switch (state)
            {
                case STATE_TEST_1:
                    last = ScreenObjectList.Count - 1;
                    for (int i = 0; i < last; i++)
                    {
                        if (ScreenObjectList[i].state > 0)
                        {
                            for (int j = i + 1; j < ScreenObjectList.Count; j++)
                            {
                                if (ScreenObjectList[j].state > 0)
                                {
                                    TEST_1_BallCollision(ScreenObjectList[i], ScreenObjectList[j]);
                                }
                            }
                        }
                    }

                    base.Update(gameTime);
                    break;
                case STATE_TEST_2:
                    last = ScreenObjectList.Count - 1;
                    for (int i = 0; i < last; i++)
                    {
                        if (ScreenObjectList[i].state > 0)
                        {
                            for (int j = i + 1; j < ScreenObjectList.Count; j++)
                            {
                                if (ScreenObjectList[j].state > 0)
                                {
                                    TEST_2_BallCollision(ScreenObjectList[i], ScreenObjectList[j]);
                                }
                            }
                        }
                    }

                    base.Update(gameTime);
                    break;
                case STATE_ACTIVE:
                    last = ScreenObjectList.Count - 1;
                    for (int i = 0; i < last; i++)
                    {
                        if (ScreenObjectList[i].state > 0)
                        {
                            switch (ScreenObjectList[i].collideType)
                            {
                                case COLLIDE_TYPE_NONE:
                                    break;
                                case COLLIDE_TYPE_BALL:
                                    for (int j = i + 1; j < ScreenObjectList.Count; j++)
                                    {
                                        if (ScreenObjectList[j].state > 0)
                                        {
                                            switch (ScreenObjectList[j].collideType)
                                            {
                                                case COLLIDE_TYPE_NONE:
                                                    break;
                                                case COLLIDE_TYPE_BALL:
                                                    BallCollision(ScreenObjectList[i], ScreenObjectList[j]);
                                                    break;
                                                case COLLIDE_TYPE_MAN:
                                                    ManCollision(ScreenObjectList[j], ScreenObjectList[i]);
                                                    break;
                                                case COLLIDE_TYPE_WALL:
                                                    //WallCollision(i, j);
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                                case COLLIDE_TYPE_MAN:
                                    for (int j = i + 1; j < ScreenObjectList.Count; j++)
                                    {
                                        if (ScreenObjectList[j].state > 0)
                                        {
                                            ManCollision(ScreenObjectList[i], ScreenObjectList[j]);
                                        }
                                    }
                                    break;
                                case COLLIDE_TYPE_WALL:
                                    break;
                            }
                        }
                    }

                    base.Update(gameTime);
                    break;
            }
        }
        private void BallCollision(ScreenObject ball1, ScreenObject ball2)
        {
            #region vars

            float accuracy = Global.currentGame.TEST_accuracy;
            float tpivot1;
            float tpivot2;
            double difference;
            float d; //distance between
            double phi1; //angle of collision
            Vector2 Fg12 = Vector2.Zero; //gravitational force by particle 1 on particle 2
            Vector2 Fg21 = Vector2.Zero;
            double dmin; //r1 + r2
            float masses;
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
            //ball 2
            Vector2 displacement2 = Vector2.Zero;
            Vector2 v22; //velocity on collision axis
            Vector2 a22; //acceleration on collision axis
            double v22mag;
            Vector2 v21; //velocity not on collision axis
            double v22fmag; //result of 1d collision along axis of collision
            Vector2 v22f;
            Vector2 v2f;
            double vAlignment2; //alignment of collision to velocity
            double cvtheta2;
            double phi2; //angle of collision
            double vtheta2; //angle of velocity

            double atheta1;
            double catheta1;
            double aAlignment1;
            double a12mag;
            double a22mag;
            double atheta2;
            double catheta2;
            double aAlignment2;

            #endregion

            //if bullet doesnt hit cue, return
            //if (ball1 is Bullet && ball2 is BilliardBall)
            //    if (((BilliardBall)ball2).type != BilliardBall.TYPE_CUE) return;
            //else if (ball2 is Bullet && ball1 is BilliardBall)
            //    if (((BilliardBall)ball1).type != BilliardBall.TYPE_CUE) return;

            d = Vector2.Distance(ball2.location, ball1.location);
            dmin = ball1.sRadius + ball2.sRadius;
            difference = dmin - d;

            if (difference > accuracy)
            {
                masses = ball1.mass + ball2.mass;

                if (d < dmin)
                {
                    ball1.hit = true;
                    ball2.hit = true;
                }

                #region displacement correction
                //Because the field is updated discretely, collisions will always overshoot and need corrections 
                //correction of location
                //note that with an acceleration, velocity should be corrected as well
                //acceleration will be corrected with gravity section
                //will need recursion here to check for extra collisions after corrections

                tpivot1 = ball1.t;
                tpivot2 = ball2.t;
                //the justification of this while loop is demonstrated in test 1 and 2
                while (!(difference > -accuracy && difference < accuracy) && tpivot1 > accuracy && tpivot2 > accuracy)
                {   //quicksort to t when objs first contact
                    tpivot1 /= 2;
                    tpivot2 /= 2;
                    if (difference > accuracy)
                    {
                        ball1.t -= tpivot1;
                        ball2.t -= tpivot2;
                    }
                    else
                    {
                        ball1.t += tpivot1;
                        ball2.t += tpivot2;
                    }

                    //replace this with modified update from BilliardBall.TEST_BounceOnScreenWalls
                    ball1.ModifiedUpdate();
                    ball2.ModifiedUpdate();

                    d = Vector2.Distance(ball2.location, ball1.location);

                    difference = dmin - d;
                }

                phi1 = Math.Atan2((ball2.location.Y - ball1.location.Y), (ball2.location.X - ball1.location.X));

                #endregion

                if (Math.Abs(difference) < accuracy)
                {
                    #region circular collision detection

                    axis = (float)Math.Tan(phi1);
                    axis /= Math.Abs(axis);
                    if (float.IsNaN(axis)) axis = -1;

                    //ball 1
                    vtheta1 = Math.Atan2(ball1.velocity.Y, ball1.velocity.X);
                    cvtheta1 = phi1 - vtheta1;
                    vAlignment1 = Math.Cos(cvtheta1);
                    v12 = new Vector2((float)((ball1.velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((ball1.velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                    v11 = ball1.velocity - v12; //split velocity into 2 vectors
                    if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                    else v12mag = v12.Length();

                    //ball 2
                    phi2 = phi1 + Math.PI;
                    vtheta2 = Math.Atan2(ball2.velocity.Y, ball2.velocity.X);
                    cvtheta2 = phi2 - vtheta2;
                    vAlignment2 = Math.Cos(cvtheta2);
                    v22 = new Vector2((float)((ball2.velocity.Length() * vAlignment2) * Math.Cos(phi2)), (float)((ball2.velocity.Length() * vAlignment2) * Math.Sin(phi2)));
                    v21 = ball2.velocity - v22; //split velocity into 2 vectors
                    if (v22.X < 0) v22mag = v22.Length() * -1; //flatten along x axis
                    else v22mag = v22.Length();

                    //conservation of momentum
                    if (ball1.mass == ball2.mass || masses == 0)
                    {
                        v12fmag = v22mag;
                        v22fmag = v12mag;
                    }
                    else
                    {
                        v12fmag = ((ball1.mass - ball2.mass) / masses) * v12mag + ((2 * ball2.mass) / masses) * v22mag; //1d collision
                        v22fmag = ((ball1.mass * 2) / masses) * v12mag + ((ball2.mass - ball1.mass) / masses) * v22mag;
                    }
                    v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                    v22f = new Vector2((float)(v22fmag * Math.Abs(Math.Cos(phi2))), (float)(v22fmag * axis * Math.Abs(Math.Sin(phi2))));
                    v1f = v12f + v11;
                    v2f = v22f + v21;

                    ball1.velocity = v1f;
                    ball2.velocity = v2f;

                    #endregion
                }

                //need to use old code if balls bounce into another in one frame
                //can be removed after recursion is implemented
                else
                {
                    #region old displacement correction
                    //Because the field is updated discretely, collisions will always overshoot and need corrections 
                    //correction of location
                    //note that with an acceleration, velocity should be corrected as well
                    //acceleration will be corrected with gravity section
                    difference = dmin;
                    difference -= d;
                    Vector2 dx = new Vector2((float)(difference * Math.Cos(phi1)), (float)(difference * Math.Sin(phi1)));
                    if (masses == 0)
                    {
                        displacement2 = dx / 2;
                        displacement1 = -displacement2;
                        ball2.location += displacement2;
                        ball1.location += displacement1;
                        ball2.updateAbsLoc();
                        ball1.updateAbsLoc();
                    }
                    else
                    {
                        displacement2 = dx * (ball1.mass / masses);
                        displacement1 = -dx * (ball2.mass / masses);
                        ball2.location += displacement2;
                        ball1.location += displacement1;
                        ball2.updateAbsLoc();
                        ball1.updateAbsLoc();
                    }
                    d = Vector2.Distance(ball2.location, ball1.location);
                    #endregion

                    #region old circular collision detection

                    axis = (float)Math.Tan(phi1);
                    axis /= Math.Abs(axis);
                    if (float.IsNaN(axis)) axis = -1;

                    //ball 1
                    vtheta1 = Math.Atan2(ball1.velocity.Y, ball1.velocity.X);
                    cvtheta1 = phi1 - vtheta1;
                    vAlignment1 = Math.Cos(cvtheta1);
                    v12 = new Vector2((float)((ball1.velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((ball1.velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                    v11 = ball1.velocity - v12; //split velocity into 2 vectors
                    if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                    else v12mag = v12.Length();
                    //velocity correction
                    if (ball1.oldAcceleration.Length() > 0)
                    {
                        atheta1 = Math.Atan2(ball1.oldAcceleration.Y, ball1.oldAcceleration.X);
                        catheta1 = phi1 - atheta1;
                        aAlignment1 = Math.Cos(catheta1);
                        a12 = new Vector2((float)((ball1.oldAcceleration.Length() * aAlignment1) * Math.Cos(phi1)), (float)((ball1.oldAcceleration.Length() * aAlignment1) * Math.Sin(phi1)));
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

                    //ball 2
                    phi2 = phi1 + Math.PI;
                    vtheta2 = Math.Atan2(ball2.velocity.Y, ball2.velocity.X);
                    cvtheta2 = phi2 - vtheta2;
                    vAlignment2 = Math.Cos(cvtheta2);
                    v22 = new Vector2((float)((ball2.velocity.Length() * vAlignment2) * Math.Cos(phi2)), (float)((ball2.velocity.Length() * vAlignment2) * Math.Sin(phi2)));
                    v21 = ball2.velocity - v22; //split velocity into 2 vectors
                    if (v22.X < 0) v22mag = v22.Length() * -1; //flatten along x axis
                    else v22mag = v22.Length();
                    //velocity correction
                    if (ball2.oldAcceleration.Length() > 0)
                    {
                        atheta2 = Math.Atan2(ball2.oldAcceleration.Y, ball2.oldAcceleration.X);
                        catheta2 = phi2 - atheta2;
                        aAlignment2 = Math.Cos(catheta2);
                        a22 = new Vector2((float)((ball2.oldAcceleration.Length() * aAlignment2) * Math.Cos(phi2)), (float)((ball2.oldAcceleration.Length() * aAlignment2) * Math.Sin(phi2)));
                        if (a22.X < 0) a22mag = a22.Length() * -1;
                        else a22mag = a22.Length();
                        if (v22mag < 0)
                        {
                            if (displacement2.X < 0) v22mag = -(Math.Sqrt((v22mag * v22mag) - 2 * a22mag * displacement2.Length())); //Vxf^2 = Vxi^2 + 2 * a * (xf - xi)
                            else v22mag = -(Math.Sqrt((v22mag * v22mag) + 2 * a22mag * displacement2.Length()));
                        }
                        else
                        {
                            if (displacement2.X < 0) v22mag = Math.Sqrt((v22mag * v22mag) - 2 * a22mag * displacement2.Length()); //Vxf^2 = Vxi^2 + 2 * a * (xf - xi)
                            else v22mag = Math.Sqrt((v22mag * v22mag) + 2 * a22mag * displacement2.Length());
                        }
                        if (double.IsNaN(v22mag)) v22mag = 0;
                    }

                    //conservation of momentum
                    if (ball1.mass == ball2.mass || masses == 0)
                    {
                        v12fmag = v22mag;
                        v22fmag = v12mag;
                    }
                    else
                    {
                        v12fmag = ((ball1.mass - ball2.mass) / masses) * v12mag + ((2 * ball2.mass) / masses) * v22mag; //1d collision
                        v22fmag = ((ball1.mass * 2) / masses) * v12mag + ((ball2.mass - ball1.mass) / masses) * v22mag;
                    }
                    v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                    v22f = new Vector2((float)(v22fmag * Math.Abs(Math.Cos(phi2))), (float)(v22fmag * axis * Math.Abs(Math.Sin(phi2))));
                    v1f = v12f + v11;
                    v2f = v22f + v21;

                    ball1.velocity = v1f;
                    ball2.velocity = v2f;

                    #endregion
                }

                #region friction between particles

                #endregion

                ball1.oldLocation = ball1.location;
                ball1.oldVelocity = ball1.velocity;
                ball2.oldLocation = ball2.location;
                ball2.oldVelocity = ball2.velocity;
            }
            else
            {
                phi1 = Math.Atan2((ball2.location.Y - ball1.location.Y), (ball2.location.X - ball1.location.X));
            }

            //Gravity(ball1, ball2, d, phi1);
        }
        private void TEST_1_BallCollision(ScreenObject ball1, ScreenObject ball2)
        {
            #region vars
            float d; //distance between
            double phi1; //angle of collision
            Vector2 Fg12 = Vector2.Zero; //gravitational force by particle 1 on particle 2
            Vector2 Fg21 = Vector2.Zero;
            double dmin; //r1 + r2
            float masses;
            float axis; //flag telling collision axis points NESW or NWSE 
            //ball 1
            Vector2 displacement1 = Vector2.Zero;
            Vector2 v12; //velocity on collision axis
            Vector2 a12; //acceleration on collision axis
            double v12mag;
            double a12mag;
            Vector2 v11; //velocity not on collision axis
            double v12fmag; //result of 1d collision along axis of collision
            Vector2 v12f;
            Vector2 v1f;
            double vAlignment1; //alignment of collision to velocity
            double aAlignment1; //alignment of collision to acceleration
            double cvtheta1; //angle between collision and velocity
            double catheta1; //angle between collision and acceleration
            double vtheta1; //angle of velocity
            double atheta1; //angle of acceleration
            //ball 2
            Vector2 displacement2 = Vector2.Zero;
            Vector2 v22; //velocity on collision axis
            Vector2 a22; //acceleration on collision axis
            double v22mag;
            double a22mag;
            Vector2 v21; //velocity not on collision axis
            double v22fmag; //result of 1d collision along axis of collision
            Vector2 v22f;
            Vector2 v2f;
            double vAlignment2; //alignment of collision to velocity
            double aAlignment2; //alignment of collision to acceleration
            double cvtheta2;
            double catheta2; //angle between collision and acceleration
            double phi2; //angle of collision
            double vtheta2; //angle of velocity
            double atheta2; //angle of acceleration
            #endregion

            d = Vector2.Distance(ball2.location, ball1.location);
            dmin = ball1.sRadius + ball2.sRadius;
            phi1 = Math.Atan2((ball2.location.Y - ball1.location.Y), (ball2.location.X - ball1.location.X));

            if (d <= dmin)
            {
                masses = ball1.mass + ball2.mass;

                if (d < dmin)
                {
                    ball1.hit = true;
                    ball2.hit = true;
                }

                #region displacement correction
                //Because the field is updated discretely, collisions will always overshoot and need corrections 
                //correction of location
                //note that with an acceleration, velocity should be corrected as well
                //acceleration will be corrected with gravity section
                double difference = dmin;
                difference -= d;
                Vector2 dx = new Vector2((float)(difference * Math.Cos(phi1)), (float)(difference * Math.Sin(phi1)));
                if (masses == 0)
                {
                    displacement2 = dx / 2;
                    displacement1 = -displacement2;
                    ball2.location += displacement2;
                    ball1.location += displacement1;
                }
                else
                {
                    displacement2 = dx * (ball1.mass / masses);
                    displacement1 = -dx * (ball2.mass / masses);
                    ball2.location += displacement2;
                    ball1.location += displacement1;
                }
                d = Vector2.Distance(ball2.location, ball1.location);
                #endregion

                #region circular collision detection

                axis = (float)Math.Tan(phi1);
                axis /= Math.Abs(axis);
                if (float.IsNaN(axis)) axis = -1;

                //ball 1
                vtheta1 = Math.Atan2(ball1.velocity.Y, ball1.velocity.X);
                cvtheta1 = phi1 - vtheta1;
                vAlignment1 = Math.Cos(cvtheta1);
                v12 = new Vector2((float)((ball1.velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((ball1.velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                v11 = ball1.velocity - v12; //split velocity into 2 vectors
                if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                else v12mag = v12.Length();
                //velocity correction
                if (ball1.oldAcceleration.Length() > 0)
                {
                    atheta1 = Math.Atan2(ball1.oldAcceleration.Y, ball1.oldAcceleration.X);
                    catheta1 = phi1 - atheta1;
                    aAlignment1 = Math.Cos(catheta1);
                    a12 = new Vector2((float)((ball1.oldAcceleration.Length() * aAlignment1) * Math.Cos(phi1)), (float)((ball1.oldAcceleration.Length() * aAlignment1) * Math.Sin(phi1)));
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

                //ball 2
                phi2 = phi1 + Math.PI;
                vtheta2 = Math.Atan2(ball2.velocity.Y, ball2.velocity.X);
                cvtheta2 = phi2 - vtheta2;
                vAlignment2 = Math.Cos(cvtheta2);
                v22 = new Vector2((float)((ball2.velocity.Length() * vAlignment2) * Math.Cos(phi2)), (float)((ball2.velocity.Length() * vAlignment2) * Math.Sin(phi2)));
                v21 = ball2.velocity - v22; //split velocity into 2 vectors
                if (v22.X < 0) v22mag = v22.Length() * -1; //flatten along x axis
                else v22mag = v22.Length();
                //velocity correction
                if (ball2.oldAcceleration.Length() > 0)
                {
                    atheta2 = Math.Atan2(ball2.oldAcceleration.Y, ball2.oldAcceleration.X);
                    catheta2 = phi2 - atheta2;
                    aAlignment2 = Math.Cos(catheta2);
                    a22 = new Vector2((float)((ball2.oldAcceleration.Length() * aAlignment2) * Math.Cos(phi2)), (float)((ball2.oldAcceleration.Length() * aAlignment2) * Math.Sin(phi2)));
                    if (a22.X < 0) a22mag = a22.Length() * -1;
                    else a22mag = a22.Length();
                    if (v22mag < 0)
                    {
                        if (displacement2.X < 0) v22mag = -(Math.Sqrt((v22mag * v22mag) - 2 * a22mag * displacement2.Length())); //Vxf^2 = Vxi^2 + 2 * a * (xf - xi)
                        else v22mag = -(Math.Sqrt((v22mag * v22mag) + 2 * a22mag * displacement2.Length()));
                    }
                    else
                    {
                        if (displacement2.X < 0) v22mag = Math.Sqrt((v22mag * v22mag) - 2 * a22mag * displacement2.Length()); //Vxf^2 = Vxi^2 + 2 * a * (xf - xi)
                        else v22mag = Math.Sqrt((v22mag * v22mag) + 2 * a22mag * displacement2.Length());
                    }
                    if (double.IsNaN(v22mag)) v22mag = 0;
                }

                //conservation of momentum
                if (ball1.mass == ball2.mass || masses == 0)
                {
                    v12fmag = v22mag;
                    v22fmag = v12mag;
                }
                else
                {
                    v12fmag = ((ball1.mass - ball2.mass) / masses) * v12mag + ((2 * ball2.mass) / masses) * v22mag; //1d collision
                    v22fmag = ((ball1.mass * 2) / masses) * v12mag + ((ball2.mass - ball1.mass) / masses) * v22mag;
                }
                v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                v22f = new Vector2((float)(v22fmag * Math.Abs(Math.Cos(phi2))), (float)(v22fmag * axis * Math.Abs(Math.Sin(phi2))));
                v1f = v12f + v11;
                v2f = v22f + v21;

                ball1.velocity = v1f;
                ball2.velocity = v2f;

                #endregion

                #region friction between particles

                #endregion
            }

            //Gravity(ball1, ball2, d, phi1);
        }
        private void TEST_2_BallCollision(ScreenObject ball1, ScreenObject ball2)
        {
            #region vars
            float t = 1; //time in this frame 0 < t < 1
            float tpivot;
            double difference;
            float d; //distance between
            double phi1; //angle of collision
            Vector2 Fg12 = Vector2.Zero; //gravitational force by particle 1 on particle 2
            Vector2 Fg21 = Vector2.Zero;
            double dmin; //r1 + r2
            float masses;
            float axis; //flag telling collision axis points NESW or NWSE 
            //ball 1
            Vector2 displacement1 = Vector2.Zero;
            Vector2 v12; //velocity on collision axis
            double v12mag;
            Vector2 v11; //velocity not on collision axis
            double v12fmag; //result of 1d collision along axis of collision
            Vector2 v12f;
            Vector2 v1f;
            double vAlignment1; //alignment of collision to velocity
            double cvtheta1; //angle between collision and velocity
            double vtheta1; //angle of velocity
            //ball 2
            Vector2 displacement2 = Vector2.Zero;
            Vector2 v22; //velocity on collision axis
            double v22mag;
            Vector2 v21; //velocity not on collision axis
            double v22fmag; //result of 1d collision along axis of collision
            Vector2 v22f;
            Vector2 v2f;
            double vAlignment2; //alignment of collision to velocity
            double cvtheta2;
            double phi2; //angle of collision
            double vtheta2; //angle of velocity
            #endregion

            d = Vector2.Distance(ball2.location, ball1.location);
            dmin = ball1.sRadius + ball2.sRadius;

            if (d <= dmin)
            {
                masses = ball1.mass + ball2.mass;

                if (d < dmin)
                {
                    ball1.hit = true;
                    ball2.hit = true;
                }

                #region displacement correction
                //Because the field is updated discretely, collisions will always overshoot and need corrections 
                //correction of location
                //note that with an acceleration, velocity should be corrected as well
                //acceleration will be corrected with gravity section
                difference = dmin;
                difference -= d;

                t = 1f;
                tpivot = 1f;
                while (tpivot > Global.currentGame.TEST_accuracy)
                {   //quicksort to t when objs first contact
                    tpivot /= 2;
                    if (difference > 0) t -=  tpivot;
                    else t += tpivot;

                    ball1.velocity = ball1.oldVelocity + ball1.oldAcceleration * t;
                    ball1.location = ball1.oldLocation + ball1.velocity * t;

                    ball2.velocity = ball2.oldVelocity + ball2.oldAcceleration * t;
                    ball2.location = ball2.oldLocation + ball2.velocity * t;

                    d = Vector2.Distance(ball2.location, ball1.location);

                    difference = dmin - d;
                }

                phi1 = Math.Atan2((ball2.location.Y - ball1.location.Y), (ball2.location.X - ball1.location.X));

                #endregion


                #region circular collision detection

                axis = (float)Math.Tan(phi1);
                axis /= Math.Abs(axis);
                if (float.IsNaN(axis)) axis = -1;

                //ball 1
                vtheta1 = Math.Atan2(ball1.velocity.Y, ball1.velocity.X);
                cvtheta1 = phi1 - vtheta1;
                vAlignment1 = Math.Cos(cvtheta1);
                v12 = new Vector2((float)((ball1.velocity.Length() * vAlignment1) * Math.Cos(phi1)), (float)((ball1.velocity.Length() * vAlignment1) * Math.Sin(phi1)));
                v11 = ball1.velocity - v12; //split velocity into 2 vectors
                if (v12.X < 0) v12mag = v12.Length() * -1; //flatten along x axis
                else v12mag = v12.Length();

                //ball 2
                phi2 = phi1 + Math.PI;
                vtheta2 = Math.Atan2(ball2.velocity.Y, ball2.velocity.X);
                cvtheta2 = phi2 - vtheta2;
                vAlignment2 = Math.Cos(cvtheta2);
                v22 = new Vector2((float)((ball2.velocity.Length() * vAlignment2) * Math.Cos(phi2)), (float)((ball2.velocity.Length() * vAlignment2) * Math.Sin(phi2)));
                v21 = ball2.velocity - v22; //split velocity into 2 vectors
                if (v22.X < 0) v22mag = v22.Length() * -1; //flatten along x axis
                else v22mag = v22.Length();

                //conservation of momentum
                if (ball1.mass == ball2.mass || masses == 0)
                {
                    v12fmag = v22mag;
                    v22fmag = v12mag;
                }
                else
                {
                    v12fmag = ((ball1.mass - ball2.mass) / masses) * v12mag + ((2 * ball2.mass) / masses) * v22mag; //1d collision
                    v22fmag = ((ball1.mass * 2) / masses) * v12mag + ((ball2.mass - ball1.mass) / masses) * v22mag;
                }
                v12f = new Vector2((float)(v12fmag * Math.Abs(Math.Cos(phi1))), (float)(v12fmag * axis * Math.Abs(Math.Sin(phi1)))); //unflatten
                v22f = new Vector2((float)(v22fmag * Math.Abs(Math.Cos(phi2))), (float)(v22fmag * axis * Math.Abs(Math.Sin(phi2))));
                v1f = v12f + v11;
                v2f = v22f + v21;

                ball1.velocity = v1f;
                ball2.velocity = v2f;

                #endregion

                #region friction between particles

                #endregion
            }
            else
            {
                phi1 = Math.Atan2((ball2.location.Y - ball1.location.Y), (ball2.location.X - ball1.location.X));
            }

            //Gravity(ball1, ball2, d, phi1);
        }
        private void ManCollision(ScreenObject man, ScreenObject ball2)
        {
            #region vars
            float d; //distance between
            double phi1; //angle of collision
            double dmin; //r1 + r2
            double difference; //move ball 2 this much outside of ball 1 after collision
            #endregion

            d = Vector2.Distance(ball2.location, man.location);
            dmin = man.radius + ball2.sRadius;
            phi1 = Math.Atan2((ball2.location.Y - man.location.Y), (ball2.location.X - man.location.X));

            if (d <= dmin)
            {
                man.hit = true;
                #region displacement correction
                difference = dmin;
                difference -= d;
                Vector2 dx = new Vector2((float)(difference * Math.Cos(phi1)), (float)(difference * Math.Sin(phi1)));
                man.location -= dx;
                #endregion

                man.oldLocation = man.location;
            }
        }
        private void Gravity(ScreenObject ball1, ScreenObject ball2, float d, double phi1)
        {
            double Fg12mag;
            Vector2 Fg12 = Vector2.Zero; //gravitational force by particle 1 on particle 2
            Vector2 Fg21 = Vector2.Zero;

            if (d != 0)
            {
                //universal gravitation equation
                Fg12mag = -G * ((ball1.mass * ball2.mass) / (d * d));
                Fg12.X = (float)(Fg12mag * Math.Cos(phi1));
                Fg12.Y = (float)(Fg12mag * Math.Sin(phi1));
                Fg21 = -Fg12;
                if (Fg12.Length() != 0) ball2.acceleration += Fg12 / ball2.mass;
                if (Fg21.Length() != 0) ball1.acceleration += Fg21 / ball1.mass;
            }
        }
        public virtual void PlaceOnTable()
        {
            foreach (ScreenObject so in ScreenObjectList)
            {
                so.reset();
            }
            scale = (float)(30.75 / 137) * Global.currentGame.Table.scale;
            Global.pocketedSolids = 0;
            Global.pocketedStripes = 0;
        }
        public virtual void RackBalls()
        {
            Global.pocketedSolids = 0;
            Global.pocketedStripes = 0;

            Global.currentGame.cueball.location = new Vector2(711, 711) + Global.currentTable.topLeftCorner;
            Global.currentGame.ball8.location = new Vector2(2241, 711) + Global.currentTable.topLeftCorner;
            Global.currentGame.ball1.location = new Vector2(2131, 711) + Global.currentTable.topLeftCorner;
            Global.currentGame.ball2.location = new Vector2(2186, 680) + Global.currentTable.topLeftCorner;
            Global.currentGame.ball3.location = new Vector2(2241, 773) + Global.currentTable.topLeftCorner;
            Global.currentGame.ball4.location = new Vector2(2296, 618) + Global.currentTable.topLeftCorner;
            Global.currentGame.ball5.location = new Vector2(2296, 742) + Global.currentTable.topLeftCorner;
            Global.currentGame.ball6.location = new Vector2(2351, 649) + Global.currentTable.topLeftCorner;
            Global.currentGame.ball7.location = new Vector2(2351, 835) + Global.currentTable.topLeftCorner;
            Global.currentGame.ball9.location = new Vector2(2186, 742) + Global.currentTable.topLeftCorner;
            Global.currentGame.ball10.location = new Vector2(2241, 649) + Global.currentTable.topLeftCorner;
            Global.currentGame.ball11.location = new Vector2(2296, 680) + Global.currentTable.topLeftCorner;
            Global.currentGame.ball12.location = new Vector2(2296, 804) + Global.currentTable.topLeftCorner;
            Global.currentGame.ball13.location = new Vector2(2351, 587) + Global.currentTable.topLeftCorner;
            Global.currentGame.ball14.location = new Vector2(2351, 711) + Global.currentTable.topLeftCorner;
            Global.currentGame.ball15.location = new Vector2(2351, 773) + Global.currentTable.topLeftCorner;

            //List<BilliardBall> balls = new List<BilliardBall>();
            //balls.Add(Global.currentGame.ball1);
            //balls.Add(Global.currentGame.ball2);
            //balls.Add(Global.currentGame.ball3);
            //balls.Add(Global.currentGame.ball4);
            //balls.Add(Global.currentGame.ball5);
            //balls.Add(Global.currentGame.ball6);
            //balls.Add(Global.currentGame.ball7);
            //balls.Add(Global.currentGame.ball9);
            //balls.Add(Global.currentGame.ball10);
            //balls.Add(Global.currentGame.ball11);
            //balls.Add(Global.currentGame.ball12);
            //balls.Add(Global.currentGame.ball13);
            //balls.Add(Global.currentGame.ball14);
            //balls.Add(Global.currentGame.ball15);
            //balls.Add(Global.currentGame.ball8);
            //balls.Add(Global.currentGame.cueball);
            //foreach (BilliardBall ball in balls)
            //{
            //    bool occupied = false;
            //    do
            //    {
            //        ball.location = Global.findRackSpot(ball.type);
            //        foreach (BilliardBall b in balls)
            //        {
            //            if (Vector2.Distance(ball.location, b.location) < 10)
            //            {
            //                occupied = true;
            //                break;
            //            }
            //        }
            //        occupied = false;
            //    } while (occupied);
            //}

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (state)
            {
                case STATE_INACTIVE:
                    break;
                case STATE_TEST_1:
                case STATE_TEST_2:
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