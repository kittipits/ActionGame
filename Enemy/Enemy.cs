using DxLibDLL;

namespace ActionGame
{
    public abstract class Enemy : GameObject
    {
        protected int maxLife;
        protected int life;
        protected float flashTimer = 0;
        protected bool isDamaged = false;

        public Enemy(PlayScene playScene) : base(playScene)
        {
        }

        public override void Update()
        {
            if (isDamaged) flashTimer++;
            if (flashTimer >= 45)
            {
                isDamaged = false;
                flashTimer = 0;
            }

            if (life <= 0)
            {
                Sound.Play(Sound.seSparkling);
                isDead = true;
                ScoreUp();
                Explode();
            }
        }

        public override void Draw()
        {
        }

        public override void OnCollision(GameObject other)
        {
            if (other is PlayerShot || other is PlayerSword)
            {
                TakeDamage(other.collisionDamage);
            }
        }

        public override void TakeDamage(int damage) 
        {
            life -= damage;
            isDamaged = true;
        }

        protected void DrawFlash(int image)
        {
            bool flip = false;
            float playerMid = (playScene.player.GetLeft() + playScene.player.GetRight()) / 2;
            float enemyMid = (GetLeft() + GetRight()) / 2;
            if (playerMid > enemyMid) flip = true;
            else flip = false;

            if (isDamaged && flashTimer % 30 <= 15)
            {
                DX.SetDrawBright(255, 0, 0);
            }

            Camera.DrawGraph(x, y, image, flip);

            if (isDamaged && flashTimer % 30 <= 15)
            {
                DX.SetDrawBright(255, 255, 255);
            }
        }

        protected virtual void ScoreUp() 
        { 
        }
    }
}
