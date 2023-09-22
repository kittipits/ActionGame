namespace ActionGame
{
    public class PlayerNormalShot : PlayerShot
    {
        const float MoveSpeed = 10f;
        float vx;

        public PlayerNormalShot(PlayScene playScene, float x, float y, Direction direction) : base(playScene)
        { 
            this.x = x;
            this.y = y;

            imageWidth = 16;
            imageHeight = 16;
            hitboxOffsetLeft = 0;
            hitboxOffsetRight = 0;
            hitboxOffsetTop = 3;
            hitboxOffsetBottom = 3;
            
            // 向きに応じた速度を設定する
            if (direction == Direction.Right) vx = MoveSpeed;
            else vx = -MoveSpeed;
            collisionDamage = 1;
        }

        public override void Update()
        {
            base.Update();

            x += vx;

            HitOnWall();
        }

        public override void Draw()
        {
            Camera.DrawGraph(x, y, Image.playerShot);
        }
    }
}
