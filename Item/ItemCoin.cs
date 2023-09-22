namespace ActionGame
{
    public class ItemCoin : Item
    {
        public ItemCoin(PlayScene playScene, float x, float y) : base(playScene)
        {
            this.x = x;
            this.y = y;

            imageWidth = 32;
            imageHeight = 32;
            hitboxOffsetLeft = 7;
            hitboxOffsetRight = 7;
            hitboxOffsetTop = 4;
            hitboxOffsetBottom = 4;
        }

        public override void Update()
        {
        }

        public override void Draw()
        {
            Camera.DrawGraph(x, y, Image.itemCoin);
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player)
            {
                Explode();
                Sound.Play(Sound.seCoin);
                playScene.coin++;
                isDead = true;
            }
        }

    }
}
