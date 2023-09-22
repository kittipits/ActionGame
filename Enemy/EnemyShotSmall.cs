using System;

namespace ActionGame
{
    public class EnemyShotSmall : EnemyShot
    {
        float vx;
        float vy;

        public EnemyShotSmall(PlayScene playScene, float x, float y, float angle, float speed) : base(playScene)
        {
            this.x = x;
            this.y = y;
            vx = (float)Math.Cos(angle) * speed;
            vy = (float)Math.Sin(angle) * speed;

            imageWidth = 16;
            imageHeight = 16;
            hitboxOffsetLeft = 4;
            hitboxOffsetRight = 4;
            hitboxOffsetTop = 4;
            hitboxOffsetBottom = 4;

            collisionDamage = 1;
            collisionRadius = 4;
        }

        public override void Update()
        {
            base.Update();

            x += vx;
            y += vy;

            HitOnWall();
        }

        public override void Draw()
        {
            Camera.DrawGraph(x, y, Image.enemyShot);
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player || other is PlayerShield || other is FloorObject || other is BlockObject || other is PlayerSword)
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
