using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ShootShapesUp
{
    class PowerUpBar : Game
    {
        private Texture2D container, powerUpBar;
        private Vector2 position;
        public int fullPower;
        public int currentPower;

        public PowerUpBar(ContentManager content)
        {
            position = new Vector2(1, 1);
            LoadContent(content);
            fullPower = powerUpBar.Width;
            currentPower = PlayerShip.Instance.Health;
        }

        private void LoadContent(ContentManager content)
        {
            container = content.Load<Texture2D>("Art/hpBar");
            powerUpBar = content.Load<Texture2D>("Art/hpbar_inner");
        }

        public void Update()
        { 
            if (currentPower < fullPower )
            {
                currentPower = PlayerShip.Instance.power;
            } else
            {
                PlayerShip.Instance.canUseSpecial = true;
             
                if (PlayerShip.Instance.isTimerOn)
                {
                    currentPower = 0;
                }
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(powerUpBar, new Rectangle((int)position.X, (int)position.Y, currentPower, powerUpBar.Height), Color.White);
            spriteBatch.Draw(container, position, Color.White);
        }
    }
}
