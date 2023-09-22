namespace ActionGame
{
    public class PlayerSwordComboThird : PlayerSword
    {
        const float MoveSpeed = 5f;
        GameObject beam = null;
        int timer;

        float vx;

        public PlayerSwordComboThird(PlayScene playScene, float x, float y, Direction direction) : base(playScene)
        {
            this.x = x;
            this.y = y;

            imageWidth = 24;
            imageHeight = 24;
            hitboxOffsetLeft = 0;
            hitboxOffsetRight = 0;
            hitboxOffsetTop = 0;
            hitboxOffsetBottom = 0;

            timer = 0;
            collisionDamage = 8;

            // 向きに応じた速度を設定する
            if (direction == Direction.Right) vx = MoveSpeed;
            else vx = -MoveSpeed;
        }

        public override void Update()
        {
            x += vx;

            timer++;
            if (timer > 40) isDead = true;

            // カメラの範囲外に出たら削除
            if (!Isvisible())
            {
                isDead = true;
            }

            HitOnWall();
        }

        public override void Draw()
        {
            Camera.DrawGraph(x, y, Image.swordBeam);
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Enemy || other is FloorObject || other is BlockObject)
            {
                isDead = true;
            }
        }

        void HitOnWall()
        {
            // 当たり判定の四隅の座標を取得
            float left = GetLeft();
            float right = GetRight() - 0.01f;
            float top = GetTop();
            float bottom = GetBottom() - 0.01f;

            // 壁にめりこんでいるか？
            if (playScene.map.IsWall(left, top) ||
                playScene.map.IsWall(left, bottom) ||
                playScene.map.IsWall(right, top) ||
                playScene.map.IsWall(right, bottom))
            {
                isDead = true;
            }
        }
    }
}
