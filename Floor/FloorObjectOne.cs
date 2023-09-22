namespace ActionGame
{
    public class FloorObjectOne: FloorObject
    {
        int counter;

        public FloorObjectOne(PlayScene playScene, float x, float y) : base(playScene)
        {
            this.x = x;
            this.y = y;
            
            imageWidth = 32 * 3;
            imageHeight = 32;
            hitboxOffsetLeft = 0;
            hitboxOffsetRight = 0;
            hitboxOffsetTop = 0;
            hitboxOffsetBottom = 0;
            counter = 0;
        }

        public override void Update()
        {
            if (counter < 100)
            {
                x += 0.5f;
                counter++;
            }
            else if (counter < 200)
            {
                y += 0.5f;
                counter++;
            }
            else if (counter < 300)
            {
                x -= 0.5f;
                counter++;
            }
            else if (counter < 400)
            {
                y -= 0.5f;
                counter++;
            }

            if (counter == 400)
            {
                counter = 0;
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
