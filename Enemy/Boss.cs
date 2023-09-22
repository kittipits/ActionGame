using MyLib;
using System;

namespace ActionGame
{
    public class Boss : Enemy
    {
        enum State
        {
            Appear,
            Normal,
            Swoon,
            Angry,
            Dying,
        }

        float vx;
        float vy;
        float vxx;
        int counter;

        State state = State.Appear;
        int appearTime = 60;
        int swoonTime = 120;
        int dyingTime = 180;
        int normalTime = 600;
        int angryTime = 0;
        int chargeTime = 300;

        float bodyAngle = 0.0f;
        float centerX;
        float centerY;

        float restartX = 364;
        float restartY = 172;
        int restartTime = 40;
        int itemSpawnTime = 0;

        int[,] teleportPositionNormal = { { 100, 64 }, { 132, 64 } , { 388, 64 } , { 644, 64 } , { 676, 64 },
                                    { 100, 256 }, { 132, 256 } , { 388, 256 } , { 644, 256 } , { 676, 256 },
                                    { 260, 160 }, { 516, 160 } , { 260, 384 } , { 516, 384 } };

        int[,] teleportPositionAngry = { { 92, 172 }, { 364, 172 } , { 636, 172 },
                                    { 236, 80 }, { 236, 284 } , { 492, 80 } , { 492, 284 } };

        int[,] ghostSpawnPosition = { { 0, 0 }, { 416, 0 }, { 832, 0 }, { 0, 256 }, { 832, 256 },
                                    { 0, 512 }, { 416, 512 }, { 832, 512 } };

        int animationCounter = 0;
        int[] enemyMotion = new int[5] { 1, 2, 1, 0, 1 };
        int motionIndex = 0;

        public Boss(PlayScene playScene, float x, float y) : base(playScene)
        { 
            this.x = x;
            this.y = y;

            imageWidth = 60;
            imageHeight = 70;
            hitboxOffsetLeft = 20;
            hitboxOffsetRight = 20;
            hitboxOffsetTop = 14;
            hitboxOffsetBottom = 10;

            maxLife = 150;
            life = maxLife;
            vx = 0.8f;
            vy = 0.8f;
            counter = 60;
        }
        public override void Update()
        {
            if (isDamaged) flashTimer++;
            if (flashTimer >= 45)
            {
                isDamaged = false;
                flashTimer = 0;
            }

            StateTransition();

            SpawnItem();

            animationCounter++;
        }

        void MoveX()
        {
            // 当たり判定の四隅の座標を取得
            float left = GetLeft();
            float right = GetRight() - 0.01f;
            float top = GetTop();
            float bottom = GetBottom() - 0.01f;

            // 左端が壁にめりこんでいるか？
            if (playScene.map.IsWall(left, top) ||
                playScene.map.IsWall(left, bottom))
            {
                float wallRight = left - left % Map.CellSize + Map.CellSize;
                SetLeft(wallRight);
                vx = -vx;
            }

            // 右端が壁にめりこんでいるか？
            else if (playScene.map.IsWall(right, top) ||
                playScene.map.IsWall(right, bottom))
            {
                float wallLeft = right - right % Map.CellSize;
                SetRight(wallLeft);
                vx = -vx;
            }
        }

        void MoveY()
        {

            // 着地したかどうか
            bool grounded = false;

            // 当たり判定の四隅の座標を取得
            float left = GetLeft();
            float right = GetRight() - 0.01f;
            float top = GetTop();
            float bottom = GetBottom() - 0.01f;

            // 上端が壁にめりこんでいるか？
            if (playScene.map.IsWall(left, top) ||
                playScene.map.IsWall(right, top))
            {
                float wallBottom = top - top % Map.CellSize + Map.CellSize;
                SetTop(wallBottom);
                vy = -vy;
            }
            // 下端が壁にめりこんでいるか？
            else if (playScene.map.IsWall(left, bottom) ||
                playScene.map.IsWall(right, bottom))
            {
                grounded = true;
            }

            // もし着地してたら
            if (grounded)
            {
                float wallTop = bottom - bottom % Map.CellSize;
                SetBottom(wallTop);
                vy = -vy;
            }
        }

        void Shoot(int way, float delta, float speed)
        {
            counter++;
            counter++;
            if (counter % 150 == 0)
            {
                float verticalDistance = GetBottom() - playScene.player.GetTop();
                float playerPosX = (playScene.player.GetLeft() + playScene.player.GetRight()) / 2;
                float posX = (GetLeft() + GetRight()) / 2;
                float posY = (GetTop() + GetBottom()) / 2;
                float angle = MyMath.PointToPointAngle(posX, posY, playerPosX, posY - y - verticalDistance);

                ShootEnemyBulletsWays(way, angle, delta, speed, posX);
            }
        }

        void ShootEnemyBulletsWays(int numWays, float standardAngle, float deltaAngle, float speed, float posX)
        {
            float startAngle = (numWays - 1) * deltaAngle / 2.0f;

            for (int i = 0; i < numWays; i++)
            {
                float firingAngle = (startAngle - deltaAngle * i) * MyMath.Deg2Rad;
                playScene.gameObjects.Add(new EnemyShotBoss(playScene, posX, y, standardAngle + firingAngle, speed));
            }
        }

        void StateTransition()
        {
            if (state == State.Appear)
            {
                x -= vx;
                MoveX();
                MoveY();

                if (x <= 500)
                {
                    appearTime--;
                    vx = 0f;
                }
                if (appearTime <= 0)
                {
                    vx = 1.2f;
                    state = State.Normal;
                }
            }
            else if (state == State.Normal)
            {
                float chargePosX = (GetLeft() + GetRight()) / 2 - 16;
                float chargePosY = (GetTop() - 32);

                x += vx;
                MoveX();
                MoveY();

                normalTime++;
                chargeTime++;
                if (chargeTime < 20)
                {
                    vxx = vx;
                }
                else if (chargeTime < 200)
                {
                    vx = 0;
                    if (chargeTime % 10 == 0)
                    {
                        Sound.Play(Sound.seSparkling);
                        playScene.gameObjects.Add(new Sparkling(playScene, chargePosX + MyRandom.PlusMinus(16), chargePosY + MyRandom.PlusMinus(16)));
                    }
                }
                else if (chargeTime == 200)
                {
                    vx = vxx;
                    playScene.gameObjects.Add(new EnemyShotHoming(playScene, chargePosX, chargePosY, 240));
                }
                else 
                {
                    Shoot(3, 20f, 5f);
                }

                if (normalTime % 900 == 0)
                {
                    for (int i = 0; i < 15; i++)
                    {
                        Explode();
                    }
                    TeleportNormal();
                    chargeTime = 0;
                }

            }
            else if (state == State.Swoon)
            {
                swoonTime--; ;


                if (swoonTime > restartTime)
                {
                    vx = (restartX - x) / 180;
                    vy = (restartY - y) / 180;

                    x += vx;
                    y += vy;

                    vx *= 0.995f;
                    vy *= 0.995f;
                }
                else
                {
                    const float Agility = 0.07f;
                    x = x + (restartX - x) * Agility;
                    y = y + (restartY - y) * Agility;
                }

                if (swoonTime <= 0)
                {
                    state = State.Angry;

                    x = restartX;
                    y = restartY;

                    chargeTime = 0;
                }
            }
            else if (state == State.Angry)
            {
                angryTime++;
                float theta = angryTime / 20;
                vx = (float)Math.Cos(theta) * 2.5f;
                vy = (float)Math.Sin(theta) * 1.5f;

                x += vx;
                y += vy;
                MoveX();
                MoveY();

                chargeTime++;
                float chargePosX = (GetLeft() + GetRight()) / 2 - 16;
                float chargePosY = (GetTop() - 32);

                if (chargeTime < 150)
                {
                    if (chargeTime % 10 == 0)
                    {
                        Sound.Play(Sound.seSparkling);
                        playScene.gameObjects.Add(new Sparkling(playScene, chargePosX + MyRandom.PlusMinus(16), chargePosY + MyRandom.PlusMinus(16)));
                    }
                }
                else if (chargeTime == 150)
                {
                    playScene.gameObjects.Add(new EnemyShotHoming(playScene, chargePosX, chargePosY, 300));
                }
                else
                {
                    Shoot(5, 15f, 6f);
                }

                if (angryTime % 900 == 0)
                {
                    for (int i = 0; i < 15; i++)
                    {
                        Explode();
                    }
                    TeleportAngry();
                    chargeTime = 0;
                }

                if (angryTime % 600 == 300)
                {
                    SpawnGhost();
                }

            }
            else if (state == State.Dying)
            {
                dyingTime--;

                // ①  傾く
                bodyAngle += (0.25f * MyMath.Deg2Rad);

                // ② 震える
                // 「タイマー」を「震えるときの角度」に変換
                float vibrationTheta = 2.8f * dyingTime * MyMath.Deg2Rad;
                x = centerX + (float)(Math.Cos(vibrationTheta * 43) * 3f);
                y = centerY + (float)(Math.Sin(vibrationTheta * 37) * 3f);

                // ③  沈む
                centerY += 0.5f;

                // ④  爆発を出す
                if (dyingTime % 5 == 0)
                {
                    Sound.Play(Sound.seSparkling);
                    Explode();
                }

                if (dyingTime <= 0)
                {
                    // ⑤  消える瞬間に大爆発を起こす
                    for (int i = 0; i < 30; i++)
                    {
                        Explode();
                    }

                    Sound.Play(Sound.seSparkling);
                    isDead = true;
                    ScoreUp();
                    playScene.state = PlayScene.State.Clear;
                }
            }
        }

        void TeleportNormal() 
        {
            int index = MyRandom.Range(0, teleportPositionNormal.GetLength(0));
            x = teleportPositionNormal[index, 0];
            y = teleportPositionNormal[index, 1];
            for (int i = 0; i < 15; i++)
            {
                Explode();
            }
            Sound.Play(Sound.seSparkling);
        }

        void TeleportAngry()
        {
            int index = MyRandom.Range(0, teleportPositionAngry.GetLength(0));
            x = teleportPositionAngry[index, 0];
            y = teleportPositionAngry[index, 1];
            for (int i = 0; i < 15; i++)
            {
                Explode();
            }
            Sound.Play(Sound.seSparkling);
        }

        void SpawnGhost()
        {
            int index = MyRandom.Range(0, ghostSpawnPosition.GetLength(0));
            playScene.gameObjects.Add(new EnemyGhostBossRoom(playScene, ghostSpawnPosition[index,0], ghostSpawnPosition[index,1]));
        }

        void SpawnItem()
        {
            itemSpawnTime++;
                
                if (itemSpawnTime % 3600 == 0)
                {
                    int index = MyRandom.Range(0, teleportPositionNormal.GetLength(0));
                    float itemPosX = teleportPositionNormal[index, 0];
                    float itemPosY = teleportPositionNormal[index, 1];
                    playScene.gameObjects.Add(new ItemLifeBoost(playScene, itemPosX, itemPosY));
                }

                if (itemSpawnTime % 5400 == 1800)
                {
                    int index = MyRandom.Range(0, teleportPositionNormal.GetLength(0));
                    float itemPosX = teleportPositionNormal[index, 0];
                    float itemPosY = teleportPositionNormal[index, 1];
                    playScene.gameObjects.Add(new ItemShieldBoost(playScene, itemPosX, itemPosY));
                }
                
                if (itemSpawnTime % 5400 == 4500)
                {
                    int index = MyRandom.Range(0, teleportPositionNormal.GetLength(0));
                    float itemPosX = teleportPositionNormal[index, 0];
                    float itemPosY = teleportPositionNormal[index, 1];
                    playScene.gameObjects.Add(new ItemSpeedBoost(playScene, itemPosX, itemPosY));
                }
        }

        public override void Draw()
        {
            motionIndex = motionIndex % 4;
            if (animationCounter % 10 == 0) motionIndex++;

            if ((state == State.Normal && chargeTime < 200) || (state == State.Angry && chargeTime < 150))
            {
                DrawFlash(Image.annette[3]);
            }
            else if (state == State.Appear || state == State.Normal || state == State.Angry)
            {
                DrawFlash(Image.annette[enemyMotion[motionIndex]]);
            }
            else if (state == State.Swoon || state == State.Dying)
            {
                DrawFlash(Image.annette[4]);
            }

            Camera.DrawLifeBar(GetLeft(), GetTop(), GetRight(), maxLife, life);
        }

        protected override void ScoreUp()
        {
            playScene.score += 1000;
        }

        public override void OnCollision(GameObject other)
        {
            if (state == State.Appear || state == State.Swoon || state == State.Dying)
                return;

            if (life <= 0)
            {
                state = State.Dying;

                centerX = x;
                centerY = y;
            }
            else if (state == State.Normal && life <= maxLife / 2)
            {
                state = State.Swoon;

                vx = 1.2f;
                vy = 1.2f;
            }

            if (other is PlayerShot || other is PlayerSword)
            {
                TakeDamage(other.collisionDamage);
            }
        }

        public override void Explode()
        {
            float explosionX = MyRandom.Range(GetLeft() - 32, GetRight());
            float explosionY = MyRandom.Range(GetTop() - 32, GetBottom());

            playScene.gameObjects.Add(new Sparkling(playScene, explosionX, explosionY));
        }
    }
}
