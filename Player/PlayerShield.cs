using DxLibDLL;

namespace ActionGame
{
    public class PlayerShield : GameObject
    {
        int radius;
        int time;

        public PlayerShield(PlayScene playScene, float x, float y, int time) : base(playScene)
        {
            this.x = x;
            this.y = y;

            imageWidth = 68;
            imageHeight = 78;
            hitboxOffsetLeft = 0;
            hitboxOffsetRight = 0;
            hitboxOffsetTop = 0;
            hitboxOffsetBottom = 0;
            radius = 42;
            this.time = time;
         }

        public override void Update()
        {
            time--;

            x = playScene.player.GetLeft() - 24.0f;
            y = playScene.player.GetTop() - 16.0f;

            if (time < 0)
            { 
                isDead = true;
            }

        }

        public override void Draw()
        {
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, 63);
            float midX = (GetLeft() + GetRight()) / 2;
            float midY = (GetTop() + GetBottom()) / 2;
            Camera.DrawCircle((int)midX, (int)midY, radius, DX.GetColor(0, 127, 191));
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 63);

            DX.DrawBoxAA(20, 51, 80, 68, DX.GetColor(0, 0, 0), DX.TRUE);
            DX.DrawBoxAA(80, 56, 80 + time / 24, 63, DX.GetColor(0, 0, 191), DX.TRUE);
            DX.DrawString(23, 51, "SHIELD", DX.GetColor(255, 255, 255));

            if (time > 0)
            {
                int sec = time / 60;
                DX.DrawString(233, 51, sec.ToString("00"), DX.GetColor(255, 255, 255));
            }
        }

        public override void OnCollision(GameObject other)
        {
            if (other is EnemyShotHoming)
            {
                isDead = true;
            }
        }
    }
}
