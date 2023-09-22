namespace ActionGame
{
    public class ItemShieldBoost : Item
    {
        public ItemShieldBoost(PlayScene playScene, float x, float y) : base(playScene)
        {
            this.x = x;
            this.y = y;

            imageWidth = 32;
            imageHeight = 32;
            hitboxOffsetLeft = 4;
            hitboxOffsetRight = 4;
            hitboxOffsetTop = 4;
            hitboxOffsetBottom = 4;
        }

        public override void Update()
        {
        }

        public override void Draw()
        {
            Camera.DrawGraph(x, y, Image.itemShieldUp);
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player)
            {
                Explode();
                Sound.Play(Sound.seCoin);
                playScene.player.ShieldBoost();
                isDead = true;
            }
        }

    }
}
