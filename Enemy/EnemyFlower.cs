using MyLib;

namespace ActionGame
{
    public class EnemyFlower : Enemy
    {
        int counter;
        
        public EnemyFlower(PlayScene playScene, float x, float y) : base(playScene)
        { 
            this.x = x;
            this.y = y;

            imageWidth = 32;
            imageHeight = 32;
            hitboxOffsetLeft = 0;
            hitboxOffsetRight = 0;
            hitboxOffsetTop = 0;
            hitboxOffsetBottom = 0;

            maxLife = 15;
            life = maxLife;
            counter = 60;
        }
        public override void Update()
        {
            base.Update();

            if (Isvisible())
            {
                Shoot();
            }
        }
        void Shoot()
        {
            counter++;
            if (counter % 120 == 0)
            {
                float playerPosX = (playScene.player.GetLeft() + playScene.player.GetRight()) / 2;
                float playerPosY = (playScene.player.GetTop() + playScene.player.GetBottom()) / 2;
                float posX = (GetLeft() + GetRight()) / 2;
                float posY = (GetTop() + GetBottom()) / 2;
                float angle = MyMath.PointToPointAngle(posX, posY, playerPosX, playerPosY);

                for (int i = 0; i < 3; i++)
                {
                    float speed = 4f + 0.25f * i;
                    playScene.gameObjects.Add(new EnemyShotSmall(playScene, posX, y, angle, speed));
                }
            }
        }

        public override void Draw()
        {
            DrawFlash(Image.flower);
            Camera.DrawLifeBar(GetLeft(), GetTop(), GetRight(), maxLife, life);
        }
        protected override void ScoreUp()
        {
            playScene.score += 150;
        }
    }
}
