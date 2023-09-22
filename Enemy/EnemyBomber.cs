using MyLib;

namespace ActionGame
{
    public class EnemyBomber : Enemy
    {
        const float fallSpeed = 3.0f;
        float vx;
        float vy;
        int counter;
        
        public EnemyBomber(PlayScene playScene, float x, float y) : base(playScene)
        { 
            this.x = x;
            this.y = y;

            imageWidth = 32;
            imageHeight = 32;
            hitboxOffsetLeft = 0;
            hitboxOffsetRight = 0;
            hitboxOffsetTop = 0;
            hitboxOffsetBottom = 0;

            maxLife = 20;
            life = maxLife;
            vx = -0.65f;
            vy = fallSpeed;
            counter = 60;
        }
        public override void Update()
        {
            base.Update();

            if (Isvisible())
            {
                MoveX();
                MoveY();
                Shoot();
            }

        }

        void MoveX()
        {
            x += vx;

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
            y += vy;

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
                vy = 0;
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
                vy = 0;
            }
            else
            {
                vy = fallSpeed;
            }
        }

        void Shoot()
        {
            counter++;
            counter++;
            if (counter % 150 == 0)
            {
                float playerPosX = (playScene.player.GetLeft() + playScene.player.GetRight()) / 2;
                //float playerPosY = (playScene.player.GetTop() + playScene.player.GetBottom()) / 2;
                float posX = (GetLeft() + GetRight()) / 2;
                float posY = (GetTop() + GetBottom()) / 2;
                float angle = MyMath.PointToPointAngle(posX, posY, playerPosX, posY - 200);

                ShootEnemyBulletsWays(3, angle, 18f, 4f, posX);
            }
        }

        void ShootEnemyBulletsWays(int numWays, float standardAngle, float deltaAngle, float speed, float posX)
        {
            float startAngle = (numWays - 1) * deltaAngle / 2.0f;

            for (int i = 0; i < numWays; i++)
            {
                float firingAngle = (startAngle - deltaAngle * i) * MyMath.Deg2Rad;
                playScene.gameObjects.Add(new EnemyShotParabolic(playScene, posX, y, standardAngle + firingAngle, speed));
            }
        }

        public override void Draw()
        {
            DrawFlash(Image.bomber);
            Camera.DrawLifeBar(GetLeft(), GetTop(), GetRight(), maxLife, life);
        }

        protected override void ScoreUp()
        {
            playScene.score += 200;
        }
    }
}
