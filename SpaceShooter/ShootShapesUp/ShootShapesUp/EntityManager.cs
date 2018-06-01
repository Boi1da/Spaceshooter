using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShootShapesUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    static class EntityManager
    {
        
        
        public static Random rand = new Random();

        static List<Entity> entities = new List<Entity>();
        static List<Enemy> enemies = new List<Enemy>();
        static List<Bullet> bullets = new List<Bullet>();
        static List<PowerUp> powerUps = new List<PowerUp>();

        static bool isUpdating;
        static List<Entity> addedEntities = new List<Entity>();

        public static int Count { get { return entities.Count; } }


        public static void Add(Entity entity)
        {
            if (!isUpdating)
                AddEntity(entity);
            else
                addedEntities.Add(entity);
          
        }

        private static void AddEntity(Entity entity)
        {
            entities.Add(entity);
            if (entity is Bullet)
                bullets.Add(entity as Bullet);

            else if (entity is Enemy)
                enemies.Add(entity as Enemy);

            else if (entity is PowerUp)
            {
                powerUps.Add(entity as PowerUp);
            }

        }


        public static void Update()
        {

            if (PlayerShip.Instance.bossDead)
            {
                foreach (Enemy enemy in enemies)
                {
                    enemy.IsExpired = true;
                }
            }
            isUpdating = true;
            HandleCollisions();

            foreach (var entity in entities)
                entity.Update();

            isUpdating = false;

            foreach (var entity in addedEntities)
                AddEntity(entity);

            addedEntities.Clear();

            entities = entities.Where(x => !x.IsExpired).ToList();
            bullets = bullets.Where(x => !x.IsExpired).ToList();
            enemies = enemies.Where(x => !x.IsExpired).ToList();
            powerUps = powerUps.Where(x => !x.IsExpired).ToList();

        }

        static void HandleCollisions()
        {
            // handle collisions between enemies
            for (int i = 0; i < enemies.Count; i++)
                for (int j = i + 1; j < enemies.Count; j++)
                {
                    if (IsColliding(enemies[i], enemies[j]))
                    {
                        enemies[i].HandleCollision(enemies[j]);
                        enemies[j].HandleCollision(enemies[i]);
                    }
                }

            // handle collisions between bullets and enemies
            for (int i = 0; i < enemies.Count; i++)
                for (int j = 0; j < bullets.Count; j++)
                {
                    if (IsColliding(enemies[i], bullets[j]))
                    {
                        enemies[i].WasShot();
                        bullets[j].IsExpired = true;

                    }
                }

            // handle collisions between the player and enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].IsActive && IsColliding(PlayerShip.Instance, enemies[i]))
                {
                   //Find out the damage of the attack based on impact
                    PlayerShip.Instance.IsHit(enemies[i].Damage);
                    // PlayerShip.Instance.Kill();
                    enemies[i].IsExpired = true;
                    enemies[i].WasShot();

                    if (PlayerShip.Instance.isGameOver )
                    {
                        
                        foreach (Enemy enemy in enemies) {
                            enemy.IsExpired = true;
                        }
                    }
                    //EnemySpawner.Reset();
                    break;
                }
            }

            //handle collisions between powerups and player
            for (int i = 0; i < powerUps.Count; i++)
            {
                if (IsColliding(PlayerShip.Instance, powerUps[i]))
                {
                    //Make Power Up Disappear, Use it too
                    powerUps[i].IsExpired = true;
                        PlayerShip.Instance.PowerUpPickedUp();
        
                    break;
                }
            }


        }

        //Find the closest enemy
        public static bool CanPickUpPowerUp()
        {
            
            Vector2 playerPos = PlayerShip.Instance.Position;
            float minDist = 20f;
            foreach (var powerUp in powerUps)
            {
                float dist = Vector2.Distance(powerUp.Position, playerPos);
                //Console.WriteLine("The distance is " + dist);
                if (dist > minDist)
                {
                    
                    return true;
                }   
            }
            return false;
        }


private static bool IsColliding(Entity a, Entity b)
        {
            float radius = a.Radius + b.Radius;
            return !a.IsExpired && !b.IsExpired && Vector2.DistanceSquared(a.Position, b.Position) < radius * radius;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
                entity.Draw(spriteBatch);
                
        }
    }
}
