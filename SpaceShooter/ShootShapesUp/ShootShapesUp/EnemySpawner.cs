using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    static class EnemySpawner
    {
        static Random rand = new Random();
        static float inverseSpawnChance = 60;
        static Vector2 bossSpawnPos = new Vector2(GameRoot.ScreenSize.X / 2, 10);
       


        public static void Update()
        {
            if(!PlayerShip.Instance.isGameOver || !PlayerShip.Instance.bossDead)
            {
                    levelSpawner();
    
            }
            
        }


        private static void levelSpawner()
        {
            int randEnemySpawn = rand.Next(1,3);

            //Level 1
            if (!PlayerShip.Instance.IsDead && EntityManager.Count < 200 )
            {
                if (rand.Next((int)inverseSpawnChance) == 0 && PlayerShip.Instance.level != 4)
                {
                    EntityManager.Add(Enemy.CreateSeeker(GetSpawnPosition()));

                }
                else if (rand.Next((int)inverseSpawnChance) == 10 && PlayerShip.Instance.level == 2)
                {
                    //Level 2
                    EntityManager.Add(Enemy.CreateStraightLineEnemy(GetSpawnPosition()));
                }

                else if (rand.Next((int)inverseSpawnChance) == 10 && PlayerShip.Instance.level == 3)
                {
                    Console.WriteLine("Random Enemy: " + randEnemySpawn);
                    //Level 3
                    switch (randEnemySpawn)
                    {
                        case 1:
                            EntityManager.Add(Enemy.CreateStraightLineEnemy(GetSpawnPosition()));
                            Console.WriteLine("Random Enemy: " + randEnemySpawn);
                            break;

                        case 2:
                            EntityManager.Add(Enemy.CreateStraightLineEnemyLeft(GetSpawnPositionRightSide()));
                            EntityManager.Add(Enemy.CreateStraightLineEnemyLeft(GetSpawnPositionRightSide()));
                            break;

                    }
                }
                //Final Level
                else if (rand.Next((int)inverseSpawnChance) == 10 && PlayerShip.Instance.level == 4)
                {
                    if (PlayerShip.Instance.bossSpawned == false)
                    {
                        EntityManager.Add(Enemy.CreateBoss(bossSpawnPos));
                    }

                    switch (randEnemySpawn)
                    {

                        case 1:
                            EntityManager.Add(Enemy.CreateStraightLineEnemy(GetSpawnPosition()));
                            break;

                        case 2:
                            EntityManager.Add(Enemy.CreateStraightLineEnemyLeft(GetSpawnPositionRightSide()));
                            break;
                        case 3:
                            EntityManager.Add(Enemy.CreateStraightLineEnemyRight(GetSpawnPositionLeftSide()));
                            break;
                    }
                }
            } 
            // slowly increase the spawn rate as time progresses
            if (inverseSpawnChance > 20)
                inverseSpawnChance -= 0.01f;
        }

        private static Vector2 GetSpawnPosition()
        {
            Vector2 pos;
            //Spawn Off Screen
            float spawnPos = GameRoot.ScreenSize.Y * -1;

            do
            {
                pos = new Vector2(rand.Next((int)GameRoot.ScreenSize.X), spawnPos);
            }
            while (Vector2.DistanceSquared(pos, PlayerShip.Instance.Position) < 250 * 250);

            return pos;
        }

        private static Vector2 GetSpawnPositionRightSide()
        {
            Vector2 pos;
            //Spawn Off Screen
            float spawnPos = rand.Next((int)GameRoot.ScreenSize.Y);

            do
            {
                pos = new Vector2(GameRoot.ScreenSize.X, spawnPos);
                Console.WriteLine("Spawning Enemy here: " + pos);
            }
            while (Vector2.DistanceSquared(pos, PlayerShip.Instance.Position) < 250 * 2);

            return pos;
        }


        private static Vector2 GetSpawnPositionLeftSide()
        {
            Vector2 pos;
            //Spawn Off Screen
            float spawnPos = rand.Next((int)GameRoot.ScreenSize.Y);

            do
            {
                pos = new Vector2(1, spawnPos);
                Console.WriteLine("Spawning Enemy here: " + pos);
            }
            while (Vector2.DistanceSquared(pos, PlayerShip.Instance.Position) < 250 * 2);

            return pos;
        }

        public static void Reset()
        {
            inverseSpawnChance = 60;
        }
    }
}
