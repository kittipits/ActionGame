namespace ActionGame
{
    public class SpikeDrop : EnemyShot
    {
        float vy;
        bool isDrop;

        public SpikeDrop(PlayScene playScene, float x, float y) : base(playScene)
        {
            this.x = x;
            this.y = y;

            imageWidth = 32;
            imageHeight = 32;
            hitboxOffsetLeft = 0;
            hitboxOffsetRight = 0;
            hitboxOffsetTop = 0;
            hitboxOffsetBottom = 0;

            vy = 6f;
            isDrop = false;
            collisionDamage = 2;
            collisionRadius = 16;
        }

        public override void Update()
        {
            HitOnWall();
            Drop();
        }

        public override void Draw()
        {
            Camera.DrawGraph(x, y, Image.spikedrop);
        }

        void Drop()
        {
            float playerMid = (playScene.player.GetLeft() + playScene.player.GetRight()) / 2;
            if (playerMid >= GetLeft() && playerMid <= GetRight())
            {
                isDrop = true;
            }

            if (isDrop)
            {
                y += vy;
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
                Explode();
                Sound.Play(Sound.seBroke2);
                isDead = true;
            }
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player || other is PlayerShield || other is FloorObject || other is BlockObject)
            {
                Explode();
                Sound.Play(Sound.seBroke2);
                isDead = true;
            }
        }
    }
}
