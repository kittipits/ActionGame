using MyLib;
using System.Collections.Generic;

namespace ActionGame
{
    public class AI : GameObject
    {
        // 状態種別
        public enum State
        {
            Walk,   // 歩き, 立ち
            Jump,   // ジャンプ中
        }

        const float WalkSpeed = 2.5f;
        const float JumpPower = 14.0f;
        const float Gravity = 0.6f;
        const float MaxFallSpeed = 12.0f;

        float vx = 0;
        float vy = 0;
        State state = State.Walk;
        Direction direction = Direction.Right;
        GameObject groundObject = null;
        GameObject currentTarget = null;

        int objectTimer = 0;
        int shotTimer = 120;
        int animationCounter = 0;
        int[] motion = new int[5] { 1, 2, 1, 0, 1 };
        int motionIndex = 0;

        public AI(PlayScene playScene, float x, float y) : base(playScene)
        {
            this.x = x;
            this.y = y;

            imageWidth = 60;
            imageHeight = 70;
            hitboxOffsetLeft = 20;
            hitboxOffsetRight = 20;
            hitboxOffsetTop = 14;
            hitboxOffsetBottom = 10;
        }


        public override void Update()
        {
            // 入力を受けての処理
            AIControl();

            // 重力による落下
            vy += Gravity;
            if (vy > MaxFallSpeed) vy = MaxFallSpeed;

            // 横移動
            MoveX();

            // 縦移動
            MoveY();

            // 乗っている床の情報を破棄
            groundObject = null;

            if (state == State.Walk)
            {
                animationCounter++;
            }
        }

        void AIControl()
        {
            float posMid = (GetLeft() + GetRight()) / 2;

            if (playScene.player.GetLeft() - GetRight() > 15)
            {
                vx = WalkSpeed;
            }
            else if (GetLeft() - playScene.player.GetRight() > 15)
            {
                vx = -WalkSpeed;

            }
            else
            {
                vx = 0;
            }

            if (playScene.player.GetLeft() > posMid)
            {
                direction = Direction.Right;
            }
            else if (playScene.player.GetRight() < posMid)
            {
                direction = Direction.Left;
            }

            if (state == State.Walk && playScene.player.GetBottom() - GetTop() < 0 && playScene.player.GetBottom() - GetTop() > -80)
            {
                vy -= JumpPower;
                state = State.Jump;
            }

            if (!Isvisible())
            {
                x = playScene.player.x - 10;
                y = playScene.player.y - 10;
            }

            shotTimer--;

            if (currentTarget != null && !currentTarget.isDead && currentTarget.Isvisible())
            {
                // ターゲットが生存している場合、そのまま狙い続ける
                float targetX = (currentTarget.GetLeft() + currentTarget.GetRight()) / 2;
                float targetY = (currentTarget.GetTop() + currentTarget.GetBottom()) / 2;
                float posX = (GetLeft() + GetRight()) / 2;
                float posY = (GetTop() + GetBottom()) / 2;
                float angle = MyMath.PointToPointAngle(posX, posY, targetX, targetY);

                if (shotTimer <= 0)
                {
                    bool randomPierceShot = MyRandom.Percent(50);
                    if (randomPierceShot)
                    {
                        playScene.gameObjects.Add(new AIPierceShot(playScene, posX, posY, angle, 10f));
                    }
                    else
                    {
                        playScene.gameObjects.Add(new AIShot(playScene, posX, posY, angle, 10f));
                    }
                    shotTimer = 120 + (int)MyRandom.PlusMinus(30.0f);
                }

                if (currentTarget.GetLeft() > posMid)
                {
                    direction = Direction.Right;
                }
                else if (currentTarget.GetRight() < posMid)
                {
                    direction = Direction.Left;
                }
            }
            else
            {
                float posX = (GetLeft() + GetRight()) / 2;
                float posY = (GetTop() + GetBottom()) / 2;
                // ターゲットが死んだか、存在しない場合、新しいターゲットを選択する
                currentTarget = SelectNewTarget(playScene.gameObjects, posX, posY);
            }
        }

        void MoveX()
        {
            // 横に移動する
            x += vx;

            // 床オブジェクトに乗っている場合は、その床の移動量だけ移動させる
            if (groundObject != null)
            {
                x += groundObject.GetDeltaX();
            }

            // 当たり判定の四隅の座標を取得
            float left = GetLeft();
            float right = GetRight() - 0.01f;
            float top = GetTop();
            float middle = top + 32;
            float bottom = GetBottom() - 0.01f;

            // 左端が壁にめりこんでいるか？
            if (playScene.map.IsWall(left, top) ||
                playScene.map.IsWall(left, middle) ||
                playScene.map.IsWall(left, bottom))
            {
                float wallRight = left - left % Map.CellSize + Map.CellSize;
                SetLeft(wallRight);     // プレイヤーの左端を壁の右端に沿わす
            }

            // 右端が壁にめりこんでいるか？
            else if (playScene.map.IsWall(right, top) ||
                playScene.map.IsWall(right, middle) ||
                playScene.map.IsWall(right, bottom))
            {
                float wallLeft = right - right % Map.CellSize;
                SetRight(wallLeft);     // プレイヤーの左端を壁の右端に沿わす
            }

        }

        void MoveY()
        {
            // 縦に移動する
            y += vy;

            // 床オブジェクトに乗っている場合は、その床の移動量だけ移動させる
            if (groundObject != null)
            {
                y += groundObject.GetDeltaY();
            }

            // 着地したかどうか
            bool grounded = false;

            // 当たり判定の四隅の座標を取得
            float left = GetLeft();
            float right = GetRight() - 0.01f;
            float top = GetTop();
            float bottom = GetBottom() - 0.01f;

            // 上端が壁にめりこんでいるか？
            if (playScene.map.IsWall(left, top) ||
                playScene.map.IsWall(right, top))
            {
                float wallBottom = top - top % Map.CellSize + Map.CellSize;
                SetTop(wallBottom);     // プレイヤーの頭を天井に沿わす
                vy = 0;
            }
            // 下端が壁にめりこんでいるか？
            else if (playScene.map.IsWall(left, bottom) ||
                playScene.map.IsWall(right, bottom))
            {
                grounded = true;
            }

            // もし着地してたら
            if (grounded)
            {
                float wallTop = bottom - bottom % Map.CellSize;
                SetBottom(wallTop);     // プレイヤーの頭を天井に沿わす
                vy = 0;
                state = State.Walk;
            }
            else
            {
                state = State.Jump;
            }
        }

        public override void Draw()
        {
            motionIndex = motionIndex % 4;
            if (animationCounter % 10 == 0) motionIndex++;
            // 左右反転するか？（左向きなら反転する）
            bool flip = direction == Direction.Right;

            if (state == State.Walk) // 歩き・立ち状態の場合
            {
                if (vx == 0) // 移動していない場合
                {
                    Camera.DrawGraph(x, y, Image.ellanoir[1], flip);
                }
                else // 移動している場合
                {
                    Camera.DrawGraph(x, y, Image.ellanoir[motion[motionIndex]], flip);
                }
            }
            else if (state == State.Jump) // ジャンプ中の場合
            {
                Camera.DrawGraph(x, y, Image.ellanoir[3], flip);
            }
        }

        public override void OnCollision(GameObject other)
        {
            if (other is FloorObject)
            {
                // プレイヤーが下方向から接触したときは何も起きない。
                // 上から床に乗ったときのみ、乗ることができる。
                if (GetPrevBottom() <= other.GetPrevTop())
                {
                    // 着地の処理
                    SetBottom(other.GetTop()); // プレイヤーの足元を床の高さに沿わす
                    vy = 0; // 縦の移動速度を0に
                    state = State.Walk; // 状態を歩きに
                    groundObject = other;
                }
            }
            else if ((other is BlockSpike))
            {
                OnCollisionBlockObject(other);
                x = playScene.player.x - 10;
                y = playScene.player.y - 10;
            }
            else if ((other is BlockObject))
            {
                OnCollisionBlockObject(other);
            }
        }

        void OnCollisionBlockObject(GameObject other)
        {
            // 当たり判定の四隅の座標を取得
            float left = GetLeft();
            float right = GetRight() - 0.01f;
            float top = GetTop();
            float bottom = GetBottom() - 0.01f;

            if (left <= other.GetRight() && left > other.GetRight() - 8.0f)
            {
                SetLeft(other.GetRight());
                vx = 0;
                groundObject = other;
            }
            if (right >= other.GetLeft() && right < other.GetLeft() + 8.0f)
            {
                SetRight(other.GetLeft());
                vx = 0;
                groundObject = other;
            }
            if (top <= other.GetBottom() && top > other.GetBottom() - 12.0f)
            {
                SetTop(other.GetBottom());
                vy = 0;
                groundObject = other;
            }
            if (bottom >= other.GetTop() && bottom < other.GetTop() + 12.0f)
            {
                SetBottom(other.GetTop());
                state = State.Walk;
                groundObject = other;
            }

            if (state == State.Walk)
            {
                objectTimer++;
            }
            if (objectTimer > 10)
            {
                vy = 0;
                objectTimer = 0;
            }
        }

        GameObject SelectNewTarget(List<GameObject> gameObjects, float x, float y)
        {
            GameObject newTarget = null;
            float closestDistance = float.MaxValue;

            foreach (GameObject go in gameObjects)
            {
                if (go is Enemy && !go.isDead)
                {
                    // ターゲットがAIから見えるかどうかを確認
                    bool isVisible = go.Isvisible();

                    if (isVisible)
                    {
                        float enemyPosX = (go.GetLeft() + go.GetRight()) / 2;
                        float enemyPosY = (go.GetTop() + go.GetBottom()) / 2;
                        float posX = x; // AIのX座標
                        float posY = y; // AIのY座標

                        float distance = MyMath.PointToPointDistance(posX, posY, enemyPosX, enemyPosY);

                        if (distance < closestDistance)
                        {
                            newTarget = go;
                            closestDistance = distance;
                        }
                    }
                }
            }

            return newTarget;
        }
    }
}
