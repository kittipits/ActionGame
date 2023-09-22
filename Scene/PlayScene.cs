using System.Collections.Generic;
using DxLibDLL;
using MyLib;

namespace ActionGame
{
    public class PlayScene : Scene
    {
        public enum State
        { 
            Active,
            PlayerDied,
            Goal,
            Clear,
        }

        const int finalLevel = 4;

        public Map map;
        public Player player;
        public List<GameObject> gameObjects = new List<GameObject>();
        public State state = State.Active;
        int timeToGameOver = 120;
        bool isPausing = false;
        public int score;
        public int coin;
        public int time;
        public int level;
        public string mode;
        bool soundCheck;
        string bgm;

        public PlayScene(int level, string mode) 
        {
            this.level = level;
            this.mode = mode;

            map = new Map(this, "stage" + level);
            Camera.LookAt(player.x);
            time = 0;
            score = 0;
            coin = 0;
            soundCheck = false;
        }

        public override void Update()
        {
            time++;
            // ポーズ中の場合
            if (isPausing)
            {
                // STARTボタン（Wキー）が押されたら再開
                if (Input.GetButtonDown(DX.PAD_INPUT_8))
                { 
                    isPausing = false;
                }
                return; // Update()を抜ける
            }

            // 全オブジェクトの更新
            int gameObjectCount = gameObjects.Count;
            for (int i = 0; i < gameObjectCount; i++)
            {
                gameObjects[i].StorePositionAndHitBox();
                gameObjects[i].Update();
            }

            // オブジェクト同士の衝突を判定
            for (int i = 0; i < gameObjects.Count; i++)
            { 
                GameObject a = gameObjects[i];

                for (int j = i + 1; j < gameObjects.Count; j++)
                {
                    if (a.isDead) break;

                    GameObject b = gameObjects[j];
                    if (b.isDead) continue;
                    if (MyMath.RectRectIntersect(
                        a.GetLeft(), a.GetTop(), a.GetRight(), a.GetBottom(),
                        b.GetLeft(), b.GetTop(), b.GetRight(), b.GetBottom()))
                    {
                        a.OnCollision(b);
                        b.OnCollision(a);
                    }
                }
            }
            // 不要となったオブジェクトを除去する
            gameObjects.RemoveAll(go => go.isDead);

            Camera.LookAt(player.x);

            if (state == State.PlayerDied)
            {
                timeToGameOver--;

                if (timeToGameOver <= 0)
                {
                    Game.ChangeScene(new GameOverScene(level, mode));
                }
            }
            if (state == State.Goal)
            {
                Game.ChangeScene(new StageClearScene(level, mode, score, coin));
            }
            if (state == State.Clear)
            {
                timeToGameOver--;

                if (timeToGameOver <= 0)
                {
                    Game.ChangeScene(new GameClearScene(mode, time));
                }
            }
            // STARTボタン（Wキー）が押されたらポーズ
            if (Input.GetButtonDown(DX.PAD_INPUT_8))
            {
                isPausing = true;
            }

            if (level == 4)
            {
                bgm = Sound.bgmBoss;
            }
            else
            {
                bgm = Sound.bgmStage;
            }

            if (!soundCheck)
            {
                DX.StopMusic();
                Sound.PlayMusic(bgm);
                Sound.ChangeBGMVolume();
                soundCheck = true;
            }
        }

        public override void Draw()
        {
            map.DrawTerrain();
            foreach (GameObject go in gameObjects)
            {
                go.Draw();
            }

            if (isPausing)
            {
                // 半透明の指定。第2引数で0～255でアルファ値（不透明度）を指定する。
                // 不透明度を変えたら、明示的に元に戻すまでは継続されるので注意
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, 80);
                // 画面全体を黒で塗りつぶす
                DX.DrawBox(0, 0, Screen.Width, Screen.Height, DX.GetColor(0, 0, 0), DX.TRUE);
                // 不透明度を元に戻す
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, 255);

                DrawStringCenter(100, "操作方法", DX.GetColor(255, 255, 255));
                DrawStringCenter(150, "方向ボタン ／ 方向キー　：　移動", DX.GetColor(255, 255, 255));
                DrawStringCenter(180, "Ⓐボタン ／ Zキー　：　（長押し）遠距離攻撃", DX.GetColor(255, 255, 255));
                DrawStringCenter(210, "Ⓑボタン ／ Xキー　：　ジャンプ", DX.GetColor(255, 255, 255));
                DrawStringCenter(240, "Ⓧボタン ／ Cキー　：　近距離攻撃", DX.GetColor(255, 255, 255));
                DrawStringCenter(270, "Ⓨボタン ／ Aキー　：　ターゲットロック・解除", DX.GetColor(255, 255, 255));
                DrawStringCenter(300, "STARTボタン ／ Wキー　：　ポース", DX.GetColor(255, 255, 255));
            }

            //foreach (GameObject go in gameObjects)
            //{
            //    go.DrawHitBox();
            //}

            player.DrawTargetedEnemy();

            //HPのUIを表示
            DX.DrawBoxAA(20, 15, 80, 32, DX.GetColor(0, 0, 0), DX.TRUE);
            DX.DrawBoxAA(80, 15, 80 + player.GetLife() * 10, 32, DX.GetColor(191, 0, 0), DX.TRUE);
            DX.DrawString(42, 15, "HP", DX.GetColor(255, 255, 255));

            //SPEEDのUIを表示
            DX.DrawBoxAA(20, 33, 80, 50, DX.GetColor(0, 0, 0), DX.TRUE);
            DX.DrawBoxAA(80, 38, 80 + player.GetSpeedBoostTimer() / 24, 45, DX.GetColor(127, 0, 191), DX.TRUE);
            DX.DrawString(27, 33, "SPEED", DX.GetColor(255, 255, 255));

            if (player.GetSpeedBoostTimer() > 0)
            {
                int sec = player.GetSpeedBoostTimer() / 60;
                DX.DrawString(233, 33, sec.ToString("00"), DX.GetColor(255, 255, 255));
            }
            
            //SHIELDのUIはPlayerShieldクラスで表示

            if (level != 4)
            {
                DX.DrawBoxAA(320, 15, 400, 32, DX.GetColor(0, 0, 0), DX.TRUE);
                DX.DrawString(330, 15, "COIN X" + coin, DX.GetColor(255, 255, 255));

                DX.DrawBoxAA(490, 15, 620, 32, DX.GetColor(0, 0, 0), DX.TRUE);
                DX.DrawString(500, 15, "SCORE: " + score, DX.GetColor(255, 255, 255));
            }
            else
            {
                int sec = (time / 60) % 60;
                int min = time / 3600;
                DX.DrawBoxAA(490, 15, 620, 32, DX.GetColor(0, 0, 0), DX.TRUE);
                DX.DrawString(500, 15, "TIME: " + min.ToString("00") + "." + sec.ToString("00") , DX.GetColor(255, 255, 255));
            }
        }
    }
}
