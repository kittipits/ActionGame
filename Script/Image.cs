using DxLibDLL;

namespace ActionGame
{
    public static class Image
    {
        public static int shiitake;
        public static int turtle;
        public static int flower;
        public static int squid;
        public static int ghost;
        public static int bomber;
        public static int playerShot;
        public static int swordBeam;
        public static int[] azalea = new int[22];
        public static int[] mapchip = new int[128]; 
        public static int itemLifeUp;
        public static int itemShieldUp;
        public static int itemSpeedUp;
        public static int itemCoin;
        public static int[] sparkling = new int[7];
        public static int FloorObject;
        public static int itemblock;
        public static int blankblock;
        public static int breakable;
        public static int spikeblock;
        public static int spikedrop;
        public static int goalOne;
        public static int goalThree;
        public static int chargedShot;
        public static int[] annette = new int[5];
        public static int enemyShot;
        public static int title;
        public static int[] ellanoir = new int[4];
        public static int gameclear;
        public static int stageclear;
        public static int gameover;
        public static int heart;
        public static int crosshair;

        public static void Load()
        {
            shiitake = DX.LoadGraph("Image/shiitake.png");
            turtle = DX.LoadGraph("Image/turtle.png");
            flower = DX.LoadGraph("Image/flower.png");
            squid = DX.LoadGraph("Image/squid.png");
            ghost = DX.LoadGraph("Image/ghost.png");
            bomber = DX.LoadGraph("Image/bomber.png");
            playerShot = DX.LoadGraph("Image/player_shot.png");
            swordBeam = DX.LoadGraph("Image/swordbeam.png");
            DX.LoadDivGraph("Image/azalea.png", azalea.Length, 4, 6, 60, 70, azalea);
            DX.LoadDivGraph("Image/mapchip.png", mapchip.Length, 16, 8, 32, 32, mapchip);
            itemLifeUp = DX.LoadGraph("Image/itemLifeUp.png");
            itemShieldUp = DX.LoadGraph("Image/itemShieldUp.png");
            itemSpeedUp = DX.LoadGraph("Image/itemSpeedUp.png");
            itemCoin = DX.LoadGraph("Image/coin.png");
            DX.LoadDivGraph("Image/sparkling.png", sparkling.Length, 7, 1, 32, 32, sparkling);
            FloorObject = DX.LoadGraph("Image/FloorObject.png");
            itemblock = DX.LoadGraph("Image/itemblock.png");
            blankblock = DX.LoadGraph("Image/blankblock.png");
            breakable = DX.LoadGraph("Image/breakable.png");
            spikeblock = DX.LoadGraph("Image/spikeblock.png");
            spikedrop = DX.LoadGraph("Image/spikedrop.png");
            goalOne = DX.LoadGraph("Image/ellanoir_goal.png");
            goalThree = DX.LoadGraph("Image/annette_goal.png");
            chargedShot = DX.LoadGraph("Image/chargedshot.png");
            DX.LoadDivGraph("Image/annette.png", annette.Length, 5, 1, 60, 70, annette);
            enemyShot = DX.LoadGraph("Image/enemy_bullet_16.png");
            title = DX.LoadGraph("Image/title.png");
            DX.LoadDivGraph("Image/ellanoir.png", ellanoir.Length, 4, 1, 60, 70, ellanoir);
            gameclear = DX.LoadGraph("Image/gameclear.png");
            stageclear = DX.LoadGraph("Image/stageclear.png");
            gameover = DX.LoadGraph("Image/gameover.png");
            heart = DX.LoadGraph("Image/heart.png");
            crosshair = DX.LoadGraph("Image/crosshair.png");
        }
    }
}
