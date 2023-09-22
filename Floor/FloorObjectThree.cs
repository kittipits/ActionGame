namespace ActionGame
{
    public class FloorObjectThree: FloorObject
    {
        public FloorObjectThree(PlayScene playScene, float x, float y) : base(playScene)
        {
            this.x = x;
            this.y = y;
            
            imageWidth = 32 * 3;
            imageHeight = 32;
            hitboxOffsetLeft = 0;
            hitboxOffsetRight = 0;
            hitboxOffsetTop = 0;
            hitboxOffsetBottom = 0;
            
        }

        public override void Update()
        {
            y -= 0.5f;

            if (y <= -imageHeight)
            {
                y = Screen.Height;
            }
        }

        public override void Draw()
        {
            Camera.DrawGraph(x, y, Image.FloorObject);
        }

        public override void OnCollision(GameObject other)
        {
        }
    }
}
