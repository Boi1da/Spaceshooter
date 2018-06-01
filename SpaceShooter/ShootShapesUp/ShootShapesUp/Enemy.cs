using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    class Enemy : Entity
    {
        public static Random rand = new Random();

        private List<IEnumerator<int>> behaviours = new List<IEnumerator<int>>();
        private int timeUntilStart = 60;
        public bool IsActive { get { return timeUntilStart <= 0; } }
        public int PointValue { get; private set; }

        //Intialize the timer 
        float counter = 0.1f;

        //Check if timer 
        public bool isTimerOn = false;



        public Enemy(Texture2D image, Vector2 position)
        {
            this.image = image;
            Position = position;
            Health = 4;
            Radius = image.Width / 2f;
            color = Color.Transparent;
            PointValue = 1;
            isBoss = false;
        }


        public static Enemy CreateSeeker(Vector2 position)
        {
            var enemy = new Enemy(GameRoot.Seeker, position);
            enemy.AddBehaviour(enemy.FollowPlayer());
            enemy.PointValue = 2;

            return enemy;
        }

        public static Enemy CreateStraightLineEnemy(Vector2 position)
        {
            var enemy = new Enemy(GameRoot.Wanderer, position);
            enemy.AddBehaviour(enemy.StraightLine());
            enemy.Health = 100;
            return enemy;
        }

        public static Enemy CreateStraightLineEnemyLeft(Vector2 position)
        {
            var enemy = new Enemy(GameRoot.Wanderer, position);
            enemy.AddBehaviour(enemy.StraightLineLeft());
            enemy.Health = 100;
            return enemy;
        }

        public static Enemy CreateStraightLineEnemyRight(Vector2 position)
        {
            var enemy = new Enemy(GameRoot.Wanderer, position);
            enemy.AddBehaviour(enemy.StraightLineRight());
            enemy.Health = 100;
            return enemy;
        }

        public static Enemy CreateBoss(Vector2 position)
        {
            var enemy = new Enemy(GameRoot.Boss, position);
            enemy.AddBehaviour(enemy.Idle());
            enemy.Health = 200;
            enemy.Velocity = new Vector2(0, 0);
            enemy.isBoss = true;
            
            PlayerShip.Instance.bossSpawned = true;
            return enemy;
        }

        public override void Update()
        {
            if (timeUntilStart <= 0)
                ApplyBehaviours();
            else
            {
                timeUntilStart--;
                color = Color.White * (1 - timeUntilStart / 60f);
            }

            if(isTimerOn)
            {

                counter -= (float)GameRoot.GameTime.ElapsedGameTime.TotalSeconds;
                if (counter <= 0)
                {
                    color = Color.White;
                    isTimerOn = false;
                }
            }
            Position += Velocity;
            

            Velocity *= 0.8f;
        }

        private void AddBehaviour(IEnumerable<int> behaviour)
        {
            behaviours.Add(behaviour.GetEnumerator());
        }

        private void ApplyBehaviours()
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (!behaviours[i].MoveNext())
                    behaviours.RemoveAt(i--);
            }
        }

        private void changeColor()
        {
            color = Color.Red;
            isTimerOn = true;
        }

        public void HandleCollision(Enemy other)
        {
            var d = Position - other.Position;
            Velocity += 10 * d / (d.LengthSquared() + 1);
        }

        public void WasShot()
        {
            changeColor();
            Health -= 1;
     
            if (this.Health == 0 )
            {

                IsExpired = true;
                GameRoot.Explosion.Play(0.5f, rand.NextFloat(-0.2f, 0.2f), 0);
                PlayerShip.Instance.enemiesKilled += 1;

                if (this.isBoss)
                {
                    PlayerShip.Instance.bossDead = true;
                }
               
                //Move down
                Vector2 moveDown = new Vector2(0, 6);
                //Set the offset to be 1 off the 
                Vector2 spawnPos = new Vector2(this.Position.X, this.Position.Y);
                //Create a rand chance of a power up dropping
                int chance = rand.Next(0, 100); 
                //50% Chance of finding a power up 
                if (chance < 100) {
                    EntityManager.Add(new PowerUp(spawnPos, moveDown));
                }
            }
        }

        #region Behaviours
        IEnumerable<int> FollowPlayer(float acceleration = 1f)
        {
            while (true)
            {
                if (!PlayerShip.Instance.IsDead)
                    Velocity += (PlayerShip.Instance.Position - Position) * (acceleration / (PlayerShip.Instance.Position - Position).Length());

                if (Velocity != Vector2.Zero)
                    Orientation = Velocity.ToAngle();

                yield return 0;
            }
        }

        //Idle (For Boss)
        IEnumerable<int> Idle(float acceleration = 1f)
        {
            
            while (true)
            {
                

                yield return 0;
            }
        }

        IEnumerable<int> StraightLine(float acceleration = 1f)
        {
            Vector2 playerLocationLast = PlayerShip.Instance.Position;
            while (true)
            {
                if (!PlayerShip.Instance.IsDead)
                    
                    Velocity += new Vector2(0,2);

                yield return 0;
            }
        }

        IEnumerable<int> StraightLineLeft(float acceleration = 1f)
        {
            Vector2 playerLocationLast = PlayerShip.Instance.Position;
            while (true)
            {
                if (!PlayerShip.Instance.IsDead)

                    Velocity += new Vector2(-2, 0);

                yield return 0;
            }
        }

        IEnumerable<int> StraightLineRight(float acceleration = 1f)
        {
            Vector2 playerLocationLast = PlayerShip.Instance.Position;
            while (true)
            {
                if (!PlayerShip.Instance.IsDead)

                    Velocity += new Vector2(2, 0);

                yield return 0;
            }
        }
        #endregion

    }
}
