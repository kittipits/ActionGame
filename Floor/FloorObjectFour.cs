namespace ActionGame
{
    public class FloorObjectFour: FloorObject
    {
        float vx;
        public FloorObjectFour(PlayScene playScene, float x, float y) : base(playScene)
        {
            this.x = x;
            this.y = y;
            
            imageWidth = 32 * 3;
            imageHeight = 32;
            hitboxOffsetLeft = 0;
            hitboxOffsetRight = 0;
            hitboxOffsetTop = 0;
            hitboxOffsetBottom = 0;
            vx = -0.5f;
        }

        public override void Update()
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

        public override void Draw()
        {
            Camera.DrawGraph(x, y, Image.FloorObject);
        }

        public override void OnCollision(GameObject other)
        {
        }
    }
}
