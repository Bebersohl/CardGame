﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CardGame
{
    public class CardSprite
    {
        Texture2D t2dTexture;

        float fFrameRate = 0.02f;
        float fElapsed = 0.0f;

        int iFrameOffsetX = 0;
        int iFrameOffsetY = 0;
        int iFrameWidth = 32;
        int iFrameHeight = 32;

        int iFrameCount = 1;
        int iCurrentFrame = 0;
        int iScreenX = 0;
        int iScreenY = 0;
        int cardIndex = 0;
        int iCardNumber = 0;
        Square currentSquare;

        bool bHighlighted = false;
        bool bAnimating = true;
        bool bMoveable = false;
        public CardSprite(
            Texture2D texture,
            int FrameOffsetX,
            int FrameOffsetY,
            int FrameWidth,
            int FrameHeight,
            int FrameCount,
            bool Selected,
            int ci,
            int CardNumber)
        {
             t2dTexture = texture;
             iFrameOffsetX = FrameOffsetX;
             iFrameOffsetY = FrameOffsetY;
             iFrameWidth = FrameWidth;
             iFrameHeight = FrameHeight;
             iFrameCount = FrameCount;
             bMoveable = Selected;
             cardIndex = ci;
             iCardNumber = CardNumber;
             bHighlighted = false;
             currentSquare = null;
        }

        public Rectangle GetSourceRect()
        {
            return new Rectangle(
            iFrameOffsetX + (iFrameWidth * iCurrentFrame),
            iFrameOffsetY,
            iFrameWidth,
            iFrameHeight);
        }
        public Square CurrentSquare
        {
            get { return currentSquare; }
            set { currentSquare = value; }
        }
        public bool Highlighted
        {
            get { return bHighlighted; }
            set { bHighlighted = value; }
        }
        public Texture2D cardTexture
        {
            get { return t2dTexture; }
            set { t2dTexture = value;  }
        }
        public int X
        {
            get { return iScreenX; }
            set { iScreenX = value; }
        }

        public int Y
        {
            get { return iScreenY; }
            set { iScreenY = value; }
        }

        public int cardInd
        {
            get { return cardIndex; }
            set { cardIndex = value; }
        }

        public int Frame
        {
            get { return iCurrentFrame; }
            set { iCurrentFrame = (int)MathHelper.Clamp(value, 0, iFrameCount); }
        }

        public float FrameLength
        {
            get { return fFrameRate; }
            set { fFrameRate = (float)Math.Max(value, 0f); }
        }

        public bool IsAnimating
        {
            get { return bAnimating; }
            set { bAnimating = value; }
        }

        public bool IsMoveable
        {
            get { return bMoveable; }
            set { bMoveable = value; }
        }
        public int CardNumber
        {
            get { return iCardNumber; }
            set { iCardNumber = value; }
        }
        public void Update(GameTime gametime)
        {
            if (bAnimating)
            {
                // Accumulate elapsed time...
                fElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

                // Until it passes our frame length
                if (fElapsed > fFrameRate)
                {
                    // Increment the current frame, wrapping back to 0 at iFrameCount
                    iCurrentFrame = (iCurrentFrame + 1) % iFrameCount;

                    // Reset the elapsed frame time.
                    fElapsed = 0.0f;
                }
            }

            
        }

        public void Draw(
          SpriteBatch spriteBatch,
          int XOffset,
          int YOffset,
          bool NeedBeginEnd)
        {
            if (NeedBeginEnd)
                spriteBatch.Begin();

            spriteBatch.Draw(
                t2dTexture,
                new Rectangle(
                  iScreenX + XOffset,
                  iScreenY + YOffset,
                  iFrameWidth,
                  iFrameHeight),
                GetSourceRect(),
                Color.White);

            if (NeedBeginEnd)
                spriteBatch.End();
        }

        public void Draw(SpriteBatch spriteBatch, int XOffset, int YOffset)
        {
            Draw(spriteBatch, XOffset, YOffset, true);
        }
    }
}
