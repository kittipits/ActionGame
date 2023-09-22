using MyLib;
using System;

namespace ActionGame
{
    public class EnemyGhostBossRoom : Enemy
    {

        int time;
        public EnemyGhostBossRoom(PlayScene playScene, float x, float y) : base(playScene)
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

            Move();

            time++;

            if (time >= 900) isDead = true;
        }

        void Move()
        {
            float playerPosX = (playScene.player.GetLeft() + playScene.player.GetRight()) / 2;
            float playerPosY = (playScene.player.GetTop() + playScene.player.GetBottom()) / 2;
            float posX = (GetLeft() + GetRight()) / 2;
            float posY = (GetTop() + GetBottom()) / 2;

            // プレイヤーに向かってくる
            float angleToPlayer = MyMath.PointToPointAngle(posX, posY, playerPosX, playerPosY);
            float speed = 1.1f;
            x += (float)Math.Cos(angleToPlayer) * speed;
            y += (float)Math.Sin(angleToPlayer) * speed;


            if (MyMath.CircleCircleIntersection(playerPosX, playerPosY, 1.0f, posX, posY, 1.0f))
            {
                isDead = true;
            }
        }

        public override void Draw()
        {
            DrawFlash(Image.ghost);

            Camera.DrawLifeBar(GetLeft(), GetTop(), GetRight(), maxLife, life);
        }

        protected override void ScoreUp()
        {
            playScene.score += 100;
        }
    }
}
