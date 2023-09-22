using System;

namespace ActionGame
{
    public class FloorObjectTwo: FloorObject
    {
        float theta;
        float vx;
        float vy;

        public FloorObjectTwo(PlayScene playScene, float x, float y) : base(playScene)
        {
            this.x = x;
            this.y = y;
            
            imageWidth = 32 * 3;
            imageHeight = 32;
            hitboxOffsetLeft = 0;
            hitboxOffsetRight = 0;
            hitboxOffsetTop = 0;
            hitboxOffsetBottom = 0;
            
            theta = 0;
            vx = 0;
            vy = 0;
        }

        public override void Update()
        {
            theta += 0.05f;
            vx = (float)Math.Cos(theta / 4);
            vy = (float)Math.Sin(theta / 4);

            x += vx;
            y += vy;
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
