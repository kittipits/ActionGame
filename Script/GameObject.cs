using DxLibDLL;
using MyLib;

namespace ActionGame
{
    // ゲーム上に表示される物体の基底クラス。
    // プレイヤーや敵、アイテムなどはこのクラスを継承して作る。
    public abstract class GameObject
    {
        public float x;                 // x座標
        public float y;                 // y座標
        public bool isDead = false;     // 死亡フラグ
        //タグ
        public int collisionDamage;

        protected PlayScene playScene;
        // 画像のピクセル数
        protected int imageWidth;
        protected int imageHeight;
        // 当たり判定のオフセット
        protected int hitboxOffsetLeft = 0;
        protected int hitboxOffsetRight = 0;
        protected int hitboxOffsetTop = 0;
        protected int hitboxOffsetBottom = 0;


        // 1フレーム前
        float prevX;
        float prevY;
        float prevLeft;
        float prevRight;
        float prevTop;
        float prevBottom;

        // コンストラクタ
        public GameObject(PlayScene playScene)
        {
            this.playScene = playScene;
        }

        // 当たり判定の取得と設定
        // 左端
        public virtual float GetLeft () 
        { 
            return x + hitboxOffsetLeft; 
        }
        public virtual void SetLeft (float left) 
        {
            x = left - hitboxOffsetLeft;
        }
        // 右端
        public virtual float GetRight()
        {
            return x + imageWidth - hitboxOffsetRight;
        }
        public virtual void SetRight(float right)
        {
            x = right + hitboxOffsetRight - imageWidth;
        }
        // 上端
        public virtual float GetTop()
        {
            return y + hitboxOffsetTop;
        }
        public virtual void SetTop(float top)
        {
            y = top - hitboxOffsetTop;
        }
        // 下端
        public virtual float GetBottom()
        {
            return y + imageHeight - hitboxOffsetBottom;
        }
        public virtual void SetBottom(float bottom)
        {
            y = bottom + hitboxOffsetBottom - imageHeight;
        }

        // 1フレーム前からの移動量
        public float GetDeltaX()
        {
            return x - prevX;
        }
        public float GetDeltaY()
        {
            return y - prevY;
        }
        // 1フレーム前の端を取得する
        public float GetPrevLeft()
        { 
            return prevLeft;
        }
        public float GetPrevRight()
        {
            return prevRight;
        }
        public float GetPrevTop()
        {
            return prevTop;
        }
        public float GetPrevBottom()
        {
            return prevBottom;
        }
        // 1フレーム前の場所と当たり判定を記憶する
        public void StorePositionAndHitBox()
        { 
            prevX = x; 
            prevY = y;
            prevLeft = GetLeft();
            prevRight = GetRight();
            prevTop = GetTop();
            prevBottom = GetBottom();
        }

        // 更新
        public abstract void Update();

        //描画
        public abstract void Draw();

        // 当たり判定を描画（デバッグ用）
        public void DrawHitBox()
        {
            // 四角形を描画
            Camera.DrawLineBox(GetLeft(), GetTop(), GetRight(), GetBottom(), DX.GetColor(255, 0, 0));
        }

        // 他のオブジェクトと衝突したときに呼ばれる
        public abstract void OnCollision(GameObject other);

        public virtual bool Isvisible()
        {
            return MyMath.RectRectIntersect(
                x, y, x + imageWidth, y + imageHeight,
                Camera.x, Camera.y, Camera.x + Screen.Width, Camera.y + Screen.Height);
        }

        public virtual void TakeDamage(int damage)
        {
        }

        public virtual void Explode()
        {
            playScene.gameObjects.Add(new Sparkling(playScene, x, y));
        }
    }
}
