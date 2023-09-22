using MyLib;
using System;

namespace ActionGame
{
    public class EnemyShotHoming : EnemyShot
    {
        int time;

        public EnemyShotHoming(PlayScene playScene, float x, float y, int time) : base(playScene)
        {
            this.x = x;
            this.y = y;

            imageWidth = 32;
            imageHeight = 32;
            hitboxOffsetLeft = 4;
            hitboxOffsetRight = 4;
            hitboxOffsetTop = 4;
            hitboxOffsetBottom = 4;

            collisionDamage = 3;
            this.time = time;
        }

        public override void Update()
        {
            time--;

            float playerPosX = (playScene.player.GetLeft() + playScene.player.GetRight()) / 2;
            float playerPosY = (playScene.player.GetTop() + playScene.player.GetBottom()) / 2;
            float angleToPlayer = MyMath.PointToPointAngle(x, y, playerPosX, playerPosY);
            float speed = 2.5f;
            x += (float)Math.Cos(angleToPlayer) * speed;
            y += (float)Math.Sin(angleToPlayer) * speed;

            if (time <= 0)
            {
                isDead = true;
            }

            if (x < 0 || x > 832 ||
                y < 0 || y > 512)
            {
                isDead = true;
            }
        }

        public override void Draw()
        {
            Camera.DrawGraph(x, y, Image.chargedShot);
        }
    }
}
