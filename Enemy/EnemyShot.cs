namespace ActionGame
{
    public abstract class EnemyShot : GameObject
    {
        protected int collisionRadius;

        public EnemyShot(PlayScene playScene) : base(playScene)
        {
        }

        public override void Update()
        {
            // カメラの範囲外に出たら削除
            if (!Isvisible())
            {
                isDead = true;
            }
        }

        public override void Draw()
        {
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player || other is PlayerShield || other is PlayerSword)
            {
                isDead = true;
            }
        }
    }
}
