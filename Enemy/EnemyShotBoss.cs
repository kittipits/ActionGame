using System;

namespace ActionGame
{
    public class EnemyShotBoss : EnemyShot
    {
        float vx;
        float vy;
        float gravity;

        public EnemyShotBoss(PlayScene playScene, float x, float y, float angle, float speed) : base(playScene)
        { 
            this.x = x;
            this.y = y;
            vx = (float)Math.Cos(angle) * speed;
            vy = (float)Math.Sin(angle) * speed;

            imageWidth = 16;
            imageHeight = 16;
            hitboxOffsetLeft = 3;
            hitboxOffsetRight = 3;
            hitboxOffsetTop = 3;
            hitboxOffsetBottom = 3;

            collisionDamage = 1;
            collisionRadius = 8;
            gravity = 0.08f;
        }

        public override void Update()
        {
            x += vx;
            y += vy;
            vy += gravity;

            if (x < 0 || x > 816 ||
                y < 0 || y > 496)
            {
                isDead = true;
            }
        }

        public override void Draw()
        {
            Camera.DrawGraph(x, y, Image.enemyShot);
        }
    }
}
