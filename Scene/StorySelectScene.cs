using DxLibDLL;
using MyLib;

namespace ActionGame
{
    public class StorySelectScene : Scene
    {
        enum State
        {
            Prologue,
            One,
            Two,
            Three,
            Epilogue,
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

        public StorySelectScene()
        {
            state = State.Prologue;
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

            if ((int)state >= 4) state = State.Epilogue;
            if ((int)state <= 0) state = State.Prologue;

            if (state == State.Prologue)
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_1))
                {
                    DX.StopMusic();
                    Sound.Play(Sound.seStart);
                    Game.ChangeScene(new StoryScene(0, "read"));
                }
            }
            else if (state == State.One)
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_1))
                {
                    DX.StopMusic();
                    Sound.Play(Sound.seStart);
                    Game.ChangeScene(new StoryScene(1, "read"));
                }
            }
            else if (state == State.Two) 
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_1))
                {
                    DX.StopMusic();
                    Sound.Play(Sound.seStart);
                    Game.ChangeScene(new StoryScene(2, "read"));
                }
            }
            else if (state == State.Three) 
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_1))
                {
                    DX.StopMusic();
                    Sound.Play(Sound.seStart);
                    Game.ChangeScene(new StoryScene(3, "read"));
                }
            }
            else if (state == State.Epilogue) 
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_1))
                {
                    DX.StopMusic();
                    Sound.Play(Sound.seStart);
                    Game.ChangeScene(new StoryScene(4, "read"));
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
            motionIndex = motionIndex % 4;
            if (animationCounter % 10 == 0) motionIndex++;

            DX.DrawGraph(0, 0, Image.title);

            DX.DrawGraph(340, 350, Image.azalea[motionB[motionIndex]]);
            DX.DrawGraph(440, 350, Image.ellanoir[motionA[motionIndex]]);
            DX.DrawGraph(540, 350, Image.annette[motionA[motionIndex]]);

            DrawStringLeftScreen(205, "序章", DX.GetColor(83, 0, 113), smallFont);
            DrawStringLeftScreen(250, "ストーリー①", DX.GetColor(83, 0, 113), smallFont);
            DrawStringLeftScreen(295, "ストーリー②", DX.GetColor(83, 0, 113), smallFont);
            DrawStringLeftScreen(340, "ストーリー③", DX.GetColor(83, 0, 113), smallFont);
            DrawStringLeftScreen(385, "終章", DX.GetColor(83, 0, 113), smallFont);
            DrawStringLeftScreen(430, "↑↓：選択　Ⓐ／Z：決定", DX.GetColor(127, 127, 127), miniFont);
            DrawStringLeftScreen(450, "Ⓑ／X：タイトルへ", DX.GetColor(127, 127, 127), miniFont);

            if (state == State.Prologue)
            {
                DX.DrawBox(80, 200, 240, 230, DX.GetColor(0, 0, 0), DX.TRUE);
                DrawStringLeftScreen(197, "序章", DX.GetColor(186, 0, 123), bigFont);
            }
            if (state == State.One)
            {
                DX.DrawBox(80, 245, 240, 275, DX.GetColor(0, 0, 0), DX.TRUE);
                DrawStringLeftScreen(242, "ストーリー①", DX.GetColor(186, 0, 123), bigFont);
            }
            else if (state == State.Two)
            {
                DX.DrawBox(80, 290, 240, 320, DX.GetColor(0, 0, 0), DX.TRUE);
                DrawStringLeftScreen(287, "ストーリー②", DX.GetColor(186, 0, 123), bigFont);
            }
            else if (state == State.Three)
            {
                DX.DrawBox(80, 335, 240, 365, DX.GetColor(0, 0, 0), DX.TRUE);
                DrawStringLeftScreen(332, "ストーリー③", DX.GetColor(186, 0, 123), bigFont);
            }
            else if (state == State.Epilogue)
            {
                DX.DrawBox(80, 380, 240, 410, DX.GetColor(0, 0, 0), DX.TRUE);
                DrawStringLeftScreen(377, "終章", DX.GetColor(186, 0, 123), bigFont);
            }
        }
    }
}
