using System;

namespace ActionGame
{
    public class AIPierceShot : PlayerShot
    {
        float vx;
        float vy;

        public AIPierceShot(PlayScene playScene, float x, float y, float angle, float speed) : base(playScene)
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
        }

        public override void Update()
        {
            base.Update();

            x += vx;
            y += vy;

        }

        public override void Draw()
        {
            Camera.DrawGraph(x, y, Image.heart);
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Enemy)
            {
                isDead = true;
            }
        }
    }
}
