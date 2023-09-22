using DxLibDLL;
using MyLib;

namespace ActionGame
{
    public class TitleScene : Scene
    {
        enum State
        {
            Start,
            Select,
            Option,
            Story,
        }

        State state;
        int bigFont;
        int smallFont;
        int miniFont;

        int animationCounter = 0;
        int[] motionA = new int[5] { 1, 2, 1, 0, 1 };
        int[] motionB = new int[5] { 5, 6, 5, 4, 5 };
        int motionIndex = 0;
        bool soundCheck;


        public TitleScene()
        {
            state = State.Start;
            miniFont = DX.CreateFontToHandle(null, 15, -1);
            smallFont = DX.CreateFontToHandle(null, 20, -1);
            bigFont = DX.CreateFontToHandle(null, 32, -1);

            Sound.PlayMusic(Sound.bgmStage);
            soundCheck = false;
        }

        public override void Update()
        {
            animationCounter++;

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

            if ((int)state >= 3) state = State.Story;
            if ((int)state <= 0) state = State.Start;

            if (state == State.Start)
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_1))
                {
                    DX.StopMusic();
                    Sound.Play(Sound.seStart);
                    Game.ChangeScene(new StoryScene(0, "play"));
                }
            }
            else if (state == State.Select) 
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_1))
                {
                    Sound.Play(Sound.seSelect);
                    Game.ChangeScene(new StageSelectScene());
                }
            }
            else if (state == State.Option) 
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_1))
                {
                    Sound.Play(Sound.seSelect);
                    Game.ChangeScene(new OptionScene());
                }
            }
            else if (state == State.Story) 
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_1))
                {
                    Sound.Play(Sound.seSelect);
                    Game.ChangeScene(new StorySelectScene());
                }
            }

            if (!soundCheck)
            {
                Sound.ChangeBGMVolume();
                soundCheck = true;
            } 
        }

        public override void Draw()
        {
            motionIndex = motionIndex % 4;
            if (animationCounter % 10 == 0) motionIndex++;

            DX.DrawGraph(0, 0, Image.title);

            DX.DrawGraph(340, 350, Image.azalea[motionB[motionIndex]]);
            DX.DrawGraph(440, 350, Image.ellanoir[motionA[motionIndex]]);
            DX.DrawGraph(540, 350, Image.annette[motionA[motionIndex]]);

            DrawStringLeftScreen(250, "スタート", DX.GetColor(83, 0, 113), smallFont);
            DrawStringLeftScreen(295, "ステージ選択", DX.GetColor(83, 0, 113), smallFont);
            DrawStringLeftScreen(340, "オプション", DX.GetColor(83, 0, 113), smallFont);
            DrawStringLeftScreen(385, "ストーリー", DX.GetColor(83, 0, 113), smallFont);
            DrawStringLeftScreen(450, "↑↓：選択　Ⓐ／Z：決定", DX.GetColor(127, 127, 127), miniFont);

            if (state == State.Start)
            {
                DX.DrawBox(80, 245, 240, 275, DX.GetColor(0, 0, 0), DX.TRUE);
                DrawStringLeftScreen(242, "スタート", DX.GetColor(186, 0, 123), bigFont);
            }
            else if (state == State.Select)
            {
                DX.DrawBox(80, 290, 240, 320, DX.GetColor(0, 0, 0), DX.TRUE);
                DrawStringLeftScreen(287, "ステージ選択", DX.GetColor(186, 0, 123), bigFont);
            }
            else if (state == State.Option)
            {
                DX.DrawBox(80, 335, 240, 365, DX.GetColor(0, 0, 0), DX.TRUE);
                DrawStringLeftScreen(332, "オプション", DX.GetColor(186, 0, 123), bigFont);
            }
            else if (state == State.Story)
            {
                DX.DrawBox(80, 380, 240, 410, DX.GetColor(0, 0, 0), DX.TRUE);
                DrawStringLeftScreen(377, "ストーリー", DX.GetColor(186, 0, 123), bigFont);
            }
        }

    }
}
