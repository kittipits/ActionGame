namespace ActionGame
{
    public class BlockItemShield: BlockObject
    {

        public BlockItemShield(PlayScene playScene, float x, float y) : base(playScene)
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
        }

        public override void Draw()
        {
            Camera.DrawGraph(x, y, Image.itemblock);
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player)
            {
                if (other.GetTop() >= GetBottom() - 8f)
                { 
                    playScene.gameObjects.Add(new ItemShieldBoost(playScene, x, y - 32));
                    playScene.gameObjects.Add(new BlockBlank(playScene, x, y));
                    playScene.gameObjects.Add(new Sparkling(playScene, x, y - 32));
                    Sound.Play(Sound.seCoin);
                    isDead = true;
                }
            }
        }
    }
}
