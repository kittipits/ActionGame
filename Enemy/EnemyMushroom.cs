namespace ActionGame
{
    public class EnemyMushroom : Enemy
    {
        const float fallSpeed = 3.0f;
        float vx;
        float vy;
        
        public EnemyMushroom(PlayScene playScene, float x, float y) : base(playScene)
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
            vx = -0.5f;
            vy = fallSpeed;
        }
        public override void Update()
        {
            base.Update();

            if (Isvisible())
            {
                MoveX();
                MoveY();
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

        public override void Draw()
        {
            DrawFlash(Image.shiitake);
            Camera.DrawLifeBar(GetLeft(), GetTop(), GetRight(), maxLife, life);
        }

        protected override void ScoreUp()
        {
            playScene.score += 100;
        }
    }
}
