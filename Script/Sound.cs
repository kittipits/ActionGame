using DxLibDLL;


namespace ActionGame
{
    public static class Sound
    {
        public static string bgmStage = "Sound/bgmStage.wav";
        public static string bgmBoss = "Sound/bgmBoss.wav";
        public static string bgmClear = "Sound/bgmClear.wav";
        public static string bgmDied = "Sound/bgmDied.wav";

        public static int seSelect;
        public static int seStart;
        public static int seShoot;
        public static int seSword1;
        public static int seSword2;
        public static int seCoin;
        public static int seSparkling;
        public static int seBroke1;
        public static int seBroke2;

        private static float bgmVolume = 1.0f;
        private static float seVolume = 1.0f;

        public static void Load()
        {
            seSelect = DX.LoadSoundMem("Sound/seSelect.wav");
            seStart = DX.LoadSoundMem("Sound/seStart.wav");
            seShoot = DX.LoadSoundMem("Sound/seShoot.wav");
            seSword1 = DX.LoadSoundMem("Sound/seSword1.wav");
            seSword2 = DX.LoadSoundMem("Sound/seSword2.wav");
            seCoin = DX.LoadSoundMem("Sound/seCoin.wav");
            seSparkling = DX.LoadSoundMem("Sound/seSparkling.wav");
            seBroke1 = DX.LoadSoundMem("Sound/seBroke1.wav");
            seBroke2 = DX.LoadSoundMem("Sound/seBroke2.wav");
        }

        //ー回
        public static void Play(int handle)
        {
            DX.PlaySoundMem(handle, DX.DX_PLAYTYPE_BACK);
        }

        //BGMを流す
        public static void PlayMusic(string fileName)
        {
            DX.PlayMusic(fileName, DX.DX_PLAYTYPE_LOOP);
        }
        //DX.StopMusic();   //BGMを停止させる。

        //ループ開始
        public static void PlayLoop(int handle)
        {
            DX.PlaySoundMem(handle, DX.DX_PLAYTYPE_LOOP);
        }

        //ループ停止
        public static void StopLoop(int handle)
        {
            DX.StopSoundMem(handle);
        }

        // BGMの音量設定処理(1-100)
        public static void SetBGMVolume(float volume)
        {
            
            bgmVolume = volume / 100;
        }

        // SEの音量設定処理(1-100)
        public static void SetSEVolume(float volume)
        {
            seVolume = volume / 100;
        }

        public static float GetBGMVolume()
        {
            return bgmVolume;
        }

        public static float GetSEVolume()
        {
            return seVolume;
        }

        public static void ChangeSEVolume()
        {
            float seVolume = GetSEVolume();
            DX.ChangeVolumeSoundMem((int)(255 * seVolume), seSelect);
            DX.ChangeVolumeSoundMem((int)(255 * seVolume), seStart);
            DX.ChangeVolumeSoundMem((int)(255 * seVolume), seShoot);
            DX.ChangeVolumeSoundMem((int)(255 * seVolume), seSword1);
            DX.ChangeVolumeSoundMem((int)(255 * seVolume), seSword2);
            DX.ChangeVolumeSoundMem((int)(255 * seVolume), seCoin);
            DX.ChangeVolumeSoundMem((int)(255 * seVolume), seSparkling);
            DX.ChangeVolumeSoundMem((int)(255 * seVolume), seBroke1);
            DX.ChangeVolumeSoundMem((int)(255 * seVolume), seBroke2);
        }

        public static void ChangeBGMVolume()
        {
            float bgmVolume = GetBGMVolume();
            DX.SetVolumeMusic((int)(255 * bgmVolume));
        }
    }
}
