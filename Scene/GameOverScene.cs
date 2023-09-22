using DxLibDLL;
using MyLib;

namespace ActionGame
{
    public class GameOverScene : Scene
    {
        int level;
        int smallFont;
        int counter = 0;
        string mode;
        bool soundCheck;

        public GameOverScene(int level, string mode)
        {
            this.level = level;
            this.mode = mode;

            smallFont = DX.CreateFontToHandle(null, 20, -1);
            this.mode = mode;

            Sound.PlayMusic(Sound.bgmDied);
            soundCheck = false;
        }

        public override void Update()
        {
            counter++;

            if (Input.GetButtonDown(DX.PAD_INPUT_1))
            {
                Sound.Play(Sound.seStart);
                Game.ChangeScene(new PlayScene(level, mode));
            }
            else if (Input.GetButtonDown(DX.PAD_INPUT_2))
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
            DX.DrawGraph(0, 0, Image.gameover);

            if (counter % 60 > 20)
            {
                DrawStringCenterToHandle(450, "Ⓐ（Zキー）：リトライ　　Ⓑ（Xキー）：タイトルへ", DX.GetColor(127, 127, 127), smallFont);
            }
        }


    }
}