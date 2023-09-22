using DxLibDLL;
using MyLib;
using System.Linq;

namespace ActionGame
{
    public class Player : GameObject
    {
        // 状態種別
        public enum State
        {
            Walk,   // 歩き, 立ち
            Jump,   // ジャンプ中
        }

        //検コンボ
        public enum Combo
        { 
            None,
            First, 
            Second, 
            Third,
        }

        const float WalkSpeed = 3f;
        const float JumpPower = 13f;
        const float Gravity = 0.6f;
        const float MaxFallSpeed = 12f;
        const int InvulCountdown = 90; //無敵時間
        const int maxLife = 15;

        float vx = 0;
        float vy = 0;
        State state = State.Walk;
        Combo combo = Combo.None;
        Direction direction = Direction.Right;
        GameObject groundObject = null;
        GameObject currentTarget = null;
        int life;
        float speedBoost = 0;

        bool isAttacking; // 攻撃中かどうかを示すフラグ
        bool isTargetCleared = true;
        int attackTimer;
        int comboTimer;
        int invulTimer = 0;
        int objectTimer = 0;
        int speedBoostTimer = 0;
        int shotCounter = 0;

        int animationCounter = 0;
        int[] playerMotionRight = new int[5] { 1, 2, 1, 0, 1 };
        int[] playerMotionLeft = new int[5] { 5, 6, 5, 4, 5 };
        int motionIndex = 0;

        int[] comboMotionRight = new int[6] { 8, 8, 9, 10, 11, 11 };
        int[] comboMotionLeft = new int[6] { 12, 12, 13, 14, 15, 15 };
        int comboMotionIndexRight = 0;
        int comboMotionIndexLeft = 0;

        public Player(PlayScene playScene, float x, float y) : base(playScene)
        {
            this.x = x;
            this.y = y;

            imageWidth = 60;
            imageHeight = 70;
            hitboxOffsetLeft = 20;
            hitboxOffsetRight = 20;
            hitboxOffsetTop = 14;
            hitboxOffsetBottom = 10;

            isAttacking = false;
            attackTimer = 0;
            comboTimer = 0;

            life = maxLife;
        }


        public override void Update()
        {
            // 入力を受けての処理
            HandleInput();

            // 重力による落下
            vy += Gravity;
            if (vy > MaxFallSpeed) vy = MaxFallSpeed;

            // 横移動
            MoveX();

            // 縦移動
            MoveY();

            // 乗っている床の情報を破棄
            groundObject = null;

            // 検の攻撃
            Attack();
            // フラグ処理
            attackTimer++;
            if (isAttacking && attackTimer > 45)
            {
                isAttacking = false;
                attackTimer = 0;
            }
            //コンボ処理
            comboTimer++;
            if ((comboTimer > 45 && combo == Combo.Third) ||
                comboTimer > 60)
            {
                comboTimer = 0;
                combo = Combo.None;
            }

            if (state == State.Walk)
            {
                if (Input.GetButton(DX.PAD_INPUT_LEFT) || Input.GetButton(DX.PAD_INPUT_RIGHT) ||
                    Input.GetButton(DX.PAD_INPUT_UP) || Input.GetButton(DX.PAD_INPUT_DOWN))
                {
                    animationCounter++;
                }
                else
                {
                    animationCounter = 9;
                }
            }

            invulTimer--;

            speedBoostTimer--;
            if (speedBoost >= 3f) speedBoost = 3f;
            if (speedBoostTimer < 0) speedBoost = 0f;

            PlayerInGoal();
        }

        void HandleInput()
        {
            // 左が押されてたら、速度を左へ
            if (Input.GetButton(DX.PAD_INPUT_LEFT) && !isAttacking)
            {
                vx = -WalkSpeed - speedBoost;
                direction = Direction.Left;
            }
            // 右が押されてたら、速度を右へ
            else if (Input.GetButton(DX.PAD_INPUT_RIGHT) && !isAttacking)
            {
                vx = WalkSpeed + speedBoost;
                direction = Direction.Right;
            }
            // 左も右も押されてなければ、速度は0にする
            else
            {
                vx = 0;
            }

            // 歩き状態 かつ Bボタンが押されたら ジャンプする
            if (state == State.Walk && Input.GetButtonDown(DX.PAD_INPUT_2))
            {
                vy -= JumpPower;
                state = State.Jump;
            }

            // ジャンプ中 かつ 上昇中 かつ ジャンプボタンが離されたら
            if ((state == State.Jump) && vy < 0 && !Input.GetButton(DX.PAD_INPUT_2))
            {
                // 上昇を中止
                vy = 0;
            }

            shotCounter++;
            // ショットボタンが押されたら
            if (!isAttacking && isTargetCleared && shotCounter % 10 == 0 && Input.GetButton(DX.PAD_INPUT_1))
            {
                // ショットのx座標を求める
                float shotX;
                if (direction == Direction.Left) shotX = GetLeft() - 16;
                else shotX = GetRight();
                // ショットのy座標
                float shotY = y + 28;
                // 発射
                playScene.gameObjects.Add(new PlayerNormalShot(playScene, shotX, shotY, direction));

                Sound.Play(Sound.seShoot);
            }
            else if (!isAttacking && !isTargetCleared && Input.GetButton(DX.PAD_INPUT_1))
            {
                float posMid = (GetLeft() + GetRight()) / 2;
                if (currentTarget.GetLeft() > posMid)
                {
                    direction = Direction.Right;
                }
                else if (currentTarget.GetRight() < posMid)
                {
                    direction = Direction.Left;
                }
                if (shotCounter % 10 == 0)
                {
                    // ターゲットが生存している場合、そのまま狙い続ける
                    float targetX = (currentTarget.GetLeft() + currentTarget.GetRight()) / 2;
                    float targetY = (currentTarget.GetTop() + currentTarget.GetBottom()) / 2;
                    float posX = (GetLeft() + GetRight()) / 2;
                    float posY = (GetTop() + GetBottom()) / 2;
                    float angle = MyMath.PointToPointAngle(posX, posY, targetX, targetY);
                    playScene.gameObjects.Add(new PlayerTargetShot(playScene, posX, posY, angle, 10f));

                    Sound.Play(Sound.seShoot);
                }
            }


            if (Input.GetButtonDown(DX.PAD_INPUT_4))
            {
                if (isTargetCleared)
                {
                    // ターゲットが解除されている場合、新しいターゲットを選択する
                    currentTarget = SelectNewTarget();
                    isTargetCleared = false; // ターゲットが再び選択されたことを示す
                }
                else
                {
                    // ターゲットが解除されていない場合、ターゲットを解除する
                    currentTarget = null;
                    isTargetCleared = true; // ターゲットが解除されたことを示す
                }
            }
        }

        void Attack()
        {
            if (!isAttacking && combo == Combo.None)
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_3))
                {
                    isAttacking = true;
                    comboTimer = 0;
                    combo++;
                    // 検のx座標
                    float swordX;
                    if (direction == Direction.Left) swordX = GetLeft() - hitboxOffsetLeft;
                    else swordX = GetRight();
                    // 検のy座標
                    float swordY = GetTop();
                    // 発射
                    playScene.gameObjects.Add(new PlayerSwordComboFirst(playScene, swordX, swordY, direction));

                    // 攻撃アニメーションの処理
                    comboMotionIndexRight = 1;
                    comboMotionIndexLeft = 4;

                    Sound.Play(Sound.seSword1);
                }
            }
            else if (!isAttacking && combo == Combo.First)
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_3))
                {
                    isAttacking = true;
                    comboTimer = 0;
                    combo++;
                    // 検のx座標
                    float swordX;
                    if (direction == Direction.Left) swordX = GetLeft() - hitboxOffsetLeft;
                    else swordX = GetRight();
                    // 検のy座標
                    float swordY = GetBottom() - 5f;
                    // 発射
                    playScene.gameObjects.Add(new PlayerSwordComboSecond(playScene, swordX, swordY, direction));

                    // 攻撃アニメーションの処理
                    comboMotionIndexRight = 4;
                    comboMotionIndexLeft = 1;

                    Sound.Play(Sound.seSword1);
                }
            }
            else if (!isAttacking && combo == Combo.Second)
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_3))
                {
                    isAttacking = true;
                    comboTimer = 0;
                    combo++;
                    // 検のx座標
                    float swordX;
                    if (direction == Direction.Left) swordX = GetLeft() - hitboxOffsetLeft;
                    else swordX = GetRight();
                    // 検のy座標
                    float swordY = y + 28f;
                    // 発射
                    playScene.gameObjects.Add(new PlayerSwordComboThird(playScene, swordX, swordY, direction));

                    // 攻撃アニメーションの処理
                    comboMotionIndexRight = 1;
                    comboMotionIndexLeft = 4;

                    Sound.Play(Sound.seSword2);
                }
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
            bool flip = false;

            if (invulTimer > 0  && invulTimer / 5 % 4 == 0)
            {
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, 128);
            }

            if (!isAttacking && combo == Combo.First)
            {
                if (comboTimer % 6 == 0)
                {
                    comboMotionIndexRight++;
                    comboMotionIndexLeft--;
                }
                if (comboMotionIndexRight > 4) comboMotionIndexRight = 4;
                if (comboMotionIndexLeft < 1) comboMotionIndexLeft = 1;
                if (direction == Direction.Right) Camera.DrawGraph(x, y, Image.azalea[comboMotionRight[comboMotionIndexRight]], flip);
                else if (direction == Direction.Left) Camera.DrawGraph(x, y, Image.azalea[comboMotionLeft[comboMotionIndexLeft]], flip);
            }
            else if (isAttacking && (combo == Combo.First || combo == Combo.Third))
            {
                if (comboTimer % 6 == 0)
                {
                    comboMotionIndexRight++;
                    comboMotionIndexLeft--;
                }
                if (comboMotionIndexRight > 4) comboMotionIndexRight = 4;
                if (comboMotionIndexLeft < 1) comboMotionIndexLeft = 1;
                if (direction == Direction.Right) Camera.DrawGraph(x, y, Image.azalea[comboMotionRight[comboMotionIndexRight]], flip);
                else if (direction == Direction.Left) Camera.DrawGraph(x, y, Image.azalea[comboMotionLeft[comboMotionIndexLeft]], flip);
            }
            else if (isAttacking && combo == Combo.Second)
            {
                if (comboTimer % 5 == 0)
                {
                    comboMotionIndexRight--;
                    comboMotionIndexLeft++;
                }
                if (comboMotionIndexRight < 1) comboMotionIndexRight = 1;
                if (comboMotionIndexLeft > 4) comboMotionIndexLeft = 4;
                if (direction == Direction.Right) Camera.DrawGraph(x, y, Image.azalea[comboMotionRight[comboMotionIndexRight]], flip);
                else if (direction == Direction.Left) Camera.DrawGraph(x, y, Image.azalea[comboMotionLeft[comboMotionIndexLeft]], flip);
            }
            else if (state == State.Walk && !isAttacking)
            {
                motionIndex = motionIndex % 4;
                if (animationCounter % 10 == 0) motionIndex++;
                if (direction == Direction.Right && vx == 0) Camera.DrawGraph(x, y, Image.azalea[1], flip);
                else if (direction == Direction.Left && vx == 0) Camera.DrawGraph(x, y, Image.azalea[5], flip);
                else if (direction == Direction.Right)  Camera.DrawGraph(x, y, Image.azalea[playerMotionRight[motionIndex]], flip);
                else if (direction == Direction.Left) Camera.DrawGraph(x, y, Image.azalea[playerMotionLeft[motionIndex]], flip);
            }
            else if (state == State.Jump && !isAttacking)
            {
                if (direction == Direction.Right)   Camera.DrawGraph(x, y, Image.azalea[3], flip);
                else if (direction == Direction.Left) Camera.DrawGraph(x, y, Image.azalea[7], flip);
            }

            if (invulTimer > 0 && invulTimer / 5 % 2 == 0)
            {
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 128);
            }
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Enemy || other is EnemyShot)
            {
                if (invulTimer < 0)
                {
                    TakeDamage();
                }
            }
            else if (other is FloorObject)
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
                TakeDamage();
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
            float objectMidX = (other.GetLeft() + other.GetRight()) / 2;
            float objectMidY = (other.GetTop() + other.GetBottom()) / 2;

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

        void TakeDamage()
        {
            life -= 1;

            if (life <= 0)
            {
                Die();
            }
            else
            {
                invulTimer = InvulCountdown;
            }
        }

        public void Die()
        {
            Sound.Play(Sound.seSparkling);
            isDead = true;
            playScene.state = PlayScene.State.PlayerDied;
            for (int i = 0; i < 30; i++)
            {
                Explode();
            }
        }

        public void RecoverHP()
        {
            life += 1;

            if (life >= maxLife) life = maxLife;
        }
        public void ShieldBoost() 
        {
            float posX = GetLeft() - 24.0f;
            float posY = GetTop() - 16.0f;
            playScene.gameObjects.Add(new PlayerShield(playScene, posX, posY, 3600));
        }
        public void SpeedBoost()
        {
            speedBoostTimer = 3600;
            speedBoost = 3.0f;
        }
        public int GetSpeedBoostTimer() 
        {
            return speedBoostTimer;
        }

        public int GetLife()
        { 
            return life;
        }

        public Direction GetDirection()
        {
            return direction;
        }

        public override void Explode()
        {
            float explosionX = MyRandom.Range(GetLeft() - 32, GetRight());
            float explosionY = MyRandom.Range(GetTop() - 32, GetBottom());

            playScene.gameObjects.Add(new Sparkling(playScene, explosionX, explosionY));
        }

        void PlayerInGoal() 
        {
            float left = GetLeft();
            float right = GetRight() - 0.01f;
            float top = GetTop();
            float bottom = GetBottom() - 0.01f;

            if (playScene.map.IsGoal(left, top) || playScene.map.IsGoal(left, bottom) ||
                playScene.map.IsGoal(right, top) || playScene.map.IsGoal(right, bottom))
            {
                playScene.state = PlayScene.State.Goal;
            }
        }

        GameObject SelectNewTarget()
        {
            if (currentTarget != null)
            {
                // 現在のターゲットがいる場合、ターゲットを解除する
                currentTarget = null;
                isTargetCleared = true; // ターゲットが解除されたことを示す
            }

            foreach (GameObject go in playScene.gameObjects)
            {
                if (go is Enemy && !go.isDead && go.Isvisible())
                {
                    return go;
                }
            }

            // 敵全体を狙ったことがある場合、最初のターゲットを再び選択する
            return playScene.gameObjects.FirstOrDefault(go => go is Enemy && !go.isDead);
        }

        public void DrawTargetedEnemy()
        {
            if (currentTarget != null)
            {
                float targetX = (currentTarget.GetLeft() + currentTarget.GetRight()) / 2;
                float targetY = (currentTarget.GetTop() + currentTarget.GetBottom()) / 2;

                Camera.DrawGraph(targetX - 24.0f, targetY - 24.0f, Image.crosshair);
            }
        }
    }
}
