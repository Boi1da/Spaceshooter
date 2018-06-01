using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    class PlayerShip : Entity
    {


        private static PlayerShip instance;
        public static PlayerShip Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlayerShip();

                return instance;
            }
        }

        const int cooldownFrames = 6;
        int cooldownRemaining = 0;

        int framesUntilRespawn = 0;
        int shieldDuration = 0;
        int maxPower = 150;
        int lives = 3;
        public int level = 1;
        public int enemiesKilled = 0;

        float speed = 8;
        //Intialize the timer 
        float counter = 10;
        int firePosition = 0;

        Vector2 offset;
        Vector2 offset2;
        Color bulletColor = Color.SeaGreen;

        public bool isTimerOn = false;

        public bool IsDead { get { return framesUntilRespawn > 0; } }
        public bool IsShieldUp { get { return shieldDuration > 0; } }
        public bool canUseSpecial = false;
        public bool isGameOver = false;
        public bool bossSpawned = false;
        public bool bossDead = false;
        

        static Random rand = new Random();

        private PlayerShip()
        {
            Health = 50;
            image = GameRoot.Player;
            Position = new Vector2(50, GameRoot.ScreenSize.Y + 400);
            Radius = 10;
        }

        public override void Update()
        {
            LevelChecker();
            Console.WriteLine("Number of enemies killed: " + enemiesKilled);
            if (IsDead && !isGameOver)
            {
                --framesUntilRespawn;
                if (lives <= 0)
                {
                    isGameOver = true;
                }
                return;
            }

            if (isTimerOn)
            {
                //Cant use special whilst already in special
                canUseSpecial = false;
                counter -= (float)GameRoot.GameTime.ElapsedGameTime.TotalSeconds;
                Console.WriteLine(counter);
                if (counter <= 0)
                {
                    speed = 8;
                    counter = 10;
                    isTimerOn = false;
                }
            }

        
            var aim = Input.GetAimDirection();
            if (aim.LengthSquared() > 0 && cooldownRemaining <= 0 && !isGameOver)
            {
                cooldownRemaining = cooldownFrames;
                Vector2 shootUp = 11f * new Vector2(0, -1);
                //Fire From the middle (Default)
                offset = new Vector2(PlayerShip.Instance.Position.X + firePosition, PlayerShip.Instance.Position.Y - 1f);
                offset2 = new Vector2(PlayerShip.Instance.Position.X - firePosition, PlayerShip.Instance.Position.Y - 1f);

                if (isTimerOn)
                {
                    bulletColor = Color.Yellow;
                    //Fire from the sides
                    firePosition = 15;
                    EntityManager.Add(new Bullet(offset2, shootUp, bulletColor));

                } else
                {
                    bulletColor = Color.White;
                    firePosition = 0;
                }

                //Fire from the middle

                EntityManager.Add(new Bullet(offset, shootUp,bulletColor));
                GameRoot.Shot.Play(0.2f, rand.NextFloat(-0.2f, 0.2f), 0);
            }

            if (cooldownRemaining > 0)
                cooldownRemaining--;

            
            Velocity = speed * Input.GetMovementDirection();
            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, GameRoot.ScreenSize - Size / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDead && !isGameOver)
                base.Draw(spriteBatch);
        }

        public void LevelChecker()
        {
            if (enemiesKilled >= 15 && enemiesKilled < 45)
            {
                level = 2;
            } else if (enemiesKilled >= 45 && enemiesKilled < 75)
            {
                level = 3; 
            } else if (enemiesKilled >= 75)
            {
                level = 4;
            }
        }

        public void Kill()
        {
            lives -= 1;
            framesUntilRespawn = 60;
            Health = 50;
        }

        public void IsHit(int damageTaken)
        {
            Health -= damageTaken;
             if(Health == 0)
             {

                Kill();
             }
        }

        public void useSpecial()
        {
            power = 0;
            speed = 15;
        }


        public void PowerUpPickedUp()
        {
            if (power < maxPower)
            {
                GameRoot.pickUpPowerUp.Play();
                power += 15;
            }
        }
    }
}
