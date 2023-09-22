namespace ActionGame
{
    public class BlockBreakable: BlockObject
    {
        const int maxLife = 10;
        int life;

        public BlockBreakable(PlayScene playScene, float x, float y) : base(playScene)
        {
            this.x = x;
            this.y = y;

            imageWidth = 32;
            imageHeight = 32;
            hitboxOffsetLeft = 0;
            hitboxOffsetRight = 0;
            hitboxOffsetTop = 0;
            hitboxOffsetBottom = 0;

            life = maxLife;
        }

        public override void Update()
        {
            if (life <= 0)
            {
                Explode();
                isDead = true;
            }
        }

        public override void Draw()
        {
            Camera.DrawGraph(x, y, Image.breakable);
        }

        public override void OnCollision(GameObject other)
        {
            if (other is PlayerShot || other is PlayerSword)
            {
                Sound.Play(Sound.seBroke1);
                TakeDamage(other.collisionDamage);
            }
            if (other is Player)
            {
                if (other.GetTop() >= GetBottom() - 8f)
                {
                    Sound.Play(Sound.seBroke1);
                    Explode();
                    isDead = true;
                }
            }
        }

        public override void TakeDamage(int damage)
        {
            life -= damage;
        }
    }
}
