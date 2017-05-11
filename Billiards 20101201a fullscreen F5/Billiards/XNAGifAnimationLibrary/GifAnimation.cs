/*
	                  XNA Gif Animation Library
    Copyright (C) 2007 Mahdi Khodadadi Fard (mahdi3466@yahoo.com)

    License:
    Permission is hereby granted, free of charge, to any person obtaining a copy of
    this software and associated documentation files (the "Software"), to deal in
    the Software without restriction, including without limitation the rights to
    use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
    the Software, and to permit persons to whom the Software is furnished to do so,
    subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software or in "About" menu.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
    FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
    COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
    IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
    CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace XNAGifAnimationLibrary
{
    public class GifAnimation
    {
        private Texture2D[] m_Textures = null;
        private int m_CurrentFrame = 0;
        private long m_CurrentTick = 0;

        private bool m_IsPaused = false;
        private bool m_IsStopped = false;


        private GifAnimation()
        {
        }

        public static GifAnimation FromTextures(Texture2D[] frames)
        {
            GifAnimation gif = new GifAnimation();
            gif.m_Textures = frames;

            return gif;
        }


        public void Update(long elapsedTicks)
        {
            if (m_IsPaused || m_IsStopped)
            {
                return;
            }

            m_CurrentTick += elapsedTicks;

            if (m_CurrentTick >= 1000000)
            {
                m_CurrentTick = 0;
                m_CurrentFrame++;

                if (m_CurrentFrame >= m_Textures.Length)
                {
                    m_CurrentFrame = 0;
                }
            }
        }

        /// <summary>
        /// Returns current frame of <seealso cref="GifAnimation"/>.
        /// </summary>
        /// <returns>Returns current frame of <seealso cref="GifAnimation"/>.</returns>
        public Texture2D GetTexture()
        {
            return m_Textures[m_CurrentFrame];
        }

        /// <summary>
        /// Returns specified frame of Gif Animation.
        /// </summary>
        /// <param name="frameIndex">frame index.</param>
        /// <returns>Returns specified frame of <seealso cref="GifAnimation"/>.</returns>
        public Texture2D GetTexture(int frameIndex)
        {
            return m_Textures[frameIndex];
        }

        /// <summary>
        /// Return total number of frames existing in this <seealso cref="GifAnimation"/>.
        /// </summary>
        public int FrameCount
        {
            get
            {
                return m_Textures.Length;
            }
        }

        public int CurrentFrame
        {
            get
            {
                return m_CurrentFrame;
            }
        }

        public int Width
        {
            get
            {
                return m_Textures[0].Width;
            }
        }

        public int Height
        {
            get
            {
                return m_Textures[0].Height;
            }
        }

        public void Pause()
        {
            m_IsPaused = true;
        }

        public void Resume()
        {
            m_IsPaused = false;
        }

        public void Stop()
        {
            m_CurrentFrame = 0;
            m_IsPaused = false;
            m_IsStopped = true;
        }

        public void Play()
        {
            m_CurrentFrame = 0;
            m_IsPaused = false;
            m_IsStopped = false;
        }

        public override string ToString()
        {
            return string.Format("Playing frame {0} of {1} -- {2:F}%", m_CurrentFrame, FrameCount, (float)(100f * (float)m_CurrentFrame / (float)FrameCount));
        }
    }
}
