using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ShootShapesUp
{
    class Parralax : Game
    {
        private Vector2 screenPos, origin, size;
        private Texture2D bgTexture;
        private int screenHeight;

        public void Load(GraphicsDevice device, Texture2D backgroundTexture)
        {
            bgTexture = backgroundTexture;
            screenHeight = device.Viewport.Height;
            int screenwidth = device.Viewport.Width;

            //Draw from the center
            origin = new Vector2(bgTexture.Width / 2, 0);
            screenPos = new Vector2(screenwidth / 2, screenHeight / 2);
            size = new Vector2(0, bgTexture.Height);
        }

        public void Update(float deltaY)
        {
            if (!PlayerShip.Instance.bossSpawned)
            {
                screenPos.Y += deltaY;
                screenPos.Y = screenPos.Y % bgTexture.Height;

            }

        }

        public void Draw(SpriteBatch batch)
        {

            //Draw the background
            if (screenPos.Y < screenHeight)
            {
                batch.Draw(bgTexture, screenPos, null, Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            }

            batch.Draw(bgTexture, screenPos - size, null, Color.White, 0, origin, 1, SpriteEffects.None, 0f);
        }


    }
}
