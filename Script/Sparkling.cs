namespace ActionGame
{
    public class Sparkling : GameObject
    {
        int counter = 0;
        int imageIndex = 0;

        public Sparkling(PlayScene playScene, float x, float y) : base(playScene)
        {
            this.x = x;
            this.y = y;

            imageWidth = 32;
            imageHeight = 32;
            hitboxOffsetLeft = 0;
            hitboxOffsetRight = 0;
            hitboxOffsetTop = 0;
            hitboxOffsetBottom = 0;
        }

        public override void Update()
        {
            counter++;

            imageIndex = counter / 8;

            if (imageIndex >= 7)
            {
                isDead = true;
            }
        }

        public override void Draw()
        {
            Camera.DrawGraph(x, y, Image.sparkling[imageIndex]);
        }

        public override void OnCollision(GameObject other)
        {
        }

    }
}
