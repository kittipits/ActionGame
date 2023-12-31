﻿using MyLib;

namespace ActionGame
{
    public class Game
    {
        static Scene scene;

        public void Init()
        {
            Input.Init();
            MyRandom.Init();
            Image.Load();
            Sound.Load();
            scene = new TitleScene();
        }

        public void Update()
        {
            Input.Update();
            scene.Update();
        }

        public void Draw()
        {
            scene.Draw();
        }

        public static void ChangeScene(Scene newScene)
        { 
            scene = newScene;
        }
    }
}
