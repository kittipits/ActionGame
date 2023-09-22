namespace ActionGame
{
    public class PlayerSwordComboFirst : PlayerSword
    {
        const float MoveSpeed = 3f;

        float vy;
        int timer;

        public PlayerSwordComboFirst(PlayScene playScene, float x, float y, Direction direction) : base(playScene)
        {
            this.x = x;
            this.y = y;

            imageWidth = 30;
            imageHeight = 5;
            hitboxOffsetLeft = 0;
            hitboxOffsetRight = 0;
            hitboxOffsetTop = 0;
            hitboxOffsetBottom = 0;

            vy = MoveSpeed;
            collisionDamage = 3;
            timer = 0;
        }

        public override void Update()
        {
            y += vy;
            if (playScene.player.GetDirection() ==  Direction.Right) x = playScene.player.GetRight();
            else x = playScene.player.GetLeft() - imageWidth;

            // カメラの範囲外に出たら削除
            if (!Isvisible())
            {
                isDead = true;
            }

            if (y >= playScene.player.GetBottom() - imageHeight)
            {
                isDead = true;
            }

            timer++;
            if (timer > 30) isDead = true;
        }

        public override void Draw()
        {
        }
        public override void OnCollision(GameObject other)
        {
            if (other is Enemy || other is BlockBreakable)
            {
                isDead = true;
            }
        }
    }
}
