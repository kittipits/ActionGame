using DxLibDLL;
using MyLib;
using System.Diagnostics.Eventing.Reader;
using System.Security.Permissions;
using System.Threading;

namespace ActionGame
{
    public class StoryScene : Scene
    {
        int level;
        int smallFont;
        int miniFont;
        int bigFont;
        int largeFont;
        string mode;
        int progress = 0;
        int endProgress = 0;
        string bgm = Sound.bgmStage;
        bool soundCheck;

        string[] dialougue0 = {
        "アザレアはミッドガードへの旅を始め、呪いの森に入ってしまった。",
        "その後、お城の主であるエラノワールと対決することになりました。",
        "結局、エラノワールは敗北しました。",
        "「お願い、殺さないで。IT会社でも何でもあげるから、お願いします」",
        "とエラノワールは懇願しました。",
        "しかし、アザレアはそのお城が実はIT会社であることを知って、驚きました。",
        "「いや、殺すつもりはないけど、それでいいのか？」",
        "とアザレアは言いました。",
        "エラノワールはアザレアの言葉に頷きました。",
        "「じゃあ、ゲームを作りましょう！」",
        "それから、アザレアはエラノワールと一緒に住むことになりました。",
        "しかし、ある日、エラノワールの姿が消えてしまいました。",
        "それで、アザレアは怪しい洞窟に向かうことになりました。",
        " ",
        " "
        };

        string[] dialougueSpeaker1 = {
        "アザレア",
        "エラノワール",
        "アザレア",
        "エラノワール",
        "アザレア",
        "エラノワール",
        "アザレア",
        "エラノワール",
        "アザレア",
        "エラノワール",
        "アザレア",
        "エラノワール",
        "アザレア",
        "エラノワール",
        "アザレア",
        "エラノワール",
        " ",
        " "
        };

        string[] dialougue1 = {
        "やっと見つけたよ〜！まったく、心配させないでくれよ。",
        "ごめんなさい！ちょっと洞窟が崩れちゃって…いつもの取\n材なのに。",
        "取材って、なんの取材なの？普通、あの森で動物のこと\nじゃないの？",
        "違うんだよ、食べ物じゃなくて、さぁ…",
        "エラーネットワーク、実はデバッグ作業をサボってるのか\nしら？言い訳ダメだよ。",
        "でも、ちょっとだけ温泉に入りたかったの。この洞窟から\n温泉の匂いがしてるんだよ。ちなみに、その呼び方をやめ\nてくれないかな。",
        "まあまあ、ネットワーク会社のエラノワール社長のことだ\nから、エラーネットワークでいいんじゃない？",
        "バカにしないでよぉ。もう！",
        "わかってるよ、エララー。でもさ、お城に温泉あるんだか\nら、なんでわざわざここまで来たの？",
        "もう、温泉の魅力知らないくせに？ああちゃんのバカ！",
        "はいはい。でも、奥から怪しい魔力感じるんだよね。それ\nに温泉の匂いだけじゃないみたい。",
        "じゃあ、どうする？",
        "仕方ないね。私も気になるし、進もうか。",
        "ああちゃんも温泉行きたいのかな？",
        "そうじゃないって。魔力の原因を調べるだけだ。",
        "イェーイ、イェーイ！レッツゴー！",
        " ",
        " "
        };

        string[] dialougueSpeaker2 = {
        "アザレア",
        "エラノワール",
        "アザレア",
        "？？？",
        "エラノワール",
        "アザレア",
        "エラノワール",
        "アザレア",
        "エラノワール",
        "？？？",
        "アザレア",
        "エラノワール",
        "アザレア",
        "エラノワール",
        "アザレア",
        "エラノワール",
        "キャサリン",
        "アザレア",
        "エラノワール",
        "アザレア",
        " ",
        " "
        };

        string[] dialougue2 = {
        "大変だよ〜！モンスターも罠も、超大変だよ！",
        "もう、ほんとに疲れちゃったわ。少し休憩しようかしら。",
        "あら？エララー、見て見て。あっちに子猫がいるよ。",
        "グゥゥゥ…グゥゥゥ…",
        "ほんとだね。でも、それは猫ちゃんじゃなくて、パンサー\nみたいだよ。",
        "まあまあ。かわいいから、そんなことどうでもいいわ。",
        "はいはい。でも、よく見ると、毛の色がちょっと変じゃな\nい？",
        "確かに。耳と足の毛がなんかピンクに見えるみたい。魔力\nの影響かも？",
        "さあ…。何か怖がってるみたいだね…",
        "グゥゥゥ…グゥゥゥ…",
        "ふむ…。決めた！この子、飼ってみるね。",
        "ええ⁉それって、飼っていいのかしら？",
        "それは気にしないで。私が面倒みるから。",
        "まあ、それならしょうがないわね。じゃあ、名前はどうし\nようかしら。",
        "猫だからキャットってことで、キャサリンにしようかな。",
        "猫じゃないってば。ちゃんと聞いてる？",
        "グゥルルル…グゥルルル…",
        "ほら、この子、うれしそうに見えるし、大丈夫さ！",
        "好きにして。とりあえず、休憩しようかしら。",
        "おう！",
        " ",
        " "
        };

        string[] dialougueSpeaker3 = {
        "アザレア",
        "エラノワール",
        "アザレア",
        "エラノワール",
        "キャサリン",
        "アザレア",
        "エラノワール",
        "？？？",
        "アザレア",
        "エラノワール",
        "？？？",
        "アザレア",
        "エラノワール",
        "アザレア",
        "エラノワール",
        "アンネット",
        "アザレア",
        "アンネット",
        "エラノワール",
        "アザレア",
        "アンネット",
        " ",
        " "
        };

        string[] dialougue3 = {
        "魔力の気配、すごく強くなってきちゃったね。",
        "うぅ、怖いよ。帰りたいなぁ。",
        "簡単に諦めちゃだめだよ。まだ温泉に入りたいんでしょ！\nエララー。",
        "そりゃそうだけど、たとえ温泉があったとしても、こんな\n怖いところで温泉なんて入りたくないよ！",
        "グゥゥゥ…グゥゥゥ…",
        "あら、どうしたのかしら？キャサリン。",
        "ああちゃん、見て。誰かの気配がするわ！",
        "お前たちは何者じゃ⁉",
        "あ、エルフさん！この世界に来てから、初めて見たわ。",
        "ああちゃん、その人怖そうよ。おそらく魔力の原因かもし\nれないわよね？",
        "そうだ。わしは冥術を使うアンネット・フォン・ドンケル\nハイト。冥術の女王と呼ばれる魔導士じゃ。闇に潜み、影\nに生きる。冥界から蘇り、力を取り戻している最中じゃ。\nそれでも、お前たちより強いと確信しているのじゃ。",
        "わあ、中二病っぽいセリフだわ！なんかワクワクしちゃう\nわ。",
        "その名前、おそらく千年前のアルフヘイムを混乱させた冥\n術の女王のことね。その時代、アルフヘイムの教会によっ\nて冥術の女王は討たれ、それから冥術は禁じられているは\nず。",
        "へえ、有名人なのね。",
        "ヴァンパイア族も冥術の女王によって生まれたと言われて\nいるわ。",
        "正解じゃ。あの教会への復讐のために、わしは生まれ変\nわったんじゃ。",
        "でも、まだ力を取り戻しきっていないってことは、本気で\n戦えないのかな。それはちょっとイヤだよね。だって私、\nチャレンジが好きなゲーマーだからね。",
        "よく舐めておるな。いいじゃろう、暗闇に沈め、冥術の力\nを思い知らせてやる！",
        "ああちゃん、逃げなさい！なんでそんなことを言うのよ！",
        "大丈夫だ、エララー。私に任せてくれ。これは腕試しの\nチャンスだよ。",
        "かかってこい！",
        " ",
        " "
        };

        string[] dialougueSpeaker4 = {
        "アザレア",
        "エラノワール",
        "アザレア",
        "エラノワール",
        "アザレア",
        "エラノワール",
        "アザレア",
        "エラノワール",
        "アザレア",
        "エラノワール",
        "アザレア",
        "エラノワール",
        "アザレア",
        "エラノワール",
        "アザレア",
        "エラノワール",
        " ",
        " "
        };

        string[] dialougue4 = {
        "やったー！",
        "すごい、ああちゃん、強すぎるよぉ。",
        "もちろん！私、天才だからさ。",
        "で、次はどうするの？",
        "ふふ、勇者が魔王を倒すように、封印するのよ。",
        "ど、どうやって封印するの？こいつ、冥術で蘇ったって\n言ってたし、普通の封印じゃ通用しないよね。",
        "私にはいいアイデアがあるんだ。",
        "はあ…",
        "私が来たところにはサイバー魔法があるの。まあ、一般的\nな魔法は、妖精や悪魔など別の次元や空間に繋がるんだけ\nど、サイバー魔法は魔力でサイバー空間に繋げるのよ。",
        "なんか、わかるようなわからないような…",
        "だから、これを使って、私たちが作ったメタバース、つま\nり、ゲームに閉じ込めるの。あれに強制的にログインさせ\nるけど、ログアウトはできない。つまり、許可がない限り、\nあの世界で永遠に閉じ込めるってこと。",
        "まあ、たぶん大丈夫かな？こいつ、千年前の人だし、メタ\nバースを知らないみたいだし。脱出は難しいでしょう。",
        "そうそう。",
        "結局、温泉はなかったね。ただ、魔力で錯覚させてしまっ\nたみたい。",
        "そうだね。今日はもう疲れたし、お城で温泉に入ろうか。",
        "うん、一緒に入りましょうね。",
        " ",
        " "
        };

        public StoryScene(int level, string mode)
        { 
            this.level = level;
            this.mode = mode;

            miniFont = DX.CreateFontToHandle(null, 12, -1);
            smallFont = DX.CreateFontToHandle(null, 16, -1);
            bigFont = DX.CreateFontToHandle(null, 22, -1);
            largeFont = DX.CreateFontToHandle(null, 25, -1);
            soundCheck = false;
        }
        public override void Update()
        {
            if (level == 0) endProgress = dialougue0.Length - 1;
            else if (level == 1) endProgress = dialougue1.Length - 1;
            else if (level == 2) endProgress = dialougue2.Length - 1;
            else if (level == 3) endProgress = dialougue3.Length - 1;
            else if (level == 4) endProgress = dialougue4.Length - 1;

            if (Input.GetButtonDown(DX.PAD_INPUT_1))
            {
                progress++;
            }
            else if (progress == endProgress)
            {
                DX.StopMusic();
                Mode();
            }
            else if (Input.GetButtonDown(DX.PAD_INPUT_8))
            {
                DX.StopMusic();
                Mode();
            }

            if (level == 3)
            {
                bgm = Sound.bgmDied;
            }
            else if (level == 4)
            {
                bgm = Sound.bgmClear;
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
            DX.DrawStringToHandle(320, 15, "Ⓐ（Zキー）：次へ　　START（Wキー）：スキップ", DX.GetColor(127, 127, 127),  miniFont);

            if (level == 0)
            {
                for (int i = 0; i < progress; i++)
                {
                    int posY = 40 + (i * 28);
                    if (i >= 3) posY += 28;
                    if (i >= 11) posY += 28;

                    DX.DrawStringToHandle(20, posY, dialougue0[i], DX.GetColor(255, 255, 255), smallFont);
                }
            }
            else if (level == 1)
            {
                for (int i = 0; i < progress; i++)
                {
                    uint speakerColor = DX.GetColor(255, 255, 255);
                    uint dialogueColor = DX.GetColor(255, 255, 255); 
                    if (dialougueSpeaker1[i] == "アザレア")
                    {
                        speakerColor = DX.GetColor(150, 0, 200);
                        dialogueColor = DX.GetColor(255, 160, 200);
                    }
                    else if (dialougueSpeaker1[i] == "エラノワール")
                    {
                        speakerColor = DX.GetColor(218, 52, 52);
                        dialogueColor = DX.GetColor(245, 222, 169);
                    }
                    DX.DrawBox(0, 270, 200, 320, DX.GetColor(0, 0, 0), DX.TRUE);
                    DX.DrawBox(0, 320, 640, 480, DX.GetColor(0, 0, 0), DX.TRUE);
                    DX.DrawStringToHandle(20, 285, dialougueSpeaker1[i], speakerColor, largeFont);
                    DX.DrawStringToHandle(20, 340, dialougue1[i], dialogueColor, bigFont);
                }
            }
            else if (level == 2)
            {
                for (int i = 0; i < progress; i++)
                {
                    uint speakerColor = DX.GetColor(255, 255, 255);
                    uint dialogueColor = DX.GetColor(255, 255, 255);
                    if (dialougueSpeaker2[i] == "アザレア")
                    {
                        speakerColor = DX.GetColor(150, 0, 200);
                        dialogueColor = DX.GetColor(255, 160, 200);
                    }
                    else if (dialougueSpeaker2[i] == "エラノワール")
                    {
                        speakerColor = DX.GetColor(218, 52, 52);
                        dialogueColor = DX.GetColor(245, 222, 169);
                    }
                    else if (dialougueSpeaker2[i] == "？？？" || dialougueSpeaker2[i] == "キャサリン")
                    {
                        speakerColor = DX.GetColor(191, 191, 191);
                        dialogueColor = DX.GetColor(191, 191, 191);
                    }
                    DX.DrawBox(0, 270, 200, 320, DX.GetColor(0, 0, 0), DX.TRUE);
                    DX.DrawBox(0, 320, 640, 480, DX.GetColor(0, 0, 0), DX.TRUE);
                    DX.DrawStringToHandle(20, 285, dialougueSpeaker2[i], speakerColor, largeFont);
                    DX.DrawStringToHandle(20, 340, dialougue2[i], dialogueColor, bigFont);
                }
            }
            else if (level == 3)
            {
                for (int i = 0; i < progress; i++)
                {
                    uint speakerColor = DX.GetColor(255, 255, 255);
                    uint dialogueColor = DX.GetColor(255, 255, 255);
                    if (dialougueSpeaker3[i] == "アザレア")
                    {
                        speakerColor = DX.GetColor(150, 0, 200);
                        dialogueColor = DX.GetColor(255, 160, 200);
                    }
                    else if (dialougueSpeaker3[i] == "エラノワール")
                    {
                        speakerColor = DX.GetColor(218, 52, 52);
                        dialogueColor = DX.GetColor(245, 222, 169);
                    }
                    else if (dialougueSpeaker3[i] == "キャサリン")
                    {
                        speakerColor = DX.GetColor(191, 191, 191);
                        dialogueColor = DX.GetColor(191, 191, 191);
                    }
                    else if (dialougueSpeaker3[i] == "？？？" || dialougueSpeaker3[i] == "アンネット")
                    {
                        speakerColor = DX.GetColor(49, 21, 252);
                        dialogueColor = DX.GetColor(49, 252, 252);
                    }
                    DX.DrawBox(0, 270, 200, 320, DX.GetColor(0, 0, 0), DX.TRUE);
                    DX.DrawBox(0, 320, 640, 480, DX.GetColor(0, 0, 0), DX.TRUE);
                    DX.DrawStringToHandle(20, 285, dialougueSpeaker3[i], speakerColor, largeFont);
                    DX.DrawStringToHandle(20, 340, dialougue3[i], dialogueColor, bigFont);
                }

            }
            else if (level == 4)
            {
                for (int i = 0; i < progress; i++)
                {
                    uint speakerColor = DX.GetColor(255, 255, 255);
                    uint dialogueColor = DX.GetColor(255, 255, 255);
                    if (dialougueSpeaker4[i] == "アザレア")
                    {
                        speakerColor = DX.GetColor(150, 0, 200);
                        dialogueColor = DX.GetColor(255, 160, 200);
                    }
                    else if (dialougueSpeaker4[i] == "エラノワール")
                    {
                        speakerColor = DX.GetColor(218, 52, 52);
                        dialogueColor = DX.GetColor(245, 222, 169);
                    }
                    DX.DrawBox(0, 270, 200, 320, DX.GetColor(0, 0, 0), DX.TRUE);
                    DX.DrawBox(0, 320, 640, 480, DX.GetColor(0, 0, 0), DX.TRUE);
                    DX.DrawStringToHandle(20, 285, dialougueSpeaker4[i], speakerColor, largeFont);
                    DX.DrawStringToHandle(20, 340, dialougue4[i], dialogueColor, bigFont);
                }
            }

            if (level != 0)
            {
                DX.DrawBox(0, 270, 200, 320, DX.GetColor(255, 255, 255), DX.FALSE);
                DX.DrawBox(0, 320, 640, 480, DX.GetColor(255, 255, 255), DX.FALSE);
            }
        }

        void Mode()
        {
            if (mode == "read" )
            {
                Game.ChangeScene(new StorySelectScene()); 
            }
            else if (level != 4 && mode == "play")
            {
                Game.ChangeScene(new PlayScene(level + 1, "story"));
            }
            else if (level == 4 && mode == "play")
            {
                Game.ChangeScene(new TitleScene());
            }
        }
    }
}