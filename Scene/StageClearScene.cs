using DxLibDLL;
using MyLib;

namespace ActionGame
{
    public class StageClearScene : Scene
    {
        int level;
        int score;
        int coin;
        int bigFont;
        int smallFont;
        int counter = 0;
        string mode;
        bool soundCheck;

        public StageClearScene(int level, string mode, int score , int coin)
        { 
            this.level = level;
            this.score = score;
            this.coin = coin;
            this.mode = mode;

            smallFont = DX.CreateFontToHandle(null, 20, -1);
            bigFont = DX.CreateFontToHandle(null, 40, -1);

            Sound.PlayMusic(Sound.bgmClear);
            soundCheck = false;
        }
        public override void Update()
        {
            counter++;

            if (Input.GetButtonDown(DX.PAD_INPUT_1))
            {
                if (mode == "story")
                {
                    Sound.Play(Sound.seStart);
                    Game.ChangeScene(new StoryScene(level, "play"));
                }
                else if (mode == "arcade")
                {
                    Sound.Play(Sound.seSelect);
                    Game.ChangeScene(new StageSelectScene());
                }
            }
            if (Input.GetButtonDown(DX.PAD_INPUT_2))
            {
                Sound.Play(Sound.seSelect);
                Game.ChangeScene(new TitleScene());
            }

            if (!soundCheck)
            {
                Sound.ChangeBGMVolume();
                soundCheck = true;
            }
        }

        public override void Draw()
        {
            DX.DrawGraph(0, 0, Image.stageclear);

            DrawStringCenterToHandle(300, "SCORE : " + score, DX.GetColor(255, 255, 255), bigFont);
            DrawStringCenterToHandle(360, "COIN : " + coin, DX.GetColor(255, 255, 255), bigFont);
            if (counter % 60 > 20)
            {
                if (mode == "story")
                {
                    DrawStringCenterToHandle(450, "Ⓐ（Zキー）：次のステージへ　　Ⓑ（Xキー）：タイトルへ", DX.GetColor(127, 127, 127), smallFont);
                }
                else if (mode == "arcade")
                {
                    DrawStringCenterToHandle(450, "Ⓐ（Zキー）：ステージ選択へ　　Ⓑ（Xキー）：タイトルへ", DX.GetColor(127, 127, 127), smallFont);
                }
            }
        }
    }
}