namespace ActionGame
{
    public class PlayerShot : GameObject
    {
        public PlayerShot(PlayScene playScene) : base(playScene)
        { 
        }

        public override void Update()
        {
            // カメラの範囲外に出たら削除
            if (!Isvisible())
            { 
                isDead = true;
            }
        }

        public override void Draw()
        {
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Enemy || other is FloorObject || other is BlockObject) 
            {
                isDead = true;
            }
        }

        protected void HitOnWall()
        {
            // 当たり判定の四隅の座標を取得
            float left = GetLeft();
            float right = GetRight() - 0.01f;
            float top = GetTop();
            float bottom = GetBottom() - 0.01f;

            // 壁にめりこんでいるか？
            if (playScene.map.IsWall(left, top) ||
                playScene.map.IsWall(left, bottom) ||
                playScene.map.IsWall(right, top) ||
                playScene.map.IsWall(right, bottom))
            {
                isDead = true;
            }
        }
    }
}
