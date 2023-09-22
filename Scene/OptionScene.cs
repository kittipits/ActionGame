using DxLibDLL;
using MyLib;

namespace ActionGame
{
    public class OptionScene : Scene
    {
        enum State
        {
            BGM,
            SE,
            Control,
        }

        State state;
        int bigFont;
        int smallFont;
        int miniFont;
        float bgmVolume;
        float seVolume;

        bool soundCheck;

        public OptionScene()
        {
            state = State.BGM;
            miniFont = DX.CreateFontToHandle(null, 15, -1);
            smallFont = DX.CreateFontToHandle(null, 20, -1);
            bigFont = DX.CreateFontToHandle(null, 32, -1);
            Sound.PlayMusic(Sound.bgmStage);
            bgmVolume = Sound.GetBGMVolume() * 100;
            seVolume = Sound.GetSEVolume() * 100;

            Sound.PlayMusic(Sound.bgmStage);
            soundCheck = false;
        }

        public override void Update()
        {

            if (Input.GetButtonDown(DX.PAD_INPUT_UP))
            {
                state--;
                Sound.Play(Sound.seSelect);
            }

            if (Input.GetButtonDown(DX.PAD_INPUT_DOWN))
            {
                state++;
                Sound.Play(Sound.seSelect);
            }

            if ((int)state >= 2) state = State.Control;
            if ((int)state <= 0) state = State.BGM;

            if (state == State.BGM)
            {
                if (Input.GetButton(DX.PAD_INPUT_RIGHT))
                {
                    bgmVolume++;
                    Sound.SetBGMVolume(bgmVolume);
                    Sound.ChangeBGMVolume();
                }
                else if (Input.GetButton(DX.PAD_INPUT_LEFT))
                {
                    bgmVolume--;
                    Sound.SetBGMVolume(bgmVolume);
                    Sound.ChangeBGMVolume();
                }

                if (bgmVolume >= 100) bgmVolume = 100;
                if (bgmVolume <= 0) bgmVolume = 0;
            }
            else if (state == State.SE)
            {
                if (Input.GetButton(DX.PAD_INPUT_RIGHT))
                {
                    seVolume++;
                    Sound.SetSEVolume(seVolume);
                    Sound.ChangeSEVolume();
                }
                else if (Input.GetButton(DX.PAD_INPUT_LEFT))
                {
                    seVolume--;
                    Sound.SetSEVolume(seVolume);
                    Sound.ChangeSEVolume();
                }
                if (Input.GetButtonUp(DX.PAD_INPUT_RIGHT)) Sound.Play(Sound.seSelect);
                if (Input.GetButtonUp(DX.PAD_INPUT_LEFT)) Sound.Play(Sound.seSelect);

                if (seVolume >= 100) seVolume = 100;
                if (seVolume <= 0) seVolume = 0;
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
            DX.DrawGraph(0, 0, Image.title);

            DrawStringLeftScreen(295, "背景音楽", DX.GetColor(83, 0, 113), smallFont);
            DrawStringLeftScreen(340, "効果音", DX.GetColor(83, 0, 113), smallFont);
            DrawStringLeftScreen(385, "操作確認", DX.GetColor(83, 0, 113), smallFont);
            DrawStringLeftScreen(450, "↑↓：選択　Ⓑ／X：タイトルへ", DX.GetColor(127, 127, 127), miniFont);

            if (state == State.BGM)
            {
                DX.DrawBox(80, 290, 240, 320, DX.GetColor(0, 0, 0), DX.TRUE);
                DrawStringLeftScreen(287, "背景音楽", DX.GetColor(186, 0, 123), bigFont);
                DrawStringRightScreen(370, "←→：調整", DX.GetColor(127, 127, 127), miniFont);
                DX.DrawStringToHandle(320, 300, "BGM", DX.GetColor(255, 255, 255), smallFont);
                DX.DrawStringToHandle(565, 300, ((int)bgmVolume).ToString(), DX.GetColor(255, 255, 255), smallFont);
                DX.DrawBoxAA(360, 298, 360 + bgmVolume * 2, 322, DX.GetColor(255, 160, 200), DX.TRUE);
            }
            else if (state == State.SE)
            {
                DX.DrawBox(80, 335, 240, 365, DX.GetColor(0, 0, 0), DX.TRUE);
                DrawStringLeftScreen(332, "効果音", DX.GetColor(186, 0, 123), bigFont);
                DrawStringRightScreen(370, "←→：調整", DX.GetColor(127, 127, 127), miniFont);
                DX.DrawStringToHandle(320, 300, "SE", DX.GetColor(255, 255, 255), smallFont);
                DX.DrawStringToHandle(565, 300, ((int)seVolume).ToString(), DX.GetColor(255, 255, 255), smallFont);
                DX.DrawBoxAA(360, 298, 360 + seVolume * 2, 322, DX.GetColor(255, 160, 200), DX.TRUE);
            }
            else if (state == State.Control)
            {
                DX.DrawBox(80, 380, 240, 410, DX.GetColor(0, 0, 0), DX.TRUE);
                DrawStringLeftScreen(377, "操作確認", DX.GetColor(186, 0, 123), bigFont);
                DrawStringRightScreen(280, "方向ボタン／方向キー：移動", DX.GetColor(255, 255, 255), miniFont);
                DrawStringRightScreen(310, "Ⓐ／ Z：（長押し）遠距離攻撃", DX.GetColor(255, 255, 255), miniFont);
                DrawStringRightScreen(340, "Ⓑ／ X：ジャンプ", DX.GetColor(255, 255, 255), miniFont);
                DrawStringRightScreen(370, "Ⓧ／ C：近距離攻撃", DX.GetColor(255, 255, 255), miniFont);
                DrawStringRightScreen(400, "Ⓨ／ A：ターゲットロック・解除", DX.GetColor(255, 255, 255), miniFont);
                DrawStringRightScreen(430, "STARTボタン／ Wキー：ポース", DX.GetColor(255, 255, 255), miniFont);
            }
        }
    }
}
