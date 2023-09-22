using DxLibDLL;
using MyLib;
using System.Reflection.Emit;

namespace ActionGame
{
    public class GameClearScene : Scene
    {
        int time;
        int smallFont;
        int bigFont;
        int counter = 0;
        string mode;

        bool soundCheck;

        public GameClearScene(string mode, int time)
        {
            this.time = time;
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
                    Game.ChangeScene(new StoryScene(4, "play"));
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
            DX.DrawGraph(0, 0, Image.gameclear);

            int sec = (time / 60) % 60;
            int min = time / 3600;
            DrawStringCenterToHandle(330, "TIME: " + min.ToString("00") + "." + sec.ToString("00"), DX.GetColor(255, 255, 255), bigFont);
            if (counter % 60 > 20)
            {
                DrawStringCenterToHandle(450, "Ⓐ（Zキー）：次へ　　Ⓑ（Xキー）：タイトルへ", DX.GetColor(127, 127, 127), smallFont);
            }
        }
    }
}