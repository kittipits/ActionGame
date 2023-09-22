namespace ActionGame
{
    public abstract class PlayerSword : GameObject
    {
        public PlayerSword(PlayScene playScene) : base(playScene)
        {
        }

        public override void Update()
        {
        }

        public override void Draw()
        {
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
