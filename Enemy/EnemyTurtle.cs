using System;

namespace ActionGame
{
    public class EnemyTurtle : Enemy
    {
        float theta;
        float vx;
        float vy;

        public EnemyTurtle(PlayScene playScene, float x, float y) : base(playScene)
        {
            this.x = x;
            this.y = y;

            imageWidth = 32;
            imageHeight = 32;
            hitboxOffsetLeft = 0;
            hitboxOffsetRight = 0;
            hitboxOffsetTop = 0;
            hitboxOffsetBottom = 0;

            maxLife = 10;
            life = maxLife;
        }
        public override void Update()
        {
            base.Update();

            if (Isvisible())
            {
                Move();
                MoveX();
                //MoveY();
            }

        }

        void Move()
        {
            theta += 0.05f;
            vx = (float)Math.Cos((theta - Math.PI) / 3 ) * 1.5f;
            vy = (float)Math.Cos(theta) / 2;

            x += vx;
            y += vy;
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
                float wallTop = bottom - bottom % Map.CellSize;
                SetBottom(wallTop);
                vy = -vy;
            }
        }

        public override void Draw()
        {
            DrawFlash(Image.turtle);
            Camera.DrawLifeBar(GetLeft(), GetTop(), GetRight(), maxLife, life);
        }

        protected override void ScoreUp()
        {
            playScene.score += 100;
        }
    }
}
