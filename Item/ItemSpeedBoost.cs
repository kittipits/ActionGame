namespace ActionGame
{
    public class ItemSpeedBoost : Item
    {
        public ItemSpeedBoost(PlayScene playScene, float x, float y) : base(playScene)
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
            Camera.DrawGraph(x, y, Image.itemSpeedUp);
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player)
            {
                Explode();
                Sound.Play(Sound.seCoin);
                playScene.player.SpeedBoost();
                isDead = true;
            }
        }

    }
}
